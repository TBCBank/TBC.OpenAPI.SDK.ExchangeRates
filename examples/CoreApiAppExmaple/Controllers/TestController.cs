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
        public async Task<ActionResult<OfficialRate>> GetOfficialRates(CancellationToken cancellationToken = default)
        {
            var result = await _exchangeRatesClient.GetOfficialRates(new string[] { "EUR", "USD"},cancellationToken);
            //var result = await _exchangeRatesClient.GetOfficialRatesByDate(new string[] { "EUR", "USD" }, "05-2022-01", cancellationToken);
            //var result = await _exchangeRatesClient.ConvertOfficialRates(120.4M,"GEL","USD", cancellationToken);
            //var result = await _exchangeRatesClient.GetCommercialRates(new string[] { "EUR", "USD" }, cancellationToken);
            //var result = await _exchangeRatesClient.ConvertCommercialRate(120.5M, "GEL", "USD" , cancellationToken);

            return Ok(result);
        }
    }
}
