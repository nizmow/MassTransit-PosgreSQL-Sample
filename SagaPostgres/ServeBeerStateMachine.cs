using Automatonymous;
using MassTransit;
using SagaPostgres.Messages;

namespace SagaPostgres
{
    public class ServeBeerStateMachine : MassTransitStateMachine<ServeBeerState>
    {
        public ServeBeerStateMachine()
        {
            Event(() => Order, x => x
                .CorrelateById(context => context.Message.OrderId)
                .SelectId(x => NewId.NextGuid()));
            Event(() => Pay, x => x
                .CorrelateById(context => context.Message.OrderId)
                .SelectId(x => NewId.NextGuid()));
            Event(() => Serve, x => x
                .CorrelateById(context => context.Message.OrderId)
                .SelectId(x => NewId.NextGuid()));

            Initially(
                When(Order)
                    .TransitionTo(Ordered));
            
            During(Ordered,
                When(Pay)
                    .TransitionTo(Paid),
                When(Serve)
                    .TransitionTo(Served));
            
            CompositeEvent(() => PaidAndServed, x => x.PaidAndServedStatus, Serve, Pay);

            DuringAny(
                When(PaidAndServed)
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