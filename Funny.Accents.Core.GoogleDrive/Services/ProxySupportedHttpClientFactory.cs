using System.Net;
using System.Net.Http;
using Google.Apis.Http;

namespace Funny.Accents.Core.Core.GoogleDrive.Services
{
    public class ProxySupportedHttpClientFactory : HttpClientFactory
    {
        private readonly string _proxyAddress = string.Empty;
        private readonly bool _bypassLocal = true;
        private readonly string[] _bypassList = null;
        private readonly ICredentials _credentials = null;

        public ProxySupportedHttpClientFactory(string proxyAddress, bool bypassLocal,
            string[] bypassList, ICredentials credentials)
        {
            _proxyAddress = proxyAddress;
            _bypassLocal = bypassLocal;
            _bypassList = bypassList;
            _credentials = credentials;
        }/*End of ProxySupportedHttpClientFactory constructor*/

        protected override HttpMessageHandler CreateHandler(CreateHttpClientArgs args)
        {
            var proxy = new WebProxy(_proxyAddress, _bypassLocal, _bypassList, _credentials);

            var webRequestHandler = new HttpClientHandler()
            {
                UseProxy = true,
                Proxy = proxy,
                UseCookies = false
            };

            return webRequestHandler;
        }/*End of CreateHandler method override*/
    }/*End of ProxySupportedHttpClientFactory class*/
}/*End of CmkGoogleDriveUtilities.Services namespace*/
