
using System.Collections.Generic;

namespace Model
{
    public class VmInfoDto
    {
        public string Name { get; set; }
        public List<CdDiskInfoDto> CdDisks { get; set; }
        public List<VirtualDiskInfoDto> VirtualDisks { get; set; }
    }
}
