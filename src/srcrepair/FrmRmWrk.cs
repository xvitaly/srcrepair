/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
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
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmRmWrk_Load(object sender, EventArgs e)
        {
            // Starting async removal sequence...
            if (!RW_Wrk.IsBusy)
            {
                RW_Wrk.RunWorkerAsync(RemDirs);
            }
        }

        /// <summary>
        /// Removes all files and directories recursively from specified directories.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void RW_Wrk_DoWork(object sender, DoWorkEventArgs e)
        {
            // Parsing arguments list...
            List<String> Arguments = e.Argument as List<String>;

            // Searching for candidates...
            List<string> DeleteQueue = DetectFilesForCleanup(Arguments);

            // Creating some counters...
            int TotalFiles = DeleteQueue.Count;
            int CurrentFile = 1, CurrentPercent;

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
                    if ((CurrentPercent >= 0) && (CurrentPercent <= 100))
                    {
                        RW_Wrk.ReportProgress(CurrentPercent);
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex);
                }
            }

            // Removing empty directories after files removal...
            foreach (string Dir in Arguments)
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
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void RW_Wrk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RW_PrgBr.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Finalizes cleanup procedure.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void RW_Wrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsRunning = false;

            if (e.Error != null)
            {
                MessageBox.Show(AppStrings.RW_RmException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(e.Error, DebugStrings.AppDbgExRmRf);
            }

            Close();
        }

        /// <summary>
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmRmWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Blocking ability to close form window during cleanup process...
            e.Cancel = IsRunning;
        }
    }
}
