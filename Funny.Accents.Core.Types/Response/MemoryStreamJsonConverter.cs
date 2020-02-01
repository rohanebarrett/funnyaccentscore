using System;
using System.IO;
using Newtonsoft.Json;

namespace Funny.Accents.Core.Types.Response
{
    public class MemoryStreamJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(MemoryStream).IsAssignableFrom(objectType);
        }/*End of CanConvert method*/

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            var bytes = serializer.Deserialize<byte[]>(reader);
            return bytes != null ? new MemoryStream(bytes) : new MemoryStream();
        }/*End of ReadJson method*/

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bytes = ((MemoryStream)value).ToArray();
            serializer.Serialize(writer, bytes);
        }/*End of WriteJson method*/
    }/*End of MemoryStreamJsonConverter class*/
}/*End of CmkTypes.Response namespace*/
