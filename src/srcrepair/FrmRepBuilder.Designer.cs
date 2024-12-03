namespace srcrepair.gui
{
    partial class FrmRepBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRepBuilder));
            this.RP_WlcMsg = new System.Windows.Forms.Label();
            this.RP_Generate = new System.Windows.Forms.Button();
            this.RP_Progress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // RP_WlcMsg
            // 
            resources.ApplyResources(this.RP_WlcMsg, "RP_WlcMsg");
            this.RP_WlcMsg.Name = "RP_WlcMsg";
            // 
            // RP_Generate
            // 
            resources.ApplyResources(this.RP_Generate, "RP_Generate");
            this.RP_Generate.Name = "RP_Generate";
            this.RP_Generate.UseVisualStyleBackColor = true;
            this.RP_Generate.Click += new System.EventHandler(this.RP_Generate_Click);
            // 
            // RP_Progress
            // 
            resources.ApplyResources(this.RP_Progress, "RP_Progress");
            this.RP_Progress.Name = "RP_Progress";
            // 
            // FrmRepBuilder
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RP_Progress);
            this.Controls.Add(this.RP_Generate);
            this.Controls.Add(this.RP_WlcMsg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRepBuilder";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRepBuilder_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label RP_WlcMsg;
        private System.Windows.Forms.Button RP_Generate;
        private System.Windows.Forms.ProgressBar RP_Progress;
    }
}