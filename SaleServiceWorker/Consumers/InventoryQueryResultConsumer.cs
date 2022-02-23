using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncCommunication.Shared;
using MassTransit;

namespace SaleServiceWorker.Consumers
{
    public class InventoryQueryResultConsumer:IConsumer<InventoryQueryResultModel>
    {
        private readonly ILogger<InventoryQueryResultConsumer> _logger;

        public InventoryQueryResultConsumer(ILogger<InventoryQueryResultConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<InventoryQueryResultModel> context)
        {
            _logger.LogWarning($"Product {context.Message.ProductName} has {context.Message.ProductCount} units in inventory");
            return Task.CompletedTask;
        }
    }
}
