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
        private readonly ILogger<SagaPostgresHostedService> _logger;

        public SagaPostgresHostedService(IBusControl busControl, ILogger<SagaPostgresHostedService> logger)
        {
            _busControl = busControl;
            _logger = logger;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _busControl.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync(cancellationToken);
        }
    }
}
