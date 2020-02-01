using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Funny.Accents.Core.Http.RequestService.Interfaces;
using Funny.Accents.Core.Http.RequestService.Enums;
using Funny.Accents.Core.Http.RequestService.Helpers;

namespace Funny.Accents.Core.Http.RequestService.HttpResults
{
    public class HttpResultStreamAsync<T, K> : IHttpResultAsync<T, K>
        where T : Stream
        where K : class,new()
    {
        public Func<HttpStatusCode, List<HttpStatusCode>, bool> ResponseCondition { get; set; }
            = ((p, s) => s.Contains(p));

        public List<HttpStatusCode> ValidStatusCodes { get; set; } = new List<HttpStatusCode>() {HttpStatusCode.OK};
        public SerializationFormat SerializationFormat { get; set; } = SerializationFormat.None;

        public HttpStatusCode StatusCode { get; set; }
        public List<Exception> ResponseErrorList { get; set; }

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

        public HttpResultStreamAsync(){}

        public HttpResultStreamAsync(HttpResponseMessage response)
        {
            AnlayzeResponse(response);
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
                return (T)(await ResponseValue.Content.ReadAsStreamAsync());
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        private async Task<K> AnalyseMessage()
        {
            if (ResponseValue == null)
            {
                return default(K);
            }

            if (ResponseCondition(ResponseValue.StatusCode, ValidStatusCodes)) { return default(K); }

            try
            {
                var jsonContent = (await ResponseValue.Content.ReadAsStringAsync());
                return SerializationHelper.DeserializeResponseType<K>(jsonContent, SerializationFormat);
            }
            catch (Exception ex)
            {
                ResponseErrorList.Add(ex);
                return default(K);
            }
        }

        private void AnlayzeResponse(HttpResponseMessage response)
        {
            if (response == null)
            {
                return;
            }

            StatusCode = response.StatusCode;
        }/*End of AnlayzeResponse method*/
    }/*End of HttpResult<T,K> class*/

    public class HttpResultStreamAsync<T> : IHttpResultAsync<T>
        where T : Stream
    {
        public Func<HttpStatusCode, List<HttpStatusCode>, bool> ResponseCondition { get; set; }
        = ((p, s) => s.Contains(p));

        public List<HttpStatusCode> ValidStatusCodes { get; set; } = new List<HttpStatusCode>() {HttpStatusCode.OK};
        public SerializationFormat SerializationFormat { get; set; } = SerializationFormat.None;

        public HttpStatusCode StatusCode { get; set; }
        public List<Exception> ResponseErrorList { get; set; }

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

        public HttpResultStreamAsync(){}

        public HttpResultStreamAsync(HttpResponseMessage response)
        {
            AnlayzeResponse(response);
        } /*End of HttpResult constructor*/

        public async Task<T> GetResponseType()
        {
            return await AnalyseType();
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
                return (T)(await ResponseValue.Content.ReadAsStreamAsync());
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
        } /*End of AnlayzeResponse method*/
    }/*End of HttpResultStream<T> class*/
}/*End of Funny.Accents.Core.Http.RequestService.HttpResults namespace*/
