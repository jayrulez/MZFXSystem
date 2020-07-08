using Newtonsoft.Json;

namespace MZ.FXSystem.Services.FX.Backends.BankOfCanada
{
    /// <summary>
    /// Mapping for error responses from the Bank of Canada Valet API
    /// </summary>
    public class ErrorResponseModel
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("docs")]
        public string Docs { get; set; }
    }
}
