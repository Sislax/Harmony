using Harmony.API;

var builder = WebApplication.CreateBuilder(args);

//Configuration of the app

//string environment = builder.Configuration["Environment"] ?? throw new NullReferenceException();

//IConfiguration configuration = new ConfigurationBuilder()
//    .SetBasePath(builder.Environment.ContentRootPath)
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
//    .AddJsonFile("secrets.json", optional: false, reloadOnChange: true)
//    .AddEnvironmentVariables()
//    .Build();

builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddPresentation(builder.Configuration);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

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
    //app.MapOpenApi();
}

app.UseExceptionHandler("/error");

app.UseCors("HarmonyUI");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();