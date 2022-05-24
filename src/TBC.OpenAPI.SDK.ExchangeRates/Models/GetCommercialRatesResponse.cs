namespace TBC.OpenAPI.SDK.ExchangeRates.Models
{

    /// <summary>
    /// კომერციული კურსის დასაბრუნებელი მოდელი
    /// </summary>
    public class GetCommercialRatesResponse
    {
        /// <summary>
        /// საბაზისო ვალუტა
        /// </summary>
        public string? Base { get; set; }

        /// <summary>
        /// ვალუტის კომერციული კურსების სია
        /// </summary>
        public List<CommercialRates>? CommercialRatesList { get; set; }
    }
}