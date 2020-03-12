using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SagaPostgres.DataAccess
{
    public class ServeBeerStateMap : SagaClassMap<ServeBeerState>
    {
        protected override void Configure(EntityTypeBuilder<ServeBeerState> entity, ModelBuilder model)
        {
            base.Configure(entity, model);

            // note: not required to configure CorrelationId, as the SagaClassMap base class does this for us.
            
            entity.HasIndex(x => x.OrderId).IsUnique();

            entity.Property(x => x.OrderId).IsRequired();
            entity.Property(x => x.BeerType).HasMaxLength(255);
            entity.Property(x => x.CurrentState).HasMaxLength(255).IsRequired();
        }
    }
}