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
        }
    }
}