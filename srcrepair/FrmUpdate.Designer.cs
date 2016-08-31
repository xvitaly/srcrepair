namespace srcrepair
{
    partial class FrmUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUpdate));
            this.UpdAppImg = new System.Windows.Forms.PictureBox();
            this.UpdDBImg = new System.Windows.Forms.PictureBox();
            this.UpdAppStatus = new System.Windows.Forms.Label();
            this.UpdDBStatus = new System.Windows.Forms.Label();
            this.WrkChkApp = new System.ComponentModel.BackgroundWorker();
            this.UpdHUDDbImg = new System.Windows.Forms.PictureBox();
            this.UpdHUDStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.UpdAppImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdDBImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdHUDDbImg)).BeginInit();
            this.SuspendLayout();
            // 
            // UpdAppImg
            // 
            this.UpdAppImg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UpdAppImg.Image = global::srcrepair.Properties.Resources.upd_av;
            resources.ApplyResources(this.UpdAppImg, "UpdAppImg");
            this.UpdAppImg.Name = "UpdAppImg";
            this.UpdAppImg.TabStop = false;
            this.UpdAppImg.Click += new System.EventHandler(this.UpdAppStatus_Click);
            // 
            // UpdDBImg
            // 
            this.UpdDBImg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UpdDBImg.Image = global::srcrepair.Properties.Resources.upd_nx;
            resources.ApplyResources(this.UpdDBImg, "UpdDBImg");
            this.UpdDBImg.Name = "UpdDBImg";
            this.UpdDBImg.TabStop = false;
            this.UpdDBImg.Click += new System.EventHandler(this.UpdDBStatus_Click);
            // 
            // UpdAppStatus
            // 
            this.UpdAppStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.UpdAppStatus, "UpdAppStatus");
            this.UpdAppStatus.Name = "UpdAppStatus";
            this.UpdAppStatus.Click += new System.EventHandler(this.UpdAppStatus_Click);
            // 
            // UpdDBStatus
            // 
            this.UpdDBStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.UpdDBStatus, "UpdDBStatus");
            this.UpdDBStatus.Name = "UpdDBStatus";
            this.UpdDBStatus.Click += new System.EventHandler(this.UpdDBStatus_Click);
            // 
            // WrkChkApp
            // 
            this.WrkChkApp.DoWork += new System.ComponentModel.DoWorkEventHandler(this.WrkChkApp_DoWork);
            this.WrkChkApp.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.WrkChkApp_RunWorkerCompleted);
            // 
            // UpdHUDDbImg
            // 
            this.UpdHUDDbImg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UpdHUDDbImg.Image = global::srcrepair.Properties.Resources.upd_chk;
            resources.ApplyResources(this.UpdHUDDbImg, "UpdHUDDbImg");
            this.UpdHUDDbImg.Name = "UpdHUDDbImg";
            this.UpdHUDDbImg.TabStop = false;
            this.UpdHUDDbImg.Click += new System.EventHandler(this.UpdHUDStatus_Click);
            // 
            // UpdHUDStatus
            // 
            this.UpdHUDStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.UpdHUDStatus, "UpdHUDStatus");
            this.UpdHUDStatus.Name = "UpdHUDStatus";
            this.UpdHUDStatus.Click += new System.EventHandler(this.UpdHUDStatus_Click);
            // 
            // FrmUpdate
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UpdHUDStatus);
            this.Controls.Add(this.UpdDBStatus);
            this.Controls.Add(this.UpdHUDDbImg);
            this.Controls.Add(this.UpdAppStatus);
            this.Controls.Add(this.UpdDBImg);
            this.Controls.Add(this.UpdAppImg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUpdate";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpdate_FormClosing);
            this.Load += new System.EventHandler(this.frmUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UpdAppImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdDBImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdHUDDbImg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox UpdAppImg;
        private System.Windows.Forms.PictureBox UpdDBImg;
        private System.Windows.Forms.Label UpdAppStatus;
        private System.Windows.Forms.Label UpdDBStatus;
        private System.ComponentModel.BackgroundWorker WrkChkApp;
        private System.Windows.Forms.PictureBox UpdHUDDbImg;
        private System.Windows.Forms.Label UpdHUDStatus;
    }
}