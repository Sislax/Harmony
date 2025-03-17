using Harmony.Application;
using Harmony.Infrastructure;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

//Configuration of the app

string environment = builder.Configuration["Environment"] ?? throw new NullReferenceException();

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("secrets.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.

// Add Logger
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddNLog(configuration);
});

// Add Services from Application Layer (MediatR, AutoMapper, Validators)
builder.Services.AddApplicationServices();

// Add Services from Infrastructure Layer (DbContext, Identity, Auth, Repositories, Services)
builder.Services.AddInfrastructure(configuration);

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Add Seed Data (Roles and Users)
using(IServiceScope scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.AddSeedData();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();