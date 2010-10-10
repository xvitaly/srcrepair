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
            this.GT_SaveApply = new System.Windows.Forms.Button();
            this.GT_Maximum_Performance = new System.Windows.Forms.Button();
            this.GT_Maximum_Graphics = new System.Windows.Forms.Button();
            this.GT_LaunchOptions_Btn = new System.Windows.Forms.Button();
            this.L_GT_LaunchOptions = new System.Windows.Forms.Label();
            this.GT_LaunchOptions = new System.Windows.Forms.TextBox();
            this.L_GT_HDR = new System.Windows.Forms.Label();
            this.L_GT_DxMode = new System.Windows.Forms.Label();
            this.L_GT_MotionBlur = new System.Windows.Forms.Label();
            this.GT_HDR = new System.Windows.Forms.ComboBox();
            this.GT_DxMode = new System.Windows.Forms.ComboBox();
            this.GT_ResVert = new System.Windows.Forms.NumericUpDown();
            this.GT_ResHor = new System.Windows.Forms.NumericUpDown();
            this.GT_MotionBlur = new System.Windows.Forms.ComboBox();
            this.L_GT_VSync = new System.Windows.Forms.Label();
            this.L_GT_Filtering = new System.Windows.Forms.Label();
            this.L_GT_AntiAliasing = new System.Windows.Forms.Label();
            this.GT_VSync = new System.Windows.Forms.ComboBox();
            this.GT_Filtering = new System.Windows.Forms.ComboBox();
            this.GT_AntiAliasing = new System.Windows.Forms.ComboBox();
            this.L_GT_ColorCorrectionT = new System.Windows.Forms.Label();
            this.L_GT_ShadowQuality = new System.Windows.Forms.Label();
            this.L_GT_WaterQuality = new System.Windows.Forms.Label();
            this.GT_ColorCorrectionT = new System.Windows.Forms.ComboBox();
            this.GT_ShadowQuality = new System.Windows.Forms.ComboBox();
            this.GT_WaterQuality = new System.Windows.Forms.ComboBox();
            this.L_GT_ShaderQuality = new System.Windows.Forms.Label();
            this.L_GT_TextureQuality = new System.Windows.Forms.Label();
            this.L_GT_ModelQuality = new System.Windows.Forms.Label();
            this.GT_ShaderQuality = new System.Windows.Forms.ComboBox();
            this.GT_TextureQuality = new System.Windows.Forms.ComboBox();
            this.GT_ModelQuality = new System.Windows.Forms.ComboBox();
            this.L_GT_ScreenType = new System.Windows.Forms.Label();
            this.L_GT_ResVert = new System.Windows.Forms.Label();
            this.GT_ResVert_Btn = new System.Windows.Forms.Button();
            this.GT_ScreenType = new System.Windows.Forms.ComboBox();
            this.GT_ResHor_Btn = new System.Windows.Forms.Button();
            this.L_GT_ResHor = new System.Windows.Forms.Label();
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
            this.L_LoginSel = new System.Windows.Forms.Label();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.SB_Info = new System.Windows.Forms.ToolStripStatusLabel();
            this.SB_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainTabControl.SuspendLayout();
            this.GraphicTweaker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GT_ResVert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GT_ResHor)).BeginInit();
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
            this.MainTabControl.Size = new System.Drawing.Size(470, 405);
            this.MainTabControl.TabIndex = 0;
            // 
            // GraphicTweaker
            // 
            this.GraphicTweaker.Controls.Add(this.GT_SaveApply);
            this.GraphicTweaker.Controls.Add(this.GT_Maximum_Performance);
            this.GraphicTweaker.Controls.Add(this.GT_Maximum_Graphics);
            this.GraphicTweaker.Controls.Add(this.GT_LaunchOptions_Btn);
            this.GraphicTweaker.Controls.Add(this.L_GT_LaunchOptions);
            this.GraphicTweaker.Controls.Add(this.GT_LaunchOptions);
            this.GraphicTweaker.Controls.Add(this.L_GT_HDR);
            this.GraphicTweaker.Controls.Add(this.L_GT_DxMode);
            this.GraphicTweaker.Controls.Add(this.L_GT_MotionBlur);
            this.GraphicTweaker.Controls.Add(this.GT_HDR);
            this.GraphicTweaker.Controls.Add(this.GT_DxMode);
            this.GraphicTweaker.Controls.Add(this.GT_ResVert);
            this.GraphicTweaker.Controls.Add(this.GT_ResHor);
            this.GraphicTweaker.Controls.Add(this.GT_MotionBlur);
            this.GraphicTweaker.Controls.Add(this.L_GT_VSync);
            this.GraphicTweaker.Controls.Add(this.L_GT_Filtering);
            this.GraphicTweaker.Controls.Add(this.L_GT_AntiAliasing);
            this.GraphicTweaker.Controls.Add(this.GT_VSync);
            this.GraphicTweaker.Controls.Add(this.GT_Filtering);
            this.GraphicTweaker.Controls.Add(this.GT_AntiAliasing);
            this.GraphicTweaker.Controls.Add(this.L_GT_ColorCorrectionT);
            this.GraphicTweaker.Controls.Add(this.L_GT_ShadowQuality);
            this.GraphicTweaker.Controls.Add(this.L_GT_WaterQuality);
            this.GraphicTweaker.Controls.Add(this.GT_ColorCorrectionT);
            this.GraphicTweaker.Controls.Add(this.GT_ShadowQuality);
            this.GraphicTweaker.Controls.Add(this.GT_WaterQuality);
            this.GraphicTweaker.Controls.Add(this.L_GT_ShaderQuality);
            this.GraphicTweaker.Controls.Add(this.L_GT_TextureQuality);
            this.GraphicTweaker.Controls.Add(this.L_GT_ModelQuality);
            this.GraphicTweaker.Controls.Add(this.GT_ShaderQuality);
            this.GraphicTweaker.Controls.Add(this.GT_TextureQuality);
            this.GraphicTweaker.Controls.Add(this.GT_ModelQuality);
            this.GraphicTweaker.Controls.Add(this.L_GT_ScreenType);
            this.GraphicTweaker.Controls.Add(this.L_GT_ResVert);
            this.GraphicTweaker.Controls.Add(this.GT_ResVert_Btn);
            this.GraphicTweaker.Controls.Add(this.GT_ScreenType);
            this.GraphicTweaker.Controls.Add(this.GT_ResHor_Btn);
            this.GraphicTweaker.Controls.Add(this.L_GT_ResHor);
            this.GraphicTweaker.Location = new System.Drawing.Point(4, 22);
            this.GraphicTweaker.Name = "GraphicTweaker";
            this.GraphicTweaker.Padding = new System.Windows.Forms.Padding(3);
            this.GraphicTweaker.Size = new System.Drawing.Size(462, 379);
            this.GraphicTweaker.TabIndex = 0;
            this.GraphicTweaker.Text = "Графические настройки";
            this.GraphicTweaker.UseVisualStyleBackColor = true;
            // 
            // GT_SaveApply
            // 
            this.GT_SaveApply.Location = new System.Drawing.Point(148, 338);
            this.GT_SaveApply.Name = "GT_SaveApply";
            this.GT_SaveApply.Size = new System.Drawing.Size(155, 23);
            this.GT_SaveApply.TabIndex = 39;
            this.GT_SaveApply.Text = "Сохранить";
            this.GT_SaveApply.UseVisualStyleBackColor = true;
            // 
            // GT_Maximum_Performance
            // 
            this.GT_Maximum_Performance.Location = new System.Drawing.Point(235, 300);
            this.GT_Maximum_Performance.Name = "GT_Maximum_Performance";
            this.GT_Maximum_Performance.Size = new System.Drawing.Size(205, 23);
            this.GT_Maximum_Performance.TabIndex = 38;
            this.GT_Maximum_Performance.Text = "Максимальная производительность";
            this.GT_Maximum_Performance.UseVisualStyleBackColor = true;
            // 
            // GT_Maximum_Graphics
            // 
            this.GT_Maximum_Graphics.Location = new System.Drawing.Point(18, 300);
            this.GT_Maximum_Graphics.Name = "GT_Maximum_Graphics";
            this.GT_Maximum_Graphics.Size = new System.Drawing.Size(205, 23);
            this.GT_Maximum_Graphics.TabIndex = 37;
            this.GT_Maximum_Graphics.Text = "Максимум графики";
            this.GT_Maximum_Graphics.UseVisualStyleBackColor = true;
            // 
            // GT_LaunchOptions_Btn
            // 
            this.GT_LaunchOptions_Btn.Cursor = System.Windows.Forms.Cursors.Help;
            this.GT_LaunchOptions_Btn.Location = new System.Drawing.Point(418, 262);
            this.GT_LaunchOptions_Btn.Name = "GT_LaunchOptions_Btn";
            this.GT_LaunchOptions_Btn.Size = new System.Drawing.Size(22, 22);
            this.GT_LaunchOptions_Btn.TabIndex = 36;
            this.GT_LaunchOptions_Btn.Text = "?";
            this.GT_LaunchOptions_Btn.UseVisualStyleBackColor = true;
            // 
            // L_GT_LaunchOptions
            // 
            this.L_GT_LaunchOptions.AutoSize = true;
            this.L_GT_LaunchOptions.Location = new System.Drawing.Point(15, 267);
            this.L_GT_LaunchOptions.Name = "L_GT_LaunchOptions";
            this.L_GT_LaunchOptions.Size = new System.Drawing.Size(141, 13);
            this.L_GT_LaunchOptions.TabIndex = 35;
            this.L_GT_LaunchOptions.Text = "Параметры запуска игры:";
            // 
            // GT_LaunchOptions
            // 
            this.GT_LaunchOptions.Location = new System.Drawing.Point(162, 264);
            this.GT_LaunchOptions.Name = "GT_LaunchOptions";
            this.GT_LaunchOptions.Size = new System.Drawing.Size(250, 20);
            this.GT_LaunchOptions.TabIndex = 34;
            // 
            // L_GT_HDR
            // 
            this.L_GT_HDR.AutoSize = true;
            this.L_GT_HDR.Location = new System.Drawing.Point(303, 212);
            this.L_GT_HDR.Name = "L_GT_HDR";
            this.L_GT_HDR.Size = new System.Drawing.Size(31, 13);
            this.L_GT_HDR.TabIndex = 33;
            this.L_GT_HDR.Text = "HDR";
            // 
            // L_GT_DxMode
            // 
            this.L_GT_DxMode.AutoSize = true;
            this.L_GT_DxMode.Location = new System.Drawing.Point(159, 212);
            this.L_GT_DxMode.Name = "L_GT_DxMode";
            this.L_GT_DxMode.Size = new System.Drawing.Size(80, 13);
            this.L_GT_DxMode.TabIndex = 32;
            this.L_GT_DxMode.Text = "Режим DirectX";
            // 
            // L_GT_MotionBlur
            // 
            this.L_GT_MotionBlur.AutoSize = true;
            this.L_GT_MotionBlur.Location = new System.Drawing.Point(15, 212);
            this.L_GT_MotionBlur.Name = "L_GT_MotionBlur";
            this.L_GT_MotionBlur.Size = new System.Drawing.Size(112, 13);
            this.L_GT_MotionBlur.TabIndex = 31;
            this.L_GT_MotionBlur.Text = "Размытие движения";
            // 
            // GT_HDR
            // 
            this.GT_HDR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_HDR.FormattingEnabled = true;
            this.GT_HDR.Items.AddRange(new object[] {
            "Нет",
            "Bloom",
            "Полные"});
            this.GT_HDR.Location = new System.Drawing.Point(306, 228);
            this.GT_HDR.Name = "GT_HDR";
            this.GT_HDR.Size = new System.Drawing.Size(134, 21);
            this.GT_HDR.TabIndex = 30;
            // 
            // GT_DxMode
            // 
            this.GT_DxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_DxMode.FormattingEnabled = true;
            this.GT_DxMode.Items.AddRange(new object[] {
            "DirectX 8.0",
            "DirectX 8.1",
            "DirectX 9.0 Plain",
            "DirectX 9.0c Full"});
            this.GT_DxMode.Location = new System.Drawing.Point(162, 228);
            this.GT_DxMode.Name = "GT_DxMode";
            this.GT_DxMode.Size = new System.Drawing.Size(134, 21);
            this.GT_DxMode.TabIndex = 29;
            // 
            // GT_ResVert
            // 
            this.GT_ResVert.Location = new System.Drawing.Point(162, 33);
            this.GT_ResVert.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.GT_ResVert.Minimum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.GT_ResVert.Name = "GT_ResVert";
            this.GT_ResVert.Size = new System.Drawing.Size(113, 20);
            this.GT_ResVert.TabIndex = 28;
            this.GT_ResVert.Value = new decimal(new int[] {
            480,
            0,
            0,
            0});
            // 
            // GT_ResHor
            // 
            this.GT_ResHor.Location = new System.Drawing.Point(17, 33);
            this.GT_ResHor.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.GT_ResHor.Minimum = new decimal(new int[] {
            640,
            0,
            0,
            0});
            this.GT_ResHor.Name = "GT_ResHor";
            this.GT_ResHor.Size = new System.Drawing.Size(107, 20);
            this.GT_ResHor.TabIndex = 27;
            this.GT_ResHor.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
            // 
            // GT_MotionBlur
            // 
            this.GT_MotionBlur.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_MotionBlur.FormattingEnabled = true;
            this.GT_MotionBlur.Items.AddRange(new object[] {
            "Выключено",
            "Включено"});
            this.GT_MotionBlur.Location = new System.Drawing.Point(18, 228);
            this.GT_MotionBlur.Name = "GT_MotionBlur";
            this.GT_MotionBlur.Size = new System.Drawing.Size(134, 21);
            this.GT_MotionBlur.TabIndex = 26;
            // 
            // L_GT_VSync
            // 
            this.L_GT_VSync.AutoSize = true;
            this.L_GT_VSync.Location = new System.Drawing.Point(303, 163);
            this.L_GT_VSync.Name = "L_GT_VSync";
            this.L_GT_VSync.Size = new System.Drawing.Size(126, 13);
            this.L_GT_VSync.TabIndex = 25;
            this.L_GT_VSync.Text = "Вертик. синхронизация";
            // 
            // L_GT_Filtering
            // 
            this.L_GT_Filtering.AutoSize = true;
            this.L_GT_Filtering.Location = new System.Drawing.Point(159, 163);
            this.L_GT_Filtering.Name = "L_GT_Filtering";
            this.L_GT_Filtering.Size = new System.Drawing.Size(71, 13);
            this.L_GT_Filtering.TabIndex = 24;
            this.L_GT_Filtering.Text = "Фильтрация";
            // 
            // L_GT_AntiAliasing
            // 
            this.L_GT_AntiAliasing.AutoSize = true;
            this.L_GT_AntiAliasing.Location = new System.Drawing.Point(15, 163);
            this.L_GT_AntiAliasing.Name = "L_GT_AntiAliasing";
            this.L_GT_AntiAliasing.Size = new System.Drawing.Size(75, 13);
            this.L_GT_AntiAliasing.TabIndex = 23;
            this.L_GT_AntiAliasing.Text = "Сглаживание";
            // 
            // GT_VSync
            // 
            this.GT_VSync.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_VSync.FormattingEnabled = true;
            this.GT_VSync.Items.AddRange(new object[] {
            "Выключена",
            "Включена"});
            this.GT_VSync.Location = new System.Drawing.Point(306, 179);
            this.GT_VSync.Name = "GT_VSync";
            this.GT_VSync.Size = new System.Drawing.Size(134, 21);
            this.GT_VSync.TabIndex = 22;
            // 
            // GT_Filtering
            // 
            this.GT_Filtering.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_Filtering.FormattingEnabled = true;
            this.GT_Filtering.Items.AddRange(new object[] {
            "Билинейная",
            "Трилинейная",
            "Анизотропная 2X",
            "Анизотропная 4X",
            "Анизотропная 8X",
            "Анизотропная 16X"});
            this.GT_Filtering.Location = new System.Drawing.Point(162, 179);
            this.GT_Filtering.Name = "GT_Filtering";
            this.GT_Filtering.Size = new System.Drawing.Size(134, 21);
            this.GT_Filtering.TabIndex = 21;
            // 
            // GT_AntiAliasing
            // 
            this.GT_AntiAliasing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_AntiAliasing.FormattingEnabled = true;
            this.GT_AntiAliasing.Items.AddRange(new object[] {
            "Нет",
            "2X MSAA",
            "4X MSAA",
            "8X CSAA",
            "16X CSAA",
            "8X MSAA",
            "16XQ CSAA"});
            this.GT_AntiAliasing.Location = new System.Drawing.Point(18, 179);
            this.GT_AntiAliasing.Name = "GT_AntiAliasing";
            this.GT_AntiAliasing.Size = new System.Drawing.Size(134, 21);
            this.GT_AntiAliasing.TabIndex = 20;
            // 
            // L_GT_ColorCorrectionT
            // 
            this.L_GT_ColorCorrectionT.AutoSize = true;
            this.L_GT_ColorCorrectionT.Location = new System.Drawing.Point(303, 114);
            this.L_GT_ColorCorrectionT.Name = "L_GT_ColorCorrectionT";
            this.L_GT_ColorCorrectionT.Size = new System.Drawing.Size(94, 13);
            this.L_GT_ColorCorrectionT.TabIndex = 19;
            this.L_GT_ColorCorrectionT.Text = "Коррекция цвета";
            // 
            // L_GT_ShadowQuality
            // 
            this.L_GT_ShadowQuality.AutoSize = true;
            this.L_GT_ShadowQuality.Location = new System.Drawing.Point(159, 114);
            this.L_GT_ShadowQuality.Name = "L_GT_ShadowQuality";
            this.L_GT_ShadowQuality.Size = new System.Drawing.Size(101, 13);
            this.L_GT_ShadowQuality.TabIndex = 18;
            this.L_GT_ShadowQuality.Text = "Прорисовка теней";
            // 
            // L_GT_WaterQuality
            // 
            this.L_GT_WaterQuality.AutoSize = true;
            this.L_GT_WaterQuality.Location = new System.Drawing.Point(15, 114);
            this.L_GT_WaterQuality.Name = "L_GT_WaterQuality";
            this.L_GT_WaterQuality.Size = new System.Drawing.Size(100, 13);
            this.L_GT_WaterQuality.TabIndex = 17;
            this.L_GT_WaterQuality.Text = "Отражения в воде";
            // 
            // GT_ColorCorrectionT
            // 
            this.GT_ColorCorrectionT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_ColorCorrectionT.FormattingEnabled = true;
            this.GT_ColorCorrectionT.Items.AddRange(new object[] {
            "Выключена",
            "Включена"});
            this.GT_ColorCorrectionT.Location = new System.Drawing.Point(306, 130);
            this.GT_ColorCorrectionT.Name = "GT_ColorCorrectionT";
            this.GT_ColorCorrectionT.Size = new System.Drawing.Size(134, 21);
            this.GT_ColorCorrectionT.TabIndex = 16;
            // 
            // GT_ShadowQuality
            // 
            this.GT_ShadowQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_ShadowQuality.FormattingEnabled = true;
            this.GT_ShadowQuality.Items.AddRange(new object[] {
            "Низкая",
            "Высокая"});
            this.GT_ShadowQuality.Location = new System.Drawing.Point(162, 130);
            this.GT_ShadowQuality.Name = "GT_ShadowQuality";
            this.GT_ShadowQuality.Size = new System.Drawing.Size(134, 21);
            this.GT_ShadowQuality.TabIndex = 15;
            // 
            // GT_WaterQuality
            // 
            this.GT_WaterQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_WaterQuality.FormattingEnabled = true;
            this.GT_WaterQuality.Items.AddRange(new object[] {
            "Простые",
            "Отражать мир",
            "Отражать всё"});
            this.GT_WaterQuality.Location = new System.Drawing.Point(18, 130);
            this.GT_WaterQuality.Name = "GT_WaterQuality";
            this.GT_WaterQuality.Size = new System.Drawing.Size(134, 21);
            this.GT_WaterQuality.TabIndex = 14;
            // 
            // L_GT_ShaderQuality
            // 
            this.L_GT_ShaderQuality.AutoSize = true;
            this.L_GT_ShaderQuality.Location = new System.Drawing.Point(303, 65);
            this.L_GT_ShaderQuality.Name = "L_GT_ShaderQuality";
            this.L_GT_ShaderQuality.Size = new System.Drawing.Size(54, 13);
            this.L_GT_ShaderQuality.TabIndex = 13;
            this.L_GT_ShaderQuality.Text = "Шейдеры";
            // 
            // L_GT_TextureQuality
            // 
            this.L_GT_TextureQuality.AutoSize = true;
            this.L_GT_TextureQuality.Location = new System.Drawing.Point(159, 65);
            this.L_GT_TextureQuality.Name = "L_GT_TextureQuality";
            this.L_GT_TextureQuality.Size = new System.Drawing.Size(117, 13);
            this.L_GT_TextureQuality.TabIndex = 12;
            this.L_GT_TextureQuality.Text = "Детализация текстур";
            // 
            // L_GT_ModelQuality
            // 
            this.L_GT_ModelQuality.AutoSize = true;
            this.L_GT_ModelQuality.Location = new System.Drawing.Point(15, 65);
            this.L_GT_ModelQuality.Name = "L_GT_ModelQuality";
            this.L_GT_ModelQuality.Size = new System.Drawing.Size(122, 13);
            this.L_GT_ModelQuality.TabIndex = 11;
            this.L_GT_ModelQuality.Text = "Детализация моделей";
            // 
            // GT_ShaderQuality
            // 
            this.GT_ShaderQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_ShaderQuality.FormattingEnabled = true;
            this.GT_ShaderQuality.Items.AddRange(new object[] {
            "Низк.",
            "Высок."});
            this.GT_ShaderQuality.Location = new System.Drawing.Point(306, 81);
            this.GT_ShaderQuality.Name = "GT_ShaderQuality";
            this.GT_ShaderQuality.Size = new System.Drawing.Size(134, 21);
            this.GT_ShaderQuality.TabIndex = 10;
            // 
            // GT_TextureQuality
            // 
            this.GT_TextureQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_TextureQuality.FormattingEnabled = true;
            this.GT_TextureQuality.Items.AddRange(new object[] {
            "Низкая",
            "Средняя",
            "Высокая"});
            this.GT_TextureQuality.Location = new System.Drawing.Point(162, 81);
            this.GT_TextureQuality.Name = "GT_TextureQuality";
            this.GT_TextureQuality.Size = new System.Drawing.Size(134, 21);
            this.GT_TextureQuality.TabIndex = 9;
            // 
            // GT_ModelQuality
            // 
            this.GT_ModelQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_ModelQuality.FormattingEnabled = true;
            this.GT_ModelQuality.Items.AddRange(new object[] {
            "Низкая",
            "Средняя",
            "Высокая"});
            this.GT_ModelQuality.Location = new System.Drawing.Point(18, 81);
            this.GT_ModelQuality.Name = "GT_ModelQuality";
            this.GT_ModelQuality.Size = new System.Drawing.Size(134, 21);
            this.GT_ModelQuality.TabIndex = 8;
            // 
            // L_GT_ScreenType
            // 
            this.L_GT_ScreenType.AutoSize = true;
            this.L_GT_ScreenType.Location = new System.Drawing.Point(306, 17);
            this.L_GT_ScreenType.Name = "L_GT_ScreenType";
            this.L_GT_ScreenType.Size = new System.Drawing.Size(86, 13);
            this.L_GT_ScreenType.TabIndex = 7;
            this.L_GT_ScreenType.Text = "Режим запуска";
            // 
            // L_GT_ResVert
            // 
            this.L_GT_ResVert.AutoSize = true;
            this.L_GT_ResVert.Location = new System.Drawing.Point(158, 17);
            this.L_GT_ResVert.Name = "L_GT_ResVert";
            this.L_GT_ResVert.Size = new System.Drawing.Size(141, 13);
            this.L_GT_ResVert.TabIndex = 6;
            this.L_GT_ResVert.Text = "Разрешение по вертикали";
            // 
            // GT_ResVert_Btn
            // 
            this.GT_ResVert_Btn.Cursor = System.Windows.Forms.Cursors.Help;
            this.GT_ResVert_Btn.Location = new System.Drawing.Point(281, 31);
            this.GT_ResVert_Btn.Name = "GT_ResVert_Btn";
            this.GT_ResVert_Btn.Size = new System.Drawing.Size(22, 22);
            this.GT_ResVert_Btn.TabIndex = 5;
            this.GT_ResVert_Btn.Text = "?";
            this.GT_ResVert_Btn.UseVisualStyleBackColor = true;
            // 
            // GT_ScreenType
            // 
            this.GT_ScreenType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_ScreenType.FormattingEnabled = true;
            this.GT_ScreenType.Items.AddRange(new object[] {
            "Полноэкранный",
            "Оконный"});
            this.GT_ScreenType.Location = new System.Drawing.Point(309, 33);
            this.GT_ScreenType.Name = "GT_ScreenType";
            this.GT_ScreenType.Size = new System.Drawing.Size(131, 21);
            this.GT_ScreenType.TabIndex = 4;
            // 
            // GT_ResHor_Btn
            // 
            this.GT_ResHor_Btn.Cursor = System.Windows.Forms.Cursors.Help;
            this.GT_ResHor_Btn.Location = new System.Drawing.Point(130, 31);
            this.GT_ResHor_Btn.Name = "GT_ResHor_Btn";
            this.GT_ResHor_Btn.Size = new System.Drawing.Size(22, 22);
            this.GT_ResHor_Btn.TabIndex = 2;
            this.GT_ResHor_Btn.Text = "?";
            this.GT_ResHor_Btn.UseVisualStyleBackColor = true;
            // 
            // L_GT_ResHor
            // 
            this.L_GT_ResHor.AutoSize = true;
            this.L_GT_ResHor.Location = new System.Drawing.Point(15, 17);
            this.L_GT_ResHor.Name = "L_GT_ResHor";
            this.L_GT_ResHor.Size = new System.Drawing.Size(137, 13);
            this.L_GT_ResHor.TabIndex = 1;
            this.L_GT_ResHor.Text = "Разрешение по горизонт.";
            // 
            // ConfigEditor
            // 
            this.ConfigEditor.Location = new System.Drawing.Point(4, 22);
            this.ConfigEditor.Name = "ConfigEditor";
            this.ConfigEditor.Padding = new System.Windows.Forms.Padding(3);
            this.ConfigEditor.Size = new System.Drawing.Size(462, 379);
            this.ConfigEditor.TabIndex = 1;
            this.ConfigEditor.Text = "Редактор конфигов";
            this.ConfigEditor.UseVisualStyleBackColor = true;
            // 
            // ProblemSolver
            // 
            this.ProblemSolver.Location = new System.Drawing.Point(4, 22);
            this.ProblemSolver.Name = "ProblemSolver";
            this.ProblemSolver.Padding = new System.Windows.Forms.Padding(3);
            this.ProblemSolver.Size = new System.Drawing.Size(462, 379);
            this.ProblemSolver.TabIndex = 2;
            this.ProblemSolver.Text = "Устранение проблем";
            this.ProblemSolver.UseVisualStyleBackColor = true;
            // 
            // FPSCfgInstall
            // 
            this.FPSCfgInstall.Location = new System.Drawing.Point(4, 22);
            this.FPSCfgInstall.Name = "FPSCfgInstall";
            this.FPSCfgInstall.Padding = new System.Windows.Forms.Padding(3);
            this.FPSCfgInstall.Size = new System.Drawing.Size(462, 379);
            this.FPSCfgInstall.TabIndex = 3;
            this.FPSCfgInstall.Text = "FPS-конфиги";
            this.FPSCfgInstall.UseVisualStyleBackColor = true;
            // 
            // RescueCentre
            // 
            this.RescueCentre.Location = new System.Drawing.Point(4, 22);
            this.RescueCentre.Name = "RescueCentre";
            this.RescueCentre.Padding = new System.Windows.Forms.Padding(3);
            this.RescueCentre.Size = new System.Drawing.Size(462, 379);
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
            // L_LoginSel
            // 
            this.L_LoginSel.AutoSize = true;
            this.L_LoginSel.Location = new System.Drawing.Point(75, 40);
            this.L_LoginSel.Name = "L_LoginSel";
            this.L_LoginSel.Size = new System.Drawing.Size(158, 13);
            this.L_LoginSel.TabIndex = 3;
            this.L_LoginSel.Text = "Выберите Ваш логин в Steam:";
            // 
            // StatusBar
            // 
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SB_Info,
            this.SB_Status});
            this.StatusBar.Location = new System.Drawing.Point(0, 482);
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
            this.ClientSize = new System.Drawing.Size(494, 504);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.L_LoginSel);
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
            this.GraphicTweaker.ResumeLayout(false);
            this.GraphicTweaker.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GT_ResVert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GT_ResHor)).EndInit();
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
        private System.Windows.Forms.Label L_LoginSel;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStripStatusLabel SB_Info;
        private System.Windows.Forms.ToolStripStatusLabel SB_Status;
        private System.Windows.Forms.Button GT_ResHor_Btn;
        private System.Windows.Forms.Label L_GT_ResHor;
        private System.Windows.Forms.ComboBox GT_ScreenType;
        private System.Windows.Forms.Label L_GT_ScreenType;
        private System.Windows.Forms.Label L_GT_ResVert;
        private System.Windows.Forms.Button GT_ResVert_Btn;
        private System.Windows.Forms.ComboBox GT_ShaderQuality;
        private System.Windows.Forms.ComboBox GT_TextureQuality;
        private System.Windows.Forms.ComboBox GT_ModelQuality;
        private System.Windows.Forms.Label L_GT_ShaderQuality;
        private System.Windows.Forms.Label L_GT_TextureQuality;
        private System.Windows.Forms.Label L_GT_ModelQuality;
        private System.Windows.Forms.Label L_GT_ColorCorrectionT;
        private System.Windows.Forms.Label L_GT_ShadowQuality;
        private System.Windows.Forms.Label L_GT_WaterQuality;
        private System.Windows.Forms.ComboBox GT_ColorCorrectionT;
        private System.Windows.Forms.ComboBox GT_ShadowQuality;
        private System.Windows.Forms.ComboBox GT_WaterQuality;
        private System.Windows.Forms.Label L_GT_VSync;
        private System.Windows.Forms.Label L_GT_Filtering;
        private System.Windows.Forms.Label L_GT_AntiAliasing;
        private System.Windows.Forms.ComboBox GT_VSync;
        private System.Windows.Forms.ComboBox GT_Filtering;
        private System.Windows.Forms.ComboBox GT_AntiAliasing;
        private System.Windows.Forms.NumericUpDown GT_ResVert;
        private System.Windows.Forms.NumericUpDown GT_ResHor;
        private System.Windows.Forms.ComboBox GT_MotionBlur;
        private System.Windows.Forms.Button GT_Maximum_Performance;
        private System.Windows.Forms.Button GT_Maximum_Graphics;
        private System.Windows.Forms.Button GT_LaunchOptions_Btn;
        private System.Windows.Forms.Label L_GT_LaunchOptions;
        private System.Windows.Forms.TextBox GT_LaunchOptions;
        private System.Windows.Forms.Label L_GT_HDR;
        private System.Windows.Forms.Label L_GT_DxMode;
        private System.Windows.Forms.Label L_GT_MotionBlur;
        private System.Windows.Forms.ComboBox GT_HDR;
        private System.Windows.Forms.ComboBox GT_DxMode;
        private System.Windows.Forms.Button GT_SaveApply;
    }
}

