using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using DotNet.Utilities;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace guiziTCPfangyuan
{
    public partial class Form1 : Form
    {
        public string database = "main";
        public string local = "115.29.251.217";
        public string user = "jck360";
        public string password = "baibang360";

        public static int storeDay = 0;

        WebClient wc;
        System.Collections.Specialized.NameValueCollection postvars = new System.Collections.Specialized.NameValueCollection();

       // public Dictionary<string, string> guizi_list = new Dictionary<string, string>();//数据库中的柜子号
        public Dictionary<string, string> express_list = new Dictionary<string, string>();//柜号，快递员卡号

        /// <summary>
        /// SQLHlper类的对象
        /// </summary>
        SQLHelper helper;
        packageFunc packFunc=new packageFunc();
        pubFunc pubFunction = new pubFunc();
        LogClass logFile = new LogClass();
        logRawData logRawData = new logRawData();
        private class insWuhanPara
        {
            public Socket sck;
            public string guizi_id;
            public string door_id;
            public string phone;
            public string express_id;
            public string member_id;
            public string pwd;
            public string data01;
            public string data02;
            public string data03;
        }
        private class insGuiziData
        {
            public Socket sck;
            public byte[] data;
        }
        private class insAppData
        {
            public Socket sck;
            public app_command data;
            public string revdata;
        }
        public Form1()
        {
            InitializeComponent();
        }


        TcpListener tcp;
        int port = 5000;
        const int PCKhed01 = 0xA5;
        const int PCKhed02 = 0xA5;
        const int Ver = 1;

        private void Form1_Load(object sender, EventArgs e)
        {
            INIFile ini = new INIFile(Application.StartupPath + @"\config.ini");
            if (pubFunc.isdebug) pubFunc.postUrl = "http://127.0.0.1/guiziServerSql.php";
            try
            {
                port = int.Parse(ini.IniReadValue("Net", "Port"));
                database = ini.IniReadValue("MySql", "db_name");
                local = ini.IniReadValue("MySql", "db_url");
                user = ini.IniReadValue("MySql", "db_user");
                password = ini.IniReadValue("MySql", "db_pwd");
                pubFunc.smsInfo.serialNO = ini.IniReadValue("SMS", "serial");
                pubFunc.smsInfo.serialPwd = ini.IniReadValue("SMS", "password");
            }
            catch {
                MessageBox.Show("读取配置文件失败!");
                Application.Exit();
            }
            helper = new SQLHelper(database, local, user, password);

            btnSend.Enabled = false;
            richTxt.ReadOnly = true;
            lstClient.Columns.Add("",1);
            lstClient.Columns.Add("IP",100);
            lstClient.Columns.Add("端口");
            lstClient.Columns.Add("柜子ID");
            lstClient.Columns.Add("心跳");
            logFile.WriteLogFile("系统启动！");
        }

        public Dictionary<Socket, pubFunc.guizi_info_class> list = new Dictionary<Socket, pubFunc.guizi_info_class>();//连接的客户端
        void Listen()
        {
            while (true)
            {
                uint dummy = 0;
                byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
                BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
                BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
                BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);
                try
                {
                    Accept(inOptionValues);
                }
                catch { }
            }
            /*
            while (true)
            {
                Socket sck = tcp.AcceptSocket();
                ThreadPool.QueueUserWorkItem(new WaitCallback(Recevie),sck);
                AddNewClient(sck);
            }
             * */
        }
        public void Accept(byte[] inOptionValues)
        {
            Socket sck = tcp.AcceptSocket();
            sck.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
            ThreadPool.QueueUserWorkItem(new WaitCallback(Recevie), sck);
            AddNewClient(sck);
        }
        public void RefreshTxT(string msg)
        {
            if (richTxt.InvokeRequired)
            {
                Action<string> act = new Action<string>(RefreshTxT);
                richTxt.Invoke(act, msg);
            }
            else
            {
                richTxt.AppendText(string.Format("{0}\t{1}\r\n",DateTime.Now.ToShortTimeString(),msg));
            }
        }

        /// <summary>
        /// 获得六位的随机数
        /// </summary>
        /// <returns></returns>
        public string get6NumRandom()
        {
            Random ro = new Random();
            int iResult;
            int iUp = 999999;
            int iDown = 100000;
            iResult = ro.Next(iDown, iUp);
            return iResult.ToString().Trim();
        }

        /// <summary>
        /// 获得三位的随机数
        /// </summary>
        /// <returns></returns>
        public string getTreeNumRandom()
        {
            Random ro = new Random();
            int iResult;
            int iUp = 999;
            int iDown = 100;
            iResult = ro.Next(iDown, iUp);
            return iResult.ToString().Trim();
        }
        private string gen_userPwd()
        {
            string pwd="";
            DateTime dateTime = DateTime.Now;
            pwd = dateTime.ToString("MMddHHmmss");
            pwd = pwd + getTreeNumRandom();
            return pwd;
        }

        public void AddNewClient(Socket sck)
        {
            pubFunc.guizi_info_class guizi_info = new pubFunc.guizi_info_class();
            guizi_info.guizi_id = "null" + getTreeNumRandom();
            list.Add(sck, guizi_info);
            IPEndPoint remoteIP = (IPEndPoint)sck.RemoteEndPoint;
            RefreshTxT(string.Format("{0}已连接",remoteIP.Address));
            BindListData(0);
        }

        public void RemoveClient(Socket sck)
        {
            IPEndPoint remoteIP = (IPEndPoint)sck.RemoteEndPoint;
            RefreshTxT(string.Format("{0}已断开连接", remoteIP.Address));
            if (list.ContainsKey(sck))
            {
                list.Remove(sck);
                BindListData(0);
            }
        }

        //n 无任何意义，仅为简化程序
        public void BindListData(int n)
        {
            if (lstClient.InvokeRequired)
            {
                Action<int> act = new Action<int>(BindListData);
                lstClient.Invoke(act, 0);
            }
            else
            {
                lstClient.Items.Clear();
                foreach (var item in list)
                {
                    ListViewItem lvItem = new ListViewItem();
                    IPEndPoint remoteIP = (IPEndPoint)item.Key.RemoteEndPoint;
                    lvItem.SubItems.Add(remoteIP.Address.ToString());
                    lvItem.SubItems.Add(remoteIP.Port.ToString());
                    lvItem.SubItems.Add(item.Value.guizi_id);
                    lvItem.SubItems.Add(item.Value.heart_on_count.ToString());
                    lstClient.Items.Add(lvItem);
                }
            }
        }

        void Recevie(object o)
        {
            Socket sck = o as Socket;
            byte[] raw_data = new byte[2048];
            try
            {
                while (true)
                {
                    Array.Clear(raw_data, 0, raw_data.Length);
                    int len = sck.Receive(raw_data);
                    //Console.WriteLine(DateTime.Now.ToString());
                    if (len > 0)
                    {
                        byte[] dataRevRaw = new byte[len];
                        Array.Copy(raw_data, 0, dataRevRaw, 0, len);
                        if (pubFunc.testmode)
                        {
                            logFile.WriteLogFile(pubFunc.HexToString(dataRevRaw));
                            //RefreshTxT(pubFunc.HexToString(dataRevRaw));
                        }
                        IPEndPoint remoteIP = (IPEndPoint)sck.RemoteEndPoint;
                        //logRawData.WriteLogFile(string.Format("接收[{0}]:{1}", remoteIP.Address, pubFunc.HexToString(dataRevRaw)));//记录原始数据
                        if (raw_data[0] == 0xA8)//来自武汉柜子的数据
                        {
                            byte[] head_data = new byte[20];
                            Array.Copy(raw_data, 0, head_data, 0, (int)20);
                            if (head_data[17] == 0x02)
                            {
                                head_data[17] = 0x03;
                                head_data[19] += 1;
                                ClientInfo sea = new ClientInfo(sck);
                                sea.SendBuffer.AddRange(head_data);
                                ThreadPool.QueueUserWorkItem(SendLoop, sea);
                                list[sck].heart_on_count++;//心跳计数加1
                                if (list[sck].heart_on_count > 10000) list[sck].heart_on_count = 0;
                                BindListData(0);//更新listview
                                //logFile.WriteLogFile(string.Format("接收到客户端{0}的消息，内容为:{1}", remoteIP.Address, pubFunc.HexToString(head_data)));
                            }
                            else if (head_data[17] == 0x01)
                            {
                                byte[] data = new byte[51];
                                Array.Copy(raw_data, 20, data, 0, (int)51);
                                int head01 = data[0];
                                int head02 = data[1];
                                int pcklen = data[2];
                                int end01 = data[49];
                                int end02 = data[50];

                                //logFile.WriteLogFile(string.Format("接收到客户端{0}的消息，内容为:{1}{2}", remoteIP.Address, pubFunc.HexToString(head_data), pubFunc.HexToString(data)));

                                //Check MagicNum and Version
                                if (head01 == 0xA5 && head02 == 0xA5 && end01 == 0x5A && end02 == 0x5A)//武汉柜子
                                {
                                    string guizi_id = new string(System.Text.Encoding.Default.GetChars(data, 3, 8));

                                    //原始数据,仅显示使用
                                    list[sck].guizi_id = guizi_id;
                                    list[sck].head_data = head_data;
                                    list[sck].heart_on_count++;//心跳计数加1
                                    if (list[sck].heart_on_count > 10000) list[sck].heart_on_count = 0;

                                    insGuiziData insGuiziPara = new insGuiziData();
                                    insGuiziPara.sck = sck;
                                    insGuiziPara.data = data;
                                    Thread thread = new Thread(new ParameterizedThreadStart(insCheck_wuhan));
                                    thread.Start(insGuiziPara);

                                    //insCheck_wuhan(sck, data);

                                    BindListData(0);//更新listview
                                }
                            }
                        }
                        else if (raw_data[0] == 0x7B)//来自APP的数据
                        {
                            string revdata = Encoding.UTF8.GetString(raw_data, 0, len);
                            app_command data = null;
                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            data = jss.Deserialize(revdata, typeof(app_command)) as app_command;
                            if (data == null)
                            {
                                return;
                            }
                            list[sck].guizi_id = "apps&pcs";

                            insAppData insAppPara = new insAppData();
                            insAppPara.sck = sck;
                            insAppPara.data = data;
                            insAppPara.revdata = revdata;
                            Thread thread = new Thread(new ParameterizedThreadStart(inscheck_app));
                            thread.Start(insAppPara);
                        }
                        else if (raw_data[0] == 0x59)//来自方氏柜子的数据
                        {
                            byte[] fy_data = new byte[len];
                            Array.Copy(raw_data, 0, fy_data, 0, len);
                            if (fy_data[0] == 0x59 && fy_data[1] == 0x47)//标识码
                            {
                                string guizi_id = new string(System.Text.Encoding.Default.GetChars(fy_data, 3, 8));
                                uint packlen = pubFunction.ReverseBytes(BitConverter.ToUInt32(fy_data, 13));
                                //原始数据,仅显示使用
                                list[sck].guizi_id = guizi_id;
                                list[sck].head_data = null;
                                list[sck].heart_on_count++;//心跳计数加1
                                if (list[sck].heart_on_count > 10000) list[sck].heart_on_count = 0;

                                insGuiziData insGuiziPara = new insGuiziData();
                                insGuiziPara.sck = sck;
                                insGuiziPara.data = fy_data;
                                Thread thread = new Thread(new ParameterizedThreadStart(insCheck_fangyuan));
                                thread.Start(insGuiziPara);

                                BindListData(0);//更新listview
                            }

                        }
                    }
                    else
                    {
                        RemoveClient(sck);
                        return;
                    }
                    Thread.Sleep(100);
                }
            }
            catch
            {
                //pubFunction.smsalert(2);//发送报警短信
                RemoveClient(sck);
            }
        }
        /************* app指令解析 *************************/
        private void inscheck_app(object obj)
        {
            insAppData insAppPara = (insAppData)obj;
            Socket sck = insAppPara.sck;
            app_command data = insAppPara.data;
            string raw_data = insAppPara.revdata;

            switch (data.command)
            {
                case "geturl"://手机在线心跳
                    {
                        app_command app_data = new app_command();
                        app_data.command = "geturl";
                        app_data.data01 = pubFunc.weburl;
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string send = jss.Serialize(app_data);
                        //应答

                        ClientInfo sea = new ClientInfo(sck);
                        byte[] send_data = Encoding.Default.GetBytes(send);//应答数据
                        sea.SendBuffer.AddRange(send_data);
                        ThreadPool.QueueUserWorkItem(SendLoop, sea);

                        break;
                    }
                case "clr_door"://清除柜子
                    {
                        logFile.WriteLogFile(string.Format("{0}APP指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "接收", data.command, "清除柜门", data.guizi_id, data.door_id, raw_data));
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.guizi_id = data.guizi_id.ToString().PadLeft(8, '0');
                        para.door_id = data.door_id.ToString().PadLeft(2, '0');
                        foreach (var item in list)
                        {
                            if (item.Value.guizi_id != para.guizi_id)
                            {
                                continue;
                            }
                            else
                            {
                                if ((pubFunction.isguiziidRight(para.guizi_id)) && (pubFunction.isdooridRight(para.door_id)))//检查柜子ID和柜门ID
                                {
                                    string sql = string.Format("select * from tab_express WHERE guizi_num = '{0}' and door_num = '{1}' ", para.guizi_id, para.door_id);
                                    DataTable dt = helper.Selectinfo(sql);
                                    if (dt == null)
                                    {
                                        pubFunction.smsalert(1, "APP清除柜子");//发送报警短信
                                        return;//无法连接数据库，返回
                                    }
                                    if (dt.Rows.Count > 0)//该柜子不为空，则清空
                                    {
                                        long gmttime = pubFunc.UNIX_TIMESTAMP(DateTime.Now);
                                        clear_door(para.guizi_id, para.door_id, gmttime, "3");//更新货品已取
                                    }
                                }
                                else
                                {
                                    logFile.WriteLogFile("无效指令");
                                }
                            }
                            break;
                        }

                        break;
                    }
                case "chg_pwd"://修改密码
                    {
                        logFile.WriteLogFile(string.Format("{0}APP指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "接收", data.command, "修改密码", data.guizi_id, "无", raw_data));
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.guizi_id = data.guizi_id.ToString().PadLeft(8, '0');
                        para.data01 = data.data01.ToString().PadLeft(1, '0');
                        para.pwd = data.data02.ToString().PadLeft(6, '0');

                        para.door_id = new string(System.Text.Encoding.Default.GetChars(new byte[] { 0x30, 0xFF }, 0, 2));
                        foreach (var item in list)
                        {
                            if (item.Value.guizi_id != para.guizi_id)
                            {
                                continue;
                            }
                            else
                            {
                                if (para.data01 == "0")//修改超级管理员密码
                                {
                                    para.door_id = new string(System.Text.Encoding.Default.GetChars(new byte[] { 0x30, 0xFF }, 0, 2));
                                }
                                else if (para.data01 == "1")//修改管理员密码
                                {
                                    para.door_id = new string(System.Text.Encoding.Default.GetChars(new byte[] { 0x31, 0xFF }, 0, 2));
                                }
                                byte[] send_data = packFunc.pack_wuhan(para.guizi_id, "I", para.door_id, null, null, null, para.pwd, item.Value.head_data);
                                ClientInfo sea = new ClientInfo(item.Key);
                                if (sea != null)
                                {
                                    if (pubFunction.isguiziidRight(para.guizi_id))//检查柜子ID
                                    {
                                        sea.SendBuffer.AddRange(send_data);
                                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "I", "修改密码", para.guizi_id, para.door_id, pubFunc.HexToString(send_data)));
                                        IPEndPoint remoteIP = (IPEndPoint)sck.RemoteEndPoint;
                                        //logRawData.WriteLogFile(string.Format("发送[{0}]:{1}", remoteIP.Address, pubFunc.HexToString(send_data)));//记录原始数据
                                        ThreadPool.QueueUserWorkItem(SendLoop, sea);
                                        Thread.Sleep(2 * 1000);//2秒后再次发送
                                        ThreadPool.QueueUserWorkItem(SendLoop, sea);
                                    }
                                    else
                                    {
                                        logFile.WriteLogFile("无效指令");
                                    }
                                }
                                else
                                {
                                    logFile.WriteLogFile(string.Format("Socket错误：{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "I", "修改密码", para.guizi_id, para.door_id, pubFunc.HexToString(send_data)));
                                }
                            }
                            break;
                        }

                        break;
                    }
                case "resendSMS"://重发短信
                    {
                        logFile.WriteLogFile(string.Format("{0}APP指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "接收", data.command, "重发短信", data.guizi_id, data.door_id, raw_data));
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.guizi_id = data.guizi_id.ToString().PadLeft(8, '0');
                        para.door_id = data.door_id.ToString().PadLeft(2, '0');
                        para.phone = data.data01.ToString();

                        string sql;
                        string xiaoqu = "", xiaoquID = "";
                        string expresscard = "", expressname = "", phone = "", pwd = "";
                        bool issendsms = false;
                        foreach (var item in list)
                        {
                            if (item.Value.guizi_id != para.guizi_id)
                            {
                                continue;
                            }
                            else
                            {
                                sql = string.Format("select province from tab_guizi where cab_name = '{0}' ", para.guizi_id);
                                DataTable dt_province = helper.Selectinfo(sql);
                                if (dt_province == null)
                                {
                                    pubFunction.smsalert(1);//发送报警短信
                                    return;//无法连接数据库，返回
                                }
                                if (dt_province.Rows.Count > 0)
                                {
                                    xiaoquID = dt_province.Rows[0]["province"].ToString();
                                    sql = string.Format("select region_name from ecs_region where region_id = '{0}' ", xiaoquID);
                                    DataTable dt_xiqoau = helper.Selectinfo(sql);
                                    if (dt_xiqoau == null)
                                    {
                                        pubFunction.smsalert(1);//发送报警短信
                                        return;//无法连接数据库，返回
                                    }
                                    if (dt_xiqoau.Rows.Count > 0)
                                        xiaoqu = dt_xiqoau.Rows[0]["region_name"].ToString();
                                    else
                                        xiaoqu = "未知";
                                }

                                sql = string.Format("select * from tab_express WHERE guizi_num = '{0}' and door_num = '{1}' and mobile = '{2}' ", para.guizi_id, para.door_id, para.phone);
                                DataTable dt_express = helper.Selectinfo(sql);
                                if (dt_express == null)
                                {
                                    pubFunction.smsalert(1);//发送报警短信
                                    return;//无法连接数据库，返回
                                }
                                if (dt_express.Rows.Count > 0)
                                {
                                    issendsms = true;
                                    expresscard = dt_express.Rows[0]["EmployeeCardID"].ToString();
                                    phone = dt_express.Rows[0]["mobile"].ToString();
                                    pwd = dt_express.Rows[0]["user_pwd"].ToString();
                                }
                                if (issendsms)
                                {
                                    //user_id为0，为快递
                                    if (dt_express.Rows[0]["user_id"].ToString().Trim() == "0")
                                    {
                                        sql = string.Format("select * from tab_member where membercard = '{0}' and card_type = '3' ", expresscard);
                                        DataTable dt_expressinc = helper.Selectinfo(sql);
                                        if (dt_expressinc == null)
                                        {
                                            pubFunction.smsalert(1);//发送报警短信
                                            return;//无法连接数据库，返回
                                        }
                                        if (dt_expressinc.Rows.Count != 0)
                                        {
                                            expressname = dt_expressinc.Rows[0]["express_inc"].ToString();
                                        }
                                        //发送短信！！！！
                                        if (!pubFunc.isdebug)
                                        {
                                            pubFunc.smsInfo.phone = phone;
                                            pubFunc.smsInfo.smsContent = string.Format("{0}尊敬的会员，您的{1}快递已配送至{2}{3}号柜,取货箱号{4}开箱验证码{5}。请及时提取！", pubFunc.compname, expressname, xiaoqu, para.guizi_id.Substring(para.guizi_id.Length - 3), para.door_id, pwd);
                                            pubFunction.sdkService.sendSMS(pubFunc.smsInfo.serialNO, pubFunc.smsInfo.serialPwd, pubFunc.smsInfo.schTime, pubFunc.smsInfo.phone.Split(new char[] { ',' }), pubFunc.smsInfo.smsContent, pubFunc.smsInfo.addSerial, "GBK", 3, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                                            logFile.WriteLogFile(string.Format("短信[{0}]:{1}", pubFunc.smsInfo.phone, pubFunc.smsInfo.smsContent));//记录原始数据
                                        }
                                    }
                                    //user_id不为0，为商店商品
                                    else
                                    {
                                        if (!pubFunc.isdebug)
                                        {
                                            //发送短信！！！！
                                            pubFunc.smsInfo.phone = phone;
                                            pubFunc.smsInfo.smsContent = string.Format("{0}尊敬的会员，您所订购货物已配送至{2}{3}号柜,取货箱号{4}开箱验证码{5}或刷会员卡取货。请及时提取，祝您购物愉快！", pubFunc.compname, dt_express.Rows[0]["express_num"].ToString(), xiaoqu, para.guizi_id.Substring(para.guizi_id.Length - 3), para.door_id, pwd);
                                            pubFunction.sdkService.sendSMS(pubFunc.smsInfo.serialNO, pubFunc.smsInfo.serialPwd, pubFunc.smsInfo.schTime, pubFunc.smsInfo.phone.Split(new char[] { ',' }), pubFunc.smsInfo.smsContent, pubFunc.smsInfo.addSerial, "GBK", 3, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                                            logFile.WriteLogFile(string.Format("短信[{0}]:{1}", pubFunc.smsInfo.phone, pubFunc.smsInfo.smsContent));//记录原始数据
                                        }
                                    }
                                }
                            }
                            break;
                        }

                        break;
                    }
                case "heart_on"://手机在线心跳
                    {
                        app_command app_data = new app_command();
                        app_data.command = "heart_on";
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string send=jss.Serialize(app_data);
                        //应答

                        ClientInfo sea = new ClientInfo(sck);
                        byte[] send_data = Encoding.Default.GetBytes(send);//应答数据
                        sea.SendBuffer.AddRange(send_data);
                        ThreadPool.QueueUserWorkItem(SendLoop, sea);

                        break;
                    }
                case "remot_open"://远程开柜功能
                    {
                        logFile.WriteLogFile(string.Format("{0}APP指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "接收", data.command, "远程开门", data.guizi_id, data.door_id, raw_data));
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.guizi_id = data.guizi_id.ToString().PadLeft(8, '0');
                        para.door_id = data.door_id.ToString().PadLeft(2, '0');

                        foreach (var item in list)
                        {
                            if (item.Value.guizi_id != para.guizi_id)
                            {
                                continue;
                            }
                            else
                            {
                                byte[] send_data = packFunc.pack_wuhan(para.guizi_id, "E", para.door_id, null, null, null, null, item.Value.head_data);
                                ClientInfo sea = new ClientInfo(item.Key);
                                if (sea != null)
                                {
                                    //远程开柜，无论如何都清柜子
                                    if ((pubFunction.isguiziidRight(para.guizi_id)) && (pubFunction.isdooridRight(para.door_id)))//检查柜子ID和柜门ID
                                    {
                                        string sql = string.Format("select * from tab_express WHERE guizi_num = '{0}' and door_num = '{1}' ", para.guizi_id, para.door_id);
                                        DataTable dt = helper.Selectinfo(sql);
                                        if (dt == null)
                                        {
                                            pubFunction.smsalert(1, "APP远程开柜_1");//发送报警短信
                                            return;//无法连接数据库，返回
                                        }
                                        if (dt.Rows.Count > 0)//该柜子不为空，则清空
                                        {
                                            long gmttime = pubFunc.UNIX_TIMESTAMP(DateTime.Now);
                                            clear_door(para.guizi_id, para.door_id, gmttime, "3");//更新货品已取
                                        }
                                        //////////////////
                                        sea.SendBuffer.AddRange(send_data);
                                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "E", "远程开柜", para.guizi_id, para.door_id, pubFunc.HexToString(send_data)));
                                        ThreadPool.QueueUserWorkItem(SendLoop, sea);
                                        IPEndPoint remoteIP = (IPEndPoint)sck.RemoteEndPoint;
                                        //logRawData.WriteLogFile(string.Format("发送[{0}]:{1}", remoteIP.Address, pubFunc.HexToString(send_data)));//记录原始数据
                                        Thread.Sleep(2 * 1000);//2秒后再次发送
                                        ThreadPool.QueueUserWorkItem(SendLoop, sea);
                                        //logRawData.WriteLogFile(string.Format("发送[{0}]:{1}", remoteIP.Address, pubFunc.HexToString(send_data)));//记录原始数据
                                    }
                                    else
                                    {
                                        logFile.WriteLogFile("无效指令");
                                    }
                                }
                                else
                                {
                                    logFile.WriteLogFile(string.Format("Socket错误：{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "E", "远程开柜", para.guizi_id, para.door_id, pubFunc.HexToString(send_data)));
                                }
                            }
                            break;
                        }


                        break;
                    }
                case "lockdoor"://锁定柜门
                    {
                        logFile.WriteLogFile(string.Format("{0}APP指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "接收", data.command, "锁定柜门", data.guizi_id, data.door_id, raw_data));
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.guizi_id = data.guizi_id.ToString().PadLeft(8, '0');
                        para.door_id = data.door_id.ToString().PadLeft(2, '0');

                        foreach (var item in list)
                        {
                            if (item.Value.guizi_id != para.guizi_id)
                            {
                                continue;
                            }
                            else
                            {
                                byte[] send_data = packFunc.pack_wuhan(para.guizi_id, "G", para.door_id, null, null, null, null, item.Value.head_data);
                                ClientInfo sea = new ClientInfo(item.Key);
                                if (sea != null)
                                {
                                    sea.SendBuffer.AddRange(send_data);
                                    ThreadPool.QueueUserWorkItem(SendLoop, sea);
                                    logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "G", "锁定柜门", para.guizi_id, para.door_id, pubFunc.HexToString(send_data)));
                                    IPEndPoint remoteIP = (IPEndPoint)sck.RemoteEndPoint;
                                    //logRawData.WriteLogFile(string.Format("发送[{0}]:{1}", remoteIP.Address, pubFunc.HexToString(send_data)));//记录原始数据
                                }
                                else
                                {
                                    logFile.WriteLogFile(string.Format("Socket错误：{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "G", "锁定柜门", para.guizi_id, para.door_id, pubFunc.HexToString(send_data)));
                                }
                            }
                            break;
                        }


                        break;
                    }
                case "sms"://企业短信管理
                    {
                        logFile.WriteLogFile(string.Format("{0}APP指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "接收", data.command, "企业短信管理", data.guizi_id, data.door_id, raw_data));
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.sck = sck;
                        para.data01 = data.data01.ToString();
                        if (para.data01 == "chargeUp")//充值
                        {
                            para.data02 = data.data02.ToString();
                            para.data03 = data.data03.ToString();
                        }

                        string sms_cmd = para.data01;
                        string data_back = "";
                        app_command app_data = new app_command();
                        app_data.command = "sms";
                        if (sms_cmd == "balance")//查询余额
                        {
                            data_back = pubFunction.sdkService.getBalance(pubFunc.smsInfo.serialNO, pubFunc.smsInfo.serialPwd).ToString();
                            app_data.data01 = "balance";
                            app_data.data02 = data_back;
                        }
                        if (sms_cmd == "EachFee")//查询单价
                        {
                            data_back = pubFunction.sdkService.getEachFee(pubFunc.smsInfo.serialNO, pubFunc.smsInfo.serialPwd).ToString();
                            app_data.data01 = "EachFee";
                            app_data.data02 = data_back;
                        }
                        if (sms_cmd == "chargeUp")//充值
                        {
                            data_back = pubFunction.sdkService.chargeUp(pubFunc.smsInfo.serialNO, pubFunc.smsInfo.serialPwd, para.data02, para.data03).ToString();
                            app_data.data01 = "chargeUp";
                            app_data.data02 = data_back;
                        }
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string send = jss.Serialize(app_data);

                        ClientInfo sea = new ClientInfo(para.sck);
                        byte[] send_data = Encoding.Default.GetBytes(send);//应答数据
                        sea.SendBuffer.AddRange(send_data);
                        ThreadPool.QueueUserWorkItem(SendLoop, sea);


                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        /**************************************/
        private void SendLoop(object data)
        {
            ClientInfo ci = data as ClientInfo;
            try
            {
                ci.Sck.Send(ci.SendBuffer.ToArray());
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                tcp = new TcpListener(new IPEndPoint(IPAddress.Any, port));
                tcp.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Thread td = new Thread(Listen);
            td.IsBackground = true;
            td.Start();
            btnStart.Enabled = false;
            btnSend.Enabled = true;
            timerStep.Enabled = true;
            timerStep.Start();

            ///////////////测试
            //.Text = Directory.GetCurrentDirectory() + "\\log\\" + DateTime.Now.ToString("yyyy-MM-dd")+ "backup";
            //timerTest.Start();
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (lstClient.SelectedIndices.Count == 0)
            {
                MessageBox.Show("请先选择一个客户端!");
            }
            else
            {
                int n = lstClient.SelectedIndices[0];
                int i = 0;
                foreach (var item in list)
                {
                    if (i != n)
                    {
                        i++;
                        continue;
                    }
                    byte[] send_data = new byte[] { 0xA8, 0x81, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x39, 0x35, 0x37, 0x37, 0x30, 0x34, 0x37, 0x32, 0x03, 0x00, 0x26 };

                    ClientInfo sea = new ClientInfo(item.Key);

                    sea.SendBuffer.AddRange(send_data);
                    ThreadPool.QueueUserWorkItem(SendLoop, sea);
                    RefreshTxT("发送消息到" + lstClient.SelectedItems[0].SubItems[1].Text);
                    break;
                }
            }
        }
        /************* 柜子指令解析 *************************/
        //方氏柜子
        private void insCheck_fangyuan(object obj)
        {
            insGuiziData insGuiziPara = (insGuiziData)obj;
            Socket sck = insGuiziPara.sck;
            byte[] data = insGuiziPara.data;

            string guizi_id = new string(System.Text.Encoding.Default.GetChars(data, 3, 8));
            guizi_id = guizi_id.Trim();
            string cmd = new string(System.Text.Encoding.Default.GetChars(data, 11, 2));
            //string door_id = new string(System.Text.Encoding.Default.GetChars(data, 12, 2));
            int gyear = packFunc.get_ushort(data, 17);//柜子的年
            int gmonth = packFunc.get_byte(data, 19);//柜子的月
            int gday = packFunc.get_byte(data, 20);//柜子的日
            int ghour = packFunc.get_byte(data, 21);//柜子的时
            int gminute = packFunc.get_byte(data, 22);//柜子的分
            int gsecond = packFunc.get_byte(data, 23);//柜子的秒
            //string gyear = new string(System.Text.Encoding.Default.GetChars(data, 17, 4));
            //string gmonth = new string(System.Text.Encoding.Default.GetChars(data, 21, 2));
            //string gday = new string(System.Text.Encoding.Default.GetChars(data, 23, 2));
            //string ghour = new string(System.Text.Encoding.Default.GetChars(data, 25, 2));
            //string gminute = new string(System.Text.Encoding.Default.GetChars(data, 27, 2));
            //string gsecond = new string(System.Text.Encoding.Default.GetChars(data, 29, 2));
            switch (cmd)
            {
                case "01"://开机指令，返回02
                    {
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],code:{4}", "接收", "01", "柜子开机", guizi_id, pubFunc.HexToString(data)));

                        byte[] sendData = packFunc.pack_fangyuan(guizi_id, "02", new byte[] { 0x00 });
                        ClientInfo sea = new ClientInfo(sck);
                        sea.SendBuffer.AddRange(sendData);
                        ThreadPool.QueueUserWorkItem(SendLoop, sea);
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],code:{4}", "发送", "02", "柜子开机反馈", guizi_id,  pubFunc.HexToString(sendData)));
                        break;
                    }
                case "03"://定时在线指令，返回04
                    {
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],code:{4}", "接收", "03", "柜子定时在线", guizi_id, pubFunc.HexToString(data)));

                        byte[] sendData = packFunc.pack_fangyuan(guizi_id, "04", new byte[] { 0x00 });
                        ClientInfo sea = new ClientInfo(sck);
                        sea.SendBuffer.AddRange(sendData);
                        ThreadPool.QueueUserWorkItem(SendLoop, sea);
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],code:{4}", "发送", "04", "柜子定时在线反馈", guizi_id, pubFunc.HexToString(sendData)));
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        //武汉柜子
        private void insCheck_wuhan(object obj)
        {
            insGuiziData insGuiziPara = (insGuiziData)obj;
            Socket sck = insGuiziPara.sck;
            byte[] data = insGuiziPara.data;

            string sql = "";
            string cabPwd = "";
            string xiaoqu = "";
            DataTable dt = null;

            //int countTimeOut = 0;

            string guizi_id = new string(System.Text.Encoding.Default.GetChars(data, 3, 8));
            guizi_id = guizi_id.Trim();
            string cmd = new string(System.Text.Encoding.Default.GetChars(data, 11, 1));
            string door_id = new string(System.Text.Encoding.Default.GetChars(data, 12, 2));
            string status = new string(System.Text.Encoding.Default.GetChars(data, 14, 1));
            string send_data = new string(System.Text.Encoding.Default.GetChars(data, 15, 20));//数据域
            string user_id = new string(System.Text.Encoding.Default.GetChars(data, 35, 8));
            string pwd = new string(System.Text.Encoding.Default.GetChars(data, 43, 6));
            switch (cmd)
            {
                case "A"://开机指令
                    {
                        int door_count = Convert.ToInt16(door_id);
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "A", "开机指令", guizi_id, "无", pubFunc.HexToString(list[sck].head_data), pubFunc.HexToString(data)));
                        //应答
                        if (!pubFunction.isguiziidRight(guizi_id))//判断柜子ID正确
                        {
                            logFile.WriteLogFile("无效指令");
                        }
                        break;
                    }
                case "B":
                    {
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "B", "运货员开箱存货", guizi_id, door_id, pubFunc.HexToString(list[sck].head_data), pubFunc.HexToString(data)));
                        //应答
                        if ((pubFunction.isguiziidRight(guizi_id)) && (pubFunction.isdooridRight(door_id)))//检查柜子ID和柜门ID
                        {
                            postvars = new System.Collections.Specialized.NameValueCollection();
                            wc = new WebClient();
                            postvars.Add("class", "barcode_input");
                            postvars.Add("cab_num", guizi_id);
                            postvars.Add("door_num", door_id);
                            postvars.Add("barcode", send_data);
                            byte[] byremoteinfo = wc.UploadValues(pubFunc.postUrl, "POST", postvars);
                            string sremoteinfo = System.Text.Encoding.UTF8.GetString(byremoteinfo);
                            sremoteinfo = sremoteinfo.Replace("﻿", "");//去除多余物
                            sremoteinfo = sremoteinfo.Replace("\n", "");//去除多余物
                            /***********************************************/
                            /****新网站返回的不是序列，与旧网站有区别***/
                            /************************************************/
                            JObject ja = (JObject)JsonConvert.DeserializeObject(sremoteinfo);
                            string ja1a = ja["result"].ToString();
                            if (ja1a == "0")//无效条码
                            {
                                //byte[] sendData = packFunc.pack_wuhan(guizi_id, "C", door_id, "0", send_data, null, null, list[sck].head_data);
                                //ClientInfo sea = new ClientInfo(sck);
                                //sea.SendBuffer.AddRange(sendData);
                                //ThreadPool.QueueUserWorkItem(SendLoop, sea);
                                //logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "C", "运货员开箱存货订单无效", guizi_id, door_id, pubFunc.HexToString(sendData)));
                                logFile.WriteLogFile("运货员开箱存货订单无效");
                                break;
                            }
                            else if (ja1a == "1")//新条码
                            {
                                byte[] sendData = packFunc.pack_wuhan(guizi_id, "C", door_id, "1", send_data, pubFunc.card10to8(ja["iccard"].ToString()), ja["user_pwd"].ToString(), list[sck].head_data);
                                ClientInfo sea = new ClientInfo(sck);
                                sea.SendBuffer.AddRange(sendData);
                                ThreadPool.QueueUserWorkItem(SendLoop, sea);
                                logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "C", "运货员开箱存货反馈", guizi_id, door_id, pubFunc.HexToString(sendData)));
                                if (!pubFunc.isdebug)
                                {
                                    pubFunc.smsInfo.phone = ja["mobile_phone"].ToString();
                                    pubFunc.smsInfo.smsContent = string.Format("{0}尊敬的会员，您所订购货物已配送至{2}{3}号柜,取货箱号{4}开箱验证码{5}或刷会员卡取货。请及时提取，祝您购物愉快！", pubFunc.compname, send_data, ja["xiaoqu"].ToString(), guizi_id.Substring(guizi_id.Length - 3), door_id, ja["user_pwd"].ToString());
                                    pubFunction.sdkService.sendSMS(pubFunc.smsInfo.serialNO, pubFunc.smsInfo.serialPwd, pubFunc.smsInfo.schTime, pubFunc.smsInfo.phone.Split(new char[] { ',' }), pubFunc.smsInfo.smsContent, pubFunc.smsInfo.addSerial, "GBK", 3, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                                    logFile.WriteLogFile(string.Format("短信[{0}]:{1}", pubFunc.smsInfo.phone, pubFunc.smsInfo.smsContent));//记录原始数据
                                }
                            }
                            else if (ja1a == "2")//已有条码
                            {
                                byte[] sendData = packFunc.pack_wuhan(guizi_id, "C", door_id, "1", send_data, pubFunc.card10to8(ja["iccard"].ToString()), ja["user_pwd"].ToString(), list[sck].head_data);
                                ClientInfo sea = new ClientInfo(sck);
                                sea.SendBuffer.AddRange(sendData);
                                ThreadPool.QueueUserWorkItem(SendLoop, sea);
                                logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "C", "运货员开箱存货反馈", guizi_id, door_id, pubFunc.HexToString(sendData)));
                            }
                        }
                        else
                        {
                            logFile.WriteLogFile("无效指令");
                        }
                        break;
                    }
                case "D":
                    {
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "D", "客户开箱指令", guizi_id, door_id, pubFunc.HexToString(list[sck].head_data), pubFunc.HexToString(data)));
                        //应答
                        if ((pubFunction.isguiziidRight(guizi_id)) && (pubFunction.isdooridRight(door_id)))//检查柜子ID和柜门ID
                        {
                            postvars = new System.Collections.Specialized.NameValueCollection();
                            wc = new WebClient();
                            postvars.Add("class", "userOpen");
                            postvars.Add("cab_num", guizi_id);
                            postvars.Add("door_num", door_id);
                            wc.UploadValues(pubFunc.postUrl, "POST", postvars);
                        }
                        else
                        {
                            logFile.WriteLogFile("无效指令");
                        }
                        /****
                        insWuhanPara para = new insWuhanPara();
                        para.sck = sck;
                        para.guizi_id = guizi_id;
                        para.door_id = door_id;
                        para.member_id = user_id;
                        para.pwd = pwd;

                        if ((pubFunction.isguiziidRight(para.guizi_id)) && (pubFunction.isdooridRight(para.door_id)))//检查柜子ID和柜门ID
                        {
                            long gmttime = pubFunc.UNIX_TIMESTAMP(DateTime.Now);
                            clear_door(para.guizi_id, para.door_id, gmttime, "1");
                        }
                        else
                        {
                            logFile.WriteLogFile("无效指令");
                        }
                        ****/
                        break;
                    }
                case "F":
                    {
                        app_command app_data = new app_command();
                        app_data.command = "remote_opendoor";
                        if (status == "1")//远程开门失败
                        {
                            logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "F", "远程开门失败", guizi_id, door_id, pubFunc.HexToString(list[sck].head_data), pubFunc.HexToString(data)));
                            app_data.data01 = "fail";
                        }
                        else if (status == "0")//远程开门成功
                        {
                            logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "F", "远程开门成功", guizi_id, door_id, pubFunc.HexToString(list[sck].head_data), pubFunc.HexToString(data)));
                            app_data.data01 = "success";
                        }
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string send = jss.Serialize(app_data);
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.sck = sck;
                        para.data01 = send;

                        foreach (var item in list)
                        {
                            if (item.Value.guizi_id != "apps&pcs")
                            {
                                continue;
                            }
                            else
                            {
                                ClientInfo sea = new ClientInfo(item.Key);
                                byte[] sendData = Encoding.Default.GetBytes(para.data01);//应答数据
                                sea.SendBuffer.AddRange(sendData);
                                ThreadPool.QueueUserWorkItem(SendLoop, sea);
                            }
                        }

                        break;
                    }
                case "J"://修改密码返回
                    {
                        app_command app_data = new app_command();
                        app_data.command = "chg_pwd";
                        if (status == "1")//设置成功！
                        {
                            logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "J", "修改密码成功", guizi_id, "无", pubFunc.HexToString(list[sck].head_data), pubFunc.HexToString(data)));
                            app_data.data01 = "success";
                        }
                        else if (status == "0")//设置失败！
                        {
                            logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "J", "修改密码失败", guizi_id, "无", pubFunc.HexToString(list[sck].head_data), pubFunc.HexToString(data)));
                            app_data.data01 = "fail";
                        }
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        string send = jss.Serialize(app_data);
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.sck = sck;
                        para.data01 = send;

                        foreach (var item in list)
                        {
                            if (item.Value.guizi_id != "apps&pcs")
                            {
                                continue;
                            }
                            else
                            {
                                ClientInfo sea = new ClientInfo(item.Key);
                                byte[] sendData = Encoding.Default.GetBytes(para.data01);//应答数据
                                sea.SendBuffer.AddRange(sendData);
                                ThreadPool.QueueUserWorkItem(SendLoop, sea);
                            }
                        }

                        break;
                    }
                case "H":
                    {
                        if (status == "0")
                        {
                            MessageBox.Show(string.Format("柜子:{0},柜门:{1} 锁定失败！", guizi_id, door_id));
                        }
                        else if (status == "1")
                        {
                            MessageBox.Show(string.Format("柜子:{0},柜门:{1} 锁定成功！", guizi_id, door_id));
                        }
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "H", "锁定柜门应答", guizi_id, door_id, pubFunc.HexToString(list[sck].head_data),pubFunc.HexToString(data)));
                        break;
                    }
                case "K"://柜子在线指令 
                    {
                        //logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "K", "在线心跳", guizi_id, "无",pubFunc.HexToString(list[sck].head_data), pubFunc.HexToString(data)));
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.sck = sck;
                        para.guizi_id = guizi_id;

                        if (pubFunction.isguiziidRight(para.guizi_id))//判断柜子ID正确
                        {
                            postvars = new System.Collections.Specialized.NameValueCollection();
                            wc = new WebClient();
                            postvars.Add("class", "online");
                            postvars.Add("data", para.guizi_id);
                            wc.UploadValues(pubFunc.postUrl, "POST", postvars);
                            /*
                            if (!is_guizi_reged)
                            {
                                sql = string.Format("insert into tab_guizi (cab_name,amount_all,used,locked,country,province) values ( '{0}', '{1}' ,'0','0','3410','3409') ", para.guizi_id, "16");
                                if (helper.AddDelUpdate(sql) == -1)//数据库访问出错
                                {
                                    pubFunction.smsalert(1, "柜子在线指令应答_1");//发送报警短信
                                    break;
                                }
                            }
                            else
                            {
                                sql = string.Format("update tab_guizi set cab_status = '0' WHERE  cab_name = '{0}' ", para.guizi_id);
                                if (helper.AddDelUpdate(sql) == -1)//数据库访问出错
                                {
                                    pubFunction.smsalert(1, "柜子在线指令应答_2");//发送报警短信
                                    break;
                                }
                            }
                             * */
                        }
                        else
                        {
                            logFile.WriteLogFile("无效指令");
                        }

                        break;
                    }
                case "L"://下位机获取验证码指令
                    {
                        string phone = new string(System.Text.Encoding.Default.GetChars(data, 15, 11));
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "L", "下位机获取验证码", guizi_id,door_id,pubFunc.HexToString(list[sck].head_data), pubFunc.HexToString(data)));
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.sck = sck;
                        para.guizi_id = guizi_id;
                        para.door_id = door_id;
                        para.phone = phone;

                        if ((pubFunction.isguiziidRight(para.guizi_id)) && (pubFunction.isdooridRight(para.door_id)) && (pubFunction.isphoneRight(para.phone)))//检查柜子ID和柜门ID
                        {
                            long gmttime = pubFunc.UNIX_TIMESTAMP(DateTime.Now);
                            sql = string.Format("select user_pwd from tab_express WHERE mobile = '{0}' and guizi_num = '{1}' and door_num = '{2}' ", para.phone, para.guizi_id, para.door_id);
                            dt = helper.Selectinfo(sql);
                            if (dt == null)
                            {
                                pubFunction.smsalert(1, "服务器发送验证码指令");//发送报警短信
                                break;//无法连接数据库，返回
                            }
                            if (dt.Rows.Count > 0)
                            {
                                cabPwd = dt.Rows[0]["user_pwd"].ToString();

                                byte[] sendData = packFunc.pack_wuhan(para.guizi_id, "M", para.door_id, null, para.phone, null, cabPwd, list[para.sck].head_data);

                                ClientInfo sea = new ClientInfo(para.sck);
                                sea.SendBuffer.AddRange(sendData);
                                ThreadPool.QueueUserWorkItem(SendLoop, sea);

                                logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "M", "服务器发送验证码指令", para.guizi_id, para.door_id, pubFunc.HexToString(sendData)));
                                IPEndPoint remoteIP = (IPEndPoint)para.sck.RemoteEndPoint;
                                //logRawData.WriteLogFile(string.Format("发送[{0}]:{1}", remoteIP.Address, pubFunc.HexToString(sendData)));//记录原始数据
                            }
                        }
                        else
                        {
                            logFile.WriteLogFile("无效指令");
                        }

                        break;
                    }
                case "N"://寄件人开箱指令
                    {
                        int express_id_len = 11;//快递公司ID长度
                        string express_id = new string(System.Text.Encoding.Default.GetChars(data, 15, express_id_len));
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "N", "寄件人和投递员开箱", guizi_id, door_id,pubFunc.HexToString(list[sck].head_data), pubFunc.HexToString(data)));
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.sck = sck;
                        para.guizi_id = guizi_id;
                        para.door_id = door_id;
                        para.express_id = express_id;

                        if ((pubFunction.isguiziidRight(para.guizi_id)) && (pubFunction.isdooridRight(para.door_id)))//检查柜子ID和柜门ID
                        {
                            long gmttime = pubFunc.UNIX_TIMESTAMP(DateTime.Now);

                            sql = string.Format("insert into tab_express (express_num,guizi_num,door_num,EmployeeCardID,store_time) VALUES('{0}','{1}','{2}','{3}','{4}') ",
                                "", para.guizi_id, para.door_id, para.express_id, gmttime);
                            if (helper.AddDelUpdate(sql) == -1)//数据库访问出错
                            {
                                pubFunction.smsalert(1, "寄件人开箱指令应答_1");//发送报警短信
                                break;
                            }

                            //读取柜子used，加1
                            sql = string.Format("select used from tab_guizi WHERE  cab_name = '{0}' ", para.guizi_id);
                            dt = helper.Selectinfo(sql);
                            if (dt == null)
                            {
                                pubFunction.smsalert(1, "寄件人开箱指令应答_2");//发送报警短信
                                break;//无法连接数据库，返回
                            }
                            if (dt.Rows.Count > 0)
                            {
                                int door_used = Convert.ToInt32(dt.Rows[0]["used"]);
                                door_used++;
                                sql = string.Format("update tab_guizi set used = '{0}' WHERE  cab_name = '{1}' ", door_used, para.guizi_id);
                                if (helper.AddDelUpdate(sql) == -1)//数据库访问出错
                                {
                                    pubFunction.smsalert(1, "寄件人开箱指令应答_3");//发送报警短信
                                    break;
                                }
                            }
                        }
                        else
                        {
                            logFile.WriteLogFile("无效指令");
                        }

                        break;
                    }
                case "P"://投递员IC卡认证
                    {
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "P", "投递员IC卡认证", guizi_id, "无",pubFunc.HexToString(list[sck].head_data), pubFunc.HexToString(data)));
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.sck = sck;
                        para.guizi_id = guizi_id;
                        para.door_id = door_id;
                        para.member_id = user_id;

                        if (pubFunction.isguiziidRight(para.guizi_id))//检查柜子ID
                        {
                            string express_inc = "";
                            string statusSend = new string(System.Text.Encoding.Default.GetChars(new byte[] { 0xFF }, 0, 1));
                            sql = string.Format("select * from tab_member where membercard = '{0}' and card_type = '3' ", para.member_id);
                            dt = helper.Selectinfo(sql);
                            if (dt == null)
                            {
                                pubFunction.smsalert(1, "投递员IC卡认证应答_1");//发送报警短信
                                break;//无法连接数据库，返回
                            }
                            if (dt.Rows.Count != 0)
                            {
                                express_inc = dt.Rows[0]["phone"].ToString();
                                //expressname = dt.Rows[0]["express_inc"].ToString();
                                if (express_list.ContainsKey(para.guizi_id))
                                {
                                    express_list[para.guizi_id] = para.member_id;
                                }
                                else
                                {
                                    express_list.Add(para.guizi_id, para.member_id);
                                }
                            }
                            else//卡号没有注册
                            {
                                statusSend = new string(System.Text.Encoding.Default.GetChars(new byte[] { 0x3F }, 0, 1));
                                express_inc = null;
                            }
                            byte[] sendData = packFunc.pack_wuhan(para.guizi_id, "Q", para.door_id, statusSend, express_inc, null, null, list[para.sck].head_data);

                            ClientInfo sea = new ClientInfo(para.sck);
                            sea.SendBuffer.AddRange(sendData);
                            ThreadPool.QueueUserWorkItem(SendLoop, sea);

                            logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "Q", "服务器返回认证指令", para.guizi_id, para.door_id, pubFunc.HexToString(sendData)));
                            IPEndPoint remoteIP = (IPEndPoint)para.sck.RemoteEndPoint;
                            //logRawData.WriteLogFile(string.Format("发送[{0}]:{1}", remoteIP.Address, pubFunc.HexToString(sendData)));//记录原始数据
                        }
                        else
                        {
                            logFile.WriteLogFile("无效指令");
                        }

                        break;
                    }
                case "R"://投递员开箱指令
                    {
                        string phone = new string(System.Text.Encoding.Default.GetChars(data, 15, 11));
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "R", "投递员开箱指令", guizi_id, door_id, pubFunc.HexToString(list[sck].head_data),pubFunc.HexToString(data)));
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.sck = sck;
                        para.guizi_id = guizi_id;
                        para.door_id = door_id;
                        para.phone = phone;

                        if ((pubFunction.isguiziidRight(para.guizi_id)) && (pubFunction.isdooridRight(para.door_id)) && (pubFunction.isphoneRight(para.phone)))//检查柜子ID和柜门ID
                        {
                            long gmttime = pubFunc.UNIX_TIMESTAMP(DateTime.Now);
                            cabPwd = get6NumRandom();//生成随机密码
                            xiaoqu = "";
                            string xiaoquID = "";
                            string expresscard = "", expressname = "";

                            sql = string.Format("select * from tab_express where guizi_num = '{0}' and door_num ='{1}' and mobile='{2}' ", para.guizi_id, para.door_id, para.phone);
                            DataTable dt_express = helper.Selectinfo(sql);
                            if (dt_express == null)
                            {
                                pubFunction.smsalert(1, "投递员开箱应答_1");//发送报警短信
                                break;//无法连接数据库，返回
                            }
                            if (dt_express.Rows.Count == 0)//在数据库中没有
                            {
                                if (express_list.ContainsKey(para.guizi_id))
                                {
                                    expresscard = express_list[para.guizi_id];
                                    sql = string.Format("select * from tab_member where membercard = '{0}' and card_type = '3' ", expresscard);
                                    DataTable dt_expressinc = helper.Selectinfo(sql);
                                    if (dt_expressinc == null)
                                    {
                                        pubFunction.smsalert(1, "投递员开箱应答_2");//发送报警短信
                                        break;//无法连接数据库，返回
                                    }
                                    if (dt_expressinc.Rows.Count != 0)
                                    {
                                        expressname = dt_expressinc.Rows[0]["express_inc"].ToString();
                                    }
                                }
                                else
                                {
                                    expresscard = "";
                                    expressname = "";
                                }
                                sql = string.Format("insert into tab_express (express_num,guizi_num,door_num,EmployeeCardID,store_time,mobile,user_pwd ) VALUES ( '{0}','{1}','{2}','{3}','{4}','{5}','{6}' ) ",
                                   "", para.guizi_id, para.door_id, expresscard, gmttime, para.phone, cabPwd);
                                if (helper.AddDelUpdate(sql) == -1)//数据库访问出错
                                {
                                    pubFunction.smsalert(1, "投递员开箱应答_3");//发送报警短信
                                    break;
                                }

                                sql = string.Format("select province from tab_guizi where cab_name = '{0}' ", para.guizi_id);
                                DataTable dt_province = helper.Selectinfo(sql);
                                if (dt_province == null)
                                {
                                    pubFunction.smsalert(1, "投递员开箱应答_4");//发送报警短信
                                    break;//无法连接数据库，返回
                                }
                                if (dt_province.Rows.Count > 0)
                                {
                                    xiaoquID = dt_province.Rows[0]["province"].ToString();
                                    sql = string.Format("select region_name from ecs_region where region_id = '{0}' ", xiaoquID);
                                    DataTable dt_xiaoqu = helper.Selectinfo(sql);
                                    if (dt_xiaoqu == null)
                                    {
                                        pubFunction.smsalert(1, "投递员开箱应答_5");//发送报警短信
                                        break;//无法连接数据库，返回
                                    }
                                    if (dt_xiaoqu.Rows.Count > 0)
                                        xiaoqu = dt_xiaoqu.Rows[0]["region_name"].ToString();
                                    else
                                        xiaoqu = "未知";
                                }
                                //发送短信！！！！
                                if (!pubFunc.isdebug)
                                {
                                    pubFunc.smsInfo.phone = para.phone;
                                    pubFunc.smsInfo.smsContent = string.Format("{0}尊敬的会员，您的{1}快递已配送至{2}{3}号柜,取货箱号{4}开箱验证码{5}。请及时提取！", pubFunc.compname, expressname, xiaoqu, para.guizi_id, para.door_id, cabPwd);
                                    pubFunction.sdkService.sendSMS(pubFunc.smsInfo.serialNO, pubFunc.smsInfo.serialPwd, pubFunc.smsInfo.schTime, pubFunc.smsInfo.phone.Split(new char[] { ',' }), pubFunc.smsInfo.smsContent, pubFunc.smsInfo.addSerial, "GBK", 3, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                                    logFile.WriteLogFile(string.Format("短信[{0}]:{1}", pubFunc.smsInfo.phone, pubFunc.smsInfo.smsContent));//记录原始数据
                                }

                                //读取柜子used，加1
                                sql = string.Format("select used from tab_guizi WHERE  cab_name = '{0}' ", para.guizi_id);
                                DataTable dt_used = helper.Selectinfo(sql);
                                if (dt_used == null)
                                {
                                    pubFunction.smsalert(1, "投递员开箱应答_6");//发送报警短信
                                    break;//无法连接数据库，返回
                                }
                                if (dt_used.Rows.Count > 0)
                                {
                                    int door_used = Convert.ToInt32(dt_used.Rows[0]["used"]);
                                    door_used++;
                                    sql = string.Format("update tab_guizi set used = '{0}' WHERE  cab_name = '{1}' ", door_used, para.guizi_id);
                                    if (helper.AddDelUpdate(sql) == -1)//数据库访问出错
                                    {
                                        pubFunction.smsalert(1, "投递员开箱应答_7");//发送报警短信
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                cabPwd = dt_express.Rows[0]["user_pwd"].ToString().Trim();
                            }
                            byte[] sendData = packFunc.pack_wuhan(para.guizi_id, "C", para.door_id, "31", para.phone, null, cabPwd, list[para.sck].head_data);

                            ClientInfo sea = new ClientInfo(para.sck);
                            sea.SendBuffer.AddRange(sendData);
                            ThreadPool.QueueUserWorkItem(SendLoop, sea);
                            IPEndPoint remoteIP = (IPEndPoint)para.sck.RemoteEndPoint;
                            //logRawData.WriteLogFile(string.Format("发送[{0}]:{1}", remoteIP.Address, pubFunc.HexToString(sendData)));//记录原始数据
                            Thread.Sleep(2 * 1000);//2秒后再次发送
                            ThreadPool.QueueUserWorkItem(SendLoop, sea);
                            logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "C", "运货员开箱应答", para.guizi_id, para.door_id, pubFunc.HexToString(sendData)));
                            //logRawData.WriteLogFile(string.Format("发送[{0}]:{1}", remoteIP.Address, pubFunc.HexToString(sendData)));//记录原始数据
                        }
                        else
                        {
                            logFile.WriteLogFile("无效指令");
                        }

                        break;
                    }
                case "S"://客户取货指令
                    {
                        string phone = new string(System.Text.Encoding.Default.GetChars(data, 15, 11));
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "S", "客户取货指令", guizi_id, door_id,pubFunc.HexToString(list[sck].head_data), pubFunc.HexToString(data)));
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.sck = sck;
                        para.guizi_id = guizi_id;
                        para.door_id = door_id;
                        para.member_id = user_id;
                        para.pwd = pwd;
                        para.phone = phone;

                        if ((pubFunction.isguiziidRight(para.guizi_id)) && (pubFunction.isdooridRight(para.door_id)))//检查柜子ID和柜门ID
                        {
                            long gmttime = pubFunc.UNIX_TIMESTAMP(DateTime.Now);
                            clear_door(para.guizi_id, para.door_id, gmttime, "1");
                        }
                        else
                        {
                            logFile.WriteLogFile("无效指令");
                        }

                        break;
                    }
                case "T"://投递员取货指令
                    {
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}{6}", "接收", "T", "投递员取货指令", guizi_id, door_id,pubFunc.HexToString(list[sck].head_data), pubFunc.HexToString(data)));
                        //应答
                        insWuhanPara para = new insWuhanPara();
                        para.sck = sck;
                        para.guizi_id = guizi_id;
                        para.door_id = door_id;
                        para.member_id = user_id;

                        if ((pubFunction.isguiziidRight(para.guizi_id)) && (pubFunction.isdooridRight(para.door_id)))//检查柜子ID和柜门ID
                        {
                            long gmttime = pubFunc.UNIX_TIMESTAMP(DateTime.Now);
                            clear_door(para.guizi_id, para.door_id, gmttime, "1");
                        }
                        else
                        {
                            logFile.WriteLogFile("无效指令");
                        }

                        break;
                    }
            }
        }
        /**************************************/

        private void btnControl_Click(object sender, EventArgs e)
        {
            if (this.txtAdminUser.Text.Trim() == "")
            {
                MessageBox.Show("用户名不能为空！");
            }
            else if (this.txtAdminPwd.Text.Trim() == "")
            {
                MessageBox.Show("密码不能为空！");
            }
            else
            {
                string adminUser = this.txtAdminUser.Text.Trim();
                string adminPwd = "";
                string adminSalt = "";
                /***从数据库取登陆值***/
                string sql = string.Format("select * from ecs_admin_user where user_name = '{0}' ", adminUser);
                DataTable dt = helper.Selectinfo(sql);
                if (dt == null)
                {
                    pubFunction.smsalert(1,"管理员登陆");//发送报警短信
                    return;//无法连接数据库，返回
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //从数据库得到 字段的值
                    adminPwd = (string)dt.Rows[i]["password"];
                    adminSalt = (string)dt.Rows[i]["ec_salt"];

                }
                /**********/
                byte[] input1 = Encoding.Default.GetBytes(this.txtAdminPwd.Text.Trim());
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output1 = md5.ComputeHash(input1);
                string result1 =(BitConverter.ToString(output1).Replace("-", "")).ToLower();
                result1 += adminSalt;
                byte[] input2 = Encoding.Default.GetBytes(result1);
                byte[] output2 = md5.ComputeHash(input2);
                string result2 = (BitConverter.ToString(output2).Replace("-", "")).ToLower();
                if (result2 != adminPwd)
                {
                    MessageBox.Show("登陆失败！");
                    return;
                }
                else
                {
                    Form2 fm2 = new Form2();
                    fm2.list = list;
                    fm2.Show();
                    txtAdminPwd.Text = "";
                }
            }
        }

        private void txtAdminPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)  //13表示回车键
                btnControl.PerformClick();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pubFunc.testmode)
            {
                pubFunc.testmode = false;
            }
            else
            {
                pubFunc.testmode = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            logFile.WriteLogFile("系统关闭！");
        }

        [Serializable]
        public class app_command
        {
            public string command { get; set; }
            public string guizi_id { get; set; }
            public string door_id { get; set; }
            public string status { get; set; }
            public string data01 { get; set; }
            public string data02 { get; set; }
            public string data03 { get; set; }
        }

        //定时程序，定时刷新柜子在线状态
        private void timerStep_Tick(object sender, EventArgs e)
        {
            List<string> guiziNum=new List<string>();
            foreach (KeyValuePair<Socket, pubFunc.guizi_info_class> item_guizi in list)
            {
                guiziNum.Add(item_guizi.Value.guizi_id);
            }
            String[] guiziNumstr = guiziNum.ToArray();
            string guiziserial = string.Join(",", guiziNumstr);
            postvars = new System.Collections.Specialized.NameValueCollection();
            wc = new WebClient();
            postvars.Add("class", "checkOnline");
            postvars.Add("data", guiziserial);
            wc.UploadValues(pubFunc.postUrl, "POST", postvars);
        }
        private void btnSmsReg_Click(object sender, EventArgs e)
        {
            Form3 fm3 = new Form3();
            fm3.Show();
        }
        private void clear_door(string guizi, string door, long gmttime, string type)//更新货品已取  type 1:用户取；2:回收；3:管理员开柜
        {
            //int countTimeOut = 0;
            string sql = string.Format("select express_num from  tab_express  WHERE  guizi_num = '{0}' and  door_num='{1}' ",
                guizi, door);
            DataTable dt = helper.Selectinfo(sql);
            if (dt == null)
            {
                //Thread.Sleep(100);
                //dt = helper.Selectinfo(sql);
                //countTimeOut++;
                //if (countTimeOut > 10) return;
                pubFunction.smsalert(1, "清柜门_1a");//发送报警短信
                return;//无法连接数据库，返回
            }
            if (dt.Rows[0]["express_num"].ToString().Trim() != "")
            {
                string order_sn = dt.Rows[0]["express_num"].ToString().Trim();
                sql = string.Format("update ecs_order_info set shipping_status='2' WHERE  order_sn = '{0}' ",   order_sn);
                if (helper.AddDelUpdate(sql) == -1)//数据库访问出错
                {
                    pubFunction.smsalert(1, "清柜门_3a");//发送报警短信
                    return;
                }
            }
            //int recordCount = dt.Rows.Count;
            //更新货品已取
            sql = string.Format("INSERT  INTO  tab_expresslog  (user_id, express_num, guizi_num, door_num,user_pwd,mobile,EmployeeCardID,store_time  )   SELECT user_id, express_num, guizi_num, door_num,user_pwd,mobile,EmployeeCardID,store_time   FROM  tab_express  WHERE  guizi_num = '{0}' and  door_num='{1}' ",
                guizi, door);
            if (helper.AddDelUpdate(sql) == -1)//数据库访问出错
            {
                pubFunction.smsalert(1,"清柜门_1");//发送报警短信
                return;
            }
            sql = string.Format("update tab_expresslog set take_time = '{0}',take_type = '{1}' order by express_id desc limit 1 ", gmttime,type);//更新取货时间
            if (helper.AddDelUpdate(sql) == -1)//数据库访问出错
            {
                pubFunction.smsalert(1,"清柜门_2");//发送报警短信
                return;
            }
            sql = string.Format("delete  from  tab_express WHERE  guizi_num = '{0}' and  door_num='{1}' ",
                guizi, door);
            if (helper.AddDelUpdate(sql) == -1)//数据库访问出错
            {
                pubFunction.smsalert(1,"清柜门_3");//发送报警短信
                return;
            }
            //读取柜子used，减1
            sql = string.Format("select used from tab_guizi WHERE  cab_name = '{0}' ", guizi);
            dt = helper.Selectinfo(sql);
            if (dt == null)
            {
                pubFunction.smsalert(1,"清柜门_4");//发送报警短信
                return;//无法连接数据库，返回
            }
            if (dt.Rows.Count > 0)
            {
                int door_used = Convert.ToInt32(dt.Rows[0]["used"]);
                door_used--;//= recordCount;
                sql = string.Format("update tab_guizi set used = '{0}' WHERE  cab_name = '{1}' ", door_used, guizi);
                if (helper.AddDelUpdate(sql) == -1)//数据库访问出错
                {
                    pubFunction.smsalert(1,"清柜门_5");//发送报警短信
                    return;
                }
            }
        }

        private void button_setweburl_Click(object sender, EventArgs e)
        {
            pubFunc.weburl = textBox_weburl.Text.Trim().ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int test=0;
            test = packFunc.get_int(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 }, 0);
            MessageBox.Show(test.ToString());
        }
    }
}
