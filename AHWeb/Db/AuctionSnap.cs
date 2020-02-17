using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AHWeb.Db
{
    class AuctionSnap
    {
        public long ID { get; set; }

        public string Url { get; set; }

        [JsonProperty("realms")]
        [NotMapped]
        public List<RealmData> RealmDatas { get; set; }

        public DateTime TimeStamp { get; set; }

        [JsonProperty("auctions")]
        public List<Auction> Auctions { get; set; }

        //foriegn links
        public List<AuctionSnapRealmData> AuctionSnapRealmDatas { get; set; }
    }

    class RealmData : IEquatable<RealmData>
    {
        public long ID { get; set; }

        [JsonProperty("name")]
        public string Realm { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        //foriegn links
        public List<AuctionSnapRealmData> AuctionSnapRealmDatas { get; set; }


        public bool Equals(RealmData o)
        {
            if (o == null)
                return false;
            return o.Realm == Realm && o.Slug == Slug;
        }

        public override bool Equals(object obj) => Equals(obj as RealmData);
        public override int GetHashCode() => (Realm, Slug).GetHashCode();

        public override string ToString() => $"Realm:{Realm}, Slug:{Slug}";
    }

    class AuctionSnapRealmData
    {
        public long AuctionSnapID { get; set; }
        public AuctionSnap AuctionSnap { get; set; }

        public long RealmDataID { get; set; }
        public RealmData RealmData { get; set; }
    }
}
