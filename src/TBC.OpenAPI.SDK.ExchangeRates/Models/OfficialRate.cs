namespace TBC.OpenAPI.SDK.ExchangeRates.Models
{
    /// <summary>
    /// Official exchange rates response model
    /// </summary>
    public class OfficialRate
    {
        /// <summary>
        /// 3-digit Currency code
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Official exchange rate value
        /// </summary>
        public decimal Value { get; set; }
    }
}