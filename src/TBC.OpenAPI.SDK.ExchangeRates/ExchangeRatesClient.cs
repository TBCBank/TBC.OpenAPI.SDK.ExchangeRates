using System.Runtime.CompilerServices;
using TBC.OpenAPI.SDK.Core;
using TBC.OpenAPI.SDK.Core.Exceptions;
using TBC.OpenAPI.SDK.Core.Models;
using TBC.OpenAPI.SDK.ExchangeRates.Helpers;
using TBC.OpenAPI.SDK.ExchangeRates.Models;

[assembly: InternalsVisibleTo("TBC.OpenAPI.SDK.ExchangeRates.Tests")]
namespace TBC.OpenAPI.SDK.ExchangeRates
{
    internal class ExchangeRatesClient : IExchangeRatesClient
    {
        private readonly IHttpHelper<ExchangeRatesClient> _http;

        public ExchangeRatesClient(IHttpHelper<ExchangeRatesClient> http)
        {
            _http = http;
        }

        #region CommercialRates
        
        /// <summary>
        /// Gets commercial exchange rates for Georgian Lari
        /// </summary>
        /// <param name="currencies">List of comma-separated 3-letter currency codes for limiting results to specific currencies. e.g. USD,EUR,JPY. If this parameter is not provided, rates will be returned for all currencies</param>
        /// <param name="cancellationToken">(optional)</param>
        /// <returns>Returns list of TBC Bank's commercial exchange rates</returns>
        public async Task<GetCommercialRatesResponse> GetCommercialRates(IEnumerable<string> currencies = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new QueryParamCollection();

            if (currencies != null)
            {
                ParametersValidationHelper.CurrencyListValidation(currencies);

                queryParams.Add("currency", string.Join(",", currencies));
            }

            var result = await _http.GetJsonAsync<GetCommercialRatesResponse>("/commercial", queryParams , cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccess)
                throw new OpenApiException(result.Problem?.Title ?? "Unexpected error occurred", result.Exception);

            return result.Data;
        }

        /// <summary>
        /// Converts amount between currencies based on TBC bank's commercial exchange rates
        /// </summary>
        /// <param name="amount">(required) Value to be converted</param>
        /// <param name="from">(required) Base currency from which given amount should be converted</param>
        /// <param name="to">(required) Target currency to which amount should be converted</param>
        /// <param name="cancellationToken">(optional)</param>
        /// <returns>Returns convertion value of amount between currencies specified in from and to parameters based on TBC bank's commercial exchange rates</returns>
        public async Task<ConvertCommercialRatesResponse> ConvertCommercialRate(decimal amount, string from, string to, CancellationToken cancellationToken = default)
        {
            ParametersValidationHelper.ConvertionParameterValidation(amount, from, to);

            var queryParams = new QueryParamCollection
            {
                { "amount", amount },
                { "from", from },
                { "to", to }
            };

            var result = await _http.GetJsonAsync<ConvertCommercialRatesResponse>("/commercial/convert", queryParams,  cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccess)
                throw new OpenApiException(result.Problem?.Title ?? "Unexpected error occurred", result.Exception);

            return result.Data;
        }

        #endregion

        #region Official Rates

        /// <summary>
        /// Gets official exchange rates for Georgian Lari
        /// </summary>
        /// <param name="currencies">List of comma-separated 3-letter currency codes for limiting results to specific currencies. e.g. USD,EUR,JPY. If this parameter is not provided, rates will be returned for all currencies</param>
        /// <param name="cancellationToken">(optional)</param>
        /// <returns>Returns list of official exchange rates</returns>
        public async Task<List<OfficialRate>> GetOfficialRates(IEnumerable<string> currencies = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new QueryParamCollection();
            
            if (currencies != null)
            {
                ParametersValidationHelper.CurrencyListValidation(currencies);

                queryParams.Add("currency", string.Join(",", currencies));
            }

            var result = await _http.GetJsonAsync<List<OfficialRate>>("/nbg", queryParams, cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccess)
                throw new OpenApiException(result.Problem?.Title ?? "Unexpected error occurred", result.Exception);

            return result.Data;
        }

        /// <summary>
        /// Gets official exchange rates for Georgian Lari by specific date
        /// </summary>
        /// <param name="date">Parameter for getting official rates for specific date. Date should be passed in YYYY-MM-dd format</param>
        /// <param name="currencies">List of comma-separated 3-letter currency codes for limiting results to specific currencies. e.g. USD,EUR,JPY. If this parameter is not provided, rates will be returned for all currencies</param>
        /// <param name="cancellationToken">(optional)</param>
        /// <returns>Returns list of official exchange rates on specific date</returns>
        public async Task<List<OfficialRate>> GetOfficialRatesByDate(IEnumerable<string> currencies = null, string? date = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new QueryParamCollection();

            if (currencies != null)
            {
                ParametersValidationHelper.CurrencyListValidation(currencies);

                queryParams.Add("currency", string.Join(",", currencies));
            }

            if (!string.IsNullOrEmpty(date))
            {
                ParametersValidationHelper.DateFormatValidation(date);

                queryParams.Add("date", date);
            }

            var result = await _http.GetJsonAsync<List<OfficialRate>>("/nbg", queryParams, cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccess)
                throw new OpenApiException(result.Problem?.Title ?? "Unexpected error occurred", result.Exception);

            return result.Data;
        }

        /// <summary>
        /// Converts amount between currencies based on official exchange rates
        /// </summary>
        /// <param name="amount">(required) Value to be converted</param>
        /// <param name="from">(required) Base currency from which given amount should be converted</param>
        /// <param name="to">(required) Target currency to which amount should be converted</param>
        /// <param name="cancellationToken">(optional)</param>
        /// <returns>Returns convertion value of amount between currencies specified in from and to parameters based on official exchange rates</returns>
        public async Task<ConvertOfficialRatesResponse> ConvertOfficialRates(decimal amount, string from, string to, CancellationToken cancellationToken = default)
        {
            ParametersValidationHelper.ConvertionParameterValidation(amount, from, to);

            var queryParams = new QueryParamCollection
            {
                { "amount", amount },
                { "from", from },
                { "to", to }
            };

            var result = await _http.GetJsonAsync<ConvertOfficialRatesResponse>("/nbg/convert", queryParams,  cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccess)
                throw new OpenApiException(result.Problem?.Title ?? "Unexpected error occurred", result.Exception);

            return result.Data;
        }
        
        #endregion
    }
}