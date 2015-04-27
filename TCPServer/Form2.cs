﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace guiziTCPfangyuan
{
    public partial class Form2 : Form
    {
        public Dictionary<Socket, pubFunc.guizi_info_class> list = new Dictionary<Socket, pubFunc.guizi_info_class>();
        packageFunc packFunc = new packageFunc();
        LogClass logFile = new LogClass();
        
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            foreach (var item in list)
            {
                cbCablst.Items.Add(item.Value.guizi_id);
            }
        }
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

        private void btnOpenDoor_Click(object sender, EventArgs e)//远程开柜功能
        {
            if (cbCablst.Text == "")
            {
                MessageBox.Show("请先选择一个柜子!");
            }
            else if (cbOpDoorID.Text.Trim().Length == 0)
            {
                MessageBox.Show("柜子号不能为空!");
            }
            else
            {
                foreach (var item in list)
                {
                    if (item.Value.guizi_id != cbCablst.Text)
                    {
                        continue;
                    }
                    else
                    {
                        string cabID = item.Value.guizi_id.ToString().PadLeft(8, '0');
                        string doorID = cbOpDoorID.Text.Trim().ToString().PadLeft(2, '0');

                        ClientInfo sea = new ClientInfo(item.Key);
                        byte[] send_data = packFunc.pack_wuhan(cabID, "E", doorID, null, null, null, null, item.Value.head_data);
                        sea.SendBuffer.AddRange(send_data);
                        ThreadPool.QueueUserWorkItem(SendLoop, sea);
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "E", "远程开柜", cabID, doorID, pubFunc.HexToString(send_data)));
                    }
                    break;
                }
            }
        }

        private void cbNeedPwd_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNeedPwd.Checked)
            {
                txtSetStatPwd.Enabled = true;
            }
            else
            {
                txtSetStatPwd.Enabled = false;
            }
        }

        private void cbSetDoor_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSetDoor.Checked)
            {
                txtSetStatShipNo.Enabled = false;
                txtSetStatMembPwd.Enabled = false;
                txtSetStatMembID.Enabled = false;
                cbNeedPwd.Enabled = false;
                txtSetStatPwd.Enabled = false;
            }
            else
            {
                txtSetStatShipNo.Enabled = true;
                txtSetStatMembPwd.Enabled = true;
                txtSetStatMembID.Enabled = true;
                cbNeedPwd.Enabled = true;
                if (cbNeedPwd.Checked)
                {
                    txtSetStatPwd.Enabled = true;
                }
                else
                {
                    txtSetStatPwd.Enabled = false;
                }
            }
        }

        private void btnSetDoorStat_Click(object sender, EventArgs e)
        {
            if (cbCablst.Text == "")
            {
                MessageBox.Show("请先选择一个柜子!");
            }
            else if (cbSetStatDoorID.Text.Trim().Length == 0)
            {
                MessageBox.Show("柜子号不能为空!");
            }
            else
            {
                foreach (var item in list)
                {
                    if (item.Value.guizi_id != cbCablst.Text)
                    {
                        continue;
                    }
                    else
                    {
                        string cabID = item.Value.guizi_id;
                        string doorID = cbSetStatDoorID.Text.Trim();
                        uint setEmpty = 1;
                        uint needPwd = 0;
                        if (cbSetDoor.Checked)
                        {
                            setEmpty = 0;
                        }
                        else
                        {
                            setEmpty = 1;
                        }
                        if (cbNeedPwd.Checked)
                        {
                            needPwd = 1;
                        }
                        else
                        {
                            needPwd = 0;
                        }

                        byte[] datatmp = packFunc.add_string(new byte[0], doorID);//柜子号
                        datatmp = packFunc.add_int(datatmp, setEmpty);//状态变化
                        if (setEmpty == 1)//柜门状态为满,有后续字段
                        {
                            if (txtSetStatShipNo.Text.Trim() == "")
                            {
                                datatmp = packFunc.add_int(datatmp, 0);
                            }
                            else
                            {
                                datatmp = packFunc.add_string(datatmp, txtSetStatShipNo.Text.Trim());
                            }

                            if (txtSetStatMembPwd.Text.Trim() == "")
                            {
                                MessageBox.Show("密码不能为空！");
                                return;
                            }
                            else
                            {
                                datatmp = packFunc.add_string(datatmp, txtSetStatMembPwd.Text.Trim());
                            }

                            if (txtSetStatMembID.Text.Trim() == "")
                            {
                                MessageBox.Show("用户卡号不能为空！");
                                return;
                            }
                            else
                            {
                                datatmp = packFunc.add_string(datatmp, txtSetStatMembID.Text.Trim());
                            }

                            datatmp = packFunc.add_int(datatmp, needPwd);
                            if (needPwd == 1)
                            {
                                if (txtSetStatPwd.Text.Trim() == "")
                                {
                                    MessageBox.Show("支付密码不能为空！");
                                    return;
                                }
                                else
                                {
                                    datatmp = packFunc.add_string(datatmp, txtSetStatPwd.Text.Trim());
                                }
                            }
                        }

                        ClientInfo sea = new ClientInfo(item.Key);
                        byte[] send_data = packFunc.make_pack(cabID, "12", datatmp);
                        sea.SendBuffer.AddRange(send_data);
                        ThreadPool.QueueUserWorkItem(SendLoop, sea);
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "12", "指定柜门状态", cabID, doorID,  pubFunc.HexToString(send_data)));
                    }
                    break;
                }
            }
        }

        private void btnLockDoor_Click(object sender, EventArgs e)//锁定柜门
        {
            if (cbCablst.Text == "")
            {
                MessageBox.Show("请先选择一个柜子!");
            }
            else if (cbLockDoorID.Text.Trim().Length == 0)
            {
                MessageBox.Show("柜门号不能为空!");
            }
            else
            {
                foreach (var item in list)
                {
                    if (item.Value.guizi_id != cbCablst.Text)
                    {
                        continue;
                    }
                    else
                    {
                        string cabID = item.Value.guizi_id.ToString().PadLeft(8, '0');
                        string doorID = cbOpDoorID.Text.Trim().ToString().PadLeft(2, '0');

                        ClientInfo sea = new ClientInfo(item.Key);
                        byte[] send_data = packFunc.pack_wuhan(cabID, "G", doorID, null, null, null, null, item.Value.head_data);
                        sea.SendBuffer.AddRange(send_data);
                        ThreadPool.QueueUserWorkItem(SendLoop, sea);
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "G", "锁定柜门", cabID, doorID,  pubFunc.HexToString(send_data)));
                    }
                    break;
                }
            }
        }

        private void btnUnlockDoor_Click(object sender, EventArgs e)
        {
            if (cbCablst.Text == "")
            {
                MessageBox.Show("请先选择一个柜子!");
            }
            else if (cbUnlockDoorID.Text.Trim().Length == 0)
            {
                MessageBox.Show("柜门号不能为空!");
            }
            else
            {
                foreach (var item in list)
                {
                    if (item.Value.guizi_id != cbCablst.Text)
                    {
                        continue;
                    }
                    else
                    {
                        string cabID = item.Value.guizi_id;
                        string doorID = cbUnlockDoorID.Text.Trim();
                        byte[] datatmp = packFunc.add_string(new byte[0], doorID);//柜子号

                        byte[] send_data = packFunc.make_pack(cabID, "23", datatmp);
                        ClientInfo sea = new ClientInfo(item.Key);
                        sea.SendBuffer.AddRange(send_data);
                        ThreadPool.QueueUserWorkItem(SendLoop, sea);
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "23", "远程解锁柜门", cabID, doorID,  pubFunc.HexToString(send_data)));
                    }
                    break;
                }
            }
        }

        private void btnGetStatDoorID_Click(object sender, EventArgs e)//指令16，获取指定柜门状态
        {
            if (cbCablst.Text == "")
            {
                MessageBox.Show("请先选择一个柜子!");
            }
            else if (cbGetStatDoorID.Text.Trim().Length == 0)
            {
                MessageBox.Show("柜门号不能为空!");
            }
            else
            {
                foreach (var item in list)
                {
                    if (item.Value.guizi_id != cbCablst.Text)
                    {
                        continue;
                    }
                    else
                    {
                        string cabID = item.Value.guizi_id;
                        string doorID = cbGetStatDoorID.Text.Trim();
                        byte[] datatmp = packFunc.add_string(new byte[0], doorID);//柜子号

                        byte[] send_data = packFunc.make_pack(cabID, "16", datatmp);
                        ClientInfo sea = new ClientInfo(item.Key);
                        sea.SendBuffer.AddRange(send_data);
                        ThreadPool.QueueUserWorkItem(SendLoop, sea);
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "16", "获取柜门信息", cabID, doorID, pubFunc.HexToString(send_data)));
                    }
                    break;
                }
            }
        }

        private void btnSetPwd_Click(object sender, EventArgs e)
        {
            if (cbCablst.Text == "")
            {
                MessageBox.Show("请先选择一个柜子!");
            }
            else
            {
                foreach (var item in list)
                {
                    if (item.Value.guizi_id != cbCablst.Text)
                    {
                        continue;
                    }
                    else
                    {
                        string cabID = item.Value.guizi_id;
                        byte[] datatmp = packFunc.add_string(new byte[0], txtAdminPwd.Text.Trim());
                        datatmp = packFunc.add_string(datatmp, txtSuperAdminPwd.Text.Trim());
                        byte[] send_data = packFunc.make_pack(cabID, "14", datatmp);
                        ClientInfo sea = new ClientInfo(item.Key);
                        sea.SendBuffer.AddRange(send_data);
                        ThreadPool.QueueUserWorkItem(SendLoop, sea);
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "14", "指定柜子密码", cabID, "无",  pubFunc.HexToString(send_data)));
                    }
                }
            }
        }

        private void btnSetServe_Click(object sender, EventArgs e)
        {
            if (cbCablst.Text == "")
            {
                MessageBox.Show("请先选择一个柜子!");
            }
            else if(txtSetServe.Text.Trim().Length==0)
            {
                MessageBox.Show("服务器不能为空!");
            }
            else if (txtSetPort.Text.Trim().Length == 0)
            {
                MessageBox.Show("端口不能为空!");
            }
            else
            {
                foreach (var item in list)
                {
                    if (item.Value.guizi_id != cbCablst.Text)
                    {
                        continue;
                    }
                    else
                    {
                        string cabID = item.Value.guizi_id;
                        byte[] datatmp = packFunc.add_string(new byte[0], txtSetServe.Text.Trim());
                        datatmp = packFunc.add_int(datatmp, (uint)System.Int32.Parse(txtSetPort.Text.Trim()));
                        byte[] send_data = packFunc.make_pack(cabID, "25", datatmp);
                        ClientInfo sea = new ClientInfo(item.Key);
                        sea.SendBuffer.AddRange(send_data);
                        ThreadPool.QueueUserWorkItem(SendLoop, sea);
                        logFile.WriteLogFile(string.Format("{0}指令{1},{2},[柜子{3}],[柜门{4}],code:{5}", "发送", "25", "指定柜子服务器", cabID, "无", pubFunc.HexToString(send_data)));
                    }
                }
            }
        }
    }
}
