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

// Add Swagger
builder.Services.AddSwaggerGen();

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

// Add CORS policy to allow client origin in localhost
builder.Services.AddCors(policy =>
{
    policy.AddPolicy("HarmonyUI", builder => builder.WithOrigins("https://localhost:7222")
    .SetIsOriginAllowed((host) => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Add Seed Data (Roles and Users)
//using(IServiceScope scope = app.Services.CreateScope())
//{
//    await scope.ServiceProvider.AddSeedData();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseCors("HarmonyUI");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();