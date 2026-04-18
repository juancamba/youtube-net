using Facturacion.Consumer.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class RabbitDispatcher
{
    private readonly IEnumerable<IEventHandler> _handlers;

    public RabbitDispatcher(IEnumerable<IEventHandler> handlers)
    {
        _handlers = handlers;
    }

    public async Task Start(IConnection connection)
    {
        foreach (var handler in _handlers)
        {
            var channel = await connection.CreateChannelAsync();

            // Declarar exchange
            await channel.ExchangeDeclareAsync(
                handler.Exchange,
                ExchangeType.Topic,
                durable: true
            );

            // Declarar cola
            var queue = await channel.QueueDeclareAsync(
                queue: handler.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            // Binding
            await channel.QueueBindAsync(
                queue: queue.QueueName,
                exchange: handler.Exchange,
                routingKey: handler.RoutingKey
            );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                try
                {
                    await handler.HandleAsync(message);

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");

                    // aquí puedes hacer retry o DLQ
                    await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                }
            };

            await channel.BasicConsumeAsync(
                queue: queue.QueueName,
                autoAck: false,
                consumer: consumer
            );
        }
    }
}