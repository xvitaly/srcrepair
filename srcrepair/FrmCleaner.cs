/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2020 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2020 EasyCoding Team.
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
using System.Drawing;
using System.IO;
using System.Text;
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
                                    DItem.LastWriteTime.ToString()
                                }
                            };

                            // Adding file to main list and incrementing counter...
                            Invoke((MethodInvoker)delegate () { CM_FTable.Items.Add(LvItem); });
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
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmCleaner_Load(object sender, EventArgs e)
        {
            // Changing window title...
            Text = String.Format(Text, CleanInfo);
            
            // Starting searching for candidates...
            if (!GttWrk.IsBusy) { GttWrk.RunWorkerAsync(); }

            // Blocking selection if required...
            CM_FTable.Enabled = !IsReadOnly;
        }

        /// <summary>
        /// "On key down" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CM_FTable_KeyDown(object sender, KeyEventArgs e)
        {
            if (!GttWrk.IsBusy)
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
        /// Removes all selected files and directories.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void ClnWrk_DoWork(object sender, DoWorkEventArgs e)
        {
            // Extracting list from arguments...
            List<String> DeleteQueue = e.Argument as List<String>;

            // Creating backup if enabled or required by policy...
            if (Properties.Settings.Default.PackBeforeCleanup || ForceBackUp)
            {
                ClnWrk.ReportProgress(0, AppStrings.PS_ProgressArchive);
                if (!FileManager.CompressFiles(DeleteQueue, FileManager.GenerateBackUpFileName(FullBackUpDirPath, Properties.Resources.BU_PrefixDef)))
                {
                    Logger.Error(AppStrings.PS_ArchFailed);
                }
            }

            // Reporting new status...
            ClnWrk.ReportProgress(0, AppStrings.PS_ProgressCleanup);

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
                        ClnWrk.ReportProgress(CurrentPercent);
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
        /// Reports progress to progress bar on form.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void ClnWrk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PrbMain.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                CM_Info.Text = (string)e.UserState;
            }
        }

        /// <summary>
        /// Finalizes cleanup procedure.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void ClnWrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Reporing new status...
            CM_Info.Text = AppStrings.PS_ProgressFinished;

            // Checking async task results...
            if (e.Error == null)
            {
                MessageBox.Show(SuccessMessage, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(AppStrings.PS_CleanupErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(e.Error, DebugStrings.AppDbgExClnQueueRun);
            }

            // Closing form...
            Close();
        }

        /// <summary>
        /// "Execute cleanup" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CM_Clean_Click(object sender, EventArgs e)
        {
            if (CM_FTable.Items.Count > 0)
            {
                if (CM_FTable.CheckedItems.Count > 0)
                {
                    if (MessageBox.Show(String.Format(AppStrings.PS_CleanupExecuteQ, CleanInfo), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        ChangeControlsState();
                        if (!ClnWrk.IsBusy)
                        {
                            ClnWrk.RunWorkerAsync(GetDeleteFilesList());
                        }
                    }
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
                ProcessManager.OpenExplorer(CM_FTable.SelectedItems[0].ToolTipText, new CurrentPlatform().OS);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExClnFm);
            }
        }

        /// <summary>
        /// Finds candidates for deletion async.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void GttWrk_DoWork(object sender, DoWorkEventArgs e)
        {
            DetectFilesForCleanup(CleanDirs, IsRecursive);
        }

        /// <summary>
        /// Finalizes candidates find procedure.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void GttWrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Showing estimated of free space to be free after removing all found files...
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
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmCleaner_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && (ClnWrk.IsBusy || GttWrk.IsBusy);
        }
    }
}
