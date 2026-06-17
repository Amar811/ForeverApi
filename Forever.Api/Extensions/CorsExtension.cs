using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forever.Api.Extensions
{
    public static class CorsExtension
    {
        public const string ReactCorsPolicy = "ReactCorsPolicy";

        public static IServiceCollection AddReactCors(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var origins =
                configuration
                .GetSection("CorsSettings:AllowedOrigins")
                .Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy(
                    ReactCorsPolicy,
                    policy =>
                    {
                        if (origins != null && origins.Length > 0)
                        {
                            //policy
                            //    .WithOrigins(origins)
                            //    .WithMethods("GET", "POST", "PUT", "DELETE")
                            //    .WithHeaders("Content-Type", "Authorization", "Accept", "X-Requested-With")
                            //    .AllowCredentials();

                            // for development used this 
                            policy
                                 .WithOrigins(origins)
                                 .AllowAnyHeader()
                                 .AllowAnyMethod()
                                 .AllowCredentials();
                        }
                        else
                        {
                            //policy
                            //    .AllowAnyOrigin()
                            //    .WithMethods("GET", "POST", "PUT", "DELETE")
                            //    .WithHeaders("Content-Type", "Authorization", "Accept", "X-Requested-With");


                            policy
                                 .WithOrigins(origins)
                                 .AllowAnyHeader()
                                 .AllowAnyMethod()
                                 .AllowCredentials();
                        }
                    });
            });

            return services;
        }
    }
}

