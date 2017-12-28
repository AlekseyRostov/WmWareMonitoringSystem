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

        public VirtualMachineClient(Uri baseUri, string accessToken)
        {
            _accessToken = accessToken;
            _baseRequestUri = new Uri(baseUri, "api/VirtualMachines/");
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
                var requestInfo = new
                {
                    Url = @"https://192.168.1.69/sdk",
                    Login = "root",
                    Password = "P@ssw0rd"
                };
                response = await client.GetAsync(new Uri(_baseRequestUri, Newtonsoft.Json.Serialization));
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