using Newtonsoft.Json;

namespace MarshalZehr.FXSystem.Services.FX.Backends.BankOfCanada
{
    public class ErrorResponseModel
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("docs")]
        public string Docs { get; set; }
    }
}
