using OrderMicroService.Context;
using OrderMicroService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

builder.Services.AddDbContext<OrderContext>(options =>
{
    options.UseInMemoryDatabase("OrderDatabase"); 
});

// Add services to the container.
Logger logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine("wwwroot", "logs", "file.log"), rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Services.AddControllers();

builder.Host.UseSerilog(logger);

builder.Services.AddRequestDecompression();
builder.Services.AddResponseCompression();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;
    var context = services.GetRequiredService<OrderContext>();
    context.Database.EnsureCreated(); 
}

app.UseSerilogRequestLogging();

app.MapGrpcService<OrderMicroService.Services.OrderServiceImplementation>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
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