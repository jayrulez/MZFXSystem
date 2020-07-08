using Newtonsoft.Json;
using System.Collections.Generic;

namespace MarshalZehr.FXSystem.Services.FX.Backends.BankOfCanada
{
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
