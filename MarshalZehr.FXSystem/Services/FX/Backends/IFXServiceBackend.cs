using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarshalZehr.FXSystem.Services.FX
{
    public interface IFXServiceBackend
    {
        string BaseCurrencyCode { get; }
        IReadOnlyList<string> SupportedCurrencies { get; }
        bool Supports(string currencyCode);
        Task<FXRecord> GetFXData(string sourceCurrencyCode, string targetCurrencyCode, string date);
    }
}