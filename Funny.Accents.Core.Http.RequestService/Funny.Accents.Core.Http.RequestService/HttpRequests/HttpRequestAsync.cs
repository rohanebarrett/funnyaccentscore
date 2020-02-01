using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Funny.Accents.Core.Http.RequestService.Enums;
using Funny.Accents.Core.Http.RequestService.Interfaces;

namespace Funny.Accents.Core.Http.RequestService.HttpRequests
{
    public static class HttpRequestAsync
    {
        public static async Task<T> Get<T>(
            string apiUrlParam, string proxyUrlParam = null, HttpRequestHeaderHelper headerParam = null
            , Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition = null
            , List<HttpStatusCode> validStatusCodes = null
            , SerializationFormat serializationFormat = SerializationFormat.None
            , TimeSpan? timeout = null
            , Action applyServicePointManager = null)
            where T : class, IHttpResult, new()
        {
            applyServicePointManager?.Invoke();

            /*Configure proxy settings*/
            var handler = string.IsNullOrWhiteSpace(proxyUrlParam)
                ? new HttpClientHandler()
                : new HttpClientHandler()
                {
                    Proxy = new WebProxy(proxyUrlParam),
                    UseProxy = true,
                };

            using (var client = new HttpClient(handler))
            {
                client.Timeout = timeout ?? TimeSpan.FromMilliseconds(300000);
                var url = apiUrlParam ?? "";

                /*Add header values to request*/
                if (headerParam != null)
                {
                    foreach (var pair in headerParam.GetHeaderValues())
                    {
                        if (pair.Value != null)
                        {
                            client.DefaultRequestHeaders.Add(pair.Key, pair.Value);
                        }
                    }
                }
                /*Execute the GET request*/
                var response = await client.GetAsync(url);

                return new T()
                {
                    ResponseValue = response,
                    ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                    ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                    SerializationFormat = serializationFormat
                };
            }
        }/*End of Get method*/

        public static async Task<T> GetWithHttpClient<T>(
            string apiUrlParam
            , Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition = null
            , List<HttpStatusCode> validStatusCodes = null
            , SerializationFormat serializationFormat = SerializationFormat.None
            , HttpClient httpClient = null
            , Action applyServicePointManager = null)
            where T : class, IHttpResult, new()
        {
            applyServicePointManager?.Invoke();

            if (httpClient == null){return default(T);}

            var url = apiUrlParam ?? "";

            /*Execute the GET request*/
            var response = await httpClient.GetAsync(url);

            return new T()
            {
                ResponseValue = response,
                ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                SerializationFormat = serializationFormat
            };
        }/*End of GetWithHttpClient method*/

        public static async Task<T> PostAsync<T>(string apiUrlParam
            , string contentBodyParam, string contentTypeParam, Encoding encodingParam
            , string proxyUrlParam = null, HttpRequestHeaderHelper headerParam = null
            , Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition = null
            , List<HttpStatusCode> validStatusCodes = null
            , SerializationFormat serializationFormat = SerializationFormat.None
            , TimeSpan? timeout = null
            , Action applyServicePointManager = null)
            where T : class, IHttpResult, new()
        {
            applyServicePointManager?.Invoke();

            var handler = new HttpClientHandler();
            if (string.IsNullOrWhiteSpace(proxyUrlParam) == false)
            {
                handler.Proxy = new WebProxy(proxyUrlParam);
                handler.UseProxy = true;
            }

            using (var client = new HttpClient(handler))
            {
                client.Timeout = timeout ?? TimeSpan.FromMilliseconds(300000);
                var url = apiUrlParam ?? "";

                if (headerParam != null)
                {
                    foreach (var pair in headerParam.GetHeaderValues())
                    {
                        if (pair.Value != null)
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(pair.Key, pair.Value);
                        }
                    }
                }

                /*Add content into the request body and post the request*/
                var content = new StringContent(contentBodyParam, encodingParam, contentTypeParam);
                var response = await client.PostAsync(url, content);

                /*Deserialize response to HttpResults object and return to the calling application*/
                return new T
                {
                    ResponseValue = response,
                    ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                    ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                    SerializationFormat = serializationFormat
                };
            }
        }/*End of PostAsync method*/

        public static async Task<T> PostWithHttpClient<T>(string apiUrlParam
            , string contentBodyParam, string contentTypeParam, Encoding encodingParam
            , Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition = null
            , List<HttpStatusCode> validStatusCodes = null
            , SerializationFormat serializationFormat = SerializationFormat.None
            , HttpClient httpClient = null
            , Action applyServicePointManager = null)
            where T : class, IHttpResult, new()
        {
            applyServicePointManager?.Invoke();

            if (httpClient == null) { return default(T); }

            var url = apiUrlParam ?? "";

            /*Add content into the request body and post the request*/
            var content = new StringContent(contentBodyParam, encodingParam, contentTypeParam);
            var response = await httpClient.PostAsync(url, content);

            /*Deserialize response to HttpResults object and return to the calling application*/
            return new T
            {
                ResponseValue = response,
                ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                SerializationFormat = serializationFormat
            };
        }/*End of PostWithHttpClient method*/


        public static async Task<T> Put<T>(string apiUrlParam
            , string contentBodyParam, string contentTypeParam, Encoding encodingParam
            , string proxyUrlParam = null, HttpRequestHeaderHelper headerParam = null
            , Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition = null
            , List<HttpStatusCode> validStatusCodes = null
            , SerializationFormat serializationFormat = SerializationFormat.None
            , TimeSpan? timeout = null
            , Action applyServicePointManager = null)
            where T : class, IHttpResult, new()
        {
            applyServicePointManager?.Invoke();

            var handler = new HttpClientHandler();
            if (string.IsNullOrWhiteSpace(proxyUrlParam) == false)
            {
                handler.Proxy = new WebProxy(proxyUrlParam);
                handler.UseProxy = true;
            }

            using (var client = new HttpClient(handler))
            {
                client.Timeout = timeout ?? TimeSpan.FromMilliseconds(300000);
                var url = apiUrlParam ?? "";

                if (headerParam != null)
                {
                    foreach (var pair in headerParam.GetHeaderValues())
                    {
                        if (pair.Value != null)
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(pair.Key, pair.Value);
                        }
                    }
                }

                /*Add content into the request body and post the request*/
                var content = new StringContent(contentBodyParam, encodingParam, contentTypeParam);
                var response = await client.PutAsync(url, content);

                /*Deserialize response to HttpResults object and return to the calling application*/
                return new T
                {
                    ResponseValue = response,
                    ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                    ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                    SerializationFormat = serializationFormat
                };
            }
        }/*End of PutAsync method*/

        public static async Task<T> PutWithHttpClientAsync<T>(string apiUrlParam
            , string contentBodyParam, string contentTypeParam, Encoding encodingParam
            , Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition = null
            , List<HttpStatusCode> validStatusCodes = null
            , SerializationFormat serializationFormat = SerializationFormat.None
            , HttpClient httpClient = null
            , Action applyServicePointManager = null)
            where T : class, IHttpResult, new()
        {
            applyServicePointManager?.Invoke();

            if (httpClient == null) { return default(T); }

            var url = apiUrlParam ?? "";

            /*Add content into the request body and post the request*/
            var content = new StringContent(contentBodyParam, encodingParam, contentTypeParam);
            var response = await httpClient.PutAsync(url, content);

            /*Deserialize response to HttpResults object and return to the calling application*/
            return new T
            {
                ResponseValue = response,
                ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                SerializationFormat = serializationFormat
            };
        }/*End of PutWithHttpClient method*/

        public static async Task<T> Patch<T>(string apiUrlParam
            , string contentBodyParam, string contentTypeParam, Encoding encodingParam
            , string proxyUrlParam = null, HttpRequestHeaderHelper headerParam = null
            , Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition = null
            , List<HttpStatusCode> validStatusCodes = null
            , SerializationFormat serializationFormat = SerializationFormat.None
            , TimeSpan? timeout = null
            , Action applyServicePointManager = null)
            where T : class, IHttpResult, new()
        {
            applyServicePointManager?.Invoke();

            var handler = new HttpClientHandler();
            if (string.IsNullOrWhiteSpace(proxyUrlParam) == false)
            {
                handler.Proxy = new WebProxy(proxyUrlParam);
                handler.UseProxy = true;
            }

            using (var client = new HttpClient(handler))
            {
                client.Timeout = timeout ?? TimeSpan.FromMilliseconds(300000);
                var url = apiUrlParam ?? "";

                if (headerParam != null)
                {
                    foreach (var pair in headerParam.GetHeaderValues())
                    {
                        if (pair.Value != null)
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(pair.Key, pair.Value);
                        }
                    }
                }

                /*Add content into the request body and post the request*/
                var content = new StringContent(contentBodyParam, encodingParam, contentTypeParam);
                var request = new HttpRequestMessage
                {
                    Method = new HttpMethod(MediaTypes.Patch.ToString().ToUpper()),
                    RequestUri = new Uri(url),
                    Content = content
                };
                var response = await client.SendAsync(request);


                /*Deserialize response to HttpResults object and return to the calling application*/
                return new T
                {
                    ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                    ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                    SerializationFormat = serializationFormat,
                    ResponseValue = response
                };
            }
        }/*End of PostAsync method*/

        public static async Task<T> PatchWithHttpClient<T>(string apiUrlParam
            , string contentBodyParam, string contentTypeParam, Encoding encodingParam
            , Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition = null
            , List<HttpStatusCode> validStatusCodes = null
            , SerializationFormat serializationFormat = SerializationFormat.None
            , HttpClient httpClient = null
            , Action applyServicePointManager = null)
            where T : class, IHttpResult, new()
        {
            applyServicePointManager?.Invoke();

            if (httpClient == null) { return default(T); }

            var url = apiUrlParam ?? "";

            /*Add content into the request body and post the request*/
            var content = new StringContent(contentBodyParam, encodingParam, contentTypeParam);
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod(MediaTypes.Patch.ToString().ToUpper()),
                RequestUri = new Uri(url),
                Content = content
            };
            var response = await httpClient.SendAsync(request);

            /*Deserialize response to HttpResults object and return to the calling application*/
            return new T
            {
                ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                SerializationFormat = serializationFormat,
                ResponseValue = response
            };
        }/*End of PostAsync method*/

        public static async Task<T> Delete<T>(
            string apiUrlParam, string proxyUrlParam = null, HttpRequestHeaderHelper headerParam = null
            , Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition = null
            , List<HttpStatusCode> validStatusCodes = null
            , SerializationFormat serializationFormat = SerializationFormat.None
            , TimeSpan? timeout = null
            , Action applyServicePointManager = null)
            where T : class, IHttpResult, new()
        {
            applyServicePointManager?.Invoke();

            /*Configure proxy settings*/
            var handler = string.IsNullOrWhiteSpace(proxyUrlParam)
                ? new HttpClientHandler()
                : new HttpClientHandler
                {
                    Proxy = new WebProxy(proxyUrlParam),
                    UseProxy = true,
                };

            using (var client = new HttpClient(handler))
            {
                client.Timeout = timeout ?? TimeSpan.FromMilliseconds(300000);
                var url = apiUrlParam ?? "";

                /*Add header values to request*/
                if (headerParam != null)
                {
                    foreach (var pair in headerParam.GetHeaderValues())
                    {
                        if (pair.Value != null)
                        {
                            client.DefaultRequestHeaders.Add(pair.Key, pair.Value);
                        }
                    }
                }
                /*Execute the GET request*/
                var response = await client.DeleteAsync(url);

                return new T
                {
                    ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                    ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                    SerializationFormat = serializationFormat,
                    ResponseValue = response
                };
            }
        }/*End of Delete method*/

        public static async Task<T> DeleteWithHttpClient<T>(string apiUrlParam
            , Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition = null
            , List<HttpStatusCode> validStatusCodes = null
            , SerializationFormat serializationFormat = SerializationFormat.None
            , HttpClient httpClient = null
            , Action applyServicePointManager = null)
            where T : class, IHttpResult, new()
        {
            applyServicePointManager?.Invoke();

            if (httpClient == null) { return default(T); }

            var url = apiUrlParam ?? "";

            /*Execute the GET request*/
            var response = await httpClient.DeleteAsync(url);

            return new T
            {
                ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                SerializationFormat = serializationFormat,
                ResponseValue = response
            };
        }/*End of DeleteWithHttpClient method*/
    }/*End of AsyncHttpRequest class*/
}/*End of Funny.Accents.Core.Http.RequestService.HttpRequests namespace*/
