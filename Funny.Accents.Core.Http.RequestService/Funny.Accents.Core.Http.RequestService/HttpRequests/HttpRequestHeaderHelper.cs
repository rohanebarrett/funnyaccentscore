using System;
using System.Collections.Generic;
using System.Linq;
using Funny.Accents.Core.Http.RequestService.Attributes;

namespace Funny.Accents.Core.Http.RequestService.HttpRequests
{
    public class HttpRequestHeaderHelper
    {
        [CustomName("Accept")]
        public string Accept { get; set; }
        [CustomName("Accept-Charset")]
        public string AcceptCharset { get; set; }
        [CustomName("Accept-Language")]
        public string AcceptLanguage { get; set; }
        [CustomName("Authorization")]
        public string Authorization { get; set; }
        [CustomName("Host")]
        public string Host { get; set; }
        public string ContentType { get; set; }
        public IDictionary<string, string> CustomHeaders { get; set; } = new Dictionary<string, string>();

        public HttpRequestHeaderHelper()
        {
        }

        public IDictionary<string, string> GetHeaderValues()
        {

            var tempHeaders = GetAttributeAndValue<HttpRequestHeaderHelper, CustomNameAttribute>(this);
            var combined =
                CustomHeaders.Union(tempHeaders)
                .GroupBy(kvp => kvp.Key)
                .Select(grp => grp.First())
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return CustomHeaders = combined;
        }

        private static string GetValueByPropetyName<T>(T data, string propertyName)
            where T : class
        {
            var t = data.GetType();
            foreach (var pi in t.GetProperties())
            {
                if (pi.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    return pi.GetValue(data, null)?.ToString();
                }
            }
            return null;
        }

        private static Dictionary<string, string> GetAttributeAndValue<T,K>(T data)
            where T : class
            where K : CustomNameAttribute
        {
            var dict = new Dictionary<string, string>();

            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    var authAttr = attr as K;
                    if (authAttr == null)
                    {
                        continue;
                    }

                    var propName = prop.Name;
                    var auth = authAttr.MemberName;
                    var value = GetValueByPropetyName(data, propName);

                    dict.Add(auth, value);
                }
            }

            return dict;
        }

    }/*End of HttpRequestHeaderHelper*/
}/*End of Funny.Accents.Core.Http.RequestService.HttpRequests namespace*/
