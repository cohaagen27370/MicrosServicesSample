using System.Text.Json.Serialization;
using Gateway.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpcClient<DogService.DogGrpcService.DogGrpcServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["Grpc:DogServiceUrl"] ?? "https://localhost:5001");

    // Si vous travaillez en développement avec HTTP/2 sans TLS (ex: http://localhost:5000),
    // vous devrez potentiellement ajouter ceci pour contourner les problèmes de certificat/TLS
    options.ChannelOptionsActions.Add(channelOptions =>
    {
        // N'utilisez pas Grpc.Core.ChannelCredentials.Insecure en production!
        // C'est uniquement pour le développement sans TLS.
        channelOptions.Credentials = Grpc.Core.ChannelCredentials.Insecure;
    });
});
builder.Services.AddGrpcClient<OrderService.OrderGrpcService.OrderGrpcServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["Grpc:OrderServiceUrl"] ?? "https://localhost:5001");

    // Si vous travaillez en développement avec HTTP/2 sans TLS (ex: http://localhost:5000),
    // vous devrez potentiellement ajouter ceci pour contourner les problèmes de certificat/TLS
    options.ChannelOptionsActions.Add(channelOptions =>
    {
        // N'utilisez pas Grpc.Core.ChannelCredentials.Insecure en production!
        // C'est uniquement pour le développement sans TLS.
        channelOptions.Credentials = Grpc.Core.ChannelCredentials.Insecure;
    });
});

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.WithOrigins("https://localhost:7165")
        .AllowAnyHeader()
        .AllowAnyMethod();
}));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.CustomSchemaIds(type => type.FullName));
builder.Services.AddScoped<IDogRepository, DogRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();