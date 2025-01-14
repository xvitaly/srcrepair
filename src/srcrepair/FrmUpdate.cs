﻿/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of the Update center module.
    /// </summary>
    public partial class FrmUpdate : Form
    {
        /// <summary>
        /// Logger instance for the FrmUpdate class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// CurrentPlatform instance for the FrmUpdate class.
        /// </summary>
        private readonly CurrentPlatform Platform = CurrentPlatform.Create();

        /// <summary>
        /// Stores the User-Agent header for outgoing HTTP queries.
        /// </summary>
        private readonly string UserAgent;

        /// <summary>
        /// Stores the full path to the local updates directory.
        /// </summary>
        private readonly string AppUpdateDir;

        /// <summary>
        /// Stores the application installation directory.
        /// </summary>
        private readonly string FullAppPath;

        /// <summary>
        /// Stores an instance of the UpdateManager class for working
        /// with updates.
        /// </summary>
        private UpdateManager UpMan;

        /// <summary>
        /// Stores the status of the currently running process.
        /// </summary>
        private bool IsCompleted = false;

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
        /// Launches a program update checker in a separate thread, waits for the
        /// result and returns a message if found.
        /// </summary>
        private async Task CheckForUpdates()
        {
            try
            {
                if (await IsUpdatesAvailable(UserAgent))
                {
                    UP_Icon.Image = Properties.Resources.IconUpdateAvailable;
                    UP_Status.Text = string.Format(AppStrings.UP_UpdateAvailable, UpMan.AppUpdateVersion);
                }
                else
                {
                    UP_Icon.Image = Properties.Resources.IconUpdateNotAvailable;
                    UP_Status.Text = AppStrings.UP_NoUpdates;
                }
                Properties.Settings.Default.LastUpdateTime = DateTime.Now;
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, DebugStrings.AppDbgExUpCheckForUpdates);
                UP_Icon.Image = Properties.Resources.IconUpdateError;
                UP_Status.Text = AppStrings.UP_CheckForUpdatesError;
            }
            IsCompleted = true;
        }

        /// <summary>
        /// Checks for application updates in a separate thread.
        /// </summary>
        /// <param name="UA">User-Agent header for outgoing HTTP queries.</param>
        /// <returns>Returns True if updates were found.</returns>
        private async Task<bool> IsUpdatesAvailable(string UA)
        {
            UpMan = await UpdateManager.Create(UA);
            return UpMan.CheckAppUpdate();
        }

        /// <summary>
        /// Installs standalone update.
        /// </summary>
        /// <param name="UpdateURL">Full download URL.</param>
        /// <returns>Result of operation.</returns>
        private bool InstallUpdate(string UpdateURL)
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
                if (UpMan.CheckAppHash(FileManager.CalculateFileSHA512(UpdateFileName)))
                {
                    // Showing message about successful download...
                    MessageBox.Show(AppStrings.UP_UpdateSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Installing standalone update...
                    try
                    {
                        // Checking if app's installation directory is writable...
                        if (FileManager.IsDirectoryWritable(FullAppPath))
                        {
                            // Running installer with current access rights...
                            Platform.StartRegularProcess(UpdateFileName);
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
                        Logger.Error(Ex, DebugStrings.AppDbgExUpInstallUpdate);
                        MessageBox.Show(AppStrings.UP_UpdateFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Writing to log...
                    Logger.Error(DebugStrings.AppDbgUpHashMissmatch);

                    // Hash missmatch. File was damaged. Removing it...
                    try
                    {
                        File.Delete(UpdateFileName);
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, DebugStrings.AppDbgExUpDeleteFile);
                    }

                    // Showing message about hash missmatch...
                    MessageBox.Show(AppStrings.UP_HashMissmatch, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Showing error about update failure...
                MessageBox.Show(AppStrings.UP_UpdateFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Returning result...
            return Result;
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private async void FrmUpdate_Load(object sender, EventArgs e)
        {
            // Starting checking for updates...
            await CheckForUpdates();
        }

        /// <summary>
        /// "Install app update" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void UP_Status_Click(object sender, EventArgs e)
        {
            if (IsCompleted)
            {
                if (UpMan.CheckAppUpdate())
                {
                    if (Platform.AutoUpdateSupported && !Properties.Settings.Default.IsPortable)
                    {
                        if (InstallUpdate(UpMan.AppUpdateURL))
                        {
                            Platform.Exit(ReturnCodes.AppUpdatePending);
                        }
                    }
                    else
                    {
                        if (MessageBox.Show(string.Format(AppStrings.UPD_AppOtherPlatform, UpMan.AppUpdateVersion), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Platform.OpenWebPage(UpMan.AppUpdateInfo);
                        }
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
        private void UP_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && !IsCompleted;
        }
    }
}
