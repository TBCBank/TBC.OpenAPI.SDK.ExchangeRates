namespace TBC.OpenAPI.SDK.ExchangeRates.Models
{
    /// <summary>
    /// TBC Bank's commercial exchange rates response model
    /// </summary>
    public class GetCommercialRatesResponse
    {
        /// <summary>
        /// Base currency (GEL)
        /// </summary>
        public string Base { get; set; }

        /// <summary>
        /// List of currency exchange rate pairs
        /// </summary>
        public List<CommercialRate> CommercialRatesList { get; set; }
    }
}