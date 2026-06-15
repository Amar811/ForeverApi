using Forever.Api.Configuration;

namespace Forever.Api.Extensions
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection AddPolicyAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                AuthorizationPolicies.AdminOnly,
                policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy(
                AuthorizationPolicies.CustomerOnly,
                policy =>
                   policy.RequireRole("Customer"));

                options.AddPolicy(
                AuthorizationPolicies.AdminOrManager,
                policy =>
                    policy.RequireRole(
                        "Admin",
                        "Manager"));


            });

            return services;
        }
    }
}
