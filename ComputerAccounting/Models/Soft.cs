using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerAccounting
{
    public class Soft : BaseModel
    {
        public string SoftName { get; set; }

        public override string ToString()
        {
            return SoftName;
        }
    }
}
