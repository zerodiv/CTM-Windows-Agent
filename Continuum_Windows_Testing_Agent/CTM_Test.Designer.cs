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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.testRunNameBox = new System.Windows.Forms.TextBox();
            this.testProgressBar = new System.Windows.Forms.ProgressBar();
            this.command = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.target = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.command,
            this.target,
            this.value});
            this.dataGridView1.Location = new System.Drawing.Point(11, 48);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(677, 524);
            this.dataGridView1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(613, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Pause";
            this.button1.UseVisualStyleBackColor = true;
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
            // testProgressBar
            // 
            this.testProgressBar.Location = new System.Drawing.Point(282, 9);
            this.testProgressBar.Name = "testProgressBar";
            this.testProgressBar.Size = new System.Drawing.Size(325, 23);
            this.testProgressBar.TabIndex = 4;
            // 
            // command
            // 
            this.command.HeaderText = "Command";
            this.command.Name = "command";
            // 
            // target
            // 
            this.target.HeaderText = "Target";
            this.target.Name = "target";
            // 
            // value
            // 
            this.value.HeaderText = "Value";
            this.value.Name = "value";
            // 
            // CTM_Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 584);
            this.Controls.Add(this.testProgressBar);
            this.Controls.Add(this.testRunNameBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "CTM_Test";
            this.Text = "CTM Test Run";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox testRunNameBox;
        private System.Windows.Forms.ProgressBar testProgressBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn command;
        private System.Windows.Forms.DataGridViewTextBoxColumn target;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
    }
}