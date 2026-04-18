

namespace Facturacion.Api.Services;
public interface IEventPublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message);
}
