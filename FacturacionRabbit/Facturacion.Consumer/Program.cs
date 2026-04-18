using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Facturacion.Contracts;

var factory = new ConnectionFactory() { HostName = "localhost" };

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("facturacion", ExchangeType.Topic, durable: true);

await channel.QueueDeclareAsync(
    queue: "facturacion.queue",
    durable: true,
    exclusive: false,
    autoDelete: false
);

await channel.QueueBindAsync(
    queue: "facturacion.queue",
    exchange: "facturacion",
    routingKey: "factura.*"
);

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync  += (model, ea) =>
{
    var routingKey = ea.RoutingKey;
    var json = Encoding.UTF8.GetString(ea.Body.ToArray());

    Console.WriteLine($"Evento: {routingKey}");
    
    switch (routingKey)
    {
        case "factura.creada":
            var creada = JsonSerializer.Deserialize<FacturaCreada>(json);
            Console.WriteLine($"Creada: {creada.Id} - {creada.Precio}");
            
            break;
    }
     return Task.CompletedTask;
};

await channel.BasicConsumeAsync("facturacion.queue", autoAck: true, consumer);

Console.WriteLine("Escuchando eventos...");
Console.ReadLine();