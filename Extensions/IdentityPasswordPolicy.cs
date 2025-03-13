using Microsoft.AspNetCore.Identity;

namespace NS.Identidade.API.Extensions
{
    public static class IdentityPasswordPolicy
    {
        public static IServiceCollection AddPasswordConfiguration(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;        
            });

            return services;
        }
    }
}
