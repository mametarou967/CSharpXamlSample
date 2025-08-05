using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E220_900T225_ConfigTool.Model.Register
{
    public class Register05 : RegisterXX
    {
        public Register05(byte value) : base(0x05, value) { }

        protected override string FormatDetail()
        {
            var lRSSIﾊﾞｲﾄの有効化 = (RSSIﾊﾞｲﾄの有効化)((data >> 7) & 0x1);
            var l送信方法 = (送信方法)((data >> 6) & 0x1);
            var lWORｻｲｸﾙ = (WORｻｲｸﾙ)(data & 0x7);
            return $"RSSIﾊﾞｲﾄの有効化:{lRSSIﾊﾞｲﾄの有効化} 送信方法:{l送信方法} WORｻｲｸﾙ:{lWORｻｲｸﾙ}ms";
        }

        enum RSSIﾊﾞｲﾄの有効化
        {
            無効 = 0,
            有効 = 1
        }

        enum 送信方法
        {
          ﾄﾗﾝｽﾍﾟｱﾚﾝﾄﾓｰﾄﾞ = 0,
          固定送信ﾓｰﾄﾞ = 1
        }


        enum WORｻｲｸﾙ
        {
            _500 = 0,
            _1000 = 1,
            _1500 = 2,
            _2000 = 3,
            _2500 = 4,
            _3000 = 5,
            _3500 = 6,
            _4000 = 7,
        }
    }
}
