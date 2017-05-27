namespace srcrepair
{
    partial class FrmStmSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStmSelector));
            this.SD_WMsg = new System.Windows.Forms.Label();
            this.SD_IDSel = new System.Windows.Forms.ComboBox();
            this.ST_OK = new System.Windows.Forms.Button();
            this.ST_Cancel = new System.Windows.Forms.Button();
            this.SD_Follow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SD_WMsg
            // 
            resources.ApplyResources(this.SD_WMsg, "SD_WMsg");
            this.SD_WMsg.Name = "SD_WMsg";
            // 
            // SD_IDSel
            // 
            this.SD_IDSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SD_IDSel.FormattingEnabled = true;
            resources.ApplyResources(this.SD_IDSel, "SD_IDSel");
            this.SD_IDSel.Name = "SD_IDSel";
            // 
            // ST_OK
            // 
            resources.ApplyResources(this.ST_OK, "ST_OK");
            this.ST_OK.Name = "ST_OK";
            this.ST_OK.UseVisualStyleBackColor = true;
            this.ST_OK.Click += new System.EventHandler(this.ST_OK_Click);
            // 
            // ST_Cancel
            // 
            this.ST_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.ST_Cancel, "ST_Cancel");
            this.ST_Cancel.Name = "ST_Cancel";
            this.ST_Cancel.UseVisualStyleBackColor = true;
            this.ST_Cancel.Click += new System.EventHandler(this.ST_Cancel_Click);
            // 
            // SD_Follow
            // 
            this.SD_Follow.Image = global::srcrepair.Properties.Resources.arrow;
            resources.ApplyResources(this.SD_Follow, "SD_Follow");
            this.SD_Follow.Name = "SD_Follow";
            this.SD_Follow.UseVisualStyleBackColor = true;
            this.SD_Follow.Click += new System.EventHandler(this.SD_Follow_Click);
            // 
            // FrmStmSelector
            // 
            this.AcceptButton = this.ST_OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ST_Cancel;
            this.Controls.Add(this.SD_Follow);
            this.Controls.Add(this.ST_Cancel);
            this.Controls.Add(this.ST_OK);
            this.Controls.Add(this.SD_IDSel);
            this.Controls.Add(this.SD_WMsg);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmStmSelector";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.FrmStmSelector_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label SD_WMsg;
        private System.Windows.Forms.ComboBox SD_IDSel;
        private System.Windows.Forms.Button ST_OK;
        private System.Windows.Forms.Button ST_Cancel;
        private System.Windows.Forms.Button SD_Follow;
    }
}