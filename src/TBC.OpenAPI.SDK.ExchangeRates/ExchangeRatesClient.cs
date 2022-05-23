using TBC.OpenAPI.SDK.Core;
using TBC.OpenAPI.SDK.Core.Exceptions;
using TBC.OpenAPI.SDK.Core.Models;
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


        #region CommercialRates
        /// <summary>
        /// კომერციული კურსის დასაბრუნებელი მეთოდი
        /// </summary>
        /// <param name="currencies">(required) ვალუტები, რომლებიც უნდა დაბრუნდეს</param>
        /// <param name="cancellationToken">(optional)</param>
        /// <returns>აბრუნებს გადაცემული ვალუტების კურსებს</returns>

        public async Task<GetCommercialRatesResponse?> GetCommercialRates(string[] currencies, CancellationToken cancellationToken = default)
        {
            var queryParams = new QueryParamCollection();
            queryParams.Add("currency", currencies.ToString());

            var result = await _http.GetJsonAsync<GetCommercialRatesResponse>("/commercial", queryParams , cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccess)
                throw new OpenApiException(result.Problem?.Title ?? "Unexpected error occurred", result.Exception);

            return result.Data!;
        }

        /// <summary>
        /// კომერციული კურსის დასაკონვერტირებელი მეთოდი
        /// </summary>
        /// <param name="amount">(required) დასაკონვენტირებელი თანხის რაოდენობა</param>
        /// <param name="from">(required) ვალუტა, საიდანაც უნდა დაკონვერტირდეს</param>
        /// <param name="to">(required) ვალუტა, რაშიც უნდა დაკონვერტირდეს</param>
        /// <param name="cancellationToken">(optional)</param>
        /// <returns>აბრუნებს დაკონვერტირებული ვალუტის კურსს</returns>

        public async Task<ConvertCommercialRatesResponse?> ConvertCommercialRate(decimal amount, string from, string to, CancellationToken cancellationToken = default)
        {
            var queryParams = new QueryParamCollection();
            queryParams.Add("amount", amount);
            queryParams.Add("from", from);
            queryParams.Add("to", to);

            var result = await _http.GetJsonAsync<ConvertCommercialRatesResponse>("/commercial/convert", queryParams,  cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccess)
                throw new OpenApiException(result.Problem?.Title ?? "Unexpected error occurred", result.Exception);

            return result.Data!;
        }
        #endregion




        #region Official Rates
        /// <summary>
        /// ოფიციალური კურსების დასაბრუნებელი მეთოდი
        /// </summary>
        /// <param name="currencies">(optional) ვალუტების მასივი, რომლებიც უნდა დაბრუნდეს(ამ პარამეტრის არგადაცემის შემთხვევაში აბრუნებს ყველა ვალუტას)</param>
        /// <param name="cancellationToken">(optional)</param>
        /// <returns>აბრუნებს ყველა ვალუტის ან გადაცემული ვალუტების კურსებს</returns>

        public async Task<List<GetOfficialRate>?> GetOfficialRates(string[]? currencies = null, CancellationToken cancellationToken = default)
        {
            var queryParams = new QueryParamCollection();
            if (currencies?.Any() ?? false)
            {
                queryParams.Add("currency", currencies.ToString());
            }

            var result = await _http.GetJsonAsync<List<GetOfficialRate>?>("/nbg", queryParams, cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccess)
                throw new OpenApiException(result.Problem?.Title ?? "Unexpected error occurred", result.Exception);

            return result.Data!;
        }

        /// <summary>
        /// ოფიციალური კურსის დასაკონვერტირებელი მეთოდი
        /// </summary>
        /// <param name="amount">(required) დასაკონვენტირებელი თანხის რაოდენობა</param>
        /// <param name="from">(required) ვალუტა, საიდანაც უნდა დაკონვერტირდეს</param>
        /// <param name="to">(required) ვალუტა, რაშიც უნდა დაკონვერტირდეს</param>
        /// <param name="cancellationToken">(optional)</param>
        /// <returns>დაკონვერტირებული ვალუტის კურსი</returns>
        public async Task<ConvertOfficialRatesResponse?> ConvertOfficialRates(string amount, string from, string to, CancellationToken cancellationToken = default)
        {
            var queryParams = new QueryParamCollection();
            queryParams.Add("amount", amount);
            queryParams.Add("from", from);
            queryParams.Add("to", to);

            var result = await _http.GetJsonAsync<ConvertOfficialRatesResponse>("/nbg/convert", queryParams,  cancellationToken).ConfigureAwait(false);

            if (!result.IsSuccess)
                throw new OpenApiException(result.Problem?.Title ?? "Unexpected error occurred", result.Exception);

            return result.Data!;
        }
        #endregion
    }
}