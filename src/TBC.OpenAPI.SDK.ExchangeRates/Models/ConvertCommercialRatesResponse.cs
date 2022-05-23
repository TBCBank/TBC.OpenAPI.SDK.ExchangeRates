namespace TBC.OpenAPI.SDK.ExchangeRates.Models
{
    /// <summary>
    /// კომერციული კურსის კონვერტაციის დასაბრუნებელი მოდელი
    /// </summary>
    public class ConvertCommercialRatesResponse
    {
        /// <summary>
        /// ვალუტა, რომლიდანაც კონვერტირდება
        /// </summary>
        public string? From { get; set; }

        /// <summary>
        /// ვალუტა, რომელშიც კონვერტირდება
        /// </summary>
        public string? To { get; set; }

        /// <summary>
        /// დასაკონვერტირებელი თანხა
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// კონვერტირებული თანხის რაოდენობა
        /// </summary>
        public decimal Value { get; set; }
    }
}