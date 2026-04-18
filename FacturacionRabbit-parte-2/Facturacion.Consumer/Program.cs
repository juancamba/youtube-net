using RabbitMQ.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Facturacion.Consumer.Services;
using Facturacion.Consumer;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IEventHandler, FacturaCreadaConsumer>();
        services.AddSingleton<IEventHandler, FacturaCanceladaConsumer>();
        services.AddSingleton<IEventHandler, FacturaPagadaConsumer>();
  services.AddSingleton<IEventHandler, UsuarioCreadoConsumer>();
        services.AddSingleton<RabbitDispatcher>();

        services.AddSingleton<IConnection>(_ =>
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
                
            };

            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

      
        services.AddHostedService<RabbitConsumerService>();
    })
    .Build();

await builder.RunAsync();