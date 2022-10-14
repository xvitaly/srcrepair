/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of interactive cleanup window.
    /// </summary>
    public partial class FrmCleaner : Form
    {
        /// <summary>
        /// Logger instance for FrmCleaner class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// CurrentPlatform instance for FrmCleaner class.
        /// </summary>
        private readonly CurrentPlatform Platform = CurrentPlatform.Create();

        /// <summary>
        /// FrmCleaner class constructor.
        /// </summary>
        /// <param name="CD">List of files and directories for cleanup.</param>
        /// <param name="BD">Path to directory for saving backups.</param>
        /// <param name="CI">Cleanup window title.</param>
        /// <param name="SM">Successful cleanup completion message text.</param>
        /// <param name="RO">Allow user to manually select files for deletion.</param>
        /// <param name="NA">Disable automatically mark found files to deletion.</param>
        /// <param name="RS">Enable recursive cleanup.</param>
        /// <param name="FB">Force backup file creation before running cleanup.</param>
        public FrmCleaner(List<String> CD, string BD, string CI, string SM, bool RO, bool NA, bool RS, bool FB)
        {
            InitializeComponent();
            CleanDirs = CD;
            CleanInfo = CI;
            IsReadOnly = RO;
            NoAutoCheck = NA;
            IsRecursive = RS;
            ForceBackUp = FB;
            SuccessMessage = SM;
            FullBackUpDirPath = BD;
        }

        /// <summary>
        /// Scales controls on current form with some additional hacks applied.
        /// </summary>
        /// <param name="ScalingFactor">Scaling factor.</param>
        /// <param name="Bounds">Bounds of control.</param>
        protected override void ScaleControl(SizeF ScalingFactor, BoundsSpecified Bounds)
        {
            base.ScaleControl(ScalingFactor, Bounds);
            if (!DpiManager.CompareFloats(Math.Max(ScalingFactor.Width, ScalingFactor.Height), 1.0f))
            {
                DpiManager.ScaleColumnsInControl(CM_FTable, ScalingFactor);
            }
        }

        /// <summary>
        /// Gets or sets list of files and directories for cleanup.
        /// </summary>
        private List<String> CleanDirs { get; set; }

        /// <summary>
        /// Gets or sets if manual selection of files is allowed.
        /// </summary>
        private bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets if automatic files selection is disallowed.
        /// </summary>
        private bool NoAutoCheck { get; set; }

        /// <summary>
        /// Gets or sets if recursive cleanup is allowed.
        /// </summary>
        private bool IsRecursive { get; set; }

        /// <summary>
        /// Gets or sets if backups are forced.
        /// </summary>
        private bool ForceBackUp { get; set; }

        /// <summary>
        /// Gets or sets full path to directory for saving backups.
        /// </summary>
        private string FullBackUpDirPath { get; set; }

        /// <summary>
        /// Gets or sets successful cleanup completion message text.
        /// </summary>
        private string SuccessMessage { get; set; }

        /// <summary>
        /// Gets or sets cleanup window title.
        /// </summary>
        private string CleanInfo { get; set; }

        /// <summary>
        /// Gets or sets total files size (in bytes).
        /// </summary>
        private long TotalSize { get; set; } = 0;

        /// <summary>
        /// Gets or sets status of currently running process.
        /// </summary>
        private bool IsRunning { get; set; } = true;

        /// <summary>
        /// Gets full list of files for deletion.
        /// </summary>
        /// <param name="CleanDirs">List of files and directories for cleanup.</param>
        /// <param name="Recursive">Enable recursive cleanup.</param>
        private void DetectFilesForCleanup(List<String> CleanDirs, bool Recursive)
        {
            foreach (string DirMs in CleanDirs)
            {
                // Extracting directory path and file mask from combined string...
                string CleanDir = Path.GetDirectoryName(DirMs);
                string CleanMask = Path.GetFileName(DirMs);
                
                // Checking if directory exists...
                if (Directory.Exists(CleanDir))
                {
                    try
                    {
                        // Getting full contents of directory and adding them to result...
                        DirectoryInfo DInfo = new DirectoryInfo(CleanDir);
                        FileInfo[] DirList = DInfo.GetFiles(CleanMask);
                        foreach (FileInfo DItem in DirList)
                        {
                            ListViewItem LvItem = new ListViewItem(DItem.Name)
                            {
                                Checked = !NoAutoCheck,
                                ToolTipText = Path.Combine(CleanDir, DItem.Name),
                                SubItems =
                                {
                                    GuiHelpers.SclBytes(DItem.Length),
                                    DItem.LastWriteTime.ToString(CultureInfo.CurrentUICulture)
                                }
                            };

                            // Adding file to main list and incrementing counter...
                            Invoke((MethodInvoker)delegate { CM_FTable.Items.Add(LvItem); });
                            TotalSize += DItem.Length;
                        }

                        if (Recursive)
                        {
                            try
                            {
                                // Getting subdirectories...
                                List<String> SubDirs = new List<string>();
                                foreach (DirectoryInfo Dir in DInfo.GetDirectories())
                                {
                                    SubDirs.Add(Path.Combine(Dir.FullName, CleanMask));
                                }

                                // If subdirectories exists, run this method recursively...
                                if (SubDirs.Count > 0)
                                {
                                    DetectFilesForCleanup(SubDirs, true);
                                }
                            }
                            catch (Exception Ex)
                            {
                                Logger.Warn(Ex);
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex);
                    }
                }
            }
        }

        /// <summary>
        /// Changes state of some controls on form on cleanup start.
        /// </summary>
        private void ChangeControlsState()
        {
            // Setting new status...
            CM_Info.Text = AppStrings.PS_ProcessPrepare;
            IsRunning = true;

            // Disabling "Execute cleanup" button...
            CM_Clean.Text = AppStrings.PS_CleanInProgress;
            CM_Clean.Enabled = false;
            CM_Clean.Visible = false;

            // Changing the state of other controls...
            CM_Cancel.Enabled = false;
            CM_Cancel.Visible = false;
            PrbMain.Visible = true;
        }

        /// <summary>
        /// Reports progress to progress bar on form.
        /// </summary>
        /// <param name="Progress">Progress tuple.</param>
        private void ReportProgressChange(Tuple<int, String> Progress)
        {
            PrbMain.Value = Progress.Item1;
            if (!String.IsNullOrEmpty(Progress.Item2))
            {
                CM_Info.Text = Progress.Item2;
            }
        }

        /// <summary>
        /// Gets list of files for removal.
        /// </summary>
        /// <returns>List of files to be removed.</returns>
        private List<String> GetDeleteFilesList()
        {
            List<String> DeleteQueue = new List<String>();
            foreach (ListViewItem LVI in CM_FTable.Items)
            {
                if (LVI.Checked)
                {
                    DeleteQueue.Add(LVI.ToolTipText);
                }
            }
            return DeleteQueue;
        }

        /// <summary>
        /// Removes all selected files and directories.
        /// </summary>
        /// <param name="DeleteQueue">List of files and directories for deletion.</param>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private void DeleteCandidates(List<String> DeleteQueue, IProgress<Tuple<int, String>> Progress)
        {
            // Reporting new status...
            Progress.Report(new Tuple<int, String>(0, AppStrings.PS_ProgressCleanup));

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
                        Progress.Report(new Tuple<int, string>(CurrentPercent, null));
                    }
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex);
                }
            }

            // Removing empty directories if allowed...
            if (Properties.Settings.Default.RemoveEmptyDirs)
            {
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
        }

        /// <summary>
        /// Asynchronously checks candidates for deletion.
        /// </summary>
        private async Task FindCandidatesTask()
        {
            await Task.Run(() =>
            {
                DetectFilesForCleanup(CleanDirs, IsRecursive);
            });
        }

        /// <summary>
        /// Asynchronously backups selected files if enabled or required by policy.
        /// </summary>
        /// <param name="DeleteQueue">List of files and directories for deletion.</param>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private async Task BackUpCandidatesTask(List<String> DeleteQueue, IProgress<Tuple<int, String>> Progress)
        {
            await Task.Run(() =>
            {
                if (Properties.Settings.Default.PackBeforeCleanup || ForceBackUp)
                {
                    Progress.Report(new Tuple<int, String>(0, AppStrings.PS_ProgressArchive));
                    if (!FileManager.CompressFiles(DeleteQueue, FileManager.GenerateBackUpFileName(FullBackUpDirPath, Properties.Resources.BU_PrefixDef)))
                    {
                        Logger.Error(AppStrings.PS_ArchFailed);
                    }
                }
            });
        }

        /// <summary>
        /// Asynchronously deletes selected files.
        /// </summary>
        /// <param name="DeleteQueue">List of files and directories for deletion.</param>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private async Task DeleteCandidatesTask(List<String> DeleteQueue, IProgress<Tuple<int, String>> Progress)
        {
            await Task.Run(() =>
            {
                DeleteCandidates(DeleteQueue, Progress);
            });
        }

        /// <summary>
        /// Finalizes candidates find procedure.
        /// </summary>
        private void HandleCandidates()
        {
            // Changing state...
            IsRunning = false;

            // Showing estimated free space to be freed after removing all found files...
            CM_Info.Text = String.Format(AppStrings.PS_FrFInfo, GuiHelpers.SclBytes(TotalSize));

            // Checking if candidates are found...
            if (CM_FTable.Items.Count == 0)
            {
                // Nothing found. Showing message and closing form...
                MessageBox.Show(AppStrings.PS_LoadErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CM_Clean.Enabled = false;
                Close();
            }
            else
            {
                // At least one candidate found. Enabling cleanup button...
                CM_Clean.Enabled = true;
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
            Text = String.Format(Text, CleanInfo);
            CM_FTable.Enabled = !IsReadOnly;

            // Starting searching for candidates...
            CM_FTable.BeginUpdate();
            await FindCandidatesTask();
            CM_FTable.EndUpdate();

            // Handling candidates...
            HandleCandidates();
        }

        /// <summary>
        /// "On key down" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CM_FTable_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsRunning)
            {
                // "Ctrl + A" pressed...
                if (e.Control && e.KeyCode == Keys.A)
                {
                    foreach (ListViewItem LVI in CM_FTable.Items)
                    {
                        LVI.Checked = true;
                    }
                }

                // "Ctrl + D" pressed...
                if (e.Control && e.KeyCode == Keys.D)
                {
                    foreach (ListViewItem LVI in CM_FTable.Items)
                    {
                        LVI.Checked = false;
                    }
                }

                // "Ctrl + R" pressed...
                if (e.Control && e.KeyCode == Keys.R)
                {
                    foreach (ListViewItem LVI in CM_FTable.Items)
                    {
                        LVI.Checked = !LVI.Checked;
                    }
                }

                // "Ctrl + C" pressed...
                if (e.Control && e.KeyCode == Keys.C)
                {
                    StringBuilder SelectedFiles = new StringBuilder();
                    foreach (ListViewItem LVI in CM_FTable.Items)
                    {
                        if (LVI.Checked)
                        {
                            SelectedFiles.AppendLine(LVI.ToolTipText);
                        }
                    }
                    Clipboard.SetText(SelectedFiles.ToString());
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
            if (MessageBox.Show(String.Format(AppStrings.PS_CleanupExecuteQ, CleanInfo), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (CM_FTable.Items.Count > 0)
                {
                    if (CM_FTable.CheckedItems.Count > 0)
                    {
                        ChangeControlsState();
                        try
                        {
                            List<String> Candidates = GetDeleteFilesList();
                            Progress<Tuple<int, String>> Progress = new Progress<Tuple<int, String>>(ReportProgressChange);
                            await BackUpCandidatesTask(Candidates, Progress);
                            await DeleteCandidatesTask(Candidates, Progress);
                            CM_Info.Text = AppStrings.PS_ProgressFinished;
                            MessageBox.Show(SuccessMessage, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception Ex)
                        {
                            Logger.Error(Ex, DebugStrings.AppDbgExClnQueueRun);
                            MessageBox.Show(AppStrings.PS_CleanupErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        IsRunning = false;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show(AppStrings.PS_SelectItemsMsg, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.PS_LoadErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            // Workaround to known bug with unchecking selected item on double click...
            CM_FTable.SelectedItems[0].Checked = !CM_FTable.SelectedItems[0].Checked;

            try
            {
                // Starting default shell and selecting file in its window...
                Platform.OpenExplorer(CM_FTable.SelectedItems[0].ToolTipText);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExClnFm);
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
