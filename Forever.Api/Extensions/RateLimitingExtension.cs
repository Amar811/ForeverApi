using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Forever.Api.Extensions
{
    public static class RateLimitingExtension
    {
        public const string IpFixedWindowPolicy = "IpFixedWindow";
        public const string LoginPolicy = "LoginPolicy";

        public static IServiceCollection AddAppRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                // IP-based fixed window: 60 requests per minute
                options.AddPolicy<string>(IpFixedWindowPolicy, context =>
                    RateLimitPartition.GetFixedWindowLimiter<string>(
                        partitionKey: GetIpPartitionKey(context),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 60,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        }));

                // Tight policy for login endpoints: 5 requests per minute
                options.AddPolicy<string>(LoginPolicy, context =>
                    RateLimitPartition.GetFixedWindowLimiter<string>(
                        partitionKey: "login", // single partition for all login requests (use literal, not a lambda)
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        }));

                options.OnRejected = (context, ct) =>
                {
                    // Add Retry-After header if possible
                    try
                    {
                        context.HttpContext.Response.Headers["Retry-After"] = "60";
                    }
                    catch
                    {
                        // ignore header set failures
                    }

                    return ValueTask.CompletedTask;
                };
            });

            return services;
        }

        private static string GetIpPartitionKey(HttpContext ctx)
        {
            var forwarded = ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            return !string.IsNullOrEmpty(forwarded)
                ? forwarded
                : ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }
    }
}