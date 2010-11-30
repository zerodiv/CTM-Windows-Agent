namespace Continuum_Windows_Testing_Agent
{
    partial class CTM_Agent
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.haltOnErrorBox = new System.Windows.Forms.CheckBox();
            this.browserGrid = new System.Windows.Forms.DataGridView();
            this.isAvailable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.browserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.browserVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hostname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.internalName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.forcePollBtn = new System.Windows.Forms.Button();
            this.regenerateGuidBtn = new System.Windows.Forms.Button();
            this.guidBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.machineNameBox = new System.Windows.Forms.TextBox();
            this.configSaveSettingsBtn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.localIpBox = new System.Windows.Forms.TextBox();
            this.osVersionBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ctmHostnameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ctmStatusBar = new System.Windows.Forms.StatusStrip();
            this.ctmStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.callHomeTimer = new System.Windows.Forms.Timer(this.components);
            this.lastRunLogBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.agentBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.browserGrid)).BeginInit();
            this.ctmStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.haltOnErrorBox);
            this.groupBox2.Controls.Add(this.browserGrid);
            this.groupBox2.Controls.Add(this.forcePollBtn);
            this.groupBox2.Controls.Add(this.regenerateGuidBtn);
            this.groupBox2.Controls.Add(this.guidBox);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.machineNameBox);
            this.groupBox2.Controls.Add(this.configSaveSettingsBtn);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.localIpBox);
            this.groupBox2.Controls.Add(this.osVersionBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.ctmHostnameBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(734, 276);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Config";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Browsers:";
            // 
            // haltOnErrorBox
            // 
            this.haltOnErrorBox.AutoSize = true;
            this.haltOnErrorBox.Location = new System.Drawing.Point(478, 25);
            this.haltOnErrorBox.Name = "haltOnErrorBox";
            this.haltOnErrorBox.Size = new System.Drawing.Size(87, 17);
            this.haltOnErrorBox.TabIndex = 14;
            this.haltOnErrorBox.Text = "Halt On Error";
            this.haltOnErrorBox.UseVisualStyleBackColor = true;
            this.haltOnErrorBox.CheckedChanged += new System.EventHandler(this.haltOnErrorBox_CheckedChanged);
            // 
            // browserGrid
            // 
            this.browserGrid.AllowUserToAddRows = false;
            this.browserGrid.AllowUserToDeleteRows = false;
            this.browserGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.browserGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.isAvailable,
            this.browserName,
            this.browserVersion,
            this.hostname,
            this.port,
            this.internalName});
            this.browserGrid.Location = new System.Drawing.Point(9, 120);
            this.browserGrid.Name = "browserGrid";
            this.browserGrid.Size = new System.Drawing.Size(718, 150);
            this.browserGrid.TabIndex = 19;
            // 
            // isAvailable
            // 
            this.isAvailable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.isAvailable.HeaderText = "Available";
            this.isAvailable.Name = "isAvailable";
            // 
            // browserName
            // 
            this.browserName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.browserName.HeaderText = "Browser";
            this.browserName.Name = "browserName";
            this.browserName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.browserName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // browserVersion
            // 
            this.browserVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.browserVersion.HeaderText = "Version";
            this.browserVersion.Name = "browserVersion";
            // 
            // hostname
            // 
            this.hostname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.hostname.HeaderText = "Host";
            this.hostname.Name = "hostname";
            // 
            // port
            // 
            this.port.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.port.HeaderText = "Port";
            this.port.Name = "port";
            // 
            // internalName
            // 
            this.internalName.HeaderText = "Internal Name";
            this.internalName.Name = "internalName";
            this.internalName.Visible = false;
            // 
            // forcePollBtn
            // 
            this.forcePollBtn.Location = new System.Drawing.Point(625, 51);
            this.forcePollBtn.Name = "forcePollBtn";
            this.forcePollBtn.Size = new System.Drawing.Size(102, 23);
            this.forcePollBtn.TabIndex = 13;
            this.forcePollBtn.Text = "Force Poll";
            this.forcePollBtn.UseVisualStyleBackColor = true;
            this.forcePollBtn.Click += new System.EventHandler(this.forcePollBtn_Click);
            // 
            // regenerateGuidBtn
            // 
            this.regenerateGuidBtn.Location = new System.Drawing.Point(281, 78);
            this.regenerateGuidBtn.Name = "regenerateGuidBtn";
            this.regenerateGuidBtn.Size = new System.Drawing.Size(102, 23);
            this.regenerateGuidBtn.TabIndex = 12;
            this.regenerateGuidBtn.Text = "Regenerate GUID";
            this.regenerateGuidBtn.UseVisualStyleBackColor = true;
            this.regenerateGuidBtn.Click += new System.EventHandler(this.regenerateGuidBtn_Click);
            // 
            // guidBox
            // 
            this.guidBox.Location = new System.Drawing.Point(54, 80);
            this.guidBox.Name = "guidBox";
            this.guidBox.ReadOnly = true;
            this.guidBox.Size = new System.Drawing.Size(221, 20);
            this.guidBox.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 83);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "GUID:";
            // 
            // machineNameBox
            // 
            this.machineNameBox.Location = new System.Drawing.Point(92, 48);
            this.machineNameBox.Name = "machineNameBox";
            this.machineNameBox.Size = new System.Drawing.Size(147, 20);
            this.machineNameBox.TabIndex = 8;
            // 
            // configSaveSettingsBtn
            // 
            this.configSaveSettingsBtn.Location = new System.Drawing.Point(625, 19);
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
            this.label9.Location = new System.Drawing.Point(6, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Machine name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(402, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Os:";
            // 
            // localIpBox
            // 
            this.localIpBox.Location = new System.Drawing.Point(294, 22);
            this.localIpBox.Name = "localIpBox";
            this.localIpBox.Size = new System.Drawing.Size(147, 20);
            this.localIpBox.TabIndex = 4;
            // 
            // osVersionBox
            // 
            this.osVersionBox.Location = new System.Drawing.Point(445, 80);
            this.osVersionBox.Name = "osVersionBox";
            this.osVersionBox.ReadOnly = true;
            this.osVersionBox.Size = new System.Drawing.Size(282, 20);
            this.osVersionBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(251, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "My IP:";
            // 
            // ctmHostnameBox
            // 
            this.ctmHostnameBox.Location = new System.Drawing.Point(92, 22);
            this.ctmHostnameBox.Name = "ctmHostnameBox";
            this.ctmHostnameBox.Size = new System.Drawing.Size(147, 20);
            this.ctmHostnameBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "CTM Server:";
            // 
            // ctmStatusBar
            // 
            this.ctmStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctmStatusLabel});
            this.ctmStatusBar.Location = new System.Drawing.Point(0, 518);
            this.ctmStatusBar.Name = "ctmStatusBar";
            this.ctmStatusBar.Size = new System.Drawing.Size(756, 22);
            this.ctmStatusBar.TabIndex = 3;
            this.ctmStatusBar.Text = "statusStrip1";
            // 
            // ctmStatusLabel
            // 
            this.ctmStatusLabel.Name = "ctmStatusLabel";
            this.ctmStatusLabel.Size = new System.Drawing.Size(50, 17);
            this.ctmStatusLabel.Text = "Started..";
            // 
            // callHomeTimer
            // 
            this.callHomeTimer.Enabled = true;
            this.callHomeTimer.Interval = 30000;
            this.callHomeTimer.Tick += new System.EventHandler(this.callHomeTimer_Tick);
            // 
            // lastRunLogBox
            // 
            this.lastRunLogBox.Location = new System.Drawing.Point(12, 327);
            this.lastRunLogBox.Multiline = true;
            this.lastRunLogBox.Name = "lastRunLogBox";
            this.lastRunLogBox.ReadOnly = true;
            this.lastRunLogBox.Size = new System.Drawing.Size(734, 179);
            this.lastRunLogBox.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 294);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Last Test Log:";
            // 
            // agentBackgroundWorker
            // 
            this.agentBackgroundWorker.WorkerSupportsCancellation = true;
            // 
            // CTM_Agent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 540);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lastRunLogBox);
            this.Controls.Add(this.ctmStatusBar);
            this.Controls.Add(this.groupBox2);
            this.Name = "CTM_Agent";
            this.Text = "Continuum Windows Testing Agent";
            this.Load += new System.EventHandler(this.CTM_Agent_Load_1);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.browserGrid)).EndInit();
            this.ctmStatusBar.ResumeLayout(false);
            this.ctmStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button configSaveSettingsBtn;
        private System.Windows.Forms.TextBox ctmHostnameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox localIpBox;
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
        private System.Windows.Forms.TextBox guidBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button regenerateGuidBtn;
        private System.Windows.Forms.Button forcePollBtn;
        private System.Windows.Forms.CheckBox haltOnErrorBox;
        private System.ComponentModel.BackgroundWorker agentBackgroundWorker;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView browserGrid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isAvailable;
        private System.Windows.Forms.DataGridViewTextBoxColumn browserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn browserVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostname;
        private System.Windows.Forms.DataGridViewTextBoxColumn port;
        private System.Windows.Forms.DataGridViewTextBoxColumn internalName;
    }
}

