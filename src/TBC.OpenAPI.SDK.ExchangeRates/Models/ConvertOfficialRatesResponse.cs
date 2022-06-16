namespace TBC.OpenAPI.SDK.ExchangeRates.Models
{
    /// <summary>
    /// Convertation by official exchange rate response model
    /// </summary>
    public class ConvertOfficialRatesResponse
    {
        /// <summary>
        /// Base currency from which given amount was converted
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Target currency to which given amount was converted
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Amount that was converted
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Conversion result
        /// </summary>
        public decimal Value { get; set; }
    }
}