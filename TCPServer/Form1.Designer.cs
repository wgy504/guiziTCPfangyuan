namespace guiziTCPfangyuan
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnSend = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.richTxt = new System.Windows.Forms.RichTextBox();
            this.lstClient = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAdminPwd = new System.Windows.Forms.TextBox();
            this.txtAdminUser = new System.Windows.Forms.TextBox();
            this.btnControl = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timerStep = new System.Windows.Forms.Timer(this.components);
            this.btnSmsReg = new System.Windows.Forms.Button();
            this.textBox_weburl = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button_setweburl = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(432, 360);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(101, 64);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "发送消息";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(432, 287);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(101, 67);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "启动服务";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "收到的消息:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(537, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "在线列表:";
            // 
            // richTxt
            // 
            this.richTxt.Location = new System.Drawing.Point(12, 38);
            this.richTxt.Name = "richTxt";
            this.richTxt.Size = new System.Drawing.Size(521, 241);
            this.richTxt.TabIndex = 5;
            this.richTxt.Text = "";
            // 
            // lstClient
            // 
            this.lstClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstClient.FullRowSelect = true;
            this.lstClient.Location = new System.Drawing.Point(539, 38);
            this.lstClient.MultiSelect = false;
            this.lstClient.Name = "lstClient";
            this.lstClient.Size = new System.Drawing.Size(290, 507);
            this.lstClient.TabIndex = 6;
            this.lstClient.UseCompatibleStateImageBehavior = false;
            this.lstClient.View = System.Windows.Forms.View.Details;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtAdminPwd);
            this.groupBox1.Controls.Add(this.txtAdminUser);
            this.groupBox1.Controls.Add(this.btnControl);
            this.groupBox1.Location = new System.Drawing.Point(15, 445);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(326, 100);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "柜子操作登陆";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(49, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "密码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "用户名";
            // 
            // txtAdminPwd
            // 
            this.txtAdminPwd.Location = new System.Drawing.Point(84, 53);
            this.txtAdminPwd.Name = "txtAdminPwd";
            this.txtAdminPwd.PasswordChar = '*';
            this.txtAdminPwd.Size = new System.Drawing.Size(125, 21);
            this.txtAdminPwd.TabIndex = 16;
            this.txtAdminPwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAdminPwd_KeyPress);
            // 
            // txtAdminUser
            // 
            this.txtAdminUser.Location = new System.Drawing.Point(84, 26);
            this.txtAdminUser.Name = "txtAdminUser";
            this.txtAdminUser.Size = new System.Drawing.Size(125, 21);
            this.txtAdminUser.TabIndex = 15;
            // 
            // btnControl
            // 
            this.btnControl.Location = new System.Drawing.Point(215, 26);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(75, 48);
            this.btnControl.TabIndex = 14;
            this.btnControl.Text = "柜子操作";
            this.btnControl.UseVisualStyleBackColor = true;
            this.btnControl.Click += new System.EventHandler(this.btnControl_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(230, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "测试模式";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timerStep
            // 
            this.timerStep.Interval = 60000;
            this.timerStep.Tick += new System.EventHandler(this.timerStep_Tick);
            // 
            // btnSmsReg
            // 
            this.btnSmsReg.Location = new System.Drawing.Point(432, 447);
            this.btnSmsReg.Name = "btnSmsReg";
            this.btnSmsReg.Size = new System.Drawing.Size(101, 41);
            this.btnSmsReg.TabIndex = 16;
            this.btnSmsReg.Text = "短信服务设置";
            this.btnSmsReg.UseVisualStyleBackColor = true;
            this.btnSmsReg.Click += new System.EventHandler(this.btnSmsReg_Click);
            // 
            // textBox_weburl
            // 
            this.textBox_weburl.Location = new System.Drawing.Point(15, 309);
            this.textBox_weburl.Name = "textBox_weburl";
            this.textBox_weburl.Size = new System.Drawing.Size(209, 21);
            this.textBox_weburl.TabIndex = 17;
            this.textBox_weburl.Text = "http://www.jck360.com";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 294);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "网站地址：";
            // 
            // button_setweburl
            // 
            this.button_setweburl.Location = new System.Drawing.Point(230, 309);
            this.button_setweburl.Name = "button_setweburl";
            this.button_setweburl.Size = new System.Drawing.Size(102, 23);
            this.button_setweburl.TabIndex = 19;
            this.button_setweburl.Text = "设置网站地址";
            this.button_setweburl.UseVisualStyleBackColor = true;
            this.button_setweburl.Click += new System.EventHandler(this.button_setweburl_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(265, 383);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 383);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(247, 21);
            this.textBox1.TabIndex = 21;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 557);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button_setweburl);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_weburl);
            this.Controls.Add(this.btnSmsReg);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lstClient);
            this.Controls.Add(this.richTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "家常客（柜子服务系统_方氏）";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTxt;
        private System.Windows.Forms.ListView lstClient;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAdminPwd;
        private System.Windows.Forms.TextBox txtAdminUser;
        private System.Windows.Forms.Button btnControl;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timerStep;
        private System.Windows.Forms.Button btnSmsReg;
        private System.Windows.Forms.TextBox textBox_weburl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_setweburl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;

    }
}

