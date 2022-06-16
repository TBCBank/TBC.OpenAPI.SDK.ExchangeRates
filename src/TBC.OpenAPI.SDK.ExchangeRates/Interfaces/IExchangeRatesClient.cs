using TBC.OpenAPI.SDK.Core;
using TBC.OpenAPI.SDK.ExchangeRates.Models;

namespace TBC.OpenAPI.SDK.ExchangeRates
{
    public interface IExchangeRatesClient : IOpenApiClient
    {
        Task<GetCommercialRatesResponse> GetCommercialRates(IEnumerable<string> currencies = null, CancellationToken cancellationToken = default);
        Task<ConvertCommercialRatesResponse> ConvertCommercialRate(decimal amount, string from, string to, CancellationToken cancellationToken = default);
        Task<List<OfficialRate>> GetOfficialRates(IEnumerable<string> currencies = null, CancellationToken cancellationToken = default);
        Task<List<OfficialRate>> GetOfficialRatesByDate(IEnumerable<string> currencies = null, string date = null, CancellationToken cancellationToken = default);
        Task<ConvertOfficialRatesResponse> ConvertOfficialRates(decimal amount, string from, string to, CancellationToken cancellationToken = default);
    }
}
