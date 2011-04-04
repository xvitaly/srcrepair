namespace srcrepair
{
    partial class frmFPGen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFPGen));
            this.GenerateCFG = new System.Windows.Forms.Button();
            this.TextureQuality = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextureQualityBox = new System.Windows.Forms.ComboBox();
            this.ModelQuality = new System.Windows.Forms.CheckBox();
            this.ModelQualityBox = new System.Windows.Forms.ComboBox();
            this.DisableRagdollPhys = new System.Windows.Forms.CheckBox();
            this.DisableShadows = new System.Windows.Forms.CheckBox();
            this.ReduceWaterQuality = new System.Windows.Forms.CheckBox();
            this.DisableEffects = new System.Windows.Forms.CheckBox();
            this.EnableTrilinearF = new System.Windows.Forms.CheckBox();
            this.DisableDecals = new System.Windows.Forms.CheckBox();
            this.DisableShaderE = new System.Windows.Forms.CheckBox();
            this.DisableBreaks = new System.Windows.Forms.CheckBox();
            this.EnableHardOpt = new System.Windows.Forms.CheckBox();
            this.DisableSmallObjs = new System.Windows.Forms.CheckBox();
            this.EnablePreload = new System.Windows.Forms.CheckBox();
            this.DisableVSync = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // GenerateCFG
            // 
            resources.ApplyResources(this.GenerateCFG, "GenerateCFG");
            this.GenerateCFG.Name = "GenerateCFG";
            this.GenerateCFG.UseVisualStyleBackColor = true;
            this.GenerateCFG.Click += new System.EventHandler(this.GenerateCFG_Click);
            // 
            // TextureQuality
            // 
            resources.ApplyResources(this.TextureQuality, "TextureQuality");
            this.TextureQuality.Name = "TextureQuality";
            this.TextureQuality.UseVisualStyleBackColor = true;
            this.TextureQuality.CheckedChanged += new System.EventHandler(this.TextureQuality_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // TextureQualityBox
            // 
            this.TextureQualityBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.TextureQualityBox, "TextureQualityBox");
            this.TextureQualityBox.FormattingEnabled = true;
            this.TextureQualityBox.Items.AddRange(new object[] {
            resources.GetString("TextureQualityBox.Items"),
            resources.GetString("TextureQualityBox.Items1")});
            this.TextureQualityBox.Name = "TextureQualityBox";
            // 
            // ModelQuality
            // 
            resources.ApplyResources(this.ModelQuality, "ModelQuality");
            this.ModelQuality.Name = "ModelQuality";
            this.ModelQuality.UseVisualStyleBackColor = true;
            this.ModelQuality.CheckedChanged += new System.EventHandler(this.ModelQuality_CheckedChanged);
            // 
            // ModelQualityBox
            // 
            this.ModelQualityBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.ModelQualityBox, "ModelQualityBox");
            this.ModelQualityBox.FormattingEnabled = true;
            this.ModelQualityBox.Items.AddRange(new object[] {
            resources.GetString("ModelQualityBox.Items"),
            resources.GetString("ModelQualityBox.Items1")});
            this.ModelQualityBox.Name = "ModelQualityBox";
            // 
            // DisableRagdollPhys
            // 
            resources.ApplyResources(this.DisableRagdollPhys, "DisableRagdollPhys");
            this.DisableRagdollPhys.Name = "DisableRagdollPhys";
            this.DisableRagdollPhys.UseVisualStyleBackColor = true;
            // 
            // DisableShadows
            // 
            resources.ApplyResources(this.DisableShadows, "DisableShadows");
            this.DisableShadows.Name = "DisableShadows";
            this.DisableShadows.UseVisualStyleBackColor = true;
            // 
            // ReduceWaterQuality
            // 
            resources.ApplyResources(this.ReduceWaterQuality, "ReduceWaterQuality");
            this.ReduceWaterQuality.Name = "ReduceWaterQuality";
            this.ReduceWaterQuality.UseVisualStyleBackColor = true;
            // 
            // DisableEffects
            // 
            resources.ApplyResources(this.DisableEffects, "DisableEffects");
            this.DisableEffects.Name = "DisableEffects";
            this.DisableEffects.UseVisualStyleBackColor = true;
            // 
            // EnableTrilinearF
            // 
            resources.ApplyResources(this.EnableTrilinearF, "EnableTrilinearF");
            this.EnableTrilinearF.Name = "EnableTrilinearF";
            this.EnableTrilinearF.UseVisualStyleBackColor = true;
            // 
            // DisableDecals
            // 
            resources.ApplyResources(this.DisableDecals, "DisableDecals");
            this.DisableDecals.Name = "DisableDecals";
            this.DisableDecals.UseVisualStyleBackColor = true;
            // 
            // DisableShaderE
            // 
            resources.ApplyResources(this.DisableShaderE, "DisableShaderE");
            this.DisableShaderE.Name = "DisableShaderE";
            this.DisableShaderE.UseVisualStyleBackColor = true;
            // 
            // DisableBreaks
            // 
            resources.ApplyResources(this.DisableBreaks, "DisableBreaks");
            this.DisableBreaks.Name = "DisableBreaks";
            this.DisableBreaks.UseVisualStyleBackColor = true;
            // 
            // EnableHardOpt
            // 
            resources.ApplyResources(this.EnableHardOpt, "EnableHardOpt");
            this.EnableHardOpt.Name = "EnableHardOpt";
            this.EnableHardOpt.UseVisualStyleBackColor = true;
            // 
            // DisableSmallObjs
            // 
            resources.ApplyResources(this.DisableSmallObjs, "DisableSmallObjs");
            this.DisableSmallObjs.Name = "DisableSmallObjs";
            this.DisableSmallObjs.UseVisualStyleBackColor = true;
            // 
            // EnablePreload
            // 
            resources.ApplyResources(this.EnablePreload, "EnablePreload");
            this.EnablePreload.Name = "EnablePreload";
            this.EnablePreload.UseVisualStyleBackColor = true;
            // 
            // DisableVSync
            // 
            resources.ApplyResources(this.DisableVSync, "DisableVSync");
            this.DisableVSync.Name = "DisableVSync";
            this.DisableVSync.UseVisualStyleBackColor = true;
            // 
            // frmFPGen
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DisableVSync);
            this.Controls.Add(this.EnablePreload);
            this.Controls.Add(this.DisableSmallObjs);
            this.Controls.Add(this.EnableHardOpt);
            this.Controls.Add(this.DisableBreaks);
            this.Controls.Add(this.DisableShaderE);
            this.Controls.Add(this.DisableDecals);
            this.Controls.Add(this.EnableTrilinearF);
            this.Controls.Add(this.DisableEffects);
            this.Controls.Add(this.ReduceWaterQuality);
            this.Controls.Add(this.DisableShadows);
            this.Controls.Add(this.DisableRagdollPhys);
            this.Controls.Add(this.ModelQualityBox);
            this.Controls.Add(this.ModelQuality);
            this.Controls.Add(this.TextureQualityBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextureQuality);
            this.Controls.Add(this.GenerateCFG);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFPGen";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GenerateCFG;
        private System.Windows.Forms.CheckBox TextureQuality;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox TextureQualityBox;
        private System.Windows.Forms.CheckBox ModelQuality;
        private System.Windows.Forms.ComboBox ModelQualityBox;
        private System.Windows.Forms.CheckBox DisableRagdollPhys;
        private System.Windows.Forms.CheckBox DisableShadows;
        private System.Windows.Forms.CheckBox ReduceWaterQuality;
        private System.Windows.Forms.CheckBox DisableEffects;
        private System.Windows.Forms.CheckBox EnableTrilinearF;
        private System.Windows.Forms.CheckBox DisableDecals;
        private System.Windows.Forms.CheckBox DisableShaderE;
        private System.Windows.Forms.CheckBox DisableBreaks;
        private System.Windows.Forms.CheckBox EnableHardOpt;
        private System.Windows.Forms.CheckBox DisableSmallObjs;
        private System.Windows.Forms.CheckBox EnablePreload;
        private System.Windows.Forms.CheckBox DisableVSync;
    }
}