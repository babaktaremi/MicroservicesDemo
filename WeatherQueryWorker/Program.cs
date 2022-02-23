using WeatherQueryWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddHttpClient(); //Set lifetime to five minutes

    })
    .Build();

await host.RunAsync();
