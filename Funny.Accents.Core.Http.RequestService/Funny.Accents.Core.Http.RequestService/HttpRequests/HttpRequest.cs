using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Funny.Accents.Core.Http.RequestService.Enums;
using Funny.Accents.Core.Http.RequestService.Interfaces;

namespace Funny.Accents.Core.Http.RequestService.HttpRequests
{
    public static class HttpRequest
    {
        public static T Get<T>(
            string apiUrlParam, string proxyUrlParam = null,
            string contentBodyParam = null,
            string contentTypeParam = null,
            Encoding encodingParam = null,
            HttpRequestHeaderHelper headerParam = null
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

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                };

                if (contentBodyParam != null)
                {
                    request.Content = new StringContent(contentBodyParam,
                        encodingParam, contentTypeParam);
                }

                var response = client.SendAsync(request).Result;

                return new T()
                {
                    ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                    ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                    SerializationFormat = serializationFormat,
                    ResponseValue = response
                };
            }
        }/*End of GetRequest method*/

        public static T GetWithHttpClient<T>(
            string apiUrlParam
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
            var response = httpClient.GetAsync(url).Result;

            return new T()
            {
                ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                SerializationFormat = serializationFormat,
                ResponseValue = response
            };
        }/*End of GetRequest method*/

        public static T Post<T>(string apiUrlParam
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
                var response = client.PostAsync(url, content).Result;

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

        public static T PostWithHttpClient<T>(string apiUrlParam
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
            var response = httpClient.PostAsync(url, content).Result;

            /*Deserialize response to HttpResults object and return to the calling application*/
            return new T
            {
                ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                SerializationFormat = serializationFormat,
                ResponseValue = response
            };
        }/*End of PostAsync method*/

        public static IHttpResult<TResponseType, TKResponseMessage> GetRequest<TIHttpResultImplementation, TResponseType, TKResponseMessage>(
            string apiUrlParam, string proxyUrlParam = null, HttpRequestHeaderHelper headerParam = null
            , Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition = null
            , List<HttpStatusCode> validStatusCodes = null
            , Action applyServicePointManager = null)
            where TIHttpResultImplementation : IHttpResult<TResponseType, TKResponseMessage>, new()
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
                var response = client.GetAsync(url).Result;

                return new TIHttpResultImplementation()
                {
                    ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                    ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK},
                    ResponseValue = response
                };
            }
        }/*End of GetRequest method*/

        public static T Put<T>(string apiUrlParam
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
                var response = client.PutAsync(url, content).Result;

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

        public static T PutWithHttpClientAsync<T>(string apiUrlParam
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
            var response = httpClient.PutAsync(url, content).Result;

            /*Deserialize response to HttpResults object and return to the calling application*/
            return new T
            {
                ResponseValue = response,
                ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                SerializationFormat = serializationFormat
            };
        }/*End of PutWithHttpClient method*/

        public static T Delete<T>(
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
                : new HttpClientHandler{
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
                var response = client.DeleteAsync(url).Result;

                return new T
                {
                    ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                    ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                    SerializationFormat = serializationFormat,
                    ResponseValue = response
                };
            }
        }/*End of Delete method*/

        public static T DeleteWithHttpClient<T>(string apiUrlParam
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
            var response = httpClient.DeleteAsync(url).Result;

            return new T
            {
                ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                SerializationFormat = serializationFormat,
                ResponseValue = response
            };
        }/*End of DeleteWithHttpClient method*/

        public static T Patch<T>(string apiUrlParam
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
                var response = client.SendAsync(request).Result;


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

        public static T PatchWithHttpClient<T>(string apiUrlParam
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
            var response = httpClient.SendAsync(request).Result;

            /*Deserialize response to HttpResults object and return to the calling application*/
            return new T
            {
                ResponseCondition = responseCondition ?? ((p, s) => s.Contains(p)),
                ValidStatusCodes = validStatusCodes ?? new List<HttpStatusCode>() { HttpStatusCode.OK },
                SerializationFormat = serializationFormat,
                ResponseValue = response
            };
        }/*End of PostAsync method*/

    }/*End of Request class*/
}/*End of Funny.Accents.Core.Http.RequestService.HttpRequests namespace*/
