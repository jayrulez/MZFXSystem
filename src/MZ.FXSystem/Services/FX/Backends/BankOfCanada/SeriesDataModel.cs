using Newtonsoft.Json;
using System.Collections.Generic;

namespace MZ.FXSystem.Services.FX.Backends.BankOfCanada
{
    /// <summary>
    /// Model representing the source data
    /// </summary>
    public class SeriesDataModel
    {
        public SeriesDataModel()
        {
            Observations = new List<Dictionary<string, object>>();
        }

        [JsonProperty("observations")]
        public List<Dictionary<string, object>> Observations { get; set; }
    }
}
