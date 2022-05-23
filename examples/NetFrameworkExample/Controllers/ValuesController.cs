using System.Threading.Tasks;
using System.Web.Http;
using TBC.OpenAPI.SDK.Core;
using TBC.OpenAPI.SDK.ExchangeRates.Extensions;

namespace NetFrameworkExample.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public async Task<IHttpActionResult> Get()
        {
            var exchangeRatesClient = OpenApiClientFactory.Instance.GetExchangeRatesClient();

            var result = await exchangeRatesClient.GetSomeObjectAsync();

            return Ok(result);
        }
    }
}
