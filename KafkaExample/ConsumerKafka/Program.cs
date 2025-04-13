using Confluent.Kafka;
using ConsumerKafka;
using ConsumerKafka.Consumers;
var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IEventConsumer, EventConsumer>();

builder.Services.Configure<ConsumerConfig>(
  builder.Configuration.GetSection(nameof(ConsumerConfig))
);
builder.Services.AddHostedService<ConsumerHostedService>();
var host = builder.Build();
host.Run();
