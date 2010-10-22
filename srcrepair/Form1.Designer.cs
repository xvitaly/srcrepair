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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainW));
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
            this.CE_Toolbar = new System.Windows.Forms.ToolStrip();
            this.CE_New = new System.Windows.Forms.ToolStripButton();
            this.CE_Open = new System.Windows.Forms.ToolStripButton();
            this.CE_Save = new System.Windows.Forms.ToolStripButton();
            this.CE_SaveAs = new System.Windows.Forms.ToolStripButton();
            this.CE_Print = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.CE_Cut = new System.Windows.Forms.ToolStripButton();
            this.CE_Copy = new System.Windows.Forms.ToolStripButton();
            this.CE_Paste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.CE_ShowHint = new System.Windows.Forms.ToolStripButton();
            this.CE_Editor = new System.Windows.Forms.DataGridView();
            this.ProblemSolver = new System.Windows.Forms.TabPage();
            this.PS_GB_Remover = new System.Windows.Forms.GroupBox();
            this.PS_AllowRemCtrls = new System.Windows.Forms.CheckBox();
            this.PS_RemGraphOpts = new System.Windows.Forms.Button();
            this.PS_RemDemos = new System.Windows.Forms.Button();
            this.PS_RemScreenShots = new System.Windows.Forms.Button();
            this.PS_RemNavFiles = new System.Windows.Forms.Button();
            this.PS_RemSoundCache = new System.Windows.Forms.Button();
            this.PS_RemGraphCache = new System.Windows.Forms.Button();
            this.PS_RemOldCfgs = new System.Windows.Forms.Button();
            this.PS_RemOldSpray = new System.Windows.Forms.Button();
            this.PS_RemDnlCache = new System.Windows.Forms.Button();
            this.PS_RemCustMaps = new System.Windows.Forms.Button();
            this.PS_GB_AdvFeat = new System.Windows.Forms.GroupBox();
            this.PS_ResetSettings = new System.Windows.Forms.Button();
            this.PS_GB_SInfo = new System.Windows.Forms.GroupBox();
            this.PS_WarningMsg = new System.Windows.Forms.Label();
            this.PS_PathDetector = new System.Windows.Forms.Label();
            this.PS_RSteamLogin = new System.Windows.Forms.Label();
            this.PS_RSteamPath = new System.Windows.Forms.Label();
            this.L_PS_PathDetector = new System.Windows.Forms.Label();
            this.L_PS_RSteamLogin = new System.Windows.Forms.Label();
            this.L_PS_RSteamPath = new System.Windows.Forms.Label();
            this.PS_GB_Solver = new System.Windows.Forms.GroupBox();
            this.PS_ExecuteNow = new System.Windows.Forms.Button();
            this.PS_SteamLang = new System.Windows.Forms.ComboBox();
            this.L_PS_SteamLang = new System.Windows.Forms.Label();
            this.PS_CleanRegistry = new System.Windows.Forms.CheckBox();
            this.PS_CleanBlobs = new System.Windows.Forms.CheckBox();
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
            this.AppSelector = new System.Windows.Forms.ComboBox();
            this.L_AppSelector = new System.Windows.Forms.Label();
            this.CE_CVName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CE_CVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainTabControl.SuspendLayout();
            this.GraphicTweaker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GT_ResVert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GT_ResHor)).BeginInit();
            this.ConfigEditor.SuspendLayout();
            this.CE_Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CE_Editor)).BeginInit();
            this.ProblemSolver.SuspendLayout();
            this.PS_GB_Remover.SuspendLayout();
            this.PS_GB_AdvFeat.SuspendLayout();
            this.PS_GB_SInfo.SuspendLayout();
            this.PS_GB_Solver.SuspendLayout();
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
            this.GT_LaunchOptions_Btn.TabStop = false;
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
            this.GT_ResVert_Btn.TabStop = false;
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
            this.GT_ResHor_Btn.TabStop = false;
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
            this.ConfigEditor.Controls.Add(this.CE_Toolbar);
            this.ConfigEditor.Controls.Add(this.CE_Editor);
            this.ConfigEditor.Location = new System.Drawing.Point(4, 22);
            this.ConfigEditor.Name = "ConfigEditor";
            this.ConfigEditor.Padding = new System.Windows.Forms.Padding(3);
            this.ConfigEditor.Size = new System.Drawing.Size(462, 379);
            this.ConfigEditor.TabIndex = 1;
            this.ConfigEditor.Text = "Редактор конфигов";
            this.ConfigEditor.UseVisualStyleBackColor = true;
            // 
            // CE_Toolbar
            // 
            this.CE_Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CE_New,
            this.CE_Open,
            this.CE_Save,
            this.CE_SaveAs,
            this.CE_Print,
            this.toolStripSeparator,
            this.CE_Cut,
            this.CE_Copy,
            this.CE_Paste,
            this.toolStripSeparator1,
            this.CE_ShowHint});
            this.CE_Toolbar.Location = new System.Drawing.Point(3, 3);
            this.CE_Toolbar.Name = "CE_Toolbar";
            this.CE_Toolbar.Size = new System.Drawing.Size(456, 25);
            this.CE_Toolbar.TabIndex = 1;
            this.CE_Toolbar.Text = "CE_Toolbar";
            // 
            // CE_New
            // 
            this.CE_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CE_New.Image = ((System.Drawing.Image)(resources.GetObject("CE_New.Image")));
            this.CE_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CE_New.Name = "CE_New";
            this.CE_New.Size = new System.Drawing.Size(23, 22);
            this.CE_New.Text = "Создать новый конфиг";
            // 
            // CE_Open
            // 
            this.CE_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CE_Open.Image = ((System.Drawing.Image)(resources.GetObject("CE_Open.Image")));
            this.CE_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CE_Open.Name = "CE_Open";
            this.CE_Open.Size = new System.Drawing.Size(23, 22);
            this.CE_Open.Text = "Открыть конфиг из файла";
            // 
            // CE_Save
            // 
            this.CE_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CE_Save.Image = ((System.Drawing.Image)(resources.GetObject("CE_Save.Image")));
            this.CE_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CE_Save.Name = "CE_Save";
            this.CE_Save.Size = new System.Drawing.Size(23, 22);
            this.CE_Save.Text = "Сохранить изменения в файл";
            // 
            // CE_SaveAs
            // 
            this.CE_SaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CE_SaveAs.Image = ((System.Drawing.Image)(resources.GetObject("CE_SaveAs.Image")));
            this.CE_SaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CE_SaveAs.Name = "CE_SaveAs";
            this.CE_SaveAs.Size = new System.Drawing.Size(23, 22);
            this.CE_SaveAs.Text = "Сохранить конфиг как...";
            // 
            // CE_Print
            // 
            this.CE_Print.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CE_Print.Image = ((System.Drawing.Image)(resources.GetObject("CE_Print.Image")));
            this.CE_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CE_Print.Name = "CE_Print";
            this.CE_Print.Size = new System.Drawing.Size(23, 22);
            this.CE_Print.Text = "Распечатать конфиг...";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // CE_Cut
            // 
            this.CE_Cut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CE_Cut.Image = ((System.Drawing.Image)(resources.GetObject("CE_Cut.Image")));
            this.CE_Cut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CE_Cut.Name = "CE_Cut";
            this.CE_Cut.Size = new System.Drawing.Size(23, 22);
            this.CE_Cut.Text = "Вырезать";
            // 
            // CE_Copy
            // 
            this.CE_Copy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CE_Copy.Image = ((System.Drawing.Image)(resources.GetObject("CE_Copy.Image")));
            this.CE_Copy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CE_Copy.Name = "CE_Copy";
            this.CE_Copy.Size = new System.Drawing.Size(23, 22);
            this.CE_Copy.Text = "Копировать";
            // 
            // CE_Paste
            // 
            this.CE_Paste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CE_Paste.Image = ((System.Drawing.Image)(resources.GetObject("CE_Paste.Image")));
            this.CE_Paste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CE_Paste.Name = "CE_Paste";
            this.CE_Paste.Size = new System.Drawing.Size(23, 22);
            this.CE_Paste.Text = "Вставить";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // CE_ShowHint
            // 
            this.CE_ShowHint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CE_ShowHint.Image = ((System.Drawing.Image)(resources.GetObject("CE_ShowHint.Image")));
            this.CE_ShowHint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CE_ShowHint.Name = "CE_ShowHint";
            this.CE_ShowHint.Size = new System.Drawing.Size(23, 22);
            this.CE_ShowHint.Text = "Подсказка (F1)";
            // 
            // CE_Editor
            // 
            this.CE_Editor.BackgroundColor = System.Drawing.SystemColors.Window;
            this.CE_Editor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CE_Editor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CE_CVName,
            this.CE_CVal});
            this.CE_Editor.Location = new System.Drawing.Point(6, 29);
            this.CE_Editor.Name = "CE_Editor";
            this.CE_Editor.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.CE_Editor.Size = new System.Drawing.Size(450, 344);
            this.CE_Editor.TabIndex = 0;
            // 
            // ProblemSolver
            // 
            this.ProblemSolver.Controls.Add(this.PS_GB_Remover);
            this.ProblemSolver.Controls.Add(this.PS_GB_AdvFeat);
            this.ProblemSolver.Controls.Add(this.PS_GB_SInfo);
            this.ProblemSolver.Controls.Add(this.PS_GB_Solver);
            this.ProblemSolver.Location = new System.Drawing.Point(4, 22);
            this.ProblemSolver.Name = "ProblemSolver";
            this.ProblemSolver.Padding = new System.Windows.Forms.Padding(3);
            this.ProblemSolver.Size = new System.Drawing.Size(462, 379);
            this.ProblemSolver.TabIndex = 2;
            this.ProblemSolver.Text = "Устранение проблем и очистка";
            this.ProblemSolver.UseVisualStyleBackColor = true;
            // 
            // PS_GB_Remover
            // 
            this.PS_GB_Remover.Controls.Add(this.PS_AllowRemCtrls);
            this.PS_GB_Remover.Controls.Add(this.PS_RemGraphOpts);
            this.PS_GB_Remover.Controls.Add(this.PS_RemDemos);
            this.PS_GB_Remover.Controls.Add(this.PS_RemScreenShots);
            this.PS_GB_Remover.Controls.Add(this.PS_RemNavFiles);
            this.PS_GB_Remover.Controls.Add(this.PS_RemSoundCache);
            this.PS_GB_Remover.Controls.Add(this.PS_RemGraphCache);
            this.PS_GB_Remover.Controls.Add(this.PS_RemOldCfgs);
            this.PS_GB_Remover.Controls.Add(this.PS_RemOldSpray);
            this.PS_GB_Remover.Controls.Add(this.PS_RemDnlCache);
            this.PS_GB_Remover.Controls.Add(this.PS_RemCustMaps);
            this.PS_GB_Remover.Location = new System.Drawing.Point(296, 6);
            this.PS_GB_Remover.Name = "PS_GB_Remover";
            this.PS_GB_Remover.Size = new System.Drawing.Size(160, 367);
            this.PS_GB_Remover.TabIndex = 3;
            this.PS_GB_Remover.TabStop = false;
            this.PS_GB_Remover.Text = "Удалить:";
            // 
            // PS_AllowRemCtrls
            // 
            this.PS_AllowRemCtrls.AutoSize = true;
            this.PS_AllowRemCtrls.Location = new System.Drawing.Point(15, 28);
            this.PS_AllowRemCtrls.Name = "PS_AllowRemCtrls";
            this.PS_AllowRemCtrls.Size = new System.Drawing.Size(132, 17);
            this.PS_AllowRemCtrls.TabIndex = 10;
            this.PS_AllowRemCtrls.Text = "Разрешить удаление";
            this.PS_AllowRemCtrls.UseVisualStyleBackColor = true;
            this.PS_AllowRemCtrls.CheckedChanged += new System.EventHandler(this.PS_AllowRemCtrls_CheckedChanged);
            // 
            // PS_RemGraphOpts
            // 
            this.PS_RemGraphOpts.Enabled = false;
            this.PS_RemGraphOpts.Location = new System.Drawing.Point(6, 328);
            this.PS_RemGraphOpts.Name = "PS_RemGraphOpts";
            this.PS_RemGraphOpts.Size = new System.Drawing.Size(148, 23);
            this.PS_RemGraphOpts.TabIndex = 9;
            this.PS_RemGraphOpts.Text = "Настройки графики";
            this.PS_RemGraphOpts.UseVisualStyleBackColor = true;
            // 
            // PS_RemDemos
            // 
            this.PS_RemDemos.Enabled = false;
            this.PS_RemDemos.Location = new System.Drawing.Point(6, 299);
            this.PS_RemDemos.Name = "PS_RemDemos";
            this.PS_RemDemos.Size = new System.Drawing.Size(148, 23);
            this.PS_RemDemos.TabIndex = 8;
            this.PS_RemDemos.Text = "Записанные демки";
            this.PS_RemDemos.UseVisualStyleBackColor = true;
            // 
            // PS_RemScreenShots
            // 
            this.PS_RemScreenShots.Enabled = false;
            this.PS_RemScreenShots.Location = new System.Drawing.Point(6, 270);
            this.PS_RemScreenShots.Name = "PS_RemScreenShots";
            this.PS_RemScreenShots.Size = new System.Drawing.Size(148, 23);
            this.PS_RemScreenShots.TabIndex = 7;
            this.PS_RemScreenShots.Text = "Все скриншоты";
            this.PS_RemScreenShots.UseVisualStyleBackColor = true;
            // 
            // PS_RemNavFiles
            // 
            this.PS_RemNavFiles.Enabled = false;
            this.PS_RemNavFiles.Location = new System.Drawing.Point(6, 240);
            this.PS_RemNavFiles.Name = "PS_RemNavFiles";
            this.PS_RemNavFiles.Size = new System.Drawing.Size(148, 23);
            this.PS_RemNavFiles.TabIndex = 6;
            this.PS_RemNavFiles.Text = "Навигацию ботов";
            this.PS_RemNavFiles.UseVisualStyleBackColor = true;
            // 
            // PS_RemSoundCache
            // 
            this.PS_RemSoundCache.Enabled = false;
            this.PS_RemSoundCache.Location = new System.Drawing.Point(6, 211);
            this.PS_RemSoundCache.Name = "PS_RemSoundCache";
            this.PS_RemSoundCache.Size = new System.Drawing.Size(148, 23);
            this.PS_RemSoundCache.TabIndex = 5;
            this.PS_RemSoundCache.Text = "Звуковой кэш";
            this.PS_RemSoundCache.UseVisualStyleBackColor = true;
            // 
            // PS_RemGraphCache
            // 
            this.PS_RemGraphCache.Enabled = false;
            this.PS_RemGraphCache.Location = new System.Drawing.Point(6, 182);
            this.PS_RemGraphCache.Name = "PS_RemGraphCache";
            this.PS_RemGraphCache.Size = new System.Drawing.Size(148, 23);
            this.PS_RemGraphCache.TabIndex = 4;
            this.PS_RemGraphCache.Text = "Графический кэш";
            this.PS_RemGraphCache.UseVisualStyleBackColor = true;
            // 
            // PS_RemOldCfgs
            // 
            this.PS_RemOldCfgs.Enabled = false;
            this.PS_RemOldCfgs.Location = new System.Drawing.Point(6, 153);
            this.PS_RemOldCfgs.Name = "PS_RemOldCfgs";
            this.PS_RemOldCfgs.Size = new System.Drawing.Size(148, 23);
            this.PS_RemOldCfgs.TabIndex = 3;
            this.PS_RemOldCfgs.Text = "Все конфиги";
            this.PS_RemOldCfgs.UseVisualStyleBackColor = true;
            // 
            // PS_RemOldSpray
            // 
            this.PS_RemOldSpray.Enabled = false;
            this.PS_RemOldSpray.Location = new System.Drawing.Point(6, 124);
            this.PS_RemOldSpray.Name = "PS_RemOldSpray";
            this.PS_RemOldSpray.Size = new System.Drawing.Size(148, 23);
            this.PS_RemOldSpray.TabIndex = 2;
            this.PS_RemOldSpray.Text = "Кэш спреев";
            this.PS_RemOldSpray.UseVisualStyleBackColor = true;
            // 
            // PS_RemDnlCache
            // 
            this.PS_RemDnlCache.Enabled = false;
            this.PS_RemDnlCache.Location = new System.Drawing.Point(6, 95);
            this.PS_RemDnlCache.Name = "PS_RemDnlCache";
            this.PS_RemDnlCache.Size = new System.Drawing.Size(148, 23);
            this.PS_RemDnlCache.TabIndex = 1;
            this.PS_RemDnlCache.Text = "Кэш загрузок";
            this.PS_RemDnlCache.UseVisualStyleBackColor = true;
            // 
            // PS_RemCustMaps
            // 
            this.PS_RemCustMaps.Enabled = false;
            this.PS_RemCustMaps.Location = new System.Drawing.Point(6, 66);
            this.PS_RemCustMaps.Name = "PS_RemCustMaps";
            this.PS_RemCustMaps.Size = new System.Drawing.Size(148, 23);
            this.PS_RemCustMaps.TabIndex = 0;
            this.PS_RemCustMaps.Text = "Кастомные карты";
            this.PS_RemCustMaps.UseVisualStyleBackColor = true;
            // 
            // PS_GB_AdvFeat
            // 
            this.PS_GB_AdvFeat.Controls.Add(this.PS_ResetSettings);
            this.PS_GB_AdvFeat.Location = new System.Drawing.Point(6, 315);
            this.PS_GB_AdvFeat.Name = "PS_GB_AdvFeat";
            this.PS_GB_AdvFeat.Size = new System.Drawing.Size(284, 58);
            this.PS_GB_AdvFeat.TabIndex = 2;
            this.PS_GB_AdvFeat.TabStop = false;
            this.PS_GB_AdvFeat.Text = "Специальные функции";
            // 
            // PS_ResetSettings
            // 
            this.PS_ResetSettings.Location = new System.Drawing.Point(11, 19);
            this.PS_ResetSettings.Name = "PS_ResetSettings";
            this.PS_ResetSettings.Size = new System.Drawing.Size(258, 23);
            this.PS_ResetSettings.TabIndex = 0;
            this.PS_ResetSettings.Text = "Восстановить настройки игры по умолчанию";
            this.PS_ResetSettings.UseVisualStyleBackColor = true;
            // 
            // PS_GB_SInfo
            // 
            this.PS_GB_SInfo.Controls.Add(this.PS_WarningMsg);
            this.PS_GB_SInfo.Controls.Add(this.PS_PathDetector);
            this.PS_GB_SInfo.Controls.Add(this.PS_RSteamLogin);
            this.PS_GB_SInfo.Controls.Add(this.PS_RSteamPath);
            this.PS_GB_SInfo.Controls.Add(this.L_PS_PathDetector);
            this.PS_GB_SInfo.Controls.Add(this.L_PS_RSteamLogin);
            this.PS_GB_SInfo.Controls.Add(this.L_PS_RSteamPath);
            this.PS_GB_SInfo.Location = new System.Drawing.Point(6, 147);
            this.PS_GB_SInfo.Name = "PS_GB_SInfo";
            this.PS_GB_SInfo.Size = new System.Drawing.Size(284, 162);
            this.PS_GB_SInfo.TabIndex = 1;
            this.PS_GB_SInfo.TabStop = false;
            this.PS_GB_SInfo.Text = "Информация о текущей установке Steam";
            // 
            // PS_WarningMsg
            // 
            this.PS_WarningMsg.Location = new System.Drawing.Point(12, 82);
            this.PS_WarningMsg.Name = "PS_WarningMsg";
            this.PS_WarningMsg.Size = new System.Drawing.Size(257, 77);
            this.PS_WarningMsg.TabIndex = 6;
            this.PS_WarningMsg.Text = "Steam установлен верно: логин и путь к каталогу (папке) Steam не содержат запрещё" +
                "нных символов: букв национальных алфавитов (русских, немецких, французских и т.д" +
                ".) и символов Юникода.";
            // 
            // PS_PathDetector
            // 
            this.PS_PathDetector.ForeColor = System.Drawing.Color.Green;
            this.PS_PathDetector.Location = new System.Drawing.Point(72, 60);
            this.PS_PathDetector.Name = "PS_PathDetector";
            this.PS_PathDetector.Size = new System.Drawing.Size(209, 13);
            this.PS_PathDetector.TabIndex = 5;
            this.PS_PathDetector.Text = "Не обнаружено non-ASCII символов";
            // 
            // PS_RSteamLogin
            // 
            this.PS_RSteamLogin.Location = new System.Drawing.Point(90, 38);
            this.PS_RSteamLogin.Name = "PS_RSteamLogin";
            this.PS_RSteamLogin.Size = new System.Drawing.Size(179, 13);
            this.PS_RSteamLogin.TabIndex = 4;
            // 
            // PS_RSteamPath
            // 
            this.PS_RSteamPath.Location = new System.Drawing.Point(90, 16);
            this.PS_RSteamPath.Name = "PS_RSteamPath";
            this.PS_RSteamPath.Size = new System.Drawing.Size(179, 13);
            this.PS_RSteamPath.TabIndex = 3;
            // 
            // L_PS_PathDetector
            // 
            this.L_PS_PathDetector.AutoSize = true;
            this.L_PS_PathDetector.Location = new System.Drawing.Point(12, 60);
            this.L_PS_PathDetector.Name = "L_PS_PathDetector";
            this.L_PS_PathDetector.Size = new System.Drawing.Size(64, 13);
            this.L_PS_PathDetector.TabIndex = 2;
            this.L_PS_PathDetector.Text = "Пров. пути:";
            // 
            // L_PS_RSteamLogin
            // 
            this.L_PS_RSteamLogin.AutoSize = true;
            this.L_PS_RSteamLogin.Location = new System.Drawing.Point(12, 38);
            this.L_PS_RSteamLogin.Name = "L_PS_RSteamLogin";
            this.L_PS_RSteamLogin.Size = new System.Drawing.Size(74, 13);
            this.L_PS_RSteamLogin.TabIndex = 1;
            this.L_PS_RSteamLogin.Text = "Логин Steam:";
            // 
            // L_PS_RSteamPath
            // 
            this.L_PS_RSteamPath.AutoSize = true;
            this.L_PS_RSteamPath.Location = new System.Drawing.Point(12, 16);
            this.L_PS_RSteamPath.Name = "L_PS_RSteamPath";
            this.L_PS_RSteamPath.Size = new System.Drawing.Size(76, 13);
            this.L_PS_RSteamPath.TabIndex = 0;
            this.L_PS_RSteamPath.Text = "Путь к Steam:";
            // 
            // PS_GB_Solver
            // 
            this.PS_GB_Solver.Controls.Add(this.PS_ExecuteNow);
            this.PS_GB_Solver.Controls.Add(this.PS_SteamLang);
            this.PS_GB_Solver.Controls.Add(this.L_PS_SteamLang);
            this.PS_GB_Solver.Controls.Add(this.PS_CleanRegistry);
            this.PS_GB_Solver.Controls.Add(this.PS_CleanBlobs);
            this.PS_GB_Solver.Location = new System.Drawing.Point(6, 6);
            this.PS_GB_Solver.Name = "PS_GB_Solver";
            this.PS_GB_Solver.Size = new System.Drawing.Size(284, 135);
            this.PS_GB_Solver.TabIndex = 0;
            this.PS_GB_Solver.TabStop = false;
            this.PS_GB_Solver.Text = "Устранение проблем Steam";
            // 
            // PS_ExecuteNow
            // 
            this.PS_ExecuteNow.Enabled = false;
            this.PS_ExecuteNow.Location = new System.Drawing.Point(93, 96);
            this.PS_ExecuteNow.Name = "PS_ExecuteNow";
            this.PS_ExecuteNow.Size = new System.Drawing.Size(101, 23);
            this.PS_ExecuteNow.TabIndex = 4;
            this.PS_ExecuteNow.Text = "Выполнить!";
            this.PS_ExecuteNow.UseVisualStyleBackColor = true;
            this.PS_ExecuteNow.Click += new System.EventHandler(this.PS_ExecuteNow_Click);
            // 
            // PS_SteamLang
            // 
            this.PS_SteamLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PS_SteamLang.Enabled = false;
            this.PS_SteamLang.FormattingEnabled = true;
            this.PS_SteamLang.Items.AddRange(new object[] {
            "Английский (English)",
            "Русский  (Russian)"});
            this.PS_SteamLang.Location = new System.Drawing.Point(140, 69);
            this.PS_SteamLang.Name = "PS_SteamLang";
            this.PS_SteamLang.Size = new System.Drawing.Size(129, 21);
            this.PS_SteamLang.TabIndex = 3;
            // 
            // L_PS_SteamLang
            // 
            this.L_PS_SteamLang.AutoSize = true;
            this.L_PS_SteamLang.Location = new System.Drawing.Point(12, 72);
            this.L_PS_SteamLang.Name = "L_PS_SteamLang";
            this.L_PS_SteamLang.Size = new System.Drawing.Size(122, 13);
            this.L_PS_SteamLang.TabIndex = 2;
            this.L_PS_SteamLang.Text = "Выберите язык Steam:";
            // 
            // PS_CleanRegistry
            // 
            this.PS_CleanRegistry.AutoSize = true;
            this.PS_CleanRegistry.Location = new System.Drawing.Point(15, 42);
            this.PS_CleanRegistry.Name = "PS_CleanRegistry";
            this.PS_CleanRegistry.Size = new System.Drawing.Size(266, 17);
            this.PS_CleanRegistry.TabIndex = 1;
            this.PS_CleanRegistry.Text = "Очистить записи Steam, хранящиеся в реестре";
            this.PS_CleanRegistry.UseVisualStyleBackColor = true;
            this.PS_CleanRegistry.CheckedChanged += new System.EventHandler(this.PS_CleanRegistry_CheckedChanged);
            // 
            // PS_CleanBlobs
            // 
            this.PS_CleanBlobs.AutoSize = true;
            this.PS_CleanBlobs.Location = new System.Drawing.Point(15, 19);
            this.PS_CleanBlobs.Name = "PS_CleanBlobs";
            this.PS_CleanBlobs.Size = new System.Drawing.Size(233, 17);
            this.PS_CleanBlobs.TabIndex = 0;
            this.PS_CleanBlobs.Text = "Очистить .blob-файлы из каталога Steam";
            this.PS_CleanBlobs.UseVisualStyleBackColor = true;
            this.PS_CleanBlobs.CheckedChanged += new System.EventHandler(this.PS_CleanBlobs_CheckedChanged);
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
            this.LoginSel.Enabled = false;
            this.LoginSel.FormattingEnabled = true;
            this.LoginSel.Location = new System.Drawing.Point(325, 35);
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
            this.MNUShowEdHint.Image = global::srcrepair.Properties.Resources.hint;
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
            this.MNUFPSWizard.Image = global::srcrepair.Properties.Resources.Wizard;
            this.MNUFPSWizard.Name = "MNUFPSWizard";
            this.MNUFPSWizard.Size = new System.Drawing.Size(297, 22);
            this.MNUFPSWizard.Text = "Ма&стер создания FPS-конфига...";
            // 
            // MNUReportBuilder
            // 
            this.MNUReportBuilder.Image = global::srcrepair.Properties.Resources.report;
            this.MNUReportBuilder.Name = "MNUReportBuilder";
            this.MNUReportBuilder.Size = new System.Drawing.Size(297, 22);
            this.MNUReportBuilder.Text = "Создание отч&ёта для Техподдержки...";
            // 
            // MNUInstaller
            // 
            this.MNUInstaller.Image = global::srcrepair.Properties.Resources.installer;
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
            this.MNUExit.Image = global::srcrepair.Properties.Resources.Exit;
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
            this.MNUHelp.Image = global::srcrepair.Properties.Resources.Help;
            this.MNUHelp.Name = "MNUHelp";
            this.MNUHelp.Size = new System.Drawing.Size(293, 22);
            this.MNUHelp.Text = "Справоч&ная система Source Repair";
            // 
            // MNUOpinion
            // 
            this.MNUOpinion.Image = global::srcrepair.Properties.Resources.Home;
            this.MNUOpinion.Name = "MNUOpinion";
            this.MNUOpinion.Size = new System.Drawing.Size(293, 22);
            this.MNUOpinion.Text = "В&ысказать мнение о программе авторам";
            // 
            // MNUReportBug
            // 
            this.MNUReportBug.Image = global::srcrepair.Properties.Resources.bug;
            this.MNUReportBug.Name = "MNUReportBug";
            this.MNUReportBug.Size = new System.Drawing.Size(293, 22);
            this.MNUReportBug.Text = "Сообщить об ошибке в программе";
            // 
            // MNUSteamGroup
            // 
            this.MNUSteamGroup.Image = global::srcrepair.Properties.Resources.steam;
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
            this.MNUGroup1.Image = global::srcrepair.Properties.Resources.EasyCoding;
            this.MNUGroup1.Name = "MNUGroup1";
            this.MNUGroup1.Size = new System.Drawing.Size(293, 22);
            this.MNUGroup1.Text = "&EasyCoding Team";
            // 
            // MNUGroup2
            // 
            this.MNUGroup2.Image = global::srcrepair.Properties.Resources.tfru;
            this.MNUGroup2.Name = "MNUGroup2";
            this.MNUGroup2.Size = new System.Drawing.Size(293, 22);
            this.MNUGroup2.Text = "&Team-Fortress.ru";
            // 
            // MNUGroup3
            // 
            this.MNUGroup3.Image = global::srcrepair.Properties.Resources.tf2world;
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
            this.MNUAbout.Image = global::srcrepair.Properties.Resources.Info;
            this.MNUAbout.Name = "MNUAbout";
            this.MNUAbout.Size = new System.Drawing.Size(293, 22);
            this.MNUAbout.Text = "&О программе";
            // 
            // L_LoginSel
            // 
            this.L_LoginSel.AutoSize = true;
            this.L_LoginSel.Location = new System.Drawing.Point(248, 38);
            this.L_LoginSel.Name = "L_LoginSel";
            this.L_LoginSel.Size = new System.Drawing.Size(74, 13);
            this.L_LoginSel.TabIndex = 3;
            this.L_LoginSel.Text = "Логин Steam:";
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
            // AppSelector
            // 
            this.AppSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AppSelector.FormattingEnabled = true;
            this.AppSelector.Items.AddRange(new object[] {
            "Team Fortress 2",
            "Counter-Strike: Source",
            "Garry\'s Mod",
            "Day of Defeat: Source",
            "Half-Life 2",
            "Half-Life 2 Episode One",
            "Half-Life 2 Episode Two"});
            this.AppSelector.Location = new System.Drawing.Point(93, 35);
            this.AppSelector.Name = "AppSelector";
            this.AppSelector.Size = new System.Drawing.Size(142, 21);
            this.AppSelector.TabIndex = 5;
            // 
            // L_AppSelector
            // 
            this.L_AppSelector.AutoSize = true;
            this.L_AppSelector.Location = new System.Drawing.Point(13, 38);
            this.L_AppSelector.Name = "L_AppSelector";
            this.L_AppSelector.Size = new System.Drawing.Size(74, 13);
            this.L_AppSelector.TabIndex = 6;
            this.L_AppSelector.Text = "Приложение:";
            // 
            // CE_CVName
            // 
            this.CE_CVName.HeaderText = "Переменная";
            this.CE_CVName.Name = "CE_CVName";
            this.CE_CVName.Width = 135;
            // 
            // CE_CVal
            // 
            this.CE_CVal.HeaderText = "Значение переменной";
            this.CE_CVal.Name = "CE_CVal";
            this.CE_CVal.Width = 255;
            // 
            // frmMainW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 504);
            this.Controls.Add(this.L_AppSelector);
            this.Controls.Add(this.AppSelector);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.L_LoginSel);
            this.Controls.Add(this.LoginSel);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.MainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            this.ConfigEditor.ResumeLayout(false);
            this.ConfigEditor.PerformLayout();
            this.CE_Toolbar.ResumeLayout(false);
            this.CE_Toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CE_Editor)).EndInit();
            this.ProblemSolver.ResumeLayout(false);
            this.PS_GB_Remover.ResumeLayout(false);
            this.PS_GB_Remover.PerformLayout();
            this.PS_GB_AdvFeat.ResumeLayout(false);
            this.PS_GB_SInfo.ResumeLayout(false);
            this.PS_GB_SInfo.PerformLayout();
            this.PS_GB_Solver.ResumeLayout(false);
            this.PS_GB_Solver.PerformLayout();
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
        private System.Windows.Forms.GroupBox PS_GB_Remover;
        private System.Windows.Forms.GroupBox PS_GB_AdvFeat;
        private System.Windows.Forms.GroupBox PS_GB_SInfo;
        private System.Windows.Forms.GroupBox PS_GB_Solver;
        private System.Windows.Forms.Button PS_ExecuteNow;
        private System.Windows.Forms.ComboBox PS_SteamLang;
        private System.Windows.Forms.Label L_PS_SteamLang;
        private System.Windows.Forms.CheckBox PS_CleanRegistry;
        private System.Windows.Forms.CheckBox PS_CleanBlobs;
        private System.Windows.Forms.Label PS_WarningMsg;
        private System.Windows.Forms.Label PS_PathDetector;
        private System.Windows.Forms.Label PS_RSteamLogin;
        private System.Windows.Forms.Label PS_RSteamPath;
        private System.Windows.Forms.Label L_PS_PathDetector;
        private System.Windows.Forms.Label L_PS_RSteamLogin;
        private System.Windows.Forms.Label L_PS_RSteamPath;
        private System.Windows.Forms.Button PS_ResetSettings;
        private System.Windows.Forms.CheckBox PS_AllowRemCtrls;
        private System.Windows.Forms.Button PS_RemGraphOpts;
        private System.Windows.Forms.Button PS_RemDemos;
        private System.Windows.Forms.Button PS_RemScreenShots;
        private System.Windows.Forms.Button PS_RemNavFiles;
        private System.Windows.Forms.Button PS_RemSoundCache;
        private System.Windows.Forms.Button PS_RemGraphCache;
        private System.Windows.Forms.Button PS_RemOldCfgs;
        private System.Windows.Forms.Button PS_RemOldSpray;
        private System.Windows.Forms.Button PS_RemDnlCache;
        private System.Windows.Forms.Button PS_RemCustMaps;
        private System.Windows.Forms.ComboBox AppSelector;
        private System.Windows.Forms.Label L_AppSelector;
        private System.Windows.Forms.DataGridView CE_Editor;
        private System.Windows.Forms.ToolStrip CE_Toolbar;
        private System.Windows.Forms.ToolStripButton CE_New;
        private System.Windows.Forms.ToolStripButton CE_Open;
        private System.Windows.Forms.ToolStripButton CE_Save;
        private System.Windows.Forms.ToolStripButton CE_Print;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton CE_Cut;
        private System.Windows.Forms.ToolStripButton CE_Copy;
        private System.Windows.Forms.ToolStripButton CE_Paste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton CE_ShowHint;
        private System.Windows.Forms.ToolStripButton CE_SaveAs;
        private System.Windows.Forms.DataGridViewTextBoxColumn CE_CVName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CE_CVal;
    }
}

