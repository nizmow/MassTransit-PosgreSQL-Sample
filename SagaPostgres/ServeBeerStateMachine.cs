using System;
using Automatonymous;
using MassTransit;
using SagaPostgres.Messages;

namespace SagaPostgres
{
    public class ServeBeerStateMachine : MassTransitStateMachine<ServeBeerState>
    {
        public ServeBeerStateMachine()
        {
            InstanceState(x => x.CurrentState);
            
            Event(() => Order, cfg => cfg
                .CorrelateBy((instance, context) => context.Message.OrderId == instance.OrderId)
                .SelectId(x => NewId.NextGuid()));
            Event(() => Pay, cfg => cfg
                .CorrelateBy((instance, context) => context.Message.OrderId == instance.OrderId)
                .OnMissingInstance(x => x.Fault()));
            Event(() => Serve, cfg => cfg
                .CorrelateBy((instance, context) => context.Message.OrderId == instance.OrderId)
                .OnMissingInstance(x => x.Fault()));

            Initially(
                When(Order)
                    .Then(context =>
                    {
                        context.Instance.OrderId = context.Data.OrderId;
                        context.Instance.BeerType = context.Data.BeerType;
                    })
                    .Then(_ => Console.WriteLine("<Initial> => Ordered"))
                    .TransitionTo(Ordered));
            
            During(Ordered,
                When(Pay)
                    .Then(_ => Console.WriteLine("Ordered => Paid"))
                    .TransitionTo(Paid),
                When(Serve)
                    .Then(_ => Console.WriteLine("Ordered => Served"))
                    .TransitionTo(Served));
            
            CompositeEvent(() => PaidAndServed, x => x.PaidAndServedStatus, Serve, Pay);

            DuringAny(
                When(Pay)
                    .Then(context => context.Instance.Tip = context.Data.Tip),
                When(PaidAndServed)
                    .Then(_ => Console.WriteLine("PaidAndServed => <Finalize>"))
                    .Finalize());
        }
        
        public State Ordered { get; private set; }
        public State Served { get; private set; }
        public State Paid { get; private set; }
        
        public Event<OrderBeer> Order { get; private set; }
        public Event<ServeBeer> Serve { get; private set; }
        public Event<PayForBeer> Pay { get; private set; }
        public Event PaidAndServed { get; private set; }
    }
}