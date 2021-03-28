using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerAccounting
{
    public class Computer : BaseModel
    {
        public int ComputerId { get; set; }
        public string ComputerName { get; set; }
    }
}
