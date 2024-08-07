﻿/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
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
        /// Stores the list of directories for cleanup.
        /// </summary>
        private readonly List<string> RemDirs;

        /// <summary>
        /// Stores status of currently running process.
        /// </summary>
        private bool IsRunning = true;

        /// <summary>
        /// FrmRmWrk class constructor.
        /// </summary>
        /// <param name="SL">List of directories for cleanup.</param>
        public FrmRmWrk(List<string> SL)
        {
            InitializeComponent();
            RemDirs = SL;
        }

        /// <summary>
        /// Gets full list of files for deletion.
        /// </summary>
        /// <param name="CleanDirs">List of directories for cleanup.</param>
        /// <returns>List of files for deletion.</returns>
        private List<string> DetectFilesForCleanup(List<string> CleanDirs)
        {
            // Generating an empty list for storing results...
            List<string> Result = new List<string>();

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
                    List<string> SubDirs = new List<string>();
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
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private void CleanupFiles(IProgress<int> Progress)
        {
            // Searching for candidates...
            List<string> DeleteQueue = DetectFilesForCleanup(RemDirs);

            // Creating some counters...
            int TotalFiles = DeleteQueue.Count;
            int CurrentFile = 1, CurrentPercent, PreviousPercent = 0;

            // Removing all files from list...
            foreach (string Fl in DeleteQueue)
            {
                try
                {
                    // Removing file if exists...
                    FileManager.RemoveFile(Fl);

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
            foreach (string Dir in RemDirs)
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
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private async Task CleanupFilesTask(IProgress<int> Progress)
        {
            await Task.Run(() =>
            {
                CleanupFiles(Progress);
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
                await CleanupFilesTask(new Progress<int>(ReportCleanupProgress));
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
