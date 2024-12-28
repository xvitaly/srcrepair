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
        /// <param name="Archive">Full path to the archive file.</param>
        /// <param name="DestDir">Full path to the destination directory.</param>
        public FrmArchWrk(string Archive, string DestDir)
        {
            InitializeComponent();
            ArchiveName = Archive;
            DestinationDirectory = DestDir;
        }

        /// <summary>
        /// Extracts the archive file to the destination directory.
        /// </summary>
        /// <param name="Progress">Instance of the IProgress interface for reporting progress.</param>
        private void UnpackArchiveFile(IProgress<int> Progress)
        {
            // Opening the archive file...
            using (ZipArchive Zip = ZipFile.OpenRead(ArchiveName))
            {
                // Creating local variables for various counters...
                int TotalFiles = Zip.Entries.Count;
                int CurrentFile = 1, CurrentPercent, PreviousPercent = 0;

                // Unpacking the archive contents...
                foreach (ZipArchiveEntry ZFile in Zip.Entries)
                {
                    try
                    {
                        // Extracting files and directories...
                        string FullName = Path.GetFullPath(Path.Combine(DestinationDirectory, ZFile.FullName));
                        string DirectoryName = Path.GetDirectoryName(FullName);
                        if (!FullName.StartsWith(Path.GetFullPath(DestinationDirectory + Path.DirectorySeparatorChar))) { throw new InvalidOperationException(DebugStrings.AppDbgExArPathTraversalDetected); }
                        if (!Directory.Exists(DirectoryName)) { Directory.CreateDirectory(DirectoryName); }
                        if (!string.IsNullOrEmpty(ZFile.Name)) { ZFile.ExtractToFile(FullName, true); }

                        // Reporting the progress...
                        CurrentPercent = (int)Math.Round(CurrentFile / (double)TotalFiles * 100.00d, 0); CurrentFile++;
                        if ((CurrentPercent >= 0) && (CurrentPercent <= 100) && (CurrentPercent > PreviousPercent))
                        {
                            PreviousPercent = CurrentPercent;
                            Progress.Report(CurrentPercent);
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, DebugStrings.AppDbgExArUnpackArchiveError, ZFile.Name, ArchiveName);
                    }
                }
            }
        }

        /// <summary>
        /// Asynchronously extracts the archive file to the destination directory.
        /// </summary>
        /// <param name="Progress">Instance of the IProgress interface for reporting progress.</param>
        private async Task UnpackArchive(IProgress<int> Progress)
        {
            await Task.Run(() =>
            {
                UnpackArchiveFile(Progress);
            });
        }

        /// <summary>
        /// Reports progress to the progress bar on the form.
        /// </summary>
        /// <param name="Progress">Current progress percentage.</param>
        private void ReportProgress(int Progress)
        {
            AR_Progress.Value = Progress;
        }

        /// <summary>
        /// Asynchronously unpacks the archive file in a separate thread
        /// and reports progress.
        /// </summary>
        private async Task FormStart()
        {
            try
            {
                await UnpackArchive(new Progress<int>(ReportProgress));
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, DebugStrings.AppDbgExArTaskError, ArchiveName);
                MessageBox.Show(AppStrings.AR_UnpackArchiveError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private async void FrmArchWrk_Load(object sender, EventArgs e)
        {
            await FormStart();
            FormFinalize();
        }

        /// <summary>
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmArchWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && IsRunning;
        }
    }
}
