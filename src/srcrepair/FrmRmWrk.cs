﻿/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
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
    /// Class of the file remover module.
    /// </summary>
    public partial class FrmRmWrk : Form
    {
        /// <summary>
        /// Logger instance for the FrmRmWrk class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Stores the list of items for deletion.
        /// </summary>
        private readonly List<string> RemItems;

        /// <summary>
        /// Stores the status of the currently running process.
        /// </summary>
        private bool IsRunning = true;

        /// <summary>
        /// FrmRmWrk class constructor.
        /// </summary>
        /// <param name="SL">The list of files and directories for deletion.</param>
        public FrmRmWrk(List<string> SL)
        {
            InitializeComponent();
            RemItems = SL;
        }

        /// <summary>
        /// Finds and deletes all files and directories recursively.
        /// </summary>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private void DeleteItems(IProgress<int> Progress)
        {
            // Searching for files for deletion...
            List<string> DeleteQueue = FileManager.FindFiles(RemItems);

            // Creating local variables for various counters...
            int TotalFiles = DeleteQueue.Count;
            int CurrentFile = 1, CurrentPercent, PreviousPercent = 0;

            // Removing all files from the list...
            foreach (string Fl in DeleteQueue)
            {
                try
                {
                    // Removing file if exists...
                    FileManager.RemoveFile(Fl);

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
                    Logger.Warn(Ex, DebugStrings.AppDbgExRmFileFailure, Fl);
                }
            }

            // Removing empty directories after the files removal...
            foreach (string Item in RemItems)
            {
                try
                {
                    if (Directory.Exists(Item))
                    {
                        FileManager.RemoveEmptyDirectories(Item);
                    }
                    else if (File.Exists(Item))
                    {
                        FileManager.RemoveEmptyDirectories(Path.GetDirectoryName(Item));
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, DebugStrings.AppDbgExRmEmptyDirs);
                }
            }
        }

        /// <summary>
        /// Reports progress to progress bar on form.
        /// </summary>
        /// <param name="Progress">Current progress percentage.</param>
        private void ReportProgress(int Progress)
        {
            RM_Progress.Value = Progress;
        }

        /// <summary>
        /// Asynchronously deletes all files and directories recursively.
        /// </summary>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private async Task DeleteFiles(IProgress<int> Progress)
        {
            await Task.Run(() =>
            {
                DeleteItems(Progress);
            });
        }

        /// <summary>
        /// Asynchronously deletes all files and directories recursively
        /// in a separate thread and reports progress.
        /// </summary>
        private async Task FormStart()
        {
            try
            {
                await DeleteFiles(new Progress<int>(ReportProgress));
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, DebugStrings.AppDbgExRmTaskError);
                MessageBox.Show(AppStrings.RM_RemoveFilesError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private async void FrmRmWrk_Load(object sender, EventArgs e)
        {
            await FormStart();
            FormFinalize();
        }

        /// <summary>
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmRmWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && IsRunning;
        }
    }
}
