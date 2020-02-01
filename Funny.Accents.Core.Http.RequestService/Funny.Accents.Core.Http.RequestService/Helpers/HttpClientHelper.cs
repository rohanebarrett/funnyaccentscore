using System;
using System.Net;
using System.Net.Http;
using Funny.Accents.Core.Http.RequestService.HttpRequests;

namespace Funny.Accents.Core.Http.RequestService.Helpers
{
    public static class HttpClientHelper
    {
        public static HttpClient ConstructHttpClient(string proxyUrl,string baseAddress = null,
            HttpRequestHeaderHelper headers = null,TimeSpan? timeout = null)
        {
            var handler = new HttpClientHandler();
            if (string.IsNullOrWhiteSpace(proxyUrl) == false)
            {
                handler.Proxy = new WebProxy(proxyUrl);
                handler.UseProxy = true;
            }

            var client = new HttpClient(handler) {Timeout = timeout ?? TimeSpan.FromMilliseconds(300000)};

            if (string.IsNullOrWhiteSpace(baseAddress) == false)
            {
                client.BaseAddress = new Uri(baseAddress);
            }
            
            if (headers == null) return client;

            foreach (var pair in headers.GetHeaderValues())
            {
                if (pair.Value != null)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(pair.Key, pair.Value);
                }
            }

            return client;
        }/*End of ConstructHttpClient method*/
    }/*End of HttpClientHelper class*/
}/*End of Funny.Accents.Core.Http.RequestService.HttpRequests*/
