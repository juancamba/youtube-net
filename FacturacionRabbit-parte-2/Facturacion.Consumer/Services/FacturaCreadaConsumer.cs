
using System.Text.Json;
using Facturacion.Contracts;

namespace Facturacion.Consumer.Services;

public class FacturaCreadaConsumer : IEventHandler
{
    public string QueueName => "factura.creada.queue";
    public string RoutingKey => "factura.creada";
    public string Exchange => "facturacion";

    public Task HandleAsync(string message)
    {
        Console.WriteLine($"Factura creada: {message}");
        return Task.CompletedTask;
    }
}
