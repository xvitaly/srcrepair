﻿namespace srcrepair.gui
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
            this.WelcomeLabel = new System.Windows.Forms.Label();
            this.GenerateNow = new System.Windows.Forms.Button();
            this.RB_Progress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // WelcomeLabel
            // 
            resources.ApplyResources(this.WelcomeLabel, "WelcomeLabel");
            this.WelcomeLabel.Name = "WelcomeLabel";
            // 
            // GenerateNow
            // 
            resources.ApplyResources(this.GenerateNow, "GenerateNow");
            this.GenerateNow.Name = "GenerateNow";
            this.GenerateNow.UseVisualStyleBackColor = true;
            this.GenerateNow.Click += new System.EventHandler(this.GenerateNow_Click);
            // 
            // RB_Progress
            // 
            resources.ApplyResources(this.RB_Progress, "RB_Progress");
            this.RB_Progress.Name = "RB_Progress";
            // 
            // FrmRepBuilder
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RB_Progress);
            this.Controls.Add(this.GenerateNow);
            this.Controls.Add(this.WelcomeLabel);
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

        private System.Windows.Forms.Label WelcomeLabel;
        private System.Windows.Forms.Button GenerateNow;
        private System.Windows.Forms.ProgressBar RB_Progress;
    }
}