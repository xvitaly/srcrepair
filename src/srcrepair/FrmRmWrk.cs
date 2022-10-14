/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of recursive directory cleanup window.
    /// </summary>
    public partial class FrmRmWrk : Form
    {
        /// <summary>
        /// Logger instance for FrmRmWrk class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets or sets status of currently running process.
        /// </summary>
        private bool IsRunning { get; set; } = true;

        /// <summary>
        /// Gets or sets list of directories for cleanup.
        /// </summary>
        private List<String> RemDirs { get; set; }

        /// <summary>
        /// FrmRmWrk class constructor.
        /// </summary>
        /// <param name="SL">List of directories for cleanup.</param>
        public FrmRmWrk(List<String> SL)
        {
            InitializeComponent();
            RemDirs = SL;
        }

        /// <summary>
        /// Gets full list of files for deletion.
        /// </summary>
        /// <param name="CleanDirs">List of directories for cleanup.</param>
        /// <returns>List of files for deletion.</returns>
        private List<String> DetectFilesForCleanup(List<String> CleanDirs)
        {
            // Generating an empty list for storing results...
            List<String> Result = new List<String>();

            // Expanding every directory from source list...
            foreach (string CleanCnd in CleanDirs)
            {
                // Checking if directory exists...
                if (Directory.Exists(CleanCnd))
                {
                    // Getting full contents of directory and adding them to result...
                    DirectoryInfo DInfo = new DirectoryInfo(CleanCnd);
                    FileInfo[] DirList = DInfo.GetFiles("*.*");
                    foreach (FileInfo DItem in DirList)
                    {
                        Result.Add(DItem.FullName);
                    }

                    // Getting subdirectories...
                    List<String> SubDirs = new List<string>();
                    foreach (DirectoryInfo Dir in DInfo.GetDirectories())
                    {
                        SubDirs.Add(Path.Combine(Dir.FullName));
                    }

                    // If subdirectories exists, run this method recursively...
                    if (SubDirs.Count > 0)
                    {
                        Result.AddRange(DetectFilesForCleanup(SubDirs));
                    }
                }
                else
                {
                    // If directory does not exists, checking for file with the same name...
                    if (File.Exists(CleanCnd))
                    {
                        Result.Add(CleanCnd);
                    }
                }
            }

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Removes all files and directories recursively from specified directories.
        /// </summary>
        /// <param name="CleanDirs">List directories for cleanup.</param>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private void CleanupFiles(List<String> CleanDirs, IProgress<int> Progress)
        {
            // Searching for candidates...
            List<string> DeleteQueue = DetectFilesForCleanup(CleanDirs);

            // Creating some counters...
            int TotalFiles = DeleteQueue.Count;
            int CurrentFile = 1, CurrentPercent, PreviousPercent = 0;

            // Removing all files from list...
            foreach (string Fl in DeleteQueue)
            {
                try
                {
                    // Removing file if exists...
                    if (File.Exists(Fl))
                    {
                        File.SetAttributes(Fl, FileAttributes.Normal);
                        File.Delete(Fl);
                    }

                    // Reporting progress to form...
                    CurrentPercent = (int)Math.Round(CurrentFile / (double)TotalFiles * 100.00d, 0); CurrentFile++;
                    if ((CurrentPercent >= 0) && (CurrentPercent <= 100) && (CurrentPercent > PreviousPercent))
                    {
                        PreviousPercent = CurrentPercent;
                        Progress.Report(CurrentPercent);
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex);
                }
            }

            // Removing empty directories after files removal...
            foreach (string Dir in CleanDirs)
            {
                try
                {
                    FileManager.RemoveEmptyDirectories(Path.GetDirectoryName(Dir));
                }
                catch (Exception Ex)
                {
                    Logger.Error(Ex, DebugStrings.AppDbgExClnEmptyDirs);
                }
            }
        }

        /// <summary>
        /// Reports progress to progress bar on form.
        /// </summary>
        /// <param name="Progress">Current progress percentage.</param>
        private void ReportCleanupProgress(int Progress)
        {
            RW_PrgBr.Value = Progress;
        }

        /// <summary>
        /// Finalizes cleanup procedure.
        /// </summary>
        private void FinalizeCleanup()
        {
            IsRunning = false;
            Close();
        }

        /// <summary>
        /// Asynchronously deletes all files and directories recursively from specified
        /// directories.
        /// </summary>
        /// <param name="CleanDirs">List directories for cleanup.</param>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private async Task CleanupFilesTask(List<String> CleanDirs, IProgress<int> Progress)
        {
            await Task.Run(() =>
            {
                CleanupFiles(CleanDirs, Progress);
            });
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private async void FrmRmWrk_Load(object sender, EventArgs e)
        {
            try
            {
                await CleanupFilesTask(RemDirs, new Progress<int>(ReportCleanupProgress));
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, DebugStrings.AppDbgExRmRf);
                MessageBox.Show(AppStrings.RW_RmException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            FinalizeCleanup();
        }

        /// <summary>
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmRmWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Blocking ability to close form window during cleanup process...
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && IsRunning;
        }
    }
}
