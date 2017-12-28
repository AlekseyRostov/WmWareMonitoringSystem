using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using OwinWebApiSelfHost.Model;
using WMWareApi;
using WMWareApi.Model;

namespace OwinWebApiSelfHost.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VirtualMachinesController : ApiController
    {
        //const string serviceUrl = @"https://192.168.75.195/sdk";
        //const string userName = "root";
        //const string password = "P@ssw0rd";

        private VmWareClient _vmWareClient = null;

        private string GetCookie(string name)
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies().FirstOrDefault();
            return cookie?.Cookies.Where(x => x.Name == name)
                                  .Select(x => x.Value)
                                  .FirstOrDefault();
        }

        // GET api/VirtualMachines
        public IHttpActionResult GetVirtualMachines()
        {
            try
            {
                string sessionId = GetCookie(VSphereAccountController.SessionId);
                string serviceUrl = GetCookie(VSphereAccountController.ServiceUrl);

                _vmWareClient = new VmWareClient(serviceUrl, sessionId);
                List<string> virtualMachines = _vmWareClient.GetVirtualMachines();
                return Ok(virtualMachines);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        // GET api/VirtualMachines/name
        public IHttpActionResult GetVirtualMachineInfo(string id)
        {
            try
            {
                string sessionId = GetCookie(VSphereAccountController.SessionId);
                string serviceUrl = GetCookie(VSphereAccountController.ServiceUrl);

                _vmWareClient = new VmWareClient(serviceUrl, sessionId);
                VirtualMachineInfo vmInfo = _vmWareClient.GetVirtualMachineInfo(id);
                if (vmInfo == null) return NotFound();
                return Ok(vmInfo);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}