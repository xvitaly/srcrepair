﻿namespace srcrepair.gui
{
    partial class FrmRmWrk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRmWrk));
            this.RW_PrgBr = new System.Windows.Forms.ProgressBar();
            this.RW_WlcMsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // RW_PrgBr
            // 
            resources.ApplyResources(this.RW_PrgBr, "RW_PrgBr");
            this.RW_PrgBr.Name = "RW_PrgBr";
            // 
            // RW_WlcMsg
            // 
            resources.ApplyResources(this.RW_WlcMsg, "RW_WlcMsg");
            this.RW_WlcMsg.Name = "RW_WlcMsg";
            // 
            // FrmRmWrk
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.RW_PrgBr);
            this.Controls.Add(this.RW_WlcMsg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRmWrk";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRmWrk_FormClosing);
            this.Load += new System.EventHandler(this.FrmRmWrk_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar RW_PrgBr;
        private System.Windows.Forms.Label RW_WlcMsg;
    }
}