using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConsumerKafka.Consumers;

public class ConsumerHostedService : IHostedService
{

    private readonly ILogger<ConsumerHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConsumerConfig _config;
    public ConsumerHostedService(ILogger<ConsumerHostedService> logger, IServiceProvider serviceProvider, IOptions<ConsumerConfig> config)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _config = config.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumer working..");
         var topic = "KAFKA_TOPIC";

        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var eventConsumer = scope.ServiceProvider
                                .GetRequiredService<IEventConsumer>();
            
            Task.Run( () => eventConsumer.Consume(topic), cancellationToken);

        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumer stopped..");
        return Task.CompletedTask;
    }
}
