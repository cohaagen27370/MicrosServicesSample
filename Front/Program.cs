using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Front;
using Front.Services;
using Radzen;
using Serilog;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddRadzenComponents();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.BrowserConsole()
    .CreateLogger();

builder.Services.AddHttpClient<DataService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5023/");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddSingleton(Log.Logger);
await builder.Build().RunAsync();