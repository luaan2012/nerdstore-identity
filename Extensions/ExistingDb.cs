using NS.Identidade.API.Data;

namespace NS.Identidade.API.Extensions
{
	public static class ExistingDb
	{
		public static void DbIsExisting(this IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				context.Database.EnsureCreated();
			}
		}
	}
}
