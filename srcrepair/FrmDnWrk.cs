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
        /// FrmDnWrk class constructor.
        /// </summary>
        /// <param name="R">Download URL.</param>
        /// <param name="L">Full path to destination file.</param>
        public FrmDnWrk(string R, string L)
        {
            InitializeComponent();
            RemoteURI = R;
            LocalFile = L;
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmDnWrk_Load(object sender, EventArgs e)
        {
            // Starting asynchronous download process in a separate thread...
            DownloaderStart(RemoteURI, LocalFile);
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
            try
            {
                // Download completed. Checking if file exists and is not empty...
                FileInfo Fi = new FileInfo(LocalFile);
                if (Fi.Exists && Fi.Length == 0)
                {
                    Fi.Delete();
                }
            }
            catch (Exception Ex) { Logger.Warn(Ex); }

            // Finalizing and closing form...
            IsRunning = false;
            Close();
        }

        /// <summary>
        /// Downloads file from the Internet.
        /// </summary>
        /// <param name="URI">Download URL.</param>
        /// <param name="FileName">Full path to destination file.</param>
        private void DownloaderStart(string URI, string FileName)
        {
            try
            {
                // Checking if destination file exists. If so, delete it...
                if (File.Exists(FileName)) { File.Delete(FileName); }

                // Starting asynchronous download...
                using (WebClient FileDownloader = new WebClient())
                {
                    FileDownloader.Headers.Add("User-Agent", Properties.Resources.AppDnlUA);
                    FileDownloader.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloaderCompleted);
                    FileDownloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloaderProgressChanged);
                    FileDownloader.DownloadFileAsync(new Uri(URI), FileName);
                }
            }
            catch (Exception Ex) { Logger.Warn(Ex); }
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
