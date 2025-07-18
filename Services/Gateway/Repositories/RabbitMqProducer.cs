using System.Text.Json;
using Common.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Gateway.Repositories;

public class RabbitMqProducer(IConnection connection, IOptions<RabbitMqSettings> rabbitMqSettings)
    : IRabbitMqProducer
{
    private readonly RabbitMqSettings _rabbitMqSettings = rabbitMqSettings.Value;

    public async Task SendMessage<T>(T message, string queueName = null)
    {
        var finalQueueName = queueName ?? _rabbitMqSettings.QueueName;

        await using var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(queue: finalQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = System.Text.Encoding.UTF8.GetBytes(json);
            
        await channel.BasicPublishAsync(string.Empty, finalQueueName,
            true,body  );

        Console.WriteLine($" [x] Sent message to queue '{finalQueueName}': {json}");
    }
}