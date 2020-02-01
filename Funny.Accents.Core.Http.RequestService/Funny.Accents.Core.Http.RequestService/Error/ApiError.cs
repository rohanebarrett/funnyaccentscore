using System.Net.Http;
using Newtonsoft.Json;

namespace Funny.Accents.Core.Http.RequestService.Error
{
    public interface IApiError
    {
        string ApiVersion { get; set; }
        string ErrorType { get; set; }
        IApiErrorMessage[] ErrorMessages { get; set; }
        HttpResponseMessage ResponseMessage { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ApiError : IApiError
    {
        [JsonProperty("apiVersion", Required = Required.Default)]
        public string ApiVersion { get; set; }

        [JsonProperty("errorType", Required = Required.Default)]
        public string ErrorType { get; set; }

        [JsonProperty("errorMessages", Required = Required.Default)]
        public IApiErrorMessage[] ErrorMessages { get; set; }

        [JsonProperty("responseMessage", Required = Required.Default)]
        public HttpResponseMessage ResponseMessage { get; set; }
    }/*End of ApiError class*/
}/*End of Funny.Accents.Core.Http.RequestService.Error*/
