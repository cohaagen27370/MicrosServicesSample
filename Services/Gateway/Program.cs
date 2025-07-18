using System.Text.Json;
using System.Text.Json.Serialization;
using Gateway.Filters;
using Gateway.Interceptors;
using Gateway.Repositories;
using Grpc.Net.ClientFactory;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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
// builder.Services.AddScoped<IRabbitMqProducer, RabbitMqProducer>();


// Add services to the container.
builder.Services.AddGrpcClient<SeedService.SeedGrpcService.SeedGrpcServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["Grpc:SeedService:Url"] ?? "http://localhost:5001");
    options.ChannelOptionsActions.Add(channelOptions =>
    {
        channelOptions.Credentials = Grpc.Core.ChannelCredentials.Insecure;
    });
}).AddInterceptor(() => new HeaderAddingInterceptor(builder.Configuration["Grpc:SeedService:Url"]!, "X-Api-Key", builder.Configuration["Grpc:SeedService:apikey"]!));
// Ou
// .AddInterceptor(() => new HeaderAddingInterceptor("X-Api-Key", "VotreCle"));
builder.Services.AddGrpcClient<CropService.CropGrpcService.CropGrpcServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["Grpc:CropService:Url"] ?? "http://localhost:5001");
    
    // Si vous travaillez en développement avec HTTP/2 sans TLS (ex: http://localhost:5000),
    // vous devrez potentiellement ajouter ceci pour contourner les problèmes de certificat/TLS
    options.ChannelOptionsActions.Add(channelOptions =>
    {
        // N'utilisez pas Grpc.Core.ChannelCredentials.Insecure en production!
        // C'est uniquement pour le développement sans TLS.
        channelOptions.Credentials = Grpc.Core.ChannelCredentials.Insecure;
    });
}).AddInterceptor(() => new HeaderAddingInterceptor(builder.Configuration["Grpc:CropService:Url"]!,"X-Api-Key", builder.Configuration["Grpc:CropService:apikey"]!));

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5023);
});

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
}));

builder.Services.AddControllers(
    options => options.Filters.Add<ExceptionFilter>()
).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.CustomSchemaIds(type => type.FullName));
builder.Services.AddScoped<ISeedRepository, SeedRepository>();
builder.Services.AddScoped<ICropRepository, CropRepository>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors(); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();