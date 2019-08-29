/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2019 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2019 EasyCoding Team.
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
            if (Properties.Settings.Default.CallUpgrade)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.CallUpgrade = false;
            }
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

            // Disable "Open in Notepad" button...
            FP_OpenNotepad.Enabled = false;

            // Disable controls on FPS-config tab...
            FP_Install.Enabled = false;
            FP_Comp.Visible = false;

            // Disable controls on HUD Manager tab...
            HD_Install.Enabled = false;
            HD_Homepage.Enabled = false;
            HD_Uninstall.Enabled = false;
            HD_OpenDir.Enabled = false;
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
        /// Handles with installed FPS-configs, shows special icon and works
        /// with "Uninstall config" button.
        /// </summary>
        private void HandleConfigs()
        {
            App.SourceGames[AppSelector.Text].FPSConfigs = FileManager.ExpandFileList(ConfigManager.ListFPSConfigs(App.SourceGames[AppSelector.Text].FullGamePath, App.SourceGames[AppSelector.Text].IsUsingUserDir), true);
            GT_Warning.Visible = App.SourceGames[AppSelector.Text].FPSConfigs.Count > 0;
            FP_Uninstall.Enabled = App.SourceGames[AppSelector.Text].FPSConfigs.Count > 0;
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
        /// Converts a single string to list.
        /// </summary>
        /// <param name="Str">Source string.</param>
        /// <returns>List with source string.</returns>
        private List<String> SingleToArray(string Str)
        {
            return new List<String> { Str };
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
            App.SourceGames[(string)e.Argument].CFGMan = new ConfigManager(App.FullAppPath, AppStrings.AppLangPrefix);
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
                    FP_ConfigSel.Enabled = true;
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

        private void FrmMainW_Load(object sender, EventArgs e)
        {
            // Событие инициализации формы...
            App = new CurrentApp(Properties.Settings.Default.IsPortable, Properties.Resources.AppName);

            // Узнаем путь к установленному клиенту Steam...
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
            
            // Начинаем платформо-зависимые процедуры...
            ChangePrvControlState(ProcessManager.IsCurrentUserAdmin());

            // Сохраним последний путь к Steam в файл конфигурации...
            Properties.Settings.Default.LastSteamPath = App.SteamClient.FullSteamPath;

            // Вставляем информацию о версии в заголовок формы...
            Text = String.Format(Text, Properties.Resources.AppName, App.Platform.OSFriendlyName, CurrentApp.AppVersion);

            // Укажем статус Безопасной очистки...
            CheckSafeClnStatus();

            // Укажем путь к Steam на странице "Устранение проблем"...
            PS_StPath.Text = String.Format(PS_StPath.Text, App.SteamClient.FullSteamPath);
            
            // Проверим на наличие запрещённых символов в пути к установленному клиенту Steam...
            CheckSymbolsSteam();

            // Запустим поиск установленных игр и проверим нашлось ли что-то...
            FindGames();

            // Проверим наличие обновлений программы...
            CheckForUpdates();
        }

        private void PS_CleanBlobs_CheckedChanged(object sender, EventArgs e)
        {
            // Управляем доступностью кнопки запуска очистки...
            PS_ExecuteNow.Enabled = PS_CleanBlobs.Checked || PS_CleanRegistry.Checked;
        }

        private void PS_CleanRegistry_CheckedChanged(object sender, EventArgs e)
        {
            // Включаем список с доступными языками клиента Steam...
            PS_SteamLang.Enabled = PS_CleanRegistry.Checked;

            // Получим значение текущего языка из реестра Windows...
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
                // Выбираем язык по умолчанию согласно языку приложения...
                PS_SteamLang.SelectedIndex = Convert.ToInt32(AppStrings.AppDefaultSteamLangID);
            }

            // Управляем доступностью кнопки запуска очистки...
            PS_ExecuteNow.Enabled = PS_CleanRegistry.Checked || PS_CleanBlobs.Checked;
        }

        private void PS_ExecuteNow_Click(object sender, EventArgs e)
        {
            // Начинаем очистку...

            // Запрашиваем подтверждение у пользователя...
            if (MessageBox.Show(AppStrings.PS_ExecuteMSG, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Подтверждение получено...
                if ((PS_CleanBlobs.Checked) || (PS_CleanRegistry.Checked))
                {
                    // Найдём и завершим работу клиента Steam...
                    if (ProcessManager.ProcessTerminate("Steam") != 0)
                    {
                        MessageBox.Show(AppStrings.PS_ProcessDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Проверяем нужно ли чистить блобы...
                    if (PS_CleanBlobs.Checked)
                    {
                        try
                        {
                            // Чистим блобы...
                            App.SteamClient.CleanBlobsNow();
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show(AppStrings.PS_CleanException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Logger.Error(Ex, DebugStrings.AppDbgExClnBlobs);
                        }
                    }

                    // Проверяем нужно ли чистить реестр...
                    if (PS_CleanRegistry.Checked)
                    {
                        try
                        {
                            // Проверяем выбрал ли пользователь язык из выпадающего списка...
                            if (PS_SteamLang.SelectedIndex != -1)
                            {
                                // Всё в порядке, чистим реестр...
                                App.SteamClient.CleanRegistryNow(PS_SteamLang.SelectedIndex);
                            }
                            else
                            {
                                // Пользователь не выбрал язык, поэтому будем использовать английский...
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

                    // Выполнение закончено, выведем сообщение о завершении...
                    MessageBox.Show(AppStrings.PS_SeqCompleted, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Запустим Steam...
                    if (File.Exists(Path.Combine(App.SteamClient.FullSteamPath, App.Platform.SteamBinaryName))) { Process.Start(Path.Combine(App.SteamClient.FullSteamPath, App.Platform.SteamBinaryName)); }
                }
            }
        }

        private void FrmMainW_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Запрашиваем подтверждение у пользователя на закрытие формы...
                e.Cancel = ((Properties.Settings.Default.ConfirmExit && !(MessageBox.Show(String.Format(AppStrings.FrmCloseQuery, Properties.Resources.AppName), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)) || (BW_BkUpRecv.IsBusy || BW_FPRecv.IsBusy || BW_HudInstall.IsBusy || BW_HUDList.IsBusy || BW_HUDScreen.IsBusy || BW_UpChk.IsBusy || BW_ClnList.IsBusy));
            }
        }

        private void AppSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Переключаем состояние некоторых контролов...
                HandleControlsOnSelGame();
                
                // Проверим наличие запрещённых символов в пути...
                CheckSymbolsGame();

                // Распознаем файловую систему на диске с игрой...
                DetectFS();

                // Считаем настройки графики...
                try { LoadGraphicSettings(); } catch (NotSupportedException) { MessageBox.Show(AppStrings.AppIncorrectSrcVersion, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); }

                // Получаем текущий SteamID...
                HandleSteamIDs(Properties.Settings.Default.LastSteamID);

                // Проверим, установлен ли FPS-конфиг...
                HandleConfigs();

                // Закроем открытые конфиги в редакторе...
                if (!(String.IsNullOrEmpty(CFGFileName))) { CloseEditorConfigs(); }

                // Считаем имеющиеся FPS-конфиги...
                if (!BW_FPRecv.IsBusy) { BW_FPRecv.RunWorkerAsync(AppSelector.Text); }

                // Обновляем статус...
                UpdateStatusBar();

                // Сохраним ID последней выбранной игры...
                Properties.Settings.Default.LastGameName = AppSelector.Text;

                // Переключаем вид страницы менеджера HUD...
                HandleHUDMode(App.SourceGames[AppSelector.Text].IsHUDsAvailable);

                // Считаем список доступных HUD для данной игры...
                if (App.SourceGames[AppSelector.Text].IsHUDsAvailable) { if (!BW_HUDList.IsBusy) { BW_HUDList.RunWorkerAsync(AppSelector.Text); } }

                // Считаем список бэкапов...
                UpdateBackUpList();

                // Создадим каталоги кэшей для HUD...
                if (App.SourceGames[AppSelector.Text].IsHUDsAvailable && !Directory.Exists(App.SourceGames[AppSelector.Text].AppHUDDir)) { Directory.CreateDirectory(App.SourceGames[AppSelector.Text].AppHUDDir); }

                // Checking for available cleanup targets...
                if (!BW_ClnList.IsBusy) { BW_ClnList.RunWorkerAsync(AppSelector.Text); }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.AppFailedToGetData, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExSelGame);
            }
        }

        private void AppRefresh_Click(object sender, EventArgs e)
        {
            // Попробуем обновить список игр...
            FindGames();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Максимальное качество".
        /// Устанавливает графические настройки на рекомендуемый максимум.
        /// </summary>
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
        /// Обработчик нажатия кнопки "Максимальная производительность".
        /// Устанавливает графические настройки на рекомендуемый минимум.
        /// </summary>
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
        /// Обработчик нажатия кнопки сохранения графических настроек.
        /// </summary>
        private void GT_SaveApply_Click(object sender, EventArgs e)
        {
            // Сохраняем изменения в графических настройках...
            if (ValidateGameSettings())
            {
                // Запрашиваем подтверждение у пользователя...
                if (MessageBox.Show(AppStrings.GT_SaveMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Сохраняем настройки графики...
                    WriteGraphicSettings();
                }
            }
            else
            {
                // Пользователь заполнил не все поля. Сообщаем ему об этом...
                MessageBox.Show(AppStrings.GT_NCFNReady, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FP_ConfigSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Выводим описание...
                FP_Description.Text = App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].Description;

                // Проверим совместимость конфига с игрой...
                FP_Comp.Visible = !App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].CheckCompatibility(App.SourceGames[AppSelector.Text].GameInternalID);

                // Включаем кнопку открытия конфига в Блокноте...
                FP_OpenNotepad.Enabled = true;

                // Включаем кнопку установки конфига...
                FP_Install.Enabled = true;
            }
            catch (Exception Ex)
            {
                // Не получилось загрузить описание выбранного конфига. Выведем стандартное сообщение...
                Logger.Warn(Ex);
                FP_Description.Text = AppStrings.FP_NoDescr;
            }
        }

        private void FP_Install_Click(object sender, EventArgs e)
        {
            // Начинаем устанавливать FPS-конфиг в управляемое приложение...
            if (FP_ConfigSel.SelectedIndex != -1)
            {
                if (MessageBox.Show(AppStrings.FP_InstallQuestion, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Проверим, не нужно ли создавать резервную копию...
                    if (Properties.Settings.Default.SafeCleanup)
                    {
                        // Проверяем есть ли установленные конфиги...
                        if (App.SourceGames[AppSelector.Text].FPSConfigs.Count > 0)
                        {
                            // Создаём резервную копию...
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

                    try
                    {
                        // Устанавливаем...
                        ConfigManager.InstallConfigNow(App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].FileName, App.FullAppPath, App.SourceGames[AppSelector.Text].FullGamePath, App.SourceGames[AppSelector.Text].IsUsingUserDir, Properties.Settings.Default.UserCustDirName);
                        
                        // Выводим сообщение об успешной установке...
                        MessageBox.Show(AppStrings.FP_InstallSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Перечитаем конфиги...
                        HandleConfigs();
                    }
                    catch (Exception Ex)
                    {
                        // Установка не удалась. Выводим сообщение об этом...
                        MessageBox.Show(AppStrings.FP_InstallFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Logger.Error(Ex, DebugStrings.AppDbgExFpsInstall);
                    }
                }
            }
            else
            {
                // Пользователь не выбрал конфиг. Сообщим об этом...
                MessageBox.Show(AppStrings.FP_NothingSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FP_Uninstall_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверим есть ли кандидаты на удаление...
                if (App.SourceGames[AppSelector.Text].FPSConfigs.Count > 0)
                {
                    // Удаляем конфиги...
                    GuiHelpers.FormShowCleanup(App.SourceGames[AppSelector.Text].FPSConfigs, ((Button)sender).Text.ToLower(), AppStrings.FP_RemoveSuccessful, App.SourceGames[AppSelector.Text].FullBackUpDirPath, App.SourceGames[AppSelector.Text].GameBinaryFile, false, false, false, Properties.Settings.Default.SafeCleanup);

                    // Перечитаем список конфигов...
                    HandleConfigs();
                }
                else
                {
                    MessageBox.Show(AppStrings.FP_RemoveNotExists, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.FP_RemoveFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(Ex, DebugStrings.AppDbgExFpsUninstall);
            }
        }

        private void GT_Warning_Click(object sender, EventArgs e)
        {
            try
            {
                // Предложим пользователю выбрать FPS-конфиг...
                string ConfigFile = GuiHelpers.FormShowCfgSelect(App.SourceGames[AppSelector.Text].FPSConfigs);

                // Проверим выбрал ли что-то пользователь в специальной форме...
                if (!(String.IsNullOrWhiteSpace(ConfigFile)))
                {
                    // Загрузим выбранный конфиг в Редактор конфигов...
                    ReadConfigFromFile(ConfigFile);

                    // Переключимся на него...
                    MainTabControl.SelectedIndex = 1;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.CS_FailedToOpenCfg, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExCfgSelection);
            }
        }

        private void CE_New_Click(object sender, EventArgs e)
        {
            // Закрываем все открытые конфиги в Редакторе конфигов и создаём новый пустой файл...
            CloseEditorConfigs();

            // Обновляем содержимое строки статуса...
            UpdateStatusBar();
        }

        private void CE_Open_Click(object sender, EventArgs e)
        {
            // Прочитаем конфиг и заполним его содержимым нашу таблицу редактора...
            
            // Указываем стартовый каталог в диалоге открытия файла на каталог с конфигами игры...
            CE_OpenCfgDialog.InitialDirectory = App.SourceGames[AppSelector.Text].FullCfgPath;

            // Считывает файл конфига и помещает записи в таблицу
            if (CE_OpenCfgDialog.ShowDialog() == DialogResult.OK) // Отображаем стандартный диалог открытия файла...
            {
                // Считываем...
                ReadConfigFromFile(CE_OpenCfgDialog.FileName);
            }
        }

        private void CE_Save_Click(object sender, EventArgs e)
        {
            // Указываем путь по умолчанию к конфигам управляемого приложения...
            CE_SaveCfgDialog.InitialDirectory = App.SourceGames[AppSelector.Text].FullCfgPath;

            // Проверяем, открыт ли какой-либо файл...
            if (!(String.IsNullOrEmpty(CFGFileName)))
            {
                // Будем бэкапить все файлы, сохраняемые в Редакторе...
                if (Properties.Settings.Default.SafeCleanup)
                {
                    // Создаём резервную копию...
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

                // Начинаем сохранение в тот же файл...
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
                // Зададим стандартное имя (см. issue 21)...
                CE_SaveCfgDialog.FileName = File.Exists(Path.Combine(App.SourceGames[AppSelector.Text].FullCfgPath, "autoexec.cfg")) ? AppStrings.UnnamedFileName : "autoexec.cfg";

                // Файл не был открыт. Отображаем стандартный диалог сохранения файла...
                if (CE_SaveCfgDialog.ShowDialog() == DialogResult.OK)
                {
                    WriteTableToFileNow(CE_SaveCfgDialog.FileName);
                    CFGFileName = CE_SaveCfgDialog.FileName;
                    UpdateStatusBar();
                }
            }
        }

        private void CE_SaveAs_Click(object sender, EventArgs e)
        {
            // Сохраняем файл с другим, выбранным пользователем, именем...
            CE_SaveCfgDialog.InitialDirectory = App.SourceGames[AppSelector.Text].FullCfgPath;

            // Отображаем стандартный диалог сохранения файла...
            if (CE_SaveCfgDialog.ShowDialog() == DialogResult.OK)
            {
                WriteTableToFileNow(CE_SaveCfgDialog.FileName);
            }
        }

        private void PS_RemCustMaps_Click(object sender, EventArgs e)
        {
            // Удаляем кастомные (нестандартные) карты...
            StartCleanup("0", ((Button)sender).Text);
        }

        private void PS_RemDnlCache_Click(object sender, EventArgs e)
        {
            // Удаляем кэш загрузок...
            StartCleanup("1", ((Button)sender).Text);
        }

        private void PS_RemSoundCache_Click(object sender, EventArgs e)
        {
            // Удаляем звуковой кэш...
            StartCleanup("2", ((Button)sender).Text);
        }

        private void PS_RemScreenShots_Click(object sender, EventArgs e)
        {
            // Удаляем все скриншоты...
            StartCleanup("3", ((Button)sender).Text);
        }

        private void PS_RemDemos_Click(object sender, EventArgs e)
        {
            // Удаляем все записанные демки...
            StartCleanup("4", ((Button)sender).Text);
        }

        private void PS_RemGameOpts_Click(object sender, EventArgs e)
        {
            // Создаём список файлов для удаления...
            List<String> CleanDirs = new List<string>();

            // Запрашиваем у пользователя подтверждение удаления...
            if (MessageBox.Show(String.Format(AppStrings.AppQuestionTemplate, ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    // Удаляем графические настройки...
                    if (!App.SourceGames[AppSelector.Text].IsUsingVideoFile)
                    {
                        // Получаем полный путь к ветке реестра игры...
                        string GameRegKey = Type1Video.GetGameRegKey(App.SourceGames[AppSelector.Text].SmallAppName);

                        // Создаём резервную копию куста реестра...
                        if (Properties.Settings.Default.SafeCleanup)
                        {
                            try
                            {
                                Type1Video.BackUpVideoSettings(GameRegKey, "Game_AutoBackUp", App.SourceGames[AppSelector.Text].FullBackUpDirPath);
                            }
                            catch (Exception Ex) { Logger.Warn(Ex); }
                        }

                        // Удаляем ключ HKEY_CURRENT_USER\Software\Valve\Source\tf\Settings из реестра...
                        Type1Video.RemoveRegKey(GameRegKey);
                    }
                    else
                    {
                        // Создадим бэкап файла с графическими настройками...
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

                        // Помечаем его на удаление...
                        CleanDirs.AddRange(App.SourceGames[AppSelector.Text].VideoCfgFiles);
                    }

                    // Создаём резервную копию...
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

                    // Помечаем конфиги игры на удаление...
                    CleanDirs.Add(Path.Combine(App.SourceGames[AppSelector.Text].FullCfgPath, "config.cfg"));
                    CleanDirs.AddRange(App.SourceGames[AppSelector.Text].CloudConfigs);

                    // Удаляем всю очередь...
                    GuiHelpers.FormShowRemoveFiles(CleanDirs);

                    // Выводим сообщение...
                    MessageBox.Show(AppStrings.PS_CleanupSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.PS_CleanupErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExRemVd);
                }
            }
        }

        private void PS_RemOldBin_Click(object sender, EventArgs e)
        {
            // Удаляем старые бинарники...
            StartCleanup("5", ((Button)sender).Text);
        }

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

        private void MNUReportBuilder_Click(object sender, EventArgs e)
        {
            if ((AppSelector.Items.Count > 0) && (AppSelector.SelectedIndex != -1))
            {
                // Запускаем форму создания отчёта для Техподдержки...
                GuiHelpers.FormShowRepBuilder(App.AppUserDir, App.SteamClient.FullSteamPath, App.SourceGames[AppSelector.Text]);
            }
            else
            {
                MessageBox.Show(AppStrings.AppNoGamesSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUInstaller_Click(object sender, EventArgs e)
        {
            // Запускаем форму установщика спреев, демок и конфигов...
            GuiHelpers.FormShowInstaller(App.SourceGames[AppSelector.Text].FullGamePath, App.SourceGames[AppSelector.Text].IsUsingUserDir, App.SourceGames[AppSelector.Text].CustomInstallDir);
        }

        private void MNUExit_Click(object sender, EventArgs e)
        {
            // Завершаем работу программы...
            Environment.Exit(ReturnCodes.Success);
        }

        private void MNUAbout_Click(object sender, EventArgs e)
        {
            // Отобразим форму "О программе"...
            GuiHelpers.FormShowAboutApp();
        }

        private void MNUReportBug_Click(object sender, EventArgs e)
        {
            // Перейдём в баг-трекер...
            try
            {
                ProcessManager.OpenWebPage(Properties.Resources.AppBtURL);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExBugRep);
            }
        }

        private void BUT_Refresh_Click(object sender, EventArgs e)
        {
            // Обновим список резервных копий...
            UpdateBackUpList();
        }

        private void BUT_RestoreB_Click(object sender, EventArgs e)
        {
            // Восстановим выделенный бэкап...
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    // Запрашиваем подтверждение...
                    if (MessageBox.Show(AppStrings.BU_QMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Обходим выбранные бэкапы в цикле...
                        foreach (ListViewItem BU_Item in BU_LVTable.SelectedItems)
                        {
                            // Проверяем что восстанавливать: конфиг или реестр...
                            switch (Path.GetExtension(BU_Item.SubItems[4].Text))
                            {
                                case ".reg":
                                    // Восстанавливаем файл реестра...
                                    try
                                    {
                                        // Восстанавливаем...
                                        Process.Start("regedit.exe", String.Format("/s \"{0}\"", Path.Combine(App.SourceGames[AppSelector.Text].FullBackUpDirPath, BU_Item.SubItems[4].Text)));
                                    }
                                    catch (Exception Ex)
                                    {
                                        // Произошло исключение...
                                        MessageBox.Show(AppStrings.BU_RestFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        Logger.Error(Ex, DebugStrings.AppDbgExRegedit);
                                    }
                                    break;
                                case ".bud":
                                    // Распаковываем архив с выводом прогресса...
                                    GuiHelpers.FormShowArchiveExtract(Path.Combine(App.SourceGames[AppSelector.Text].FullBackUpDirPath, BU_Item.SubItems[4].Text), Path.GetPathRoot(App.SourceGames[AppSelector.Text].FullGamePath));

                                    // Обновляем список FPS-конфигов...
                                    HandleConfigs();
                                    break;
                                default:
                                    // Выводим сообщение о неизвестном формате резервной копии...
                                    MessageBox.Show(AppStrings.BU_UnknownType, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    break;
                            }

                            // Выводим сообщение об успехе...
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

        private void BUT_DelB_Click(object sender, EventArgs e)
        {
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    // Запросим подтверждение...
                    if (MessageBox.Show(AppStrings.BU_DelMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Обходим выбранные бэкапы в цикле...
                        foreach (ListViewItem BU_Item in BU_LVTable.SelectedItems)
                        {
                            try
                            {
                                // Удаляем файл...
                                File.Delete(Path.Combine(App.SourceGames[AppSelector.Text].FullBackUpDirPath, BU_Item.SubItems[4].Text));

                                // Удаляем строку...
                                BU_LVTable.Items.Remove(BU_Item);
                            }
                            catch (Exception Ex)
                            {
                                // Произошло исключение при попытке удаления файла резервной копии...
                                MessageBox.Show(AppStrings.BU_DelFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                Logger.Error(Ex, DebugStrings.AppDbgExBackupRem);
                            }
                        }

                        // Показываем сообщение об успешном удалении...
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

        private void BUT_CrBkupReg_ButtonClick(object sender, EventArgs e)
        {
            // Отображаем выпадающее меню...
            BUT_CrBkupReg.ShowDropDown();
        }

        private void BUT_L_GameSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppStrings.BU_RegCreate, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Создадим резервную копию графических настроек игры...
                try
                {
                    if (!App.SourceGames[AppSelector.Text].IsUsingVideoFile)
                    {
                        // Создаём конфиг ветки реестра...
                        Type1Video.BackUpVideoSettings(Type1Video.GetGameRegKey(App.SourceGames[AppSelector.Text].SmallAppName), "Game_Options", App.SourceGames[AppSelector.Text].FullBackUpDirPath);
                    }
                    else
                    {
                        // Проверяем существование файла с графическими настройками игры...
                        try
                        {
                            FileManager.CreateConfigBackUp(App.SourceGames[AppSelector.Text].VideoCfgFiles, App.SourceGames[AppSelector.Text].FullBackUpDirPath, Properties.Resources.BU_PrefixVideo);
                        }
                        catch (Exception Ex)
                        {
                            Logger.Warn(Ex, DebugStrings.AppDbgExBkGsAuto);
                        }
                    }
                    
                    // Выводим сообщение об успехе...
                    MessageBox.Show(AppStrings.BU_RegDone, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Обновляем список резервных копий...
                    UpdateBackUpList();
                }
                catch (Exception Ex)
                {
                    // Выводим сообщение об ошибке и пишем в журнал отладки...
                    MessageBox.Show(AppStrings.BU_RegErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExBkSg);
                }
            }
        }

        private void BUT_L_AllSteam_Click(object sender, EventArgs e)
        {
            // Создадим резервную копию всех настроек Steam...
            if (MessageBox.Show(AppStrings.BU_RegCreate, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // Создаём...
                    Type1Video.CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve"), "Steam_BackUp", App.SourceGames[AppSelector.Text].FullBackUpDirPath);
                    
                    // Выводим сообщение...
                    MessageBox.Show(AppStrings.BU_RegDone, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Обновим список бэкапов...
                    UpdateBackUpList();
                }
                catch (Exception Ex)
                {
                    // Произошло исключение, уведомим пользователя...
                    MessageBox.Show(AppStrings.BU_RegErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExBkAllStm);
                }
            }
        }

        private void BUT_L_AllSRC_Click(object sender, EventArgs e)
        {
            // Созданим резервную копию графических настроек всех Source-игр...
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

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Пользователь переключил вкладку. Обновляем содержимое строки статуса...
            UpdateStatusBar();
        }

        private void CE_ShowHint_Click(object sender, EventArgs e)
        {
            try
            {
                string Buf = CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[0].Value.ToString();
                if (!String.IsNullOrEmpty(Buf)) { Buf = CvarFetcher.GetString(Buf); if (!String.IsNullOrEmpty(Buf)) { MessageBox.Show(Buf, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); } else { MessageBox.Show(AppStrings.CE_ClNoDescr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); } } else { MessageBox.Show(AppStrings.CE_ClSelErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            catch
            {
                MessageBox.Show(AppStrings.CE_ClSelErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUHelp_Click(object sender, EventArgs e)
        {
            // Сгенерируем путь к файлу CHM справочной системы...
            string CHMFile = Path.Combine(App.FullAppPath, "help", String.Format(Properties.Resources.AppHelpFileName, AppStrings.AppLangPrefix));

            // Проверим существование файла справки...
            if (File.Exists(CHMFile))
            {
                // Отобразим справочную систему в зависимости от контекста...
                Help.ShowHelp(this, CHMFile, HelpNavigator.Topic, GetHelpWebPage(MainTabControl.SelectedIndex));
            }
            else
            {
                // Скомпилированный файл справки не найден. Выводим сообщение об ошибке...
                MessageBox.Show(AppStrings.AppHelpCHMNotFound, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

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

        private void CE_RmRow_Click(object sender, EventArgs e)
        {
            try
            {
                if (CE_Editor.Rows.Count > 0)
                {
                    CE_Editor.Rows.Remove(CE_Editor.CurrentRow);
                }
            }
            catch (Exception Ex) { Logger.Warn(Ex, DebugStrings.AppDbgExCfgEdRemRow); }
        }

        private void CE_Copy_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                foreach (DataGridViewCell DV in CE_Editor.SelectedCells) { if (DV.Value != null) { SB.AppendFormat("{0} ", DV.Value); } }
                Clipboard.SetText(SB.ToString().Trim());
            }
            catch (Exception Ex) { Logger.Warn(Ex, DebugStrings.AppDbgExCfgEdCopy); }
        }

        private void CE_Cut_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                foreach (DataGridViewCell DV in CE_Editor.SelectedCells) { if (DV.Value != null) { SB.AppendFormat("{0} ", DV.Value); DV.Value = null; } }
                Clipboard.SetText(SB.ToString().Trim());
            }
            catch (Exception Ex) { Logger.Warn(Ex, DebugStrings.AppDbgExCfgEdCut); }
        }

        private void CE_Paste_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value = Clipboard.GetText();
                }
            }
            catch (Exception Ex) { Logger.Warn(Ex, DebugStrings.AppDbgExCfgEdPaste); }
        }

        private void FP_OpenNotepad_Click(object sender, EventArgs e)
        {
            // Сгенерируем путь к файлу...
            string ConfigFile = Path.Combine(App.FullAppPath, "cfgs", App.SourceGames[AppSelector.Text].CFGMan[FP_ConfigSel.Text].FileName);
            
            // Проверим зажал ли пользователь Shift перед тем, как кликнуть по кнопке...
            if (Control.ModifierKeys == Keys.Shift)
            {
                // Загрузим выбранный конфиг в Редактор конфигов...
                ReadConfigFromFile(ConfigFile);

                // Переключимся на него...
                MainTabControl.SelectedIndex = 1;
            }
            else
            {
                // Загрузим файл в Блокноте...
                try
                {
                    ProcessManager.OpenTextEditor(ConfigFile, Properties.Settings.Default.EditorBin, App.Platform.OS);
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, DebugStrings.AppDbgExCfgEdExtEdt);
                }
            }
        }

        private void MNUUpdateCheck_Click(object sender, EventArgs e)
        {
            // Откроем форму модуля проверки обновлений...
            GuiHelpers.FormShowUpdater(App.UserAgent, App.FullAppPath, App.AppUserDir, App.Platform);
            
            // Перечитаем базу игр...
            FindGames();
        }

        private void BUT_OpenNpad_Click(object sender, EventArgs e)
        {
            // Откроем выбранный бэкап в Блокноте Windows...
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

        private void MNUAppOptions_Click(object sender, EventArgs e)
        {
            // Показываем форму настроек...
            GuiHelpers.FormShowOptions();
        }

        private void BU_LVTable_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            // Запрещаем изменение размеров столбцов таблицы...
            e.NewWidth = BU_LVTable.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void BUT_ExploreBUp_Click(object sender, EventArgs e)
        {
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    // Откроем выбранный бэкап в Проводнике Windows...
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

        private void FrmMainW_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Форма была закрыта, сохраняем настройки приложения...
            Properties.Settings.Default.Save();
        }

        private void MNUWinMnuDisabler_Click(object sender, EventArgs e)
        {
            // Показываем модуля отключения клавиш...
            GuiHelpers.FormShowKBHelper();
        }

        private void CE_OpenInNotepad_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(CFGFileName)))
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

        private void PS_PathDetector_Click(object sender, EventArgs e)
        {
            if (((Label)sender).ForeColor == Color.Red) { MessageBox.Show(AppStrings.SteamNonASCIIDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); } else { MessageBox.Show(AppStrings.SteamNonASCIINotDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void PS_PathGame_Click(object sender, EventArgs e)
        {
            if (((Label)sender).ForeColor == Color.Red) { MessageBox.Show(AppStrings.GameNonASCIIDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); } else { MessageBox.Show(AppStrings.GameNonASCIINotDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void PS_RemReplays_Click(object sender, EventArgs e)
        {
            // Удаляем все реплеи...
            StartCleanup("6", ((Button)sender).Text);
        }

        private void PS_RemTextures_Click(object sender, EventArgs e)
        {
            // Удаляем все кастомные текстуры...
            StartCleanup("7", ((Button)sender).Text);
        }

        private void PS_RemSecndCache_Click(object sender, EventArgs e)
        {
            // Удаляем содержимое вторичного кэша загрузок...
            StartCleanup("8", ((Button)sender).Text);
        }

        private void SB_App_DoubleClick(object sender, EventArgs e)
        {
            // Переключим статус безопасной очистки...
            Properties.Settings.Default.SafeCleanup = !Properties.Settings.Default.SafeCleanup;
            
            // Сообщим пользователю если он отключил безопасную очистку...
            if (!Properties.Settings.Default.SafeCleanup)
            {
                MessageBox.Show(AppStrings.AppSafeClnDisabled, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            // Обновим статусную строку...
            CheckSafeClnStatus();
        }

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

        private void PS_RemSounds_Click(object sender, EventArgs e)
        {
            // Удаляем кастомные звуки...
            StartCleanup("9", ((Button)sender).Text);
        }

        private void PS_RemCustDir_Click(object sender, EventArgs e)
        {
            // Удаляем пользовательного каталога...
            StartCleanup("10", ((Button)sender).Text);
        }

        private void PS_DeepCleanup_Click(object sender, EventArgs e)
        {
            // Проведём глубокую очистку...
            List<String> CleanDirs = new List<String>(App.SourceGames[AppSelector.Text].CloudConfigs);
            if (App.SourceGames[AppSelector.Text].IsUsingVideoFile)
            {
                CleanDirs.AddRange(App.SourceGames[AppSelector.Text].VideoCfgFiles);
            }
            StartCleanup("11", ((Button)sender).Text, CleanDirs);
        }

        private void PS_RemConfigs_Click(object sender, EventArgs e)
        {
            // Удаляем пользовательного каталога...
            StartCleanup("12", ((Button)sender).Text, App.SourceGames[AppSelector.Text].CloudConfigs);
        }

        private void HD_HSel_SelectedIndexChanged(object sender, EventArgs e)
        {                
            // Проверяем результат...
            bool Success = !String.IsNullOrEmpty(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].Name);

            // Переключаем статус элементов управления...
            HD_GB_Pbx.Image = Properties.Resources.LoadingFile;
            HD_Install.Enabled = Success;
            HD_Homepage.Enabled = Success;
            HD_Warning.Visible = Success && !App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].IsUpdated;

            // Выводим информацию о последнем обновлении HUD...
            HD_LastUpdate.Visible = Success;
            if (Success) { HD_LastUpdate.Text = String.Format(AppStrings.HD_LastUpdateInfo, FileManager.Unix2DateTime(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].LastUpdate).ToLocalTime()); }

            // Проверяем установлен ли выбранный HUD...
            SetHUDButtons(HUDManager.CheckInstalledHUD(App.SourceGames[AppSelector.Text].CustomInstallDir, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].InstallDir));

            // Загрузим скриншот выбранного HUD...
            if (Success && !BW_HUDScreen.IsBusy) { BW_HUDScreen.RunWorkerAsync(argument: new List<String> { AppSelector.Text, HD_HSel.Text }); }
        }

        private void HD_Install_Click(object sender, EventArgs e)
        {
            if (!HUDManager.CheckHUDDatabase(Properties.Settings.Default.LastHUDTime))
            {
                // Проверим поддерживает ли выбранный HUD последнюю версию игры...
                if (App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].IsUpdated)
                {
                    // Спросим пользователя о необходимости установки/обновления HUD...
                    if (MessageBox.Show(String.Format("{0}?", ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Начинаем загрузку архива с HUD...
                        GuiHelpers.FormShowDownloader(Properties.Settings.Default.HUDUseUpstream ? App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].UpURI : App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].URI, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].LocalFile);

                        // Проверяем существует ли файл с архивом...
                        if (File.Exists(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].LocalFile))
                        {
                            // Проверяем контрольную сумму загруженного архива...
                            if (Properties.Settings.Default.HUDUseUpstream || FileManager.CalculateFileMD5(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].LocalFile) == App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].FileHash)
                            {
                                // Проверим установлен ли выбранный HUD...
                                if (HUDManager.CheckInstalledHUD(App.SourceGames[AppSelector.Text].CustomInstallDir, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].InstallDir))
                                {
                                    // Удаляем уже установленные файлы HUD...
                                    GuiHelpers.FormShowRemoveFiles(SingleToArray(Path.Combine(App.SourceGames[AppSelector.Text].CustomInstallDir, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].InstallDir)));
                                }

                                // Распаковываем загруженный архив с файлами HUD...
                                GuiHelpers.FormShowArchiveExtract(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].LocalFile, Path.Combine(App.SourceGames[AppSelector.Text].CustomInstallDir, "hudtemp"));

                                // Запускаем установку пакета в отдельном потоке...
                                ((Button)sender).Enabled = false;
                                if (!BW_HudInstall.IsBusy) { BW_HudInstall.RunWorkerAsync(argument: new List<String> { AppSelector.Text, HD_HSel.Text }); }
                            }
                            else
                            {
                                // Проверка хеша загруженного файла не удалась. Выведем сообщение об этом...
                                MessageBox.Show(AppStrings.HD_HashError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            // Удаляем загруженный файл архива...
                            try
                            {
                                File.Delete(App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].LocalFile);
                            }
                            catch (Exception Ex) { Logger.Warn(Ex, DebugStrings.AppDbgExHudArchRem); }
                        }
                        else
                        {
                            MessageBox.Show(AppStrings.HD_DownloadError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    // Выбран устаревший HUD. Выведем сообщение об этом...
                    MessageBox.Show(AppStrings.HD_Outdated, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // База HUD устарела. Требуется обновление. Выведем сообщение...
                MessageBox.Show(AppStrings.HD_DbOutdated, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void HD_Uninstall_Click(object sender, EventArgs e)
        {
            // Спросим пользователя о необходимости удаления HUD...
            if (MessageBox.Show(String.Format("{0}?", ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Сгенерируем полный путь к установленному HUD...
                string HUDPath = Path.Combine(App.SourceGames[AppSelector.Text].CustomInstallDir, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].InstallDir);

                // Воспользуемся модулем быстрой очистки для удаления выбранного HUD...
                GuiHelpers.FormShowRemoveFiles(SingleToArray(HUDPath));

                // Проверяем установлен ли выбранный HUD...
                bool IsInstalled = HUDManager.CheckInstalledHUD(App.SourceGames[AppSelector.Text].CustomInstallDir, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].InstallDir);

                // При успешном удалении HUD выводим сообщение и сносим и его каталог...
                if (!IsInstalled) { MessageBox.Show(AppStrings.PS_CleanupSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); if (Directory.Exists(HUDPath)) { Directory.Delete(HUDPath); } }

                // Включаем / отключаем кнопки...
                SetHUDButtons(IsInstalled);
            }
        }

        private void HD_Homepage_Click(object sender, EventArgs e)
        {
            // Откроем домашнюю страницу выбранного HUD...
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

        private void MNUExtClnAppCache_Click(object sender, EventArgs e)
        {
            // Очистим загруженные приложением файлы...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(App.AppUserDir, StringsManager.HudDirectoryName, "*.*")
            };
            GuiHelpers.FormShowCleanup(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", String.Empty), AppStrings.PS_CleanupSuccess, App.SourceGames[AppSelector.Text].FullBackUpDirPath, App.SourceGames[AppSelector.Text].GameBinaryFile);
        }

        private void MNUExtClnTmpDir_Click(object sender, EventArgs e)
        {
            // Очистим каталоги с временными файлами системы...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(Path.GetTempPath(), "*.*")
            };
            GuiHelpers.FormShowCleanup(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", String.Empty), AppStrings.PS_CleanupSuccess, App.SourceGames[AppSelector.Text].FullBackUpDirPath, App.SourceGames[AppSelector.Text].GameBinaryFile);
        }

        private void MNUShowLog_Click(object sender, EventArgs e)
        {
            // Выведем на экран содержимое отладочного журнала...
            string DFile = CurrentApp.LogFileName;
            if (File.Exists(DFile)) { GuiHelpers.FormShowLogViewer(DFile); } else { MessageBox.Show(AppStrings.AppNoDebugFile, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void HD_Warning_Click(object sender, EventArgs e)
        {
            // Выведем предупреждающие сообщения...
            if (!App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].IsUpdated) { MessageBox.Show(AppStrings.HD_NotTested, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void HD_OpenDir_Click(object sender, EventArgs e)
        {
            // Покажем файлы установленного HUD в Проводнике...
            try
            {
                ProcessManager.OpenExplorer(Path.Combine(App.SourceGames[AppSelector.Text].CustomInstallDir, App.SourceGames[AppSelector.Text].HUDMan[HD_HSel.Text].InstallDir), App.Platform.OS);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExHudExtFm);
            }
        }

        private void MNUExtClnSteam_Click(object sender, EventArgs e)
        {
            // Запустим модуль очистки кэшей Steam...
            GuiHelpers.FormShowStmCleaner(App.SteamClient.FullSteamPath, App.SourceGames[AppSelector.Text].FullBackUpDirPath, App.Platform.SteamAppsFolderName, App.Platform.SteamProcName);
        }

        private void MNUMuteMan_Click(object sender, EventArgs e)
        {
            // Запустим менеджер управления отключёнными игроками...
            GuiHelpers.FormShowMuteManager(App.SourceGames[AppSelector.Text].GetActualBanlistFile(), App.SourceGames[AppSelector.Text].FullBackUpDirPath);
        }

        private void SB_SteamID_Click(object sender, EventArgs e)
        {
            // Открываем диалог выбора SteamID и прописываем пользовательский выбор...
            try
            {
                string Result = GuiHelpers.FormShowIDSelect(App.SteamClient.SteamIDs);
                if (!String.IsNullOrWhiteSpace(Result))
                {
                    SB_SteamID.Text = Result;
                    Properties.Settings.Default.LastSteamID = Result;
                    FindGames();
                }
            } catch (Exception Ex) { Logger.Warn(Ex, DebugStrings.AppDbgExUserIdSel); }
        }

        private void BU_LVTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Блокируем некоторые кнопки на панели инструментов модуля управления резервными копиями если выбрано более одной...
            bool IsSingle = BU_LVTable.SelectedItems.Count <= 1;
            BUT_OpenNpad.Enabled = IsSingle;
            BUT_ExploreBUp.Enabled = IsSingle;
        }
    }
}
