/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
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
        /// Opens the file downloader module and downloads the specified URL
        /// to the specified location.
        /// </summary>
        /// <param name="URI">Full download URL.</param>
        /// <param name="FileName">Full path to the destination file.</param>
        public static void FormShowDownloader(string URI, string FileName)
        {
            using (FrmDnWrk DnWrkForm = new FrmDnWrk(URI, FileName))
            {
                DnWrkForm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the archive unpacking module and extracts the
        /// specified archive file to the specified directory.
        /// </summary>
        /// <param name="ArchName">Full path to the archive file.</param>
        /// <param name="DestDir">Full path to the destination directory.</param>
        public static void FormShowArchiveExtract(string ArchName, string DestDir)
        {
            using (FrmArchWrk ArchWrkForm = new FrmArchWrk(ArchName, DestDir))
            {
                ArchWrkForm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens interactive cleanup window.
        /// </summary>
        /// <param name="Paths">List of files and directories for cleanup.</param>
        /// <param name="LText">Cleanup window title.</param>
        /// <param name="CheckBin">Process name to be checked before cleanup.</param>
        /// <param name="ResultMsg">Successful cleanup completion message text.</param>
        /// <param name="SteamDir">Full path to the Steam installation directory.</param>
        /// <param name="BackUpDir">Path to directory for saving backups.</param>
        /// <param name="ReadOnly">Allow user to manually select files for deletion.</param>
        /// <param name="NoAuto">Disable automatically mark found files to deletion.</param>
        /// <param name="Recursive">Enable recursive cleanup.</param>
        /// <param name="ForceBackUp">Force backup file creation before running cleanup.</param>
        public static void FormShowCleanup(List<string> Paths, string LText, string ResultMsg, string SteamDir, string BackUpDir, string CheckBin, bool ReadOnly, bool NoAuto, bool Recursive, bool ForceBackUp)
        {
            if (!ProcessManager.IsProcessRunning(Path.GetFileNameWithoutExtension(CheckBin)))
            {
                using (FrmCleaner CleanerForm = new FrmCleaner(Paths, SteamDir, BackUpDir, LText, ResultMsg, ReadOnly, NoAuto, Recursive, ForceBackUp))
                {
                    CleanerForm.ShowDialog();
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
        /// <param name="SteamDir">Full path to the Steam installation directory.</param>
        /// <param name="BackUpDir">Path to directory for saving backups.</param>
        public static void FormShowCleanup(List<string> Paths, string LText, string ResultMsg, string SteamDir, string BackUpDir, string CheckBin)
        {
            FormShowCleanup(Paths, LText, ResultMsg, SteamDir, BackUpDir, CheckBin, false, false, true, false);
        }

        /// <summary>
        /// Opens the file remover module.
        /// </summary>
        /// <param name="Paths">The list of files and directories for deletion.</param>
        public static void FormShowRemoveFiles(List<string> Paths)
        {
            using (FrmRmWrk RmWrkForm = new FrmRmWrk(Paths))
            {
                RmWrkForm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the file remover module.
        /// </summary>
        /// <param name="RmPath">A single path for deletion.</param>
        public static void FormShowRemoveFiles(string RmPath)
        {
            FormShowRemoveFiles(new List<string> { RmPath });
        }

        /// <summary>
        /// Opens the UserID selection module.
        /// </summary>
        /// <param name="SteamIDs">The list of available Steam UserIDs.</param>
        /// <returns>Selected by user Steam UserID.</returns>
        public static string FormShowIDSelect(List<string> SteamIDs)
        {
            // Checking the number of available Steam UserIDs...
            if (SteamIDs.Count < 1)
            {
                throw new ArgumentOutOfRangeException(DebugStrings.AppDbgExStmIDsNotEnough);
            }

            // Creating a local variable for storing result...
            string Result = string.Empty;

            // Starting the form...
            using (FrmStmSelector StmSelectorForm = new FrmStmSelector(SteamIDs))
            {
                if (StmSelectorForm.ShowDialog() == DialogResult.OK)
                {
                    Result = StmSelectorForm.SteamID;
                }
            }

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Opens the config file selection module.
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

            // Creating a local variable for storing result...
            string Result = string.Empty;

            // Starting the form...
            using (FrmCfgSelector CfgSelectorForm = new FrmCfgSelector(Cfgs))
            {
                if (CfgSelectorForm.ShowDialog() == DialogResult.OK)
                {
                    Result = CfgSelectorForm.Config;
                }
            }

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Opens the report builder module.
        /// </summary>
        /// <param name="AppReportDir">Full path to the application reports directory.</param>
        /// <param name="AppLogDir">Full path to the application logs directory.</param>
        /// <param name="FullDumpsPath">Full path to the Steam crash dumps directory.</param>
        /// <param name="FullLogsPath">Full path to the Steam logs directory.</param>
        /// <param name="SelectedGame">Instance of the SourceGame class, selected in main window.</param>
        public static void FormShowRepBuilder(string AppReportDir, string AppLogDir, string FullDumpsPath, string FullLogsPath, SourceGame SelectedGame)
        {
            using (FrmRepBuilder RepBuilderForm = new FrmRepBuilder(AppReportDir, AppLogDir, FullDumpsPath, FullLogsPath, SelectedGame))
            {
                RepBuilderForm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the quick add-on installer module.
        /// </summary>
        /// <param name="FullGamePath">Full path to the game installation directory.</param>
        /// <param name="IsUsingUserDir">If current game is using a special directory for storing custom user content.</param>
        /// <param name="CustomInstallDir">Full path to the custom user content directory.</param>
        public static void FormShowInstaller(string FullGamePath, bool IsUsingUserDir, string CustomInstallDir)
        {
            using (FrmInstaller InstallerForm = new FrmInstaller(FullGamePath, IsUsingUserDir, CustomInstallDir))
            {
                InstallerForm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the about module.
        /// </summary>
        public static void FormShowAbout()
        {
            using (FrmAbout AboutForm = new FrmAbout())
            {
                AboutForm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the update center module.
        /// </summary>
        /// <param name="UserAgent">User-Agent header for outgoing HTTP queries.</param>
        /// <param name="FullAppPath">Full path to the application installation directory.</param>
        /// <param name="AppUpdateDir">Full path to the local updates directory.</param>
        public static void FormShowUpdater(string UserAgent, string FullAppPath, string AppUpdateDir)
        {
            using (FrmUpdate UpdateForm = new FrmUpdate(UserAgent, FullAppPath, AppUpdateDir))
            {
                UpdateForm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the options module.
        /// </summary>
        public static void FormShowOptions()
        {
            using (FrmOptions OptionsForm = new FrmOptions())
            {
                OptionsForm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the log viewer module.
        /// </summary>
        /// <param name="LogFile">Full path to the log file.</param>
        public static void FormShowLogViewer(string LogFile)
        {
            using (FrmLogView LogViewForm = new FrmLogView(LogFile))
            {
                LogViewForm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the Steam clenup module.
        /// </summary>
        /// <param name="FullSteamPath">Full path to the Steam client installation directory.</param>
        /// <param name="FullBackUpDirPath">Full path to the directory for saving backups.</param>
        public static void FormShowStmCleaner(string FullSteamPath, string FullBackUpDirPath)
        {
            using (FrmStmClean StmCleanForm = new FrmStmClean(FullSteamPath, FullBackUpDirPath))
            {
                StmCleanForm.ShowDialog();
            }
        }

        /// <summary>
        /// Opens the muted players manager module.
        /// </summary>
        /// <param name="Banlist">Full path to the muted players database file.</param>
        /// <param name="FullBackUpDirPath">Full path to the directory for saving backups.</param>
        public static void FormShowMuteManager(string Banlist, string FullBackUpDirPath)
        {
            using (FrmMute MuteForm = new FrmMute(Banlist, FullBackUpDirPath))
            {
                MuteForm.ShowDialog();
            }
        }

        /// <summary>
        /// Formats the file size in bytes to the user-friendly format.
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

            // Checking bytes...
            if ((InpNumber >= 0) && (InpNumber < B)) { return string.Format(AppStrings.AppSizeBytes, InpNumber); }
            // ...kilobytes...
            else if ((InpNumber >= B) && (InpNumber < KB)) { return string.Format(AppStrings.AppSizeKilobytes, Math.Round((float)InpNumber / B, 2)); }
            // ...megabytes...
            else if ((InpNumber >= KB) && (InpNumber < MB)) { return string.Format(AppStrings.AppSizeMegabytes, Math.Round((float)InpNumber / KB, 2)); }
            // ...gigabytes...
            else if ((InpNumber >= MB) && (InpNumber < GB)) { return string.Format(AppStrings.AppSizeGigabytes, Math.Round((float)InpNumber / MB, 2)); }

            // Returning the source as a result...
            return InpNumber.ToString();
        }
    }
}
