using Ghak.libraries.GoogleMap.Utils;
using MapTest;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton(new GoogleMapsConfiguration
{
    ApiKey = "AIzaSyBdkgvniMdyFPAcTlcZivr8f30iU-kn1T0",
    WithLog = false,
    InitLocation = new GoogleMapLocation(23.5880, 58.3829),
    InitZoom = 11,
});

await builder.Build().RunAsync();
