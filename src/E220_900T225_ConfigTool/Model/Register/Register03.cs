using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E220_900T225_ConfigTool.Model.Register
{
    public class Register03 : RegisterXX
    {
        public Register03(byte value) : base(0x03, value) { }

        protected override string FormatDetail()
        {
            var lｻﾌﾞﾊﾟｹｯﾄ長 = (ｻﾌﾞﾊﾟｹｯﾄ長)(data >> 6);
            var lRSSI環境ノイズの有効化 = (RSSI環境ノイズの有効化)((data >> 4) & 0x1);
            var l送信出力電力 = (送信出力電力)(data & 0x3);
            return $"ｻﾌﾞﾊﾟｹｯﾄ長:{lｻﾌﾞﾊﾟｹｯﾄ長}  RSSI環境ノイズの有効化:{lRSSI環境ノイズの有効化} 送信出力電力:{l送信出力電力}";
        }

        enum ｻﾌﾞﾊﾟｹｯﾄ長
        {
            _200Byte = 0,
            _128Byte = 1,
            _64Byte = 2,
            _32Byte = 3,
        }

        enum RSSI環境ノイズの有効化
        {
            無効 = 0,
            有効 = 1
        }

        enum 送信出力電力
        {
            使用不可 = 0,
            _13dBm = 1,
            _0dBm = 2,
            _7dBm = 3,
        }

    }
}
