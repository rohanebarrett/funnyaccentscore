using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Funny.Accents.Core.Http.RequestService.Enums;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Funny.Accents.Core.Http.RequestService.Helpers
{
    public static class SerializationHelper
    {
        private const string XmlFormat = "version=\"1.0\" encoding=\"UTF-8\"";
        private const string FormatXml = "xml";

        public static StringBuilder SerializeXml<T>(T value)
            where T : class, new()
        {
            var sbValue = new StringBuilder();
            var xmlWriter = XmlWriter.Create(sbValue);
            xmlWriter.WriteProcessingInstruction(FormatXml, XmlFormat);
            var serializerRequest = new XmlSerializer(typeof(T));
            serializerRequest.Serialize(xmlWriter, value);

            return sbValue;
        }

        public static string SerializeNewtonJson<T>(T value,
            Formatting formating = Formatting.Indented,
            JsonSerializerSettings jsonSerializerSettings = null)
            where T : class, new()
        {
            var output = JsonConvert.SerializeObject(value
                , formating
                , jsonSerializerSettings 
                  ?? new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return output;
        }/*End of SerializeNewtonJson method*/

        public static T DeserializeXml<T>(Stream value) 
            where T : class,new()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                TextReader reader = new StreamReader(value);
                return (T) serializer.Deserialize(reader);
            }
            catch (Exception)
            {
                return default;
            }
        } /*End of DeserializeXML method*/

        public static T DeserializeXml<T>(string value)
            where T : new()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                TextReader reader = new StringReader(value);
                return (T)serializer.Deserialize(reader);
            }
            catch (Exception)
            {
                return default;
            }
        } /*End of DeserializeXML method*/

        public static T DeserializeJsonJavaScript<T>(string value)
            where T : class, new()
        {
            try
            {
                //var serializer = new JavaScriptSerializer();
                //return serializer.Deserialize<T>(value);
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception)
            {
                return default;
            }
        } /*End of SerializeJsonJavaScript method*/

        public static T DeserializeJsonDataContract<T>(string value)
            where T : class, new()
        {
            var ser = new DataContractJsonSerializer(typeof(T));

            try
            {
                using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(value)))
                {
                    return (T)ser.ReadObject(stream);
                }
            }
            catch (Exception)
            {
                return default;
            }
        }/*End of SerializeJsonJavaScript method*/

        public static T DeserializeNewtonJson<T>(string value)
            where T : class, new()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                return default;
            }
        } /*End of SerializeJsonJavaScript method*/

        public static T DeserializeResponseType<T>(string value, SerializationFormat deserializationFormat)
            where T : class, new()
        {
            if (deserializationFormat.Equals(SerializationFormat.JsonNewton))
            {
                return DeserializeNewtonJson<T>(value);
                //return JsonConvert.DeserializeObject<T>(value);
            }
            else if (deserializationFormat.Equals(SerializationFormat.JsonJavaScriptSerializer))
            {
                return DeserializeJsonJavaScript<T>(value);
            }
            else if (deserializationFormat.Equals(SerializationFormat.JsonDataContract))
            {
                return DeserializeJsonDataContract<T>(value);
            }
            else if (deserializationFormat.Equals(SerializationFormat.Xml))
            {
                return DeserializeXml<T>(value);
            }
            else
            {
                return default;
            }
        }/*End of DeserializeResponseType method*/
    }/*End of SerializationHelper class*/
}/*End of Funny.Accents.Core.Http.RequestService.Helpers namespace*/
