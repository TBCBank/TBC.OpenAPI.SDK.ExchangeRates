using TBC.OpenAPI.SDK.Core;
using TBC.OpenAPI.SDK.Core.Exceptions;
using TBC.OpenAPI.SDK.ExchangeRates.Models;

namespace TBC.OpenAPI.SDK.ExchangeRates
{
    public class ExchangeRatesClient : IExchangeRatesClient
    {
        private readonly HttpHelper<ExchangeRatesClient> _http;

        public ExchangeRatesClient(HttpHelper<ExchangeRatesClient> http)
        {
            _http = http;
        }

        public async Task<SomeObject> GetSomeObjectAsync(CancellationToken cancellationToken = default)
        {
            var result = await _http.GetJsonAsync<SomeObject>("/", cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccess)
                throw new OpenApiException(result.Problem?.Title ?? "Unexpected error occurred", result.Exception);

            return result.Data!;
        }
    }
}