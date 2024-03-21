/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
        public static void FormShowCleanup(List<string> Paths, string LText, string ResultMsg, string BackUpDir, string CheckBin, bool ReadOnly, bool NoAuto, bool Recursive, bool ForceBackUp)
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
                MessageBox.Show(string.Format(AppStrings.PS_AppRunning, CheckBin), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Opens interactive cleanup window with default options.
        /// </summary>
        /// <param name="Paths">List of files and directories for cleanup.</param>
        /// <param name="LText">Cleanup window title.</param>
        /// <param name="CheckBin">Process name to be checked before cleanup.</param>
        /// <param name="ResultMsg">Successful cleanup completion message text.</param>
        /// <param name="BackUpDir">Path to directory for saving backups.</param>
        public static void FormShowCleanup(List<string> Paths, string LText, string ResultMsg, string BackUpDir, string CheckBin)
        {
            FormShowCleanup(Paths, LText, ResultMsg, BackUpDir, CheckBin, false, false, true, false);
        }

        /// <summary>
        /// Opens non-interactive cleanup window.
        /// </summary>
        /// <param name="Paths">List of files and directories for cleanup.</param>
        public static void FormShowRemoveFiles(List<string> Paths)
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
            FormShowRemoveFiles(new List<string> { RmPath });
        }

        /// <summary>
        /// Opens Steam UserID selection window.
        /// </summary>
        /// <param name="SteamIDs">List of available Steam UserIDs.</param>
        /// <returns>Selected by user Steam UserID.</returns>
        public static string FormShowIDSelect(List<string> SteamIDs)
        {
            // Checking number of available Steam UserIDs...
            if (SteamIDs.Count < 1)
            {
                throw new ArgumentOutOfRangeException(DebugStrings.AppDbgExStmIDsNotEnough);
            }

            // Creating local variable for storing result...
            string Result = string.Empty;

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
        public static string FormShowCfgSelect(List<string> Cfgs)
        {
            // Checking number of available configs...
            if (Cfgs.Count < 1)
            {
                throw new ArgumentOutOfRangeException(AppStrings.CS_NEParamsFormException);
            }

            // Creating local variable for storing result...
            string Result = string.Empty;

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
        /// <param name="FullDumpsPath">Full path to Steam crash dumps directory.</param>
        /// <param name="FullLogsPath">Full path to Steam logs directory.</param>
        /// <param name="SelectedGame">Instance of SourceGame class, selected in main window.</param>
        public static void FormShowRepBuilder(string AppUserDir, string FullDumpsPath, string FullLogsPath, SourceGame SelectedGame)
        {
            using (FrmRepBuilder RBF = new FrmRepBuilder(AppUserDir, FullDumpsPath, FullLogsPath, SelectedGame))
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
        /// <param name="AppUpdateDir">App's local updates directory.</param>
        public static void FormShowUpdater(string UserAgent, string FullAppPath, string AppUpdateDir)
        {
            using (FrmUpdate UpdFrm = new FrmUpdate(UserAgent, FullAppPath, AppUpdateDir))
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
        public static void FormShowStmCleaner(string FullSteamPath, string FullBackUpDirPath)
        {
            using (FrmStmClean StmCln = new FrmStmClean(FullSteamPath, FullBackUpDirPath))
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
            if ((InpNumber >= 0) && (InpNumber < B)) { return string.Format(Template, InpNumber, AppStrings.AppSizeBytes); }
            // ...kilobytes...
            else if ((InpNumber >= B) && (InpNumber < KB)) { return string.Format(Template, Math.Round((float)InpNumber / B, 2), AppStrings.AppSizeKilobytes); }
            // ...megabytes...
            else if ((InpNumber >= KB) && (InpNumber < MB)) { return string.Format(Template, Math.Round((float)InpNumber / KB, 2), AppStrings.AppSizeMegabytes); }
            // ...gitabytes.
            else if ((InpNumber >= MB) && (InpNumber < GB)) { return string.Format(Template, Math.Round((float)InpNumber / MB, 2), AppStrings.AppSizeGigabytes); }

            // Return source as result...
            return InpNumber.ToString();
        }
    }
}
