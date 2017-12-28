using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OwinWebApiClient.ApiClients
{
    public class VSphereClientProvider
    {
        private readonly Uri _baseUri;
        private readonly string _accessToken;

        public VSphereClientProvider(Uri baseUri, string accessToken)
        {
            _baseUri = baseUri;
            _accessToken = accessToken;
        }

        public async Task<string> GetSessionId(string url, string userName, string password)
        {
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            IEnumerable<Cookie> responseCookies;
            var content = new
                          {
                              Url = url,
                              Login = userName,
                              Password = password
                          };
            
            using (var client = new HttpClient(handler))
            {
                var cookieEndpoint = new Uri(_baseUri, "api/vSphereAccount/Register");
                SetClientAuthentication(client);
                await client.PostAsJsonAsync(cookieEndpoint, content);
                responseCookies = cookies.GetCookies(cookieEndpoint).Cast<Cookie>();
            }

            var sessionCookie = responseCookies.FirstOrDefault(x => x.Name == "vSphereSessionId");
            string sessionId = sessionCookie?.Value;

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new Exception("Error session cookie");
            }
            
            return sessionId;
        }

        private void SetClientAuthentication(HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }
        
    }
}