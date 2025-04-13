using Common;
using Confluent.Kafka;
using Crm.Kafka.Producer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IProducer, Producer>();
builder.Services.Configure<KafkaSettings>
       (builder.Configuration.GetSection(nameof(KafkaSettings)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.MapPost("/customer", async (CustomerDto customerdto, IProducer producer, IOptions<KafkaSettings> kafkaSettings) =>
{

    var @event = new CustomerEvent(Guid.NewGuid(), customerdto.FirstName, customerdto.LastName);

    await producer.SendAsync(kafkaSettings.Value.Topic, @event);
    return Results.Accepted();
});


app.Run();



record CustomerDto(string FirstName, string LastName);
