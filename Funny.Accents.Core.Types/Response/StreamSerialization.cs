using System.IO;
using Newtonsoft.Json;

namespace Funny.Accents.Core.Types.Response
{
    [JsonObject(MemberSerialization.OptIn)]
    public class StreamSerialization
    {
        [JsonProperty("streamValue", Required = Required.Always)]
        [JsonConverter(typeof(MemoryStreamJsonConverter))]
        public Stream StreamProperty { get; set; }
    }/*End of StreamSerialization class*/
}/*End of CmkTypes.Response*/
