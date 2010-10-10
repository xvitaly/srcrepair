namespace srcrepair
{
    partial class frmMainW
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
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.GraphicTweaker = new System.Windows.Forms.TabPage();
            this.ConfigEditor = new System.Windows.Forms.TabPage();
            this.ProblemSolver = new System.Windows.Forms.TabPage();
            this.FPSCfgInstall = new System.Windows.Forms.TabPage();
            this.RescueCentre = new System.Windows.Forms.TabPage();
            this.LoginSel = new System.Windows.Forms.ComboBox();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.ToolsMNU = new System.Windows.Forms.ToolStripMenuItem();
            this.MNUShowEdHint = new System.Windows.Forms.ToolStripMenuItem();
            this.MNUSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.MNUFPSWizard = new System.Windows.Forms.ToolStripMenuItem();
            this.MNUReportBuilder = new System.Windows.Forms.ToolStripMenuItem();
            this.MNUInstaller = new System.Windows.Forms.ToolStripMenuItem();
            this.MNUSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.MNUExit = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMNU = new System.Windows.Forms.ToolStripMenuItem();
            this.MNUHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.MNUOpinion = new System.Windows.Forms.ToolStripMenuItem();
            this.MNUReportBug = new System.Windows.Forms.ToolStripMenuItem();
            this.MNUSteamGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.MNUSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.MNUGroup1 = new System.Windows.Forms.ToolStripMenuItem();
            this.MNUGroup2 = new System.Windows.Forms.ToolStripMenuItem();
            this.MNUGroup3 = new System.Windows.Forms.ToolStripMenuItem();
            this.MNUSep4 = new System.Windows.Forms.ToolStripSeparator();
            this.MNUAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.LLoginSel = new System.Windows.Forms.Label();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.SB_Info = new System.Windows.Forms.ToolStripStatusLabel();
            this.SB_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainTabControl.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.GraphicTweaker);
            this.MainTabControl.Controls.Add(this.ConfigEditor);
            this.MainTabControl.Controls.Add(this.ProblemSolver);
            this.MainTabControl.Controls.Add(this.FPSCfgInstall);
            this.MainTabControl.Controls.Add(this.RescueCentre);
            this.MainTabControl.Location = new System.Drawing.Point(12, 64);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(470, 394);
            this.MainTabControl.TabIndex = 0;
            // 
            // GraphicTweaker
            // 
            this.GraphicTweaker.Location = new System.Drawing.Point(4, 22);
            this.GraphicTweaker.Name = "GraphicTweaker";
            this.GraphicTweaker.Padding = new System.Windows.Forms.Padding(3);
            this.GraphicTweaker.Size = new System.Drawing.Size(462, 368);
            this.GraphicTweaker.TabIndex = 0;
            this.GraphicTweaker.Text = "Графические настройки";
            this.GraphicTweaker.UseVisualStyleBackColor = true;
            // 
            // ConfigEditor
            // 
            this.ConfigEditor.Location = new System.Drawing.Point(4, 22);
            this.ConfigEditor.Name = "ConfigEditor";
            this.ConfigEditor.Padding = new System.Windows.Forms.Padding(3);
            this.ConfigEditor.Size = new System.Drawing.Size(462, 368);
            this.ConfigEditor.TabIndex = 1;
            this.ConfigEditor.Text = "Редактор конфигов";
            this.ConfigEditor.UseVisualStyleBackColor = true;
            // 
            // ProblemSolver
            // 
            this.ProblemSolver.Location = new System.Drawing.Point(4, 22);
            this.ProblemSolver.Name = "ProblemSolver";
            this.ProblemSolver.Padding = new System.Windows.Forms.Padding(3);
            this.ProblemSolver.Size = new System.Drawing.Size(462, 368);
            this.ProblemSolver.TabIndex = 2;
            this.ProblemSolver.Text = "Устранение проблем";
            this.ProblemSolver.UseVisualStyleBackColor = true;
            // 
            // FPSCfgInstall
            // 
            this.FPSCfgInstall.Location = new System.Drawing.Point(4, 22);
            this.FPSCfgInstall.Name = "FPSCfgInstall";
            this.FPSCfgInstall.Padding = new System.Windows.Forms.Padding(3);
            this.FPSCfgInstall.Size = new System.Drawing.Size(462, 368);
            this.FPSCfgInstall.TabIndex = 3;
            this.FPSCfgInstall.Text = "FPS-конфиги";
            this.FPSCfgInstall.UseVisualStyleBackColor = true;
            // 
            // RescueCentre
            // 
            this.RescueCentre.Location = new System.Drawing.Point(4, 22);
            this.RescueCentre.Name = "RescueCentre";
            this.RescueCentre.Padding = new System.Windows.Forms.Padding(3);
            this.RescueCentre.Size = new System.Drawing.Size(462, 368);
            this.RescueCentre.TabIndex = 4;
            this.RescueCentre.Text = "Резервные копии";
            this.RescueCentre.UseVisualStyleBackColor = true;
            // 
            // LoginSel
            // 
            this.LoginSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LoginSel.FormattingEnabled = true;
            this.LoginSel.Location = new System.Drawing.Point(239, 37);
            this.LoginSel.Name = "LoginSel";
            this.LoginSel.Size = new System.Drawing.Size(149, 21);
            this.LoginSel.TabIndex = 1;
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolsMNU,
            this.HelpMNU});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(494, 24);
            this.MainMenu.TabIndex = 2;
            this.MainMenu.Text = "menuStrip1";
            // 
            // ToolsMNU
            // 
            this.ToolsMNU.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MNUShowEdHint,
            this.MNUSep1,
            this.MNUFPSWizard,
            this.MNUReportBuilder,
            this.MNUInstaller,
            this.MNUSep2,
            this.MNUExit});
            this.ToolsMNU.Name = "ToolsMNU";
            this.ToolsMNU.Size = new System.Drawing.Size(87, 20);
            this.ToolsMNU.Text = "&Инструменты";
            // 
            // MNUShowEdHint
            // 
            this.MNUShowEdHint.Name = "MNUShowEdHint";
            this.MNUShowEdHint.Size = new System.Drawing.Size(297, 22);
            this.MNUShowEdHint.Text = "&Показать подсказку";
            // 
            // MNUSep1
            // 
            this.MNUSep1.Name = "MNUSep1";
            this.MNUSep1.Size = new System.Drawing.Size(294, 6);
            // 
            // MNUFPSWizard
            // 
            this.MNUFPSWizard.Name = "MNUFPSWizard";
            this.MNUFPSWizard.Size = new System.Drawing.Size(297, 22);
            this.MNUFPSWizard.Text = "Ма&стер создания FPS-конфига...";
            // 
            // MNUReportBuilder
            // 
            this.MNUReportBuilder.Name = "MNUReportBuilder";
            this.MNUReportBuilder.Size = new System.Drawing.Size(297, 22);
            this.MNUReportBuilder.Text = "Создание отч&ёта для Техподдержки...";
            // 
            // MNUInstaller
            // 
            this.MNUInstaller.Name = "MNUInstaller";
            this.MNUInstaller.Size = new System.Drawing.Size(297, 22);
            this.MNUInstaller.Text = "Ус&тановщик спреев, демок и конфигов...";
            // 
            // MNUSep2
            // 
            this.MNUSep2.Name = "MNUSep2";
            this.MNUSep2.Size = new System.Drawing.Size(294, 6);
            // 
            // MNUExit
            // 
            this.MNUExit.Name = "MNUExit";
            this.MNUExit.Size = new System.Drawing.Size(297, 22);
            this.MNUExit.Text = "&Выход";
            // 
            // HelpMNU
            // 
            this.HelpMNU.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MNUHelp,
            this.MNUOpinion,
            this.MNUReportBug,
            this.MNUSteamGroup,
            this.MNUSep3,
            this.MNUGroup1,
            this.MNUGroup2,
            this.MNUGroup3,
            this.MNUSep4,
            this.MNUAbout});
            this.HelpMNU.Name = "HelpMNU";
            this.HelpMNU.Size = new System.Drawing.Size(62, 20);
            this.HelpMNU.Text = "Справка";
            // 
            // MNUHelp
            // 
            this.MNUHelp.Name = "MNUHelp";
            this.MNUHelp.Size = new System.Drawing.Size(293, 22);
            this.MNUHelp.Text = "Справоч&ная система Source Repair";
            // 
            // MNUOpinion
            // 
            this.MNUOpinion.Name = "MNUOpinion";
            this.MNUOpinion.Size = new System.Drawing.Size(293, 22);
            this.MNUOpinion.Text = "В&ысказать мнение о программе авторам";
            // 
            // MNUReportBug
            // 
            this.MNUReportBug.Name = "MNUReportBug";
            this.MNUReportBug.Size = new System.Drawing.Size(293, 22);
            this.MNUReportBug.Text = "Сообщить об ошибке в программе";
            // 
            // MNUSteamGroup
            // 
            this.MNUSteamGroup.Name = "MNUSteamGroup";
            this.MNUSteamGroup.Size = new System.Drawing.Size(293, 22);
            this.MNUSteamGroup.Text = "Оф&ициальная группа программы в Steam";
            // 
            // MNUSep3
            // 
            this.MNUSep3.Name = "MNUSep3";
            this.MNUSep3.Size = new System.Drawing.Size(290, 6);
            // 
            // MNUGroup1
            // 
            this.MNUGroup1.Name = "MNUGroup1";
            this.MNUGroup1.Size = new System.Drawing.Size(293, 22);
            this.MNUGroup1.Text = "&EasyCoding Team";
            // 
            // MNUGroup2
            // 
            this.MNUGroup2.Name = "MNUGroup2";
            this.MNUGroup2.Size = new System.Drawing.Size(293, 22);
            this.MNUGroup2.Text = "&Team-Fortress.ru";
            // 
            // MNUGroup3
            // 
            this.MNUGroup3.Name = "MNUGroup3";
            this.MNUGroup3.Size = new System.Drawing.Size(293, 22);
            this.MNUGroup3.Text = "T&F2World.ru";
            // 
            // MNUSep4
            // 
            this.MNUSep4.Name = "MNUSep4";
            this.MNUSep4.Size = new System.Drawing.Size(290, 6);
            // 
            // MNUAbout
            // 
            this.MNUAbout.Name = "MNUAbout";
            this.MNUAbout.Size = new System.Drawing.Size(293, 22);
            this.MNUAbout.Text = "&О программе";
            // 
            // LLoginSel
            // 
            this.LLoginSel.AutoSize = true;
            this.LLoginSel.Location = new System.Drawing.Point(75, 40);
            this.LLoginSel.Name = "LLoginSel";
            this.LLoginSel.Size = new System.Drawing.Size(158, 13);
            this.LLoginSel.TabIndex = 3;
            this.LLoginSel.Text = "Выберите Ваш логин в Steam:";
            // 
            // StatusBar
            // 
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SB_Info,
            this.SB_Status});
            this.StatusBar.Location = new System.Drawing.Point(0, 469);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(494, 22);
            this.StatusBar.TabIndex = 4;
            // 
            // SB_Info
            // 
            this.SB_Info.Name = "SB_Info";
            this.SB_Info.Size = new System.Drawing.Size(47, 17);
            this.SB_Info.Text = "Статус:";
            // 
            // SB_Status
            // 
            this.SB_Status.Name = "SB_Status";
            this.SB_Status.Size = new System.Drawing.Size(224, 17);
            this.SB_Status.Text = "Все системы работают в штатном режиме.";
            // 
            // frmMainW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 491);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.LLoginSel);
            this.Controls.Add(this.LoginSel);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.MainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.MainMenu;
            this.MaximizeBox = false;
            this.Name = "frmMainW";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Source Repair .NET (версия ####)";
            this.Load += new System.EventHandler(this.frmMainW_Load);
            this.MainTabControl.ResumeLayout(false);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage GraphicTweaker;
        private System.Windows.Forms.TabPage ConfigEditor;
        private System.Windows.Forms.TabPage ProblemSolver;
        private System.Windows.Forms.TabPage FPSCfgInstall;
        private System.Windows.Forms.TabPage RescueCentre;
        private System.Windows.Forms.ComboBox LoginSel;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem ToolsMNU;
        private System.Windows.Forms.ToolStripMenuItem MNUShowEdHint;
        private System.Windows.Forms.ToolStripSeparator MNUSep1;
        private System.Windows.Forms.ToolStripMenuItem MNUFPSWizard;
        private System.Windows.Forms.ToolStripMenuItem MNUReportBuilder;
        private System.Windows.Forms.ToolStripMenuItem MNUInstaller;
        private System.Windows.Forms.ToolStripSeparator MNUSep2;
        private System.Windows.Forms.ToolStripMenuItem MNUExit;
        private System.Windows.Forms.ToolStripMenuItem HelpMNU;
        private System.Windows.Forms.ToolStripMenuItem MNUHelp;
        private System.Windows.Forms.ToolStripMenuItem MNUOpinion;
        private System.Windows.Forms.ToolStripMenuItem MNUReportBug;
        private System.Windows.Forms.ToolStripMenuItem MNUSteamGroup;
        private System.Windows.Forms.ToolStripSeparator MNUSep3;
        private System.Windows.Forms.ToolStripMenuItem MNUGroup1;
        private System.Windows.Forms.ToolStripMenuItem MNUGroup2;
        private System.Windows.Forms.ToolStripMenuItem MNUGroup3;
        private System.Windows.Forms.ToolStripSeparator MNUSep4;
        private System.Windows.Forms.ToolStripMenuItem MNUAbout;
        private System.Windows.Forms.Label LLoginSel;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStripStatusLabel SB_Info;
        private System.Windows.Forms.ToolStripStatusLabel SB_Status;
    }
}

