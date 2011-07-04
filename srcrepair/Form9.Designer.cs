namespace srcrepair
{
    partial class frmOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
            this.GB_MainOpts = new System.Windows.Forms.GroupBox();
            this.MO_RemEmptyDirs = new System.Windows.Forms.CheckBox();
            this.MO_AllowUnsafeNCFOps = new System.Windows.Forms.CheckBox();
            this.MO_AutoUpdate = new System.Windows.Forms.CheckBox();
            this.MO_SortGameList = new System.Windows.Forms.CheckBox();
            this.MO_HideNotInst = new System.Windows.Forms.CheckBox();
            this.MO_ConfirmExit = new System.Windows.Forms.CheckBox();
            this.MO_Okay = new System.Windows.Forms.Button();
            this.MO_Cancel = new System.Windows.Forms.Button();
            this.GB_MainOpts.SuspendLayout();
            this.SuspendLayout();
            // 
            // GB_MainOpts
            // 
            this.GB_MainOpts.Controls.Add(this.MO_RemEmptyDirs);
            this.GB_MainOpts.Controls.Add(this.MO_AllowUnsafeNCFOps);
            this.GB_MainOpts.Controls.Add(this.MO_AutoUpdate);
            this.GB_MainOpts.Controls.Add(this.MO_SortGameList);
            this.GB_MainOpts.Controls.Add(this.MO_HideNotInst);
            this.GB_MainOpts.Controls.Add(this.MO_ConfirmExit);
            resources.ApplyResources(this.GB_MainOpts, "GB_MainOpts");
            this.GB_MainOpts.Name = "GB_MainOpts";
            this.GB_MainOpts.TabStop = false;
            // 
            // MO_RemEmptyDirs
            // 
            resources.ApplyResources(this.MO_RemEmptyDirs, "MO_RemEmptyDirs");
            this.MO_RemEmptyDirs.Name = "MO_RemEmptyDirs";
            this.MO_RemEmptyDirs.UseVisualStyleBackColor = true;
            // 
            // MO_AllowUnsafeNCFOps
            // 
            resources.ApplyResources(this.MO_AllowUnsafeNCFOps, "MO_AllowUnsafeNCFOps");
            this.MO_AllowUnsafeNCFOps.Name = "MO_AllowUnsafeNCFOps";
            this.MO_AllowUnsafeNCFOps.UseVisualStyleBackColor = true;
            // 
            // MO_AutoUpdate
            // 
            resources.ApplyResources(this.MO_AutoUpdate, "MO_AutoUpdate");
            this.MO_AutoUpdate.Name = "MO_AutoUpdate";
            this.MO_AutoUpdate.UseVisualStyleBackColor = true;
            // 
            // MO_SortGameList
            // 
            resources.ApplyResources(this.MO_SortGameList, "MO_SortGameList");
            this.MO_SortGameList.Name = "MO_SortGameList";
            this.MO_SortGameList.UseVisualStyleBackColor = true;
            // 
            // MO_HideNotInst
            // 
            resources.ApplyResources(this.MO_HideNotInst, "MO_HideNotInst");
            this.MO_HideNotInst.Name = "MO_HideNotInst";
            this.MO_HideNotInst.UseVisualStyleBackColor = true;
            // 
            // MO_ConfirmExit
            // 
            resources.ApplyResources(this.MO_ConfirmExit, "MO_ConfirmExit");
            this.MO_ConfirmExit.Name = "MO_ConfirmExit";
            this.MO_ConfirmExit.UseVisualStyleBackColor = true;
            // 
            // MO_Okay
            // 
            resources.ApplyResources(this.MO_Okay, "MO_Okay");
            this.MO_Okay.Name = "MO_Okay";
            this.MO_Okay.UseVisualStyleBackColor = true;
            this.MO_Okay.Click += new System.EventHandler(this.MO_Okay_Click);
            // 
            // MO_Cancel
            // 
            this.MO_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.MO_Cancel, "MO_Cancel");
            this.MO_Cancel.Name = "MO_Cancel";
            this.MO_Cancel.UseVisualStyleBackColor = true;
            this.MO_Cancel.Click += new System.EventHandler(this.MO_Cancel_Click);
            // 
            // frmOptions
            // 
            this.AcceptButton = this.MO_Okay;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.MO_Cancel;
            this.ControlBox = false;
            this.Controls.Add(this.MO_Cancel);
            this.Controls.Add(this.MO_Okay);
            this.Controls.Add(this.GB_MainOpts);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.GB_MainOpts.ResumeLayout(false);
            this.GB_MainOpts.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GB_MainOpts;
        private System.Windows.Forms.CheckBox MO_ConfirmExit;
        private System.Windows.Forms.CheckBox MO_HideNotInst;
        private System.Windows.Forms.Button MO_Okay;
        private System.Windows.Forms.Button MO_Cancel;
        private System.Windows.Forms.CheckBox MO_SortGameList;
        private System.Windows.Forms.CheckBox MO_AutoUpdate;
        private System.Windows.Forms.CheckBox MO_AllowUnsafeNCFOps;
        private System.Windows.Forms.CheckBox MO_RemEmptyDirs;
    }
}