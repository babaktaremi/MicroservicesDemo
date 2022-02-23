using System.Reflection;
using AsyncCommunication.Shared;
using InventoryServiceWorker;
using InventoryServiceWorker.Consumers;
using MassTransit;

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddMassTransit(x =>
        {
            x.AddConsumer<InventoryQueryConsumer>();
            x.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                configurator.ReceiveEndpoint("InventoryQueue", endpointConfigurator =>
                {
                    endpointConfigurator.ConfigureConsumer<InventoryQueryConsumer>(context);

                });
            });
           
        });

        services.AddMassTransitHostedService();
    })
    .Build();

await host.RunAsync();
