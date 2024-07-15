namespace srcrepair.gui
{
    partial class FrmStmClean
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStmClean));
            this.EC_Execute = new System.Windows.Forms.Button();
            this.EC_Panel = new System.Windows.Forms.Panel();
            this.EC_GB_Basic = new System.Windows.Forms.GroupBox();
            this.EC_LibraryCache = new System.Windows.Forms.CheckBox();
            this.EC_ShaderCache = new System.Windows.Forms.CheckBox();
            this.EC_DepotCache = new System.Windows.Forms.CheckBox();
            this.EC_HTTPCache = new System.Windows.Forms.CheckBox();
            this.EC_HTMLCache = new System.Windows.Forms.CheckBox();
            this.EC_GB_Trsh = new System.Windows.Forms.GroupBox();
            this.EC_Guard = new System.Windows.Forms.CheckBox();
            this.EC_Updater = new System.Windows.Forms.CheckBox();
            this.EC_GB_Garb = new System.Windows.Forms.GroupBox();
            this.EC_BuildCache = new System.Windows.Forms.CheckBox();
            this.EC_ErrDmps = new System.Windows.Forms.CheckBox();
            this.EC_OldBins = new System.Windows.Forms.CheckBox();
            this.EC_Logs = new System.Windows.Forms.CheckBox();
            this.EC_GB_ExCn = new System.Windows.Forms.GroupBox();
            this.EC_Skins = new System.Windows.Forms.CheckBox();
            this.EC_Music = new System.Windows.Forms.CheckBox();
            this.EC_Stats = new System.Windows.Forms.CheckBox();
            this.EC_Cloud = new System.Windows.Forms.CheckBox();
            this.EC_GameIcons = new System.Windows.Forms.CheckBox();
            this.EC_Cancel = new System.Windows.Forms.Button();
            this.EC_Buttons = new System.Windows.Forms.TableLayoutPanel();
            this.EC_Panel.SuspendLayout();
            this.EC_GB_Basic.SuspendLayout();
            this.EC_GB_Trsh.SuspendLayout();
            this.EC_GB_Garb.SuspendLayout();
            this.EC_GB_ExCn.SuspendLayout();
            this.EC_Buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // EC_Execute
            // 
            resources.ApplyResources(this.EC_Execute, "EC_Execute");
            this.EC_Execute.Name = "EC_Execute";
            this.EC_Execute.UseVisualStyleBackColor = true;
            this.EC_Execute.Click += new System.EventHandler(this.EC_Execute_Click);
            // 
            // EC_Panel
            // 
            resources.ApplyResources(this.EC_Panel, "EC_Panel");
            this.EC_Panel.Controls.Add(this.EC_GB_Basic);
            this.EC_Panel.Controls.Add(this.EC_GB_Trsh);
            this.EC_Panel.Controls.Add(this.EC_GB_Garb);
            this.EC_Panel.Controls.Add(this.EC_GB_ExCn);
            this.EC_Panel.Name = "EC_Panel";
            // 
            // EC_GB_Basic
            // 
            this.EC_GB_Basic.Controls.Add(this.EC_LibraryCache);
            this.EC_GB_Basic.Controls.Add(this.EC_ShaderCache);
            this.EC_GB_Basic.Controls.Add(this.EC_DepotCache);
            this.EC_GB_Basic.Controls.Add(this.EC_HTTPCache);
            this.EC_GB_Basic.Controls.Add(this.EC_HTMLCache);
            resources.ApplyResources(this.EC_GB_Basic, "EC_GB_Basic");
            this.EC_GB_Basic.Name = "EC_GB_Basic";
            this.EC_GB_Basic.TabStop = false;
            // 
            // EC_LibraryCache
            // 
            resources.ApplyResources(this.EC_LibraryCache, "EC_LibraryCache");
            this.EC_LibraryCache.Name = "EC_LibraryCache";
            this.EC_LibraryCache.UseVisualStyleBackColor = true;
            // 
            // EC_ShaderCache
            // 
            resources.ApplyResources(this.EC_ShaderCache, "EC_ShaderCache");
            this.EC_ShaderCache.Name = "EC_ShaderCache";
            this.EC_ShaderCache.UseVisualStyleBackColor = true;
            // 
            // EC_DepotCache
            // 
            resources.ApplyResources(this.EC_DepotCache, "EC_DepotCache");
            this.EC_DepotCache.Name = "EC_DepotCache";
            this.EC_DepotCache.UseVisualStyleBackColor = true;
            // 
            // EC_HTTPCache
            // 
            resources.ApplyResources(this.EC_HTTPCache, "EC_HTTPCache");
            this.EC_HTTPCache.Name = "EC_HTTPCache";
            this.EC_HTTPCache.UseVisualStyleBackColor = true;
            // 
            // EC_HTMLCache
            // 
            resources.ApplyResources(this.EC_HTMLCache, "EC_HTMLCache");
            this.EC_HTMLCache.Name = "EC_HTMLCache";
            this.EC_HTMLCache.UseVisualStyleBackColor = true;
            // 
            // EC_GB_Trsh
            // 
            this.EC_GB_Trsh.Controls.Add(this.EC_Guard);
            this.EC_GB_Trsh.Controls.Add(this.EC_Updater);
            resources.ApplyResources(this.EC_GB_Trsh, "EC_GB_Trsh");
            this.EC_GB_Trsh.Name = "EC_GB_Trsh";
            this.EC_GB_Trsh.TabStop = false;
            // 
            // EC_Guard
            // 
            resources.ApplyResources(this.EC_Guard, "EC_Guard");
            this.EC_Guard.Name = "EC_Guard";
            this.EC_Guard.UseVisualStyleBackColor = true;
            // 
            // EC_Updater
            // 
            resources.ApplyResources(this.EC_Updater, "EC_Updater");
            this.EC_Updater.Name = "EC_Updater";
            this.EC_Updater.UseVisualStyleBackColor = true;
            // 
            // EC_GB_Garb
            // 
            this.EC_GB_Garb.Controls.Add(this.EC_BuildCache);
            this.EC_GB_Garb.Controls.Add(this.EC_ErrDmps);
            this.EC_GB_Garb.Controls.Add(this.EC_OldBins);
            this.EC_GB_Garb.Controls.Add(this.EC_Logs);
            resources.ApplyResources(this.EC_GB_Garb, "EC_GB_Garb");
            this.EC_GB_Garb.Name = "EC_GB_Garb";
            this.EC_GB_Garb.TabStop = false;
            // 
            // EC_BuildCache
            // 
            resources.ApplyResources(this.EC_BuildCache, "EC_BuildCache");
            this.EC_BuildCache.Name = "EC_BuildCache";
            this.EC_BuildCache.UseVisualStyleBackColor = true;
            // 
            // EC_ErrDmps
            // 
            resources.ApplyResources(this.EC_ErrDmps, "EC_ErrDmps");
            this.EC_ErrDmps.Name = "EC_ErrDmps";
            this.EC_ErrDmps.UseVisualStyleBackColor = true;
            // 
            // EC_OldBins
            // 
            resources.ApplyResources(this.EC_OldBins, "EC_OldBins");
            this.EC_OldBins.Name = "EC_OldBins";
            this.EC_OldBins.UseVisualStyleBackColor = true;
            // 
            // EC_Logs
            // 
            resources.ApplyResources(this.EC_Logs, "EC_Logs");
            this.EC_Logs.Name = "EC_Logs";
            this.EC_Logs.UseVisualStyleBackColor = true;
            // 
            // EC_GB_ExCn
            // 
            this.EC_GB_ExCn.Controls.Add(this.EC_Skins);
            this.EC_GB_ExCn.Controls.Add(this.EC_Music);
            this.EC_GB_ExCn.Controls.Add(this.EC_Stats);
            this.EC_GB_ExCn.Controls.Add(this.EC_Cloud);
            this.EC_GB_ExCn.Controls.Add(this.EC_GameIcons);
            resources.ApplyResources(this.EC_GB_ExCn, "EC_GB_ExCn");
            this.EC_GB_ExCn.Name = "EC_GB_ExCn";
            this.EC_GB_ExCn.TabStop = false;
            // 
            // EC_Skins
            // 
            resources.ApplyResources(this.EC_Skins, "EC_Skins");
            this.EC_Skins.Name = "EC_Skins";
            this.EC_Skins.UseVisualStyleBackColor = true;
            // 
            // EC_Music
            // 
            resources.ApplyResources(this.EC_Music, "EC_Music");
            this.EC_Music.Name = "EC_Music";
            this.EC_Music.UseVisualStyleBackColor = true;
            // 
            // EC_Stats
            // 
            resources.ApplyResources(this.EC_Stats, "EC_Stats");
            this.EC_Stats.Name = "EC_Stats";
            this.EC_Stats.UseVisualStyleBackColor = true;
            // 
            // EC_Cloud
            // 
            resources.ApplyResources(this.EC_Cloud, "EC_Cloud");
            this.EC_Cloud.Name = "EC_Cloud";
            this.EC_Cloud.UseVisualStyleBackColor = true;
            // 
            // EC_GameIcons
            // 
            resources.ApplyResources(this.EC_GameIcons, "EC_GameIcons");
            this.EC_GameIcons.Name = "EC_GameIcons";
            this.EC_GameIcons.UseVisualStyleBackColor = true;
            // 
            // EC_Cancel
            // 
            resources.ApplyResources(this.EC_Cancel, "EC_Cancel");
            this.EC_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.EC_Cancel.Name = "EC_Cancel";
            this.EC_Cancel.UseVisualStyleBackColor = true;
            this.EC_Cancel.Click += new System.EventHandler(this.EC_Cancel_Click);
            // 
            // EC_Buttons
            // 
            resources.ApplyResources(this.EC_Buttons, "EC_Buttons");
            this.EC_Buttons.Controls.Add(this.EC_Execute, 0, 0);
            this.EC_Buttons.Controls.Add(this.EC_Cancel, 1, 0);
            this.EC_Buttons.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.EC_Buttons.Name = "EC_Buttons";
            // 
            // FrmStmClean
            // 
            this.AcceptButton = this.EC_Execute;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.EC_Cancel;
            this.Controls.Add(this.EC_Panel);
            this.Controls.Add(this.EC_Buttons);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmStmClean";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.EC_Panel.ResumeLayout(false);
            this.EC_GB_Basic.ResumeLayout(false);
            this.EC_GB_Basic.PerformLayout();
            this.EC_GB_Trsh.ResumeLayout(false);
            this.EC_GB_Trsh.PerformLayout();
            this.EC_GB_Garb.ResumeLayout(false);
            this.EC_GB_Garb.PerformLayout();
            this.EC_GB_ExCn.ResumeLayout(false);
            this.EC_GB_ExCn.PerformLayout();
            this.EC_Buttons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button EC_Execute;
        private System.Windows.Forms.Panel EC_Panel;
        private System.Windows.Forms.GroupBox EC_GB_Basic;
        private System.Windows.Forms.CheckBox EC_LibraryCache;
        private System.Windows.Forms.CheckBox EC_ShaderCache;
        private System.Windows.Forms.CheckBox EC_DepotCache;
        private System.Windows.Forms.CheckBox EC_HTTPCache;
        private System.Windows.Forms.CheckBox EC_HTMLCache;
        private System.Windows.Forms.GroupBox EC_GB_Trsh;
        private System.Windows.Forms.CheckBox EC_Guard;
        private System.Windows.Forms.CheckBox EC_Updater;
        private System.Windows.Forms.GroupBox EC_GB_Garb;
        private System.Windows.Forms.CheckBox EC_BuildCache;
        private System.Windows.Forms.CheckBox EC_ErrDmps;
        private System.Windows.Forms.CheckBox EC_OldBins;
        private System.Windows.Forms.CheckBox EC_Logs;
        private System.Windows.Forms.GroupBox EC_GB_ExCn;
        private System.Windows.Forms.CheckBox EC_Skins;
        private System.Windows.Forms.CheckBox EC_Music;
        private System.Windows.Forms.CheckBox EC_Stats;
        private System.Windows.Forms.CheckBox EC_Cloud;
        private System.Windows.Forms.CheckBox EC_GameIcons;
        private System.Windows.Forms.Button EC_Cancel;
        private System.Windows.Forms.TableLayoutPanel EC_Buttons;
    }
}