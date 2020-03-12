using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SagaPostgres.DataAccess;

namespace SagaPostgres
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        /// <summary>
        /// EFCore tools use this to instantiate a dbcontext for migrations and so on.
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging(ConfigureLogging)
                .ConfigureServices(ConfigureServices)
                .ConfigureContainer<ContainerBuilder>(ConfigureContainer);
        }

        private static void ConfigureLogging(ILoggingBuilder logging)
        {
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Debug);
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddHostedService<SagaPostgresHostedService>();
            services.AddDbContext<ServeBeerStateDbContext>(builder =>
            {
                builder.UseNpgsql(context.Configuration.GetConnectionString("local"));
            });
        }

        private static void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddMassTransit(cfg =>
            {
                cfg.AddSagaStateMachine<ServeBeerStateMachine, ServeBeerState>().InMemoryRepository();
                cfg.AddInMemoryBus((context, busConfig) =>
                {
                    busConfig.ConfigureEndpoints(context);
                });
            });

            containerBuilder.RegisterType<UserInterface>();
        }
    }
}