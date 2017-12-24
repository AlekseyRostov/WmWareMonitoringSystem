using System.Collections.Generic;
using System.Web.Http;
using Model;
using RestService.Utils;
using WMWareApi;
using WMWareApi.Model;

namespace RestService.Controllers
{
    public class VirtualMachinesController : ApiController
    {
        const string serviceUrl = @"https://192.168.1.69/sdk";
        const string userName = "root";
        const string password = "P@ssw0rd";

        private WMWareService _vmWareService = null;
        

        // GET: api/VirtualMachines
        public IHttpActionResult GetVirtualMachines()
        {
            _vmWareService = new WMWareService(serviceUrl, userName, password);
            List<string> virtualMachines = _vmWareService.GetVirtualMachines();
            return Ok(virtualMachines);
        }

        // GET: api/VirtualMachines/5
        public IHttpActionResult GetVirtualMachines(string id)
        {
            _vmWareService = new WMWareService(serviceUrl, userName, password);
            VirtualMachineInfo vmInfo = _vmWareService.GetVirtualMachineInfo(id);
            if (vmInfo == null) return NotFound();

            //VmInfoDto result = new VmInfoDto();
            //VirtualMachineInfo result1 = new VirtualMachineInfo();
            //vmInfo.CopyPublicPropertyTo(result1);
            return Ok(vmInfo);
        }
        
    }
}
