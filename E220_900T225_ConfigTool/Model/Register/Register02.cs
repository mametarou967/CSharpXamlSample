using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E220_900T225_ConfigTool.Model.Register
{
    public class Register02 : RegisterXX
    {
        public Register02(byte value) : base(0x02, value) { }

        protected override string FormatDetail()
        {
            var baurate = (Baurate)(data >> 5);
            var airDataRateSfBw = (AirDataRate_SF_BW)(data & 0x1f);
            return $"ﾎﾞｰﾚｰﾄ:{baurate}  AirDataRate:{GetAirDataRate(airDataRateSfBw)}bps SF(拡散率):{GetSF(airDataRateSfBw)} BW(帯域幅):{GetBW(airDataRateSfBw)}kHz";
        }

        string GetAirDataRate(AirDataRate_SF_BW airDataRateSfBw)
        {
            switch(airDataRateSfBw)
            {
                case AirDataRate_SF_BW.AirDataRate_15625_SF_5_BW_125kHz: return "15625";
                case AirDataRate_SF_BW.AirDataRate_9357_SF_6_BW_125kHz: return "9357"; 
                case AirDataRate_SF_BW.AirDataRate_5469_SF_7_BW_125kHz: return "5469";
                case AirDataRate_SF_BW.AirDataRate_3125_SF_8_BW_125kHz: return "3125";
                case AirDataRate_SF_BW.AirDataRate_31250_SF_5_BW_250kHz: return "31250";
                case AirDataRate_SF_BW.AirDataRate_18750_SF_6_BW_250kHz: return "18750";
                case AirDataRate_SF_BW.AirDataRate_6250_SF_8_BW_250kHz: return "6250";
                case AirDataRate_SF_BW.AirDataRate_3516_SF_9_BW_250kHz: return "3516";
                case AirDataRate_SF_BW.AirDataRate_1953_SF_10_BW_250kHz: return "1953";
                case AirDataRate_SF_BW.AirDataRate_62500_SF_5_BW_500kHz: return "62500";
                case AirDataRate_SF_BW.AirDataRate_37500_SF_6_BW_500kHz: return "37500";
                case AirDataRate_SF_BW.AirDataRate_21875_SF_7_BW_500kHz: return "21875";
                case AirDataRate_SF_BW.AirDataRate_12500_SF_8_BW_500kHz: return "12500";
                case AirDataRate_SF_BW.AirDataRate_3906_SF_10_BW_500kHz: return "3906";
                case AirDataRate_SF_BW.AirDataRate_2148_SF_11_BW_500kHz: return "2148";
                default: return "?";
            }
        }


        string GetSF(AirDataRate_SF_BW airDataRateSfBw)
        {
            switch (airDataRateSfBw)
            {
                case AirDataRate_SF_BW.AirDataRate_15625_SF_5_BW_125kHz:
                case AirDataRate_SF_BW.AirDataRate_31250_SF_5_BW_250kHz:
                case AirDataRate_SF_BW.AirDataRate_62500_SF_5_BW_500kHz:
                    return "5";
                case AirDataRate_SF_BW.AirDataRate_9357_SF_6_BW_125kHz:
                case AirDataRate_SF_BW.AirDataRate_18750_SF_6_BW_250kHz:
                case AirDataRate_SF_BW.AirDataRate_37500_SF_6_BW_500kHz:
                    return "6";
                case AirDataRate_SF_BW.AirDataRate_5469_SF_7_BW_125kHz:
                case AirDataRate_SF_BW.AirDataRate_21875_SF_7_BW_500kHz:
                    return "7";
                case AirDataRate_SF_BW.AirDataRate_3125_SF_8_BW_125kHz:
                case AirDataRate_SF_BW.AirDataRate_6250_SF_8_BW_250kHz:
                case AirDataRate_SF_BW.AirDataRate_12500_SF_8_BW_500kHz:
                    return "8";
                case AirDataRate_SF_BW.AirDataRate_3516_SF_9_BW_250kHz:
                    return "9";
                case AirDataRate_SF_BW.AirDataRate_1953_SF_10_BW_250kHz:
                case AirDataRate_SF_BW.AirDataRate_3906_SF_10_BW_500kHz:
                    return "10";
                case AirDataRate_SF_BW.AirDataRate_2148_SF_11_BW_500kHz:
                    return "11";
                default: return "?";
            }
        }


        string GetBW(AirDataRate_SF_BW airDataRateSfBw)
        {
            switch (airDataRateSfBw)
            {
                case AirDataRate_SF_BW.AirDataRate_15625_SF_5_BW_125kHz:
                case AirDataRate_SF_BW.AirDataRate_9357_SF_6_BW_125kHz:
                case AirDataRate_SF_BW.AirDataRate_5469_SF_7_BW_125kHz: 
                case AirDataRate_SF_BW.AirDataRate_3125_SF_8_BW_125kHz:
                    return "125";
                case AirDataRate_SF_BW.AirDataRate_31250_SF_5_BW_250kHz:
                case AirDataRate_SF_BW.AirDataRate_18750_SF_6_BW_250kHz:
                case AirDataRate_SF_BW.AirDataRate_6250_SF_8_BW_250kHz:
                case AirDataRate_SF_BW.AirDataRate_3516_SF_9_BW_250kHz:
                case AirDataRate_SF_BW.AirDataRate_1953_SF_10_BW_250kHz:
                    return "250";
                case AirDataRate_SF_BW.AirDataRate_62500_SF_5_BW_500kHz: 
                case AirDataRate_SF_BW.AirDataRate_37500_SF_6_BW_500kHz:
                case AirDataRate_SF_BW.AirDataRate_21875_SF_7_BW_500kHz: 
                case AirDataRate_SF_BW.AirDataRate_12500_SF_8_BW_500kHz: 
                case AirDataRate_SF_BW.AirDataRate_3906_SF_10_BW_500kHz: 
                case AirDataRate_SF_BW.AirDataRate_2148_SF_11_BW_500kHz:
                    return "500";
                default: return "?";
            }
        }


        enum Baurate
        {
            _1200 = 0,
            _2400 = 1,
            _4800 = 2,
            _9600 = 3,
            _19200 = 4,
            _38400 = 5,
            _57600 = 6,
            _115200 = 7,
        }
    }
}
