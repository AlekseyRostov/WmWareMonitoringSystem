﻿using System.Collections.Generic;
using System.Web.Http;
using WMWareApi;
using WMWareApi.Model;

namespace RestService.Controllers
{
    public class VirtualMachinesController : ApiController
    {
        const string serviceUrl = @"https://192.168.1.69/sdk";
        const string userName = "root";
        const string password = "P@ssw0rd";

        private VmWareClient _vmWareService = null;
        

        // GET: api/VirtualMachines
        public IHttpActionResult GetVirtualMachines()
        {
            _vmWareService = new VmWareClient(serviceUrl, userName, password);
            List<string> virtualMachines = _vmWareService.GetVirtualMachines();
            return Ok(virtualMachines);
        }

        // GET: api/VirtualMachines/5
        public IHttpActionResult GetVirtualMachines(string id)
        {
            _vmWareService = new VmWareClient(serviceUrl, userName, password);
            VirtualMachineInfo vmInfo = _vmWareService.GetVirtualMachineInfo(id);
            if (vmInfo == null) return NotFound();
            return Ok(vmInfo);
        }
        
    }
}
