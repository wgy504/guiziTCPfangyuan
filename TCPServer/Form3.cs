﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNet.Utilities;

namespace guiziTCPfangyuan
{
    public partial class Form3 : Form
    {
        pubFunc pubFunction = new pubFunc();
        public Form3()
        {
            InitializeComponent();
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            pubFunc.smsInfo.serialNO = txtSN.Text.Trim().ToString();
            pubFunc.smsInfo.serialPwd = txtPWD.Text.Trim().ToString();
            INIFile ini = new INIFile(Application.StartupPath + @"\config.ini");
            try
            {
                ini.IniWriteValue("SMS", "serial", pubFunc.smsInfo.serialNO);
                ini.IniWriteValue("SMS", "password", pubFunc.smsInfo.serialPwd);
            }
            catch
            {
                MessageBox.Show("写配置文件失败!");
                Application.Exit();
            }
            MessageBox.Show("序列号注册返回结果:" + pubFunction.sdkService.registEx(txtSN.Text.Trim().ToString(), txtPWD.Text.Trim().ToString(), txtPWD.Text.Trim().ToString()));
            MessageBox.Show("企业信息注册返回结果:" + pubFunction.sdkService.registDetailInfo(txtSN.Text.Trim().ToString(), txtPWD.Text.Trim().ToString(), txtEName.Text.Trim().ToString(), txtUName.Text.Trim().ToString(), txtTel.Text.Trim().ToString(), txtMobile.Text.Trim().ToString(), txtEmail.Text.Trim().ToString(), txtFax.Text.Trim().ToString(), txtAddress.Text.Trim().ToString(), txtPostcode.Text.Trim().ToString()));
        }

        private void btnUnReg_Click(object sender, EventArgs e)
        {
            MessageBox.Show("序列号注销返回结果:" + pubFunction.sdkService.logout(txtSN.Text.Trim().ToString(), txtPWD.Text.Trim().ToString()));
        }

        private void btnReCharge_Click(object sender, EventArgs e)
        {
            MessageBox.Show("序列号充值返回结果:" + pubFunction.sdkService.chargeUp(txtSN.Text.Trim().ToString(), txtPWD.Text.Trim().ToString(), txtCardNo.Text.Trim().ToString(), txtCardPwd.Text.Trim().ToString()));
        }

        private void btnSerialPwdUpd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("序列号密码更改返回结果:" + pubFunction.sdkService.serialPwdUpd(txtSN.Text.Trim().ToString(), txtPWD.Text.Trim().ToString(), txtPWD.Text.Trim().ToString(), txtNewPwd.Text.Trim().ToString()));
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            txtSN.Text=(pubFunc.smsInfo.serialNO);
            txtPWD.Text = (pubFunc.smsInfo.serialPwd);
        }

    }
}
