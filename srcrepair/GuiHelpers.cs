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
using System.IO;
using System.Windows.Forms;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class with helper methods for working with forms.
    /// </summary>
    public static class GuiHelpers
    {
        /// <summary>
        /// Opens downloader window and downloads specified file
        /// to a specified location.
        /// </summary>
        /// <param name="URI">Download URL.</param>
        /// <param name="FileName">Full path to destination file.</param>
        public static void FormShowDownloader(string URI, string FileName)
        {
            using (FrmDnWrk DnW = new FrmDnWrk(URI, FileName))
            {
                DnW.ShowDialog();
            }
        }

        /// <summary>
        /// Opens archive extract window with progress and extracts
        /// specified file to a specified directory.
        /// </summary>
        /// <param name="ArchName">Full path to archive.</param>
        /// <param name="DestDir">Full destination path.</param>
        public static void FormShowArchiveExtract(string ArchName, string DestDir)
        {
            using (FrmArchWrk ArW = new FrmArchWrk(ArchName, DestDir))
            {
                ArW.ShowDialog();
            }
        }

        /// <summary>
        /// Opens interactive cleanup window.
        /// </summary>
        /// <param name="Paths">List of files and directories for cleanup.</param>
        /// <param name="LText">Cleanup window title.</param>
        /// <param name="CheckBin">Process name to be checked before cleanup.</param>
        /// <param name="ResultMsg">Successful cleanup completion message text.</param>
        /// <param name="BackUpDir">Path to directory for saving backups.</param>
        /// <param name="ReadOnly">Allow user to manually select files for deletion.</param>
        /// <param name="NoAuto">Disable automatically mark found files to deletion.</param>
        /// <param name="Recursive">Enable recursive cleanup.</param>
        /// <param name="ForceBackUp">Force backup file creation before running cleanup.</param>
        public static void FormShowCleanup(List<String> Paths, string LText, string ResultMsg, string BackUpDir, string CheckBin, bool ReadOnly = false, bool NoAuto = false, bool Recursive = true, bool ForceBackUp = false)
        {
            if (!ProcessManager.IsProcessRunning(Path.GetFileNameWithoutExtension(CheckBin)))
            {
                using (FrmCleaner FCl = new FrmCleaner(Paths, BackUpDir, LText, ResultMsg, ReadOnly, NoAuto, Recursive, ForceBackUp))
                {
                    FCl.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show(String.Format(AppStrings.PS_AppRunning, CheckBin), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Opens non-interactive cleanup window.
        /// </summary>
        /// <param name="Paths">List of files and directories for cleanup.</param>
        public static void FormShowRemoveFiles(List<String> Paths)
        {
            using (FrmRmWrk Rm = new FrmRmWrk(Paths))
            {
                Rm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens non-interactive cleanup window.
        /// </summary>
        /// <param name="RmPath">Path for cleanup.</param>
        public static void FormShowRemoveFiles(string RmPath)
        {
            FormShowRemoveFiles(new List<String> { RmPath });
        }

        /// <summary>
        /// Opens Steam UserID selection window.
        /// </summary>
        /// <param name="SteamIDs">List of available Steam UserIDs.</param>
        /// <returns>Selected by user Steam UserID.</returns>
        public static string FormShowIDSelect(List<String> SteamIDs)
        {
            // Checking number of available Steam UserIDs...
            if (SteamIDs.Count < 1)
            {
                throw new ArgumentOutOfRangeException(AppStrings.SD_NEParamsFormException);
            }

            // Creating local variable for storing result...
            string Result = String.Empty;

            // Starting form...
            using (FrmStmSelector StmSel = new FrmStmSelector(SteamIDs))
            {
                if (StmSel.ShowDialog() == DialogResult.OK)
                {
                    Result = StmSel.SteamID;
                }
            }

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Opens config selection window.
        /// </summary>
        /// <param name="Cfgs">List of available configs.</param>
        /// <returns>Selected by user config.</returns>
        public static string FormShowCfgSelect(List<String> Cfgs)
        {
            // Checking number of available configs...
            if (Cfgs.Count < 1)
            {
                throw new ArgumentOutOfRangeException(AppStrings.CS_NEParamsFormException);
            }

            // Creating local variable for storing result...
            string Result = String.Empty;

            // Starting form...
            using (FrmCfgSelector CfgSel = new FrmCfgSelector(Cfgs))
            {
                if (CfgSel.ShowDialog() == DialogResult.OK)
                {
                    Result = CfgSel.Config;
                }
            }

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Opens reports generation window.
        /// </summary>
        /// <param name="AppUserDir">App's user directory.</param>
        /// <param name="FullSteamPath">Full path to Steam client directory.</param>
        /// <param name="SelectedGame">Instance of SourceGame class, selected in main window.</param>
        public static void FormShowRepBuilder(string AppUserDir, string FullSteamPath, SourceGame SelectedGame)
        {
            using (FrmRepBuilder RBF = new FrmRepBuilder(AppUserDir, FullSteamPath, SelectedGame))
            {
                RBF.ShowDialog();
            }
        }

        /// <summary>
        /// Opens installer window.
        /// </summary>
        /// <param name="FullGamePath">Path to game installation directory.</param>
        /// <param name="IsUsingUserDir">If current game is using a special directory for custom user stuff.</param>
        /// <param name="CustomInstallDir">Path to custom user stuff directory.</param>
        public static void FormShowInstaller(string FullGamePath, bool IsUsingUserDir, string CustomInstallDir)
        {
            using (FrmInstaller InstF = new FrmInstaller(FullGamePath, IsUsingUserDir, CustomInstallDir))
            {
                InstF.ShowDialog();
            }
        }

        /// <summary>
        /// Opens about application window.
        /// </summary>
        public static void FormShowAboutApp()
        {
            using (FrmAbout AboutFrm = new FrmAbout())
            {
                AboutFrm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens updater window.
        /// </summary>
        /// <param name="UserAgent">User-Agent header for outgoing HTTP queries.</param>
        /// <param name="FullAppPath">App's installation directory.</param>
        /// <param name="AppUserDir">App's user directory.</param>
        /// <param name="Platform">Currently running operating system ID.</param>
        public static void FormShowUpdater(string UserAgent, string FullAppPath, string AppUserDir, CurrentPlatform Platform)
        {
            using (FrmUpdate UpdFrm = new FrmUpdate(UserAgent, FullAppPath, AppUserDir, Platform))
            {
                UpdFrm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens settings window.
        /// </summary>
        public static void FormShowOptions()
        {
            using (FrmOptions OptsFrm = new FrmOptions())
            {
                OptsFrm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens system keyboard keys disable window.
        /// </summary>
        public static void FormShowKBHelper()
        {
            using (FrmKBHelper KBHlp = new FrmKBHelper())
            {
                KBHlp.ShowDialog();
            }
        }

        /// <summary>
        /// Opens log viewer window.
        /// </summary>
        /// <param name="LogFile">Full path to active log file.</param>
        public static void FormShowLogViewer(string LogFile)
        {
            using (FrmLogView Lv = new FrmLogView(LogFile))
            {
                Lv.ShowDialog();
            }
        }

        /// <summary>
        /// Opens interactive cleanup window.
        /// </summary>
        /// <param name="FullSteamPath">Full path to Steam client directory.</param>
        /// <param name="FullBackUpDirPath">Path to directory for saving backups.</param>
        /// <param name="SteamAppsDirName">Platform-dependent SteamApps directory name.</param>
        /// <param name="SteamProcName">Platform-dependent Steam process name.</param>
        public static void FormShowStmCleaner(string FullSteamPath, string FullBackUpDirPath, string SteamAppsDirName, string SteamProcName)
        {
            using (FrmStmClean StmCln = new FrmStmClean(FullSteamPath, FullBackUpDirPath, SteamAppsDirName, SteamProcName))
            {
                StmCln.ShowDialog();
            }
        }

        /// <summary>
        /// Opens muted players manager window.
        /// </summary>
        /// <param name="Banlist">Full path to muted players database file.</param>
        /// <param name="FullBackUpDirPath">Full path to game backups directory.</param>
        public static void FormShowMuteManager(string Banlist, string FullBackUpDirPath)
        {
            using (FrmMute FMm = new FrmMute(Banlist, FullBackUpDirPath))
            {
                FMm.ShowDialog();
            }
        }

        /// <summary>
        /// Formats bytes file size to user-friendly format.
        /// </summary>
        /// <param name="InpNumber">File size in bytes.</param>
        /// <returns>User-friendly formatted string.</returns>
        public static string SclBytes(long InpNumber)
        {
            // Setting constants...
            const long B = 1024;
            const long KB = B * B;
            const long MB = B * B * B;
            const long GB = B * B * B * B;
            const string Template = "{0} {1}";

            // Checking bytes...
            if ((InpNumber >= 0) && (InpNumber < B)) { return String.Format(Template, InpNumber, AppStrings.AppSizeBytes); }
            // ...kilobytes...
            else if ((InpNumber >= B) && (InpNumber < KB)) { return String.Format(Template, Math.Round((float)InpNumber / B, 2), AppStrings.AppSizeKilobytes); }
            // ...megabytes...
            else if ((InpNumber >= KB) && (InpNumber < MB)) { return String.Format(Template, Math.Round((float)InpNumber / KB, 2), AppStrings.AppSizeMegabytes); }
            // ...gitabytes.
            else if ((InpNumber >= MB) && (InpNumber < GB)) { return String.Format(Template, Math.Round((float)InpNumber / MB, 2), AppStrings.AppSizeGigabytes); }

            // Return source as result...
            return InpNumber.ToString();
        }
    }
}
