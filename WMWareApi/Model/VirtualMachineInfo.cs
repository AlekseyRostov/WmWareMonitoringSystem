using System.Collections.Generic;

namespace WMWareApi.Model
{
    public class VirtualMachineInfo
    {
        public string Name { get; set; }
        public List<CdDiskInfo> CdDisks { get; set; }
        public List<VirtualDiskInfo> VirtualDisks { get; set; }

    }
}