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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.RegisterDependencyInjection();

var rabbitMqSection = builder.Configuration.GetSection("RabbitMq");

builder.Services.Configure<RabbitMqOptions>(rabbitMqSection);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddHostedService<VenueRabbitMqService>();
builder.Services.AddHostedService<MenuRabbitMqService>();
builder.Services.AddHostedService<MenuItemRabbitMqService>();
builder.Services.AddHostedService<UserRabbitMqService>();

var app = builder.Build();

// Update CORS
app.UseCors("AllowAll");


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<UserExperienceDbContext>();

    await dbContext.Database.MigrateAsync();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAllOrigins");

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();