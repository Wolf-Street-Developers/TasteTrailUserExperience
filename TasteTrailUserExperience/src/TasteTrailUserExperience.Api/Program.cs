using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TasteTrailData.Api.Common.Extensions.ServiceCollection;
using TasteTrailUserExperience.Api.Common.Extensions.ServiceCollection;
using TasteTrailUserExperience.Api.Common.Utilities;
using TasteTrailUserExperience.Core.Common.Options;
using TasteTrailUserExperience.Infrastructure.BackgroundServices;
using TasteTrailUserExperience.Infrastructure.Common.Data;

var builder = WebApplication.CreateBuilder(args);

SetupEnvironmentVariables.SetupEnvironmentVariablesMethod(builder.Configuration, builder.Environment.IsDevelopment());

builder.Services.InitDbContext(builder.Configuration);
builder.Services.InitAuth(builder.Configuration);
builder.Services.InitSwagger();
builder.Services.InitCors();

builder.Services.RegisterDependencyInjection();

var rabbitMqSection = builder.Configuration.GetSection("RabbitMq");

builder.Services.Configure<RabbitMqOptions>(rabbitMqSection);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddHostedService<VenueRabbitMqService>();
builder.Services.AddHostedService<MenuRabbitMqService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<UserExperienceDbContext>();

    await dbContext.Database.MigrateAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();