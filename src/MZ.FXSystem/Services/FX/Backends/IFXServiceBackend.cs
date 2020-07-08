using System.Collections.Generic;
using System.Threading.Tasks;

namespace MZ.FXSystem.Services.FX
{
    /// <summary>
    /// Interface for a FX Service backend
    /// </summary>
    public interface IFXServiceBackend
    {
        /// <summary>
        /// The base currency code of the backend
        /// </summary>
        string BaseCurrencyCode { get; }

        /// <summary>
        /// A list of currencies supported by the backend
        /// </summary>
        IReadOnlyList<string> SupportedCurrencies { get; }

        /// <summary>
        /// Checks if the backend supports the given currency
        /// </summary>
        /// <param name="currencyCode">The currency code</param>
        /// <returns>Returns True if the currency is supported, false otherwise.</returns>
        bool Supports(string currencyCode);

        /// <summary>
        /// Gets a record representing the exchange rate when converting from source to target currency
        /// </summary>
        /// <param name="sourceCurrencyCode">The source currency code</param>
        /// <param name="targetCurrencyCode">The target currency code</param>
        /// <param name="date">The date the record is being requested for</param>
        /// <returns>A <see cref="FXRecord"/> for the currency pair on the specified date.</returns>
        Task<FXRecord> GetFXData(string sourceCurrencyCode, string targetCurrencyCode, string date);
    }
}