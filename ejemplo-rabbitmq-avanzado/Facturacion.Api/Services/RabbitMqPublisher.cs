
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Facturacion.Api.Services;
public class RabbitMqPublisher : IEventPublisher, IDisposable
{
    private readonly IChannel _channel;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public RabbitMqPublisher(IConnection connection)
    {
        _channel = connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message)
    {
        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await _semaphore.WaitAsync();
        try
        {
            await _channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                body: body
            );
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
    }
}