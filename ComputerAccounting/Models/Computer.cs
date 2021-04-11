using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerAccounting
{
    public class Computer : BaseModel
    {
        public int ComputerId { get; set; }
        public string CPUName { get; set; }
        public string GPUName { get; set; }
        public string RAMName { get; set; }
        public int? RAMCapacity { get; set; }
        public string DiskName { get; set; }
        public int? DiskCapacity { get; set; }
        public Cabinet Cabinet { get; set; }
        public string Softs { get; set; }
        public string InventoryNumber { get; set; }

        public void CreateInventoryNumber(int cabinetNumber, string computerNumber)
        {
            if (Convert.ToInt32(computerNumber) < 10)
                computerNumber = $"0{computerNumber}";
            InventoryNumber = $"PC-{cabinetNumber}-{computerNumber}";
        }
    }
}
