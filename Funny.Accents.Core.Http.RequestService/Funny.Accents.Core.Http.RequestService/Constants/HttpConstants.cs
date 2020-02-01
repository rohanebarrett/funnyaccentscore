namespace Funny.Accents.Core.Http.RequestService.Constants
{
    public sealed class HttpConstants
    {
        private readonly string _name;

        private HttpConstants(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }

        private static readonly HttpConstants ApplicationJson
            = new HttpConstants("application/json");
        private static readonly HttpConstants ApplicationOctetStream
            = new HttpConstants("application/octet-stream");

        public static string GetApplicationJson()
        {
            return ApplicationJson.ToString();
        }

        public static string GetApplicationOctetStream()
        {
            return ApplicationOctetStream.ToString();
        }
    }/*End of HttpConstants sealed class*/
}/*End of Funny.Accents.Core.Http.RequestService.Constants namespace*/