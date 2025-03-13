using NetDevPack.Security.JwtSigningCredentials.AspNetCore;
using NS.APICore.Extensions;
using NS.APICore.Identity;

namespace NS.Identidade.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
          
            services.AddControllers();

            services.AddCorsGeral();
        }

        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Total");

            app.UseAuthConfiguration();

            app.UseEndpoints(endpoins =>
            {
                endpoins.MapControllers();
            });

            app.UseJwksDiscovery();
        }
    }
}
