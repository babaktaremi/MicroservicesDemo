using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherQueryWorker.Services
{
    internal class WeatherService:IWeatherService
    {
        private readonly HttpClient _client;

        public WeatherService(HttpClient client)
        {
            _client = client;
        }
        public async Task<bool> TestApi()
        {
            var testApi =await _client.GetAsync("http://localhost:5125/weatherforecast");

            return testApi.IsSuccessStatusCode;
        }
    }
}
