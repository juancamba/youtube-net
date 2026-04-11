

using Contracts;
using MassTransit;

namespace Stock.Service;

public class ReserveStockConsumer : IConsumer<ReserveStock>
{
    public async Task Consume(ConsumeContext<ReserveStock> context)
    {
        var success = true;

        if (success)
        {


            Console.WriteLine($"Stock StockReserved {context.Message.OrderId}");
            await context.Publish(new StockReserved(context.Message.OrderId));
        }

        else
        {
            Console.WriteLine($"Stock StockReserved Failed {context.Message.OrderId}");
            await context.Publish(new StockFailed(context.Message.OrderId, "No stock"));
        }

    }
}