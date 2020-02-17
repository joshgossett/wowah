using AHWeb.Db;
using AHWeb.Schema;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AHWeb.Service
{
    public class AHDownloader : IHostedService
    {
        private static readonly string ClientSecret = "186PmjIPXwD3427MG8dBZpgmtgPbpTDw";
        private static readonly string ClientID = "01b2db67e68f4e9ea169bfca1253e17a";

        private static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        private Task updateTask;


        private static Task GetAuctionSnap()
        {
            return Task.Run(() =>
            {
                using (HttpClient client = GetAuthorizedClient().Result)
                using (AuctionsDB db = new AuctionsDB())
                {
                    var response = client.GetAsync("https://us.api.blizzard.com/wow/auction/data/Korgath?locale=en_US").Result;
                    var files = AhFiles.FromJson(response.Content.ReadAsStringAsync().Result);

                    foreach (var f in files.Files)
                    {
                        Console.WriteLine($"Found file: {f.Url}");
                        if (db.AuctionSnaps.Where(x => x.TimeStamp == f.LastModified && x.Url == f.Url.ToString()).Count() > 0)
                        {
                            Console.WriteLine("File already exists in the database");
                            //already exists
                            continue;
                        }
                        Console.WriteLine("Downloading auction data...");
                        Stopwatch sw = new Stopwatch();
                        Stopwatch swtotal = new Stopwatch();
                        sw.Start();
                        swtotal.Start();
                        AuctionSnap snap = DeserializeAHSnap.FromJson(client.GetAsync(f.Url).Result.Content.ReadAsStringAsync().Result);
                        snap.Url = f.Url.ToString();
                        snap.TimeStamp = f.LastModified;

                        Action<string> timetaken = (string msg) =>
                        {
                            Console.WriteLine($"{sw.Elapsed} taken to {msg}");
                            sw.Restart();
                        };

                        timetaken("download and parse data");
                        Console.WriteLine("Adding Snap...");
                        db.Add(snap);
                        db.SaveChanges();
                        timetaken("add snap");

                        db.RealmDatas.AddRange(snap.RealmDatas.Except(db.RealmDatas));
                        db.SaveChanges();
                        timetaken("add add auctions");

                        if (snap.AuctionSnapRealmDatas == null)
                            snap.AuctionSnapRealmDatas = new List<AuctionSnapRealmData>();
                        snap.AuctionSnapRealmDatas.AddRange(snap.RealmDatas.Select(x => new AuctionSnapRealmData { AuctionSnap = snap, RealmData = db.RealmDatas.Where(rd => rd.Realm == x.Realm && rd.Slug == x.Slug).First() }));
                        db.SaveChanges();
                        timetaken("link autions and realms");

                        db.Modifiers.AddRange(snap.Auctions
                            .Where(x => x.Modifiers != null)
                            .SelectMany(x => x.Modifiers)
                            .Except(db.Modifiers));
                        db.SaveChanges();
                        timetaken("add new modifiers");

                        db.BonusLists.AddRange(snap.Auctions
                            .Where(x => x.BonusLists != null)
                            .SelectMany(x => x.BonusLists)
                            .Except(db.BonusLists));
                        db.SaveChanges();
                        timetaken("add new bonuslists");

                        db.AuctionModifiers.AddRange(snap.Auctions
                            .Where(x => x.Modifiers != null)
                            .SelectMany(x => db.Modifiers.Where(m => x.Modifiers.Any(om => om.Type == m.Type && om.Value == m.Value))
                            .Select(am => new AuctionModifier { Auction = x, Modifier = am })));
                        db.SaveChanges();
                        timetaken("link autions and modifiers");

                        db.AuctionBonusLists.AddRange(snap.Auctions
                            .Where(x => x.BonusLists != null)
                            .SelectMany(x => db.BonusLists.Where(b => x.BonusLists.Any(ob => ob.BonusListId == b.BonusListId))
                            .Select(ab => new AuctionBonusList { Auction = x, BonusList = ab })));

                        db.SaveChanges();
                        timetaken("link auctions and bonus lists");

                        Console.WriteLine($"Total time taken to save {snap.Auctions.Count}: {swtotal.Elapsed}");
                    }
                }
            });
        }

        private static async Task<HttpClient> GetAuthorizedClient()
        {
            HttpClient client = new HttpClient();
            var context = new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", "client_credentials" } });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{ClientID}:{ClientSecret}")));

            var response = await client.PostAsync("https://us.battle.net/oauth/token", context);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string s = await response.Content.ReadAsStringAsync();
                var token = CredentialsToken.FromJson(s);
                HttpClient authclient = new HttpClient();
                authclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                return authclient;
            }
            else
            {
                throw new Exception($"STATUS CODE: {response.StatusCode}");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            updateTask = Task.Run(async () =>
            {
                var token = cancelTokenSource.Token;
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        await GetAuctionSnap();
                        GC.Collect();
                        await Task.Delay(new TimeSpan(0, 29, 0), token);
                    }
                    catch (Exception) { }//maybe log this and keep it from spamming if it can't complete
                    finally
                    {
                        await Task.Delay(new TimeSpan(0, 1, 0), token);//don't try this except every 1 min if it can't complete
                    }
                }
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancelTokenSource.Cancel();
            return Task.Run(() => updateTask.Wait());
        }
    }
}
