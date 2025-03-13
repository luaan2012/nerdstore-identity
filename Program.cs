using Microsoft.EntityFrameworkCore;
using NS.APICore.Extensions;
using NS.Identidade.API.Configuration;
using NS.Identidade.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.AddDependecyInjection();

builder.Services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        context.Database.Migrate();
        Console.WriteLine("Banco criado ou migrado com sucesso.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

app.UseSwaggerConfiguration();

app.UseApiConfiguration(builder.Environment);

app.Run();
