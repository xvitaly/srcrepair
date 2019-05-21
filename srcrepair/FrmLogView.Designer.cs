namespace srcrepair.gui
{
    partial class FrmLogView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogView));
            this.LV_StatusBar = new System.Windows.Forms.StatusStrip();
            this.LV_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.LV_Menu = new System.Windows.Forms.MenuStrip();
            this.LV_MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_MenuFileReload = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_MunuFileClearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            this.LV_MenuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_MenuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_MenuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_LogArea = new System.Windows.Forms.TextBox();
            this.LV_StatusBar.SuspendLayout();
            this.LV_Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // LV_StatusBar
            // 
            this.LV_StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LV_Status});
            resources.ApplyResources(this.LV_StatusBar, "LV_StatusBar");
            this.LV_StatusBar.Name = "LV_StatusBar";
            // 
            // LV_Status
            // 
            this.LV_Status.Name = "LV_Status";
            resources.ApplyResources(this.LV_Status, "LV_Status");
            // 
            // LV_Menu
            // 
            this.LV_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LV_MenuFile,
            this.LV_MenuHelp});
            resources.ApplyResources(this.LV_Menu, "LV_Menu");
            this.LV_Menu.Name = "LV_Menu";
            // 
            // LV_MenuFile
            // 
            this.LV_MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LV_MenuFileReload,
            this.LV_MunuFileClearLog,
            this.LV_Sep1,
            this.LV_MenuFileExit});
            this.LV_MenuFile.Name = "LV_MenuFile";
            resources.ApplyResources(this.LV_MenuFile, "LV_MenuFile");
            // 
            // LV_MenuFileReload
            // 
            this.LV_MenuFileReload.Image = global::srcrepair.Properties.Resources.Refresh;
            this.LV_MenuFileReload.Name = "LV_MenuFileReload";
            resources.ApplyResources(this.LV_MenuFileReload, "LV_MenuFileReload");
            this.LV_MenuFileReload.Click += new System.EventHandler(this.LV_MenuFileReload_Click);
            // 
            // LV_MunuFileClearLog
            // 
            this.LV_MunuFileClearLog.Image = global::srcrepair.Properties.Resources.clean;
            this.LV_MunuFileClearLog.Name = "LV_MunuFileClearLog";
            resources.ApplyResources(this.LV_MunuFileClearLog, "LV_MunuFileClearLog");
            this.LV_MunuFileClearLog.Click += new System.EventHandler(this.LV_MunuFileClearLog_Click);
            // 
            // LV_Sep1
            // 
            this.LV_Sep1.Name = "LV_Sep1";
            resources.ApplyResources(this.LV_Sep1, "LV_Sep1");
            // 
            // LV_MenuFileExit
            // 
            this.LV_MenuFileExit.Image = global::srcrepair.Properties.Resources.Exit;
            this.LV_MenuFileExit.Name = "LV_MenuFileExit";
            resources.ApplyResources(this.LV_MenuFileExit, "LV_MenuFileExit");
            this.LV_MenuFileExit.Click += new System.EventHandler(this.LV_MenuFileExit_Click);
            // 
            // LV_MenuHelp
            // 
            this.LV_MenuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LV_MenuHelpAbout});
            this.LV_MenuHelp.Name = "LV_MenuHelp";
            resources.ApplyResources(this.LV_MenuHelp, "LV_MenuHelp");
            // 
            // LV_MenuHelpAbout
            // 
            this.LV_MenuHelpAbout.Image = global::srcrepair.Properties.Resources.Info;
            this.LV_MenuHelpAbout.Name = "LV_MenuHelpAbout";
            resources.ApplyResources(this.LV_MenuHelpAbout, "LV_MenuHelpAbout");
            this.LV_MenuHelpAbout.Click += new System.EventHandler(this.LV_MenuHelpAbout_Click);
            // 
            // LV_LogArea
            // 
            resources.ApplyResources(this.LV_LogArea, "LV_LogArea");
            this.LV_LogArea.Name = "LV_LogArea";
            // 
            // FrmLogView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LV_LogArea);
            this.Controls.Add(this.LV_StatusBar);
            this.Controls.Add(this.LV_Menu);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.LV_Menu;
            this.MaximizeBox = false;
            this.Name = "FrmLogView";
            this.Load += new System.EventHandler(this.FrmLogView_Load);
            this.LV_StatusBar.ResumeLayout(false);
            this.LV_StatusBar.PerformLayout();
            this.LV_Menu.ResumeLayout(false);
            this.LV_Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip LV_StatusBar;
        private System.Windows.Forms.ToolStripStatusLabel LV_Status;
        private System.Windows.Forms.MenuStrip LV_Menu;
        private System.Windows.Forms.ToolStripMenuItem LV_MenuFile;
        private System.Windows.Forms.ToolStripMenuItem LV_MenuFileReload;
        private System.Windows.Forms.ToolStripMenuItem LV_MenuFileExit;
        private System.Windows.Forms.ToolStripMenuItem LV_MenuHelp;
        private System.Windows.Forms.ToolStripMenuItem LV_MenuHelpAbout;
        private System.Windows.Forms.TextBox LV_LogArea;
        private System.Windows.Forms.ToolStripSeparator LV_Sep1;
        private System.Windows.Forms.ToolStripMenuItem LV_MunuFileClearLog;
    }
}