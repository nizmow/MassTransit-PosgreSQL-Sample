using Autofac;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace SagaPostgres.DataAccess
{
    public class ServeBeerStateDbContextFactory : ISagaDbContextFactory<ServeBeerState>
    {
        private readonly ILifetimeScope _defaultLifetimeScope;

        public ServeBeerStateDbContextFactory(ILifetimeScope defaultLifetimeScope)
        {
            _defaultLifetimeScope = defaultLifetimeScope;
        }

        public DbContext Create()
        {
            return _defaultLifetimeScope.Resolve<DbContext>();
        }

        public DbContext CreateScoped<T>(ConsumeContext<T> context)
            where T : class
        {            
            if (context.TryGetPayload(out ILifetimeScope currentScope))
                return currentScope.Resolve<DbContext>();

            return Create();
        }

        public void Release(DbContext dbContext)
        {
            // Purposely left blank, the disposal of the dbContext is controlled by the container (autofac in this case)
        }
    }
}