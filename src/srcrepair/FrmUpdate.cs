/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 *
 * Copyright (c) 2011 - 2021 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2021 EasyCoding Team.
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
        /// CurrentPlatform instance for FrmMute class.
        /// </summary>
        private readonly CurrentPlatform Platform = CurrentPlatform.Create();

        /// <summary>
        /// FrmUpdate class constructor.
        /// </summary>
        /// <param name="UA">User-Agent header for outgoing HTTP queries.</param>
        /// <param name="A">App's installation directory.</param>
        /// <param name="U">App's local updates directory.</param>
        public FrmUpdate(string UA, string A, string U)
        {
            InitializeComponent();
            UserAgent = UA;
            FullAppPath = A;
            AppUpdateDir = U;
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
        /// Gets or sets full path to the local updates directory.
        /// </summary>
        private string AppUpdateDir { get; set; }

        /// <summary>
        /// Gets or sets app's installation directory.
        /// </summary>
        private string FullAppPath { get; set; }

        /// <summary>
        /// Sets time of last application update check.
        /// </summary>
        private void UpdateTimeSetApp()
        {
            Properties.Settings.Default.LastUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// Starts update checking sequence in a separate thread.
        /// </summary>
        private void CheckForUpdates()
        {
            // Changing icons...
            UpdAppImg.Image = Properties.Resources.upd_chk;

            // Starting updates check in a separate thread...
            if (!WrkChkApp.IsBusy) { WrkChkApp.RunWorkerAsync(new List<String> { FullAppPath, UserAgent }); }
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
            string UpdateFileName = UpdateManager.GenerateUpdateFileName(Path.Combine(AppUpdateDir, Path.GetFileName(UpdateURL)));

            // Downloading update from server...
            GuiHelpers.FormShowDownloader(UpMan.AppUpdateURL, UpdateFileName);

            // Checking if downloaded file exists...
            if (File.Exists(UpdateFileName))
            {
                // Checking hashes...
                if (FileManager.CalculateFileSHA512(UpdateFileName) == UpdateHash)
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
                            Platform.StartElevatedProcess(UpdateFileName);
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
                }
                else
                {
                    // Changing controls texts...
                    UpdAppImg.Image = Properties.Resources.upd_err;
                    UpdAppStatus.Text = AppStrings.UPD_AppCheckFailure;

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
        /// "Close" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void UpdClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
