namespace srcrepair.gui
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
            this.components = new System.ComponentModel.Container();
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
            this.MM_Context = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MM_CCut = new System.Windows.Forms.ToolStripMenuItem();
            this.MM_CCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.MM_CPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.MM_Sep4 = new System.Windows.Forms.ToolStripSeparator();
            this.MM_CRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.MM_CConvert = new System.Windows.Forms.ToolStripMenuItem();
            this.showSteamProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MM_Toolbar = new System.Windows.Forms.ToolStrip();
            this.MM_Refresh = new System.Windows.Forms.ToolStripButton();
            this.MM_Save = new System.Windows.Forms.ToolStripButton();
            this.MM_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            this.MM_Cut = new System.Windows.Forms.ToolStripButton();
            this.MM_Copy = new System.Windows.Forms.ToolStripButton();
            this.MM_Paste = new System.Windows.Forms.ToolStripButton();
            this.MM_Sep2 = new System.Windows.Forms.ToolStripSeparator();
            this.MM_Delete = new System.Windows.Forms.ToolStripButton();
            this.MM_Convert = new System.Windows.Forms.ToolStripButton();
            this.MM_Steam = new System.Windows.Forms.ToolStripButton();
            this.MM_Sep3 = new System.Windows.Forms.ToolStripSeparator();
            this.MM_About = new System.Windows.Forms.ToolStripButton();
            this.MM_Menu.SuspendLayout();
            this.MM_StatusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MM_Table)).BeginInit();
            this.MM_Context.SuspendLayout();
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
            this.MM_FReload.Image = global::srcrepair.gui.Properties.Resources.Refresh;
            this.MM_FReload.Name = "MM_FReload";
            resources.ApplyResources(this.MM_FReload, "MM_FReload");
            this.MM_FReload.Click += new System.EventHandler(this.UpdateTable);
            // 
            // MM_FSave
            // 
            this.MM_FSave.Image = global::srcrepair.gui.Properties.Resources.Save;
            this.MM_FSave.Name = "MM_FSave";
            resources.ApplyResources(this.MM_FSave, "MM_FSave");
            this.MM_FSave.Click += new System.EventHandler(this.WriteTable);
            // 
            // MM_Exit
            // 
            this.MM_Exit.Image = global::srcrepair.gui.Properties.Resources.Exit;
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
            this.MM_HAbout.Image = global::srcrepair.gui.Properties.Resources.Info;
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
            this.MM_Table.ContextMenuStrip = this.MM_Context;
            resources.ApplyResources(this.MM_Table, "MM_Table");
            this.MM_Table.Name = "MM_Table";
            // 
            // SteamID
            // 
            resources.ApplyResources(this.SteamID, "SteamID");
            this.SteamID.Name = "SteamID";
            // 
            // MM_Context
            // 
            this.MM_Context.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MM_CCut,
            this.MM_CCopy,
            this.MM_CPaste,
            this.MM_Sep4,
            this.MM_CRemove,
            this.MM_CConvert,
            this.showSteamProfileToolStripMenuItem});
            this.MM_Context.Name = "MM_Context";
            resources.ApplyResources(this.MM_Context, "MM_Context");
            // 
            // MM_CCut
            // 
            this.MM_CCut.Image = global::srcrepair.gui.Properties.Resources.Cut;
            this.MM_CCut.Name = "MM_CCut";
            resources.ApplyResources(this.MM_CCut, "MM_CCut");
            this.MM_CCut.Click += new System.EventHandler(this.MM_Cut_Click);
            // 
            // MM_CCopy
            // 
            this.MM_CCopy.Image = global::srcrepair.gui.Properties.Resources.Copy;
            this.MM_CCopy.Name = "MM_CCopy";
            resources.ApplyResources(this.MM_CCopy, "MM_CCopy");
            this.MM_CCopy.Click += new System.EventHandler(this.MM_Copy_Click);
            // 
            // MM_CPaste
            // 
            this.MM_CPaste.Image = global::srcrepair.gui.Properties.Resources.Paste;
            this.MM_CPaste.Name = "MM_CPaste";
            resources.ApplyResources(this.MM_CPaste, "MM_CPaste");
            this.MM_CPaste.Click += new System.EventHandler(this.MM_Paste_Click);
            // 
            // MM_Sep4
            // 
            this.MM_Sep4.Name = "MM_Sep4";
            resources.ApplyResources(this.MM_Sep4, "MM_Sep4");
            // 
            // MM_CRemove
            // 
            this.MM_CRemove.Image = global::srcrepair.gui.Properties.Resources.Delete;
            this.MM_CRemove.Name = "MM_CRemove";
            resources.ApplyResources(this.MM_CRemove, "MM_CRemove");
            this.MM_CRemove.Click += new System.EventHandler(this.MM_Delete_Click);
            // 
            // MM_CConvert
            // 
            this.MM_CConvert.Image = global::srcrepair.gui.Properties.Resources.Convert;
            this.MM_CConvert.Name = "MM_CConvert";
            resources.ApplyResources(this.MM_CConvert, "MM_CConvert");
            this.MM_CConvert.Click += new System.EventHandler(this.MM_Convert_Click);
            // 
            // showSteamProfileToolStripMenuItem
            // 
            this.showSteamProfileToolStripMenuItem.Image = global::srcrepair.gui.Properties.Resources.steam;
            this.showSteamProfileToolStripMenuItem.Name = "showSteamProfileToolStripMenuItem";
            resources.ApplyResources(this.showSteamProfileToolStripMenuItem, "showSteamProfileToolStripMenuItem");
            this.showSteamProfileToolStripMenuItem.Click += new System.EventHandler(this.MM_Steam_Click);
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
            this.MM_Convert,
            this.MM_Steam,
            this.MM_Sep3,
            this.MM_About});
            resources.ApplyResources(this.MM_Toolbar, "MM_Toolbar");
            this.MM_Toolbar.Name = "MM_Toolbar";
            // 
            // MM_Refresh
            // 
            this.MM_Refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MM_Refresh.Image = global::srcrepair.gui.Properties.Resources.Refresh;
            resources.ApplyResources(this.MM_Refresh, "MM_Refresh");
            this.MM_Refresh.Name = "MM_Refresh";
            this.MM_Refresh.Click += new System.EventHandler(this.UpdateTable);
            // 
            // MM_Save
            // 
            this.MM_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MM_Save.Image = global::srcrepair.gui.Properties.Resources.Save;
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
            this.MM_Cut.Image = global::srcrepair.gui.Properties.Resources.Cut;
            resources.ApplyResources(this.MM_Cut, "MM_Cut");
            this.MM_Cut.Name = "MM_Cut";
            this.MM_Cut.Click += new System.EventHandler(this.MM_Cut_Click);
            // 
            // MM_Copy
            // 
            this.MM_Copy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MM_Copy.Image = global::srcrepair.gui.Properties.Resources.Copy;
            resources.ApplyResources(this.MM_Copy, "MM_Copy");
            this.MM_Copy.Name = "MM_Copy";
            this.MM_Copy.Click += new System.EventHandler(this.MM_Copy_Click);
            // 
            // MM_Paste
            // 
            this.MM_Paste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MM_Paste.Image = global::srcrepair.gui.Properties.Resources.Paste;
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
            this.MM_Delete.Image = global::srcrepair.gui.Properties.Resources.Delete;
            resources.ApplyResources(this.MM_Delete, "MM_Delete");
            this.MM_Delete.Name = "MM_Delete";
            this.MM_Delete.Click += new System.EventHandler(this.MM_Delete_Click);
            // 
            // MM_Convert
            // 
            this.MM_Convert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MM_Convert.Image = global::srcrepair.gui.Properties.Resources.Convert;
            resources.ApplyResources(this.MM_Convert, "MM_Convert");
            this.MM_Convert.Name = "MM_Convert";
            this.MM_Convert.Click += new System.EventHandler(this.MM_Convert_Click);
            // 
            // MM_Steam
            // 
            this.MM_Steam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MM_Steam.Image = global::srcrepair.gui.Properties.Resources.steam;
            resources.ApplyResources(this.MM_Steam, "MM_Steam");
            this.MM_Steam.Name = "MM_Steam";
            this.MM_Steam.Click += new System.EventHandler(this.MM_Steam_Click);
            // 
            // MM_Sep3
            // 
            this.MM_Sep3.Name = "MM_Sep3";
            resources.ApplyResources(this.MM_Sep3, "MM_Sep3");
            // 
            // MM_About
            // 
            this.MM_About.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MM_About.Image = global::srcrepair.gui.Properties.Resources.Info;
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
            this.MM_Context.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripButton MM_About;
        private System.Windows.Forms.ToolStripButton MM_Delete;
        private System.Windows.Forms.ToolStripSeparator MM_Sep3;
        private System.Windows.Forms.DataGridViewTextBoxColumn SteamID;
        private System.Windows.Forms.ContextMenuStrip MM_Context;
        private System.Windows.Forms.ToolStripMenuItem MM_CRemove;
        private System.Windows.Forms.ToolStripMenuItem MM_CCut;
        private System.Windows.Forms.ToolStripMenuItem MM_CCopy;
        private System.Windows.Forms.ToolStripMenuItem MM_CPaste;
        private System.Windows.Forms.ToolStripSeparator MM_Sep4;
        private System.Windows.Forms.ToolStripButton MM_Convert;
        private System.Windows.Forms.ToolStripMenuItem MM_CConvert;
        private System.Windows.Forms.ToolStripButton MM_Steam;
        private System.Windows.Forms.ToolStripMenuItem showSteamProfileToolStripMenuItem;
    }
}