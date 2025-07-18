using System.Text;
using Common.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SeedMicroService.Context;
using SeedMicroService.Entities;

namespace SeedMicroService.BackServices;

public class RabbitMqConsumerService: BackgroundService
{
    private readonly ILogger<RabbitMqConsumerService> _logger;
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly IServiceScopeFactory _scopeFactory;

    public RabbitMqConsumerService(
        ILogger<RabbitMqConsumerService> logger,
        IConnection connection,
        IOptions<RabbitMqSettings> rabbitMqSettings,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _connection = connection;
        _rabbitMqSettings = rabbitMqSettings.Value;
        _scopeFactory = scopeFactory;
        
        _channel = _connection.CreateChannelAsync().Result;

        // Déclarer la queue. S'assurer qu'elle existe et est durable.
        _channel.QueueDeclareAsync(queue: _rabbitMqSettings.QueueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        _logger.LogInformation("RabbitMQ Consumer Service initialized for queue: {queueName}", _rabbitMqSettings.QueueName);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        
        // Configurer le consommateur asynchrone
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation(" [x] Received '{s}' from queue '{queueName}'", message, _rabbitMqSettings.QueueName);
            
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<SeedContext>();

                    var allDogs = await dbContext.Seeds.ToListAsync(stoppingToken);
                    dbContext.Seeds.RemoveRange(allDogs);
                    await dbContext.SaveChangesAsync(stoppingToken);

                    dbContext.Seeds.Add(
                        new SeedEntity { Id = Guid.NewGuid(), Name = "Buddy", Species = "Labrador", RisingTime = 3 });
                    dbContext.Seeds.Add(new SeedEntity
                        { Id = Guid.NewGuid(), Name = "Lucy", Species = "Golden Retriever", RisingTime = 5 });
                    dbContext.Seeds.Add(new SeedEntity
                        { Id = Guid.NewGuid(), Name = "Max", Species = "German Shepherd", RisingTime = 2 });

                    await dbContext.SaveChangesAsync(stoppingToken);
                }

                // Confirmer que le message a été traité (ACK)
                // C'est très important pour éviter que le message ne soit re-délivré si le traitement échoue.
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                _logger.LogInformation(" [x] Acknowledged message: {eventDeliveryTag}", ea.DeliveryTag);
            }
            catch (OperationCanceledException)
            {
                // Le service est arrêté, ne pas traiter le message
                _logger.LogWarning("Processing of message {eventDeliveryTag} cancelled.", ea.DeliveryTag);
                // Ne pas ACK ni NACK, laisser le message retourner à la queue ou être re-délivré si pas de reconnexion
                // Pour éviter la perte, un nack peut être approprié si l'on sait que le traitement a échoué.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing message {ea.DeliveryTag}: {message}");
                // En cas d'erreur de traitement, re-mettre le message dans la queue (NACK)
                // 'requeue: true' pour le remettre immédiatement
                // 'multiple: false' pour NACK un seul message
                await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true, stoppingToken);
            }
        };

        // Démarrer la consommation
        _channel.BasicConsumeAsync(_rabbitMqSettings.QueueName, false, consumer, stoppingToken);

        // Cette tâche continue de fonctionner en arrière-plan tant que le service est actif.
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _logger.LogInformation("RabbitMQ Consumer Service is stopping. Disposing channel and connection.");
        _channel?.CloseAsync();
        _connection?.CloseAsync();
        base.Dispose();
    }
    
}