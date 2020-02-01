using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Funny.Accents.Core.Http.RequestService.Enums;

namespace Funny.Accents.Core.Http.RequestService.Interfaces
{
    public interface IHttpResultBase
    {
        HttpResponseMessage ResponseValue { get; set; }
        HttpStatusCode StatusCode { get; set; }
        List<Exception> ResponseErrorList { get; set; }
    }

    public interface IHttpResult : IHttpResultBase
    {
        Func<HttpStatusCode, List<HttpStatusCode>, bool> ResponseCondition { get; set; }
        List<HttpStatusCode> ValidStatusCodes { get; set; }
        SerializationFormat SerializationFormat { get; set; }
    }/*End of IHttpResult class*/

    public interface IHttpResult<T> : IHttpResult
    {
        T ResponseType { get; }
    }/*End of IHttpResult<T> class*/

    public interface IHttpResult<T, K> : IHttpResult
    {
        T ResponseType { get; }
        K ResponseMessage { get; }
    }/*End of IHttpResult<T, K> class*/

    public interface IHttpResultAsync<T> : IHttpResult
    {
        Task<T> GetResponseType();
    }/*End of IHttpResult<T> class*/

    public interface IHttpResultAsync<T, K> : IHttpResult
    {
        Task<T> GetResponseType();
        Task<K> GetResponseMessage();
    }/*End of IHttpResult<T, K> class*/
}/*End of Funny.Accents.Core.Http.RequestService.Interfaces.HttpResults namespace*/
