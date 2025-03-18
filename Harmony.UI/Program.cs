using Blazored.LocalStorage;
using Harmony.UI;
using Harmony.UI.Handlers;
using Harmony.UI.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add Session Storage
builder.Services.AddBlazoredLocalStorageAsSingleton();

// Add Services
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<AuthenticationHandler>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());

// Configuration for the HTTP Handler
builder.Services.AddHttpClient("HarmonyAPI")
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["HarmonyAPI"] ?? throw new NullReferenceException()))
    .AddHttpMessageHandler<AuthenticationHandler>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
