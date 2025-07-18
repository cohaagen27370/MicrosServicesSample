namespace Gateway.Repositories;

public interface IRabbitMqProducer
{
    Task SendMessage<T>(T message, string queueName = null);
}