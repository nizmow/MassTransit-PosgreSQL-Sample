using System;
using Automatonymous;

namespace SagaPostgres
{
    public class ServeBeerState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        
        // using OrderId to correlated events just for example's sake.
        public Guid OrderId { get; set; }
        
        public string State { get; set; }
        
        public string BeerType { get; set; }
        
        public decimal Tip { get; set; }
        
        // composite event support, for when the order is both served and paid.
        public int PaidAndServedStatus { get; set; }
    }
}
