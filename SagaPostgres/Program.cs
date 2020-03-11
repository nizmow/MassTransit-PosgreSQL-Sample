using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SagaPostgres
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging(ConfigureLogging)
                .ConfigureServices(ConfigureServices)
                .ConfigureContainer<ContainerBuilder>(ConfigureContainer);

            await hostBuilder.RunConsoleAsync();
        }

        private static void ConfigureLogging(ILoggingBuilder logging)
        {
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Debug);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<SagaPostgresHostedService>();
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