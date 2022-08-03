using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using YogoServer.StartupManager;

namespace YogoServer.ErrorHandling.Middleware
{
    public class ApiErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions = JsonConfiguration.Get();

        public ApiErrorHandler(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ApiErrorHandler>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context is null)
            {
                _logger.LogCritical($"{JsonSerializer.Serialize("Contexto nulo.", _jsonSerializerOptions)}");

                throw new ArgumentNullException(nameof(context), "Contexto nulo.");
            }

            try
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;

                await _next(context);
            }
            catch (ApiException ex)
            {
                string logDetails = string.Empty;

                if (ex.MessagesToLog != null && ex.MessagesToLog.Any())
                {
                    logDetails = string.Join(Environment.NewLine, ex.MessagesToLog);
                }

                string exceptionName = ex.GetType().Name;

                context.Response.StatusCode = ex.Status;

                await context.Response.WriteAsync(GetError(context.Response.StatusCode, ex.Message, ex.MessagesToLog));
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync(GetError(context.Response.StatusCode, "An unexpected error ocurred"));
            }
        }

        private string GetError(int status, string message)
        {
            return GetError(status, message, Enumerable.Empty<string>());
        }

        private string GetError(int status, string message, IEnumerable<string> messageToLog)
        {
            ApiException erroMessage = new ApiException(status, message,messageToLog);

            string result = JsonSerializer.Serialize(erroMessage, _jsonSerializerOptions);

            return result;
        }
    }
}