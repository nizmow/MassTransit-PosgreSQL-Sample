using System;

namespace SagaPostgres.Messages
{
    public interface PayForBeer
    {
        Guid OrderId { get; }
        
        decimal Tip { get; }
    }
}