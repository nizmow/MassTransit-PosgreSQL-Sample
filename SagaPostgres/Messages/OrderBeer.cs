using System;

namespace SagaPostgres.Messages
{
    public interface OrderBeer
    {
        Guid OrderId { get; }
        
        string BeerType { get; }
    }
}