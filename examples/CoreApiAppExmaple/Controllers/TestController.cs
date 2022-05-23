using Microsoft.AspNetCore.Mvc;
using TBC.OpenAPI.SDK.ExchangeRates.Models;
using TBC.OpenAPI.SDK.ExchangeRates;

namespace CoreApiAppExmaple.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IExchangeRatesClient _exchangeRatesClient;

        public TestController(IExchangeRatesClient exchangeRatesClient)
        {
            _exchangeRatesClient = exchangeRatesClient;
        }

        [HttpGet]
        public async Task<ActionResult<SomeObject>> GetSomeObject(CancellationToken cancellationToken = default)
        {
            var result = await _exchangeRatesClient.GetSomeObjectAsync(cancellationToken);
            return Ok(result);
        }
    }
}
