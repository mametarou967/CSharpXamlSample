using System.Collections.Generic;
using System.IO.Ports;

namespace E220_900T225_ConfigTool.Model.SerialPortManager
{
    static class SerialPortManager
    {
        static public List<string> GetAvailablePortNames()
        {
            string[] portNames = SerialPort.GetPortNames();
            var portList = new List<string>(portNames);
            portList.Reverse();
            return portList;
        }
    }
}
