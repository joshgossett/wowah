using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AHWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                // .ConfigureLogging(logging =>
                //{
                //    logging.ClearProviders();
                //    logging.AddConsole();

                //})
                .UseStartup<Startup>()
                .ConfigureKestral((context, options) =>
                {
                    // Set properties and call methods on options
                });
    }
}
