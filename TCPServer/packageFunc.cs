using System;
using System.Collections.Generic;
using System.Text;

namespace guiziTCPfangyuan
{
    class packageFunc:pubFunc
    {
        public byte[] make_pack(string cab_id, string command_id, byte[] data)//将data数据打成包
        {
            if (cab_id == "")
                cab_id = "00000000";
            uint pack_len = (uint)data.Length;
            byte[] returnBytes = new byte[21 + pack_len];
            Array.Copy(new byte[] { 0x59, 0x47, 0x01 }, 0, returnBytes, 0, 3);//包头及版本号
            Array.Copy(System.Text.Encoding.Default.GetBytes(cab_id), 0, returnBytes, 3, 8);//柜子id
            Array.Copy(System.Text.Encoding.Default.GetBytes(command_id), 0, returnBytes, 11, 2);
            Array.Copy(BitConverter.GetBytes(ReverseBytes(pack_len)), 0, returnBytes, 13, 4);
            Array.Copy(BitConverter.GetBytes(ReverseBytes((uint)UNIX_TIMESTAMP(DateTime.Now))), 0, returnBytes, 17, 4);
            Array.Copy(data, 0, returnBytes, 21, pack_len);

            return returnBytes;
        }

        public byte[] add_int(byte[] data, uint int_num)//往data中添加int数据
        {
            byte[] returnBytes = new byte[data.Length + 4];
            Array.Copy(data, 0, returnBytes, 0, data.Length);
            Array.Copy(BitConverter.GetBytes(ReverseBytes(int_num)), 0, returnBytes, data.Length, 4);
            return returnBytes;
        }

        public byte[] add_string(byte[] data, string str_data)//往data中添加string数据
        {
            uint strlen = (uint)(str_data.Length);
            byte[] returnBytes = new byte[data.Length + 4 + str_data.Length];
            Array.Copy(data, 0, returnBytes, 0, data.Length);
            Array.Copy(BitConverter.GetBytes(ReverseBytes(strlen)), 0, returnBytes, data.Length, 4);
            Array.Copy(System.Text.Encoding.Default.GetBytes(str_data), 0, returnBytes, data.Length + 4, str_data.Length);
            return returnBytes;
        }

        public int get_int(byte[] data, int startIndex)
        {
            return (int)ReverseBytes(BitConverter.ToUInt32(data, startIndex));
        }
        public int get_ushort(byte[] data, int startIndex)
        {
            return (int)ReverseBytes(BitConverter.ToUInt32(new byte[] { 0x00,0x00,data[startIndex], data[startIndex+1] }, 0));
        }
        public int get_byte(byte[] data, int startIndex)
        {
            return (int)ReverseBytes(BitConverter.ToUInt32(new byte[] { 0x00,0x00,0x00,data[startIndex] }, 0));
        }

        public string get_string(byte[] data, int startIndex)
        {
            string returnString = "";
            int unit_len = (int)ReverseBytes(BitConverter.ToUInt32(data, startIndex));
            if (unit_len != 0)
                returnString = new string(System.Text.Encoding.Default.GetChars(data, startIndex + 4, unit_len));
            return returnString;
        }
        //方缘柜子打包
        public byte[] pack_fangyuan(string cab_id, string command_id, byte[] data)
        {
            if (cab_id == "")
                cab_id = "00000000";
            uint pack_len = (uint)data.Length;
            byte[] returnBytes = new byte[31 + pack_len];
            Array.Copy(new byte[] { 0x59, 0x47, 0x01 }, 0, returnBytes, 0, 3);//包头及版本号
            Array.Copy(System.Text.Encoding.Default.GetBytes(cab_id), 0, returnBytes, 3, 8);//柜子id
            Array.Copy(System.Text.Encoding.Default.GetBytes(command_id), 0, returnBytes, 11, 2);
            Array.Copy(BitConverter.GetBytes(ReverseBytes(pack_len)), 0, returnBytes, 13, 4);

            Array.Copy(BitConverter.GetBytes(ReverseBytes((ushort)System.DateTime.Now.Year)), 0, returnBytes, 17, 2);//年
            Array.Copy(BitConverter.GetBytes((System.Byte)System.DateTime.Now.Month), 0, returnBytes, 19, 1);//月
            Array.Copy(BitConverter.GetBytes((System.Byte)System.DateTime.Now.Day), 0, returnBytes, 20, 1);//日
            Array.Copy(BitConverter.GetBytes((System.Byte)System.DateTime.Now.Hour), 0, returnBytes, 21, 1);//时
            Array.Copy(BitConverter.GetBytes((System.Byte)System.DateTime.Now.Minute), 0, returnBytes, 22, 1);//分
            Array.Copy(BitConverter.GetBytes((System.Byte)System.DateTime.Now.Second), 0, returnBytes, 23, 1);//秒

            //Array.Copy(System.Text.Encoding.Default.GetBytes(System.DateTime.Now.ToString("yyyyMMddHHmmss")), 0, returnBytes, 17, 14);//时间

            Array.Copy(data, 0, returnBytes, 24, pack_len);

            return returnBytes;
        }
        //武汉柜子打包
        public byte[] pack_wuhan(string guizi_id, string cmd, string door_id, string status, string send_data, string user_id, string pwd, byte[] head_data)//将data数据打成包
        {
            byte[] returnBytes = new byte[51];
            byte[] returnBytes2 = new byte[71];
            for (int i = 0; i < 51; i++)
            { returnBytes[i] = 0xFF; }

            Array.Copy(new byte[] { 0xA5, 0xA5, 0x2E }, 0, returnBytes, 0, 3);//包头及len
            Array.Copy(new byte[] { 0x5A,0x5A }, 0, returnBytes, 49, 2);//包尾

            if (guizi_id != null)
                Array.Copy(System.Text.Encoding.Default.GetBytes(guizi_id), 0, returnBytes, 3, 8);//柜子id
            
            if (cmd != null)
                Array.Copy(System.Text.Encoding.Default.GetBytes(cmd), 0, returnBytes, 11, 1);//command type
            
            if(door_id!=null)
                Array.Copy(System.Text.Encoding.Default.GetBytes(door_id), 0, returnBytes, 12, 2);//box num
            
            if(status!=null)
                Array.Copy(System.Text.Encoding.Default.GetBytes(status), 0, returnBytes, 14, 1);

            if(user_id!=null)
                Array.Copy(System.Text.Encoding.Default.GetBytes(user_id), 0, returnBytes, 35, 8);

            if(pwd!=null)
                Array.Copy(System.Text.Encoding.Default.GetBytes(pwd), 0, returnBytes, 43, 6);

            if(send_data!=null)
                Array.Copy(System.Text.Encoding.Default.GetBytes(send_data), 0, returnBytes, 15, send_data.Length);

            Array.Copy(head_data, 0, returnBytes2, 0, 20);
            Array.Copy(returnBytes, 0, returnBytes2, 20, 51);
            return returnBytes2;
        }

    }
}
