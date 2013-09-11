namespace srcrepair
{
    partial class frmBugReporter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBugReporter));
            this.BR_WlcMsg = new System.Windows.Forms.Label();
            this.BR_L_Title = new System.Windows.Forms.Label();
            this.BR_Title = new System.Windows.Forms.TextBox();
            this.BR_L_Category = new System.Windows.Forms.Label();
            this.BR_Category = new System.Windows.Forms.ComboBox();
            this.BR_L_Message = new System.Windows.Forms.Label();
            this.BR_Message = new System.Windows.Forms.TextBox();
            this.BR_Send = new System.Windows.Forms.Button();
            this.BR_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BR_WlcMsg
            // 
            resources.ApplyResources(this.BR_WlcMsg, "BR_WlcMsg");
            this.BR_WlcMsg.Name = "BR_WlcMsg";
            // 
            // BR_L_Title
            // 
            resources.ApplyResources(this.BR_L_Title, "BR_L_Title");
            this.BR_L_Title.Name = "BR_L_Title";
            // 
            // BR_Title
            // 
            resources.ApplyResources(this.BR_Title, "BR_Title");
            this.BR_Title.Name = "BR_Title";
            // 
            // BR_L_Category
            // 
            resources.ApplyResources(this.BR_L_Category, "BR_L_Category");
            this.BR_L_Category.Name = "BR_L_Category";
            // 
            // BR_Category
            // 
            this.BR_Category.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BR_Category.FormattingEnabled = true;
            this.BR_Category.Items.AddRange(new object[] {
            resources.GetString("BR_Category.Items"),
            resources.GetString("BR_Category.Items1"),
            resources.GetString("BR_Category.Items2")});
            resources.ApplyResources(this.BR_Category, "BR_Category");
            this.BR_Category.Name = "BR_Category";
            // 
            // BR_L_Message
            // 
            resources.ApplyResources(this.BR_L_Message, "BR_L_Message");
            this.BR_L_Message.Name = "BR_L_Message";
            // 
            // BR_Message
            // 
            resources.ApplyResources(this.BR_Message, "BR_Message");
            this.BR_Message.Name = "BR_Message";
            // 
            // BR_Send
            // 
            resources.ApplyResources(this.BR_Send, "BR_Send");
            this.BR_Send.Name = "BR_Send";
            this.BR_Send.UseVisualStyleBackColor = true;
            this.BR_Send.Click += new System.EventHandler(this.BR_Send_Click);
            // 
            // BR_Cancel
            // 
            this.BR_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.BR_Cancel, "BR_Cancel");
            this.BR_Cancel.Name = "BR_Cancel";
            this.BR_Cancel.UseVisualStyleBackColor = true;
            this.BR_Cancel.Click += new System.EventHandler(this.BR_Cancel_Click);
            // 
            // frmBugReporter
            // 
            this.AcceptButton = this.BR_Send;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BR_Cancel;
            this.Controls.Add(this.BR_Cancel);
            this.Controls.Add(this.BR_Send);
            this.Controls.Add(this.BR_Message);
            this.Controls.Add(this.BR_L_Message);
            this.Controls.Add(this.BR_Category);
            this.Controls.Add(this.BR_L_Category);
            this.Controls.Add(this.BR_Title);
            this.Controls.Add(this.BR_L_Title);
            this.Controls.Add(this.BR_WlcMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBugReporter";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmBugReporter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label BR_WlcMsg;
        private System.Windows.Forms.Label BR_L_Title;
        private System.Windows.Forms.TextBox BR_Title;
        private System.Windows.Forms.Label BR_L_Category;
        private System.Windows.Forms.ComboBox BR_Category;
        private System.Windows.Forms.Label BR_L_Message;
        private System.Windows.Forms.TextBox BR_Message;
        private System.Windows.Forms.Button BR_Send;
        private System.Windows.Forms.Button BR_Cancel;
    }
}