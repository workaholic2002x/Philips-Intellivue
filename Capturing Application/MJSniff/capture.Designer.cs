namespace capture
{
    partial class capturerForm
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
            this.components = new System.ComponentModel.Container();
            this.treeView = new System.Windows.Forms.TreeView();
            this.btnStart = new System.Windows.Forms.Button();
            this.cmbInterfaces = new System.Windows.Forms.ComboBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox_Port = new System.Windows.Forms.CheckBox();
            this.checkBox_Protocol = new System.Windows.Forms.CheckBox();
            this.checkBox_IP = new System.Windows.Forms.CheckBox();
            this.textBox_PORT = new System.Windows.Forms.TextBox();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.comboBox_Protocol = new System.Windows.Forms.ComboBox();
            this.listBox_Filtered = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox_rmvDuplicates = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox_MyIP = new System.Windows.Forms.CheckBox();
            this.label_MyIP = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox_IPE = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxHeader = new System.Windows.Forms.CheckBox();
            this.labelClient = new System.Windows.Forms.Label();
            this.labelServer = new System.Windows.Forms.Label();
            this.checkBoxSave = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxClient = new System.Windows.Forms.TextBox();
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.Location = new System.Drawing.Point(12, 12);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(345, 253);
            this.treeView.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStart.Location = new System.Drawing.Point(12, 290);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(91, 33);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "&Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // cmbInterfaces
            // 
            this.cmbInterfaces.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbInterfaces.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInterfaces.FormattingEnabled = true;
            this.cmbInterfaces.Location = new System.Drawing.Point(128, 297);
            this.cmbInterfaces.Name = "cmbInterfaces";
            this.cmbInterfaces.Size = new System.Drawing.Size(579, 21);
            this.cmbInterfaces.TabIndex = 2;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(363, 160);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(318, 108);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(363, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 25);
            this.button1.TabIndex = 5;
            this.button1.Text = "Clear List";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox_Port
            // 
            this.checkBox_Port.AutoSize = true;
            this.checkBox_Port.Location = new System.Drawing.Point(384, 13);
            this.checkBox_Port.Name = "checkBox_Port";
            this.checkBox_Port.Size = new System.Drawing.Size(84, 17);
            this.checkBox_Port.TabIndex = 6;
            this.checkBox_Port.Text = "Filter by Port";
            this.checkBox_Port.UseVisualStyleBackColor = true;
            // 
            // checkBox_Protocol
            // 
            this.checkBox_Protocol.AutoSize = true;
            this.checkBox_Protocol.Checked = true;
            this.checkBox_Protocol.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Protocol.Location = new System.Drawing.Point(384, 36);
            this.checkBox_Protocol.Name = "checkBox_Protocol";
            this.checkBox_Protocol.Size = new System.Drawing.Size(162, 17);
            this.checkBox_Protocol.TabIndex = 7;
            this.checkBox_Protocol.Text = "Filter by Protocol (TCP/UDP)";
            this.checkBox_Protocol.UseVisualStyleBackColor = true;
            // 
            // checkBox_IP
            // 
            this.checkBox_IP.AutoSize = true;
            this.checkBox_IP.Location = new System.Drawing.Point(384, 59);
            this.checkBox_IP.Name = "checkBox_IP";
            this.checkBox_IP.Size = new System.Drawing.Size(145, 17);
            this.checkBox_IP.TabIndex = 8;
            this.checkBox_IP.Text = "Filter by IP (Source/Dest)";
            this.checkBox_IP.UseVisualStyleBackColor = true;
            // 
            // textBox_PORT
            // 
            this.textBox_PORT.Location = new System.Drawing.Point(545, 10);
            this.textBox_PORT.Name = "textBox_PORT";
            this.textBox_PORT.Size = new System.Drawing.Size(100, 20);
            this.textBox_PORT.TabIndex = 9;
            this.textBox_PORT.Text = "44780";
            // 
            // textBox_IP
            // 
            this.textBox_IP.Location = new System.Drawing.Point(546, 57);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(100, 20);
            this.textBox_IP.TabIndex = 10;
            this.textBox_IP.Text = "192.168.0.12";
            // 
            // comboBox_Protocol
            // 
            this.comboBox_Protocol.FormattingEnabled = true;
            this.comboBox_Protocol.Items.AddRange(new object[] {
            "UDP",
            "TCP"});
            this.comboBox_Protocol.Location = new System.Drawing.Point(544, 32);
            this.comboBox_Protocol.Name = "comboBox_Protocol";
            this.comboBox_Protocol.Size = new System.Drawing.Size(101, 21);
            this.comboBox_Protocol.TabIndex = 11;
            this.comboBox_Protocol.Text = "UDP";
            this.comboBox_Protocol.SelectedIndexChanged += new System.EventHandler(this.comboBox_Protocol_SelectedIndexChanged);
            // 
            // listBox_Filtered
            // 
            this.listBox_Filtered.FormattingEnabled = true;
            this.listBox_Filtered.Location = new System.Drawing.Point(687, 157);
            this.listBox_Filtered.Name = "listBox_Filtered";
            this.listBox_Filtered.Size = new System.Drawing.Size(206, 108);
            this.listBox_Filtered.TabIndex = 12;
            this.listBox_Filtered.SelectedIndexChanged += new System.EventHandler(this.listBox_Filtered_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(469, 94);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(71, 35);
            this.button2.TabIndex = 13;
            this.button2.Text = "Apply Filters";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox_rmvDuplicates
            // 
            this.checkBox_rmvDuplicates.AutoSize = true;
            this.checkBox_rmvDuplicates.Checked = true;
            this.checkBox_rmvDuplicates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_rmvDuplicates.Location = new System.Drawing.Point(667, 17);
            this.checkBox_rmvDuplicates.Name = "checkBox_rmvDuplicates";
            this.checkBox_rmvDuplicates.Size = new System.Drawing.Size(117, 17);
            this.checkBox_rmvDuplicates.TabIndex = 14;
            this.checkBox_rmvDuplicates.Text = "Remove duplicates";
            this.checkBox_rmvDuplicates.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(785, 134);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(50, 20);
            this.textBox1.TabIndex = 15;
            // 
            // checkBox_MyIP
            // 
            this.checkBox_MyIP.AutoSize = true;
            this.checkBox_MyIP.Checked = true;
            this.checkBox_MyIP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_MyIP.Location = new System.Drawing.Point(667, 41);
            this.checkBox_MyIP.Name = "checkBox_MyIP";
            this.checkBox_MyIP.Size = new System.Drawing.Size(94, 17);
            this.checkBox_MyIP.TabIndex = 16;
            this.checkBox_MyIP.Text = "Exclude My IP";
            this.checkBox_MyIP.UseVisualStyleBackColor = true;
            // 
            // label_MyIP
            // 
            this.label_MyIP.AutoSize = true;
            this.label_MyIP.Location = new System.Drawing.Point(803, 10);
            this.label_MyIP.Name = "label_MyIP";
            this.label_MyIP.Size = new System.Drawing.Size(73, 13);
            this.label_MyIP.TabIndex = 17;
            this.label_MyIP.Text = "My IP: 0.0.0.0";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(918, 13);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(173, 255);
            this.treeView1.TabIndex = 18;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1093, 36);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(42, 20);
            this.textBox2.TabIndex = 19;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(357, 104);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(44, 20);
            this.textBox3.TabIndex = 20;
            // 
            // textBox_IPE
            // 
            this.textBox_IPE.Location = new System.Drawing.Point(546, 81);
            this.textBox_IPE.Name = "textBox_IPE";
            this.textBox_IPE.Size = new System.Drawing.Size(102, 20);
            this.textBox_IPE.TabIndex = 21;
            this.textBox_IPE.Text = "192.168.0.12";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(687, 83);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(92, 31);
            this.button3.TabIndex = 22;
            this.button3.Text = "IP to HostName";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(785, 75);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(90, 20);
            this.textBox4.TabIndex = 23;
            this.textBox4.Text = "10.226.43.251";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(785, 104);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(89, 20);
            this.textBox5.TabIndex = 24;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(630, 137);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(51, 20);
            this.textBox6.TabIndex = 25;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.checkBoxHeader);
            this.groupBox1.Controls.Add(this.labelClient);
            this.groupBox1.Controls.Add(this.labelServer);
            this.groupBox1.Controls.Add(this.checkBoxSave);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxClient);
            this.groupBox1.Controls.Add(this.textBoxServer);
            this.groupBox1.Location = new System.Drawing.Point(1125, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 193);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Recorder";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Saving at :";
            // 
            // checkBoxHeader
            // 
            this.checkBoxHeader.AutoSize = true;
            this.checkBoxHeader.Checked = true;
            this.checkBoxHeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHeader.Location = new System.Drawing.Point(12, 85);
            this.checkBoxHeader.Name = "checkBoxHeader";
            this.checkBoxHeader.Size = new System.Drawing.Size(99, 17);
            this.checkBoxHeader.TabIndex = 7;
            this.checkBoxHeader.Text = "Include Header";
            this.checkBoxHeader.UseVisualStyleBackColor = true;
            // 
            // labelClient
            // 
            this.labelClient.AutoSize = true;
            this.labelClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClient.Location = new System.Drawing.Point(173, 54);
            this.labelClient.Name = "labelClient";
            this.labelClient.Size = new System.Drawing.Size(16, 16);
            this.labelClient.TabIndex = 6;
            this.labelClient.Text = "0";
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServer.Location = new System.Drawing.Point(172, 26);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(16, 16);
            this.labelServer.TabIndex = 5;
            this.labelServer.Text = "0";
            // 
            // checkBoxSave
            // 
            this.checkBoxSave.AutoSize = true;
            this.checkBoxSave.Location = new System.Drawing.Point(11, 147);
            this.checkBoxSave.Name = "checkBoxSave";
            this.checkBoxSave.Size = new System.Drawing.Size(87, 17);
            this.checkBoxSave.TabIndex = 4;
            this.checkBoxSave.Text = "Enable Save";
            this.checkBoxSave.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(106, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Client IP";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server IP";
            // 
            // textBoxClient
            // 
            this.textBoxClient.Location = new System.Drawing.Point(11, 49);
            this.textBoxClient.Name = "textBoxClient";
            this.textBoxClient.Size = new System.Drawing.Size(94, 20);
            this.textBoxClient.TabIndex = 1;
            this.textBoxClient.Text = "255.255.255.255";
            // 
            // textBoxServer
            // 
            this.textBoxServer.Location = new System.Drawing.Point(12, 23);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.Size = new System.Drawing.Size(93, 20);
            this.textBoxServer.TabIndex = 0;
            this.textBoxServer.Text = "0.0.0.0";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(439, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 23);
            this.label4.TabIndex = 27;
            this.label4.Text = "Remote System Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(359, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Count";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(594, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Count";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(749, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Count";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1094, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Count";
            // 
            // capturerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1362, 329);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox_IPE);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.label_MyIP);
            this.Controls.Add(this.checkBox_MyIP);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkBox_rmvDuplicates);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listBox_Filtered);
            this.Controls.Add(this.comboBox_Protocol);
            this.Controls.Add(this.textBox_IP);
            this.Controls.Add(this.textBox_PORT);
            this.Controls.Add(this.checkBox_IP);
            this.Controls.Add(this.checkBox_Protocol);
            this.Controls.Add(this.checkBox_Port);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.cmbInterfaces);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.treeView);
            this.Name = "capturerForm";
            this.Text = "Intellivue Capture";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SnifferForm_FormClosing);
            this.Load += new System.EventHandler(this.SnifferForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cmbInterfaces;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox_Port;
        private System.Windows.Forms.CheckBox checkBox_Protocol;
        private System.Windows.Forms.CheckBox checkBox_IP;
        private System.Windows.Forms.TextBox textBox_PORT;
        private System.Windows.Forms.TextBox textBox_IP;
        private System.Windows.Forms.ComboBox comboBox_Protocol;
        private System.Windows.Forms.ListBox listBox_Filtered;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox_rmvDuplicates;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox_MyIP;
        private System.Windows.Forms.Label label_MyIP;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox_IPE;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxClient;
        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxSave;
        private System.Windows.Forms.Label labelClient;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.CheckBox checkBoxHeader;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}

