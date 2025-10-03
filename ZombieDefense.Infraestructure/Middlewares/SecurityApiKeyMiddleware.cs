using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ZombieDefense.Infraestructure.Middlewares
{
    public class SecurityApiKeyMiddleware(RequestDelegate next)
    {
        private const string API_KEY_HEADER_NAME = "X-API-KEY";

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            var path = context.Request.Path.Value;
            if (path.StartsWith("/swagger") || path.StartsWith("/favicon"))
            {
                await next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key is missing");
                return;
            }

            var apiKey = configuration["ApiKeySettings:Key"];

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            await next(context);
        }
    }
}
