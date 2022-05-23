using TBC.OpenAPI.SDK.Core;
using TBC.OpenAPI.SDK.ExchangeRates.Models;

namespace TBC.OpenAPI.SDK.ExchangeRates
{
    public interface IExchangeRatesClient : IOpenApiClient
    {
        Task<SomeObject> GetSomeObjectAsync(CancellationToken cancellationToken = default);
    }
}
