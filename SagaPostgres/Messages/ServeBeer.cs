using System;

namespace SagaPostgres.Messages
{
    public interface ServeBeer
    {
        Guid OrderId { get; }
    }
}