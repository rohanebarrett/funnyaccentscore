using Newtonsoft.Json;

namespace Funny.Accents.Core.Http.RequestService.Error
{
    public interface IApiErrorMessage
    {
        string ErrorCode { get; set; }
        string ErrorMessage { get; set; }
    }/*End of IApiErrorMessage interface*/

    [JsonObject(MemberSerialization.OptIn)]
    public class ApiErrorMessage : IApiErrorMessage
    {
        [JsonProperty("ErrorCode", Required = Required.Default)]
        public string ErrorCode { get; set; }
        [JsonProperty("ErrorMessage", Required = Required.Default)]
        public string ErrorMessage { get; set; }
    }/*End of ApiErrorMessage class*/
}/*End of Funny.Accents.Core.Http.RequestService.Error namespace*/
