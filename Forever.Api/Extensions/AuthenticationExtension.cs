using Forever.Api.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Forever.Api.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection
            AddJwtAuthentication(
                this IServiceCollection services,
                IConfiguration configuration)
        {
            var jwtSettings =
                configuration
                .GetSection("JwtSettings")
                .Get<JwtSettings>();

            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters =
            //        new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,

            //            ValidateAudience = true,

            //            ValidateLifetime = true,

            //            ValidateIssuerSigningKey = true,

            //            ValidIssuer =
            //                jwtSettings.Issuer,

            //            ValidAudience =
            //                jwtSettings.Audience,

            //            IssuerSigningKey =
            //                new SymmetricSecurityKey(
            //                    Encoding.UTF8.GetBytes(
            //                        jwtSettings.Key))
            //        };
            //});

                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(jwtSettings.Key)),
                            ClockSkew = TimeSpan.Zero
                        };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine(
                                $"JWT Error: {context.Exception.Message}");

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
