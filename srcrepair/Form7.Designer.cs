namespace srcrepair
{
    partial class frmCleaner
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
            this.CM_WelcMsg = new System.Windows.Forms.Label();
            this.CM_FTable = new System.Windows.Forms.ListView();
            this.FName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CM_Info = new System.Windows.Forms.Label();
            this.CM_Clean = new System.Windows.Forms.Button();
            this.CM_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CM_WelcMsg
            // 
            this.CM_WelcMsg.Location = new System.Drawing.Point(12, 9);
            this.CM_WelcMsg.Name = "CM_WelcMsg";
            this.CM_WelcMsg.Size = new System.Drawing.Size(397, 29);
            this.CM_WelcMsg.TabIndex = 0;
            this.CM_WelcMsg.Text = "Пожалуйста, проверьте список файлов, которые будут удалены и подтвердите удаление" +
                ". Не выбранные файлы удалены не будут!";
            // 
            // CM_FTable
            // 
            this.CM_FTable.CheckBoxes = true;
            this.CM_FTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FName,
            this.FSize});
            this.CM_FTable.GridLines = true;
            this.CM_FTable.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.CM_FTable.Location = new System.Drawing.Point(12, 41);
            this.CM_FTable.MultiSelect = false;
            this.CM_FTable.Name = "CM_FTable";
            this.CM_FTable.ShowGroups = false;
            this.CM_FTable.ShowItemToolTips = true;
            this.CM_FTable.Size = new System.Drawing.Size(397, 183);
            this.CM_FTable.TabIndex = 1;
            this.CM_FTable.UseCompatibleStateImageBehavior = false;
            this.CM_FTable.View = System.Windows.Forms.View.Details;
            this.CM_FTable.DoubleClick += new System.EventHandler(this.CM_FTable_DoubleClick);
            this.CM_FTable.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CM_FTable_KeyDown);
            // 
            // FName
            // 
            this.FName.Text = "Имя файла";
            this.FName.Width = 310;
            // 
            // FSize
            // 
            this.FSize.Text = "Размер";
            // 
            // CM_Info
            // 
            this.CM_Info.Location = new System.Drawing.Point(9, 227);
            this.CM_Info.Name = "CM_Info";
            this.CM_Info.Size = new System.Drawing.Size(400, 17);
            this.CM_Info.TabIndex = 2;
            this.CM_Info.Text = "В результате очистки может быть освобождено до {0} МБ.";
            // 
            // CM_Clean
            // 
            this.CM_Clean.Location = new System.Drawing.Point(39, 256);
            this.CM_Clean.Name = "CM_Clean";
            this.CM_Clean.Size = new System.Drawing.Size(155, 23);
            this.CM_Clean.TabIndex = 3;
            this.CM_Clean.Text = "Подтверждаю очистку";
            this.CM_Clean.UseVisualStyleBackColor = true;
            this.CM_Clean.Click += new System.EventHandler(this.CM_Clean_Click);
            // 
            // CM_Cancel
            // 
            this.CM_Cancel.Location = new System.Drawing.Point(223, 256);
            this.CM_Cancel.Name = "CM_Cancel";
            this.CM_Cancel.Size = new System.Drawing.Size(155, 23);
            this.CM_Cancel.TabIndex = 4;
            this.CM_Cancel.Text = "Отказываюсь от очистки";
            this.CM_Cancel.UseVisualStyleBackColor = true;
            this.CM_Cancel.Click += new System.EventHandler(this.CM_Cancel_Click);
            // 
            // frmCleaner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 295);
            this.ControlBox = false;
            this.Controls.Add(this.CM_Cancel);
            this.Controls.Add(this.CM_Clean);
            this.Controls.Add(this.CM_Info);
            this.Controls.Add(this.CM_FTable);
            this.Controls.Add(this.CM_WelcMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCleaner";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Модуль очистки - {0}";
            this.Load += new System.EventHandler(this.frmCleaner_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label CM_WelcMsg;
        private System.Windows.Forms.ListView CM_FTable;
        private System.Windows.Forms.Label CM_Info;
        private System.Windows.Forms.Button CM_Clean;
        private System.Windows.Forms.Button CM_Cancel;
        private System.Windows.Forms.ColumnHeader FName;
        private System.Windows.Forms.ColumnHeader FSize;
    }
}