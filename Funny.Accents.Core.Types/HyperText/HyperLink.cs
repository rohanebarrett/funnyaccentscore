using Newtonsoft.Json;

namespace Funny.Accents.Core.Types.HyperText
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HyperLink
    {
        [JsonProperty("rel", Required = Required.Default)]

        public string Rel { get; set; }

        [JsonProperty("href", Required = Required.Default)]

        public string Href { get; set; }

        [JsonProperty("mediaType", Required = Required.Default)]

        public string MediaType { get; set; }

        [JsonProperty("linkIndex", Required = Required.Default)]
        public string LinkIndex { get; set; }
    }/*End of HyperLink*/
}/*End of CmkTypes.HyperText namespace*/
