using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E220_900T225_ConfigTool.Model.Register
{
    public class Register04 : RegisterXX
    {
        public Register04(byte value) : base(0x04, value) { }

        protected override string FormatDetail()
        {
            return $"ﾁｬﾝﾈﾙ:{data}ch";
        }
    }
}
