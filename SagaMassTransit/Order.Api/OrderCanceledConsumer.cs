using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace Order.Api
{
    public class OrderCanceledConsumer : IConsumer<OrderCancelled>
    {
        public Task Consume(ConsumeContext<OrderCancelled> context)
        {
            Console.WriteLine(
                $"📧 EMAIL SIMULADO: El pedido {context.Message.OrderId} se CANCELÓ."
            );

            return Task.CompletedTask;
        }
    }
}