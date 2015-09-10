namespace srcrepair
{
    partial class frmLogView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogView));
            this.LV_StatusBar = new System.Windows.Forms.StatusStrip();
            this.LV_Menu = new System.Windows.Forms.MenuStrip();
            this.LV_MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_MenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_MenuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_MenuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_MenuFileReload = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_MenuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_MenuEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_MenuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_MenuEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_MenuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.LV_Status = new System.Windows.Forms.ToolStripStatusLabel();
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
            // LV_Menu
            // 
            this.LV_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LV_MenuFile,
            this.LV_MenuEdit,
            this.LV_MenuHelp});
            resources.ApplyResources(this.LV_Menu, "LV_Menu");
            this.LV_Menu.Name = "LV_Menu";
            // 
            // LV_MenuFile
            // 
            this.LV_MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LV_MenuFileOpen,
            this.LV_MenuFileReload,
            this.LV_MenuFileExit});
            this.LV_MenuFile.Name = "LV_MenuFile";
            resources.ApplyResources(this.LV_MenuFile, "LV_MenuFile");
            // 
            // LV_MenuEdit
            // 
            this.LV_MenuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LV_MenuEditCut,
            this.LV_MenuEditCopy,
            this.LV_MenuEditPaste});
            this.LV_MenuEdit.Name = "LV_MenuEdit";
            resources.ApplyResources(this.LV_MenuEdit, "LV_MenuEdit");
            // 
            // LV_MenuHelp
            // 
            this.LV_MenuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LV_MenuHelpAbout});
            this.LV_MenuHelp.Name = "LV_MenuHelp";
            resources.ApplyResources(this.LV_MenuHelp, "LV_MenuHelp");
            // 
            // LV_MenuFileOpen
            // 
            this.LV_MenuFileOpen.Name = "LV_MenuFileOpen";
            resources.ApplyResources(this.LV_MenuFileOpen, "LV_MenuFileOpen");
            // 
            // LV_MenuFileReload
            // 
            this.LV_MenuFileReload.Name = "LV_MenuFileReload";
            resources.ApplyResources(this.LV_MenuFileReload, "LV_MenuFileReload");
            // 
            // LV_MenuFileExit
            // 
            this.LV_MenuFileExit.Name = "LV_MenuFileExit";
            resources.ApplyResources(this.LV_MenuFileExit, "LV_MenuFileExit");
            // 
            // LV_MenuEditCut
            // 
            this.LV_MenuEditCut.Name = "LV_MenuEditCut";
            resources.ApplyResources(this.LV_MenuEditCut, "LV_MenuEditCut");
            // 
            // LV_MenuEditCopy
            // 
            this.LV_MenuEditCopy.Name = "LV_MenuEditCopy";
            resources.ApplyResources(this.LV_MenuEditCopy, "LV_MenuEditCopy");
            // 
            // LV_MenuEditPaste
            // 
            this.LV_MenuEditPaste.Name = "LV_MenuEditPaste";
            resources.ApplyResources(this.LV_MenuEditPaste, "LV_MenuEditPaste");
            // 
            // LV_MenuHelpAbout
            // 
            this.LV_MenuHelpAbout.Name = "LV_MenuHelpAbout";
            resources.ApplyResources(this.LV_MenuHelpAbout, "LV_MenuHelpAbout");
            // 
            // LV_Status
            // 
            this.LV_Status.Name = "LV_Status";
            resources.ApplyResources(this.LV_Status, "LV_Status");
            // 
            // LV_LogArea
            // 
            resources.ApplyResources(this.LV_LogArea, "LV_LogArea");
            this.LV_LogArea.Name = "LV_LogArea";
            // 
            // frmLogView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LV_LogArea);
            this.Controls.Add(this.LV_StatusBar);
            this.Controls.Add(this.LV_Menu);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.LV_Menu;
            this.Name = "frmLogView";
            this.Load += new System.EventHandler(this.frmLogView_Load);
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
        private System.Windows.Forms.ToolStripMenuItem LV_MenuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem LV_MenuFileReload;
        private System.Windows.Forms.ToolStripMenuItem LV_MenuFileExit;
        private System.Windows.Forms.ToolStripMenuItem LV_MenuEdit;
        private System.Windows.Forms.ToolStripMenuItem LV_MenuEditCut;
        private System.Windows.Forms.ToolStripMenuItem LV_MenuEditCopy;
        private System.Windows.Forms.ToolStripMenuItem LV_MenuEditPaste;
        private System.Windows.Forms.ToolStripMenuItem LV_MenuHelp;
        private System.Windows.Forms.ToolStripMenuItem LV_MenuHelpAbout;
        private System.Windows.Forms.TextBox LV_LogArea;
    }
}