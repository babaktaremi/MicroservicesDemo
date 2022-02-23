using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncCommunication.Shared;
using MassTransit;

namespace InventoryServiceWorker.Consumers
{
    public class InventoryQueryConsumer:IConsumer<InventoryQueryModel>
    {
        private readonly ILogger<InventoryQueryConsumer> _logger;
        private readonly IBus _bus;

        public InventoryQueryConsumer(ILogger<InventoryQueryConsumer> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<InventoryQueryModel> context)
        {

            _logger.LogWarning($"Received Query For Product ID {context.Message.Id}");

           var product = Inventory.DummyInventory.FirstOrDefault(c => c.Id == context.Message.Id);

           if (product != null)
           {
               var endPoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://localhost/SalesQueue"));

               await endPoint.Send(new InventoryQueryResultModel()
                   { ProductCount = product.ProductCount, ProductName = product.ProductName });
           }
        }
    }
}
