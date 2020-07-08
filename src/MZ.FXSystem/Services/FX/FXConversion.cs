using System;

namespace MZ.FXSystem.Services.FX
{
    /// <summary>
    /// Represents the result of a conversion between two currencies
    /// </summary>
    public class FXConversion
    {
        public string SourceCurrencyCode { get; set; }
        public string TargetCurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal Value { get; set; }
        public string Date { get; set; }

        public bool Direct { get; set; }

        public override string ToString()
        {
            return $"{SourceCurrencyCode} {Amount}={TargetCurrencyCode} {Math.Round(Value, 4)} using Exchange Rate 1 {SourceCurrencyCode}={ExchangeRate} {TargetCurrencyCode} on date '{Date}'.";
        }
    }
}
