using NS.APICore.Extensions;
using NS.Identidade.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAppsettingsEnvironment(builder.Environment);

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.AddDependecyInjection();

builder.Services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();

//using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
//{
//    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    context.Database.EnsureCreated();
//}

app.UseSwaggerConfiguration();

app.UseApiConfiguration(builder.Environment);

app.Run();
