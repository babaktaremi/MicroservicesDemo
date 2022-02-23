using System.Reflection;
using AsyncCommunication.Shared;
using MassTransit;
using SaleServiceWorker;
using SaleServiceWorker.Consumers;

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<InventoryQueryResultConsumer>();
            x.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                configurator.ReceiveEndpoint("SalesQueue", endpointConfigurator =>
                {
                    endpointConfigurator.ConfigureConsumer<InventoryQueryResultConsumer>(context);

                });
            });
            x.AddConsumer<InventoryQueryResultConsumer>();
        });

        services.AddMassTransitHostedService();

    })
    .Build();

await host.RunAsync();
