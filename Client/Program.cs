using System.Runtime.InteropServices.JavaScript;
using BlazorBingo.Client;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<GameSettings>();

#pragma warning disable CA1416 // Validate platform compatibility
await JSHost.ImportAsync("interopModule", "../script/interop.js");
#pragma warning restore CA1416 // Validate platform compatibility

await builder.Build().RunAsync();
