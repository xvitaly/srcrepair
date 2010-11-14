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
            this.GT_Warning = new System.Windows.Forms.PictureBox();
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
            this.CE_CVName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CE_CVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.FP_CreateBackUp = new System.Windows.Forms.CheckBox();
            this.FP_Uninstall = new System.Windows.Forms.Button();
            this.FP_Install = new System.Windows.Forms.Button();
            this.FP_GB_Desc = new System.Windows.Forms.GroupBox();
            this.FP_Description = new System.Windows.Forms.Label();
            this.FP_ConfigSel = new System.Windows.Forms.ComboBox();
            this.L_FP_ConfigSel = new System.Windows.Forms.Label();
            this.FP_TopLabel = new System.Windows.Forms.Label();
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
            this.SB_App = new System.Windows.Forms.ToolStripStatusLabel();
            this.AppSelector = new System.Windows.Forms.ComboBox();
            this.L_AppSelector = new System.Windows.Forms.Label();
            this.CE_OpenCfgDialog = new System.Windows.Forms.OpenFileDialog();
            this.CE_SaveCfgDialog = new System.Windows.Forms.SaveFileDialog();
            this.MainTabControl.SuspendLayout();
            this.GraphicTweaker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GT_Warning)).BeginInit();
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
            this.FPSCfgInstall.SuspendLayout();
            this.FP_GB_Desc.SuspendLayout();
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
            resources.ApplyResources(this.MainTabControl, "MainTabControl");
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            // 
            // GraphicTweaker
            // 
            this.GraphicTweaker.Controls.Add(this.GT_Warning);
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
            resources.ApplyResources(this.GraphicTweaker, "GraphicTweaker");
            this.GraphicTweaker.Name = "GraphicTweaker";
            this.GraphicTweaker.UseVisualStyleBackColor = true;
            // 
            // GT_Warning
            // 
            this.GT_Warning.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.GT_Warning, "GT_Warning");
            this.GT_Warning.Name = "GT_Warning";
            this.GT_Warning.TabStop = false;
            this.GT_Warning.Click += new System.EventHandler(this.GT_Warning_Click);
            // 
            // GT_SaveApply
            // 
            resources.ApplyResources(this.GT_SaveApply, "GT_SaveApply");
            this.GT_SaveApply.Name = "GT_SaveApply";
            this.GT_SaveApply.UseVisualStyleBackColor = true;
            this.GT_SaveApply.Click += new System.EventHandler(this.GT_SaveApply_Click);
            // 
            // GT_Maximum_Performance
            // 
            resources.ApplyResources(this.GT_Maximum_Performance, "GT_Maximum_Performance");
            this.GT_Maximum_Performance.Name = "GT_Maximum_Performance";
            this.GT_Maximum_Performance.UseVisualStyleBackColor = true;
            this.GT_Maximum_Performance.Click += new System.EventHandler(this.GT_Maximum_Performance_Click);
            // 
            // GT_Maximum_Graphics
            // 
            resources.ApplyResources(this.GT_Maximum_Graphics, "GT_Maximum_Graphics");
            this.GT_Maximum_Graphics.Name = "GT_Maximum_Graphics";
            this.GT_Maximum_Graphics.UseVisualStyleBackColor = true;
            this.GT_Maximum_Graphics.Click += new System.EventHandler(this.GT_Maximum_Graphics_Click);
            // 
            // GT_LaunchOptions_Btn
            // 
            this.GT_LaunchOptions_Btn.Cursor = System.Windows.Forms.Cursors.Help;
            resources.ApplyResources(this.GT_LaunchOptions_Btn, "GT_LaunchOptions_Btn");
            this.GT_LaunchOptions_Btn.Name = "GT_LaunchOptions_Btn";
            this.GT_LaunchOptions_Btn.TabStop = false;
            this.GT_LaunchOptions_Btn.UseVisualStyleBackColor = true;
            // 
            // L_GT_LaunchOptions
            // 
            resources.ApplyResources(this.L_GT_LaunchOptions, "L_GT_LaunchOptions");
            this.L_GT_LaunchOptions.Name = "L_GT_LaunchOptions";
            // 
            // GT_LaunchOptions
            // 
            resources.ApplyResources(this.GT_LaunchOptions, "GT_LaunchOptions");
            this.GT_LaunchOptions.Name = "GT_LaunchOptions";
            // 
            // L_GT_HDR
            // 
            resources.ApplyResources(this.L_GT_HDR, "L_GT_HDR");
            this.L_GT_HDR.Name = "L_GT_HDR";
            // 
            // L_GT_DxMode
            // 
            resources.ApplyResources(this.L_GT_DxMode, "L_GT_DxMode");
            this.L_GT_DxMode.Name = "L_GT_DxMode";
            // 
            // L_GT_MotionBlur
            // 
            resources.ApplyResources(this.L_GT_MotionBlur, "L_GT_MotionBlur");
            this.L_GT_MotionBlur.Name = "L_GT_MotionBlur";
            // 
            // GT_HDR
            // 
            this.GT_HDR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_HDR.FormattingEnabled = true;
            this.GT_HDR.Items.AddRange(new object[] {
            resources.GetString("GT_HDR.Items"),
            resources.GetString("GT_HDR.Items1"),
            resources.GetString("GT_HDR.Items2")});
            resources.ApplyResources(this.GT_HDR, "GT_HDR");
            this.GT_HDR.Name = "GT_HDR";
            // 
            // GT_DxMode
            // 
            this.GT_DxMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_DxMode.FormattingEnabled = true;
            this.GT_DxMode.Items.AddRange(new object[] {
            resources.GetString("GT_DxMode.Items"),
            resources.GetString("GT_DxMode.Items1"),
            resources.GetString("GT_DxMode.Items2"),
            resources.GetString("GT_DxMode.Items3")});
            resources.ApplyResources(this.GT_DxMode, "GT_DxMode");
            this.GT_DxMode.Name = "GT_DxMode";
            // 
            // GT_ResVert
            // 
            resources.ApplyResources(this.GT_ResVert, "GT_ResVert");
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
            this.GT_ResVert.Value = new decimal(new int[] {
            480,
            0,
            0,
            0});
            // 
            // GT_ResHor
            // 
            resources.ApplyResources(this.GT_ResHor, "GT_ResHor");
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
            resources.GetString("GT_MotionBlur.Items"),
            resources.GetString("GT_MotionBlur.Items1")});
            resources.ApplyResources(this.GT_MotionBlur, "GT_MotionBlur");
            this.GT_MotionBlur.Name = "GT_MotionBlur";
            // 
            // L_GT_VSync
            // 
            resources.ApplyResources(this.L_GT_VSync, "L_GT_VSync");
            this.L_GT_VSync.Name = "L_GT_VSync";
            // 
            // L_GT_Filtering
            // 
            resources.ApplyResources(this.L_GT_Filtering, "L_GT_Filtering");
            this.L_GT_Filtering.Name = "L_GT_Filtering";
            // 
            // L_GT_AntiAliasing
            // 
            resources.ApplyResources(this.L_GT_AntiAliasing, "L_GT_AntiAliasing");
            this.L_GT_AntiAliasing.Name = "L_GT_AntiAliasing";
            // 
            // GT_VSync
            // 
            this.GT_VSync.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_VSync.FormattingEnabled = true;
            this.GT_VSync.Items.AddRange(new object[] {
            resources.GetString("GT_VSync.Items"),
            resources.GetString("GT_VSync.Items1")});
            resources.ApplyResources(this.GT_VSync, "GT_VSync");
            this.GT_VSync.Name = "GT_VSync";
            // 
            // GT_Filtering
            // 
            this.GT_Filtering.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_Filtering.FormattingEnabled = true;
            this.GT_Filtering.Items.AddRange(new object[] {
            resources.GetString("GT_Filtering.Items"),
            resources.GetString("GT_Filtering.Items1"),
            resources.GetString("GT_Filtering.Items2"),
            resources.GetString("GT_Filtering.Items3"),
            resources.GetString("GT_Filtering.Items4"),
            resources.GetString("GT_Filtering.Items5")});
            resources.ApplyResources(this.GT_Filtering, "GT_Filtering");
            this.GT_Filtering.Name = "GT_Filtering";
            // 
            // GT_AntiAliasing
            // 
            this.GT_AntiAliasing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_AntiAliasing.FormattingEnabled = true;
            this.GT_AntiAliasing.Items.AddRange(new object[] {
            resources.GetString("GT_AntiAliasing.Items"),
            resources.GetString("GT_AntiAliasing.Items1"),
            resources.GetString("GT_AntiAliasing.Items2"),
            resources.GetString("GT_AntiAliasing.Items3"),
            resources.GetString("GT_AntiAliasing.Items4"),
            resources.GetString("GT_AntiAliasing.Items5"),
            resources.GetString("GT_AntiAliasing.Items6")});
            resources.ApplyResources(this.GT_AntiAliasing, "GT_AntiAliasing");
            this.GT_AntiAliasing.Name = "GT_AntiAliasing";
            // 
            // L_GT_ColorCorrectionT
            // 
            resources.ApplyResources(this.L_GT_ColorCorrectionT, "L_GT_ColorCorrectionT");
            this.L_GT_ColorCorrectionT.Name = "L_GT_ColorCorrectionT";
            // 
            // L_GT_ShadowQuality
            // 
            resources.ApplyResources(this.L_GT_ShadowQuality, "L_GT_ShadowQuality");
            this.L_GT_ShadowQuality.Name = "L_GT_ShadowQuality";
            // 
            // L_GT_WaterQuality
            // 
            resources.ApplyResources(this.L_GT_WaterQuality, "L_GT_WaterQuality");
            this.L_GT_WaterQuality.Name = "L_GT_WaterQuality";
            // 
            // GT_ColorCorrectionT
            // 
            this.GT_ColorCorrectionT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_ColorCorrectionT.FormattingEnabled = true;
            this.GT_ColorCorrectionT.Items.AddRange(new object[] {
            resources.GetString("GT_ColorCorrectionT.Items"),
            resources.GetString("GT_ColorCorrectionT.Items1")});
            resources.ApplyResources(this.GT_ColorCorrectionT, "GT_ColorCorrectionT");
            this.GT_ColorCorrectionT.Name = "GT_ColorCorrectionT";
            // 
            // GT_ShadowQuality
            // 
            this.GT_ShadowQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_ShadowQuality.FormattingEnabled = true;
            this.GT_ShadowQuality.Items.AddRange(new object[] {
            resources.GetString("GT_ShadowQuality.Items"),
            resources.GetString("GT_ShadowQuality.Items1")});
            resources.ApplyResources(this.GT_ShadowQuality, "GT_ShadowQuality");
            this.GT_ShadowQuality.Name = "GT_ShadowQuality";
            // 
            // GT_WaterQuality
            // 
            this.GT_WaterQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_WaterQuality.FormattingEnabled = true;
            this.GT_WaterQuality.Items.AddRange(new object[] {
            resources.GetString("GT_WaterQuality.Items"),
            resources.GetString("GT_WaterQuality.Items1"),
            resources.GetString("GT_WaterQuality.Items2")});
            resources.ApplyResources(this.GT_WaterQuality, "GT_WaterQuality");
            this.GT_WaterQuality.Name = "GT_WaterQuality";
            // 
            // L_GT_ShaderQuality
            // 
            resources.ApplyResources(this.L_GT_ShaderQuality, "L_GT_ShaderQuality");
            this.L_GT_ShaderQuality.Name = "L_GT_ShaderQuality";
            // 
            // L_GT_TextureQuality
            // 
            resources.ApplyResources(this.L_GT_TextureQuality, "L_GT_TextureQuality");
            this.L_GT_TextureQuality.Name = "L_GT_TextureQuality";
            // 
            // L_GT_ModelQuality
            // 
            resources.ApplyResources(this.L_GT_ModelQuality, "L_GT_ModelQuality");
            this.L_GT_ModelQuality.Name = "L_GT_ModelQuality";
            // 
            // GT_ShaderQuality
            // 
            this.GT_ShaderQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_ShaderQuality.FormattingEnabled = true;
            this.GT_ShaderQuality.Items.AddRange(new object[] {
            resources.GetString("GT_ShaderQuality.Items"),
            resources.GetString("GT_ShaderQuality.Items1")});
            resources.ApplyResources(this.GT_ShaderQuality, "GT_ShaderQuality");
            this.GT_ShaderQuality.Name = "GT_ShaderQuality";
            // 
            // GT_TextureQuality
            // 
            this.GT_TextureQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_TextureQuality.FormattingEnabled = true;
            this.GT_TextureQuality.Items.AddRange(new object[] {
            resources.GetString("GT_TextureQuality.Items"),
            resources.GetString("GT_TextureQuality.Items1"),
            resources.GetString("GT_TextureQuality.Items2")});
            resources.ApplyResources(this.GT_TextureQuality, "GT_TextureQuality");
            this.GT_TextureQuality.Name = "GT_TextureQuality";
            // 
            // GT_ModelQuality
            // 
            this.GT_ModelQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_ModelQuality.FormattingEnabled = true;
            this.GT_ModelQuality.Items.AddRange(new object[] {
            resources.GetString("GT_ModelQuality.Items"),
            resources.GetString("GT_ModelQuality.Items1"),
            resources.GetString("GT_ModelQuality.Items2")});
            resources.ApplyResources(this.GT_ModelQuality, "GT_ModelQuality");
            this.GT_ModelQuality.Name = "GT_ModelQuality";
            // 
            // L_GT_ScreenType
            // 
            resources.ApplyResources(this.L_GT_ScreenType, "L_GT_ScreenType");
            this.L_GT_ScreenType.Name = "L_GT_ScreenType";
            // 
            // L_GT_ResVert
            // 
            resources.ApplyResources(this.L_GT_ResVert, "L_GT_ResVert");
            this.L_GT_ResVert.Name = "L_GT_ResVert";
            // 
            // GT_ResVert_Btn
            // 
            this.GT_ResVert_Btn.Cursor = System.Windows.Forms.Cursors.Help;
            resources.ApplyResources(this.GT_ResVert_Btn, "GT_ResVert_Btn");
            this.GT_ResVert_Btn.Name = "GT_ResVert_Btn";
            this.GT_ResVert_Btn.TabStop = false;
            this.GT_ResVert_Btn.UseVisualStyleBackColor = true;
            // 
            // GT_ScreenType
            // 
            this.GT_ScreenType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GT_ScreenType.FormattingEnabled = true;
            this.GT_ScreenType.Items.AddRange(new object[] {
            resources.GetString("GT_ScreenType.Items"),
            resources.GetString("GT_ScreenType.Items1")});
            resources.ApplyResources(this.GT_ScreenType, "GT_ScreenType");
            this.GT_ScreenType.Name = "GT_ScreenType";
            // 
            // GT_ResHor_Btn
            // 
            this.GT_ResHor_Btn.Cursor = System.Windows.Forms.Cursors.Help;
            resources.ApplyResources(this.GT_ResHor_Btn, "GT_ResHor_Btn");
            this.GT_ResHor_Btn.Name = "GT_ResHor_Btn";
            this.GT_ResHor_Btn.TabStop = false;
            this.GT_ResHor_Btn.UseVisualStyleBackColor = true;
            // 
            // L_GT_ResHor
            // 
            resources.ApplyResources(this.L_GT_ResHor, "L_GT_ResHor");
            this.L_GT_ResHor.Name = "L_GT_ResHor";
            // 
            // ConfigEditor
            // 
            this.ConfigEditor.Controls.Add(this.CE_Toolbar);
            this.ConfigEditor.Controls.Add(this.CE_Editor);
            resources.ApplyResources(this.ConfigEditor, "ConfigEditor");
            this.ConfigEditor.Name = "ConfigEditor";
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
            resources.ApplyResources(this.CE_Toolbar, "CE_Toolbar");
            this.CE_Toolbar.Name = "CE_Toolbar";
            // 
            // CE_New
            // 
            this.CE_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.CE_New, "CE_New");
            this.CE_New.Name = "CE_New";
            this.CE_New.Click += new System.EventHandler(this.CE_New_Click);
            // 
            // CE_Open
            // 
            this.CE_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.CE_Open, "CE_Open");
            this.CE_Open.Name = "CE_Open";
            this.CE_Open.Click += new System.EventHandler(this.CE_Open_Click);
            // 
            // CE_Save
            // 
            this.CE_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.CE_Save, "CE_Save");
            this.CE_Save.Name = "CE_Save";
            this.CE_Save.Click += new System.EventHandler(this.CE_Save_Click);
            // 
            // CE_SaveAs
            // 
            this.CE_SaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.CE_SaveAs, "CE_SaveAs");
            this.CE_SaveAs.Name = "CE_SaveAs";
            this.CE_SaveAs.Click += new System.EventHandler(this.CE_SaveAs_Click);
            // 
            // CE_Print
            // 
            this.CE_Print.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.CE_Print, "CE_Print");
            this.CE_Print.Name = "CE_Print";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            resources.ApplyResources(this.toolStripSeparator, "toolStripSeparator");
            // 
            // CE_Cut
            // 
            this.CE_Cut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.CE_Cut, "CE_Cut");
            this.CE_Cut.Name = "CE_Cut";
            // 
            // CE_Copy
            // 
            this.CE_Copy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.CE_Copy, "CE_Copy");
            this.CE_Copy.Name = "CE_Copy";
            // 
            // CE_Paste
            // 
            this.CE_Paste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.CE_Paste, "CE_Paste");
            this.CE_Paste.Name = "CE_Paste";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // CE_ShowHint
            // 
            this.CE_ShowHint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.CE_ShowHint, "CE_ShowHint");
            this.CE_ShowHint.Name = "CE_ShowHint";
            // 
            // CE_Editor
            // 
            this.CE_Editor.BackgroundColor = System.Drawing.SystemColors.Window;
            this.CE_Editor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CE_Editor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CE_CVName,
            this.CE_CVal});
            resources.ApplyResources(this.CE_Editor, "CE_Editor");
            this.CE_Editor.Name = "CE_Editor";
            // 
            // CE_CVName
            // 
            resources.ApplyResources(this.CE_CVName, "CE_CVName");
            this.CE_CVName.Name = "CE_CVName";
            // 
            // CE_CVal
            // 
            resources.ApplyResources(this.CE_CVal, "CE_CVal");
            this.CE_CVal.Name = "CE_CVal";
            // 
            // ProblemSolver
            // 
            this.ProblemSolver.Controls.Add(this.PS_GB_Remover);
            this.ProblemSolver.Controls.Add(this.PS_GB_AdvFeat);
            this.ProblemSolver.Controls.Add(this.PS_GB_SInfo);
            this.ProblemSolver.Controls.Add(this.PS_GB_Solver);
            resources.ApplyResources(this.ProblemSolver, "ProblemSolver");
            this.ProblemSolver.Name = "ProblemSolver";
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
            resources.ApplyResources(this.PS_GB_Remover, "PS_GB_Remover");
            this.PS_GB_Remover.Name = "PS_GB_Remover";
            this.PS_GB_Remover.TabStop = false;
            // 
            // PS_AllowRemCtrls
            // 
            resources.ApplyResources(this.PS_AllowRemCtrls, "PS_AllowRemCtrls");
            this.PS_AllowRemCtrls.Name = "PS_AllowRemCtrls";
            this.PS_AllowRemCtrls.UseVisualStyleBackColor = true;
            this.PS_AllowRemCtrls.CheckedChanged += new System.EventHandler(this.PS_AllowRemCtrls_CheckedChanged);
            // 
            // PS_RemGraphOpts
            // 
            resources.ApplyResources(this.PS_RemGraphOpts, "PS_RemGraphOpts");
            this.PS_RemGraphOpts.Name = "PS_RemGraphOpts";
            this.PS_RemGraphOpts.UseVisualStyleBackColor = true;
            // 
            // PS_RemDemos
            // 
            resources.ApplyResources(this.PS_RemDemos, "PS_RemDemos");
            this.PS_RemDemos.Name = "PS_RemDemos";
            this.PS_RemDemos.UseVisualStyleBackColor = true;
            // 
            // PS_RemScreenShots
            // 
            resources.ApplyResources(this.PS_RemScreenShots, "PS_RemScreenShots");
            this.PS_RemScreenShots.Name = "PS_RemScreenShots";
            this.PS_RemScreenShots.UseVisualStyleBackColor = true;
            // 
            // PS_RemNavFiles
            // 
            resources.ApplyResources(this.PS_RemNavFiles, "PS_RemNavFiles");
            this.PS_RemNavFiles.Name = "PS_RemNavFiles";
            this.PS_RemNavFiles.UseVisualStyleBackColor = true;
            // 
            // PS_RemSoundCache
            // 
            resources.ApplyResources(this.PS_RemSoundCache, "PS_RemSoundCache");
            this.PS_RemSoundCache.Name = "PS_RemSoundCache";
            this.PS_RemSoundCache.UseVisualStyleBackColor = true;
            // 
            // PS_RemGraphCache
            // 
            resources.ApplyResources(this.PS_RemGraphCache, "PS_RemGraphCache");
            this.PS_RemGraphCache.Name = "PS_RemGraphCache";
            this.PS_RemGraphCache.UseVisualStyleBackColor = true;
            // 
            // PS_RemOldCfgs
            // 
            resources.ApplyResources(this.PS_RemOldCfgs, "PS_RemOldCfgs");
            this.PS_RemOldCfgs.Name = "PS_RemOldCfgs";
            this.PS_RemOldCfgs.UseVisualStyleBackColor = true;
            // 
            // PS_RemOldSpray
            // 
            resources.ApplyResources(this.PS_RemOldSpray, "PS_RemOldSpray");
            this.PS_RemOldSpray.Name = "PS_RemOldSpray";
            this.PS_RemOldSpray.UseVisualStyleBackColor = true;
            // 
            // PS_RemDnlCache
            // 
            resources.ApplyResources(this.PS_RemDnlCache, "PS_RemDnlCache");
            this.PS_RemDnlCache.Name = "PS_RemDnlCache";
            this.PS_RemDnlCache.UseVisualStyleBackColor = true;
            // 
            // PS_RemCustMaps
            // 
            resources.ApplyResources(this.PS_RemCustMaps, "PS_RemCustMaps");
            this.PS_RemCustMaps.Name = "PS_RemCustMaps";
            this.PS_RemCustMaps.UseVisualStyleBackColor = true;
            // 
            // PS_GB_AdvFeat
            // 
            this.PS_GB_AdvFeat.Controls.Add(this.PS_ResetSettings);
            resources.ApplyResources(this.PS_GB_AdvFeat, "PS_GB_AdvFeat");
            this.PS_GB_AdvFeat.Name = "PS_GB_AdvFeat";
            this.PS_GB_AdvFeat.TabStop = false;
            // 
            // PS_ResetSettings
            // 
            resources.ApplyResources(this.PS_ResetSettings, "PS_ResetSettings");
            this.PS_ResetSettings.Name = "PS_ResetSettings";
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
            resources.ApplyResources(this.PS_GB_SInfo, "PS_GB_SInfo");
            this.PS_GB_SInfo.Name = "PS_GB_SInfo";
            this.PS_GB_SInfo.TabStop = false;
            // 
            // PS_WarningMsg
            // 
            resources.ApplyResources(this.PS_WarningMsg, "PS_WarningMsg");
            this.PS_WarningMsg.Name = "PS_WarningMsg";
            // 
            // PS_PathDetector
            // 
            this.PS_PathDetector.ForeColor = System.Drawing.Color.Green;
            resources.ApplyResources(this.PS_PathDetector, "PS_PathDetector");
            this.PS_PathDetector.Name = "PS_PathDetector";
            // 
            // PS_RSteamLogin
            // 
            resources.ApplyResources(this.PS_RSteamLogin, "PS_RSteamLogin");
            this.PS_RSteamLogin.Name = "PS_RSteamLogin";
            // 
            // PS_RSteamPath
            // 
            resources.ApplyResources(this.PS_RSteamPath, "PS_RSteamPath");
            this.PS_RSteamPath.Name = "PS_RSteamPath";
            // 
            // L_PS_PathDetector
            // 
            resources.ApplyResources(this.L_PS_PathDetector, "L_PS_PathDetector");
            this.L_PS_PathDetector.Name = "L_PS_PathDetector";
            // 
            // L_PS_RSteamLogin
            // 
            resources.ApplyResources(this.L_PS_RSteamLogin, "L_PS_RSteamLogin");
            this.L_PS_RSteamLogin.Name = "L_PS_RSteamLogin";
            // 
            // L_PS_RSteamPath
            // 
            resources.ApplyResources(this.L_PS_RSteamPath, "L_PS_RSteamPath");
            this.L_PS_RSteamPath.Name = "L_PS_RSteamPath";
            // 
            // PS_GB_Solver
            // 
            this.PS_GB_Solver.Controls.Add(this.PS_ExecuteNow);
            this.PS_GB_Solver.Controls.Add(this.PS_SteamLang);
            this.PS_GB_Solver.Controls.Add(this.L_PS_SteamLang);
            this.PS_GB_Solver.Controls.Add(this.PS_CleanRegistry);
            this.PS_GB_Solver.Controls.Add(this.PS_CleanBlobs);
            resources.ApplyResources(this.PS_GB_Solver, "PS_GB_Solver");
            this.PS_GB_Solver.Name = "PS_GB_Solver";
            this.PS_GB_Solver.TabStop = false;
            // 
            // PS_ExecuteNow
            // 
            resources.ApplyResources(this.PS_ExecuteNow, "PS_ExecuteNow");
            this.PS_ExecuteNow.Name = "PS_ExecuteNow";
            this.PS_ExecuteNow.UseVisualStyleBackColor = true;
            this.PS_ExecuteNow.Click += new System.EventHandler(this.PS_ExecuteNow_Click);
            // 
            // PS_SteamLang
            // 
            this.PS_SteamLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.PS_SteamLang, "PS_SteamLang");
            this.PS_SteamLang.FormattingEnabled = true;
            this.PS_SteamLang.Items.AddRange(new object[] {
            resources.GetString("PS_SteamLang.Items"),
            resources.GetString("PS_SteamLang.Items1")});
            this.PS_SteamLang.Name = "PS_SteamLang";
            // 
            // L_PS_SteamLang
            // 
            resources.ApplyResources(this.L_PS_SteamLang, "L_PS_SteamLang");
            this.L_PS_SteamLang.Name = "L_PS_SteamLang";
            // 
            // PS_CleanRegistry
            // 
            resources.ApplyResources(this.PS_CleanRegistry, "PS_CleanRegistry");
            this.PS_CleanRegistry.Name = "PS_CleanRegistry";
            this.PS_CleanRegistry.UseVisualStyleBackColor = true;
            this.PS_CleanRegistry.CheckedChanged += new System.EventHandler(this.PS_CleanRegistry_CheckedChanged);
            // 
            // PS_CleanBlobs
            // 
            resources.ApplyResources(this.PS_CleanBlobs, "PS_CleanBlobs");
            this.PS_CleanBlobs.Name = "PS_CleanBlobs";
            this.PS_CleanBlobs.UseVisualStyleBackColor = true;
            this.PS_CleanBlobs.CheckedChanged += new System.EventHandler(this.PS_CleanBlobs_CheckedChanged);
            // 
            // FPSCfgInstall
            // 
            this.FPSCfgInstall.Controls.Add(this.FP_CreateBackUp);
            this.FPSCfgInstall.Controls.Add(this.FP_Uninstall);
            this.FPSCfgInstall.Controls.Add(this.FP_Install);
            this.FPSCfgInstall.Controls.Add(this.FP_GB_Desc);
            this.FPSCfgInstall.Controls.Add(this.FP_ConfigSel);
            this.FPSCfgInstall.Controls.Add(this.L_FP_ConfigSel);
            this.FPSCfgInstall.Controls.Add(this.FP_TopLabel);
            resources.ApplyResources(this.FPSCfgInstall, "FPSCfgInstall");
            this.FPSCfgInstall.Name = "FPSCfgInstall";
            this.FPSCfgInstall.UseVisualStyleBackColor = true;
            // 
            // FP_CreateBackUp
            // 
            resources.ApplyResources(this.FP_CreateBackUp, "FP_CreateBackUp");
            this.FP_CreateBackUp.Name = "FP_CreateBackUp";
            this.FP_CreateBackUp.UseVisualStyleBackColor = true;
            // 
            // FP_Uninstall
            // 
            resources.ApplyResources(this.FP_Uninstall, "FP_Uninstall");
            this.FP_Uninstall.Name = "FP_Uninstall";
            this.FP_Uninstall.UseVisualStyleBackColor = true;
            this.FP_Uninstall.Click += new System.EventHandler(this.FP_Uninstall_Click);
            // 
            // FP_Install
            // 
            resources.ApplyResources(this.FP_Install, "FP_Install");
            this.FP_Install.Name = "FP_Install";
            this.FP_Install.UseVisualStyleBackColor = true;
            this.FP_Install.Click += new System.EventHandler(this.FP_Install_Click);
            // 
            // FP_GB_Desc
            // 
            this.FP_GB_Desc.Controls.Add(this.FP_Description);
            resources.ApplyResources(this.FP_GB_Desc, "FP_GB_Desc");
            this.FP_GB_Desc.Name = "FP_GB_Desc";
            this.FP_GB_Desc.TabStop = false;
            // 
            // FP_Description
            // 
            resources.ApplyResources(this.FP_Description, "FP_Description");
            this.FP_Description.Name = "FP_Description";
            // 
            // FP_ConfigSel
            // 
            this.FP_ConfigSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FP_ConfigSel.FormattingEnabled = true;
            resources.ApplyResources(this.FP_ConfigSel, "FP_ConfigSel");
            this.FP_ConfigSel.Name = "FP_ConfigSel";
            this.FP_ConfigSel.SelectedIndexChanged += new System.EventHandler(this.FP_ConfigSel_SelectedIndexChanged);
            // 
            // L_FP_ConfigSel
            // 
            resources.ApplyResources(this.L_FP_ConfigSel, "L_FP_ConfigSel");
            this.L_FP_ConfigSel.Name = "L_FP_ConfigSel";
            // 
            // FP_TopLabel
            // 
            resources.ApplyResources(this.FP_TopLabel, "FP_TopLabel");
            this.FP_TopLabel.Name = "FP_TopLabel";
            // 
            // RescueCentre
            // 
            resources.ApplyResources(this.RescueCentre, "RescueCentre");
            this.RescueCentre.Name = "RescueCentre";
            this.RescueCentre.UseVisualStyleBackColor = true;
            // 
            // LoginSel
            // 
            this.LoginSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LoginSel.FormattingEnabled = true;
            resources.ApplyResources(this.LoginSel, "LoginSel");
            this.LoginSel.Name = "LoginSel";
            this.LoginSel.SelectedIndexChanged += new System.EventHandler(this.LoginSel_SelectedIndexChanged);
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolsMNU,
            this.HelpMNU});
            resources.ApplyResources(this.MainMenu, "MainMenu");
            this.MainMenu.Name = "MainMenu";
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
            resources.ApplyResources(this.ToolsMNU, "ToolsMNU");
            // 
            // MNUShowEdHint
            // 
            this.MNUShowEdHint.Image = global::srcrepair.Properties.Resources.hint;
            this.MNUShowEdHint.Name = "MNUShowEdHint";
            resources.ApplyResources(this.MNUShowEdHint, "MNUShowEdHint");
            // 
            // MNUSep1
            // 
            this.MNUSep1.Name = "MNUSep1";
            resources.ApplyResources(this.MNUSep1, "MNUSep1");
            // 
            // MNUFPSWizard
            // 
            this.MNUFPSWizard.Image = global::srcrepair.Properties.Resources.Wizard;
            this.MNUFPSWizard.Name = "MNUFPSWizard";
            resources.ApplyResources(this.MNUFPSWizard, "MNUFPSWizard");
            // 
            // MNUReportBuilder
            // 
            this.MNUReportBuilder.Image = global::srcrepair.Properties.Resources.report;
            this.MNUReportBuilder.Name = "MNUReportBuilder";
            resources.ApplyResources(this.MNUReportBuilder, "MNUReportBuilder");
            // 
            // MNUInstaller
            // 
            this.MNUInstaller.Image = global::srcrepair.Properties.Resources.installer;
            this.MNUInstaller.Name = "MNUInstaller";
            resources.ApplyResources(this.MNUInstaller, "MNUInstaller");
            // 
            // MNUSep2
            // 
            this.MNUSep2.Name = "MNUSep2";
            resources.ApplyResources(this.MNUSep2, "MNUSep2");
            // 
            // MNUExit
            // 
            this.MNUExit.Image = global::srcrepair.Properties.Resources.Exit;
            this.MNUExit.Name = "MNUExit";
            resources.ApplyResources(this.MNUExit, "MNUExit");
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
            resources.ApplyResources(this.HelpMNU, "HelpMNU");
            // 
            // MNUHelp
            // 
            this.MNUHelp.Image = global::srcrepair.Properties.Resources.Help;
            this.MNUHelp.Name = "MNUHelp";
            resources.ApplyResources(this.MNUHelp, "MNUHelp");
            // 
            // MNUOpinion
            // 
            this.MNUOpinion.Image = global::srcrepair.Properties.Resources.Home;
            this.MNUOpinion.Name = "MNUOpinion";
            resources.ApplyResources(this.MNUOpinion, "MNUOpinion");
            // 
            // MNUReportBug
            // 
            this.MNUReportBug.Image = global::srcrepair.Properties.Resources.bug;
            this.MNUReportBug.Name = "MNUReportBug";
            resources.ApplyResources(this.MNUReportBug, "MNUReportBug");
            // 
            // MNUSteamGroup
            // 
            this.MNUSteamGroup.Image = global::srcrepair.Properties.Resources.steam;
            this.MNUSteamGroup.Name = "MNUSteamGroup";
            resources.ApplyResources(this.MNUSteamGroup, "MNUSteamGroup");
            // 
            // MNUSep3
            // 
            this.MNUSep3.Name = "MNUSep3";
            resources.ApplyResources(this.MNUSep3, "MNUSep3");
            // 
            // MNUGroup1
            // 
            this.MNUGroup1.Image = global::srcrepair.Properties.Resources.EasyCoding;
            this.MNUGroup1.Name = "MNUGroup1";
            resources.ApplyResources(this.MNUGroup1, "MNUGroup1");
            // 
            // MNUGroup2
            // 
            this.MNUGroup2.Image = global::srcrepair.Properties.Resources.tfru;
            this.MNUGroup2.Name = "MNUGroup2";
            resources.ApplyResources(this.MNUGroup2, "MNUGroup2");
            // 
            // MNUGroup3
            // 
            this.MNUGroup3.Image = global::srcrepair.Properties.Resources.tf2world;
            this.MNUGroup3.Name = "MNUGroup3";
            resources.ApplyResources(this.MNUGroup3, "MNUGroup3");
            // 
            // MNUSep4
            // 
            this.MNUSep4.Name = "MNUSep4";
            resources.ApplyResources(this.MNUSep4, "MNUSep4");
            // 
            // MNUAbout
            // 
            this.MNUAbout.Image = global::srcrepair.Properties.Resources.Info;
            this.MNUAbout.Name = "MNUAbout";
            resources.ApplyResources(this.MNUAbout, "MNUAbout");
            // 
            // L_LoginSel
            // 
            resources.ApplyResources(this.L_LoginSel, "L_LoginSel");
            this.L_LoginSel.Name = "L_LoginSel";
            // 
            // StatusBar
            // 
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SB_Info,
            this.SB_Status,
            this.SB_App});
            resources.ApplyResources(this.StatusBar, "StatusBar");
            this.StatusBar.Name = "StatusBar";
            // 
            // SB_Info
            // 
            this.SB_Info.Name = "SB_Info";
            resources.ApplyResources(this.SB_Info, "SB_Info");
            // 
            // SB_Status
            // 
            resources.ApplyResources(this.SB_Status, "SB_Status");
            this.SB_Status.Name = "SB_Status";
            // 
            // SB_App
            // 
            resources.ApplyResources(this.SB_App, "SB_App");
            this.SB_App.Name = "SB_App";
            // 
            // AppSelector
            // 
            this.AppSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.AppSelector, "AppSelector");
            this.AppSelector.FormattingEnabled = true;
            this.AppSelector.Items.AddRange(new object[] {
            resources.GetString("AppSelector.Items"),
            resources.GetString("AppSelector.Items1"),
            resources.GetString("AppSelector.Items2"),
            resources.GetString("AppSelector.Items3"),
            resources.GetString("AppSelector.Items4"),
            resources.GetString("AppSelector.Items5"),
            resources.GetString("AppSelector.Items6")});
            this.AppSelector.Name = "AppSelector";
            this.AppSelector.SelectedIndexChanged += new System.EventHandler(this.AppSelector_SelectedIndexChanged);
            // 
            // L_AppSelector
            // 
            resources.ApplyResources(this.L_AppSelector, "L_AppSelector");
            this.L_AppSelector.Name = "L_AppSelector";
            // 
            // CE_OpenCfgDialog
            // 
            this.CE_OpenCfgDialog.DefaultExt = "cfg";
            resources.ApplyResources(this.CE_OpenCfgDialog, "CE_OpenCfgDialog");
            // 
            // CE_SaveCfgDialog
            // 
            this.CE_SaveCfgDialog.CreatePrompt = true;
            this.CE_SaveCfgDialog.DefaultExt = "cfg";
            resources.ApplyResources(this.CE_SaveCfgDialog, "CE_SaveCfgDialog");
            // 
            // frmMainW
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.L_AppSelector);
            this.Controls.Add(this.AppSelector);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.L_LoginSel);
            this.Controls.Add(this.LoginSel);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.MainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.MainMenu;
            this.MaximizeBox = false;
            this.Name = "frmMainW";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMainW_FormClosing);
            this.Load += new System.EventHandler(this.frmMainW_Load);
            this.MainTabControl.ResumeLayout(false);
            this.GraphicTweaker.ResumeLayout(false);
            this.GraphicTweaker.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GT_Warning)).EndInit();
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
            this.FPSCfgInstall.ResumeLayout(false);
            this.FPSCfgInstall.PerformLayout();
            this.FP_GB_Desc.ResumeLayout(false);
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
        private System.Windows.Forms.Button FP_Uninstall;
        private System.Windows.Forms.Button FP_Install;
        private System.Windows.Forms.GroupBox FP_GB_Desc;
        private System.Windows.Forms.Label FP_Description;
        private System.Windows.Forms.ComboBox FP_ConfigSel;
        private System.Windows.Forms.Label L_FP_ConfigSel;
        private System.Windows.Forms.Label FP_TopLabel;
        private System.Windows.Forms.CheckBox FP_CreateBackUp;
        private System.Windows.Forms.PictureBox GT_Warning;
        private System.Windows.Forms.ToolStripStatusLabel SB_App;
        private System.Windows.Forms.OpenFileDialog CE_OpenCfgDialog;
        private System.Windows.Forms.SaveFileDialog CE_SaveCfgDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn CE_CVName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CE_CVal;
    }
}

