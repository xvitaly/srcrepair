﻿namespace srcrepair.gui
{
    partial class FrmCleaner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCleaner));
            this.CM_WlcMsg = new System.Windows.Forms.Label();
            this.CM_FTable = new System.Windows.Forms.ListView();
            this.FName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CM_Info = new System.Windows.Forms.Label();
            this.CM_Clean = new System.Windows.Forms.Button();
            this.CM_Cancel = new System.Windows.Forms.Button();
            this.CM_Progress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // CM_WlcMsg
            // 
            resources.ApplyResources(this.CM_WlcMsg, "CM_WlcMsg");
            this.CM_WlcMsg.Name = "CM_WlcMsg";
            // 
            // CM_FTable
            // 
            this.CM_FTable.CheckBoxes = true;
            this.CM_FTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FName,
            this.FSize,
            this.FDate});
            this.CM_FTable.FullRowSelect = true;
            this.CM_FTable.GridLines = true;
            this.CM_FTable.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.CM_FTable.HideSelection = false;
            resources.ApplyResources(this.CM_FTable, "CM_FTable");
            this.CM_FTable.MultiSelect = false;
            this.CM_FTable.Name = "CM_FTable";
            this.CM_FTable.ShowGroups = false;
            this.CM_FTable.ShowItemToolTips = true;
            this.CM_FTable.UseCompatibleStateImageBehavior = false;
            this.CM_FTable.View = System.Windows.Forms.View.Details;
            this.CM_FTable.DoubleClick += new System.EventHandler(this.CM_FTable_DoubleClick);
            this.CM_FTable.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CM_FTable_KeyUp);
            // 
            // FName
            // 
            resources.ApplyResources(this.FName, "FName");
            // 
            // FSize
            // 
            resources.ApplyResources(this.FSize, "FSize");
            // 
            // FDate
            // 
            resources.ApplyResources(this.FDate, "FDate");
            // 
            // CM_Info
            // 
            resources.ApplyResources(this.CM_Info, "CM_Info");
            this.CM_Info.Name = "CM_Info";
            // 
            // CM_Clean
            // 
            resources.ApplyResources(this.CM_Clean, "CM_Clean");
            this.CM_Clean.Name = "CM_Clean";
            this.CM_Clean.UseVisualStyleBackColor = true;
            this.CM_Clean.Click += new System.EventHandler(this.CM_Clean_Click);
            // 
            // CM_Cancel
            // 
            this.CM_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.CM_Cancel, "CM_Cancel");
            this.CM_Cancel.Name = "CM_Cancel";
            this.CM_Cancel.UseVisualStyleBackColor = true;
            this.CM_Cancel.Click += new System.EventHandler(this.CM_Cancel_Click);
            // 
            // CM_Progress
            // 
            resources.ApplyResources(this.CM_Progress, "CM_Progress");
            this.CM_Progress.Name = "CM_Progress";
            // 
            // FrmCleaner
            // 
            this.AcceptButton = this.CM_Clean;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CM_Cancel;
            this.ControlBox = false;
            this.Controls.Add(this.CM_Progress);
            this.Controls.Add(this.CM_Cancel);
            this.Controls.Add(this.CM_Clean);
            this.Controls.Add(this.CM_Info);
            this.Controls.Add(this.CM_FTable);
            this.Controls.Add(this.CM_WlcMsg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCleaner";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCleaner_FormClosing);
            this.Load += new System.EventHandler(this.FrmCleaner_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label CM_WlcMsg;
        private System.Windows.Forms.ListView CM_FTable;
        private System.Windows.Forms.Label CM_Info;
        private System.Windows.Forms.Button CM_Clean;
        private System.Windows.Forms.Button CM_Cancel;
        private System.Windows.Forms.ColumnHeader FName;
        private System.Windows.Forms.ColumnHeader FSize;
        private System.Windows.Forms.ColumnHeader FDate;
        private System.Windows.Forms.ProgressBar CM_Progress;
    }
}