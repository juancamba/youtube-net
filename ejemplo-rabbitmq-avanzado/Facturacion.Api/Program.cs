using System.Text;
using System.Text.Json;
using Facturacion.Api.Services;
using Facturacion.Contracts;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();


builder.Services.AddSingleton<IConnection>(_ =>
{
    var factory = new ConnectionFactory
    {
        HostName = "localhost"
    };

    return factory.CreateConnectionAsync().GetAwaiter().GetResult();
});



builder.Services.AddSingleton<IEventPublisher, RabbitMqPublisher>();
builder.Services.AddScoped<FacturacionPublisher>();
builder.Services.AddScoped<UsuarioPublisher>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}




app.MapPost("/factura/crear", async (FacturacionPublisher facturacionPublisher) =>
{

    var factura = new FacturaCreada(Guid.NewGuid(), 100);
    Console.WriteLine(factura);
    await facturacionPublisher.FacturaCreada(factura);
    return Results.Ok(factura);
});

app.MapPost("/factura/pagar", async (FacturacionPublisher facturacionPublisher) =>
{

    var factura = new FacturaPagada(Guid.NewGuid(), 100);
    Console.WriteLine(factura);
    await facturacionPublisher.FacturaPagada(factura);
    return Results.Ok(factura);
});


app.MapPost("/usuarios/crear", async (UsuarioPublisher usuarioPublisher) =>
{
    var usuario = new UsuarioCreado(Guid.NewGuid(), "some@email.com");
    Console.WriteLine(usuario);
    await usuarioPublisher.UsuarioCreado(usuario);
    return Results.Ok(usuario);
});

app.UseHttpsRedirection();




app.Run();


