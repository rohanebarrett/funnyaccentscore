using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using Funny.Accents.Core.Http.RequestService.Interfaces;
using Funny.Accents.Core.Http.RequestService.Enums;
using Funny.Accents.Core.Http.RequestService.Helpers;

namespace Funny.Accents.Core.Http.RequestService.HttpResults
{
    public class HttpResultStream<T, K> : IHttpResult<T, K>
        where T : Stream
        where K : class,new()
    {
        public Func<HttpStatusCode, List<HttpStatusCode>, bool> ResponseCondition { get; set; }
            = ((p, s) => s.Contains(p));

        public List<HttpStatusCode> ValidStatusCodes { get; set; } = new List<HttpStatusCode>() {HttpStatusCode.OK};
        public SerializationFormat SerializationFormat { get; set; } = SerializationFormat.None;

        public HttpStatusCode StatusCode { get; set; }
        public List<Exception> ResponseErrorList { get; set; }

        private T _responseType;
        public T ResponseType
        {
            get
            {
                return AnalyseType();
            }
            private set { _responseType = value; }
        }

        private K _responseMessage;

        public K ResponseMessage
        {
            get
            {
                return AnalyseMessage();
            }
            private set { _responseMessage = value; }

        }

        private HttpResponseMessage _responseValue;
        public HttpResponseMessage ResponseValue
        {
            get { return _responseValue; }
            set
            {
                _responseValue = value;
                AnlayzeResponse(value);
            }
        }

        public HttpResultStream()
        {
        }

        public HttpResultStream(HttpResponseMessage response)
        {
            AnlayzeResponse(response);
        } /*End of HttpResult constructor*/

        private T AnalyseType()
        {
            if (ResponseValue == null)
            {
                return default(T);
            }

            if (!ResponseCondition(ResponseValue.StatusCode, ValidStatusCodes)) { return default(T); }

            try
            {
                return (T)ResponseValue.Content.ReadAsStreamAsync().Result;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        private K AnalyseMessage()
        {
            if (ResponseValue == null)
            {
                return default(K);
            }

            if (ResponseCondition(ResponseValue.StatusCode, ValidStatusCodes)) { return default(K); }

            try
            {
                var jsonContent = ResponseValue.Content.ReadAsStringAsync().Result;
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
            AnalyseType();
            AnalyseMessage();

        }/*End of AnlayzeResponse method*/
    }/*End of HttpResult<T,K> class*/

    public class HttpResultStream<T> : IHttpResult<T>
        where T : Stream
    {
        public Func<HttpStatusCode, List<HttpStatusCode>, bool> ResponseCondition { get; set; }
        = ((p, s) => s.Contains(p));

        public List<HttpStatusCode> ValidStatusCodes { get; set; } = new List<HttpStatusCode>() {HttpStatusCode.OK};
        public SerializationFormat SerializationFormat { get; set; } = SerializationFormat.None;

        public HttpStatusCode StatusCode { get; set; }
        public List<Exception> ResponseErrorList { get; set; }

        private T _responseType;
        public T ResponseType
        {
            get
            {
                return AnalyseType();
            }
            private set { _responseType = value; }
        }

        private HttpResponseMessage _responseValue;

        public HttpResponseMessage ResponseValue
        {
            get { return _responseValue; }
            set
            {
                _responseValue = value;
                AnlayzeResponse(value);
            }
        }

        public HttpResultStream()
        {
        }

        public HttpResultStream(HttpResponseMessage response)
        {
            AnlayzeResponse(response);
        } /*End of HttpResult constructor*/

        private T AnalyseType()
        {
            if (ResponseValue == null)
            {
                return default(T);
            }

            if (!ResponseCondition(ResponseValue.StatusCode, ValidStatusCodes)) { return default(T); }

            try
            {
                return (T)ResponseValue.Content.ReadAsStreamAsync().Result;
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
            AnalyseType();

        } /*End of AnlayzeResponse method*/
    }/*End of HttpResultStream<T> class*/
}/*End of Funny.Accents.Core.Http.RequestService.HttpResults namespace*/
