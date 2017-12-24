using System.Collections.Generic;

namespace WMWareApi.Model
{
    public class VmWareInfo
    {
        public string Name { get; set; }
        public DatastoreInfo Datastore { get; set; }

        public List<VirtualDiskInfo> VirtualDisks { get; set; }
        public List<CdDiskInfo> CdDisks { get; set; }
    }
}