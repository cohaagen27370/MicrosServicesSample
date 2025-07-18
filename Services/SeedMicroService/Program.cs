using SeedMicroService.Context;
using SeedMicroService.Repositories;
using Microsoft.EntityFrameworkCore;
using SeedMicroService.Middlewares;
using Serilog;
using Serilog.Core;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
builder.Services.AddScoped<ISeedDbRepository, SeedDbRepository>();

builder.Services.AddDbContext<SeedContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


// builder.Services.AddDbContext<SeedContext>(options =>
// {
//     options.UseInMemoryDatabase("DogDatabase"); 
// });

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
    serverOptions.ListenAnyIP(5075, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

builder.Services.AddResponseCaching();

// builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));
// builder.Services.AddSingleton<IConnectionFactory>(sp =>
// {
//     var rabbitMqSettings = sp.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
//     return new ConnectionFactory()
//     {
//         HostName = rabbitMqSettings.HostName,
//         Port = rabbitMqSettings.Port,
//         UserName = rabbitMqSettings.UserName,
//         Password = rabbitMqSettings.Password
//     };
// });
//
// builder.Services.AddSingleton<IConnection>(sp =>
// {
//     var factory = sp.GetRequiredService<IConnectionFactory>();
//     return factory.CreateConnectionAsync().Result;
// });
//
// builder.Services.AddHostedService<RabbitMqConsumerService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SeedContext>();
    context.Database.Migrate();
}

app.UseSerilogRequestLogging();

app.UseMiddleware<ApiKeyMiddleware>();

app.MapGrpcService<SeedMicroService.Services.SeedServiceImplementation>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

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