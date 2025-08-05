using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E220_900T225_ConfigTool.Model.Register
{
    public class Register00 : RegisterXX
    {
        public Register00(byte value) : base(0x00, value) { }

        protected override string FormatDetail()
        {
            return $"ADDH(ｱﾄﾞﾚｽ上位):0x{data:X2}";
        }
    }
}
