using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Osprey3
{
    public static class HttpClientFactory
    {
        public static HttpMessageHandler CreateHandler()
        {
            // Create a custom handler to bypass SSL validation (for development only)
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            return handler;
        }

        public static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient(CreateHandler());
            return httpClient;
        }
    }
}