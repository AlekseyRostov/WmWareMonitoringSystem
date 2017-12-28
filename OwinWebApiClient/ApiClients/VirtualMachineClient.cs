using System;
using System.Collections.Generic;
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
        private string _vSphereSessionId;

        public VirtualMachineClient(Uri baseUri, string accessToken, string vSphereSessionId)
        {
            _accessToken = accessToken;
            _baseRequestUri = new Uri(baseUri, "api/VirtualMachines/");
            _vSphereSessionId = vSphereSessionId;
        }

        // Handy helper method to set the access token for each request:
        private void SetClientAuthentication(HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _accessToken);
        }
        
        public async Task<List<string>> GetVirtualMachinesAsync()
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                SetClientAuthentication(client);
                response = await client.GetAsync(_baseRequestUri);
            }
            return await response.Content.ReadAsAsync<List<string>>();
        }

        public async Task<VmInfoDto> GetVirtualMachineAsync(string name)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                SetClientAuthentication(client);
                response = await client.GetAsync(new Uri(_baseRequestUri, name));
            }
            var result = await response.Content.ReadAsAsync<VmInfoDto>();
            return result;
        }
        
    }
}