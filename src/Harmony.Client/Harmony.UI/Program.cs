using Blazored.LocalStorage;
using Harmony.UI;
using Harmony.UI.Handlers;
using Harmony.UI.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add Session Storage
builder.Services.AddBlazoredLocalStorageAsSingleton();

// Add MudBlazor
builder.Services.AddMudServices();

// Add Services
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<AuthenticationHandler>();
builder.Services.AddSingleton<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<ServerService>();
builder.Services.AddSingleton<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());

// Configuration for the HTTP Handler
builder.Services.AddHttpClient("HarmonyAPI")
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["HarmonyAPI"] ?? throw new NullReferenceException()))
    .AddHttpMessageHandler<AuthenticationHandler>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
