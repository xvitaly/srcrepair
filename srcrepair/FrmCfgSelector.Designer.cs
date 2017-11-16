namespace srcrepair
{
    partial class FrmCfgSelector
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCfgSelector));
            this.CS_Cancel = new System.Windows.Forms.Button();
            this.CS_OK = new System.Windows.Forms.Button();
            this.CS_CfgSel = new System.Windows.Forms.ComboBox();
            this.CS_WMsg = new System.Windows.Forms.Label();
            this.CS_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // CS_Cancel
            // 
            this.CS_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.CS_Cancel, "CS_Cancel");
            this.CS_Cancel.Name = "CS_Cancel";
            this.CS_Cancel.UseVisualStyleBackColor = true;
            this.CS_Cancel.Click += new System.EventHandler(this.CS_Cancel_Click);
            // 
            // CS_OK
            // 
            resources.ApplyResources(this.CS_OK, "CS_OK");
            this.CS_OK.Name = "CS_OK";
            this.CS_OK.UseVisualStyleBackColor = true;
            this.CS_OK.Click += new System.EventHandler(this.CS_OK_Click);
            // 
            // CS_CfgSel
            // 
            this.CS_CfgSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CS_CfgSel.FormattingEnabled = true;
            resources.ApplyResources(this.CS_CfgSel, "CS_CfgSel");
            this.CS_CfgSel.Name = "CS_CfgSel";
            this.CS_CfgSel.SelectedIndexChanged += new System.EventHandler(this.CS_CfgSel_SelectedIndexChanged);
            // 
            // CS_WMsg
            // 
            resources.ApplyResources(this.CS_WMsg, "CS_WMsg");
            this.CS_WMsg.Name = "CS_WMsg";
            // 
            // FrmCfgSelector
            // 
            this.AcceptButton = this.CS_OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CS_Cancel;
            this.Controls.Add(this.CS_Cancel);
            this.Controls.Add(this.CS_OK);
            this.Controls.Add(this.CS_CfgSel);
            this.Controls.Add(this.CS_WMsg);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCfgSelector";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.FrmCfgSelector_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CS_Cancel;
        private System.Windows.Forms.Button CS_OK;
        private System.Windows.Forms.ComboBox CS_CfgSel;
        private System.Windows.Forms.Label CS_WMsg;
        private System.Windows.Forms.ToolTip CS_ToolTip;
    }
}