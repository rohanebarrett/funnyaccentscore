using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Funny.Accents.Core.Http.RequestService.Interfaces;
using Funny.Accents.Core.Http.RequestService.Helpers;
using SerializationFormat = Funny.Accents.Core.Http.RequestService.Enums.SerializationFormat;

namespace Funny.Accents.Core.Http.RequestService.HttpResults
{
    public class HttpResultAsync : IHttpResult
    {
        public HttpResponseMessage ResponseValue { get; set; }
        public Func<HttpStatusCode, List<HttpStatusCode>, bool> ResponseCondition { get; set; }
            = ((p, s) => s.Contains(p));

        public List<HttpStatusCode> ValidStatusCodes { get; set; }
            = new List<HttpStatusCode>() { HttpStatusCode.OK };

        public HttpStatusCode StatusCode { get; set; }
        public List<Exception> ResponseErrorList { get; set; }

        public SerializationFormat SerializationFormat { get; set; } = SerializationFormat.None;
    }

    public class HttpResultAsync<T, K> : IHttpResultAsync<T, K>
        where T : class, new()
        where K : class, new()
    {
        private HttpResponseMessage _responseValue;
        public HttpResponseMessage ResponseValue
        {
            get => _responseValue;
            set
            {
                _responseValue = value;
                AnlayzeResponse(value);
            }
        }

        public Func<HttpStatusCode, List<HttpStatusCode>, bool> ResponseCondition { get; set; }
        = ((p, s) => s.Contains(p));

        public List<HttpStatusCode> ValidStatusCodes { get; set; } 
            = new List<HttpStatusCode>() {HttpStatusCode.OK};

        public HttpStatusCode StatusCode { get; set; }

        public List<Exception> ResponseErrorList { get; set; } = new List<Exception>();
    
        public SerializationFormat SerializationFormat { get; set; } = SerializationFormat.None;

        public HttpResultAsync(){}

        public HttpResultAsync(HttpResponseMessage response, Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition
            , List<HttpStatusCode> validStatusCodes, SerializationFormat serializationFormat)
        {
            ResponseCondition = responseCondition;
            ValidStatusCodes = validStatusCodes;
            SerializationFormat = serializationFormat;
            ResponseValue = response;
        } /*End of HttpResult constructor*/

        public async Task<T> GetResponseType()
        {
            return await AnalyseType();
        }

        public async Task<K> GetResponseMessage()
        {
            return await AnalyseMessage();
        }

        private async Task<T> AnalyseType()
        {
            if (ResponseValue == null)
            {
                return default(T);
            }

            if (!ResponseCondition(ResponseValue.StatusCode, ValidStatusCodes)) { return default(T); }

            try
            {
                var buffer = await ResponseValue.Content.ReadAsByteArrayAsync();
                var byteArray = buffer.ToArray();
                var jsonContent = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

                return SerializationHelper.DeserializeResponseType<T>(jsonContent, SerializationFormat);
            }
            catch (Exception ex)
            {
                ResponseErrorList.Add(ex);
                return default(T);
            }
        }/*End of AnalyseType method*/

        private async Task<K> AnalyseMessage()
        {
            if (ResponseValue == null)
            {
                return default(K);
            }

            if (ResponseCondition(ResponseValue.StatusCode, ValidStatusCodes)) { return default(K); }

            try
            {
                var buffer = await ResponseValue.Content.ReadAsByteArrayAsync();
                var byteArray = buffer.ToArray();
                var jsonContent = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

                return SerializationHelper.DeserializeResponseType<K>(jsonContent, SerializationFormat);
            }
            catch (Exception ex)
            {
                ResponseErrorList.Add(ex);
                return default(K);
            }
        }/*End of AnalyseMessage method*/

        private void AnlayzeResponse(HttpResponseMessage response)
        {
            if (response == null)
            {
                return;
            }

            StatusCode = response.StatusCode;
        }
    } /*End of HttpResult class*/

    public class HttpResultAsync<T> : IHttpResultAsync<T>
        where T : class,new()
    {
        private HttpResponseMessage _responseValue;
        public HttpResponseMessage ResponseValue
        {
            get => _responseValue;
            set
            {
                _responseValue = value;
                AnlayzeResponse(value);
            }
        }

        public HttpStatusCode StatusCode { get; set; }
        public List<Exception> ResponseErrorList { get; set; }

        public Func<HttpStatusCode, List<HttpStatusCode>, bool> ResponseCondition { get; set; }
            = ((p, s) => s.Contains(p));

        public List<HttpStatusCode> ValidStatusCodes { get; set; }
            = new List<HttpStatusCode>() { HttpStatusCode.OK };

        public async Task<T> GetResponseType()
        {
            return await AnalyseType();
        }

        public SerializationFormat SerializationFormat { get; set; } = SerializationFormat.None;

        public HttpResultAsync(){}

        public HttpResultAsync(HttpResponseMessage response, Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition
            , List<HttpStatusCode> validStatusCodes, SerializationFormat serializationFormat)
        {
            ResponseCondition = responseCondition;
            ValidStatusCodes = validStatusCodes;
            SerializationFormat = serializationFormat;
            ResponseValue = response;
        } /*End of HttpResult constructor*/

        private async Task<T> AnalyseType()
        {
            if (ResponseValue == null)
            {
                return default(T);
            }

            if (!ResponseCondition(ResponseValue.StatusCode, ValidStatusCodes)) { return default(T); }

            try
            {
                var buffer = await ResponseValue.Content.ReadAsByteArrayAsync();
                var byteArray = buffer.ToArray();
                var jsonContent = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                return SerializationHelper.DeserializeResponseType<T>(jsonContent, SerializationFormat);
            }
            catch (Exception ex)
            {
                ResponseErrorList.Add(ex);
                return default(T);
            }
        }

        private void AnlayzeResponse(HttpResponseMessage response)
        {
            if (response == null)
            {
                return;
            }

            StatusCode = response.StatusCode;
        }
    }/*End of ttpResult<T> class*/
}/*End of Funny.Accents.Core.Http.RequestService.HttpResults namespace*/
