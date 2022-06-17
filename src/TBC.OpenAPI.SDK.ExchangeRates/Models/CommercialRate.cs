namespace TBC.OpenAPI.SDK.ExchangeRates.Models
{
    /// <summary>
    /// TBC Bank's commercial exchange rate response model
    /// </summary>
    public class CommercialRate
    {
        /// <summary>
        /// 3-digit currency code
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Rate value at which bank is buying target currency
        /// </summary>
        public decimal Buy { get; set; }

        /// <summary>
        /// Rate value at which bank is selling target currency
        /// </summary>
        public decimal Sell { get; set; }
    }
}