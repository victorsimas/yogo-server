using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YogoServer.StartupManager;

namespace YogoServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder().Run();
        }

        public static IWebHost CreateHostBuilder()
        {
            Microsoft.Extensions.Configuration.IConfigurationRoot config = Configurator.GetConfiguration();

            IWebHost host = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);
                        logging.AddConfiguration(config.GetSection("Logging"));
                    })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://0.0.0.0.0:5000")
                .Build();

            return host;
        }
    }
}
