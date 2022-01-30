using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorClient;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("WeatherApi")
    .AddHttpMessageHandler(sp =>
    {
        var handler = sp.GetService<AuthorizationMessageHandler>()
            .ConfigureHandler(
                authorizedUrls: new[] { "https://localhost:5002" },
                scopes: new[] { "weatherapi" });
        return handler;
    });

builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("WeatherApi"));

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
  //  builder.Configuration.Bind("Local", options.ProviderOptions);
    builder.Configuration.Bind("oidc", options.ProviderOptions);
});

await builder.Build().RunAsync();
