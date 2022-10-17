/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;

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
        /// Extracts archive to specified destination directory.
        /// </summary>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private void UnpackArchive(IProgress<int> Progress)
        {
            // Checking if archive file exists...
            if (File.Exists(ArchiveName))
            {
                // Opening archive...
                using (ZipArchive Zip = ZipFile.OpenRead(ArchiveName))
                {
                    // Creating some counters...
                    int TotalFiles = Zip.Entries.Count;
                    int CurrentFile = 1, CurrentPercent, PreviousPercent = 0;

                    // Unpacking archive contents...
                    foreach (ZipArchiveEntry ZFile in Zip.Entries)
                    {
                        try
                        {
                            // Extracting file or directory...
                            string FullName = Path.GetFullPath(Path.Combine(DestinationDirectory, ZFile.FullName));
                            if (!FullName.StartsWith(Path.GetFullPath(DestinationDirectory + Path.DirectorySeparatorChar))) { throw new InvalidOperationException(DebugStrings.AppDbgZipPathTraversalDetected); }
                            if (string.IsNullOrEmpty(ZFile.Name)) { Directory.CreateDirectory(FullName); } else { ZFile.ExtractToFile(FullName, true); }

                            // Reporting progress...
                            CurrentPercent = (int)Math.Round(CurrentFile / (double)TotalFiles * 100.00d, 0); CurrentFile++;
                            if ((CurrentPercent >= 0) && (CurrentPercent <= 100) && (CurrentPercent > PreviousPercent))
                            {
                                PreviousPercent = CurrentPercent;
                                Progress.Report(CurrentPercent);
                            }
                        }
                        catch (Exception Ex)
                        {
                            Logger.Warn(Ex, DebugStrings.AppDbgZipExtractFailure, ZFile.Name);
                        }
                    }
                }
            }
            else
            {
                throw new FileNotFoundException(DebugStrings.AppDebgZipExtractArchiveNotFound, ArchiveName);
            }
        }

        /// <summary>
        /// Asynchronously extracts archive to specified destination directory.
        /// </summary>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private async Task UnpackArchiveTask(IProgress<int> Progress)
        {
            await Task.Run(() =>
            {
                UnpackArchive(Progress);
            });
        }

        /// <summary>
        /// Reports progress to progress bar on form.
        /// </summary>
        /// <param name="Progress">Current progress percentage.</param>
        private void ReportUnpackProgress(int Progress)
        {
            AR_PrgBr.Value = Progress;
        }

        /// <summary>
        /// Finalizes archive unpacking procedure.
        /// </summary>
        private void FinalizeUnpack()
        {
            IsRunning = false;
            Close();
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private async void FrmArchWrk_Load(object sender, EventArgs e)
        {
            try
            {
                await UnpackArchiveTask(new Progress<int>(ReportUnpackProgress));
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, DebugStrings.AppDbgExArWrkUnpack);
                MessageBox.Show(AppStrings.AR_UnpackException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            FinalizeUnpack();
        }

        /// <summary>
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmArchWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Blocking ability to close form window during extraction process...
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && IsRunning;
        }
    }
}
