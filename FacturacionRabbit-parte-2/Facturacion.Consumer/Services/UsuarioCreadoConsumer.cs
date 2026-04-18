using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facturacion.Consumer.Services;

public class UsuarioCreadoConsumer : IEventHandler
{
    public string QueueName => "usuarios.creado.queue";
    public string RoutingKey => "usuario.#";
    public string Exchange => "usuarios";

    public Task HandleAsync(string message)
    {
        Console.WriteLine($"Usuario -> : {message}");
        return Task.CompletedTask;
    }
}
