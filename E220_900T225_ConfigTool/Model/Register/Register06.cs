using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E220_900T225_ConfigTool.Model.Register
{
    public class Register06 : RegisterXX
    {
        public Register06(byte value) : base(0x06, value) { }

        protected override string FormatDetail()
        {
            return $"※ Write Only";
        }

    }
}
