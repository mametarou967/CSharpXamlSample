using E220_900T225_ConfigTool.Model.Logging;
using E220_900T225_ConfigTool.Model.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E220_900T225_ConfigTool.Model.E220_900_T225_ProtocolManager
{
    public class E220_900_T225_ProtocolManager
    {
        E220_900T225_ConfigTool.Model.SerialCom.SerialCom serialCom;
        Queue<byte> receiveDataQueue = new Queue<byte>();
        ILogWriteRequester logWriteRequester;
        private readonly object sendLock = new object(); // ロックオブジェクト
        Dictionary<byte, Func<byte, RegisterXX>> registerXxList = new Dictionary<byte, Func<byte, RegisterXX>>();

        public E220_900_T225_ProtocolManager()
        {
            registerXxList = new Dictionary<byte, Func<byte, RegisterXX>>()
            {
                { 0x00 , data => new Register00(data) },
                { 0x01 , data => new Register01(data) },
                { 0x02 , data => new Register02(data) },
                { 0x03 , data => new Register03(data) },
                { 0x04 , data => new Register04(data) },
                { 0x05 , data => new Register05(data) },
                { 0x06 , data => new Register06(data) },
                { 0x07 , data => new Register07(data) },
                { 0x08 , data => new Register08(data) },

            };
        }


        public void ComStart(

                string comPort,
                ILogWriteRequester logWriteRequester)
        {
            this.logWriteRequester = logWriteRequester;
            serialCom = new SerialCom.SerialCom(comPort, DataReceiveAction, logWriteRequester);
            serialCom.StartCom();
        }

        public void ComStop()
        {
            serialCom?.StopCom();
        }



        private void DataReceiveAction(byte[] datas)
        {
            if (datas[0] == 0xC1)
            {
                var startRegisterAddress = datas[1];
                var endRegisterAddress = startRegisterAddress + datas[2] - 1;

                for (byte dataIndex = 3; dataIndex < datas[2] + 3; dataIndex++)
                {
                    var registerAddress = (byte)(startRegisterAddress + dataIndex - 3);

                    try
                    {
                        var register = GenerateRegister(registerAddress, datas[dataIndex]);
                        logWriteRequester.WriteRequest(register.ToString());
                    }
                    catch(Exception ex)
                    {
                        logWriteRequester.WriteRequest(ex.Message);
                    }
                }
            }
        }

        public void SendDataWrite(byte[] datas) => serialCom.Send(datas);

        public void SendReg02Write(AirDataRate_SF_BW airDataRate_SF_BW)
        {
            var uartSerialPortRate = (byte)(0x03 << 5); // ﾎﾞｰﾚｰﾄ 9600bps
            var writeValue = (byte)((byte)uartSerialPortRate | (byte)airDataRate_SF_BW);
            serialCom.Send(new byte[] { 0xC0, 0x02, 0x01, writeValue });
            // new byte[] { }
        }

        public void SendReg04Write(byte channel)
        {
            serialCom.Send(new byte[] { 0xC0, 0x04, 0x01, channel });
        }

        RegisterXX GenerateRegister(byte registerAddress, byte value)
        {
            if (registerXxList.TryGetValue(registerAddress, out var constructor))
            {
                return constructor(value);
            }

            throw new ArgumentException($"Unknown register address: 0x{registerAddress:X2}");
        }
    }
}
