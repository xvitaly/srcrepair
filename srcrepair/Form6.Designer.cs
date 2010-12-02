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
            this.SuspendLayout();
            // 
            // GenerateCFG
            // 
            resources.ApplyResources(this.GenerateCFG, "GenerateCFG");
            this.GenerateCFG.Name = "GenerateCFG";
            this.GenerateCFG.UseVisualStyleBackColor = true;
            this.GenerateCFG.Click += new System.EventHandler(this.GenerateCFG_Click);
            // 
            // frmFPGen
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GenerateCFG);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFPGen";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button GenerateCFG;
    }
}