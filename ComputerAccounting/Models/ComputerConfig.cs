using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerAccounting
{
    public class ComputerConfig : BaseModel
    {
        public int ComputerConfigId { get; set; }
        public string ConfigName { get; set; }
        public string CPUName { get; set; }
        public string GPUName { get; set; }
        public string RAMName { get; set; }
        public int? RAMCapacity { get; set; }
        public string DiskName { get; set; }
        public int? DiskCapacity { get; set; }
    }
}
