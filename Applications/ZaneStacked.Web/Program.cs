using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ZaneStacked.Web.Components;
using ZaneStacked.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["ApiUrl"] ??
                          throw new Exception("ApiUrl must be set in appsettings.json"))
});

builder.Services.AddScoped<SkillService>();
builder.Services.AddScoped<ProjectService>();

await builder.Build().RunAsync();