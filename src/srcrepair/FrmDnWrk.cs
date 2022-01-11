/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.IO;
using NLog;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of Internet downloader window.
    /// </summary>
    public partial class FrmDnWrk : Form
    {
        /// <summary>
        /// Logger instance for FrmDnWrk class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets or sets status of currently running process.
        /// </summary>
        private bool IsRunning { get; set; } = true;

        /// <summary>
        /// Gets or sets URL of download.
        /// </summary>
        private string RemoteURI { get; set; }

        /// <summary>
        /// Gets or sets full path of local destination file.
        /// </summary>
        private string LocalFile { get; set; }

        /// <summary>
        /// Gets or sets full path to the destination directory.
        /// </summary>
        private string LocalDirectory { get; set; }

        /// <summary>
        /// FrmDnWrk class constructor.
        /// </summary>
        /// <param name="R">Download URL.</param>
        /// <param name="L">Full path to destination file.</param>
        public FrmDnWrk(string R, string L)
        {
            InitializeComponent();
            RemoteURI = R;
            LocalFile = L;
            LocalDirectory = Path.GetDirectoryName(L);
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmDnWrk_Load(object sender, EventArgs e)
        {
            // Starting asynchronous download process in a separate thread...
            DownloaderStart();
        }

        /// <summary>
        /// Reports progress to progress bar on form.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void DownloaderProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Rendering progress on form...
            // Sometimes it can give us incorrect (out of range) values.
            try { DN_PrgBr.Value = e.ProgressPercentage; } catch (Exception Ex) { Logger.Warn(Ex); }
        }

        /// <summary>
        /// Finalizes download sequence.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void DownloaderCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Download task completed. Checking for errors...
            if (e.Error != null)
            {
                Logger.Error(e.Error, DebugStrings.AppDbgExDnWrkDownloadFile);
            }

            // Performing additional actions...
            DownloaderVerifyResult();
            DownloaderFinalize();
        }

        /// <summary>
        /// Checks if the destination directory exists. If not - creates it.
        /// </summary>
        private void DownloaderCheckLocalDirectory()
        {
            if (!Directory.Exists(LocalDirectory))
            {
                Directory.CreateDirectory(LocalDirectory);
            }
        }

        /// <summary>
        /// Checks if the destination file exists. If so - deletes it.
        /// </summary>
        private void DownloaderCheckLocalFile()
        {
            if (File.Exists(LocalFile))
            {
                File.Delete(LocalFile);
            }
        }

        /// <summary>
        /// Performs preliminary checks.
        /// </summary>
        private void DownloaderRunChecks()
        {
            DownloaderCheckLocalDirectory();
            DownloaderCheckLocalFile();
        }

        /// <summary>
        /// Downloads file from the Internet.
        /// </summary>
        private void DownloaderStart()
        {
            try
            {
                DownloaderRunChecks();

                // Starting asynchronous download...
                using (WebClient FileDownloader = new WebClient())
                {
                    FileDownloader.Headers.Add("User-Agent", Properties.Resources.AppDnlUA);
                    FileDownloader.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloaderCompleted);
                    FileDownloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloaderProgressChanged);
                    FileDownloader.DownloadFileAsync(new Uri(RemoteURI), LocalFile);
                }
            }
            catch (Exception Ex) { Logger.Warn(Ex); }
        }

        /// <summary>
        /// Checks if the downloaded file exists and is not empty.
        /// </summary>
        private void DownloaderVerifyResult()
        {
            try
            {
                FileInfo Fi = new FileInfo(LocalFile);
                if (Fi.Exists && Fi.Length == 0)
                {
                    Fi.Delete();
                }
            }
            catch (Exception Ex) { Logger.Warn(Ex); }
        }

        /// <summary>
        /// Performs finalizing actions and closes the form.
        /// </summary>
        private void DownloaderFinalize()
        {
            IsRunning = false;
            Close();
        }

        /// <summary>
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmDnWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = IsRunning;
        }
    }
}
