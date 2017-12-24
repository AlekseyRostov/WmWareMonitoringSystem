using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Model;

namespace ConsoleClient
{
    class Program
    {
        static HttpClient _client = new HttpClient();

        private static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            _client.BaseAddress = new Uri("http://localhost:42505/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                List<string> vms = await GetVirtualMachines();
                
                // Get the product
                VmInfoDto vmInfo = await GetVirtualMachineInfoByName(vms.FirstOrDefault());
               
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static async Task<List<string>> GetVirtualMachines()
        {
            List<string> vms = null;
            HttpResponseMessage response = await _client.GetAsync("api/VirtualMachines");
            if (response.IsSuccessStatusCode)
            {
                vms = await response.Content.ReadAsAsync<List<string>>();
            }
            return vms;
        }

        private static async Task<VmInfoDto> GetVirtualMachineInfoByName(string vmName)
        {
            VmInfoDto vmInfoDto = null;
            HttpResponseMessage response = await _client.GetAsync($"api/VirtualMachines/{vmName}");
            if (response.IsSuccessStatusCode)
            {
                vmInfoDto = await response.Content.ReadAsAsync<VmInfoDto>();
            }
            return vmInfoDto;
        }
    }
}