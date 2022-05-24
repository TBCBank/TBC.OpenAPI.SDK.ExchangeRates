namespace TBC.OpenAPI.SDK.ExchangeRates.Models
{
    /// <summary>
    /// კომერციული კურსის მოდელი
    /// </summary>
    public class CommercialRates
    {
        /// <summary>
        /// ვალუტა
        /// </summary>
        public string? Currency { get; set; }

        /// <summary>
        /// ყიდვის კურსი
        /// </summary>
        public decimal Buy { get; set; }

        /// <summary>
        /// გაყიდვის კურსი
        /// </summary>
        public decimal Sell { get; set; }
    }
}