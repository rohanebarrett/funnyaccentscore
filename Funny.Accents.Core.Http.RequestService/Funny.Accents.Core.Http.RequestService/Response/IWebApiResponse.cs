namespace Funny.Accents.Core.Http.RequestService.Response
{
    public interface IWebApiResponse
    {
        string StatusCode { get; set; }
        string ResponseDescription { get; set; }
    }/*End of IHttpResult class*/

    public interface IWebApiResponse<out T> : IWebApiResponse
    {
        T Response { get; }
    }/*End of IHttpResult class*/

    public interface IWebApiResponse<out T, out TK> : IWebApiResponse
    {
        T Response { get; }
        TK ResponseMessge { get; }
    }/*End of IHttpResult class*/
}/*End of Funny.Accents.Core.Http.RequestService.Response namespace*/
