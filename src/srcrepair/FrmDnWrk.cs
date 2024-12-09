/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.IO;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of the Internet download module.
    /// </summary>
    public partial class FrmDnWrk : Form
    {
        /// <summary>
        /// Logger instance for the FrmDnWrk class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Stores the download URL.
        /// </summary>
        private readonly string RemoteURI;

        /// <summary>
        /// Stores the full path to the destination file.
        /// </summary>
        private readonly string LocalFile;

        /// <summary>
        /// Stores the full path to the destination directory.
        /// </summary>
        private readonly string LocalDirectory;

        /// <summary>
        /// Stores the status of the currently running process.
        /// </summary>
        private bool IsRunning = true;

        /// <summary>
        /// FrmDnWrk class constructor.
        /// </summary>
        /// <param name="URL">Download URL.</param>
        /// <param name="FileName">Full path to destination file.</param>
        public FrmDnWrk(string URL, string FileName)
        {
            InitializeComponent();
            RemoteURI = URL;
            LocalFile = FileName;
            LocalDirectory = Path.GetDirectoryName(FileName);
        }

        /// <summary>
        /// Creates the destination directory.
        /// </summary>
        private void CreateLocalDirectory()
        {
            if (!Directory.Exists(LocalDirectory))
            {
                Directory.CreateDirectory(LocalDirectory);
            }
        }

        /// <summary>
        /// Removes the destination file.
        /// </summary>
        private void RemoveLocalFile()
        {
            FileManager.RemoveFile(LocalFile);
        }

        /// <summary>
        /// Performs various preliminary checks and prepares for downloading.
        /// </summary>
        private void RunChecks()
        {
            CreateLocalDirectory();
            RemoveLocalFile();
        }

        /// <summary>
        /// Asynchronously downloads file from the Internet in a separate thread
        /// and reports progress.
        /// </summary>
        private void FormStart()
        {
            try
            {
                RunChecks();
                using (WebClient FileDownloader = new WebClient())
                {
                    FileDownloader.Headers.Add("User-Agent", Properties.Resources.AppDownloadUserAgent);
                    FileDownloader.DownloadFileCompleted += new AsyncCompletedEventHandler(FrmDnWrk_DownloadFileCompleted);
                    FileDownloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(FrmDnWrk_DownloadProgressChanged);
                    FileDownloader.DownloadFileAsync(new Uri(RemoteURI), LocalFile);
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExDnWrkTask, RemoteURI, LocalFile);
            }
        }

        /// <summary>
        /// Checks if the downloaded file exists and is not empty.
        /// </summary>
        private void VerifyResult()
        {
            try
            {
                FileInfo Fi = new FileInfo(LocalFile);
                if (Fi.Exists && Fi.Length == 0)
                {
                    Fi.Delete();
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExDnResultVerify, LocalFile);
            }
        }

        /// <summary>
        /// Performs finalizing actions and closes the form.
        /// </summary>
        private void FormFinalize()
        {
            IsRunning = false;
            Close();
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmDnWrk_Load(object sender, EventArgs e)
        {
            FormStart();
        }

        /// <summary>
        /// "Download progress changed" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmDnWrk_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                DN_PrgBr.Value = e.ProgressPercentage;
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExDnProgressChanged);
            }
        }

        /// <summary>
        /// "Download file completed" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event completion arguments and results.</param>
        private void FrmDnWrk_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Download task completed. Checking for errors...
            if (e.Error != null)
            {
                Logger.Error(e.Error, DebugStrings.AppDbgExDnWrkDownloadFile, RemoteURI, LocalFile);
            }

            // Performing additional actions...
            VerifyResult();
            FormFinalize();
        }

        /// <summary>
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmDnWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && IsRunning;
        }
    }
}
