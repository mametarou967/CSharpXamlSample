using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E220_900T225_ConfigTool.Model.Register
{
    public class Register01 : RegisterXX
    {
        public Register01(byte value) : base(0x01, value) { }

        protected override string FormatDetail()
        {
            return $"ADDL(ｱﾄﾞﾚｽ下位):0x{data:X2}";
        }
    }
}
