namespace TBC.OpenAPI.SDK.ExchangeRates.Models
{
    /// <summary>
    /// ოფიციალური კურსის მოდელი
    /// </summary>
    public class OfficialRate
    {
        /// <summary>
        /// ვალუტა
        /// </summary>
        public string? Currency { get; set; }

        /// <summary>
        /// კურსი
        /// </summary>
        public decimal Value { get; set; }
    }
}