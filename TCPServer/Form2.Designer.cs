﻿namespace guiziTCPfangyuan
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbCablst = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbOpDoor = new System.Windows.Forms.CheckBox();
            this.cbOpDoorID = new System.Windows.Forms.ComboBox();
            this.btnOpenDoor = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSetDoorStat = new System.Windows.Forms.Button();
            this.txtSetStatPwd = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbNeedPwd = new System.Windows.Forms.CheckBox();
            this.txtSetStatMembID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSetStatMembPwd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSetStatShipNo = new System.Windows.Forms.TextBox();
            this.cbSetDoor = new System.Windows.Forms.CheckBox();
            this.cbSetStatDoorID = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbLockDoorID = new System.Windows.Forms.ComboBox();
            this.btnLockDoor = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbUnlockDoorID = new System.Windows.Forms.ComboBox();
            this.btnUnlockDoor = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbGetStatDoorID = new System.Windows.Forms.ComboBox();
            this.btnGetStatDoorID = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtSuperAdminPwd = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtAdminPwd = new System.Windows.Forms.TextBox();
            this.btnSetPwd = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnSetServe = new System.Windows.Forms.Button();
            this.txtSetPort = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtSetServe = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbCablst
            // 
            this.cbCablst.FormattingEnabled = true;
            this.cbCablst.Location = new System.Drawing.Point(203, 12);
            this.cbCablst.Name = "cbCablst";
            this.cbCablst.Size = new System.Drawing.Size(192, 20);
            this.cbCablst.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(144, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择柜子";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckbOpDoor);
            this.groupBox1.Controls.Add(this.cbOpDoorID);
            this.groupBox1.Controls.Add(this.btnOpenDoor);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(121, 116);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "远程开柜";
            // 
            // ckbOpDoor
            // 
            this.ckbOpDoor.AutoSize = true;
            this.ckbOpDoor.Location = new System.Drawing.Point(23, 51);
            this.ckbOpDoor.Name = "ckbOpDoor";
            this.ckbOpDoor.Size = new System.Drawing.Size(72, 16);
            this.ckbOpDoor.TabIndex = 4;
            this.ckbOpDoor.Text = "状态置空";
            this.ckbOpDoor.UseVisualStyleBackColor = true;
            // 
            // cbOpDoorID
            // 
            this.cbOpDoorID.FormattingEnabled = true;
            this.cbOpDoorID.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24"});
            this.cbOpDoorID.Location = new System.Drawing.Point(59, 21);
            this.cbOpDoorID.Name = "cbOpDoorID";
            this.cbOpDoorID.Size = new System.Drawing.Size(43, 20);
            this.cbOpDoorID.TabIndex = 3;
            this.cbOpDoorID.Text = "01";
            // 
            // btnOpenDoor
            // 
            this.btnOpenDoor.Location = new System.Drawing.Point(22, 79);
            this.btnOpenDoor.Name = "btnOpenDoor";
            this.btnOpenDoor.Size = new System.Drawing.Size(75, 23);
            this.btnOpenDoor.TabIndex = 2;
            this.btnOpenDoor.Text = "开柜门";
            this.btnOpenDoor.UseVisualStyleBackColor = true;
            this.btnOpenDoor.Click += new System.EventHandler(this.btnOpenDoor_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "柜门号";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSetDoorStat);
            this.groupBox2.Controls.Add(this.txtSetStatPwd);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.cbNeedPwd);
            this.groupBox2.Controls.Add(this.txtSetStatMembID);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtSetStatMembPwd);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtSetStatShipNo);
            this.groupBox2.Controls.Add(this.cbSetDoor);
            this.groupBox2.Controls.Add(this.cbSetStatDoorID);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(176, 50);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(193, 223);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "指定柜门状态";
            // 
            // btnSetDoorStat
            // 
            this.btnSetDoorStat.Location = new System.Drawing.Point(54, 182);
            this.btnSetDoorStat.Name = "btnSetDoorStat";
            this.btnSetDoorStat.Size = new System.Drawing.Size(75, 23);
            this.btnSetDoorStat.TabIndex = 15;
            this.btnSetDoorStat.Text = "指定状态";
            this.btnSetDoorStat.UseVisualStyleBackColor = true;
            this.btnSetDoorStat.Click += new System.EventHandler(this.btnSetDoorStat_Click);
            // 
            // txtSetStatPwd
            // 
            this.txtSetStatPwd.Enabled = false;
            this.txtSetStatPwd.Location = new System.Drawing.Point(73, 152);
            this.txtSetStatPwd.Name = "txtSetStatPwd";
            this.txtSetStatPwd.Size = new System.Drawing.Size(100, 21);
            this.txtSetStatPwd.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "支付密码";
            // 
            // cbNeedPwd
            // 
            this.cbNeedPwd.AutoSize = true;
            this.cbNeedPwd.Location = new System.Drawing.Point(31, 130);
            this.cbNeedPwd.Name = "cbNeedPwd";
            this.cbNeedPwd.Size = new System.Drawing.Size(84, 16);
            this.cbNeedPwd.TabIndex = 12;
            this.cbNeedPwd.Text = "需支付密码";
            this.cbNeedPwd.UseVisualStyleBackColor = true;
            this.cbNeedPwd.CheckedChanged += new System.EventHandler(this.cbNeedPwd_CheckedChanged);
            // 
            // txtSetStatMembID
            // 
            this.txtSetStatMembID.Location = new System.Drawing.Point(73, 103);
            this.txtSetStatMembID.Name = "txtSetStatMembID";
            this.txtSetStatMembID.Size = new System.Drawing.Size(100, 21);
            this.txtSetStatMembID.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "用户卡号";
            // 
            // txtSetStatMembPwd
            // 
            this.txtSetStatMembPwd.Location = new System.Drawing.Point(73, 76);
            this.txtSetStatMembPwd.Name = "txtSetStatMembPwd";
            this.txtSetStatMembPwd.Size = new System.Drawing.Size(100, 21);
            this.txtSetStatMembPwd.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "用户密码";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "送货单号";
            // 
            // txtSetStatShipNo
            // 
            this.txtSetStatShipNo.Location = new System.Drawing.Point(73, 49);
            this.txtSetStatShipNo.Name = "txtSetStatShipNo";
            this.txtSetStatShipNo.Size = new System.Drawing.Size(100, 21);
            this.txtSetStatShipNo.TabIndex = 7;
            // 
            // cbSetDoor
            // 
            this.cbSetDoor.AutoSize = true;
            this.cbSetDoor.Location = new System.Drawing.Point(113, 24);
            this.cbSetDoor.Name = "cbSetDoor";
            this.cbSetDoor.Size = new System.Drawing.Size(72, 16);
            this.cbSetDoor.TabIndex = 6;
            this.cbSetDoor.Text = "状态置空";
            this.cbSetDoor.UseVisualStyleBackColor = true;
            this.cbSetDoor.CheckedChanged += new System.EventHandler(this.cbSetDoor_CheckedChanged);
            // 
            // cbSetStatDoorID
            // 
            this.cbSetStatDoorID.FormattingEnabled = true;
            this.cbSetStatDoorID.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24"});
            this.cbSetStatDoorID.Location = new System.Drawing.Point(59, 23);
            this.cbSetStatDoorID.Name = "cbSetStatDoorID";
            this.cbSetStatDoorID.Size = new System.Drawing.Size(43, 20);
            this.cbSetStatDoorID.TabIndex = 5;
            this.cbSetStatDoorID.Text = "01";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "柜门号";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbLockDoorID);
            this.groupBox3.Controls.Add(this.btnLockDoor);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(157, 308);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(121, 88);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "锁定柜门";
            // 
            // cbLockDoorID
            // 
            this.cbLockDoorID.FormattingEnabled = true;
            this.cbLockDoorID.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24"});
            this.cbLockDoorID.Location = new System.Drawing.Point(59, 21);
            this.cbLockDoorID.Name = "cbLockDoorID";
            this.cbLockDoorID.Size = new System.Drawing.Size(43, 20);
            this.cbLockDoorID.TabIndex = 3;
            this.cbLockDoorID.Text = "01";
            // 
            // btnLockDoor
            // 
            this.btnLockDoor.Location = new System.Drawing.Point(24, 50);
            this.btnLockDoor.Name = "btnLockDoor";
            this.btnLockDoor.Size = new System.Drawing.Size(75, 23);
            this.btnLockDoor.TabIndex = 2;
            this.btnLockDoor.Text = "锁定柜门";
            this.btnLockDoor.UseVisualStyleBackColor = true;
            this.btnLockDoor.Click += new System.EventHandler(this.btnLockDoor_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "柜门号";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbUnlockDoorID);
            this.groupBox4.Controls.Add(this.btnUnlockDoor);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Location = new System.Drawing.Point(289, 308);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(121, 88);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "解锁柜门";
            // 
            // cbUnlockDoorID
            // 
            this.cbUnlockDoorID.FormattingEnabled = true;
            this.cbUnlockDoorID.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24"});
            this.cbUnlockDoorID.Location = new System.Drawing.Point(59, 21);
            this.cbUnlockDoorID.Name = "cbUnlockDoorID";
            this.cbUnlockDoorID.Size = new System.Drawing.Size(43, 20);
            this.cbUnlockDoorID.TabIndex = 3;
            this.cbUnlockDoorID.Text = "01";
            // 
            // btnUnlockDoor
            // 
            this.btnUnlockDoor.Location = new System.Drawing.Point(24, 50);
            this.btnUnlockDoor.Name = "btnUnlockDoor";
            this.btnUnlockDoor.Size = new System.Drawing.Size(75, 23);
            this.btnUnlockDoor.TabIndex = 2;
            this.btnUnlockDoor.Text = "解锁柜门";
            this.btnUnlockDoor.UseVisualStyleBackColor = true;
            this.btnUnlockDoor.Click += new System.EventHandler(this.btnUnlockDoor_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 1;
            this.label9.Text = "柜门号";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbGetStatDoorID);
            this.groupBox5.Controls.Add(this.btnGetStatDoorID);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Location = new System.Drawing.Point(12, 202);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(121, 88);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "获取柜门状态";
            // 
            // cbGetStatDoorID
            // 
            this.cbGetStatDoorID.FormattingEnabled = true;
            this.cbGetStatDoorID.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24"});
            this.cbGetStatDoorID.Location = new System.Drawing.Point(59, 21);
            this.cbGetStatDoorID.Name = "cbGetStatDoorID";
            this.cbGetStatDoorID.Size = new System.Drawing.Size(43, 20);
            this.cbGetStatDoorID.TabIndex = 3;
            this.cbGetStatDoorID.Text = "01";
            // 
            // btnGetStatDoorID
            // 
            this.btnGetStatDoorID.Location = new System.Drawing.Point(24, 50);
            this.btnGetStatDoorID.Name = "btnGetStatDoorID";
            this.btnGetStatDoorID.Size = new System.Drawing.Size(75, 23);
            this.btnGetStatDoorID.TabIndex = 2;
            this.btnGetStatDoorID.Text = "获取状态";
            this.btnGetStatDoorID.UseVisualStyleBackColor = true;
            this.btnGetStatDoorID.Click += new System.EventHandler(this.btnGetStatDoorID_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "柜门号";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnSetPwd);
            this.groupBox6.Controls.Add(this.txtSuperAdminPwd);
            this.groupBox6.Controls.Add(this.label13);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.txtAdminPwd);
            this.groupBox6.Location = new System.Drawing.Point(406, 50);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(193, 116);
            this.groupBox6.TabIndex = 14;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "指定柜子密码";
            // 
            // txtSuperAdminPwd
            // 
            this.txtSuperAdminPwd.Location = new System.Drawing.Point(107, 55);
            this.txtSuperAdminPwd.Name = "txtSuperAdminPwd";
            this.txtSuperAdminPwd.Size = new System.Drawing.Size(75, 21);
            this.txtSuperAdminPwd.TabIndex = 9;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 58);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 12);
            this.label13.TabIndex = 8;
            this.label13.Text = "超级管理员密码";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(14, 33);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 8;
            this.label14.Text = "管理员密码";
            // 
            // txtAdminPwd
            // 
            this.txtAdminPwd.Location = new System.Drawing.Point(107, 28);
            this.txtAdminPwd.Name = "txtAdminPwd";
            this.txtAdminPwd.Size = new System.Drawing.Size(75, 21);
            this.txtAdminPwd.TabIndex = 7;
            // 
            // btnSetPwd
            // 
            this.btnSetPwd.Location = new System.Drawing.Point(54, 86);
            this.btnSetPwd.Name = "btnSetPwd";
            this.btnSetPwd.Size = new System.Drawing.Size(75, 23);
            this.btnSetPwd.TabIndex = 16;
            this.btnSetPwd.Text = "指定密码";
            this.btnSetPwd.UseVisualStyleBackColor = true;
            this.btnSetPwd.Click += new System.EventHandler(this.btnSetPwd_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnSetServe);
            this.groupBox7.Controls.Add(this.txtSetPort);
            this.groupBox7.Controls.Add(this.label11);
            this.groupBox7.Controls.Add(this.label12);
            this.groupBox7.Controls.Add(this.txtSetServe);
            this.groupBox7.Location = new System.Drawing.Point(406, 180);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(193, 116);
            this.groupBox7.TabIndex = 17;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "指定柜子服务器";
            // 
            // btnSetServe
            // 
            this.btnSetServe.Location = new System.Drawing.Point(54, 86);
            this.btnSetServe.Name = "btnSetServe";
            this.btnSetServe.Size = new System.Drawing.Size(75, 23);
            this.btnSetServe.TabIndex = 16;
            this.btnSetServe.Text = "指定服务器";
            this.btnSetServe.UseVisualStyleBackColor = true;
            this.btnSetServe.Click += new System.EventHandler(this.btnSetServe_Click);
            // 
            // txtSetPort
            // 
            this.txtSetPort.Location = new System.Drawing.Point(72, 55);
            this.txtSetPort.Name = "txtSetPort";
            this.txtSetPort.Size = new System.Drawing.Size(110, 21);
            this.txtSetPort.TabIndex = 9;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 58);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 8;
            this.label11.Text = "端口";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 33);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 8;
            this.label12.Text = "服务器";
            // 
            // txtSetServe
            // 
            this.txtSetServe.Location = new System.Drawing.Point(72, 28);
            this.txtSetServe.Name = "txtSetServe";
            this.txtSetServe.Size = new System.Drawing.Size(110, 21);
            this.txtSetServe.TabIndex = 7;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 411);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCablst);
            this.Name = "Form2";
            this.Text = "柜子操作";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbCablst;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ckbOpDoor;
        private System.Windows.Forms.ComboBox cbOpDoorID;
        private System.Windows.Forms.Button btnOpenDoor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSetDoorStat;
        private System.Windows.Forms.TextBox txtSetStatPwd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbNeedPwd;
        private System.Windows.Forms.TextBox txtSetStatMembID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSetStatMembPwd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSetStatShipNo;
        private System.Windows.Forms.CheckBox cbSetDoor;
        private System.Windows.Forms.ComboBox cbSetStatDoorID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbLockDoorID;
        private System.Windows.Forms.Button btnLockDoor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cbUnlockDoorID;
        private System.Windows.Forms.Button btnUnlockDoor;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cbGetStatDoorID;
        private System.Windows.Forms.Button btnGetStatDoorID;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txtSuperAdminPwd;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtAdminPwd;
        private System.Windows.Forms.Button btnSetPwd;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnSetServe;
        private System.Windows.Forms.TextBox txtSetPort;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtSetServe;
    }
}