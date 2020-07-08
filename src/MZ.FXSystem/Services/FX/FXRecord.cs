namespace MZ.FXSystem.Services.FX
{
    /// <summary>
    /// Represents the exchange rate for a currency pair on a particular date
    /// </summary>
    public class FXRecord
    {
        /// <summary>
        /// The source currency code
        /// </summary>
        public string SourceCurrencyCode { get; set; }

        /// <summary>
        /// The target currency code
        /// </summary>
        public string TargetCurrencyCode { get; set; }

        /// <summary>
        /// The exchange rate
        /// </summary>
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// The date represented by the record
        /// </summary>
        public string Date { get; set; }
    }
}
