using System.Collections.Generic;
using System.Web.Http;
using OwinWebApiSelfHost.Model;
using WMWareApi;
using WMWareApi.Model;

namespace OwinWebApiSelfHost.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VirtualMachinesController : ApiController
    {
        const string serviceUrl = @"https://192.168.75.195/sdk";
        const string userName = "root";
        const string password = "P@ssw0rd";

        private VmWareClient _vmWareClient = null;

        // GET api/VirtualMachines
        public IHttpActionResult GetVirtualMachines()
        {
            _vmWareClient = new VmWareClient(serviceUrl, userName, password);
            List<string> virtualMachines = _vmWareClient.GetVirtualMachines();
            return Ok(virtualMachines);
        }

        // GET api/VirtualMachines/name
        public IHttpActionResult GetVirtualMachineInfo(string id)
        {
            _vmWareClient = new VmWareClient(serviceUrl, userName, password);
            VirtualMachineInfo vmInfo = _vmWareClient.GetVirtualMachineInfo(id);
            if (vmInfo == null) return NotFound();
            return Ok(vmInfo);
        }
    }
}