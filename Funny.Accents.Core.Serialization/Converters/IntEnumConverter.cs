using System;
using Newtonsoft.Json;

namespace Funny.Accents.Core.Serialization.Converters
{
    public class IntEnumConverter : JsonConverter
    {
        public override void WriteJson(
            JsonWriter writer, object value, JsonSerializer serializer)
        {
            var objType = value.GetType();
            if (objType.IsEnum)
            {
                writer.WriteValue((int)Convert.ChangeType(value, objType));
            }
            else
            {
                throw new NotSupportedException($"Values: {value} is not an enum type");
            }
        }

        public override object ReadJson(
            JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {

            var objValue = reader.Value;
            if (objValue.GetType().IsEnum)
            {
                return (int)Convert.ChangeType(objValue, objValue.GetType());
            }

            throw new NotSupportedException($"Values: {objValue} is not an enum type");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }
    }/*End of IntEnumConverter class*/
}/*End of EcommerceInventoryUpload.Helpers*/
