using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using YogoServer.Services;

namespace YogoServer.StartupManager
{
    public static class DependencyResolver
    {
        public static IServiceCollection AddDefaultServices(this IServiceCollection services)
        {
            services.AddMvc()
            .AddJsonOptions(options => 
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = JsonConfiguration.Get().PropertyNameCaseInsensitive;
                    options.JsonSerializerOptions.IgnoreNullValues = JsonConfiguration.Get().IgnoreNullValues;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonConfiguration.Get().PropertyNamingPolicy;
                    options.JsonSerializerOptions.Encoder = JsonConfiguration.Get().Encoder;
                    options.JsonSerializerOptions.WriteIndented = JsonConfiguration.Get().WriteIndented;
                }
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "YogoServer", Version = "v1" });
            });

            services.AddSingleton<Process>();
            services.AddSingleton<IYogoService, YogoService>();

            return services;
        }
    }
}