using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace guiziTCPfangyuan
{
    public class pubFunc
    {
        public static bool testmode = false;
        public static int wuhan_door_cnt=16;
        public static string weburl = "http://121.41.94.224:8080/jck";
        public static string postUrl = "http://121.41.94.224:8080/jck/api/synCabinet";
        public static string alertphone = "13913971910";
        public static string compname = "【家常客】";
        public static bool isdebug = false;

        System.Collections.Specialized.NameValueCollection postvars = new System.Collections.Specialized.NameValueCollection();
        
        public class guizi_info_class
        {
            public string guizi_id;
            public byte[] head_data = { 0xa8, 0x81, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x39, 0x35, 0x37, 0x37, 0x30, 0x34, 0x37, 0x32, 0x01, 0x00, 0x24 };
            public bool is_on = false;
            public int heart_on_count = 0;
        }
        public class smsInfoC
        {
            public string serialNO = "0SDK-EAA-6688-JEQTO";
            public string serialPwd = "357021";
            public string schTime = "";
            public string phone = "";
            public string smsContent = "";
            public string addSerial = "";
        }
        public static smsInfoC smsInfo = new smsInfoC();

        public Webservice.SDKService sdkService = new global::guiziTCPfangyuan.Webservice.SDKService();

        //Little、BigEndian Reverse
        public UInt32 ReverseBytes(UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        public ushort ReverseBytes(ushort value)
        {
            return (ushort)((value & 0x000000FFU) << 8 | (value & 0x0000FF00U) >> 8);
        }

        /// <summary>
        /// 时间转成Unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long UNIX_TIMESTAMP(DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - DateTime.Parse("1970-01-01 00:00:00").Ticks) / 10000000;
        }

        //Unix timespan to Windows
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        //long to byte[]
        public static byte[] longToBytes(long n)
        {
            byte[] b = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                b[i] = (byte)(n >> (24 - i * 8));
            }
            return b;
        }

        /**  
	 * bytes转换成十六进制字符串  
	 * @param byte[] b byte数组  
	 * @return String 每个Byte值之间空格分隔  
	 */
        public static String byte2HexStr2(byte[] b)
        {
            String stmp = "";
            stmp = HexToString(b);
            return stmp.ToString().ToLower().Trim();
        }

        public static string card10to8(string card10)
        {
            if (card10.Length != 10)
            {
                card10 = "0000000000";
            }
            long cardlong = long.Parse(card10);
            byte[] cardbyte = longToBytes(cardlong);
            for (int i = 0; i < cardbyte.Length; i++)
            {
                cardbyte[i] = (byte)~cardbyte[i];
            }
            byte[] cardbytestr = new byte[cardbyte.Length * 2];
            for (int j = 0; j < cardbyte.Length; j++)
            {
                byte tmp = cardbyte[j];
                cardbytestr[2 * j] = (byte)((tmp >> 4) + 0x30);
                cardbytestr[2 * j + 1] = (byte)((tmp & 0x0F) + 0x30);
            }
            return new string(System.Text.Encoding.Default.GetChars(cardbytestr, 0, 8));
        }

        //Hex to String
        public static string HexToString(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2")+" ";
                }
            }
            return returnStr;
        }

        public static byte[] StringToHex(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            hexString = hexString.Replace("\r\n", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        //返回1正确，返回0错误  
        public bool isdooridRight(string str)
        {
            int len = str.Length;
            int num;
            if (len != 2)//长度为2
                return false;
            bool result = int.TryParse(str, out num);
            if (!result)//不合格数字
                return false;
            if ((num < 1) || (num > wuhan_door_cnt))//不在范围内
                return false;
            return true;
        }
        //返回1正确，返回0错误  
        public bool isguiziidRight(string str)
        {
            int len = str.Length;
            int num;
            if (len != 8)//长度为8
                return false;
            bool result = int.TryParse(str, out num);
            if (!result)//不合格数字
                return false;
            if ((num < 0) || (num > 99999999))//不在范围内
                return false;
            return true;
        }
        public bool isphoneRight(string str)
        {
            return true;
        }

        public void smsalert(int type)//发送报警短信
        {
            string enevt = "";
            if (type == 1) enevt = "数据库连接错误！";
            else if (type == 2) enevt = "接收客户端数据出错！";
            smsInfo.phone = alertphone;
            smsInfo.smsContent = string.Format("{0}提醒您：柜子服务软件发生故障！请及时处理。故障类型：{1}", compname, enevt);
            sdkService.sendSMS(smsInfo.serialNO, smsInfo.serialPwd, smsInfo.schTime, smsInfo.phone.Split(new char[] { ',' }), smsInfo.smsContent, smsInfo.addSerial, "GBK", 3, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
        }
        public void smsalert(int type,string code)//发送报警短信
        {
            string enevt = "";
            if (type == 1) enevt = "数据库连接错误！";
            else if (type == 2) enevt = "接收客户端数据出错！";
            smsInfo.phone = alertphone;
            smsInfo.smsContent = string.Format("{0}提醒您：柜子服务软件发生故障！请及时处理。故障类型：{1},备注：{2}", compname, enevt,code);
            sdkService.sendSMS(smsInfo.serialNO, smsInfo.serialPwd, smsInfo.schTime, smsInfo.phone.Split(new char[] { ',' }), smsInfo.smsContent, smsInfo.addSerial, "GBK", 3, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
        }
    }
}
