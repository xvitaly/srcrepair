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
using System.IO;
using System.Windows.Forms;
using NLog;
using Ionic.Zip;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of archive extractor window.
    /// </summary>
    public partial class FrmArchWrk : Form
    {
        /// <summary>
        /// Logger instance for FrmArchWrk class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets or sets status of currently running process.
        /// </summary>
        private bool IsRunning { get; set; } = true;

        /// <summary>
        /// Gets or sets archive name with full path.
        /// </summary>
        private string ArchiveName { get; set; }

        /// <summary>
        /// Gets or sets destination directory.
        /// </summary>
        private string DestinationDirectory { get; set; }

        /// <summary>
        /// FrmArchWrk class constructor.
        /// </summary>
        /// <param name="A">Full path to archive.</param>
        /// <param name="D">Full destination path.</param>
        public FrmArchWrk(string A, string D)
        {
            InitializeComponent();
            ArchiveName = A;
            DestinationDirectory = D;
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmArchWrk_Load(object sender, EventArgs e)
        {
            // Starting async unpack sequence...
            if (!AR_Wrk.IsBusy) { AR_Wrk.RunWorkerAsync(new List<String> { ArchiveName, DestinationDirectory }); }
        }

        /// <summary>
        /// Extracts archive to specified destination directory.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void AR_Wrk_DoWork(object sender, DoWorkEventArgs e)
        {
            // Parsing arguments list...
            List<String> Arguments = e.Argument as List<String>;

            // Checking if archive file exists...
            if (File.Exists(Arguments[0]))
            {
                // Opening archive...
                using (ZipFile Zip = ZipFile.Read(Arguments[0]))
                {
                    // Creating some counters...
                    int TotalFiles = Zip.Count;
                    int CurrentFile = 1, CurrentPercent = 0;
                    
                    // Unpacking archive contents...
                    foreach (ZipEntry ZFile in Zip)
                    {
                        try
                        {
                            // Extracting file, then counting and reporting progress...
                            ZFile.Extract(Arguments[1], ExtractExistingFileAction.OverwriteSilently);
                            CurrentPercent = (int)Math.Round(CurrentFile / (double)TotalFiles * 100.00d, 0); CurrentFile++;
                            if ((CurrentPercent >= 0) && (CurrentPercent <= 100))
                            {
                                AR_Wrk.ReportProgress(CurrentPercent);
                            }
                        }
                        catch (Exception Ex) { Logger.Warn(Ex); }
                    }
                }
            }
            else
            {
                throw new FileNotFoundException(AppStrings.AR_BkgWrkExText, Arguments[0]);
            }
        }

        /// <summary>
        /// Reports progress to progress bar on form.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void AR_Wrk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            AR_PrgBr.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Finalizes unpacking procedure.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void AR_Wrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsRunning = false;

            if (e.Error != null)
            {
                MessageBox.Show(AppStrings.AR_UnpackException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(e.Error, DebugStrings.AppDbgExArWrkUnpack);
            }

            Close();
        }

        /// <summary>
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmArchWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Blocking ability to close form window during extraction process...
            e.Cancel = IsRunning;
        }
    }
}
