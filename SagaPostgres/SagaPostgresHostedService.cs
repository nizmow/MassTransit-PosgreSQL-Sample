using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SagaPostgres
{
    public class SagaPostgresHostedService : IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly UserInterface _userInterface;

        public SagaPostgresHostedService(IBusControl busControl, UserInterface userInterface)
        {
            _busControl = busControl;
            _userInterface = userInterface;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _busControl.StartAsync(cancellationToken);

            // we want this to run on another thread and this is a quick and dirty way to do it.
#pragma warning disable 4014
            Task.Run(async () =>
            {
                // danger that the bus may start before we display our UI!
                await Task.Delay(1000, cancellationToken);
                await _userInterface.Run();
            }, cancellationToken);
#pragma warning restore 4014
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync(cancellationToken);
        }
    }
}
