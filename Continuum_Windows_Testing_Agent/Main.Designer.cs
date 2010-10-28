namespace Continuum_Windows_Testing_Agent
{
    partial class Main
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.safariVersionBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chromeVersionBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.firefoxVersionBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ieVersionBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.haltOnErrorBox = new System.Windows.Forms.CheckBox();
            this.buildBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.forcePollBtn = new System.Windows.Forms.Button();
            this.regenerateGuidBtn = new System.Windows.Forms.Button();
            this.guidBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.useVerboseTestLogsCheckbox = new System.Windows.Forms.CheckBox();
            this.machineNameBox = new System.Windows.Forms.TextBox();
            this.configSaveSettingsBtn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.osVersionBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.hostnameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ctmStatusBar = new System.Windows.Forms.StatusStrip();
            this.ctmStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.callHomeTimer = new System.Windows.Forms.Timer(this.components);
            this.lastRunLogBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.ctmStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.safariVersionBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.chromeVersionBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.firefoxVersionBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.ieVersionBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(195, 148);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Browsers";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // safariVersionBox
            // 
            this.safariVersionBox.Location = new System.Drawing.Point(97, 106);
            this.safariVersionBox.Name = "safariVersionBox";
            this.safariVersionBox.ReadOnly = true;
            this.safariVersionBox.Size = new System.Drawing.Size(83, 20);
            this.safariVersionBox.TabIndex = 16;
            this.safariVersionBox.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Safari:";
            // 
            // chromeVersionBox
            // 
            this.chromeVersionBox.Location = new System.Drawing.Point(97, 78);
            this.chromeVersionBox.Name = "chromeVersionBox";
            this.chromeVersionBox.ReadOnly = true;
            this.chromeVersionBox.Size = new System.Drawing.Size(83, 20);
            this.chromeVersionBox.TabIndex = 12;
            this.chromeVersionBox.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Chrome:";
            // 
            // firefoxVersionBox
            // 
            this.firefoxVersionBox.Location = new System.Drawing.Point(97, 50);
            this.firefoxVersionBox.Name = "firefoxVersionBox";
            this.firefoxVersionBox.ReadOnly = true;
            this.firefoxVersionBox.Size = new System.Drawing.Size(83, 20);
            this.firefoxVersionBox.TabIndex = 8;
            this.firefoxVersionBox.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Firefox:";
            // 
            // ieVersionBox
            // 
            this.ieVersionBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.ieVersionBox.Location = new System.Drawing.Point(97, 25);
            this.ieVersionBox.Name = "ieVersionBox";
            this.ieVersionBox.ReadOnly = true;
            this.ieVersionBox.Size = new System.Drawing.Size(83, 20);
            this.ieVersionBox.TabIndex = 1;
            this.ieVersionBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Internet Explorer:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.haltOnErrorBox);
            this.groupBox2.Controls.Add(this.buildBox);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.forcePollBtn);
            this.groupBox2.Controls.Add(this.regenerateGuidBtn);
            this.groupBox2.Controls.Add(this.guidBox);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.useVerboseTestLogsCheckbox);
            this.groupBox2.Controls.Add(this.machineNameBox);
            this.groupBox2.Controls.Add(this.configSaveSettingsBtn);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.ipBox);
            this.groupBox2.Controls.Add(this.osVersionBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.hostnameBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(222, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(534, 164);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Config";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // haltOnErrorBox
            // 
            this.haltOnErrorBox.AutoSize = true;
            this.haltOnErrorBox.Location = new System.Drawing.Point(296, 48);
            this.haltOnErrorBox.Name = "haltOnErrorBox";
            this.haltOnErrorBox.Size = new System.Drawing.Size(87, 17);
            this.haltOnErrorBox.TabIndex = 14;
            this.haltOnErrorBox.Text = "Halt On Error";
            this.haltOnErrorBox.UseVisualStyleBackColor = true;
            // 
            // buildBox
            // 
            this.buildBox.Location = new System.Drawing.Point(414, 132);
            this.buildBox.Name = "buildBox";
            this.buildBox.ReadOnly = true;
            this.buildBox.Size = new System.Drawing.Size(114, 20);
            this.buildBox.TabIndex = 6;
            this.buildBox.TextChanged += new System.EventHandler(this.buildBox_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(366, 136);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "Version:";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // forcePollBtn
            // 
            this.forcePollBtn.Location = new System.Drawing.Point(419, 48);
            this.forcePollBtn.Name = "forcePollBtn";
            this.forcePollBtn.Size = new System.Drawing.Size(102, 23);
            this.forcePollBtn.TabIndex = 13;
            this.forcePollBtn.Text = "Force Poll";
            this.forcePollBtn.UseVisualStyleBackColor = true;
            this.forcePollBtn.Click += new System.EventHandler(this.forcePollBtn_Click);
            // 
            // regenerateGuidBtn
            // 
            this.regenerateGuidBtn.Location = new System.Drawing.Point(240, 130);
            this.regenerateGuidBtn.Name = "regenerateGuidBtn";
            this.regenerateGuidBtn.Size = new System.Drawing.Size(102, 23);
            this.regenerateGuidBtn.TabIndex = 12;
            this.regenerateGuidBtn.Text = "Regenerate GUID";
            this.regenerateGuidBtn.UseVisualStyleBackColor = true;
            this.regenerateGuidBtn.Click += new System.EventHandler(this.regenerateGuidBtn_Click);
            // 
            // guidBox
            // 
            this.guidBox.Location = new System.Drawing.Point(49, 133);
            this.guidBox.Name = "guidBox";
            this.guidBox.ReadOnly = true;
            this.guidBox.Size = new System.Drawing.Size(185, 20);
            this.guidBox.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 136);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "GUID:";
            // 
            // useVerboseTestLogsCheckbox
            // 
            this.useVerboseTestLogsCheckbox.AutoSize = true;
            this.useVerboseTestLogsCheckbox.Location = new System.Drawing.Point(296, 24);
            this.useVerboseTestLogsCheckbox.Name = "useVerboseTestLogsCheckbox";
            this.useVerboseTestLogsCheckbox.Size = new System.Drawing.Size(115, 17);
            this.useVerboseTestLogsCheckbox.TabIndex = 9;
            this.useVerboseTestLogsCheckbox.Text = "Verbose Test Logs";
            this.useVerboseTestLogsCheckbox.UseVisualStyleBackColor = true;
            this.useVerboseTestLogsCheckbox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // machineNameBox
            // 
            this.machineNameBox.Location = new System.Drawing.Point(92, 74);
            this.machineNameBox.Name = "machineNameBox";
            this.machineNameBox.Size = new System.Drawing.Size(185, 20);
            this.machineNameBox.TabIndex = 8;
            // 
            // configSaveSettingsBtn
            // 
            this.configSaveSettingsBtn.Location = new System.Drawing.Point(419, 20);
            this.configSaveSettingsBtn.Name = "configSaveSettingsBtn";
            this.configSaveSettingsBtn.Size = new System.Drawing.Size(102, 23);
            this.configSaveSettingsBtn.TabIndex = 2;
            this.configSaveSettingsBtn.Text = "Save Settings";
            this.configSaveSettingsBtn.UseVisualStyleBackColor = true;
            this.configSaveSettingsBtn.Click += new System.EventHandler(this.configSaveSettingsBtn_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Machine name:";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Os:";
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(92, 48);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(185, 20);
            this.ipBox.TabIndex = 4;
            // 
            // osVersionBox
            // 
            this.osVersionBox.Location = new System.Drawing.Point(49, 100);
            this.osVersionBox.Name = "osVersionBox";
            this.osVersionBox.ReadOnly = true;
            this.osVersionBox.Size = new System.Drawing.Size(351, 20);
            this.osVersionBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "My IP:";
            // 
            // hostnameBox
            // 
            this.hostnameBox.Location = new System.Drawing.Point(92, 22);
            this.hostnameBox.Name = "hostnameBox";
            this.hostnameBox.Size = new System.Drawing.Size(185, 20);
            this.hostnameBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "CTM Server:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // ctmStatusBar
            // 
            this.ctmStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctmStatusLabel});
            this.ctmStatusBar.Location = new System.Drawing.Point(0, 518);
            this.ctmStatusBar.Name = "ctmStatusBar";
            this.ctmStatusBar.Size = new System.Drawing.Size(766, 22);
            this.ctmStatusBar.TabIndex = 3;
            this.ctmStatusBar.Text = "statusStrip1";
            this.ctmStatusBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ctmStatusBar_ItemClicked);
            // 
            // ctmStatusLabel
            // 
            this.ctmStatusLabel.Name = "ctmStatusLabel";
            this.ctmStatusLabel.Size = new System.Drawing.Size(51, 17);
            this.ctmStatusLabel.Text = "Started..";
            this.ctmStatusLabel.Click += new System.EventHandler(this.toolStripStatusLabel1_Click_1);
            // 
            // callHomeTimer
            // 
            this.callHomeTimer.Enabled = true;
            this.callHomeTimer.Interval = 30000;
            this.callHomeTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lastRunLogBox
            // 
            this.lastRunLogBox.Location = new System.Drawing.Point(12, 182);
            this.lastRunLogBox.Multiline = true;
            this.lastRunLogBox.Name = "lastRunLogBox";
            this.lastRunLogBox.ReadOnly = true;
            this.lastRunLogBox.Size = new System.Drawing.Size(744, 304);
            this.lastRunLogBox.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Last Test Log:";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 540);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lastRunLogBox);
            this.Controls.Add(this.ctmStatusBar);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Main";
            this.Text = "Continuum Windows Testing Agent";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ctmStatusBar.ResumeLayout(false);
            this.ctmStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ieVersionBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox firefoxVersionBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox chromeVersionBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox safariVersionBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button configSaveSettingsBtn;
        private System.Windows.Forms.TextBox hostnameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ipBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.StatusStrip ctmStatusBar;
        private System.Windows.Forms.ToolStripStatusLabel ctmStatusLabel;
        private System.Windows.Forms.TextBox osVersionBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer callHomeTimer;
        private System.Windows.Forms.TextBox lastRunLogBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox machineNameBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox useVerboseTestLogsCheckbox;
        private System.Windows.Forms.TextBox guidBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button regenerateGuidBtn;
        private System.Windows.Forms.Button forcePollBtn;
        private System.Windows.Forms.TextBox buildBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox haltOnErrorBox;
    }
}

