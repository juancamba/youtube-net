using System.Text;
using System.Text.Json;
using Facturacion.Contracts;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



var factory = new ConnectionFactory() { HostName = "localhost" };
app.MapPost("/factura/crear", async () =>
{
   
    using var connection = await factory.CreateConnectionAsync();
    using var channel = await connection.CreateChannelAsync();

    await channel.ExchangeDeclareAsync("facturacion", ExchangeType.Topic, durable: true);
    // guardar en bd

    var factura = new FacturaCreada(Guid.NewGuid(), 100);

    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(factura));

    await channel.BasicPublishAsync(
        exchange: "facturacion",
        routingKey: "factura.creada",
        body: body
    );

    return Results.Ok(factura);
});

app.UseHttpsRedirection();




app.Run();


