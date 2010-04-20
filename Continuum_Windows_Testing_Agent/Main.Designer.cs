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
            this.ipBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.configSaveSettingsBtn = new System.Windows.Forms.Button();
            this.hostnameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ctmStatusBar = new System.Windows.Forms.StatusStrip();
            this.ctmStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.osVersionBox = new System.Windows.Forms.TextBox();
            this.callHomeTimer = new System.Windows.Forms.Timer(this.components);
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
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(195, 139);
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
            this.groupBox2.Controls.Add(this.ipBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.configSaveSettingsBtn);
            this.groupBox2.Controls.Add(this.hostnameBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(222, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(280, 139);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Config";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(70, 53);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(185, 20);
            this.ipBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "IP:";
            // 
            // configSaveSettingsBtn
            // 
            this.configSaveSettingsBtn.Location = new System.Drawing.Point(109, 82);
            this.configSaveSettingsBtn.Name = "configSaveSettingsBtn";
            this.configSaveSettingsBtn.Size = new System.Drawing.Size(83, 19);
            this.configSaveSettingsBtn.TabIndex = 2;
            this.configSaveSettingsBtn.Text = "Save Settings";
            this.configSaveSettingsBtn.UseVisualStyleBackColor = true;
            this.configSaveSettingsBtn.Click += new System.EventHandler(this.configSaveSettingsBtn_Click);
            // 
            // hostnameBox
            // 
            this.hostnameBox.Location = new System.Drawing.Point(70, 22);
            this.hostnameBox.Name = "hostnameBox";
            this.hostnameBox.Size = new System.Drawing.Size(185, 20);
            this.hostnameBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Hostname:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // ctmStatusBar
            // 
            this.ctmStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctmStatusLabel});
            this.ctmStatusBar.Location = new System.Drawing.Point(0, 185);
            this.ctmStatusBar.Name = "ctmStatusBar";
            this.ctmStatusBar.Size = new System.Drawing.Size(516, 22);
            this.ctmStatusBar.TabIndex = 3;
            this.ctmStatusBar.Text = "statusStrip1";
            // 
            // ctmStatusLabel
            // 
            this.ctmStatusLabel.Name = "ctmStatusLabel";
            this.ctmStatusLabel.Size = new System.Drawing.Size(51, 17);
            this.ctmStatusLabel.Text = "Started..";
            this.ctmStatusLabel.Click += new System.EventHandler(this.toolStripStatusLabel1_Click_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Os:";
            // 
            // osVersionBox
            // 
            this.osVersionBox.Location = new System.Drawing.Point(74, 157);
            this.osVersionBox.Name = "osVersionBox";
            this.osVersionBox.ReadOnly = true;
            this.osVersionBox.Size = new System.Drawing.Size(351, 20);
            this.osVersionBox.TabIndex = 6;
            // 
            // callHomeTimer
            // 
            this.callHomeTimer.Enabled = true;
            this.callHomeTimer.Interval = 30000;
            this.callHomeTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 207);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.osVersionBox);
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
    }
}

