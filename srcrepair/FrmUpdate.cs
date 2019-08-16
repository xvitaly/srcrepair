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
using System.IO;
using System.Windows.Forms;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of update checker window.
    /// </summary>
    public partial class FrmUpdate : Form
    {
        /// <summary>
        /// Logger instance for FrmUpdate class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// FrmUpdate class constructor.
        /// </summary>
        /// <param name="UA">User-Agent header for outgoing HTTP queries.</param>
        /// <param name="A">App's installation directory.</param>
        /// <param name="U">App's user directory.</param>
        /// <param name="O">Currently operating system ID.</param>
        public FrmUpdate(string UA, string A, string U, CurrentPlatform O)
        {
            InitializeComponent();
            UserAgent = UA;
            FullAppPath = A;
            AppUserDir = U;
            Platform = O;
        }

        /// <summary>
        /// Gets or sets instance of UpdateManager class for working
        /// with updates.
        /// </summary>
        private UpdateManager UpMan { get; set; }

        /// <summary>
        /// Gets or sets User-Agent header for outgoing HTTP queries.
        /// </summary>
        private string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets app's user directory.
        /// </summary>
        private string AppUserDir { get; set; }

        /// <summary>
        /// Gets or sets app's installation directory.
        /// </summary>
        private string FullAppPath { get; set; }

        /// <summary>
        /// Gets or sets currently operating system ID.
        /// </summary>
        private CurrentPlatform Platform { get; set; }

        /// <summary>
        /// Sets time of last application update check.
        /// </summary>
        private void UpdateTimeSetApp()
        {
            Properties.Settings.Default.LastUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// Sets time of last HUD database update check.
        /// </summary>
        private void UpdateTimeSetHUD()
        {
            Properties.Settings.Default.LastHUDTime = DateTime.Now;
        }

        /// <summary>
        /// Starts update checking sequence in a separate thread.
        /// </summary>
        private void CheckForUpdates()
        {
            // Changing icons...
            UpdAppImg.Image = Properties.Resources.upd_chk;
            UpdDBImg.Image = Properties.Resources.upd_chk;
            UpdHUDDbImg.Image = Properties.Resources.upd_chk;

            // Starting updates check in a separate thread...
            if (!WrkChkApp.IsBusy) { WrkChkApp.RunWorkerAsync(new List<String> { FullAppPath, UserAgent }); }
        }

        /// <summary>
        /// Installs database update.
        /// </summary>
        /// <param name="ResFileName">Full path to local file, to be updated.</param>
        /// <param name="UpdateURL">Full download URL.</param>
        /// <param name="UpdateHash">Download hash.</param>
        /// <returns>Result of operation.</returns>
        private bool InstallDatabaseUpdate(string ResFileName, string UpdateURL, string UpdateHash)
        {
            // Setting default value for result...
            bool Result = false;

            // Checking if app's installation directory is writable...
            if (FileManager.IsDirectoryWritable(FullAppPath))
            {
                // Generating full paths to files...
                string UpdateFileName = UpdateManager.GenerateUpdateFileName(Path.Combine(FullAppPath, ResFileName));
                string UpdateTempFile = Path.GetTempFileName();

                // Downloading update from server...
                GuiHelpers.FormShowDownloader(UpdateURL, UpdateTempFile);

                try
                {
                    // Checking hashes...
                    if (FileManager.CalculateFileMD5(UpdateTempFile) == UpdateHash)
                    {
                        // Overwriting old file by downloaded one...
                        File.Copy(UpdateTempFile, UpdateFileName, true);

                        // Showing message about successful update...
                        MessageBox.Show(AppStrings.UPD_UpdateDBSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Setting result...
                        Result = true;
                    }
                    else
                    {
                        // Showing message about hash missmatch...
                        MessageBox.Show(AppStrings.UPD_HashFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception Ex)
                {
                    // An error occured. Showing message and writing this issue to log...
                    MessageBox.Show(AppStrings.UPD_UpdateFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.Error(Ex, DebugStrings.AppDbgExUpdXmlDbInst);
                }

                // Removing downloaded file if it still exists...
                if (File.Exists(UpdateTempFile))
                {
                    File.Delete(UpdateTempFile);
                }

                // Checking for updates again...
                CheckForUpdates();
            }
            else
            {
                // Showing message if we have no permissions to rewrite existing files...
                MessageBox.Show(AppStrings.UPD_NoWritePermissions, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Installs standalone update.
        /// </summary>
        /// <param name="UpdateURL">Full download URL.</param>
        /// <param name="UpdateHash">Download hash.</param>
        /// <returns>Result of operation.</returns>
        private bool InstallBinaryUpdate(string UpdateURL, string UpdateHash)
        {
            // Setting default value for result...
            bool Result = false;

            // Generating full paths to files...
            string UpdateFileName = UpdateManager.GenerateUpdateFileName(Path.Combine(AppUserDir, Path.GetFileName(UpdateURL)));

            // Downloading update from server...
            GuiHelpers.FormShowDownloader(UpMan.AppUpdateURL, UpdateFileName);

            // Checking if downloaded file exists...
            if (File.Exists(UpdateFileName))
            {
                // Checking hashes...
                if (FileManager.CalculateFileMD5(UpdateFileName) == UpdateHash)
                {
                    // Setting last update check date...
                    UpdateTimeSetApp();

                    // Showing message about successful download...
                    MessageBox.Show(AppStrings.UPD_UpdateSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Installing standalone update...
                    try
                    {
                        // Checking if app's installation directory is writable...
                        if (FileManager.IsDirectoryWritable(FullAppPath))
                        {
                            // Running installer with current access rights...
                            Process.Start(UpdateFileName);
                        }
                        else
                        {
                            // Running installer with UAC access rights elevation...
                            ProcessManager.StartWithUAC(UpdateFileName);
                        }
                        Result = true;
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(AppStrings.UPD_UpdateFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Logger.Error(Ex, DebugStrings.AppDbgExUpdBinInst);
                    }
                }
                else
                {
                    // Hash missmatch. File was damaged. Removing it...
                    try
                    {
                        File.Delete(UpdateFileName);
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex);
                    }

                    // Showing message about hash missmatch...
                    MessageBox.Show(AppStrings.UPD_HashFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Showing error about update failure...
                MessageBox.Show(AppStrings.UPD_UpdateFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Checking for updates again...
            CheckForUpdates();

            // Returning result...
            return Result;
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmUpdate_Load(object sender, EventArgs e)
        {
            // Setting app name in form title...
            Text = String.Format(Text, Properties.Resources.AppName);

            // Starting checking for updates...
            CheckForUpdates();
        }

        /// <summary>
        /// Checks for updates.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void WrkChkApp_DoWork(object sender, DoWorkEventArgs e)
        {
            // Parsing arguments list...
            List<String> Arguments = e.Argument as List<String>;

            // Checking for updates...
            UpMan = new UpdateManager(Arguments[0], Arguments[1]);
        }

        /// <summary>
        /// Finalizes update checking procedure.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void WrkChkApp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    // Checking for application update...
                    if (UpMan.CheckAppUpdate())
                    {
                        UpdAppImg.Image = Properties.Resources.upd_av;
                        UpdAppStatus.Text = String.Format(AppStrings.UPD_AppUpdateAvail, UpMan.AppUpdateVersion);
                    }
                    else
                    {
                        UpdAppImg.Image = Properties.Resources.upd_nx;
                        UpdAppStatus.Text = AppStrings.UPD_AppNoUpdates;
                        UpdateTimeSetApp();
                    }

                    // Checking for game database update...
                    if (UpMan.CheckGameDBUpdate())
                    {
                        UpdDBImg.Image = Properties.Resources.upd_av;
                        UpdDBStatus.Text = String.Format(AppStrings.UPD_DbUpdateAvail, UpMan.GameUpdateHash.Substring(0, 7));
                    }
                    else
                    {
                        UpdDBImg.Image = Properties.Resources.upd_nx;
                        UpdDBStatus.Text = AppStrings.UPD_DbNoUpdates;
                    }

                    // Checking for HUD database update...
                    if (UpMan.CheckHUDUpdate())
                    {
                        UpdHUDDbImg.Image = Properties.Resources.upd_av;
                        UpdHUDStatus.Text = String.Format(AppStrings.UPD_HUDUpdateAvail, UpMan.HUDUpdateHash.Substring(0, 7));
                    }
                    else
                    {
                        UpdHUDDbImg.Image = Properties.Resources.upd_nx;
                        UpdHUDStatus.Text = AppStrings.UPD_HUDNoUpdates;
                        UpdateTimeSetHUD();
                    }
                }
                else
                {
                    // Changing controls texts...
                    UpdAppImg.Image = Properties.Resources.upd_err;
                    UpdAppStatus.Text = AppStrings.UPD_AppCheckFailure;
                    UpdDBImg.Image = Properties.Resources.upd_err;
                    UpdDBStatus.Text = AppStrings.UPD_DbCheckFailure;
                    UpdHUDDbImg.Image = Properties.Resources.upd_err;
                    UpdHUDStatus.Text = AppStrings.UPD_HUDCheckFailure;

                    // Writing issue to log...
                    Logger.Warn(e.Error);
                }

            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
            }
        }

        /// <summary>
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && WrkChkApp.IsBusy;
        }

        /// <summary>
        /// "Install app update" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void UpdAppStatus_Click(object sender, EventArgs e)
        {
            if (!WrkChkApp.IsBusy)
            {
                if (UpMan.CheckAppUpdate())
                {
                    if (Platform.OS == CurrentPlatform.OSType.Windows)
                    {
                        if (InstallBinaryUpdate(UpMan.AppUpdateURL, UpMan.AppUpdateHash))
                        {
                            Environment.Exit(9);
                        }
                    }
                    else
                    {
                        MessageBox.Show(String.Format(AppStrings.UPD_AppOtherPlatform, Platform.OSFriendlyName), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.UPD_LatestInstalled, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.DB_WrkInProgress, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// "Install HUD database update" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void UpdHUDStatus_Click(object sender, EventArgs e)
        {
            if (!WrkChkApp.IsBusy)
            {
                if (UpMan.CheckHUDUpdate())
                {
                    if (InstallDatabaseUpdate(StringsManager.HudDatabaseName, UpMan.HUDUpdateURL, UpMan.HUDUpdateHash))
                    {
                        UpdateTimeSetHUD();
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.UPD_LatestDBInstalled, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.DB_WrkInProgress, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// "Install game database update" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void UpdDBStatus_Click(object sender, EventArgs e)
        {
            if (!WrkChkApp.IsBusy)
            {
                if (UpMan.CheckGameDBUpdate())
                {
                    InstallDatabaseUpdate(StringsManager.GameDatabaseName, UpMan.GameUpdateURL, UpMan.GameUpdateHash);
                }
                else
                {
                    MessageBox.Show(AppStrings.UPD_LatestDBInstalled, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.DB_WrkInProgress, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
