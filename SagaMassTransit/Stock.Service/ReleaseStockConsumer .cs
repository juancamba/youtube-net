using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace Stock.Service;


public class ReleaseStockConsumer : IConsumer<ReleaseStock>
{
    public Task Consume(ConsumeContext<ReleaseStock> context)
    {
        Console.WriteLine($"Stock liberado {context.Message.OrderId}");
        return Task.CompletedTask;
    }
}
