using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Model;

namespace OwinWebApiClient.ApiClients
{
    public class VirtualMachineClient
    {
        private readonly string _accessToken;
        private readonly Uri _baseRequestUri;
        private string _vSphereUrl;
        private string _vSphereSessionId;

        private static string VSphereUrlCookie = "vSphereServiceUrl";
        private static string VSphereSessionIdCookie = "vSphereSessionId";

        public VirtualMachineClient(Uri baseUri, string accessToken, string vSphereUrl, string vSphereSessionId)
        {
            _accessToken = accessToken;
            _baseRequestUri = new Uri(baseUri, "api/VirtualMachines/");
            _vSphereUrl = vSphereUrl;
            _vSphereSessionId = vSphereSessionId;
        }

        // Handy helper method to set the access token for each request:
        private void SetClientAuthentication(HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        private void SetClientCookie(CookieContainer cookieContainer)
        {
            cookieContainer.Add(_baseRequestUri, new Cookie(VSphereUrlCookie, _vSphereUrl));
            cookieContainer.Add(_baseRequestUri, new Cookie(VSphereSessionIdCookie, _vSphereSessionId));
        }


        public async Task<List<string>> GetVirtualMachinesAsync()
        {
            HttpResponseMessage response;
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _baseRequestUri })
            {
                SetClientCookie(cookieContainer);
                SetClientAuthentication(client);
                response = await client.GetAsync(_baseRequestUri);
            }
            
            return await response.Content.ReadAsAsync<List<string>>();
        }

        public async Task<VmInfoDto> GetVirtualMachineAsync(string name)
        {
            HttpResponseMessage response;
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _baseRequestUri })
            {
                SetClientCookie(cookieContainer);
                SetClientAuthentication(client);
                response = await client.GetAsync(new Uri(_baseRequestUri, name));
            }

            return await response.Content.ReadAsAsync<VmInfoDto>();
        }
        
    }
}