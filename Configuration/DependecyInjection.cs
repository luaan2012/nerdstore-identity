using System.Collections;
using NS.APICore.User;
using NS.Identidade.API.Services;

namespace NS.Identidade.API.Configuration
{
    public static class DependecyInjectionConfig
    {
        public static void AddDependecyInjection(this IServiceCollection services)
        {
            var envVars = Environment.GetEnvironmentVariables()
            .Cast<DictionaryEntry>()
            .ToDictionary(entry => (string)entry.Key, entry => (string)entry.Value);

            foreach (var env in envVars)
            {
                Console.WriteLine($"{env.Key}: {env.Value}");
            }

            services.AddScoped<AuthenticationService>();

            services.AddScoped<IAspNetUser, AspNetUser>();
        }
    }
}
