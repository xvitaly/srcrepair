namespace srcrepair
{
    partial class frmAbout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.iconApp = new System.Windows.Forms.PictureBox();
            this.labelProductName = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelCompanyName = new System.Windows.Forms.Label();
            this.btnContact = new System.Windows.Forms.Button();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelFlags = new System.Windows.Forms.Label();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.iconApp)).BeginInit();
            this.SuspendLayout();
            // 
            // iconApp
            // 
            resources.ApplyResources(this.iconApp, "iconApp");
            this.iconApp.Name = "iconApp";
            this.iconApp.TabStop = false;
            // 
            // labelProductName
            // 
            resources.ApplyResources(this.labelProductName, "labelProductName");
            this.labelProductName.Name = "labelProductName";
            // 
            // labelVersion
            // 
            resources.ApplyResources(this.labelVersion, "labelVersion");
            this.labelVersion.Name = "labelVersion";
            // 
            // labelCompanyName
            // 
            resources.ApplyResources(this.labelCompanyName, "labelCompanyName");
            this.labelCompanyName.Name = "labelCompanyName";
            // 
            // btnContact
            // 
            resources.ApplyResources(this.btnContact, "btnContact");
            this.btnContact.Name = "btnContact";
            this.btnContact.TabStop = false;
            this.btnContact.UseVisualStyleBackColor = true;
            this.btnContact.Click += new System.EventHandler(this.btnContact_Click);
            // 
            // textBoxDescription
            // 
            resources.ApplyResources(this.textBoxDescription, "textBoxDescription");
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.TabStop = false;
            // 
            // labelFlags
            // 
            resources.ApplyResources(this.labelFlags, "labelFlags");
            this.labelFlags.Name = "labelFlags";
            // 
            // labelCopyright
            // 
            resources.ApplyResources(this.labelCopyright, "labelCopyright");
            this.labelCopyright.Name = "labelCopyright";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // frmAbout
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.okButton;
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.labelCopyright);
            this.Controls.Add(this.labelFlags);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.btnContact);
            this.Controls.Add(this.labelCompanyName);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelProductName);
            this.Controls.Add(this.iconApp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iconApp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox iconApp;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelCompanyName;
        private System.Windows.Forms.Button btnContact;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelFlags;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.Button okButton;

    }
}
