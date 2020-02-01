using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Funny.Accents.Core.Http.RequestService.Interfaces;
using Funny.Accents.Core.Http.RequestService.Helpers;
using SerializationFormat = Funny.Accents.Core.Http.RequestService.Enums.SerializationFormat;

namespace Funny.Accents.Core.Http.RequestService.HttpResults
{
    public class HttpResult : IHttpResult
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
            = new List<HttpStatusCode>() { HttpStatusCode.OK };

        public HttpStatusCode StatusCode { get; set; }
        public List<Exception> ResponseErrorList { get; set; }

        public SerializationFormat SerializationFormat { get; set; } = SerializationFormat.None;

        private void AnlayzeResponse(HttpResponseMessage response)
        {
            if (response == null)
            {
                return;
            }

            StatusCode = response.StatusCode;
        }
    }

    public class HttpResult<T, K> : IHttpResult<T, K>
        where T : class, new()
        where K : class, new()
    {
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

        public Func<HttpStatusCode, List<HttpStatusCode>, bool> ResponseCondition { get; set; }
        = ((p, s) => s.Contains(p));

        public List<HttpStatusCode> ValidStatusCodes { get; set; } 
            = new List<HttpStatusCode>() {HttpStatusCode.OK};

        public HttpStatusCode StatusCode { get; set; }

        public List<Exception> ResponseErrorList { get; set; } = new List<Exception>();

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

        public SerializationFormat SerializationFormat { get; set; } = SerializationFormat.None;

        public HttpResult()
        {
        }

        public HttpResult(HttpResponseMessage response, Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition
            , List<HttpStatusCode> validStatusCodes, SerializationFormat serializationFormat)
        {
            ResponseCondition = responseCondition;
            ValidStatusCodes = validStatusCodes;
            SerializationFormat = serializationFormat;
            ResponseValue = response;
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
                //var jsonContent = ResponseValue.Content.ReadAsStringAsync().Result;
                var buffer = ResponseValue.Content.ReadAsByteArrayAsync().Result;
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

        private K AnalyseMessage()
        {
            if (ResponseValue == null)
            {
                return default(K);
            }

            if (ResponseCondition(ResponseValue.StatusCode, ValidStatusCodes)) { return default(K); }

            try
            {
                var buffer = ResponseValue.Content.ReadAsByteArrayAsync().Result;
                var byteArray = buffer.ToArray();
                var jsonContent = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

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
            ResponseType = AnalyseType();
            ResponseMessage = AnalyseMessage();
        }
    } /*End of HttpResult class*/

    public class HttpResult<T> : IHttpResult<T>
        where T : class,new()
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<Exception> ResponseErrorList { get; set; }

        public Func<HttpStatusCode, List<HttpStatusCode>, bool> ResponseCondition { get; set; }
            = ((p, s) => s.Contains(p));

        public List<HttpStatusCode> ValidStatusCodes { get; set; }
            = new List<HttpStatusCode>() { HttpStatusCode.OK };

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

        private T _responseType;
        public T ResponseType
        {
            get
            {
                return AnalyseType();
            }
            private set { _responseType = value; }
        }

        public SerializationFormat SerializationFormat { get; set; } = SerializationFormat.None;

        public HttpResult()
        {
        }

        public HttpResult(HttpResponseMessage response, Func<HttpStatusCode, List<HttpStatusCode>, bool> responseCondition
            , List<HttpStatusCode> validStatusCodes, SerializationFormat serializationFormat)
        {
            ResponseCondition = responseCondition;
            ValidStatusCodes = validStatusCodes;
            SerializationFormat = serializationFormat;
            ResponseValue = response;
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
                //var jsonContent = ResponseValue.Content.ReadAsStringAsync().Result;
                var buffer = ResponseValue.Content.ReadAsByteArrayAsync().Result;
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
            ResponseType = AnalyseType();
        }
    }/*End of ttpResult<T> class*/
}/*End of Funny.Accents.Core.Http.RequestService.HttpResults namespace*/
