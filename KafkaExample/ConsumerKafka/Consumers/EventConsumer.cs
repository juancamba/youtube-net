using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Common;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace ConsumerKafka.Consumers
{
    public class EventConsumer : IEventConsumer
    {
 private readonly ILogger<EventConsumer> _logger;
        private readonly ConsumerConfig _config;
        private readonly IServiceScopeFactory _serviceProvider;

        public EventConsumer(
            IOptions<ConsumerConfig> config,
            IServiceScopeFactory serviceProvider,
            ILogger<EventConsumer> logger)
        {
            _config = config.Value;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void Consume(string topic)
        {
            using var consumer = new ConsumerBuilder<string, string>(_config)
                       .SetKeyDeserializer(Deserializers.Utf8)
                       .SetValueDeserializer(Deserializers.Utf8)
                       .Build();

            consumer.Subscribe(topic);

            while (true)
            {
                var consumeResult = consumer.Consume();
                if (consumeResult is null) continue;
                if (consumeResult.Message is null) continue;

                var options = new JsonSerializerOptions
                {
                    //Converters = { new EventJsonConverter() }
                };

                var @event = JsonSerializer
                                .Deserialize<CustomerEvent>(
                                    consumeResult.Message.Value,
                                    options
                                );


                if (@event is null)
                {
                    throw new ArgumentNullException("no se pudo procesar el mensaje");
                }

             
               _logger.LogInformation($"Message received {consumeResult.Message.Value}");
                consumer.Commit(consumeResult);

            }

        }
    }
}