using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace YogoServer.StartupManager
{
    public class Configurator
    {
        public static IConfigurationRoot GetConfiguration(IWebHostEnvironment environment)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                           .SetBasePath(environment.ContentRootPath)
                           .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: false)
                           .AddEnvironmentVariables();

            return builder.Build();
        }

        public static IConfigurationRoot GetConfiguration()
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder().AddJsonFile($"appsettings.{environmentName}.json", optional: true).Build();
        }
    }
}