﻿/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of the cleanup module.
    /// </summary>
    public partial class FrmCleaner : Form
    {
        /// <summary>
        /// Logger instance for the FrmCleaner class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// CurrentPlatform instance for the FrmCleaner class.
        /// </summary>
        private readonly CurrentPlatform Platform = CurrentPlatform.Create();

        /// <summary>
        /// Stores the list of items for cleanup.
        /// </summary>
        private readonly List<string> CleanItems;

        /// <summary>
        /// Stores whether the manual file selection is enabled.
        /// </summary>
        private readonly bool IsReadOnly;

        /// <summary>
        /// Stores whether the automatic file selection is disabled.
        /// </summary>
        private readonly bool NoAutoCheck;

        /// <summary>
        /// Stores whether the recursive cleanup is enabled.
        /// </summary>
        private readonly bool IsRecursive;

        /// <summary>
        /// Stores whether a backup file creation is required.
        /// </summary>
        private readonly bool ForceBackUp;

        /// <summary>
        /// Stores the full path to the directory for saving backups.
        /// </summary>
        private readonly string FullBackUpDirPath;

        /// <summary>
        /// Stores the message text of the successful cleanup completion.
        /// </summary>
        private readonly string SuccessMessage;

        /// <summary>
        /// Stores the cleanup module form title.
        /// </summary>
        private readonly string CleanInfo;

        /// <summary>
        /// Stores total files size (in bytes).
        /// </summary>
        private long TotalSize = 0;

        /// <summary>
        /// Stores the status of the currently running process.
        /// </summary>
        private bool IsRunning = true;

        /// <summary>
        /// FrmCleaner class constructor.
        /// </summary>
        /// <param name="CD">The list of files and directories for cleanup.</param>
        /// <param name="BD">Path to the directory for saving backups.</param>
        /// <param name="CI">Cleanup window title.</param>
        /// <param name="SM">Successful cleanup completion message text.</param>
        /// <param name="RO">Allow user to manually select files for deletion.</param>
        /// <param name="NA">Disable automatically mark found files to deletion.</param>
        /// <param name="RS">Enable recursive cleanup.</param>
        /// <param name="FB">Force backup file creation before running cleanup.</param>
        public FrmCleaner(List<string> CD, string BD, string CI, string SM, bool RO, bool NA, bool RS, bool FB)
        {
            InitializeComponent();
            CleanItems = CD;
            CleanInfo = CI;
            IsReadOnly = RO;
            NoAutoCheck = NA;
            IsRecursive = RS;
            ForceBackUp = FB;
            SuccessMessage = SM;
            FullBackUpDirPath = BD;
        }

        /// <summary>
        /// Scales controls on current form with some additional workarounds applied.
        /// </summary>
        /// <param name="ScalingFactor">Scaling factor.</param>
        /// <param name="ControlBounds">The bounds of the control.</param>
        protected override void ScaleControl(SizeF ScalingFactor, BoundsSpecified ControlBounds)
        {
            base.ScaleControl(ScalingFactor, ControlBounds);
            if (!DpiManager.CompareFloats(Math.Max(ScalingFactor.Width, ScalingFactor.Height), 1.0f))
            {
                DpiManager.ScaleColumnsInControl(CM_FTable, ScalingFactor);
            }
        }

        /// <summary>
        /// Finds files for deletion.
        /// </summary>
        /// <param name="Targets">List of files and directories for cleanup.</param>
        /// <param name="Recursive">Enable recursive cleanup.</param>
        private void DetectFilesForCleanup(List<string> Targets, bool Recursive)
        {
            foreach (string DirMs in Targets)
            {
                string CleanDir = Path.GetDirectoryName(DirMs);
                string CleanMask = Path.GetFileName(DirMs);

                try
                {
                    DirectoryInfo DInfo = new DirectoryInfo(CleanDir);
                    if (!DInfo.Exists) { continue; }
                    foreach (FileInfo DItem in DInfo.EnumerateFiles(CleanMask, Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                    {
                        ListViewItem LvItem = new ListViewItem(DItem.Name)
                        {
                            Checked = !NoAutoCheck,
                            ToolTipText = DItem.FullName,
                            SubItems =
                            {
                                GuiHelpers.SclBytes(DItem.Length),
                                DItem.LastWriteTime.ToString(CultureInfo.CurrentCulture)
                            }
                        };

                        Invoke((MethodInvoker)delegate { CM_FTable.Items.Add(LvItem); });
                        TotalSize += DItem.Length;
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, DebugStrings.AppDbgExCmDetectFiles, CleanDir, CleanMask);
                }
            }
        }

        /// <summary>
        /// Changes the state of some controls on form on cleanup start.
        /// </summary>
        private void FormStart()
        {
            // Setting new status...
            CM_Info.Text = AppStrings.CM_ProgressPrepare;
            IsRunning = true;

            // Disabling "Execute cleanup" button...
            CM_Clean.Text = AppStrings.CM_ButtonWorking;
            CM_Clean.Enabled = false;
            CM_Clean.Visible = false;

            // Changing the state of other controls...
            CM_Cancel.Enabled = false;
            CM_Cancel.Visible = false;
            CM_Progress.Visible = true;
        }

        /// <summary>
        /// Reports progress to the progress bar on the form.
        /// </summary>
        /// <param name="Progress">Current progress tuple.</param>
        private void ReportProgress(Tuple<int, string> Progress)
        {
            CM_Progress.Value = Progress.Item1;
            if (!string.IsNullOrEmpty(Progress.Item2))
            {
                CM_Info.Text = Progress.Item2;
            }
        }

        /// <summary>
        /// Gets the list of selected files for deletion.
        /// </summary>
        /// <returns>List of selected files for deletion.</returns>
        private List<string> GetSelectedFiles()
        {
            List<string> Result = new List<string>();
            foreach (ListViewItem Item in CM_FTable.Items)
            {
                if (Item.Checked)
                {
                    Result.Add(Item.ToolTipText);
                }
            }
            return Result;
        }

        /// <summary>
        /// Removes all selected files and directories.
        /// </summary>
        /// <param name="DeleteQueue">List of files and directories for deletion.</param>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private void DeleteSelectedFiles(List<string> DeleteQueue, IProgress<Tuple<int, string>> Progress)
        {
            // Reporting new status...
            Progress.Report(new Tuple<int, string>(0, AppStrings.CM_ProgressCleanup));

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
                        Progress.Report(new Tuple<int, string>(CurrentPercent, null));
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, DebugStrings.AppDbgExCmDeleteFiles, Fl);
                }
            }

            // Removing empty directories if allowed...
            if (Properties.Settings.Default.RemoveEmptyDirs)
            {
                foreach (string Item in CleanItems)
                {
                    try
                    {
                        FileManager.RemoveEmptyDirectories(Path.GetDirectoryName(Item));
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, DebugStrings.AppDbgExCmEmptyDirs);
                    }
                }
            }
        }

        /// <summary>
        /// Asynchronously finds files for deletion.
        /// </summary>
        private async Task FindFiles()
        {
            await Task.Run(() =>
            {
                DetectFilesForCleanup(CleanItems, IsRecursive);
            });
        }

        /// <summary>
        /// Asynchronously backups selected files if enabled by the user or required by policy.
        /// </summary>
        /// <param name="DeleteQueue">List of files and directories for deletion.</param>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private async Task BackUpFiles(List<string> DeleteQueue, IProgress<Tuple<int, string>> Progress)
        {
            await Task.Run(() =>
            {
                if (Properties.Settings.Default.PackBeforeCleanup || ForceBackUp)
                {
                    Progress.Report(new Tuple<int, string>(0, AppStrings.CM_ProgressArchive));
                    if (!FileManager.CompressFiles(DeleteQueue, FileManager.GenerateBackUpFileName(FullBackUpDirPath, Properties.Resources.BU_PrefixDef)))
                    {
                        Logger.Warn(DebugStrings.AppDbgExCmBackUpFiles);
                    }
                }
            });
        }

        /// <summary>
        /// Asynchronously deletes selected files.
        /// </summary>
        /// <param name="DeleteQueue">List of files and directories for deletion.</param>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private async Task DeleteFiles(List<string> DeleteQueue, IProgress<Tuple<int, string>> Progress)
        {
            await Task.Run(() =>
            {
                DeleteSelectedFiles(DeleteQueue, Progress);
            });
        }

        /// <summary>
        /// Asynchronously starts the cleanup process and report progress.
        /// </summary>
        private async Task StartCleanup()
        {
            try
            {
                List<string> DeleteQueue = GetSelectedFiles();
                Progress<Tuple<int, string>> Progress = new Progress<Tuple<int, string>>(ReportProgress);
                await BackUpFiles(DeleteQueue, Progress);
                await DeleteFiles(DeleteQueue, Progress);
                CM_Info.Text = AppStrings.CM_ProgressFinished;
                MessageBox.Show(SuccessMessage, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, DebugStrings.AppDbgExCmCleanupError);
                MessageBox.Show(AppStrings.CM_CleanupError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Checks if any files were found and performs initialization actions
        /// depending on this.
        /// </summary>
        private void FormInitialize()
        {
            // Changing the form state...
            IsRunning = false;

            // Showing the estimated free space to be freed after removing all found files...
            CM_Info.Text = string.Format(AppStrings.PS_FrFInfo, GuiHelpers.SclBytes(TotalSize));

            // Checking if any files were found...
            if (CM_FTable.Items.Count == 0)
            {
                // Nothing found. Showing an error message and closing the form...
                CM_Clean.Enabled = false;
                MessageBox.Show(AppStrings.CM_NoFilesDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                // At least one file found. Enabling cleanup button...
                CM_Clean.Enabled = true;
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
        /// Selects all files from the table.
        /// </summary>
        private void HandleSelectAll()
        {
            foreach (ListViewItem Item in CM_FTable.Items)
            {
                Item.Checked = true;
            }
        }

        /// <summary>
        /// Deselects all files from the table.
        /// </summary>
        private void HandleDeselectAll()
        {
            foreach (ListViewItem Item in CM_FTable.Items)
            {
                Item.Checked = false;
            }
        }

        /// <summary>
        /// Selects unselected files from the table and vice versa.
        /// </summary>
        private void HandleInvertSelection()
        {
            foreach (ListViewItem Item in CM_FTable.Items)
            {
                Item.Checked = !Item.Checked;
            }
        }

        /// <summary>
        /// Copies full paths of the selected files to the clipboard.
        /// </summary>
        private void HandleCopyToClipboard()
        {
            try
            {
                string Result = string.Join(Environment.NewLine, GetSelectedFiles());
                if (!string.IsNullOrWhiteSpace(Result))
                {
                    Clipboard.SetText(Result);
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExCmCopyToClipboard);
            }
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private async void FrmCleaner_Load(object sender, EventArgs e)
        {
            // Setting form properties...
            Text = string.Format(Text, CleanInfo);
            CM_FTable.Enabled = !IsReadOnly;

            // Starting searching for files...
            CM_FTable.BeginUpdate();
            await FindFiles();
            CM_FTable.EndUpdate();

            // Processing found files...
            FormInitialize();
        }

        /// <summary>
        /// "On key up" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CM_FTable_KeyUp(object sender, KeyEventArgs e)
        {
            if (!IsRunning && e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.A: // "Ctrl + A"...
                        HandleSelectAll();
                        break;
                    case Keys.D: // "Ctrl + D"...
                        HandleDeselectAll();
                        break;
                    case Keys.R: // "Ctrl + R"...
                        HandleInvertSelection();
                        break;
                    case Keys.C: // "Ctrl + C"...
                        HandleCopyToClipboard();
                        break;
                    default: // Unknown combination...
                        Logger.Warn(DebugStrings.AppDbgExClnHotkeyUnknown, e.KeyCode);
                        break;
                }
            }
        }

        /// <summary>
        /// "Execute cleanup" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private async void CM_Clean_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format(AppStrings.CM_CleanupQuestion, CleanInfo), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (CM_FTable.CheckedItems.Count > 0)
                {
                    FormStart();
                    await StartCleanup();
                    FormFinalize();
                }
                else
                {
                    MessageBox.Show(AppStrings.CM_NoFilesSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// "Cancel" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CM_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// ListView item double click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CM_FTable_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                // Workaround for known bug where double clicking on selected item would uncheck it...
                CM_FTable.SelectedItems[0].Checked = !CM_FTable.SelectedItems[0].Checked;

                // Starting the default file manager and shows the selected file in its window...
                Platform.OpenExplorer(CM_FTable.SelectedItems[0].ToolTipText);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExCmFileManager);
            }
        }

        /// <summary>
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmCleaner_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && IsRunning;
        }
    }
}
