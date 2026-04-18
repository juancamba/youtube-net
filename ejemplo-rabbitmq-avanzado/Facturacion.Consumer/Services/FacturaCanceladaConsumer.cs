
using System.Text.Json;
using Facturacion.Contracts;

namespace Facturacion.Consumer.Services;

public class FacturaCanceladaConsumer : IEventHandler
{
    public string QueueName => "factura.cancelada.queue";
    public string RoutingKey => "factura.cancelada";
    public string Exchange => "facturacion";

    public Task HandleAsync(string message)
    {
        Console.WriteLine($"Factura cancelada: {message}");
        return Task.CompletedTask;
    }
}