/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
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
        /// Stores User-Agent header for outgoing HTTP queries.
        /// </summary>
        private readonly string UserAgent;

        /// <summary>
        /// Stores full path to the local updates directory.
        /// </summary>
        private readonly string AppUpdateDir;

        /// <summary>
        /// Stores app's installation directory.
        /// </summary>
        private readonly string FullAppPath;

        /// <summary>
        /// Stores an instance of UpdateManager class for working
        /// with updates.
        /// </summary>
        private UpdateManager UpMan;

        /// <summary>
        /// Stores current state of the async tasks.
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
        /// Sets time of last application update check.
        /// </summary>
        private void UpdateTimeSetApp()
        {
            Properties.Settings.Default.LastUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// Launches a program update checker in a separate thread, waits for the
        /// result and returns a message if found.
        /// </summary>
        private async void CheckForUpdates()
        {
            try
            {
                if (await CheckForUpdatesTask(UserAgent))
                {
                    UpdAppImg.Image = Properties.Resources.IconUpdateAvailable;
                    UpdAppStatus.Text = string.Format(AppStrings.UPD_AppUpdateAvail, UpMan.AppUpdateVersion);
                }
                else
                {
                    UpdAppImg.Image = Properties.Resources.IconUpdateNotAvailable;
                    UpdAppStatus.Text = AppStrings.UPD_AppNoUpdates;
                    UpdateTimeSetApp();
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExUpdChk);
                UpdAppImg.Image = Properties.Resources.IconUpdateError;
                UpdAppStatus.Text = AppStrings.UPD_AppCheckFailure;
            }
            IsCompleted = true;
        }

        /// <summary>
        /// Checks for application updates in a separate thread.
        /// </summary>
        /// <param name="UA">User-Agent header for outgoing HTTP queries.</param>
        /// <returns>Returns True if updates were found.</returns>
        private async Task<bool> CheckForUpdatesTask(string UA)
        {
            UpMan = await UpdateManager.Create(UA);
            return UpMan.CheckAppUpdate();
        }

        /// <summary>
        /// Installs standalone update.
        /// </summary>
        /// <param name="UpdateURL">Full download URL.</param>
        /// <returns>Result of operation.</returns>
        private bool InstallBinaryUpdate(string UpdateURL)
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
            // Starting checking for updates...
            CheckForUpdates();
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

        /// <summary>
        /// "Install app update" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void UpdAppStatus_Click(object sender, EventArgs e)
        {
            if (IsCompleted)
            {
                if (UpMan.CheckAppUpdate())
                {
                    if (Platform.AutoUpdateSupported)
                    {
                        if (InstallBinaryUpdate(UpMan.AppUpdateURL))
                        {
                            Platform.Exit(ReturnCodes.AppUpdatePending);
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Format(AppStrings.UPD_AppOtherPlatform, Platform.OSFriendlyName), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
