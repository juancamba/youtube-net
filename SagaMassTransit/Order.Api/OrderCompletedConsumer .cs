using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace Order.Api;

public class OrderCompletedConsumer : IConsumer<OrderCompleted>
{
    public Task Consume(ConsumeContext<OrderCompleted> context)
    {
        Console.WriteLine(
            $"📧 EMAIL SIMULADO: El pedido {context.Message.OrderId} se completó correctamente."
        );

        return Task.CompletedTask;
    }
}
