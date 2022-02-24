using System.Net;
using System.Net.Http.Json;
using Polly;
using Polly.Extensions.Http;
using WeatherQueryWorker.Services;

namespace WeatherQueryWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var client = scope.ServiceProvider.GetService<IWeatherService>();

            while (!stoppingToken.IsCancellationRequested)
            {
                var apiTest = await client.TestApi();

                _logger.LogWarning(apiTest ? "Api is Healthy" : "Api is not healthy");
                await Task.Delay(1_000, stoppingToken);
            }
        }
    }
}