namespace srcrepair.gui
{
    partial class FrmKBHelper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmKBHelper));
            this.GB_Mn = new System.Windows.Forms.GroupBox();
            this.Dis_Restore = new System.Windows.Forms.Button();
            this.Dis_BWinMnu = new System.Windows.Forms.Button();
            this.Dis_RWinMnu = new System.Windows.Forms.Button();
            this.Dis_BWIN = new System.Windows.Forms.Button();
            this.Dis_LWIN = new System.Windows.Forms.Button();
            this.GB_Mn.SuspendLayout();
            this.SuspendLayout();
            // 
            // GB_Mn
            // 
            this.GB_Mn.Controls.Add(this.Dis_Restore);
            this.GB_Mn.Controls.Add(this.Dis_BWinMnu);
            this.GB_Mn.Controls.Add(this.Dis_RWinMnu);
            this.GB_Mn.Controls.Add(this.Dis_BWIN);
            this.GB_Mn.Controls.Add(this.Dis_LWIN);
            resources.ApplyResources(this.GB_Mn, "GB_Mn");
            this.GB_Mn.Name = "GB_Mn";
            this.GB_Mn.TabStop = false;
            // 
            // Dis_Restore
            // 
            resources.ApplyResources(this.Dis_Restore, "Dis_Restore");
            this.Dis_Restore.Name = "Dis_Restore";
            this.Dis_Restore.UseVisualStyleBackColor = true;
            this.Dis_Restore.Click += new System.EventHandler(this.Dis_Restore_Click);
            // 
            // Dis_BWinMnu
            // 
            resources.ApplyResources(this.Dis_BWinMnu, "Dis_BWinMnu");
            this.Dis_BWinMnu.Name = "Dis_BWinMnu";
            this.Dis_BWinMnu.UseVisualStyleBackColor = true;
            this.Dis_BWinMnu.Click += new System.EventHandler(this.Dis_BWinMnu_Click);
            // 
            // Dis_RWinMnu
            // 
            resources.ApplyResources(this.Dis_RWinMnu, "Dis_RWinMnu");
            this.Dis_RWinMnu.Name = "Dis_RWinMnu";
            this.Dis_RWinMnu.UseVisualStyleBackColor = true;
            this.Dis_RWinMnu.Click += new System.EventHandler(this.Dis_RWinMnu_Click);
            // 
            // Dis_BWIN
            // 
            resources.ApplyResources(this.Dis_BWIN, "Dis_BWIN");
            this.Dis_BWIN.Name = "Dis_BWIN";
            this.Dis_BWIN.UseVisualStyleBackColor = true;
            this.Dis_BWIN.Click += new System.EventHandler(this.Dis_BWIN_Click);
            // 
            // Dis_LWIN
            // 
            resources.ApplyResources(this.Dis_LWIN, "Dis_LWIN");
            this.Dis_LWIN.Name = "Dis_LWIN";
            this.Dis_LWIN.UseVisualStyleBackColor = true;
            this.Dis_LWIN.Click += new System.EventHandler(this.Dis_LWIN_Click);
            // 
            // FrmKBHelper
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GB_Mn);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmKBHelper";
            this.ShowInTaskbar = false;
            this.GB_Mn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GB_Mn;
        private System.Windows.Forms.Button Dis_Restore;
        private System.Windows.Forms.Button Dis_BWinMnu;
        private System.Windows.Forms.Button Dis_RWinMnu;
        private System.Windows.Forms.Button Dis_BWIN;
        private System.Windows.Forms.Button Dis_LWIN;
    }
}