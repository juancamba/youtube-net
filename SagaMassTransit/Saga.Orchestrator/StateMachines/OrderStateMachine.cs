using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace Saga.Orchestrator.StateMachines;


public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public State AwaitingStock { get; private set; }
    public State AwaitingPayment { get; private set; }

    public Event<CreateOrder> OrderCreated { get; private set; }
    public Event<StockReserved> StockReserved { get; private set; }
    public Event<StockFailed> StockFailed { get; private set; }
    public Event<PaymentCompleted> PaymentCompleted { get; private set; }
    public Event<PaymentFailed> PaymentFailed { get; private set; }

    public OrderStateMachine()
    {
        InstanceState(x => x.CurrentState);

        /*
        Event: Básicamente le está diciendo al sistema: “cuando llegue un mensaje como OrderCreated o PaymentCompleted,
        usa el OrderId que viene dentro para saber a qué pedido pertenece y a qué instancia de la saga debe aplicarse
        */

        Event(() => OrderCreated, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockReserved, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockFailed, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => PaymentCompleted, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => PaymentFailed, x => x.CorrelateById(m => m.Message.OrderId));

        
        
        Initially(
            // “Cuando llegue un mensaje OrderCreated a mi cola (order-state), ejecuto esto”
            When(OrderCreated)
                .Then(ctx =>
                {
                    Console.WriteLine($"[SAGA] CreateOrder recibido: {ctx.Message.OrderId}");
                    ctx.Saga.ProductId = ctx.Message.ProductId;
                    ctx.Saga.Quantity = ctx.Message.Quantity;
                })
                // publico en la cola stock-service que está escuchando stock service, orden para reservar stock
                .Send(new Uri("queue:stock-service"), ctx =>
                    new ReserveStock(ctx.Saga.CorrelationId,
                                    ctx.Saga.ProductId,
                                    ctx.Saga.Quantity))
                // cambio estado a AwaitingStock               
                .TransitionTo(AwaitingStock)
        );

        During(AwaitingStock,


            When(StockReserved)
                .Then(ctx => Console.WriteLine($"[SAGA] StockReserved recibido: {ctx.Message.OrderId}"))
                .Send(new Uri("queue:payment-service"), ctx =>
                    new ProcessPayment(ctx.Saga.CorrelationId, 100))
                .TransitionTo(AwaitingPayment),

            When(StockFailed)
                .Then(ctx => Console.WriteLine($"[SAGA] StockFailed recibido: {ctx.Message.OrderId}"))
                .Publish(ctx => new OrderCancelled(ctx.Saga.CorrelationId, ctx.Message.Reason))
                .Finalize()
        );

        During(AwaitingPayment,
            When(PaymentCompleted)
                .Publish(ctx => new OrderCompleted(ctx.Saga.CorrelationId))
                .Finalize(),

            When(PaymentFailed)
                .Send(new Uri("queue:stock-service"), ctx =>
                    new ReleaseStock(ctx.Saga.CorrelationId))
                .Publish(ctx => new OrderCancelled(ctx.Saga.CorrelationId, ctx.Message.Reason))
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}

