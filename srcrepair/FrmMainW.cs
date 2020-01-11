/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2020 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2020 EasyCoding Team.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of main form.
    /// </summary>
    public partial class FrmMainW : Form
    {
        /// <summary>
        /// FrmMainW class constructor.
        /// </summary>
        public FrmMainW()
        {
            // Initializing controls...
            InitializeComponent();

            // Importing settings from previous versions (if any)...
            ImportSettings();
        }

        #region HiDPI hacks

        /// <summary>
        /// Scales controls on current form with some additional hacks applied.
        /// </summary>
        /// <param name="ScalingFactor">Scaling factor.</param>
        /// <param name="Bounds">Bounds of control.</param>
        protected override void ScaleControl(SizeF ScalingFactor, BoundsSpecified Bounds)
        {
            base.ScaleControl(ScalingFactor, Bounds);
            if (!DpiManager.CompareFloats(Math.Max(ScalingFactor.Width, ScalingFactor.Height), 1.0f))
            {
                DpiManager.ScaleColumnsInControl(CE_Editor, ScalingFactor);
                DpiManager.ScaleColumnsInControl(BU_LVTable, ScalingFactor);
                DpiManager.ScaleColumnsInControl(StatusBar, ScalingFactor);
            }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets loaded in Config Editor file name.
        /// </summary>
        private string CFGFileName { get; set; }

        /// <summary>
        /// Gets or sets instance of CurrentApp class.
        /// </summary>
        private CurrentApp App { get; set; }

        #endregion

        #region Internal Fields

        /// <summary>
        /// Logger instance for FrmMainW class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ResourceManager instance for managing Cvar descriptions in Config Editor.
        /// </summary>
        private readonly ResourceManager CvarFetcher = new ResourceManager(Properties.Resources.CE_CVResDf, typeof(FrmMainW).Assembly);

        #endregion

        #region Internal Methods

        /// <summary>
        /// Imports settings from previous versions of application.
        /// </summary>
        private void ImportSettings()
        {
            try
            {
                if (Properties.Settings.Default.CallUpgrade)
                {
                    Properties.Settings.Default.Upgrade();
                    Properties.Settings.Default.CallUpgrade = false;
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExSettingsLoad);
            }
        }
        
        /// <summary>
        /// Saves contents of Config Editor to a text file. Used by Save
        /// and Save As operations.
        /// </summary>
        /// <param name="Path">Full path to config file.</param>
        private void WriteTableToFileNow(string Path)
        {
            using (StreamWriter CFile = new StreamWriter(Path))
            {
                for (int i = 0; i < CE_Editor.Rows.Count; i++)
                {
                    CFile.WriteLine("{0} {1}", CE_Editor.Rows[i].Cells[0].Value, CE_Editor.Rows[i].Cells[1].Value);
                }
            }
        }

        /// <summary>
        /// Seaching for installed games and adds them to ComboBox control
        /// of main window.
        /// </summary>
        private void DetectInstalledGames()
        {
            try
            {
                // Reading game database...
                App.SourceGames = new GameManager(App, Properties.Settings.Default.HideUnsupportedGames);

                // Clearing all existing items...
                AppSelector.Items.Clear();

                // Adding found games to selector...
                AppSelector.Items.AddRange(App.SourceGames.InstalledGameNames.ToArray());

            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
            }
        }

        /// <summary>
        /// Saves video settings of Type 1 game.
        /// </summary>
        private void WriteType1VideoSettings()
        {
            // Creating Type1Video instance for operating with video settings...
            Type1Video Video = new Type1Video(App.SourceGames[AppSelector.Text].ConfDir, false)
            {
                // Changing default properties by user selection...
                ScreenWidth = (int)GT_ResHor.Value,
                ScreenHeight = (int)GT_ResVert.Value,
                DisplayMode = GT_ScreenType.SelectedIndex,
                ModelQuality = GT_ModelQuality.SelectedIndex,
                TextureQuality = GT_TextureQuality.SelectedIndex,
                ShaderQuality = GT_ShaderQuality.SelectedIndex,
                ReflectionsQuality = GT_WaterQuality.SelectedIndex,
                ShadowQuality = GT_ShadowQuality.SelectedIndex,
                ColorCorrection = GT_ColorCorrectionT.SelectedIndex,
                AntiAliasing = GT_AntiAliasing.SelectedIndex,
                FilteringMode = GT_Filtering.SelectedIndex,
                VSync = GT_VSync.SelectedIndex,
                MotionBlur = GT_MotionBlur.SelectedIndex,
                DirectXMode = GT_DxMode.SelectedIndex,
                HDRType = GT_HDR.SelectedIndex
            };

            // Saving settings to disk or registry...
            Video.WriteSettings();
        }

        /// <summary>
        /// Saves video settings of Type 2 game.
        /// </summary>
        private void WriteType2VideoSettings()
        {
            // Creating Type2Video instance for operating with video settings...
            Type2Video Video = new Type2Video(App.SourceGames[AppSelector.Text].GetActualVideoFile(), false)
            {
                // Changing default properties by user selection...
                ScreenWidth = (int)GT_NCF_HorRes.Value,
                ScreenHeight = (int)GT_NCF_VertRes.Value,
                ScreenRatio = GT_NCF_Ratio.SelectedIndex,
                ScreenGamma = GT_NCF_Brightness.Text,
                ShadowQuality = GT_NCF_Shadows.SelectedIndex,
                MotionBlur = GT_NCF_MBlur.SelectedIndex,
                ScreenMode = GT_NCF_DispMode.SelectedIndex,
                AntiAliasing = GT_NCF_AntiAlias.SelectedIndex,
                FilteringMode = GT_NCF_Filtering.SelectedIndex,
                VSync = GT_NCF_VSync.SelectedIndex,
                RenderingMode = GT_NCF_Multicore.SelectedIndex,
                ShaderEffects = GT_NCF_ShaderE.SelectedIndex,
                Effects = GT_NCF_EffectD.SelectedIndex,
                MemoryPool = GT_NCF_MemPool.SelectedIndex,
                ModelQuality = GT_NCF_Quality.SelectedIndex
            };

            // Saving settings to disk or registry...
            Video.WriteSettings();
        }

        /// <summary>
        /// Fetches video settings of Type 1 game.
        /// </summary>
        private void ReadType1VideoSettings()
        {
            try
            {
                // Creating Type1Video instance for operating with video settings...
                Type1Video Video = new Type1Video(App.SourceGames[AppSelector.Text].ConfDir, true);

                // Reading settings and setting them on form...
                GT_ResHor.Value = Video.ScreenWidth;
                GT_ResVert.Value = Video.ScreenHeight;
                GT_ScreenType.SelectedIndex = Video.DisplayMode;
                GT_ModelQuality.SelectedIndex = Video.ModelQuality;
                GT_TextureQuality.SelectedIndex = Video.TextureQuality;
                GT_ShaderQuality.SelectedIndex = Video.ShaderQuality;
                GT_WaterQuality.SelectedIndex = Video.ReflectionsQuality;
                GT_ShadowQuality.SelectedIndex = Video.ShadowQuality;
                GT_ColorCorrectionT.SelectedIndex = Video.ColorCorrection;
                GT_AntiAliasing.SelectedIndex = Video.AntiAliasing;
                GT_Filtering.SelectedIndex = Video.FilteringMode;
                GT_VSync.SelectedIndex = Video.VSync;
                GT_MotionBlur.SelectedIndex = Video.MotionBlur;
                GT_DxMode.SelectedIndex = Video.DirectXMode;
                GT_HDR.SelectedIndex = Video.HDRType;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.GT_RegOpenErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExT1LoadFail);
            }
        }

        /// <summary>
        /// Fetches video settings of Type 2 game.
        /// </summary>
        private void ReadType2VideoSettings()
        {
            try
            {
                // Retrieving actual file with video settings...
                string VFileName = App.SourceGames[AppSelector.Text].GetActualVideoFile();

                // Checking if file with video settings exists...
                if (File.Exists(VFileName))
                {
                    // Creating Type2Video instance for operating with video settings...
                    Type2Video Video = new Type2Video(VFileName, true);

                    // Reading settings and setting them on form...
                    GT_NCF_HorRes.Value = Video.ScreenWidth;
                    GT_NCF_VertRes.Value = Video.ScreenHeight;
                    GT_NCF_Ratio.SelectedIndex = Video.ScreenRatio;
                    GT_NCF_Brightness.Text = Video.ScreenGamma;
                    GT_NCF_Shadows.SelectedIndex = Video.ShadowQuality;
                    GT_NCF_MBlur.SelectedIndex = Video.MotionBlur;
                    GT_NCF_DispMode.SelectedIndex = Video.ScreenMode;
                    GT_NCF_AntiAlias.SelectedIndex = Video.AntiAliasing;
                    GT_NCF_Filtering.SelectedIndex = Video.FilteringMode;
                    GT_NCF_VSync.SelectedIndex = Video.VSync;
                    GT_NCF_Multicore.SelectedIndex = Video.RenderingMode;
                    GT_NCF_ShaderE.SelectedIndex = Video.ShaderEffects;
                    GT_NCF_EffectD.SelectedIndex = Video.Effects;
                    GT_NCF_MemPool.SelectedIndex = Video.MemoryPool;
                    GT_NCF_Quality.SelectedIndex = Video.ModelQuality;
                }
                else
                {
                    Logger.Warn(String.Format(AppStrings.AppVideoDbNotFound, App.SourceGames[AppSelector.Text].FullAppName, VFileName));
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.GT_NCFLoadFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExT2LoadFail);
            }
        }

        /// <summary>
        /// Checks for application updates.
        /// </summary>
        /// <returns>Check result.</returns>
        private bool AutoUpdateCheck()
        {
            UpdateManager UpMan = new UpdateManager(App.FullAppPath, App.UserAgent);
            return UpMan.CheckAppUpdate();
        }

        /// <summary>
        /// Loads config file to Config Editor.
        /// </summary>
        /// <param name="ConfFileName">Full path to config file.</param>
        private void ReadConfigFromFile(string ConfFileName)
        {
            // Creating some variables...
            string ImpStr, CVarName, CVarContent;

            // Checking if config file exists...
            if (File.Exists(ConfFileName))
            {
                // Setting config file name (without full path)...
                CFGFileName = ConfFileName;

                // Checking if currently opened file is not config.cfg. If so, show warning to user.
                if (Path.GetFileName(CFGFileName) == "config.cfg")
                {
                    MessageBox.Show(AppStrings.CE_RestConfigOpenWarn, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Clearing editor...
                CE_Editor.Rows.Clear();

                try
                {
                    // Loading config file...
                    using (StreamReader ConfigFile = new StreamReader(ConfFileName, Encoding.Default))
                    {
                        // Reading config file to the end...
                        while (ConfigFile.Peek() >= 0)
                        {
                            // Clearing string from special chars...
                            ImpStr = StringsManager.CleanString(ConfigFile.ReadLine());

                            // Checking if source string is not empty...
                            if (!String.IsNullOrEmpty(ImpStr))
                            {
                                // Checking if source string is not a commentary...
                                if (ImpStr[0] != '/')
                                {
                                    // Checking for value in source string...
                                    if (ImpStr.IndexOf(" ") != -1)
                                    {
                                        // Extracting variable...
                                        CVarName = ImpStr.Substring(0, ImpStr.IndexOf(" "));
                                        ImpStr = ImpStr.Remove(0, ImpStr.IndexOf(" ") + 1);

                                        // Extracting value (excluding commentaries if exists)...
                                        CVarContent = ImpStr.IndexOf("//") >= 1 ? ImpStr.Substring(0, ImpStr.IndexOf("//") - 1) : ImpStr;

                                        // Adding to table Cvar and its value...
                                        CE_Editor.Rows.Add(CVarName, CVarContent);
                                    }
                                    else
                                    {
                                        // Adding to table only Cvar with empty value...
                                        CE_Editor.Rows.Add(ImpStr, String.Empty);
                                    }
                                }
                            }
                        }
                    }

                    // Updating status bar text...
                    UpdateStatusBar();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.CE_ExceptionDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExCfgEdLoad);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.CE_OpenFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Gets user-friendly names for backup files.
        /// </summary>
        /// <param name="FileName">Backup file name.</param>
        /// <returns>Returns tuple with backup type and its friendly name.</returns>
        private Tuple<string, string> GenUserFriendlyBackupDesc(FileInfo FileName)
        {
            string ConfRow, ConfType;
            switch (FileName.Name.Substring(0, FileName.Name.LastIndexOf('_')))
            {
                case "Container":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_Bud, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Cont;
                    break;
                case "Config":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_Config, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Cfg;
                    break;
                case "VoiceBan":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_VChat, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_DB;
                    break;
                case "VideoCfg":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_GRGame, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Video;
                    break;
                case "VideoAutoCfg":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_GameAuto, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Video;
                    break;
                case "Game_Options":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_GRGame, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Reg;
                    break;
                case "Source_Options":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_SRCAll, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Reg;
                    break;
                case "Steam_BackUp":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_SteamAll, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Reg;
                    break;
                case "Game_AutoBackUp":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_GameAuto, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Reg;
                    break;
                default:
                    ConfRow = Path.GetFileNameWithoutExtension(FileName.Name);
                    ConfType = AppStrings.BU_BType_Unkn;
                    break;
            }
            return Tuple.Create(ConfType, ConfRow);
        }

        /// <summary>
        /// Resets controls of Type 1 video settings on Graphic Tweaker
        /// to default values.
        /// </summary>
        private void NullType1Settings()
        {
            GT_ResHor.Value = 640;
            GT_ResVert.Value = 640;
            GT_ScreenType.SelectedIndex = -1;
            GT_ModelQuality.SelectedIndex = -1;
            GT_TextureQuality.SelectedIndex = -1;
            GT_ShaderQuality.SelectedIndex = -1;
            GT_WaterQuality.SelectedIndex = -1;
            GT_ShadowQuality.SelectedIndex = -1;
            GT_ColorCorrectionT.SelectedIndex = -1;
            GT_AntiAliasing.SelectedIndex = -1;
            GT_Filtering.SelectedIndex = -1;
            GT_VSync.SelectedIndex = -1;
            GT_MotionBlur.SelectedIndex = -1;
            GT_DxMode.SelectedIndex = -1;
            GT_HDR.SelectedIndex = -1;
        }

        /// <summary>
        /// Resets controls of Type 2 video settings on Graphic Tweaker
        /// to default values.
        /// </summary>
        private void NullType2Settings()
        {
            GT_NCF_HorRes.Value = 640;
            GT_NCF_VertRes.Value = 480;
            GT_NCF_Brightness.SelectedIndex = -1;
            GT_NCF_Shadows.SelectedIndex = -1;
            GT_NCF_MBlur.SelectedIndex = -1;
            GT_NCF_Ratio.SelectedIndex = -1;
            GT_NCF_DispMode.SelectedIndex = -1;
            GT_NCF_AntiAlias.SelectedIndex = -1;
            GT_NCF_Filtering.SelectedIndex = -1;
            GT_NCF_VSync.SelectedIndex = -1;
            GT_NCF_Multicore.SelectedIndex = -1;
            GT_NCF_ShaderE.SelectedIndex = -1;
            GT_NCF_EffectD.SelectedIndex = -1;
            GT_NCF_MemPool.SelectedIndex = -1;
            GT_NCF_Quality.SelectedIndex = -1;
        }

        /// <summary>
        /// Resets controls of video settings on Graphic Tweaker
        /// to default values.
        /// </summary>
        private void NullGraphSettings()
        {
            switch (App.SourceGames[AppSelector.Text].SourceType)
            {
                case "1":
                    if (App.Platform.OS == CurrentPlatform.OSType.Windows) { NullType1Settings(); } else { NullType2Settings(); }
                    break;
                case "2":
                    NullType2Settings();
                    break;
            }
        }

        /// <summary>
        /// Changes the state of some controls on form.
        /// </summary>
        private void HandleControlsOnSelGame()
        {
            // Enable main tab selector...
            MainTabControl.Enabled = true;

            // Clearing lists...
            FP_ConfigSel.Items.Clear();
            HD_HSel.Items.Clear();

            // Disable controls on FPS-config tab...
            SetFPSButtons(false);
            FP_Install.Enabled = false;
            FP_Comp.Visible = false;

            // Disable controls on HUD Manager tab...
            SetHUDButtons(false);
            HD_Install.Enabled = false;
            HD_Homepage.Enabled = false;
            HD_Warning.Visible = false;
            HD_GB_Pbx.Image = null;
            HD_LastUpdate.Visible = false;

            // Enable custom installer menu element...
            MNUInstaller.Enabled = true;
        }

        /// <summary>
        /// Loads video settings of selected game.
        /// </summary>
        private void LoadGraphicSettings()
        {
            // Nulling controls of graphic tweaker...
            NullGraphSettings();

            // Reading video settings...
            switch (App.SourceGames[AppSelector.Text].SourceType)
            {
                case "1": /* Source 1, Type 1 (ex. GCF). */
                    if (App.Platform.OS == CurrentPlatform.OSType.Windows) { ReadType1VideoSettings(); } else { ReadType2VideoSettings(); }
                    break;
                case "2": /* Source 1, Type 2 (ex. NCF). */
                    ReadType2VideoSettings();
                    break;
                default:
                    throw new NotSupportedException();
            }

            // Switching between graphic tweaker modes...
            SelectGraphicWidget((App.Platform.OS != CurrentPlatform.OSType.Windows) && (App.SourceGames[AppSelector.Text].SourceType == "1") ? "2" : App.SourceGames[AppSelector.Text].SourceType);
        }

        /// <summary>
        /// Performes backup and then writes video settings of Type 1 game,
        /// selected in main window.
        /// </summary>
        private void PrepareWriteType1VideoSettings()
        {
            // Generating registry key name for working with video settings...
            string GameRegKey = Type1Video.GetGameRegKey(App.SourceGames[AppSelector.Text].SmallAppName);

            // Creating backup if Safe Clean feature is enabled...
            if (Properties.Settings.Default.SafeCleanup)
            {
                try
                {
                    Type1Video.BackUpVideoSettings(GameRegKey, "Game_AutoBackUp", App.SourceGames[AppSelector.Text].FullBackUpDirPath);
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex);
                }
            }

            try
            {
                // Creating registry key if does not exists...
                if (!Type1Video.CheckRegKeyExists(GameRegKey))
                {
                    Type1Video.CreateRegKey(GameRegKey);
                }

                // Saving video settings to registry...
                WriteType1VideoSettings();

                // Showing message...
                MessageBox.Show(AppStrings.GT_SaveSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.GT_SaveFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExT1SaveFail);
            }
        }

        /// <summary>
        /// Performes backup and then writes video settings of Type 2 game,
        /// selected in main window.
        /// </summary>
        private void PrepareWriteType2VideoSettings()
        {
            // Creating backup if Safe Clean feature is enabled...
            if (Properties.Settings.Default.SafeCleanup)
            {
                try
                {
                    FileManager.CreateConfigBackUp(App.SourceGames[AppSelector.Text].VideoCfgFiles, App.SourceGames[AppSelector.Text].FullBackUpDirPath, Properties.Resources.BU_PrefixVidAuto);
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, DebugStrings.AppDbgExT2AutoFail);
                }
            }

            try
            {
                // Saving video settings to file...
                WriteType2VideoSettings();

                // Showing message...
                MessageBox.Show(AppStrings.GT_SaveSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.GT_NCFFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExT2SaveFail);
            }
        }

        /// <summary>
        /// Resets controls of video settings on Graphic Tweaker
        /// to default values.
        /// </summary>
        private void WriteGraphicSettings()
        {
            switch (App.SourceGames[AppSelector.Text].SourceType)
            {
                case "1":
                    if (App.Platform.OS == CurrentPlatform.OSType.Windows) { PrepareWriteType1VideoSettings(); } else { PrepareWriteType2VideoSettings(); }
                    break;
                case "2":
                    PrepareWriteType2VideoSettings();
                    break;
            }
        }

        /// <summary>
        /// Switches between different views of Graphic Tweaker.
        /// </summary>
        /// <param name="SType">Source type.</param>
        private void SelectGraphicWidget(string SType)
        {
            switch (SType)
            {
                case "1":
                    GT_GType1.Visible = true;
                    GT_GType2.Visible = false;
                    break;
                case "2":
                    GT_GType1.Visible = false;
                    GT_GType2.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// Shows current Safe Clean status on form.
        /// </summary>
        private void CheckSafeClnStatus()
        {
            if (Properties.Settings.Default.SafeCleanup)
            {
                SB_App.Text = AppStrings.AppSafeClnStTextOn;
                SB_App.Image = Properties.Resources.green_circle;
            }
            else
            {
                SB_App.Text = AppStrings.AppSafeClnStTextOff;
                SB_App.Image = Properties.Resources.red_circle;
            }
        }

        /// <summary>
        /// Returns manually entered by user path to installed Steam client.
        /// </summary>
        /// <returns>Path to installed Steam client.</returns>
        private string GetPathByMEnter()
        {
            string Result;

            if (FldrBrwse.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(Path.Combine(FldrBrwse.SelectedPath, App.Platform.SteamBinaryName)))
                {
                    throw new FileNotFoundException(AppStrings.AppSteamPathEnterInvalid, Path.Combine(FldrBrwse.SelectedPath, App.Platform.SteamBinaryName));
                }
                else
                {
                    Result = FldrBrwse.SelectedPath;
                }
            }
            else
            {
                throw new OperationCanceledException(AppStrings.AppSteamPathEnterWinClosed);
            }

            return Result;
        }

        /// <summary>
        /// Checks if source string contains valid path to Steam client
        /// installation directory. If not, opens special dialog.
        /// </summary>
        /// <param name="OldPath">Valid path to Steam client installation directory.</param>
        private string CheckLastSteamPath(string OldPath)
        {
            return (!String.IsNullOrWhiteSpace(OldPath) && File.Exists(Path.Combine(OldPath, App.Platform.SteamBinaryName))) ? OldPath : GetPathByMEnter();
        }

        /// <summary>
        /// Tries to find Steam client installation directory.
        /// </summary>
        private void ValidateAndHandle()
        {
            try
            {
                App.SteamClient.FullSteamPath = CheckLastSteamPath(Properties.Settings.Default.LastSteamPath);
            }
            catch (FileNotFoundException Ex)
            {
                MessageBox.Show(AppStrings.SteamPathEnterErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(Ex, DebugStrings.AppDbgExSteamPath);
                Environment.Exit(ReturnCodes.StmWrongPath);
            }
            catch (OperationCanceledException Ex)
            {
                MessageBox.Show(AppStrings.SteamPathCancel, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(Ex, DebugStrings.AppDbgExSteamPath);
                Environment.Exit(ReturnCodes.StmPathCancel);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.AppGenericError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(Ex, DebugStrings.AppDbgExSteamPath);
                Environment.Exit(ReturnCodes.StmPathException);
            }
        }

        /// <summary>
        /// Creates instances of CurrentApp and SteamManager classes.
        /// </summary>
        private void InitializeApp()
        {
            // Create a new instance of CurrentApp class...
            App = new CurrentApp(Properties.Settings.Default.IsPortable, Properties.Resources.AppName);

            // Create a new instance of SteamManager class and take care of possible errors...
            try
            {
                App.SteamClient = new SteamManager(Properties.Settings.Default.LastSteamID, App.Platform.OS);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show(AppStrings.AppNoSteamIDSetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(ReturnCodes.NoUserIdsDetected);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExStmmInit);
                ValidateAndHandle();
            }
        }

        /// <summary>
        /// Sets strings data for main form.
        /// </summary>
        private void SetAppStrings()
        {
            // Save last Steam path in application's settings...
            Properties.Settings.Default.LastSteamPath = App.SteamClient.FullSteamPath;

            // Add application version and platform name to form's title...
            Text = String.Format(Text, Properties.Resources.AppName, App.Platform.OSFriendlyName, CurrentApp.AppVersion);
            
            // Add Steam client installation path to Troubleshooting page...
            PS_StPath.Text = String.Format(PS_StPath.Text, App.SteamClient.FullSteamPath);
        }

        /// <summary>
        /// Changes state of some controls, depending on current running
        /// platform or access level.
        /// </summary>
        /// <param name="State">Current access level state.</param>
        private void ChangePrvControlState(bool State)
        {
            // Checking platform...
            if (App.Platform.OS == CurrentPlatform.OSType.Windows)
            {
                // On Windows we will disable "Keyboard disabler" module due to absent admin rights...
                MNUWinMnuDisabler.Enabled = State;
            }
            else
            {
                // On Linux and MacOS we will disable "Keyboard disabler" and Reporter modules...
                MNUReportBuilder.Enabled = false;
                MNUWinMnuDisabler.Enabled = false;
            }
        }

        /// <summary>
        /// Checks file system name on game installation drive and shows this on form.
        /// </summary>
        private void DetectFS()
        {
            try
            {
                PS_OSDrive.Text = String.Format(PS_OSDrive.Text, FileManager.DetectDriveFileSystem(Path.GetPathRoot(App.SourceGames[AppSelector.Text].FullGamePath)));
            }
            catch (Exception Ex)
            {
                PS_OSDrive.Text = String.Format(PS_OSDrive.Text, "Unknown");
                Logger.Warn(Ex);
            }
        }

        /// <summary>
        /// Checks the number of found supported games and performes some actions.
        /// </summary>
        /// <param name="GamesCount">The number of found supported games.</param>
        private void CheckGames(int GamesCount)
        {
            switch (GamesCount)
            {
                case 0: // No games found.
                    {
                        Logger.Warn(String.Format(AppStrings.AppNoGamesDLog, App.SteamClient.FullSteamPath));
                        MessageBox.Show(AppStrings.AppNoGamesDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Environment.Exit(ReturnCodes.NoGamesDetected);
                    }
                    break;
                case 1: // Only one game found. Select it automatically.
                    {
                        AppSelector.SelectedIndex = 0;
                        UpdateStatusBar();
                    }
                    break;
                default: // More than one game found. Selecting last used game.
                    {
                        int Ai = AppSelector.Items.IndexOf(Properties.Settings.Default.LastGameName);
                        AppSelector.SelectedIndex = Ai != -1 ? Ai : 0;
                    }
                    break;
            }
        }

        /// <summary>
        /// Checks for restricted symbols in Steam installation path and shows
        /// result on form.
        /// </summary>
        private void CheckSymbolsSteam()
        {
            if (!FileManager.CheckNonASCII(App.SteamClient.FullSteamPath))
            {
                PS_PathSteam.ForeColor = Color.Red;
                PS_PathSteam.Image = Properties.Resources.upd_err;
                Logger.Warn(String.Format(AppStrings.AppRestrSymbLog, App.SteamClient.FullSteamPath));
            }
        }

        /// <summary>
        /// Checks for restricted symbols in selected game installation path
        /// and shows result on form.
        /// </summary>
        private void CheckSymbolsGame()
        {
            if (!FileManager.CheckNonASCII(App.SourceGames[AppSelector.Text].FullGamePath))
            {
                PS_PathGame.ForeColor = Color.Red;
                PS_PathGame.Image = Properties.Resources.upd_err;
                Logger.Warn(String.Format(AppStrings.AppRestrSymbLog, App.SourceGames[AppSelector.Text].FullGamePath));
            }
            else
            {
                PS_PathGame.ForeColor = Color.Green;
                PS_PathGame.Image = Properties.Resources.upd_nx;
            }
        }

        /// <summary>
        /// Handles with installed FPS-configs and shows special icon.
        /// </summary>
        private void HandleConfigs()
        {
            if (!Directory.Exists(App.SourceGames[AppSelector.Text].AppCfgDir)) { Directory.CreateDirectory(App.SourceGames[AppSelector.Text].AppCfgDir); }
            App.SourceGames[AppSelector.Text].FPSConfigs = FileManager.ExpandFileList(ConfigManager.ListFPSConfigs(App.SourceGames[AppSelector.Text].FullGamePath, App.SourceGames[AppSelector.Text].IsUsingUserDir), true);
            GT_Warning.Visible = App.SourceGames[AppSelector.Text].FPSConfigs.Count > 0;
        }

        /// <summary>
        /// Handles with current Steam UserID.
        /// </summary>
        /// <param name="SID">Last used Steam UserID.</param>
        private void HandleSteamIDs(string SID)
        {
            try
            {
                string Result = App.SteamClient.GetCurrentSteamID(SID);
                SB_SteamID.Text = Result;
                Properties.Settings.Default.LastSteamID = Result;
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
                SB_SteamID.Text = String.Empty;
            }
        }

        /// <summary>
        /// Changes the state of some controls on "HUD Manager" page.
        /// </summary>
        /// <param name="State">If selected HUD installed.</param>
        private void SetHUDButtons(bool State)
        {
            HD_Install.Text = State ? AppStrings.HD_BtnUpdateText : AppStrings.HD_BtnInstallText;
            HD_Uninstall.Enabled = State;
            HD_OpenDir.Enabled = State;
        }

        /// <summary>
        /// Changes the state of some controls on "FPS-config" page.
        /// </summary>
        /// <param name="State">If selected FPS-config installed.</param>
        private void SetFPSButtons(bool State)
        {
            FP_Install.Text = State ? App.SourceGames[AppSelector.Text].IsUsingUserDir ? AppStrings.FP_BtnUpdateText : AppStrings.FP_BtnReplaceText : AppStrings.FP_BtnInstallText;
            FP_Uninstall.Text = App.SourceGames[AppSelector.Text].IsUsingUserDir ? AppStrings.FP_BtnUninstallSelectedText : AppStrings.FP_BtnUninstallAllText;
            FP_Uninstall.Enabled = State;
            FP_OpenNotepad.Enabled = State;
        }

        /// <summary>
        /// Changes the state of the status bar depending of currently selected tab.
        /// </summary>
        private void UpdateStatusBar()
        {
            switch (MainTabControl.SelectedIndex)
            {
                case 1: // "Config Editor" page selected...
                    {
                        MNUShowEdHint.Enabled = true;
                        SB_Status.ForeColor = Color.Black;
                        SB_Status.Text = String.Format(AppStrings.StatusOpenedFile, String.IsNullOrEmpty(CFGFileName) ? AppStrings.UnnamedFileName : Path.GetFileName(CFGFileName));
                    }
                    break;
                case 4: // "HUD manager" page selected...
                    {
                        bool HUDDbStatus = HUDManager.CheckHUDDatabase(Properties.Settings.Default.LastHUDTime);
                        MNUShowEdHint.Enabled = false;
                        SB_Status.ForeColor = HUDDbStatus ? Color.Red : Color.Black;
                        SB_Status.Text = String.Format(AppStrings.HD_DynBarText, HUDDbStatus ? AppStrings.HD_StatusOutdated : AppStrings.HD_StatusUpdated, Properties.Settings.Default.LastHUDTime);
                    }
                    break;
                default: // Any other page selected...
                    {
                        MNUShowEdHint.Enabled = false;
                        SB_Status.ForeColor = Color.Black;
                        SB_Status.Text = AppStrings.StatusNormal;
                    }
                    break;
            }
        }

        /// <summary>
        /// Checks if all Type 1 game video settings were set by user.
        /// </summary>
        private bool CheckType1Settings()
        {
            return (GT_ScreenType.SelectedIndex != -1) && (GT_ModelQuality.SelectedIndex != -1)
                && (GT_TextureQuality.SelectedIndex != -1) && (GT_ShaderQuality.SelectedIndex != -1)
                && (GT_WaterQuality.SelectedIndex != -1) && (GT_ShadowQuality.SelectedIndex != -1)
                && (GT_ColorCorrectionT.SelectedIndex != -1) && (GT_AntiAliasing.SelectedIndex != -1)
                && (GT_Filtering.SelectedIndex != -1) && (GT_VSync.SelectedIndex != -1)
                && (GT_MotionBlur.SelectedIndex != -1) && (GT_DxMode.SelectedIndex != -1)
                && (GT_HDR.SelectedIndex != -1);
        }

        /// <summary>
        /// Checks if all Type 2 game video settings were set by user.
        /// </summary>
        private bool CheckType2Settings()
        {
            return (GT_NCF_Quality.SelectedIndex != -1) && (GT_NCF_MemPool.SelectedIndex != -1)
                && (GT_NCF_EffectD.SelectedIndex != -1) && (GT_NCF_ShaderE.SelectedIndex != -1)
                && (GT_NCF_Multicore.SelectedIndex != -1) && (GT_NCF_VSync.SelectedIndex != -1)
                && (GT_NCF_Filtering.SelectedIndex != -1) && (GT_NCF_AntiAlias.SelectedIndex != -1)
                && (GT_NCF_DispMode.SelectedIndex != -1) && (GT_NCF_Ratio.SelectedIndex != -1)
                && (GT_NCF_Brightness.SelectedIndex != -1) && (GT_NCF_Shadows.SelectedIndex != -1)
                && (GT_NCF_MBlur.SelectedIndex != -1);
        }

        /// <summary>
        /// Checks if all game video settings were set by user.
        /// </summary>
        private bool ValidateGameSettings()
        {
            bool Result = false;
            switch (App.SourceGames[AppSelector.Text].SourceType)
            {
                case "1":
                    Result = App.Platform.OS == CurrentPlatform.OSType.Windows ? CheckType1Settings() : CheckType2Settings();
                    break;
                case "2":
                    Result = CheckType2Settings();
                    break;
            }
            return Result;
        }

        /// <summary>
        /// Closes all loaded files in Config Editor and clean its window.
        /// </summary>
        private void CloseEditorConfigs()
        {
            CFGFileName = String.Empty;
            CE_Editor.Rows.Clear();
        }

        /// <summary>
        /// Gets full list of available backups in a separate thread.
        /// </summary>
        private void UpdateBackUpList()
        {
            if (!BW_BkUpRecv.IsBusy)
            {
                BW_BkUpRecv.RunWorkerAsync(AppSelector.Text);
            }
        }

        /// <summary>
        /// Searches for installed supported games.
        /// </summary>
        private void FindGames()
        {
            // Searching for installed games...
            try
            {
                DetectInstalledGames();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.AppXMLParseError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(Ex, DebugStrings.AppDbgExXmlParse);
                Environment.Exit(ReturnCodes.GameDbParseError);
            }

            // Checking if any installed supported games were found...
            CheckGames(App.SourceGames.InstalledGameNames.Count);
        }

        /// <summary>
        /// Enables or disables HUD Manager page.
        /// </summary>
        /// <param name="Mode">If current game supports custom HUDs.</param>
        private void HandleHUDMode(bool Mode)
        {
            HUD_Panel.Visible = Mode;
            HUD_NotAvailable.Visible = !Mode;
        }

        /// <summary>
        /// Launches program update checker.
        /// </summary>
        private void CheckForUpdates()
        {
            if (Properties.Settings.Default.AutoUpdateCheck && IsAutoUpdateCheckNeeded())
            {
                if (!BW_UpChk.IsBusy)
                {
                    BW_UpChk.RunWorkerAsync();
                }
            }
        }

        /// <summary>
        /// Generates internal offline help URL, based on current tab.
        /// </summary>
        /// <param name="TabIndex">Current tab index.</param>
        /// <returns>Internal offline help URL.</returns>
        private string GetHelpWebPage(int TabIndex)
        {
            // Creating variable for storing result...
            string Result = String.Empty;

            // Generating page name, based on tab ID...
            switch (TabIndex)
            {
                case 0: // Graphic tweaker...
                    Result = "graphic-tweaker";
                    break;
                case 1: // Config editor...
                    Result = "config-editor";
                    break;
                case 2: // Problem solver...
                    Result = "cleanup";
                    break;
                case 3: // FPS-config manager...
                    Result = "fps-configs";
                    break;
                case 4: // HUD manager...
                    Result = "hud-manager";
                    break;
                case 5: // BackUps manager...
                    Result = "backups";
                    break;
            }

            // Returns result...
            return String.Format("{0}.html", Result);
        }

        /// <summary>
        /// Checks if application update check is required.
        /// </summary>
        private bool IsAutoUpdateCheckNeeded()
        {
            TimeSpan TS = DateTime.Now - Properties.Settings.Default.LastUpdateTime;
            return TS.Days >= 7;
        }

        /// <summary>
        /// Adds found backup files to collection on BackUps tab.
        /// </summary>
        /// <param name="DItems">Found items.</param>
        private void AddBackUpsToTable(FileInfo[] DItems)
        {
            foreach (FileInfo DItem in DItems)
            {
                Tuple<string, string> Rs = GenUserFriendlyBackupDesc(DItem);
                ListViewItem LvItem = new ListViewItem(Rs.Item2)
                {
                    BackColor = Properties.Settings.Default.HighlightOldBackUps && (DateTime.UtcNow - DItem.CreationTimeUtc > TimeSpan.FromDays(30)) ? Color.LightYellow : BU_LVTable.BackColor,
                    SubItems =
                    {
                        Rs.Item1,
                        GuiHelpers.SclBytes(DItem.Length),
                        DItem.CreationTime.ToString(),
                        DItem.Name
                    }
                };
                BU_LVTable.Items.Add(LvItem);
            }
        }

        /// <summary>
        /// Opens cleanup window and start cleanup sequence with additional targets.
        /// </summary>
        /// <param name="ID">Cleanup target ID.</param>
        /// <param name="Title">Title for cleanup window.</param>
        /// <param name="Targets">Additional targets for cleanup.</param>
        private void StartCleanup(string ID, string Title, List<String> Targets)
        {
            if (!BW_ClnList.IsBusy)
            {
                try
                {
                    List<String> CleanDirs = new List<String>(App.SourceGames[AppSelector.Text].ClnMan[ID].Directories);
                    if (Targets.Count > 0)
                    {
                        CleanDirs.AddRange(Targets);
                    }
                    GuiHelpers.FormShowCleanup(CleanDirs, Title.ToLower(), AppStrings.PS_CleanupSuccess, App.SourceGames[AppSelector.Text].FullBackUpDirPath, App.SourceGames[AppSelector.Text].GameBinaryFile);
                }
                catch (Exception Ex)
                {
                    Logger.Error(Ex);
                    MessageBox.Show(AppStrings.PS_ClnWndInitError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.PS_BwBusy, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Opens cleanup window and start cleanup sequence.
        /// </summary>
        /// <param name="ID">Cleanup target ID.</param>
        /// <param name="Title">Title for cleanup window.</param>
        private void StartCleanup(string ID, string Title)
        {
            StartCleanup(ID, Title, new List<String>());
        }

        /// <summary>
        /// Searches for available HUDs.
        /// </summary>
        private void HandleHUDs()
        {
            if (App.SourceGames[AppSelector.Text].IsHUDsAvailable)
            {
                // Creating local directory for saving HUDs if does not exists...
                if (!Directory.Exists(App.SourceGames[AppSelector.Text].AppHUDDir)) { Directory.CreateDirectory(App.SourceGames[AppSelector.Text].AppHUDDir); }

                // Updating HUDs list in a separate thread...
                if (!BW_HUDList.IsBusy)
                {
                    BW_HUDList.RunWorkerAsync(AppSelector.Text);
                }
            }
        }

        /// <summary>
        /// Searches for available FPS-configs.
        /// </summary>
        private void HandleFpsConfigs()
        {
            if (!BW_FPRecv.IsBusy)
            {
                BW_FPRecv.RunWorkerAsync(AppSelector.Text);
            }
        }

        /// <summary>
        /// Searches for available cleanup targets.
        /// </summary>
        private void HandleCleanupTargets()
        {
            if (!BW_ClnList.IsBusy)
            {
                BW_ClnList.RunWorkerAsync(AppSelector.Text);
            }
        }

        /// <summary>
        /// BackUp available FPS-configs to archive if SafeCleanup
        /// is enabled.
        /// </summary>
        private void BackUpFPSConfigs()
        {
            if (Properties.Settings.Default.SafeCleanup)
            {
                if (App.SourceGames[AppSelector.Text].FPSConfigs.Count > 0)
                {
                    try
                    {
                        FileManager.CompressFiles(App.SourceGames[AppSelector.Text].FPSConfigs, FileManager.GenerateBackUpFileName(App.SourceGames[AppSelector.Text].FullBackUpDirPath, Properties.Resources.BU_PrefixCfg));
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, DebugStrings.AppDbgExFpsInstBackup);
                    }
                }
            }
        }

        /// <summary>
        /// Shows FPS-config selection dialog and load selected config
        /// in Config Editor module or default text editor.
        /// </summary>
        /// <param name="ConfigFile">Full path to config file to edit.</param>
        /// <param name="UseNotepad">Use default text editor instead of Config Editor.</param>
        private void EditFPSConfig(string ConfigFile, bool UseNotepad = false)
        {
            if (!String.IsNullOrWhiteSpace(ConfigFile) && File.Exists(ConfigFile))
            {
                if (UseNotepad)
                {
                    ProcessManager.OpenTextEditor(ConfigFile, Properties.Settings.Default.EditorBin, App.Platform.OS);
                }
                else
                {
                    ReadConfigFromFile(ConfigFile);
                    MainTabControl.SelectedIndex = 1;
                }
            }
            else
            {
                Logger.Warn(String.Format(DebugStrings.AppDbgExCfgEditorLoad, ConfigFile));
            }
        }

        /// <summary>
        /// Checks if any of FPS-configs are installed.
        /// </summary>
        /// <returns>Returns True if at least one FPS-config is installed.</returns>
        private bool CheckIfFPSConfigInstalled()
        {
            return App.SourceGames[AppSelector.Text].CFGMan.CheckInstalledConfig(App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].InstallDir, App.SourceGames[AppSelector.Text].IsUsingUserDir);
        }

        #endregion

        #region Internal Workers

        /// <summary>
        /// Checks for application updates in a separate thread.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void BW_UpChk_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = AutoUpdateCheck();
        }

        /// <summary>
        /// Handles result of application update check.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void BW_UpChk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if ((bool)e.Result)
                {
                    MessageBox.Show(String.Format(AppStrings.AppUpdateAvailable, Properties.Resources.AppName), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Properties.Settings.Default.LastUpdateTime = DateTime.Now;
                }
            }
            else
            {
                Logger.Warn(e.Error, DebugStrings.AppDbgExBgWChk);
            }
        }

        /// <summary>
        /// Gets collection of available FPS-configs.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void BW_FPRecv_DoWork(object sender, DoWorkEventArgs e)
        {
            App.SourceGames[(string)e.Argument].CFGMan = new ConfigManager(App.FullAppPath, App.SourceGames[(string)e.Argument].AppCfgDir, App.SourceGames[(string)e.Argument].IsUsingUserDir ? App.SourceGames[(string)e.Argument].CustomInstallDir : App.SourceGames[(string)e.Argument].FullGamePath, AppStrings.AppLangPrefix);
        }

        /// <summary>
        /// Renders collection of available FPS-configs on its tab.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void BW_FPRecv_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                // Adding configs to collection...
                FP_ConfigSel.Items.AddRange(App.SourceGames[AppSelector.Text].CFGMan.ConfigNames.ToArray());

                // Checking if collection contains any items...
                if (FP_ConfigSel.Items.Count >= 1)
                {
                    FP_Description.Text = AppStrings.FP_SelectFromList;
                    FP_Description.ForeColor = Color.Black;
                }
            }
            else
            {
                // Exception detected. Writing to log...
                Logger.Warn(e.Error);

                // Showing message...
                FP_Description.Text = AppStrings.FP_NoCfgGame;
                FP_Description.ForeColor = Color.Red;

                // Blockg some form controls...
                FP_Install.Enabled = false;
                FP_ConfigSel.Enabled = false;
                FP_OpenNotepad.Enabled = false;
            }
        }

        /// <summary>
        /// Gets collection of available backups.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void BW_BkUpRecv_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!Directory.Exists(App.SourceGames[(string)e.Argument].FullBackUpDirPath))
            {
                Directory.CreateDirectory(App.SourceGames[(string)e.Argument].FullBackUpDirPath);
            }
            DirectoryInfo DInfo = new DirectoryInfo(App.SourceGames[(string)e.Argument].FullBackUpDirPath);
            e.Result = DInfo.GetFiles("*.*");
        }

        /// <summary>
        /// Renders collection of available backups on its tab.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void BW_BkUpRecv_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                try
                {
                    BU_LVTable.Items.Clear();
                    AddBackUpsToTable((FileInfo[])e.Result);
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex);
                }
            }
            else
            {
                Logger.Warn(e.Error);
            }
        }

        /// <summary>
        /// Gets collection of available HUDs.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void BW_HUDList_DoWork(object sender, DoWorkEventArgs e)
        {
            App.SourceGames[(string)e.Argument].HUDMan = new HUDManager(App.SourceGames[(string)e.Argument].SmallAppName, App.FullAppPath, App.SourceGames[(string)e.Argument].AppHUDDir, Properties.Settings.Default.HUDHideOutdated);
        }


        /// <summary>
        /// Renders collection of available HUDs on its tab.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void BW_HUDList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                HD_HSel.Items.AddRange(App.SourceGames[AppSelector.Text].HUDMan.AvailableHUDNames.ToArray<object>());
            }
            else
            {
                Logger.Warn(e.Error);
            }
        }

        /// <summary>
        /// Gets screenshot of selected HUD from disk or URL.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void BW_HUDScreen_DoWork(object sender, DoWorkEventArgs e)
        {
            // Parsing arguments...
            List<String> Agruments = e.Argument as List<String>;

            // Generating full file name for HUD screenshot...
            string ScreenFile = Path.Combine(App.SourceGames[Agruments[0]].AppHUDDir, Path.GetFileName(App.SourceGames[Agruments[0]].HUDMan[Agruments[1]].Preview));

            // Downloading file if it doesn't exists...
            if (!File.Exists(ScreenFile))
            {
                using (WebClient Downloader = new WebClient())
                {
                    Downloader.Headers.Add("User-Agent", App.UserAgent);
                    Downloader.DownloadFile(App.SourceGames[Agruments[0]].HUDMan[Agruments[1]].Preview, ScreenFile);
                }
            }

            // Returning result to callback...
            e.Result = ScreenFile;
        }

        /// <summary>
        /// Renders screenshot of selected HUD on its tab.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void BW_HUDScreen_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                HD_GB_Pbx.Image = Image.FromFile((string)e.Result);
            }
            else
            {
                Logger.Warn(e.Error);
                if (File.Exists((string)e.Result)) { File.Delete((string)e.Result); }
            }
        }

        /// <summary>
        /// Finalizes installation of selected HUD.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void BW_HudInstall_DoWork(object sender, DoWorkEventArgs e)
        {
            List<String> Arguments = e.Argument as List<String>;
            string InstallTmp = Path.Combine(App.SourceGames[Arguments[0]].CustomInstallDir, "hudtemp");

            try
            {
                Directory.Move(Path.Combine(InstallTmp, HUDManager.FormatIntDir(App.SourceGames[Arguments[0]].HUDMan[Arguments[1]].ArchiveDir)), Path.Combine(App.SourceGames[Arguments[0]].CustomInstallDir, App.SourceGames[Arguments[0]].HUDMan[Arguments[1]].InstallDir));
            }
            finally
            {
                try
                {
                    if (Directory.Exists(InstallTmp))
                    {
                        Directory.Delete(InstallTmp, true);
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex);
                }
            }
        }

        /// <summary>
        /// Returns information with HUD installation result.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void BW_HudInstall_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            HD_Install.Enabled = true;
            if (e.Error == null)
            {
                MessageBox.Show(AppStrings.HD_InstallSuccessfull, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(AppStrings.HD_InstallError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(e.Error, DebugStrings.AppDbgExHUDInstall);
            }
            SetHUDButtons(HUDManager.CheckInstalledHUD(App.SourceGames[AppSelector.Text].CustomInstallDir, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].InstallDir));
        }

        /// <summary>
        /// Gets collection of available cleanup targets for selected game.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void BW_ClnList_DoWork(object sender, DoWorkEventArgs e)
        {
            App.SourceGames[(string)e.Argument].ClnMan = new CleanupManager(App.FullAppPath, App.SourceGames[(string)e.Argument], Properties.Settings.Default.AllowUnSafeCleanup);
        }

        /// <summary>
        /// Finalizes check of available cleanup targets for selected game.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void BW_ClnList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.Warn(e.Error);
            }
        }

        #endregion

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmMainW_Load(object sender, EventArgs e)
        {
            InitializeApp();
            SetAppStrings();
            ChangePrvControlState(ProcessManager.IsCurrentUserAdmin());
            CheckSafeClnStatus();
            CheckSymbolsSteam();
            FindGames();
            CheckForUpdates();
        }

        /// <summary>
        /// "Clean blobs" checkbox status changed event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_CleanBlobs_CheckedChanged(object sender, EventArgs e)
        {
            PS_ExecuteNow.Enabled = PS_CleanBlobs.Checked || PS_CleanRegistry.Checked;
        }

        /// <summary>
        /// "Clean registry" checkbox status changed event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_CleanRegistry_CheckedChanged(object sender, EventArgs e)
        {
            // Enable control with selector of available Steam languages...
            PS_SteamLang.Enabled = PS_CleanRegistry.Checked;

            // Getting current language name from Registry...
            if (App.Platform.OS == CurrentPlatform.OSType.Windows)
            {
                try
                {
                    switch (App.SteamClient.GetSteamLanguage())
                    {
                        case "russian":
                            PS_SteamLang.SelectedIndex = 1;
                            break;
                        default:
                            PS_SteamLang.SelectedIndex = 0;
                            break;
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex);
                    PS_SteamLang.SelectedIndex = 0;
                }
            }
            else
            {
                // Performing selection based on current application locale...
                PS_SteamLang.SelectedIndex = Convert.ToInt32(AppStrings.AppDefaultSteamLangID);
            }

            // Enable or disable button based on checkboxes...
            PS_ExecuteNow.Enabled = PS_CleanRegistry.Checked || PS_CleanBlobs.Checked;
        }

        /// <summary>
        /// "Execute cleanup" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_ExecuteNow_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppStrings.PS_ExecuteMSG, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if ((PS_CleanBlobs.Checked) || (PS_CleanRegistry.Checked))
                {
                    if (ProcessManager.ProcessTerminate("Steam") != 0)
                    {
                        MessageBox.Show(AppStrings.PS_ProcessDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    if (PS_CleanBlobs.Checked)
                    {
                        try
                        {
                            App.SteamClient.CleanBlobsNow();
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show(AppStrings.PS_CleanException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Logger.Error(Ex, DebugStrings.AppDbgExClnBlobs);
                        }
                    }

                    if (PS_CleanRegistry.Checked)
                    {
                        try
                        {
                            if (PS_SteamLang.SelectedIndex != -1)
                            {
                                App.SteamClient.CleanRegistryNow(PS_SteamLang.SelectedIndex);
                            }
                            else
                            {
                                MessageBox.Show(AppStrings.PS_NoLangSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                App.SteamClient.CleanRegistryNow(0);
                            }
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show(AppStrings.PS_CleanException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Logger.Error(Ex, DebugStrings.AppDbgExClnReg);
                        }
                    }

                    MessageBox.Show(AppStrings.PS_SeqCompleted, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (File.Exists(Path.Combine(App.SteamClient.FullSteamPath, App.Platform.SteamBinaryName)))
                    {
                        Process.Start(Path.Combine(App.SteamClient.FullSteamPath, App.Platform.SteamBinaryName));
                    }
                }
            }
        }

        /// <summary>
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmMainW_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = (Properties.Settings.Default.ConfirmExit && !(MessageBox.Show(String.Format(AppStrings.FrmCloseQuery, Properties.Resources.AppName), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)) || (BW_BkUpRecv.IsBusy || BW_FPRecv.IsBusy || BW_HudInstall.IsBusy || BW_HUDList.IsBusy || BW_HUDScreen.IsBusy || BW_UpChk.IsBusy || BW_ClnList.IsBusy);
            }
        }

        /// <summary>
        /// "Game selected" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void AppSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HandleControlsOnSelGame();
                CheckSymbolsGame();
                DetectFS();

                try
                {
                    LoadGraphicSettings();
                }
                catch (NotSupportedException)
                {
                    MessageBox.Show(AppStrings.AppIncorrectSrcVersion, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                HandleSteamIDs(Properties.Settings.Default.LastSteamID);
                HandleConfigs();
                CloseEditorConfigs();
                HandleFpsConfigs();
                UpdateStatusBar();
                Properties.Settings.Default.LastGameName = AppSelector.Text;
                HandleHUDMode(App.SourceGames[AppSelector.Text].IsHUDsAvailable);
                HandleHUDs();
                UpdateBackUpList();
                HandleCleanupTargets();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.AppFailedToGetData, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExSelGame);
            }
        }

        /// <summary>
        /// "Refresh game list" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void AppRefresh_Click(object sender, EventArgs e)
        {
            FindGames();
        }

        /// <summary>
        /// "Maximum quality" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void GT_Maximum_Graphics_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppStrings.GT_MaxPerfMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                switch (App.SourceGames[AppSelector.Text].SourceType)
                {
                    case "1":
                        GT_ScreenType.SelectedIndex = 0;
                        GT_ModelQuality.SelectedIndex = 2;
                        GT_TextureQuality.SelectedIndex = 2;
                        GT_ShaderQuality.SelectedIndex = 1;
                        GT_WaterQuality.SelectedIndex = 1;
                        GT_ShadowQuality.SelectedIndex = 1;
                        GT_ColorCorrectionT.SelectedIndex = 1;
                        GT_AntiAliasing.SelectedIndex = 5;
                        GT_Filtering.SelectedIndex = 5;
                        GT_VSync.SelectedIndex = 0;
                        GT_MotionBlur.SelectedIndex = 0;
                        GT_DxMode.SelectedIndex = 3;
                        GT_HDR.SelectedIndex = 2;
                        break;
                    case "2":
                        GT_NCF_DispMode.SelectedIndex = 0;
                        GT_NCF_Ratio.SelectedIndex = 1;
                        GT_NCF_Brightness.Text = "22";
                        GT_NCF_AntiAlias.SelectedIndex = 5;
                        GT_NCF_Filtering.SelectedIndex = 5;
                        GT_NCF_Shadows.SelectedIndex = 3;
                        GT_NCF_MBlur.SelectedIndex = 1;
                        GT_NCF_VSync.SelectedIndex = 0;
                        GT_NCF_Multicore.SelectedIndex = 1;
                        GT_NCF_ShaderE.SelectedIndex = 3;
                        GT_NCF_EffectD.SelectedIndex = 2;
                        GT_NCF_MemPool.SelectedIndex = 2;
                        GT_NCF_Quality.SelectedIndex = 2;
                        break;
                }
                MessageBox.Show(AppStrings.GT_PerfSet, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// "Maximum performance" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void GT_Maximum_Performance_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppStrings.GT_MinPerfMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                switch (App.SourceGames[AppSelector.Text].SourceType)
                {
                    case "1":
                        GT_ScreenType.SelectedIndex = 0;
                        GT_ModelQuality.SelectedIndex = 0;
                        GT_TextureQuality.SelectedIndex = 0;
                        GT_ShaderQuality.SelectedIndex = 0;
                        GT_WaterQuality.SelectedIndex = 0;
                        GT_ShadowQuality.SelectedIndex = 0;
                        GT_ColorCorrectionT.SelectedIndex = 0;
                        GT_AntiAliasing.SelectedIndex = 0;
                        GT_Filtering.SelectedIndex = 1;
                        GT_VSync.SelectedIndex = 0;
                        GT_MotionBlur.SelectedIndex = 0;
                        GT_DxMode.SelectedIndex = MessageBox.Show(AppStrings.GT_DxLevelMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes ? 0 : 3;
                        GT_HDR.SelectedIndex = 0;
                        break;
                    case "2":
                        GT_NCF_DispMode.SelectedIndex = 0;
                        GT_NCF_Ratio.SelectedIndex = 1;
                        GT_NCF_Brightness.Text = "22";
                        GT_NCF_AntiAlias.SelectedIndex = 0;
                        GT_NCF_Filtering.SelectedIndex = 1;
                        GT_NCF_Shadows.SelectedIndex = 0;
                        GT_NCF_MBlur.SelectedIndex = 0;
                        GT_NCF_VSync.SelectedIndex = 0;
                        GT_NCF_Multicore.SelectedIndex = 1;
                        GT_NCF_ShaderE.SelectedIndex = 0;
                        GT_NCF_EffectD.SelectedIndex = 0;
                        GT_NCF_MemPool.SelectedIndex = 0;
                        GT_NCF_Quality.SelectedIndex = 0;
                        break;
                }
                MessageBox.Show(AppStrings.GT_PerfSet, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// "Save video settings" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void GT_SaveApply_Click(object sender, EventArgs e)
        {
            if (ValidateGameSettings())
            {
                if (MessageBox.Show(AppStrings.GT_SaveMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    WriteGraphicSettings();
                }
            }
            else
            {
                MessageBox.Show(AppStrings.GT_NCFNReady, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "FPS-config selected" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FP_ConfigSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Checking result...
                bool Success = !String.IsNullOrEmpty(App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].Name);

                // Changing some controls state...
                FP_Description.Text = App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].Description;
                FP_Comp.Visible = !App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].CheckCompatibility(App.SourceGames[AppSelector.Text].GameInternalID);
                FP_Install.Enabled = Success;

                // Checking if selected FPS-config is installed...
                SetFPSButtons(CheckIfFPSConfigInstalled());
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
                FP_Description.Text = AppStrings.FP_NoDescr;
            }
        }

        /// <summary>
        /// "Install FPS-config" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FP_Install_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(AppStrings.FP_InstallQuestion, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Creating backup of current FPS-config files...
                    BackUpFPSConfigs();

                    // Downloading FPS-config archive...
                    GuiHelpers.FormShowDownloader(App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].URI, App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].LocalFile);

                    // Checking if downloaded archive exists...
                    if (File.Exists(App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].LocalFile))
                    {
                        // Checking hash of downloaded file...
                        if (FileManager.CalculateFileSHA512(App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].LocalFile) == App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].FileHash)
                        {
                            // Checking if selected FPS-config is installed...
                            if (CheckIfFPSConfigInstalled())
                            {
                                // Removing installed files...
                                GuiHelpers.FormShowRemoveFiles(Path.Combine(App.SourceGames[AppSelector.Text].CFGMan.FPSConfigInstallPath, App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].InstallDir));
                            }

                            // Extracting downloaded archove...
                            GuiHelpers.FormShowArchiveExtract(App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].LocalFile, App.SourceGames[AppSelector.Text].CFGMan.FPSConfigInstallPath);

                            // Moving archive contents for games without user directory support...
                            if (!App.SourceGames[AppSelector.Text].IsUsingUserDir)
                            {
                                App.SourceGames[AppSelector.Text].CFGMan.MoveLegacyConfig(App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].InstallDir, App.SourceGames[AppSelector.Text].FullCfgPath);
                            }

                            // Installation successful message...
                            MessageBox.Show(AppStrings.FP_InstallSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Hash missmatch. Show error...
                            MessageBox.Show(AppStrings.FP_HashError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show(AppStrings.FP_DownloadError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Changing the state of some controls...
                    SetFPSButtons(true);
                    HandleConfigs();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.FP_InstallFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExFpsInstall);
            }
            finally
            {
                try
                {
                    // Removing downloaded file...
                    if (File.Exists(App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].LocalFile))
                    {
                        File.Delete(App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].LocalFile);
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, DebugStrings.AppDbgExCfgArchRem);
                }
            }
        }

        /// <summary>
        /// "Uninstall FPS-config" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FP_Uninstall_Click(object sender, EventArgs e)
        {
            try
            {
                // Asking for confirmation...
                if (MessageBox.Show(AppStrings.FP_UninstallQuestion, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    // If the game is using custom user directory and it exists, silently removing its contents...
                    if (App.SourceGames[AppSelector.Text].IsUsingUserDir)
                    {
                        // Generating full path...
                        string CfgPath = Path.Combine(App.SourceGames[AppSelector.Text].CFGMan.FPSConfigInstallPath, App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].InstallDir);

                        // Removing files...
                        GuiHelpers.FormShowRemoveFiles(CfgPath);

                        // Showing message...
                        MessageBox.Show(AppStrings.FP_RemoveSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Showing interactive dialog with detected FPS-configs...
                        GuiHelpers.FormShowCleanup(App.SourceGames[AppSelector.Text].FPSConfigs, ((Button)sender).Text.ToLower(), AppStrings.FP_RemoveSuccessful, App.SourceGames[AppSelector.Text].FullBackUpDirPath, App.SourceGames[AppSelector.Text].GameBinaryFile, false, false, false, Properties.Settings.Default.SafeCleanup);
                    }

                    // Changing the state of some controls...
                    SetFPSButtons(CheckIfFPSConfigInstalled());
                    HandleConfigs();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.FP_RemoveFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(Ex, DebugStrings.AppDbgExFpsUninstall);
            }
        }

        /// <summary>
        /// "FPS-config warning" icon click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void GT_Warning_Click(object sender, EventArgs e)
        {
            try
            {
                EditFPSConfig(GuiHelpers.FormShowCfgSelect(App.SourceGames[AppSelector.Text].FPSConfigs), ModifierKeys == Keys.Shift);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.CS_FailedToOpenCfg, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExCfgSelection);
            }
        }

        /// <summary>
        /// "New config" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CE_New_Click(object sender, EventArgs e)
        {
            CloseEditorConfigs();
            UpdateStatusBar();
        }

        /// <summary>
        /// "Open config" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CE_Open_Click(object sender, EventArgs e)
        {
            CE_OpenCfgDialog.InitialDirectory = App.SourceGames[AppSelector.Text].FullCfgPath;

            if (CE_OpenCfgDialog.ShowDialog() == DialogResult.OK)
            {
                ReadConfigFromFile(CE_OpenCfgDialog.FileName);
            }
        }

        /// <summary>
        /// "Save config" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CE_Save_Click(object sender, EventArgs e)
        {
            CE_SaveCfgDialog.InitialDirectory = App.SourceGames[AppSelector.Text].FullCfgPath;

            if (!String.IsNullOrEmpty(CFGFileName))
            {
                if (Properties.Settings.Default.SafeCleanup)
                {
                    if (File.Exists(CFGFileName))
                    {
                        try
                        {
                            FileManager.CreateConfigBackUp(CFGFileName, App.SourceGames[AppSelector.Text].FullBackUpDirPath, Properties.Resources.BU_PrefixCfg);
                        }
                        catch (Exception Ex)
                        {
                            Logger.Warn(Ex, DebugStrings.AppDbgExCfgEdAutoBackup);
                        }
                    }
                }

                try
                {
                    WriteTableToFileNow(CFGFileName);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.CE_CfgSVVEx, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExCfgEdSave);
                }
            }
            else
            {
                CE_SaveCfgDialog.FileName = File.Exists(Path.Combine(App.SourceGames[AppSelector.Text].FullCfgPath, "autoexec.cfg")) ? AppStrings.UnnamedFileName : "autoexec.cfg";

                if (CE_SaveCfgDialog.ShowDialog() == DialogResult.OK)
                {
                    WriteTableToFileNow(CE_SaveCfgDialog.FileName);
                    CFGFileName = CE_SaveCfgDialog.FileName;
                    UpdateStatusBar();
                }
            }
        }

        /// <summary>
        /// "Save config as" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CE_SaveAs_Click(object sender, EventArgs e)
        {
            CE_SaveCfgDialog.InitialDirectory = App.SourceGames[AppSelector.Text].FullCfgPath;

            if (CE_SaveCfgDialog.ShowDialog() == DialogResult.OK)
            {
                WriteTableToFileNow(CE_SaveCfgDialog.FileName);
            }
        }

        /// <summary>
        /// "Clean custom maps" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_RemCustMaps_Click(object sender, EventArgs e)
        {
            StartCleanup("0", ((Button)sender).Text);
        }

        /// <summary>
        /// "Clean download cache" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_RemDnlCache_Click(object sender, EventArgs e)
        {
            StartCleanup("1", ((Button)sender).Text);
        }

        /// <summary>
        /// "Clean sound cache" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_RemSoundCache_Click(object sender, EventArgs e)
        {
            StartCleanup("2", ((Button)sender).Text);
        }

        /// <summary>
        /// "Clean screenshots" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_RemScreenShots_Click(object sender, EventArgs e)
        {
            StartCleanup("3", ((Button)sender).Text);
        }

        /// <summary>
        /// "Clean recorded demos" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_RemDemos_Click(object sender, EventArgs e)
        {
            StartCleanup("4", ((Button)sender).Text);
        }

        /// <summary>
        /// "Clean game configs" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_RemGameOpts_Click(object sender, EventArgs e)
        {
            // Creating list with candidates...
            List<String> CleanDirs = new List<string>();

            // Asking for confirmation...
            if (MessageBox.Show(String.Format(AppStrings.AppQuestionTemplate, ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    // Removing video settings...
                    if (!App.SourceGames[AppSelector.Text].IsUsingVideoFile)
                    {
                        string GameRegKey = Type1Video.GetGameRegKey(App.SourceGames[AppSelector.Text].SmallAppName);

                        if (Properties.Settings.Default.SafeCleanup)
                        {
                            try
                            {
                                Type1Video.BackUpVideoSettings(GameRegKey, "Game_AutoBackUp", App.SourceGames[AppSelector.Text].FullBackUpDirPath);
                            }
                            catch (Exception Ex) { Logger.Warn(Ex); }
                        }

                        Type1Video.RemoveRegKey(GameRegKey);
                    }
                    else
                    {
                        if (Properties.Settings.Default.SafeCleanup)
                        {
                            try
                            {
                                FileManager.CreateConfigBackUp(App.SourceGames[AppSelector.Text].VideoCfgFiles, App.SourceGames[AppSelector.Text].FullBackUpDirPath, Properties.Resources.BU_PrefixVidAuto);
                            }
                            catch (Exception Ex)
                            {
                                Logger.Warn(Ex, DebugStrings.AppDbgExRemVdAutoGs);
                            }
                        }

                        CleanDirs.AddRange(App.SourceGames[AppSelector.Text].VideoCfgFiles);
                    }

                    // Creating backup...
                    if (Properties.Settings.Default.SafeCleanup)
                    {
                        try
                        {
                            FileManager.CreateConfigBackUp(App.SourceGames[AppSelector.Text].CloudConfigs, App.SourceGames[AppSelector.Text].FullBackUpDirPath, Properties.Resources.BU_PrefixCfg);
                        }
                        catch (Exception Ex)
                        {
                            Logger.Warn(Ex, DebugStrings.AppDbgExRemVdAutoCfg);
                        }
                    }

                    // Adding configs to list...
                    CleanDirs.Add(Path.Combine(App.SourceGames[AppSelector.Text].FullCfgPath, "config.cfg"));
                    CleanDirs.AddRange(App.SourceGames[AppSelector.Text].CloudConfigs);

                    // Removing all candidates...
                    GuiHelpers.FormShowRemoveFiles(CleanDirs);
                    MessageBox.Show(AppStrings.PS_CleanupSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.PS_CleanupErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExRemVd);
                }
            }
        }

        /// <summary>
        /// "Clean old binaries" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_RemOldBin_Click(object sender, EventArgs e)
        {
            StartCleanup("5", ((Button)sender).Text);
        }

        /// <summary>
        /// "Verify integrity of game cache" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_CheckCache_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format(AppStrings.AppQuestionTemplate, ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    Process.Start(String.Format("steam://validate/{0}", App.SourceGames[AppSelector.Text].GameInternalID));
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.AppStartSteamFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExValCache);
                }
            }
        }

        /// <summary>
        /// "Open Reporter" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUReportBuilder_Click(object sender, EventArgs e)
        {
            if ((AppSelector.Items.Count > 0) && (AppSelector.SelectedIndex != -1))
            {
                GuiHelpers.FormShowRepBuilder(App.AppUserDir, App.SteamClient.FullSteamPath, App.SourceGames[AppSelector.Text]);
            }
            else
            {
                MessageBox.Show(AppStrings.AppNoGamesSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Open Installer" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUInstaller_Click(object sender, EventArgs e)
        {
            GuiHelpers.FormShowInstaller(App.SourceGames[AppSelector.Text].FullGamePath, App.SourceGames[AppSelector.Text].IsUsingUserDir, App.SourceGames[AppSelector.Text].CustomInstallDir);
        }

        /// <summary>
        /// "Quit application" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(ReturnCodes.Success);
        }

        /// <summary>
        /// "About" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUAbout_Click(object sender, EventArgs e)
        {
            GuiHelpers.FormShowAboutApp();
        }

        /// <summary>
        /// "Report bug" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUReportBug_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessManager.OpenWebPage(Properties.Resources.AppBtURL);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExBugRep);
            }
        }

        /// <summary>
        /// "Refresh backups" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BUT_Refresh_Click(object sender, EventArgs e)
        {
            UpdateBackUpList();
        }

        /// <summary>
        /// "Restore backup" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BUT_RestoreB_Click(object sender, EventArgs e)
        {
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    if (MessageBox.Show(AppStrings.BU_QMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        foreach (ListViewItem BU_Item in BU_LVTable.SelectedItems)
                        {
                            switch (Path.GetExtension(BU_Item.SubItems[4].Text))
                            {
                                case ".reg": // Registry file...
                                    try
                                    {
                                        Process.Start("regedit.exe", String.Format("/s \"{0}\"", Path.Combine(App.SourceGames[AppSelector.Text].FullBackUpDirPath, BU_Item.SubItems[4].Text)));
                                    }
                                    catch (Exception Ex)
                                    {
                                        MessageBox.Show(AppStrings.BU_RestFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        Logger.Error(Ex, DebugStrings.AppDbgExRegedit);
                                    }
                                    break;
                                case ".bud": // Standard archive...
                                    GuiHelpers.FormShowArchiveExtract(Path.Combine(App.SourceGames[AppSelector.Text].FullBackUpDirPath, BU_Item.SubItems[4].Text), Path.GetPathRoot(App.SourceGames[AppSelector.Text].FullGamePath));
                                    HandleConfigs();
                                    break;
                                default: // Unknown type...
                                    MessageBox.Show(AppStrings.BU_UnknownType, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    break;
                            }
                            MessageBox.Show(AppStrings.BU_RestSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.BU_NoSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.BU_NoFiles, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Delete backup" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BUT_DelB_Click(object sender, EventArgs e)
        {
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    if (MessageBox.Show(AppStrings.BU_DelMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        foreach (ListViewItem BU_Item in BU_LVTable.SelectedItems)
                        {
                            try
                            {
                                File.Delete(Path.Combine(App.SourceGames[AppSelector.Text].FullBackUpDirPath, BU_Item.SubItems[4].Text));
                                BU_LVTable.Items.Remove(BU_Item);
                            }
                            catch (Exception Ex)
                            {
                                MessageBox.Show(AppStrings.BU_DelFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                Logger.Error(Ex, DebugStrings.AppDbgExBackupRem);
                            }
                        }
                        MessageBox.Show(AppStrings.BU_DelSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.BU_NoSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.BU_NoFiles, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Create backup" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BUT_CrBkupReg_ButtonClick(object sender, EventArgs e)
        {
            BUT_CrBkupReg.ShowDropDown();
        }

        /// <summary>
        /// "Create game settings backup" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BUT_L_GameSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppStrings.BU_RegCreate, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (!App.SourceGames[AppSelector.Text].IsUsingVideoFile)
                    {
                        Type1Video.BackUpVideoSettings(Type1Video.GetGameRegKey(App.SourceGames[AppSelector.Text].SmallAppName), "Game_Options", App.SourceGames[AppSelector.Text].FullBackUpDirPath);
                    }
                    else
                    {
                        try
                        {
                            FileManager.CreateConfigBackUp(App.SourceGames[AppSelector.Text].VideoCfgFiles, App.SourceGames[AppSelector.Text].FullBackUpDirPath, Properties.Resources.BU_PrefixVideo);
                        }
                        catch (Exception Ex)
                        {
                            Logger.Warn(Ex, DebugStrings.AppDbgExBkGsAuto);
                        }
                    }
                    MessageBox.Show(AppStrings.BU_RegDone, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateBackUpList();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.BU_RegErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExBkSg);
                }
            }
        }

        /// <summary>
        /// "Create Steam registry settings backup" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BUT_L_AllSteam_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppStrings.BU_RegCreate, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Type1Video.CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve"), "Steam_BackUp", App.SourceGames[AppSelector.Text].FullBackUpDirPath);
                    MessageBox.Show(AppStrings.BU_RegDone, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateBackUpList();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.BU_RegErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExBkAllStm);
                }
            }
        }

        /// <summary>
        /// "Create all Source games setting backup" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BUT_L_AllSRC_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppStrings.BU_RegCreate, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Type1Video.CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve", "Source"), "Source_Options", App.SourceGames[AppSelector.Text].FullBackUpDirPath);
                    MessageBox.Show(AppStrings.BU_RegDone, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateBackUpList();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.BU_RegErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExBkAllGames);
                }
            }
        }

        /// <summary>
        /// "Current tab changed" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStatusBar();
        }

        /// <summary>
        /// "Show Cvar description" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CE_ShowHint_Click(object sender, EventArgs e)
        {
            try
            {
                string Buf = CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[0].Value.ToString();
                if (!String.IsNullOrEmpty(Buf))
                {
                    Buf = CvarFetcher.GetString(Buf);
                    if (!String.IsNullOrEmpty(Buf))
                    {
                        MessageBox.Show(Buf, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(AppStrings.CE_ClNoDescr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.CE_ClSelErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExCvarDescFetchFailure);
                MessageBox.Show(AppStrings.CE_ClSelErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Show help system" menu click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUHelp_Click(object sender, EventArgs e)
        {
            string CHMFile = Path.Combine(App.FullAppPath, "help", String.Format(Properties.Resources.AppHelpFileName, AppStrings.AppLangPrefix));

            if (File.Exists(CHMFile))
            {
                Help.ShowHelp(this, CHMFile, HelpNavigator.Topic, GetHelpWebPage(MainTabControl.SelectedIndex));
            }
            else
            {
                MessageBox.Show(AppStrings.AppHelpCHMNotFound, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Add review" menu click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUOpinion_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessManager.OpenWebPage(Properties.Resources.AppURLReply);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExUrlHome);
            }
        }

        /// <summary>
        /// "Show Steam group" menu click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUSteamGroup_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Properties.Resources.AppURLSteamGrID);
            }
            catch
            {
                try
                {
                    ProcessManager.OpenWebPage(Properties.Resources.AppURLSteamGroup);
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, DebugStrings.AppDbgExUrlGroup);
                }
            }
        }

        /// <summary>
        /// "Remove rows" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CE_RmRow_Click(object sender, EventArgs e)
        {
            try
            {
                if (CE_Editor.Rows.Count > 0)
                {
                    CE_Editor.Rows.Remove(CE_Editor.CurrentRow);
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExCfgEdRemRow);
            }
        }

        /// <summary>
        /// "Copy selected rows" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CE_Copy_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                foreach (DataGridViewCell DV in CE_Editor.SelectedCells)
                {
                    if (DV.Value != null)
                    {
                        SB.AppendFormat("{0} ", DV.Value);
                    }
                }
                Clipboard.SetText(SB.ToString().Trim());
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExCfgEdCopy);
            }
        }

        /// <summary>
        /// "Cut selected rows" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CE_Cut_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                foreach (DataGridViewCell DV in CE_Editor.SelectedCells)
                {
                    if (DV.Value != null)
                    {
                        SB.AppendFormat("{0} ", DV.Value);
                        DV.Value = null;
                    }
                }
                Clipboard.SetText(SB.ToString().Trim());
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExCfgEdCut);
            }
        }

        /// <summary>
        /// "Paste from clipboard" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CE_Paste_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value = Clipboard.GetText();
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExCfgEdPaste);
            }
        }

        /// <summary>
        /// "Load FPS-config to editor" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FP_OpenNotepad_Click(object sender, EventArgs e)
        {
            try
            {
                // Saving state of current ModifierKeys before loading new form...
                Keys MfKeysState = ModifierKeys;

                // Generating full path to installed FPS-config...
                string ConfigFile = App.SourceGames[AppSelector.Text].IsUsingUserDir ? Path.Combine(App.SourceGames[AppSelector.Text].CFGMan.FPSConfigInstallPath, App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].InstallDir, "cfg", "autoexec.cfg") : GuiHelpers.FormShowCfgSelect(App.SourceGames[AppSelector.Text].FPSConfigs);

                // Loading FPS-config to Notepad or built-in Config Editor...
                EditFPSConfig(ConfigFile, MfKeysState != Keys.Shift);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.FP_EditorLoadFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExEditCfg);
            }
        }

        /// <summary>
        /// "Open Updater" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUUpdateCheck_Click(object sender, EventArgs e)
        {
            GuiHelpers.FormShowUpdater(App.UserAgent, App.FullAppPath, App.AppUserDir, App.Platform);
            FindGames();
        }

        /// <summary>
        /// "Load backup in text editor" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BUT_OpenNpad_Click(object sender, EventArgs e)
        {
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    if (Regex.IsMatch(Path.GetExtension(BU_LVTable.SelectedItems[0].SubItems[4].Text), @"\.(txt|cfg|[0-9]|reg)"))
                    {
                        try
                        {
                            ProcessManager.OpenTextEditor(Path.Combine(App.SourceGames[AppSelector.Text].FullBackUpDirPath, BU_LVTable.SelectedItems[0].SubItems[4].Text), Properties.Settings.Default.EditorBin, App.Platform.OS);
                        }
                        catch (Exception Ex)
                        {
                            Logger.Warn(Ex, DebugStrings.AppDbgExBkExtEdt);
                        }
                    }
                    else
                    {
                        MessageBox.Show(AppStrings.BU_BinaryFile, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.BU_NoSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.BU_NoFiles, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Open Settings" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUAppOptions_Click(object sender, EventArgs e)
        {
            GuiHelpers.FormShowOptions();
        }

        /// <summary>
        /// "Config editor table resized" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BU_LVTable_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            // Blocking resizing...
            e.NewWidth = BU_LVTable.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        /// <summary>
        /// "Show selected backup file in shell" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BUT_ExploreBUp_Click(object sender, EventArgs e)
        {
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    try
                    {
                        ProcessManager.OpenExplorer(Path.Combine(App.SourceGames[AppSelector.Text].FullBackUpDirPath, BU_LVTable.SelectedItems[0].SubItems[4].Text), App.Platform.OS);
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, DebugStrings.AppDbgExBkFMan);
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.BU_NoSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.BU_NoFiles, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Form closed" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmMainW_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Saving application settings...
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// "Open Keyboard buttons disabler" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUWinMnuDisabler_Click(object sender, EventArgs e)
        {
            GuiHelpers.FormShowKBHelper();
        }

        /// <summary>
        /// "Load config in text editor" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CE_OpenInNotepad_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(CFGFileName))
            {
                try
                {
                    ProcessManager.OpenTextEditor(CFGFileName, Properties.Settings.Default.EditorBin, App.Platform.OS);
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, DebugStrings.AppDbgExCfgEdExtEdt);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.CE_NoFileOpened, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Restricted symbols in Steam path detector" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_PathDetector_Click(object sender, EventArgs e)
        {
            if (((Label)sender).ForeColor == Color.Red)
            {
                MessageBox.Show(AppStrings.SteamNonASCIIDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(AppStrings.SteamNonASCIINotDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// "Restricted symbols in game path detector" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_PathGame_Click(object sender, EventArgs e)
        {
            if (((Label)sender).ForeColor == Color.Red) { MessageBox.Show(AppStrings.GameNonASCIIDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); } else { MessageBox.Show(AppStrings.GameNonASCIINotDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        /// <summary>
        /// "Clean saved replays" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_RemReplays_Click(object sender, EventArgs e)
        {
            StartCleanup("6", ((Button)sender).Text);
        }

        /// <summary>
        /// "Clean custom models and textures" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_RemTextures_Click(object sender, EventArgs e)
        {
            StartCleanup("7", ((Button)sender).Text);
        }

        /// <summary>
        /// "Clean secondary cache contents" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_RemSecndCache_Click(object sender, EventArgs e)
        {
            StartCleanup("8", ((Button)sender).Text);
        }

        /// <summary>
        /// "Safe Clean status" icon click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void SB_App_DoubleClick(object sender, EventArgs e)
        {
            // Changing Safe Clean status...
            Properties.Settings.Default.SafeCleanup = !Properties.Settings.Default.SafeCleanup;
            
            // Showing message about consequences of disabling...
            if (!Properties.Settings.Default.SafeCleanup)
            {
                MessageBox.Show(AppStrings.AppSafeClnDisabled, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            // Updating status bar...
            CheckSafeClnStatus();
        }

        /// <summary>
        /// "Open list of console variables" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CE_OpenCVList_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessManager.OpenWebPage(AppStrings.AppCVListURL);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExUrlCvList);
            }
        }

        /// <summary>
        /// "Create backup of current config" toolbar button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CE_ManualBackUpCfg_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(CFGFileName)))
            {
                if (File.Exists(CFGFileName))
                {
                    try
                    {
                        FileManager.CreateConfigBackUp(CFGFileName, App.SourceGames[AppSelector.Text].FullBackUpDirPath, Properties.Resources.BU_PrefixCfg);
                        MessageBox.Show(String.Format(AppStrings.CE_BackUpCreated, Path.GetFileName(CFGFileName)), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, DebugStrings.AppDbgExCfgEdBkMan);
                    }
                }
            }
            else
            {
                MessageBox.Show(AppStrings.CE_NoFileOpened, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Clean custom sounds" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_RemSounds_Click(object sender, EventArgs e)
        {
            StartCleanup("9", ((Button)sender).Text);
        }

        /// <summary>
        /// "Clean custom directory contents" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_RemCustDir_Click(object sender, EventArgs e)
        {
            StartCleanup("10", ((Button)sender).Text);
        }

        /// <summary>
        /// "Execute deep cleanup" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_DeepCleanup_Click(object sender, EventArgs e)
        {
            List<String> CleanDirs = new List<String>(App.SourceGames[AppSelector.Text].CloudConfigs);
            if (App.SourceGames[AppSelector.Text].IsUsingVideoFile)
            {
                CleanDirs.AddRange(App.SourceGames[AppSelector.Text].VideoCfgFiles);
            }
            StartCleanup("11", ((Button)sender).Text, CleanDirs);
        }

        /// <summary>
        /// "Clean FPS-configs" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void PS_RemConfigs_Click(object sender, EventArgs e)
        {
            GuiHelpers.FormShowCleanup(App.SourceGames[AppSelector.Text].FPSConfigs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, App.SourceGames[AppSelector.Text].FullBackUpDirPath, App.SourceGames[AppSelector.Text].GameBinaryFile, false, false, false, Properties.Settings.Default.SafeCleanup);
        }

        /// <summary>
        /// "Selected HUD changed" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void HD_HSel_SelectedIndexChanged(object sender, EventArgs e)
        {                
            // Checking result...
            bool Success = !String.IsNullOrEmpty(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].Name);

            // Changing some controls state...
            HD_GB_Pbx.Image = Properties.Resources.LoadingFile;
            HD_Install.Enabled = Success;
            HD_Homepage.Enabled = Success;
            HD_Warning.Visible = Success && !App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].IsUpdated;

            // Adding information about last update...
            HD_LastUpdate.Visible = Success;
            if (Success)
            {
                HD_LastUpdate.Text = String.Format(AppStrings.HD_LastUpdateInfo, FileManager.Unix2DateTime(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].LastUpdate).ToLocalTime());
            }

            // Checking if selected HUD is installed...
            SetHUDButtons(HUDManager.CheckInstalledHUD(App.SourceGames[AppSelector.Text].CustomInstallDir, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].InstallDir));

            // Downloading screenshot...
            if (Success && !BW_HUDScreen.IsBusy)
            {
                BW_HUDScreen.RunWorkerAsync(argument: new List<String> { AppSelector.Text, HD_HSel.Text });
            }
        }

        /// <summary>
        /// "Install selected HUD" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void HD_Install_Click(object sender, EventArgs e)
        {
            if (!HUDManager.CheckHUDDatabase(Properties.Settings.Default.LastHUDTime))
            {
                if (App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].IsUpdated)
                {
                    if (MessageBox.Show(String.Format("{0}?", ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Downloading HUD archive...
                        GuiHelpers.FormShowDownloader(Properties.Settings.Default.HUDUseUpstream ? App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].UpURI : App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].URI, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].LocalFile);

                        // Checking if downloaded archive exists...
                        if (File.Exists(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].LocalFile))
                        {
                            // Checking hash of downloaded file...
                            if (Properties.Settings.Default.HUDUseUpstream || FileManager.CalculateFileSHA512(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].LocalFile) == App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].FileHash)
                            {
                                // Checking if selected HUD is installed...
                                if (HUDManager.CheckInstalledHUD(App.SourceGames[AppSelector.Text].CustomInstallDir, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].InstallDir))
                                {
                                    // Removing installed files...
                                    GuiHelpers.FormShowRemoveFiles(Path.Combine(App.SourceGames[AppSelector.Text].CustomInstallDir, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].InstallDir));
                                }

                                // Extracting downloaded archove...
                                GuiHelpers.FormShowArchiveExtract(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].LocalFile, Path.Combine(App.SourceGames[AppSelector.Text].CustomInstallDir, "hudtemp"));

                                // Installing files in a separate thread...
                                ((Button)sender).Enabled = false;
                                if (!BW_HudInstall.IsBusy)
                                {
                                    BW_HudInstall.RunWorkerAsync(argument: new List<String> { AppSelector.Text, HD_HSel.Text });
                                }
                            }
                            else
                            {
                                // Hash missmatch. Show error...
                                MessageBox.Show(AppStrings.HD_HashError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            // Removing downloaded file...
                            try
                            {
                                File.Delete(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].LocalFile);
                            }
                            catch (Exception Ex)
                            {
                                Logger.Warn(Ex, DebugStrings.AppDbgExHudArchRem);
                            }
                        }
                        else
                        {
                            MessageBox.Show(AppStrings.HD_DownloadError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.HD_Outdated, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.HD_DbOutdated, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Uninstall selected HUD" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void HD_Uninstall_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format("{0}?", ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Generating full path...
                string HUDPath = Path.Combine(App.SourceGames[AppSelector.Text].CustomInstallDir, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].InstallDir);

                // Removing HUD files...
                GuiHelpers.FormShowRemoveFiles(HUDPath);

                // Checking if HUD installed...
                bool IsInstalled = HUDManager.CheckInstalledHUD(App.SourceGames[AppSelector.Text].CustomInstallDir, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].InstallDir);

                // Showing message and removing empty directory...
                if (!IsInstalled)
                {
                    MessageBox.Show(AppStrings.PS_CleanupSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Changing the state of some controls...
                SetHUDButtons(IsInstalled);
            }
        }

        /// <summary>
        /// "Visit HUD's homepage" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void HD_Homepage_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].Site))
            {
                try
                {
                    ProcessManager.OpenWebPage(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].Site);
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, DebugStrings.AppDbgExUrlHudHome);
                }
            }
        }

        /// <summary>
        /// "Clean downloaded by application files" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUExtClnAppCache_Click(object sender, EventArgs e)
        {
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(App.AppUserDir, StringsManager.HudDirectoryName, "*.*"),
                Path.Combine(App.AppUserDir, StringsManager.ConfigDirectoryName, "*.*")
            };
            GuiHelpers.FormShowCleanup(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", String.Empty), AppStrings.PS_CleanupSuccess, App.SourceGames[AppSelector.Text].FullBackUpDirPath, App.SourceGames[AppSelector.Text].GameBinaryFile);
        }

        /// <summary>
        /// "Clean system temporary files" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUExtClnTmpDir_Click(object sender, EventArgs e)
        {
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(Path.GetTempPath(), "*.*")
            };
            GuiHelpers.FormShowCleanup(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", String.Empty), AppStrings.PS_CleanupSuccess, App.SourceGames[AppSelector.Text].FullBackUpDirPath, App.SourceGames[AppSelector.Text].GameBinaryFile);
        }

        /// <summary>
        /// "Show Log viewer" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUShowLog_Click(object sender, EventArgs e)
        {
            string DFile = CurrentApp.LogFileName;
            if (File.Exists(DFile))
            {
                GuiHelpers.FormShowLogViewer(DFile);
            }
            else
            {
                MessageBox.Show(AppStrings.AppNoDebugFile, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Outdated HUD warning" icon click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void HD_Warning_Click(object sender, EventArgs e)
        {
            if (!App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].IsUpdated)
            {
                MessageBox.Show(AppStrings.HD_NotTested, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Show HUD files in shell" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void HD_OpenDir_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessManager.OpenExplorer(Path.Combine(App.SourceGames[AppSelector.Text].CustomInstallDir, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].InstallDir), App.Platform.OS);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExHudExtFm);
            }
        }

        /// <summary>
        /// "Open Steam cache cleaner" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUExtClnSteam_Click(object sender, EventArgs e)
        {
            GuiHelpers.FormShowStmCleaner(App.SteamClient.FullSteamPath, App.SourceGames[AppSelector.Text].FullBackUpDirPath, App.Platform.SteamAppsFolderName, App.Platform.SteamProcName);
        }

        /// <summary>
        /// "Open Muted players manager" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MNUMuteMan_Click(object sender, EventArgs e)
        {
            GuiHelpers.FormShowMuteManager(App.SourceGames[AppSelector.Text].GetActualBanlistFile(), App.SourceGames[AppSelector.Text].FullBackUpDirPath);
        }

        /// <summary>
        /// "Current Steam UserID" status bar click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void SB_SteamID_Click(object sender, EventArgs e)
        {
            try
            {
                string Result = GuiHelpers.FormShowIDSelect(App.SteamClient.SteamIDs);

                if (!String.IsNullOrWhiteSpace(Result))
                {
                    SB_SteamID.Text = Result;
                    Properties.Settings.Default.LastSteamID = Result;
                    FindGames();
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExUserIdSel);
            }
        }

        /// <summary>
        /// "Backup file selected" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BU_LVTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Blocking some buttons if user selected more than one backup file...
            bool IsSingle = BU_LVTable.SelectedItems.Count <= 1;
            BUT_OpenNpad.Enabled = IsSingle;
            BUT_ExploreBUp.Enabled = IsSingle;
        }
    }
}
