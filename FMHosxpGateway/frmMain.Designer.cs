namespace FMHosxpGateway
{
    partial class frmMain
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmManagement = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmWorkListManagement = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSeverConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmClientConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmdStart = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmManagement,
            this.tsmConfig});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(559, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmManagement
            // 
            this.tsmManagement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmWorkListManagement});
            this.tsmManagement.Name = "tsmManagement";
            this.tsmManagement.Size = new System.Drawing.Size(90, 20);
            this.tsmManagement.Text = "&Management";
            this.tsmManagement.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsmWorkListManagement
            // 
            this.tsmWorkListManagement.Name = "tsmWorkListManagement";
            this.tsmWorkListManagement.Size = new System.Drawing.Size(191, 22);
            this.tsmWorkListManagement.Text = "Worklist Management";
            // 
            // tsmConfig
            // 
            this.tsmConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmSeverConfig,
            this.tsmClientConfig});
            this.tsmConfig.Name = "tsmConfig";
            this.tsmConfig.Size = new System.Drawing.Size(55, 20);
            this.tsmConfig.Text = "&Config";
            // 
            // tsmSeverConfig
            // 
            this.tsmSeverConfig.Name = "tsmSeverConfig";
            this.tsmSeverConfig.Size = new System.Drawing.Size(145, 22);
            this.tsmSeverConfig.Text = "Server Config";
            // 
            // tsmClientConfig
            // 
            this.tsmClientConfig.Name = "tsmClientConfig";
            this.tsmClientConfig.Size = new System.Drawing.Size(145, 22);
            this.tsmClientConfig.Text = "Client Config";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // cmdStart
            // 
            this.cmdStart.Location = new System.Drawing.Point(472, 470);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(75, 23);
            this.cmdStart.TabIndex = 3;
            this.cmdStart.Text = "Start";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstLog);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 437);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // lstLog
            // 
            this.lstLog.FormattingEnabled = true;
            this.lstLog.Location = new System.Drawing.Point(6, 19);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(523, 407);
            this.lstLog.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 505);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdStart);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Fine Med Hoxp Gateway";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmConfig;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmSeverConfig;
        private System.Windows.Forms.ToolStripMenuItem tsmClientConfig;
        private System.Windows.Forms.ToolStripMenuItem tsmManagement;
        private System.Windows.Forms.ToolStripMenuItem tsmWorkListManagement;
        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstLog;
    }
}

