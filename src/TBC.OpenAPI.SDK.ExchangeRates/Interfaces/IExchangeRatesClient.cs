using TBC.OpenAPI.SDK.Core;
using TBC.OpenAPI.SDK.ExchangeRates.Models;

namespace TBC.OpenAPI.SDK.ExchangeRates
{
    public interface IExchangeRatesClient : IOpenApiClient
    {
        Task<GetCommercialRatesResponse?> GetCommercialRates(string[] currencies, CancellationToken cancellationToken = default);
        Task<ConvertCommercialRatesResponse?> ConvertCommercialRate(decimal amount, string from, string to, CancellationToken cancellationToken = default);
        Task<List<GetOfficialRate>?> GetOfficialRates(string[]? currencies = null, CancellationToken cancellationToken = default);
        Task<ConvertOfficialRatesResponse?> ConvertOfficialRates(string amount, string from, string to, CancellationToken cancellationToken = default);
    }
}
