using CropMicroService.Context;
using CropMicroService.Middlewares;
using CropMicroService.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
builder.Services.AddScoped<ICropDbRepository, CropDbRepository>();

builder.Services.AddDbContext<CropContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
Logger logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine("wwwroot", "logs", "file.log"), rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Services.AddControllers();

builder.Host.UseSerilog(logger);

builder.Services.AddRequestDecompression();
builder.Services.AddResponseCompression();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5104, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

WebApplication app = builder.Build();

app.UseSerilogRequestLogging();

app.UseMiddleware<ApiKeyMiddleware>();

app.MapGrpcService<CropMicroService.Services.CropServiceImplementation>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");


using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;
    var context = services.GetRequiredService<CropContext>();
    context.Database.Migrate();
}

try
{
    app.Logger.LogTrace("init service");
    app.Run();
}
catch (Exception exception)
{
    //NLog: catch setup errors
    app.Logger.LogError(exception, "Stopped program because of exception");
    throw;
}