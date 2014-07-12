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
            this.DnlInstall = new System.Windows.Forms.Button();
            this.DnlProgBar = new System.Windows.Forms.ProgressBar();
            this.UpdAppImg = new System.Windows.Forms.PictureBox();
            this.UpdDBImg = new System.Windows.Forms.PictureBox();
            this.UpdAppStatus = new System.Windows.Forms.Label();
            this.UpdDBStatus = new System.Windows.Forms.Label();
            this.WrkChkApp = new System.ComponentModel.BackgroundWorker();
            this.WrkChkDb = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.UpdAppImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdDBImg)).BeginInit();
            this.SuspendLayout();
            // 
            // DnlInstall
            // 
            resources.ApplyResources(this.DnlInstall, "DnlInstall");
            this.DnlInstall.Name = "DnlInstall";
            this.DnlInstall.UseVisualStyleBackColor = true;
            this.DnlInstall.Click += new System.EventHandler(this.DnlInstall_Click);
            // 
            // DnlProgBar
            // 
            resources.ApplyResources(this.DnlProgBar, "DnlProgBar");
            this.DnlProgBar.Name = "DnlProgBar";
            // 
            // UpdAppImg
            // 
            this.UpdAppImg.Image = global::srcrepair.Properties.Resources.upd_av;
            resources.ApplyResources(this.UpdAppImg, "UpdAppImg");
            this.UpdAppImg.Name = "UpdAppImg";
            this.UpdAppImg.TabStop = false;
            // 
            // UpdDBImg
            // 
            this.UpdDBImg.Image = global::srcrepair.Properties.Resources.upd_nx;
            resources.ApplyResources(this.UpdDBImg, "UpdDBImg");
            this.UpdDBImg.Name = "UpdDBImg";
            this.UpdDBImg.TabStop = false;
            // 
            // UpdAppStatus
            // 
            resources.ApplyResources(this.UpdAppStatus, "UpdAppStatus");
            this.UpdAppStatus.Name = "UpdAppStatus";
            // 
            // UpdDBStatus
            // 
            resources.ApplyResources(this.UpdDBStatus, "UpdDBStatus");
            this.UpdDBStatus.Name = "UpdDBStatus";
            // 
            // WrkChkApp
            // 
            this.WrkChkApp.DoWork += new System.ComponentModel.DoWorkEventHandler(this.WrkChkApp_DoWork);
            this.WrkChkApp.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.WrkChkApp_RunWorkerCompleted);
            // 
            // WrkChkDb
            // 
            this.WrkChkDb.DoWork += new System.ComponentModel.DoWorkEventHandler(this.WrkChkDb_DoWork);
            this.WrkChkDb.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.WrkChkDb_RunWorkerCompleted);
            // 
            // frmUpdate
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UpdDBStatus);
            this.Controls.Add(this.UpdAppStatus);
            this.Controls.Add(this.UpdDBImg);
            this.Controls.Add(this.UpdAppImg);
            this.Controls.Add(this.DnlProgBar);
            this.Controls.Add(this.DnlInstall);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpdate";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UpdAppImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdDBImg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button DnlInstall;
        private System.Windows.Forms.ProgressBar DnlProgBar;
        private System.Windows.Forms.PictureBox UpdAppImg;
        private System.Windows.Forms.PictureBox UpdDBImg;
        private System.Windows.Forms.Label UpdAppStatus;
        private System.Windows.Forms.Label UpdDBStatus;
        private System.ComponentModel.BackgroundWorker WrkChkApp;
        private System.ComponentModel.BackgroundWorker WrkChkDb;
    }
}