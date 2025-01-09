namespace srcrepair.gui
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
            this.ST_WlcMsg = new System.Windows.Forms.Label();
            this.ST_IDSel = new System.Windows.Forms.ComboBox();
            this.ST_Okay = new System.Windows.Forms.Button();
            this.ST_Cancel = new System.Windows.Forms.Button();
            this.ST_ShowProfile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ST_WlcMsg
            // 
            resources.ApplyResources(this.ST_WlcMsg, "ST_WlcMsg");
            this.ST_WlcMsg.Name = "ST_WlcMsg";
            // 
            // ST_IDSel
            // 
            this.ST_IDSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ST_IDSel.FormattingEnabled = true;
            resources.ApplyResources(this.ST_IDSel, "ST_IDSel");
            this.ST_IDSel.Name = "ST_IDSel";
            // 
            // ST_Okay
            // 
            resources.ApplyResources(this.ST_Okay, "ST_Okay");
            this.ST_Okay.Name = "ST_Okay";
            this.ST_Okay.UseVisualStyleBackColor = true;
            this.ST_Okay.Click += new System.EventHandler(this.ST_Okay_Click);
            // 
            // ST_Cancel
            // 
            this.ST_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.ST_Cancel, "ST_Cancel");
            this.ST_Cancel.Name = "ST_Cancel";
            this.ST_Cancel.UseVisualStyleBackColor = true;
            this.ST_Cancel.Click += new System.EventHandler(this.ST_Cancel_Click);
            // 
            // ST_ShowProfile
            // 
            this.ST_ShowProfile.Image = global::srcrepair.gui.Properties.Resources.IconArrow;
            resources.ApplyResources(this.ST_ShowProfile, "ST_ShowProfile");
            this.ST_ShowProfile.Name = "ST_ShowProfile";
            this.ST_ShowProfile.UseVisualStyleBackColor = true;
            this.ST_ShowProfile.Click += new System.EventHandler(this.ST_ShowProfile_Click);
            // 
            // FrmStmSelector
            // 
            this.AcceptButton = this.ST_Okay;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ST_Cancel;
            this.Controls.Add(this.ST_ShowProfile);
            this.Controls.Add(this.ST_Cancel);
            this.Controls.Add(this.ST_Okay);
            this.Controls.Add(this.ST_IDSel);
            this.Controls.Add(this.ST_WlcMsg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmStmSelector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.FrmStmSelector_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ST_WlcMsg;
        private System.Windows.Forms.ComboBox ST_IDSel;
        private System.Windows.Forms.Button ST_Okay;
        private System.Windows.Forms.Button ST_Cancel;
        private System.Windows.Forms.Button ST_ShowProfile;
    }
}