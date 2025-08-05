using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E220_900T225_ConfigTool.Model.Register
{
    public abstract class RegisterXX
    {
        protected byte data;

        byte address;

        public RegisterXX(byte address , byte data)
        {
            this.address = address;
            this.data = data;
        }

        protected abstract string FormatDetail();

        public override string ToString()
        {
            return $"[REG:0x{address:X2}] {FormatDetail()}";
        }
    }
}
