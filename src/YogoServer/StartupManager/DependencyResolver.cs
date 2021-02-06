using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using YogoServer.ErrorHandling.Middleware;
using YogoServer.Services;

namespace YogoServer.StartupManager
{
    public static class DependencyResolver
    {
        public static IServiceCollection AddDefaultServices(this IServiceCollection services)
        {
            services
            .AddMvc(options => options.EnableEndpointRouting = false)
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

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });
            
            services.AddSingleton<Process>();
            services.AddSingleton<IYogoService, YogoService>();

            return services;
        }

        public static IApplicationBuilder UseApiErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiErrorHanlder>();
        }
    }
}