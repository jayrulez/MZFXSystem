using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace MarshalZehr.FXSystem.Services.FX
{
    public class FxService
    {
        private readonly IFXServiceBackend _fxServiceBackend;
        private readonly ILogger _logger;
        public string DateFormat { get { return "yyyy-MM-dd"; } }

        public FxService(IFXServiceBackend fxServiceBackend, ILogger<FxService> logger)
        {
            _fxServiceBackend = fxServiceBackend;
            _logger = logger;
        }

        public IReadOnlyList<string> GetSupportedCurrencies()
        {
            return _fxServiceBackend.SupportedCurrencies;
        }

        public bool ValidateDate(string date)
        {
            return DateTime.TryParseExact(date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var _);
        }

        private async Task<ServiceResponse<FXConversion>> ConvertDirect(string sourceCurrencyCode, string targetCurrencyCode, decimal amount, string date)
        {
            var response = new ServiceResponse<FXConversion>();

            var fxRecord = await _fxServiceBackend.GetFXData(sourceCurrencyCode, targetCurrencyCode, date);

            if (fxRecord == null)
                return response.Fail(FXErrorCode.RecordNotFoundForDate, $"No exchange rate was found for source currency '{sourceCurrencyCode}' and target currency '{targetCurrencyCode}' on date '{date}'.");

            var conversion = new FXConversion
            {
                SourceCurrencyCode = fxRecord.SourceCurrencyCode,
                TargetCurrencyCode = fxRecord.TargetCurrencyCode,
                Amount = amount,
                ExchangeRate = fxRecord.ExchangeRate,
                Value = amount * fxRecord.ExchangeRate,
                Date = fxRecord.Date,
                Direct = true
            };

            return response.Succeed(conversion);
        }

        private async Task<ServiceResponse<FXConversion>> ConvertIndirect(string sourceCurrencyCode, string targetCurrencyCode, decimal amount, string date)
        {
            var response = new ServiceResponse<FXConversion>();

            var sourceToBaseRecord = await _fxServiceBackend.GetFXData(sourceCurrencyCode, _fxServiceBackend.BaseCurrencyCode, date);
            if (sourceToBaseRecord == null)
                return response.Fail(FXErrorCode.RecordNotFoundForDate, $"No exchange rate was found for source currency '{sourceCurrencyCode}' and target currency '{_fxServiceBackend.BaseCurrencyCode}' on date '{date}'.");

            var baseToTargetRecord = await _fxServiceBackend.GetFXData(_fxServiceBackend.BaseCurrencyCode, targetCurrencyCode, date);
            if (baseToTargetRecord == null)
                return response.Fail(FXErrorCode.RecordNotFoundForDate, $"No exchange rate was found for source currency '{_fxServiceBackend.BaseCurrencyCode}' and target currency '{targetCurrencyCode}' on date '{date}'.");

            var sourceToBaseValue = amount * sourceToBaseRecord.ExchangeRate;
            var baseToTargetValue = sourceToBaseValue * baseToTargetRecord.ExchangeRate;

            var conversion = new FXConversion
            {
                SourceCurrencyCode = sourceToBaseRecord.SourceCurrencyCode,
                TargetCurrencyCode = baseToTargetRecord.TargetCurrencyCode,
                Amount = amount,
                ExchangeRate = baseToTargetValue / amount,
                Value = baseToTargetValue,
                Date = baseToTargetRecord.Date,
                Direct = false
            };

            return response.Succeed(conversion);
        }

        public async Task<ServiceResponse<FXConversion>> Convert(string sourceCurrencyCode, string targetCurrencyCode, decimal amount, string date = null)
        {
            var response = new ServiceResponse<FXConversion>();

            try
            {
                if (string.IsNullOrWhiteSpace(date))
                    date = DateTime.Now.ToString(DateFormat);
                else
                {
                    // validate the date provided
                    if (!ValidateDate(date))
                    {
                        return response.Fail(FXErrorCode.InvalidDate, $"The date '{date}' is not valid. Specify a valid date in the format '{DateFormat.ToUpper()}'.");
                    }
                }

                if (!_fxServiceBackend.Supports(sourceCurrencyCode))
                    return response.Fail(FXErrorCode.SourceCurrencyNotSupported, $"Source currency '{sourceCurrencyCode}' is not supported.");

                if (!_fxServiceBackend.Supports(targetCurrencyCode))
                    return response.Fail(FXErrorCode.TargetCurrencyNotSupported, $"Target currency '{targetCurrencyCode}' is not supported.");

                if (sourceCurrencyCode.Equals(targetCurrencyCode, StringComparison.OrdinalIgnoreCase))
                    return response.Fail(FXErrorCode.InvalidCurrencyPair, "Source currency and target currency must be different.");

                // if neither source nor target currency is the base currency then an indirect conversion from source to base then base to target will be performed
                if(!(sourceCurrencyCode.Equals(_fxServiceBackend.BaseCurrencyCode, StringComparison.OrdinalIgnoreCase) || targetCurrencyCode.Equals(_fxServiceBackend.BaseCurrencyCode, StringComparison.OrdinalIgnoreCase)))
                {
                    return await ConvertIndirect(sourceCurrencyCode, targetCurrencyCode, amount, date);
                }
                else
                {
                    return await ConvertDirect(sourceCurrencyCode, targetCurrencyCode, amount, date);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while retrieving exchange rate data for currency pair: {ex.Message}");

                return response.Fail(FXErrorCode.UnexpectedError, ex.Message);
            }
        }
    }
}