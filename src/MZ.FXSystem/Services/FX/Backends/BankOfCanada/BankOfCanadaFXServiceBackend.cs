using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MZ.FXSystem.Services.FX.Backends.BankOfCanada
{
    /// <summary>
    /// Bank of Canada FX Backend implementation
    /// </summary>
    public class BankOfCanadaFXServiceBackend : IFXServiceBackend
    {
        private readonly HttpClient _httpClient;

        public string BaseCurrencyCode { get { return "CAD"; } }

        public IReadOnlyList<string> SupportedCurrencies
        {
            get
            {
                return new List<string>
                {
                    "CAD",
                    "USD",
                    "EUR",
                    "JPY",
                    "GBP",
                    "AUD",
                    "CHF",
                    "CNY",
                    "HKD",
                    "MXN",
                    "INR"
                };
            }
        }

        public BankOfCanadaFXServiceBackend()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://www.bankofcanada.ca/valet/")
            };

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public bool Supports(string currencyCode)
        {
            return SupportedCurrencies.Contains(currencyCode.ToUpper());
        }

        public async Task<FXRecord> GetFXData(string sourceCurrencyCode, string targetCurrencyCode, string date)
        {
            // we could cache this so that we don't need to hit the Bank of Canada API for each call
            var records = await GetFXData(sourceCurrencyCode, targetCurrencyCode);

            return records.FirstOrDefault(r => r.Date.Equals(date));
        }

        private async Task<List<FXRecord>> GetFXData(string sourceCurrencyCode, string targetCurrencyCode)
        {
            string fxPair = $"FX{sourceCurrencyCode.ToUpper()}{targetCurrencyCode.ToUpper()}";

            var response = await _httpClient.GetAsync($"observations/{fxPair}/json");

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var seriesData = JsonConvert.DeserializeObject<SeriesDataModel>(content);

                // The source data does not map neatly to a convenient .NET object 
                // So let us map them to a list of [FXRecord]s
                var fxRecords = new List<FXRecord>();

                foreach (var obversation in seriesData.Observations)
                {
                    var fxRecord = new FXRecord()
                    {
                        SourceCurrencyCode = sourceCurrencyCode.ToUpper(),
                        TargetCurrencyCode = targetCurrencyCode.ToUpper()
                    };

                    // get the date and exchange rate for the record
                    if (obversation.ContainsKey("d") && obversation.ContainsKey(fxPair))
                    {
                        fxRecord.Date = obversation["d"] as string;

                        if (string.IsNullOrWhiteSpace(fxRecord.Date))
                        {
                            // This shouldn't happen unless there is something wrong with the record.
                            // If there is something wrong then we will just skip it
                            continue;
                        }

                        var fxPairRateObject = obversation[fxPair] as JObject;

                        if (fxPairRateObject == null)
                        {
                            // This shouldn't happen unless there is something wrong with the record.
                            // If there is something wrong then we will just skip it
                            continue;
                        }

                        fxRecord.ExchangeRate = fxPairRateObject.Value<decimal>("v");
                    }
                    else
                    {
                        // This observation does not look like something we expect
                        // Let us just skip it.
                        // This should not happen unless the source data format changes
                        continue;
                    }

                    fxRecords.Add(fxRecord);
                }

                return fxRecords;
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorData = JsonConvert.DeserializeObject<ErrorResponseModel>(content);

                    throw new FXServiceException(errorData.Message);
                }

                throw new FXServiceException($"An unexpected error has occured while retrieving data for '{fxPair}'.");
            }
        }
    }
}
