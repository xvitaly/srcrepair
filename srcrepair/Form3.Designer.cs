namespace srcrepair
{
    partial class frmHEd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHEd));
            this.HEd_Table = new System.Windows.Forms.DataGridView();
            this.HDV_IPAddr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HDV_Domain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HEd_MTool = new System.Windows.Forms.ToolStrip();
            this.HEd_T_Refresh = new System.Windows.Forms.ToolStripButton();
            this.HEd_T_Save = new System.Windows.Forms.ToolStripButton();
            this.HEd_MMenu = new System.Windows.Forms.MenuStrip();
            this.HEd_M_File = new System.Windows.Forms.ToolStripMenuItem();
            this.HEd_M_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.HEd_M_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.HEd_M_Quit = new System.Windows.Forms.ToolStripMenuItem();
            this.HEd_M_Adv = new System.Windows.Forms.ToolStripMenuItem();
            this.HEd_M_HBack = new System.Windows.Forms.ToolStripMenuItem();
            this.HEd_M_RestDef = new System.Windows.Forms.ToolStripMenuItem();
            this.HEd_M_Hlp = new System.Windows.Forms.ToolStripMenuItem();
            this.HEd_M_OnlHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.HEd_M_About = new System.Windows.Forms.ToolStripMenuItem();
            this.HEd_MStatus = new System.Windows.Forms.StatusStrip();
            this.HEd_St_Wrn = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.HEd_Table)).BeginInit();
            this.HEd_MTool.SuspendLayout();
            this.HEd_MMenu.SuspendLayout();
            this.HEd_MStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // HEd_Table
            // 
            this.HEd_Table.BackgroundColor = System.Drawing.SystemColors.Window;
            this.HEd_Table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.HEd_Table.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.HDV_IPAddr,
            this.HDV_Domain});
            this.HEd_Table.Location = new System.Drawing.Point(0, 52);
            this.HEd_Table.Name = "HEd_Table";
            this.HEd_Table.Size = new System.Drawing.Size(388, 246);
            this.HEd_Table.TabIndex = 0;
            // 
            // HDV_IPAddr
            // 
            this.HDV_IPAddr.HeaderText = "IP Address";
            this.HDV_IPAddr.Name = "HDV_IPAddr";
            this.HDV_IPAddr.Width = 130;
            // 
            // HDV_Domain
            // 
            this.HDV_Domain.HeaderText = "Domain Name";
            this.HDV_Domain.Name = "HDV_Domain";
            this.HDV_Domain.Width = 190;
            // 
            // HEd_MTool
            // 
            this.HEd_MTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HEd_T_Refresh,
            this.HEd_T_Save});
            this.HEd_MTool.Location = new System.Drawing.Point(0, 24);
            this.HEd_MTool.Name = "HEd_MTool";
            this.HEd_MTool.Size = new System.Drawing.Size(388, 25);
            this.HEd_MTool.TabIndex = 1;
            this.HEd_MTool.Text = "toolStrip1";
            // 
            // HEd_T_Refresh
            // 
            this.HEd_T_Refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.HEd_T_Refresh.Image = global::srcrepair.Properties.Resources.Refresh;
            this.HEd_T_Refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.HEd_T_Refresh.Name = "HEd_T_Refresh";
            this.HEd_T_Refresh.Size = new System.Drawing.Size(23, 22);
            this.HEd_T_Refresh.Text = "toolStripButton1";
            // 
            // HEd_T_Save
            // 
            this.HEd_T_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.HEd_T_Save.Image = global::srcrepair.Properties.Resources.Save;
            this.HEd_T_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.HEd_T_Save.Name = "HEd_T_Save";
            this.HEd_T_Save.Size = new System.Drawing.Size(23, 22);
            this.HEd_T_Save.Text = "toolStripButton1";
            // 
            // HEd_MMenu
            // 
            this.HEd_MMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HEd_M_File,
            this.HEd_M_Adv,
            this.HEd_M_Hlp});
            this.HEd_MMenu.Location = new System.Drawing.Point(0, 0);
            this.HEd_MMenu.Name = "HEd_MMenu";
            this.HEd_MMenu.Size = new System.Drawing.Size(388, 24);
            this.HEd_MMenu.TabIndex = 2;
            // 
            // HEd_M_File
            // 
            this.HEd_M_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HEd_M_Refresh,
            this.HEd_M_Save,
            this.HEd_M_Quit});
            this.HEd_M_File.Name = "HEd_M_File";
            this.HEd_M_File.Size = new System.Drawing.Size(35, 20);
            this.HEd_M_File.Text = "&File";
            // 
            // HEd_M_Refresh
            // 
            this.HEd_M_Refresh.Image = global::srcrepair.Properties.Resources.Refresh;
            this.HEd_M_Refresh.Name = "HEd_M_Refresh";
            this.HEd_M_Refresh.Size = new System.Drawing.Size(123, 22);
            this.HEd_M_Refresh.Text = "&Refresh";
            // 
            // HEd_M_Save
            // 
            this.HEd_M_Save.Image = global::srcrepair.Properties.Resources.Save;
            this.HEd_M_Save.Name = "HEd_M_Save";
            this.HEd_M_Save.Size = new System.Drawing.Size(123, 22);
            this.HEd_M_Save.Text = "&Save";
            // 
            // HEd_M_Quit
            // 
            this.HEd_M_Quit.Image = global::srcrepair.Properties.Resources.Exit;
            this.HEd_M_Quit.Name = "HEd_M_Quit";
            this.HEd_M_Quit.Size = new System.Drawing.Size(123, 22);
            this.HEd_M_Quit.Text = "&Quit";
            // 
            // HEd_M_Adv
            // 
            this.HEd_M_Adv.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HEd_M_HBack,
            this.HEd_M_RestDef});
            this.HEd_M_Adv.Name = "HEd_M_Adv";
            this.HEd_M_Adv.Size = new System.Drawing.Size(67, 20);
            this.HEd_M_Adv.Text = "&Advanced";
            // 
            // HEd_M_HBack
            // 
            this.HEd_M_HBack.Image = global::srcrepair.Properties.Resources.Add;
            this.HEd_M_HBack.Name = "HEd_M_HBack";
            this.HEd_M_HBack.Size = new System.Drawing.Size(177, 22);
            this.HEd_M_HBack.Text = "&BackUp hosts file";
            // 
            // HEd_M_RestDef
            // 
            this.HEd_M_RestDef.Image = global::srcrepair.Properties.Resources.Restore;
            this.HEd_M_RestDef.Name = "HEd_M_RestDef";
            this.HEd_M_RestDef.Size = new System.Drawing.Size(177, 22);
            this.HEd_M_RestDef.Text = "R&estore default file";
            // 
            // HEd_M_Hlp
            // 
            this.HEd_M_Hlp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HEd_M_OnlHelp,
            this.HEd_M_About});
            this.HEd_M_Hlp.Name = "HEd_M_Hlp";
            this.HEd_M_Hlp.Size = new System.Drawing.Size(40, 20);
            this.HEd_M_Hlp.Text = "&Help";
            // 
            // HEd_M_OnlHelp
            // 
            this.HEd_M_OnlHelp.Image = global::srcrepair.Properties.Resources.Help;
            this.HEd_M_OnlHelp.Name = "HEd_M_OnlHelp";
            this.HEd_M_OnlHelp.Size = new System.Drawing.Size(157, 22);
            this.HEd_M_OnlHelp.Text = "&Online help";
            // 
            // HEd_M_About
            // 
            this.HEd_M_About.Image = global::srcrepair.Properties.Resources.Info;
            this.HEd_M_About.Name = "HEd_M_About";
            this.HEd_M_About.Size = new System.Drawing.Size(157, 22);
            this.HEd_M_About.Text = "Abo&ut plugin...";
            // 
            // HEd_MStatus
            // 
            this.HEd_MStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HEd_St_Wrn});
            this.HEd_MStatus.Location = new System.Drawing.Point(0, 298);
            this.HEd_MStatus.Name = "HEd_MStatus";
            this.HEd_MStatus.Size = new System.Drawing.Size(388, 22);
            this.HEd_MStatus.TabIndex = 3;
            // 
            // HEd_St_Wrn
            // 
            this.HEd_St_Wrn.Name = "HEd_St_Wrn";
            this.HEd_St_Wrn.Size = new System.Drawing.Size(347, 17);
            this.HEd_St_Wrn.Text = "Use this tool on your own risk. Wrong parameters can damage system!";
            // 
            // frmHEd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 320);
            this.Controls.Add(this.HEd_MStatus);
            this.Controls.Add(this.HEd_MTool);
            this.Controls.Add(this.HEd_MMenu);
            this.Controls.Add(this.HEd_Table);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.HEd_MMenu;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHEd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced Hosts Editor";
            ((System.ComponentModel.ISupportInitialize)(this.HEd_Table)).EndInit();
            this.HEd_MTool.ResumeLayout(false);
            this.HEd_MTool.PerformLayout();
            this.HEd_MMenu.ResumeLayout(false);
            this.HEd_MMenu.PerformLayout();
            this.HEd_MStatus.ResumeLayout(false);
            this.HEd_MStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView HEd_Table;
        private System.Windows.Forms.DataGridViewTextBoxColumn HDV_IPAddr;
        private System.Windows.Forms.DataGridViewTextBoxColumn HDV_Domain;
        private System.Windows.Forms.ToolStrip HEd_MTool;
        private System.Windows.Forms.ToolStripButton HEd_T_Refresh;
        private System.Windows.Forms.ToolStripButton HEd_T_Save;
        private System.Windows.Forms.MenuStrip HEd_MMenu;
        private System.Windows.Forms.ToolStripMenuItem HEd_M_File;
        private System.Windows.Forms.ToolStripMenuItem HEd_M_Refresh;
        private System.Windows.Forms.ToolStripMenuItem HEd_M_Save;
        private System.Windows.Forms.ToolStripMenuItem HEd_M_Adv;
        private System.Windows.Forms.ToolStripMenuItem HEd_M_HBack;
        private System.Windows.Forms.ToolStripMenuItem HEd_M_RestDef;
        private System.Windows.Forms.ToolStripMenuItem HEd_M_Hlp;
        private System.Windows.Forms.ToolStripMenuItem HEd_M_OnlHelp;
        private System.Windows.Forms.ToolStripMenuItem HEd_M_About;
        private System.Windows.Forms.StatusStrip HEd_MStatus;
        private System.Windows.Forms.ToolStripStatusLabel HEd_St_Wrn;
        private System.Windows.Forms.ToolStripMenuItem HEd_M_Quit;
    }
}