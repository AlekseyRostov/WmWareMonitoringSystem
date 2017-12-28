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
        const string serviceUrl = @"https://192.168.1.69/sdk";
        const string userName = "root";
        const string password = "P@ssw0rd";

        private VmWareClient _vmWareClient = null;
        
        [Route("api/VirtualMachines")]
        public IHttpActionResult GetVirtualMachines(VmRequestInfo vm)
        {
            _vmWareClient = new VmWareClient(serviceUrl, userName, password);
            List<string> virtualMachines = _vmWareClient.GetVirtualMachines();
            return Ok(virtualMachines);
        }

        [Route("api/VirtualMachines/{vm.name}")]
        public IHttpActionResult GetVirtualMachineInfo(VmRequestInfo vm)
        {
            _vmWareClient = new VmWareClient(serviceUrl, userName, password);
            VirtualMachineInfo vmInfo = _vmWareClient.GetVirtualMachineInfo(id);
            if (vmInfo == null) return NotFound();
            return Ok(vmInfo);
        }
    }
}