using System.Globalization;
using System.Text.RegularExpressions;
using TBC.OpenAPI.SDK.Core.Exceptions;

namespace TBC.OpenAPI.SDK.ExchangeRates.Helpers
{
    internal static class ParametersValidationHelper
    {
        private const string CURRENCY_PATTERN = "^[A-Z]{3}$";
        private const string CURRENCY_LIST_PATTERN = "^[A-Z]{3}(?:,[A-Z]{3})*$";

        internal static void ConvertionParameterValidation(decimal amount, string from, string to)
        {
            Regex currencyRegEx = new Regex(CURRENCY_PATTERN);

            if (string.IsNullOrEmpty(from))
                throw new OpenApiException("Base currency parameter 'from' must not be empty.");

            if (!currencyRegEx.IsMatch(from))
                throw new OpenApiException("Base currency format is invalid. Please use 3-letter currency codes.");

            if (string.IsNullOrEmpty(to))
                throw new OpenApiException("Target currency parameter 'to' must not be empty.");

            if (!currencyRegEx.IsMatch(to))
                throw new OpenApiException("Target currency format is invalid. Please use 3-letter currency codes.");
        }

        internal static void CurrencyFormatValidation(IEnumerable<string> currencies)
        {
            Regex currencyRegEx = new Regex(CURRENCY_PATTERN);

            if (currencies.Any(x => !currencyRegEx.IsMatch(x)))
                throw new OpenApiException("Currency format is invalid. Please use 3-letter currency codes.");
        }

        internal static void CurrencyListValidation(IEnumerable<string> currencies)
        {
            if (currencies.Any(x => string.IsNullOrEmpty(x)))
                throw new OpenApiException("List of currencies contains empty element.");

            CurrencyFormatValidation(currencies);
        }

        internal static void DateFormatValidation(string date)
        {
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                throw new OpenApiException("Date format is invalid. Please use YYYY-MM-dd format.");
        }
    }
}
