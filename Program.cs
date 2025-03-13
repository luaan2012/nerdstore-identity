using NS.Identidade.API.Configuration;
using NS.Identidade.API.Data;
using NS.MessageBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.AddDependecyInjection();

builder.Services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();

app.Services.EnsureCreatedDatabase<ApplicationDbContext>();

app.UseSwaggerConfiguration();

app.UseApiConfiguration(builder.Environment);

app.Run();
