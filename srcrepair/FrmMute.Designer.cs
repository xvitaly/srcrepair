namespace srcrepair
{
    partial class FrmMute
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMute));
            this.MM_Menu = new System.Windows.Forms.MenuStrip();
            this.MM_File = new System.Windows.Forms.ToolStripMenuItem();
            this.MM_FReload = new System.Windows.Forms.ToolStripMenuItem();
            this.MM_FSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MM_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.MM_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.MM_HAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.MM_StatusBar = new System.Windows.Forms.StatusStrip();
            this.MM_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.MM_Table = new System.Windows.Forms.DataGridView();
            this.SteamID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MM_Toolbar = new System.Windows.Forms.ToolStrip();
            this.MM_Refresh = new System.Windows.Forms.ToolStripButton();
            this.MM_Save = new System.Windows.Forms.ToolStripButton();
            this.MM_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            this.MM_Cut = new System.Windows.Forms.ToolStripButton();
            this.MM_Copy = new System.Windows.Forms.ToolStripButton();
            this.MM_Paste = new System.Windows.Forms.ToolStripButton();
            this.MM_Sep2 = new System.Windows.Forms.ToolStripSeparator();
            this.MM_Delete = new System.Windows.Forms.ToolStripButton();
            this.MM_Sep3 = new System.Windows.Forms.ToolStripSeparator();
            this.MM_About = new System.Windows.Forms.ToolStripButton();
            this.MM_Menu.SuspendLayout();
            this.MM_StatusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MM_Table)).BeginInit();
            this.MM_Toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // MM_Menu
            // 
            this.MM_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MM_File,
            this.MM_Help});
            resources.ApplyResources(this.MM_Menu, "MM_Menu");
            this.MM_Menu.Name = "MM_Menu";
            // 
            // MM_File
            // 
            this.MM_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MM_FReload,
            this.MM_FSave,
            this.MM_Exit});
            this.MM_File.Name = "MM_File";
            resources.ApplyResources(this.MM_File, "MM_File");
            // 
            // MM_FReload
            // 
            this.MM_FReload.Image = global::srcrepair.Properties.Resources.Refresh;
            this.MM_FReload.Name = "MM_FReload";
            resources.ApplyResources(this.MM_FReload, "MM_FReload");
            this.MM_FReload.Click += new System.EventHandler(this.UpdateTable);
            // 
            // MM_FSave
            // 
            this.MM_FSave.Image = global::srcrepair.Properties.Resources.Save;
            this.MM_FSave.Name = "MM_FSave";
            resources.ApplyResources(this.MM_FSave, "MM_FSave");
            this.MM_FSave.Click += new System.EventHandler(this.WriteTable);
            // 
            // MM_Exit
            // 
            this.MM_Exit.Image = global::srcrepair.Properties.Resources.Exit;
            this.MM_Exit.Name = "MM_Exit";
            resources.ApplyResources(this.MM_Exit, "MM_Exit");
            this.MM_Exit.Click += new System.EventHandler(this.MM_Exit_Click);
            // 
            // MM_Help
            // 
            this.MM_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MM_HAbout});
            this.MM_Help.Name = "MM_Help";
            resources.ApplyResources(this.MM_Help, "MM_Help");
            // 
            // MM_HAbout
            // 
            this.MM_HAbout.Image = global::srcrepair.Properties.Resources.Info;
            this.MM_HAbout.Name = "MM_HAbout";
            resources.ApplyResources(this.MM_HAbout, "MM_HAbout");
            this.MM_HAbout.Click += new System.EventHandler(this.AboutDlg);
            // 
            // MM_StatusBar
            // 
            this.MM_StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MM_Status});
            resources.ApplyResources(this.MM_StatusBar, "MM_StatusBar");
            this.MM_StatusBar.Name = "MM_StatusBar";
            // 
            // MM_Status
            // 
            this.MM_Status.Name = "MM_Status";
            resources.ApplyResources(this.MM_Status, "MM_Status");
            // 
            // MM_Table
            // 
            this.MM_Table.BackgroundColor = System.Drawing.SystemColors.Window;
            this.MM_Table.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MM_Table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MM_Table.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SteamID});
            resources.ApplyResources(this.MM_Table, "MM_Table");
            this.MM_Table.Name = "MM_Table";
            // 
            // SteamID
            // 
            resources.ApplyResources(this.SteamID, "SteamID");
            this.SteamID.Name = "SteamID";
            // 
            // MM_Toolbar
            // 
            this.MM_Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MM_Refresh,
            this.MM_Save,
            this.MM_Sep1,
            this.MM_Cut,
            this.MM_Copy,
            this.MM_Paste,
            this.MM_Sep2,
            this.MM_Delete,
            this.MM_Sep3,
            this.MM_About});
            resources.ApplyResources(this.MM_Toolbar, "MM_Toolbar");
            this.MM_Toolbar.Name = "MM_Toolbar";
            // 
            // MM_Refresh
            // 
            this.MM_Refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MM_Refresh.Image = global::srcrepair.Properties.Resources.Refresh;
            resources.ApplyResources(this.MM_Refresh, "MM_Refresh");
            this.MM_Refresh.Name = "MM_Refresh";
            this.MM_Refresh.Click += new System.EventHandler(this.UpdateTable);
            // 
            // MM_Save
            // 
            this.MM_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.MM_Save, "MM_Save");
            this.MM_Save.Name = "MM_Save";
            this.MM_Save.Click += new System.EventHandler(this.WriteTable);
            // 
            // MM_Sep1
            // 
            this.MM_Sep1.Name = "MM_Sep1";
            resources.ApplyResources(this.MM_Sep1, "MM_Sep1");
            // 
            // MM_Cut
            // 
            this.MM_Cut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.MM_Cut, "MM_Cut");
            this.MM_Cut.Name = "MM_Cut";
            this.MM_Cut.Click += new System.EventHandler(this.MM_Cut_Click);
            // 
            // MM_Copy
            // 
            this.MM_Copy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.MM_Copy, "MM_Copy");
            this.MM_Copy.Name = "MM_Copy";
            this.MM_Copy.Click += new System.EventHandler(this.MM_Copy_Click);
            // 
            // MM_Paste
            // 
            this.MM_Paste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.MM_Paste, "MM_Paste");
            this.MM_Paste.Name = "MM_Paste";
            this.MM_Paste.Click += new System.EventHandler(this.MM_Paste_Click);
            // 
            // MM_Sep2
            // 
            this.MM_Sep2.Name = "MM_Sep2";
            resources.ApplyResources(this.MM_Sep2, "MM_Sep2");
            // 
            // MM_Delete
            // 
            this.MM_Delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MM_Delete.Image = global::srcrepair.Properties.Resources.Delete;
            resources.ApplyResources(this.MM_Delete, "MM_Delete");
            this.MM_Delete.Name = "MM_Delete";
            this.MM_Delete.Click += new System.EventHandler(this.MM_Delete_Click);
            // 
            // MM_Sep3
            // 
            this.MM_Sep3.Name = "MM_Sep3";
            resources.ApplyResources(this.MM_Sep3, "MM_Sep3");
            // 
            // MM_About
            // 
            this.MM_About.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.MM_About, "MM_About");
            this.MM_About.Name = "MM_About";
            this.MM_About.Click += new System.EventHandler(this.AboutDlg);
            // 
            // FrmMute
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MM_Toolbar);
            this.Controls.Add(this.MM_Table);
            this.Controls.Add(this.MM_StatusBar);
            this.Controls.Add(this.MM_Menu);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.MM_Menu;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMute";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.FrmMute_Load);
            this.MM_Menu.ResumeLayout(false);
            this.MM_Menu.PerformLayout();
            this.MM_StatusBar.ResumeLayout(false);
            this.MM_StatusBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MM_Table)).EndInit();
            this.MM_Toolbar.ResumeLayout(false);
            this.MM_Toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MM_Menu;
        private System.Windows.Forms.StatusStrip MM_StatusBar;
        private System.Windows.Forms.ToolStripMenuItem MM_File;
        private System.Windows.Forms.ToolStripMenuItem MM_FReload;
        private System.Windows.Forms.ToolStripMenuItem MM_FSave;
        private System.Windows.Forms.ToolStripMenuItem MM_Exit;
        private System.Windows.Forms.ToolStripMenuItem MM_Help;
        private System.Windows.Forms.ToolStripMenuItem MM_HAbout;
        private System.Windows.Forms.ToolStripStatusLabel MM_Status;
        private System.Windows.Forms.DataGridView MM_Table;
        private System.Windows.Forms.ToolStrip MM_Toolbar;
        private System.Windows.Forms.ToolStripButton MM_Refresh;
        private System.Windows.Forms.ToolStripButton MM_Save;
        private System.Windows.Forms.ToolStripSeparator MM_Sep1;
        private System.Windows.Forms.ToolStripButton MM_Cut;
        private System.Windows.Forms.ToolStripButton MM_Copy;
        private System.Windows.Forms.ToolStripButton MM_Paste;
        private System.Windows.Forms.ToolStripSeparator MM_Sep2;
        private System.Windows.Forms.DataGridViewTextBoxColumn SteamID;
        private System.Windows.Forms.ToolStripButton MM_About;
        private System.Windows.Forms.ToolStripButton MM_Delete;
        private System.Windows.Forms.ToolStripSeparator MM_Sep3;
    }
}