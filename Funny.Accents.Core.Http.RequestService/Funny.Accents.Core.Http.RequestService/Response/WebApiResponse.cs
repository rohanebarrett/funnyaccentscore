using Newtonsoft.Json;

namespace Funny.Accents.Core.Http.RequestService.Response
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WebApiResponse : IWebApiResponse
    {
        [JsonProperty("status-code", Required = Required.Always)]
        public string StatusCode { get; set; }
        [JsonProperty("response-description", Required = Required.Always)]
        public string ResponseDescription { get; set; }
    }/*End of WebApiResponse class*/

    [JsonObject(MemberSerialization.OptIn)]
    public class WebApiResponse<T> : IWebApiResponse<T>
    {
        [JsonProperty("status-code", Required = Required.Always)]
        public string StatusCode { get; set; }
        [JsonProperty("response-description", Required = Required.Always)]
        public string ResponseDescription { get; set; }
        [JsonProperty("response-payload", Required = Required.Always)]
        public T Response { get; set; }
    }/*End of WebApiResponse class*/

    [JsonObject(MemberSerialization.OptIn)]
    public class WebApiResponse<T, TK> : IWebApiResponse<T, TK>
    {
        [JsonProperty("status-code", Required = Required.Always)]
        public string StatusCode { get; set; }

        [JsonProperty("response-description", Required = Required.Always)]
        public string ResponseDescription { get; set; }

        [JsonProperty("response-payload", Required = Required.Default)]
        public T Response { get; set; }

        [JsonProperty("response-message", Required = Required.Default)]
        public TK ResponseMessge { get; set; }
    }/*End of WebApiResponse class*/
}/*End of Funny.Accents.Core.Http.RequestService.Response namespace*/
