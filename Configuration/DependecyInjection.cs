using NS.APICore.User;
using NS.Identidade.API.Services;

namespace NS.Identidade.API.Configuration
{
    public static class DependecyInjectionConfig
    {
        public static void AddDependecyInjection(this IServiceCollection services)
        {


            services.AddScoped<AuthenticationService>();

            services.AddScoped<IAspNetUser, AspNetUser>();
        }
    }
}
