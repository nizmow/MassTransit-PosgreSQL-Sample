using System.Collections.Generic;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;

namespace SagaPostgres.DataAccess
{
    public class ServeBeerStateDbContext : SagaDbContext
    {
        public ServeBeerStateDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new ServeBeerStateMap(); }
        }
    }
}