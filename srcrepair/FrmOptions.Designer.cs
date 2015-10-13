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
            this.MO_UseUpstream = new System.Windows.Forms.CheckBox();
            this.MO_RemEmptyDirs = new System.Windows.Forms.CheckBox();
            this.MO_ZipCompress = new System.Windows.Forms.CheckBox();
            this.MO_AutoRestAfterUpdateDb = new System.Windows.Forms.CheckBox();
            this.MO_AutoUpdate = new System.Windows.Forms.CheckBox();
            this.MO_SaveHUDPackages = new System.Windows.Forms.CheckBox();
            this.MO_ConfirmExit = new System.Windows.Forms.CheckBox();
            this.MO_TP2 = new System.Windows.Forms.TabPage();
            this.MO_AllowBeta = new System.Windows.Forms.CheckBox();
            this.MO_UnSafeOps = new System.Windows.Forms.CheckBox();
            this.MO_CustDirName = new System.Windows.Forms.TextBox();
            this.L_MO_CustDirName = new System.Windows.Forms.Label();
            this.MO_FindShBin = new System.Windows.Forms.Button();
            this.MO_FindTextEd = new System.Windows.Forms.Button();
            this.MO_ShBin = new System.Windows.Forms.TextBox();
            this.L_MO_ShBin = new System.Windows.Forms.Label();
            this.MO_TextEdBin = new System.Windows.Forms.TextBox();
            this.L_MO_TextEdBin = new System.Windows.Forms.Label();
            this.MO_EnableAppLogs = new System.Windows.Forms.CheckBox();
            this.MO_SearchBin = new System.Windows.Forms.OpenFileDialog();
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
            this.MO_TP1.Controls.Add(this.MO_UseUpstream);
            this.MO_TP1.Controls.Add(this.MO_RemEmptyDirs);
            this.MO_TP1.Controls.Add(this.MO_ZipCompress);
            this.MO_TP1.Controls.Add(this.MO_AutoRestAfterUpdateDb);
            this.MO_TP1.Controls.Add(this.MO_AutoUpdate);
            this.MO_TP1.Controls.Add(this.MO_SaveHUDPackages);
            this.MO_TP1.Controls.Add(this.MO_ConfirmExit);
            resources.ApplyResources(this.MO_TP1, "MO_TP1");
            this.MO_TP1.Name = "MO_TP1";
            this.MO_TP1.UseVisualStyleBackColor = true;
            // 
            // MO_UseUpstream
            // 
            resources.ApplyResources(this.MO_UseUpstream, "MO_UseUpstream");
            this.MO_UseUpstream.Name = "MO_UseUpstream";
            this.MO_UseUpstream.UseVisualStyleBackColor = true;
            // 
            // MO_RemEmptyDirs
            // 
            resources.ApplyResources(this.MO_RemEmptyDirs, "MO_RemEmptyDirs");
            this.MO_RemEmptyDirs.Name = "MO_RemEmptyDirs";
            this.MO_RemEmptyDirs.UseVisualStyleBackColor = true;
            // 
            // MO_ZipCompress
            // 
            resources.ApplyResources(this.MO_ZipCompress, "MO_ZipCompress");
            this.MO_ZipCompress.Name = "MO_ZipCompress";
            this.MO_ZipCompress.UseVisualStyleBackColor = true;
            // 
            // MO_AutoRestAfterUpdateDb
            // 
            resources.ApplyResources(this.MO_AutoRestAfterUpdateDb, "MO_AutoRestAfterUpdateDb");
            this.MO_AutoRestAfterUpdateDb.Name = "MO_AutoRestAfterUpdateDb";
            this.MO_AutoRestAfterUpdateDb.UseVisualStyleBackColor = true;
            // 
            // MO_AutoUpdate
            // 
            resources.ApplyResources(this.MO_AutoUpdate, "MO_AutoUpdate");
            this.MO_AutoUpdate.Name = "MO_AutoUpdate";
            this.MO_AutoUpdate.UseVisualStyleBackColor = true;
            // 
            // MO_SaveHUDPackages
            // 
            resources.ApplyResources(this.MO_SaveHUDPackages, "MO_SaveHUDPackages");
            this.MO_SaveHUDPackages.Name = "MO_SaveHUDPackages";
            this.MO_SaveHUDPackages.UseVisualStyleBackColor = true;
            // 
            // MO_ConfirmExit
            // 
            resources.ApplyResources(this.MO_ConfirmExit, "MO_ConfirmExit");
            this.MO_ConfirmExit.Name = "MO_ConfirmExit";
            this.MO_ConfirmExit.UseVisualStyleBackColor = true;
            // 
            // MO_TP2
            // 
            this.MO_TP2.Controls.Add(this.MO_AllowBeta);
            this.MO_TP2.Controls.Add(this.MO_UnSafeOps);
            this.MO_TP2.Controls.Add(this.MO_CustDirName);
            this.MO_TP2.Controls.Add(this.L_MO_CustDirName);
            this.MO_TP2.Controls.Add(this.MO_FindShBin);
            this.MO_TP2.Controls.Add(this.MO_FindTextEd);
            this.MO_TP2.Controls.Add(this.MO_ShBin);
            this.MO_TP2.Controls.Add(this.L_MO_ShBin);
            this.MO_TP2.Controls.Add(this.MO_TextEdBin);
            this.MO_TP2.Controls.Add(this.L_MO_TextEdBin);
            this.MO_TP2.Controls.Add(this.MO_EnableAppLogs);
            resources.ApplyResources(this.MO_TP2, "MO_TP2");
            this.MO_TP2.Name = "MO_TP2";
            this.MO_TP2.UseVisualStyleBackColor = true;
            // 
            // MO_AllowBeta
            // 
            resources.ApplyResources(this.MO_AllowBeta, "MO_AllowBeta");
            this.MO_AllowBeta.Name = "MO_AllowBeta";
            this.MO_AllowBeta.UseVisualStyleBackColor = true;
            // 
            // MO_UnSafeOps
            // 
            resources.ApplyResources(this.MO_UnSafeOps, "MO_UnSafeOps");
            this.MO_UnSafeOps.Name = "MO_UnSafeOps";
            this.MO_UnSafeOps.UseVisualStyleBackColor = true;
            // 
            // MO_CustDirName
            // 
            resources.ApplyResources(this.MO_CustDirName, "MO_CustDirName");
            this.MO_CustDirName.Name = "MO_CustDirName";
            this.MO_CustDirName.TextChanged += new System.EventHandler(this.MO_CustDirName_TextChanged);
            // 
            // L_MO_CustDirName
            // 
            resources.ApplyResources(this.L_MO_CustDirName, "L_MO_CustDirName");
            this.L_MO_CustDirName.Name = "L_MO_CustDirName";
            // 
            // MO_FindShBin
            // 
            this.MO_FindShBin.Image = global::srcrepair.Properties.Resources.Search;
            resources.ApplyResources(this.MO_FindShBin, "MO_FindShBin");
            this.MO_FindShBin.Name = "MO_FindShBin";
            this.MO_FindShBin.UseVisualStyleBackColor = true;
            this.MO_FindShBin.Click += new System.EventHandler(this.MO_FindShBin_Click);
            // 
            // MO_FindTextEd
            // 
            this.MO_FindTextEd.Image = global::srcrepair.Properties.Resources.Search;
            resources.ApplyResources(this.MO_FindTextEd, "MO_FindTextEd");
            this.MO_FindTextEd.Name = "MO_FindTextEd";
            this.MO_FindTextEd.UseVisualStyleBackColor = true;
            this.MO_FindTextEd.Click += new System.EventHandler(this.MO_FindTextEd_Click);
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
            // MO_SearchBin
            // 
            resources.ApplyResources(this.MO_SearchBin, "MO_SearchBin");
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
            this.MO_TP2.ResumeLayout(false);
            this.MO_TP2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button MO_Okay;
        private System.Windows.Forms.Button MO_Cancel;
        private System.Windows.Forms.TabControl MO_TC;
        private System.Windows.Forms.TabPage MO_TP1;
        private System.Windows.Forms.CheckBox MO_AutoUpdate;
        private System.Windows.Forms.CheckBox MO_SaveHUDPackages;
        private System.Windows.Forms.CheckBox MO_ConfirmExit;
        private System.Windows.Forms.TabPage MO_TP2;
        private System.Windows.Forms.CheckBox MO_EnableAppLogs;
        private System.Windows.Forms.TextBox MO_ShBin;
        private System.Windows.Forms.Label L_MO_ShBin;
        private System.Windows.Forms.TextBox MO_TextEdBin;
        private System.Windows.Forms.Label L_MO_TextEdBin;
        private System.Windows.Forms.Button MO_FindShBin;
        private System.Windows.Forms.Button MO_FindTextEd;
        private System.Windows.Forms.OpenFileDialog MO_SearchBin;
        private System.Windows.Forms.CheckBox MO_AutoRestAfterUpdateDb;
        private System.Windows.Forms.TextBox MO_CustDirName;
        private System.Windows.Forms.Label L_MO_CustDirName;
        private System.Windows.Forms.CheckBox MO_ZipCompress;
        private System.Windows.Forms.CheckBox MO_UnSafeOps;
        private System.Windows.Forms.CheckBox MO_RemEmptyDirs;
        private System.Windows.Forms.CheckBox MO_UseUpstream;
        private System.Windows.Forms.CheckBox MO_AllowBeta;
    }
}