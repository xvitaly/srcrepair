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
            this.MO_Okay = new System.Windows.Forms.Button();
            this.MO_Cancel = new System.Windows.Forms.Button();
            this.MO_TC = new System.Windows.Forms.TabControl();
            this.MO_TP1 = new System.Windows.Forms.TabPage();
            this.MO_PrefHelpSystem = new System.Windows.Forms.ComboBox();
            this.MO_L_HelpSystem = new System.Windows.Forms.Label();
            this.MO_AutoUpdate = new System.Windows.Forms.CheckBox();
            this.MO_SortGameList = new System.Windows.Forms.CheckBox();
            this.MO_HideNotInst = new System.Windows.Forms.CheckBox();
            this.MO_ConfirmExit = new System.Windows.Forms.CheckBox();
            this.MO_TP2 = new System.Windows.Forms.TabPage();
            this.MO_RemEmptyDirs = new System.Windows.Forms.CheckBox();
            this.MO_ShBin = new System.Windows.Forms.TextBox();
            this.L_MO_ShBin = new System.Windows.Forms.Label();
            this.MO_TextEdBin = new System.Windows.Forms.TextBox();
            this.L_MO_TextEdBin = new System.Windows.Forms.Label();
            this.MO_EnableAppLogs = new System.Windows.Forms.CheckBox();
            this.MO_AllowUnsafeNCFOps = new System.Windows.Forms.CheckBox();
            this.MO_TC.SuspendLayout();
            this.MO_TP1.SuspendLayout();
            this.MO_TP2.SuspendLayout();
            this.SuspendLayout();
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
            // MO_TC
            // 
            this.MO_TC.Controls.Add(this.MO_TP1);
            this.MO_TC.Controls.Add(this.MO_TP2);
            resources.ApplyResources(this.MO_TC, "MO_TC");
            this.MO_TC.Name = "MO_TC";
            this.MO_TC.SelectedIndex = 0;
            // 
            // MO_TP1
            // 
            this.MO_TP1.Controls.Add(this.MO_PrefHelpSystem);
            this.MO_TP1.Controls.Add(this.MO_L_HelpSystem);
            this.MO_TP1.Controls.Add(this.MO_AutoUpdate);
            this.MO_TP1.Controls.Add(this.MO_SortGameList);
            this.MO_TP1.Controls.Add(this.MO_HideNotInst);
            this.MO_TP1.Controls.Add(this.MO_ConfirmExit);
            resources.ApplyResources(this.MO_TP1, "MO_TP1");
            this.MO_TP1.Name = "MO_TP1";
            this.MO_TP1.UseVisualStyleBackColor = true;
            // 
            // MO_PrefHelpSystem
            // 
            this.MO_PrefHelpSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MO_PrefHelpSystem.FormattingEnabled = true;
            this.MO_PrefHelpSystem.Items.AddRange(new object[] {
            resources.GetString("MO_PrefHelpSystem.Items"),
            resources.GetString("MO_PrefHelpSystem.Items1")});
            resources.ApplyResources(this.MO_PrefHelpSystem, "MO_PrefHelpSystem");
            this.MO_PrefHelpSystem.Name = "MO_PrefHelpSystem";
            // 
            // MO_L_HelpSystem
            // 
            resources.ApplyResources(this.MO_L_HelpSystem, "MO_L_HelpSystem");
            this.MO_L_HelpSystem.Name = "MO_L_HelpSystem";
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
            // MO_TP2
            // 
            this.MO_TP2.Controls.Add(this.MO_RemEmptyDirs);
            this.MO_TP2.Controls.Add(this.MO_ShBin);
            this.MO_TP2.Controls.Add(this.L_MO_ShBin);
            this.MO_TP2.Controls.Add(this.MO_TextEdBin);
            this.MO_TP2.Controls.Add(this.L_MO_TextEdBin);
            this.MO_TP2.Controls.Add(this.MO_EnableAppLogs);
            this.MO_TP2.Controls.Add(this.MO_AllowUnsafeNCFOps);
            resources.ApplyResources(this.MO_TP2, "MO_TP2");
            this.MO_TP2.Name = "MO_TP2";
            this.MO_TP2.UseVisualStyleBackColor = true;
            // 
            // MO_RemEmptyDirs
            // 
            resources.ApplyResources(this.MO_RemEmptyDirs, "MO_RemEmptyDirs");
            this.MO_RemEmptyDirs.Name = "MO_RemEmptyDirs";
            this.MO_RemEmptyDirs.UseVisualStyleBackColor = true;
            // 
            // MO_ShBin
            // 
            resources.ApplyResources(this.MO_ShBin, "MO_ShBin");
            this.MO_ShBin.Name = "MO_ShBin";
            // 
            // L_MO_ShBin
            // 
            resources.ApplyResources(this.L_MO_ShBin, "L_MO_ShBin");
            this.L_MO_ShBin.Name = "L_MO_ShBin";
            // 
            // MO_TextEdBin
            // 
            resources.ApplyResources(this.MO_TextEdBin, "MO_TextEdBin");
            this.MO_TextEdBin.Name = "MO_TextEdBin";
            // 
            // L_MO_TextEdBin
            // 
            resources.ApplyResources(this.L_MO_TextEdBin, "L_MO_TextEdBin");
            this.L_MO_TextEdBin.Name = "L_MO_TextEdBin";
            // 
            // MO_EnableAppLogs
            // 
            resources.ApplyResources(this.MO_EnableAppLogs, "MO_EnableAppLogs");
            this.MO_EnableAppLogs.Name = "MO_EnableAppLogs";
            this.MO_EnableAppLogs.UseVisualStyleBackColor = true;
            // 
            // MO_AllowUnsafeNCFOps
            // 
            resources.ApplyResources(this.MO_AllowUnsafeNCFOps, "MO_AllowUnsafeNCFOps");
            this.MO_AllowUnsafeNCFOps.Name = "MO_AllowUnsafeNCFOps";
            this.MO_AllowUnsafeNCFOps.UseVisualStyleBackColor = true;
            // 
            // frmOptions
            // 
            this.AcceptButton = this.MO_Okay;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.MO_Cancel;
            this.ControlBox = false;
            this.Controls.Add(this.MO_TC);
            this.Controls.Add(this.MO_Cancel);
            this.Controls.Add(this.MO_Okay);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.MO_TC.ResumeLayout(false);
            this.MO_TP1.ResumeLayout(false);
            this.MO_TP1.PerformLayout();
            this.MO_TP2.ResumeLayout(false);
            this.MO_TP2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button MO_Okay;
        private System.Windows.Forms.Button MO_Cancel;
        private System.Windows.Forms.TabControl MO_TC;
        private System.Windows.Forms.TabPage MO_TP1;
        private System.Windows.Forms.ComboBox MO_PrefHelpSystem;
        private System.Windows.Forms.Label MO_L_HelpSystem;
        private System.Windows.Forms.CheckBox MO_AutoUpdate;
        private System.Windows.Forms.CheckBox MO_SortGameList;
        private System.Windows.Forms.CheckBox MO_HideNotInst;
        private System.Windows.Forms.CheckBox MO_ConfirmExit;
        private System.Windows.Forms.TabPage MO_TP2;
        private System.Windows.Forms.CheckBox MO_EnableAppLogs;
        private System.Windows.Forms.CheckBox MO_AllowUnsafeNCFOps;
        private System.Windows.Forms.TextBox MO_ShBin;
        private System.Windows.Forms.Label L_MO_ShBin;
        private System.Windows.Forms.TextBox MO_TextEdBin;
        private System.Windows.Forms.Label L_MO_TextEdBin;
        private System.Windows.Forms.CheckBox MO_RemEmptyDirs;
    }
}