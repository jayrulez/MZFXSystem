namespace MarshalZehr.FXSystem.Services.FX
{
    public class FXRecord
    {
        public string SourceCurrencyCode { get; set; }
        public string TargetCurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Date { get; set; }
    }
}
