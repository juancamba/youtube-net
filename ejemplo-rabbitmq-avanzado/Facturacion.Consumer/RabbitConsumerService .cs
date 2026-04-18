using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace Facturacion.Consumer;
public class RabbitConsumerService : BackgroundService
{
    private readonly RabbitDispatcher _dispatcher;
    private readonly IConnection _connection;

    public RabbitConsumerService(
        RabbitDispatcher dispatcher,
        IConnection connection)
    {
        _dispatcher = dispatcher;
        _connection = connection;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _dispatcher.Start(_connection);

        Console.WriteLine("Consumer iniciado...");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
