
using System.Text.Json;
using Facturacion.Contracts;

namespace Facturacion.Consumer.Services;
public class FacturaPagadaConsumer : IEventHandler
{
    public string QueueName => "factura.pagada.queue";
    public string RoutingKey => "factura.pagada";
    public string Exchange => "facturacion";

    public Task HandleAsync(string message)
    {
        Console.WriteLine($"Factura pagada: {message}");

        // tratamiento del mensaje
        return Task.CompletedTask;
    }
}