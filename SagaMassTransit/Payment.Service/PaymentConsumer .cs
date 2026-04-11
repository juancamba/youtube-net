using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace Payment.Service;
public class PaymentConsumer : IConsumer<ProcessPayment>
{
    public async Task Consume(ConsumeContext<ProcessPayment> context)
    {
        var success = new Random().Next(0, 2) == 1;

        if (success)
        {
            
            Console.WriteLine("Payment Completed !");
            // esto va a un exchange, no a una cola
            await context.Publish(new PaymentCompleted(context.Message.OrderId));
        }
            
        else
        {
            Console.WriteLine("PaymentFailed !");
            await context.Publish(new PaymentFailed(context.Message.OrderId, "Pago fallido"));
        }
            
    }
}
