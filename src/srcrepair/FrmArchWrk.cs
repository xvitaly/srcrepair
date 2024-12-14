/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
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
    /// Class of the archive unpacking module.
    /// </summary>
    public partial class FrmArchWrk : Form
    {
        /// <summary>
        /// Logger instance for the FrmArchWrk class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Stores the full path to the archive file.
        /// </summary>
        private readonly string ArchiveName;

        /// <summary>
        /// Stores the full path to the destination directory.
        /// </summary>
        private readonly string DestinationDirectory;

        /// <summary>
        /// Stores the status of the currently running process.
        /// </summary>
        private bool IsRunning = true;

        /// <summary>
        /// FrmArchWrk class constructor.
        /// </summary>
        /// <param name="A">Full path to the archive file.</param>
        /// <param name="D">Full path to the destination directory.</param>
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
        private void UnpackArchiveFile(IProgress<int> Progress)
        {
            // Checking if archive file exists...
            if (File.Exists(ArchiveName))
            {
                // Opening archive...
                using (ZipArchive Zip = ZipFile.OpenRead(ArchiveName))
                {
                    // Creating some counters...
                    int TotalFiles = Zip.Entries.Count;
                    int CurrentFile = 1, CurrentPercent = 0, PreviousPercent = 0;

                    // Unpacking archive contents...
                    foreach (ZipArchiveEntry ZFile in Zip.Entries)
                    {
                        try
                        {
                            // Extracting file or directory...
                            string FullName = Path.GetFullPath(Path.Combine(DestinationDirectory, ZFile.FullName));
                            string DirectoryName = Path.GetDirectoryName(FullName);
                            if (!FullName.StartsWith(Path.GetFullPath(DestinationDirectory + Path.DirectorySeparatorChar))) { throw new InvalidOperationException(DebugStrings.AppDbgZipPathTraversalDetected); }
                            if (!Directory.Exists(DirectoryName)) { Directory.CreateDirectory(DirectoryName); }
                            if (!string.IsNullOrEmpty(ZFile.Name)) { ZFile.ExtractToFile(FullName, true); }

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
                throw new FileNotFoundException(DebugStrings.AppDbgZipExtractArchiveNotFound, ArchiveName);
            }
        }

        /// <summary>
        /// Asynchronously extracts archive to specified destination directory.
        /// </summary>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private async Task UnpackArchive(IProgress<int> Progress)
        {
            await Task.Run(() =>
            {
                UnpackArchiveFile(Progress);
            });
        }

        /// <summary>
        /// Reports progress to progress bar on form.
        /// </summary>
        /// <param name="Progress">Current progress percentage.</param>
        private void ReportUnpackProgress(int Progress)
        {
            AR_Progress.Value = Progress;
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
                await UnpackArchive(new Progress<int>(ReportUnpackProgress));
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
