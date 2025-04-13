
using Common;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;
namespace Crm.Kafka.Producer;

public class Producer : IProducer
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly ILogger<Producer> _logger;

     public Producer(IOptions<KafkaSettings> kafkaSettings, ILogger<Producer> logger)
    {
        _kafkaSettings = kafkaSettings.Value;
        _logger = logger;
    }
    public async Task SendAsync(string topic, CustomerEvent customerEvent)
    {
        var config = new ProducerConfig
       {
            BootstrapServers = $"{_kafkaSettings.Hostname}:{_kafkaSettings.Port}"
       };

        using var producer = new ProducerBuilder<string, string>(config)
        .SetKeySerializer(Serializers.Utf8)
        .SetValueSerializer(Serializers.Utf8)
        .Build();

        var eventMessage = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(customerEvent)
        };

        var deliveryStatus = await producer.ProduceAsync(topic,eventMessage );
        
        if(deliveryStatus.Status == PersistenceStatus.NotPersisted)
        {
            throw new Exception(@$"
            No se pudo enviar el mensaje {customerEvent.GetType().Name} 
            hacia el topic - {topic}, 
            por la siguiente razon: {deliveryStatus.Message}");
        }
        _logger.LogInformation("Event sent!");
    }
}
