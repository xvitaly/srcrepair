namespace srcrepair
{
    partial class frmUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdate));
            this.LCurrVersion = new System.Windows.Forms.Label();
            this.LWelcome = new System.Windows.Forms.Label();
            this.DnlInstall = new System.Windows.Forms.Button();
            this.LAvailVersion = new System.Windows.Forms.Label();
            this.DnlProgBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // LCurrVersion
            // 
            resources.ApplyResources(this.LCurrVersion, "LCurrVersion");
            this.LCurrVersion.Name = "LCurrVersion";
            // 
            // LWelcome
            // 
            resources.ApplyResources(this.LWelcome, "LWelcome");
            this.LWelcome.Name = "LWelcome";
            // 
            // DnlInstall
            // 
            resources.ApplyResources(this.DnlInstall, "DnlInstall");
            this.DnlInstall.Name = "DnlInstall";
            this.DnlInstall.UseVisualStyleBackColor = true;
            this.DnlInstall.Click += new System.EventHandler(this.DnlInstall_Click);
            // 
            // LAvailVersion
            // 
            this.LAvailVersion.ForeColor = System.Drawing.Color.Green;
            resources.ApplyResources(this.LAvailVersion, "LAvailVersion");
            this.LAvailVersion.Name = "LAvailVersion";
            // 
            // DnlProgBar
            // 
            resources.ApplyResources(this.DnlProgBar, "DnlProgBar");
            this.DnlProgBar.Name = "DnlProgBar";
            // 
            // frmUpdate
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DnlProgBar);
            this.Controls.Add(this.LAvailVersion);
            this.Controls.Add(this.DnlInstall);
            this.Controls.Add(this.LWelcome);
            this.Controls.Add(this.LCurrVersion);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpdate";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmUpdate_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LCurrVersion;
        private System.Windows.Forms.Label LWelcome;
        private System.Windows.Forms.Button DnlInstall;
        private System.Windows.Forms.Label LAvailVersion;
        private System.Windows.Forms.ProgressBar DnlProgBar;
    }
}