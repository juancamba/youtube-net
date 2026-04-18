using Facturacion.Contracts;

namespace Facturacion.Api.Services;




public class FacturacionPublisher
{
    private const string EXCHANGE = "facturacion";
    private readonly IEventPublisher _publisher;

    public FacturacionPublisher(IEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public Task FacturaCreada(FacturaCreada e)
        => _publisher.PublishAsync(EXCHANGE, "factura.creada", e);

    public Task FacturaPagada(FacturaPagada e)
        => _publisher.PublishAsync(EXCHANGE, "factura.pagada", e);
}
