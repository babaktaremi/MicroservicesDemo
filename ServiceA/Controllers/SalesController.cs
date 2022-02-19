using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceA.Model;

namespace ServiceA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ILogger<SalesController> _logger;
        private readonly HttpClient _httpClient;
        public SalesController(ILogger<SalesController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _httpClient = clientFactory.CreateClient();
        }

        [HttpGet("SalesStatus")]
        public async Task<IActionResult> GetSaleStatus()
        {
           using var scope= _logger.BeginScope("Initializing Sale Service Request at {RequestTime}",DateTime.Now);

            _logger.LogWarning($"Making Api Request to Inventory Service");

            var apiResult =
                await _httpClient.GetFromJsonAsync<List<InventoryInquiryModel>>("https://localhost:7045/InventoryInquiry");

            var result = apiResult.Select(item => item.ProductCount > 0
                    ? new SalesInquiryModel() { ProductName = item.ProductName, IsAvailable = true }
                    : new SalesInquiryModel() { ProductName = item.ProductName, IsAvailable = false })
                .ToList();

            return Ok(result);
        }

    }
}
