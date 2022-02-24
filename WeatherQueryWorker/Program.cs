using Polly;
using WeatherQueryWorker;
using WeatherQueryWorker.Services;
using Polly.Extensions.Http;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddHttpClient<IWeatherService,WeatherService>().SetHandlerLifetime(TimeSpan.FromMinutes(5)) 
            .AddPolicyHandler(_ => HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.BadRequest)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                    retryAttempt)))); 

    })
    .Build();

await host.RunAsync();
