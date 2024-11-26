namespace srcrepair.gui
{
    partial class FrmInstaller
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInstaller));
            this.QI_WlcMsg = new System.Windows.Forms.Label();
            this.QI_InstallPathLabel = new System.Windows.Forms.Label();
            this.QI_InstallPath = new System.Windows.Forms.TextBox();
            this.QI_Browse = new System.Windows.Forms.Button();
            this.QI_Install = new System.Windows.Forms.Button();
            this.QI_OpenFile = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // QI_WlcMsg
            // 
            resources.ApplyResources(this.QI_WlcMsg, "QI_WlcMsg");
            this.QI_WlcMsg.Name = "QI_WlcMsg";
            // 
            // QI_InstallPathLabel
            // 
            resources.ApplyResources(this.QI_InstallPathLabel, "QI_InstallPathLabel");
            this.QI_InstallPathLabel.Name = "QI_InstallPathLabel";
            // 
            // QI_InstallPath
            // 
            resources.ApplyResources(this.QI_InstallPath, "QI_InstallPath");
            this.QI_InstallPath.Name = "QI_InstallPath";
            this.QI_InstallPath.ReadOnly = true;
            this.QI_InstallPath.TabStop = false;
            // 
            // QI_Browse
            // 
            resources.ApplyResources(this.QI_Browse, "QI_Browse");
            this.QI_Browse.Name = "QI_Browse";
            this.QI_Browse.UseVisualStyleBackColor = true;
            this.QI_Browse.Click += new System.EventHandler(this.QI_Browse_Click);
            // 
            // QI_Install
            // 
            resources.ApplyResources(this.QI_Install, "QI_Install");
            this.QI_Install.Name = "QI_Install";
            this.QI_Install.UseVisualStyleBackColor = true;
            this.QI_Install.Click += new System.EventHandler(this.QI_Install_Click);
            // 
            // QI_OpenFile
            // 
            resources.ApplyResources(this.QI_OpenFile, "QI_OpenFile");
            // 
            // FrmInstaller
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.QI_Install);
            this.Controls.Add(this.QI_Browse);
            this.Controls.Add(this.QI_InstallPath);
            this.Controls.Add(this.QI_InstallPathLabel);
            this.Controls.Add(this.QI_WlcMsg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInstaller";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label QI_WlcMsg;
        private System.Windows.Forms.Label QI_InstallPathLabel;
        private System.Windows.Forms.TextBox QI_InstallPath;
        private System.Windows.Forms.Button QI_Browse;
        private System.Windows.Forms.Button QI_Install;
        private System.Windows.Forms.OpenFileDialog QI_OpenFile;
    }
}