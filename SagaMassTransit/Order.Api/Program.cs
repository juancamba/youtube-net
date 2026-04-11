using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddMassTransit(x =>
{
    
     x.AddConsumer<OrderCompletedConsumer>();
     x.AddConsumer<OrderCanceledConsumer>();

    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost");
         cfg.ConfigureEndpoints(context);
    });
      x.SetInMemorySagaRepositoryProvider();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapPost("/order", async ([FromServices] IPublishEndpoint publish) =>
{
    var id = Guid.NewGuid();

    await publish.Publish(new CreateOrder(id, 1, 1));

    return TypedResults.Ok(id);

}).WithName("CreateOrder");


app.Run();



