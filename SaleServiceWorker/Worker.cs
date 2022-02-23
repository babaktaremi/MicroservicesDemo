
using AsyncCommunication.Shared;
using MassTransit;

namespace SaleServiceWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBus _bus;

        public Worker(ILogger<Worker> logger, IBusControl bus)
        {
            _logger = logger;
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Sale Service running... ");
            while (!stoppingToken.IsCancellationRequested)
            {
                var rnd = (new Random()).Next(1, 3);
               
                var endPoint= await _bus.GetSendEndpoint(new Uri("rabbitmq://localhost/InventoryQueue"));
                await endPoint.Send(new InventoryQueryModel() { Id = rnd }, stoppingToken);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}