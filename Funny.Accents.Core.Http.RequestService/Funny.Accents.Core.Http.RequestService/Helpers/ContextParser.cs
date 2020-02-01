using Funny.Accents.Core.Http.RequestService.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Text;

namespace Funny.Accents.Core.Http.RequestService.Helpers
{
    public class ContextParser
    {
        public static T GetBodyParameters<T>(string valueFromBody,
            SerializationFormat deserializationFormat)
            where T : class, new()
        {
            return SerializationHelper.DeserializeResponseType<T>(valueFromBody,
                deserializationFormat);
        }/*End of GetShippingParameters method*/

        public static string GetRequestValue(ModelBindingContext bindingContext,
            RequestType requestType, string value)
        {
            switch (requestType)
            {
                case RequestType.Header:
                    bindingContext.HttpContext.Request.Headers.TryGetValue(value, out var foundValue);
                    return foundValue;
                case RequestType.QueryRoute:
                    return bindingContext.ValueProvider.GetValue(value).FirstOrDefault();
                default:
                    return string.Empty;
            }
        }/*End of GetRequestValue*/

        public static string GetContent(byte[] contentBuffer)
        {
            var byteArray = contentBuffer.ToArray();
            var jsonContent = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
            return jsonContent;
        }
    }/*End of ContextParser class*/
}/*End of Funny.Accents.Core.Http.RequestService.Helpers namespace*/
