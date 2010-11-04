namespace Continuum_Windows_Testing_Agent
{
    partial class CTM_Test
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
            this.activeTestGrid = new System.Windows.Forms.DataGridView();
            this.pauseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.testRunNameBox = new System.Windows.Forms.TextBox();
            this.testRunProgressBar = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.testNameBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.testRunWorker = new System.ComponentModel.BackgroundWorker();
            this.command = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.target = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.elapsed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.activeTestGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // activeTestGrid
            // 
            this.activeTestGrid.AllowUserToAddRows = false;
            this.activeTestGrid.AllowUserToDeleteRows = false;
            this.activeTestGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.activeTestGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.activeTestGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.activeTestGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.activeTestGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.command,
            this.target,
            this.value,
            this.start,
            this.stop,
            this.elapsed,
            this.message});
            this.activeTestGrid.Location = new System.Drawing.Point(11, 66);
            this.activeTestGrid.Name = "activeTestGrid";
            this.activeTestGrid.Size = new System.Drawing.Size(677, 506);
            this.activeTestGrid.TabIndex = 0;
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(613, 9);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(75, 48);
            this.pauseButton.TabIndex = 1;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Test Run:";
            // 
            // testRunNameBox
            // 
            this.testRunNameBox.Location = new System.Drawing.Point(69, 11);
            this.testRunNameBox.Name = "testRunNameBox";
            this.testRunNameBox.ReadOnly = true;
            this.testRunNameBox.Size = new System.Drawing.Size(207, 20);
            this.testRunNameBox.TabIndex = 3;
            // 
            // testRunProgressBar
            // 
            this.testRunProgressBar.Location = new System.Drawing.Point(368, 9);
            this.testRunProgressBar.Name = "testRunProgressBar";
            this.testRunProgressBar.Size = new System.Drawing.Size(239, 23);
            this.testRunProgressBar.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(293, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Run Progress:";
            // 
            // testNameBox
            // 
            this.testNameBox.Location = new System.Drawing.Point(69, 37);
            this.testNameBox.Name = "testNameBox";
            this.testNameBox.ReadOnly = true;
            this.testNameBox.Size = new System.Drawing.Size(538, 20);
            this.testNameBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Test:";
            // 
            // testRunWorker
            // 
            this.testRunWorker.WorkerReportsProgress = true;
            this.testRunWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.testRunWorker_DoWork);
            // 
            // command
            // 
            this.command.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.command.HeaderText = "Command";
            this.command.Name = "command";
            this.command.ReadOnly = true;
            // 
            // target
            // 
            this.target.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.target.HeaderText = "Target";
            this.target.Name = "target";
            this.target.ReadOnly = true;
            // 
            // value
            // 
            this.value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.value.HeaderText = "Value";
            this.value.Name = "value";
            this.value.ReadOnly = true;
            // 
            // start
            // 
            this.start.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.start.HeaderText = "Start";
            this.start.Name = "start";
            this.start.ReadOnly = true;
            // 
            // stop
            // 
            this.stop.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.stop.HeaderText = "Stop";
            this.stop.Name = "stop";
            this.stop.ReadOnly = true;
            // 
            // elapsed
            // 
            this.elapsed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.elapsed.HeaderText = "Elapsed";
            this.elapsed.Name = "elapsed";
            this.elapsed.ReadOnly = true;
            // 
            // message
            // 
            this.message.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.message.HeaderText = "Message";
            this.message.Name = "message";
            this.message.ReadOnly = true;
            // 
            // CTM_Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 584);
            this.Controls.Add(this.testNameBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.testRunProgressBar);
            this.Controls.Add(this.testRunNameBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.activeTestGrid);
            this.Name = "CTM_Test";
            this.Text = "CTM Test Run";
            ((System.ComponentModel.ISupportInitialize)(this.activeTestGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView activeTestGrid;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox testRunNameBox;
        private System.Windows.Forms.ProgressBar testRunProgressBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox testNameBox;
        private System.Windows.Forms.Label label4;
        public System.ComponentModel.BackgroundWorker testRunWorker;
        private System.Windows.Forms.DataGridViewTextBoxColumn command;
        private System.Windows.Forms.DataGridViewTextBoxColumn target;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
        private System.Windows.Forms.DataGridViewTextBoxColumn start;
        private System.Windows.Forms.DataGridViewTextBoxColumn stop;
        private System.Windows.Forms.DataGridViewTextBoxColumn elapsed;
        private System.Windows.Forms.DataGridViewTextBoxColumn message;
    }
}