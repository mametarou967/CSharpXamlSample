using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace E220_900T225_ConfigTool.Model.Common
{
    static class Common
    {
        static public int ExtractNumber(string input)
        {
            string numericString = Regex.Replace(input, "[^0-9]", "");
            int extractedNumber = int.Parse(numericString);
            return extractedNumber;
        }
    }
}
