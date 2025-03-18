using Blazored.SessionStorage;
using Harmony.UI;
using Harmony.UI.Handlers;
using Harmony.UI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<AuthenticationHandler>();

builder.Services.AddHttpClient("HarmonyAPI")
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["HarmonyAPI"] ?? throw new NullReferenceException()))
    .AddHttpMessageHandler<AuthenticationHandler>();

builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

builder.Services.AddBlazoredSessionStorageAsSingleton();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
