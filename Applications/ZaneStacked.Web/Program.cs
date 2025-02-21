using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ZaneStacked.Web.Components;
using ZaneStacked.Web.Identity;
using ZaneStacked.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Source: https://github.com/dotnet/blazor-samples/tree/main/8.0/BlazorWebAssemblyStandaloneWithIdentity

// Register the cookie handler
builder.Services.AddTransient<CookieHandler>();

// Set up authorization
builder.Services.AddAuthorizationCore();

// Register the custom state provider
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

// Register the account management interface
builder.Services.AddScoped(
    sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

// builder.Services.AddScoped(_ => new HttpClient
// {
//     BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
// });
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
    DefaultRequestHeaders =
    {
        { "Accept", "application/json" }
    }
});
builder.Services.AddHttpClient(
        "Api",
        opt => opt.BaseAddress = new Uri(builder.Configuration["ApiUrl"] ??
                                         throw new Exception("ApiUrl must be set in appsettings.json"))
    )
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped<SkillService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<ExcuseService>();

await builder.Build().RunAsync();