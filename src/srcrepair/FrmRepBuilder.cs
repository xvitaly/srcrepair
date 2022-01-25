﻿/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Ionic.Zip;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of report generator window.
    /// </summary>
    public partial class FrmRepBuilder : Form
    {
        /// <summary>
        /// Logger instance for FrmRepBuilder class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// CurrentPlatform instance for FrmRepBuilder class.
        /// </summary>
        private readonly CurrentPlatform Platform = CurrentPlatform.Create();

        /// <summary>
        /// FrmRepBuilder class constructor.
        /// </summary>
        /// <param name="A">Path to app's user directory.</param>
        /// <param name="FS">Full path to Steam client directory.</param>
        /// <param name="SG">Instance of SourceGame class, selected in main window.</param>
        public FrmRepBuilder(string A, string FS, SourceGame SG)
        {
            InitializeComponent();
            AppUserDir = A;
            FullSteamPath = FS;
            SelectedGame = SG;
        }

        /// <summary>
        /// Gets or sets status of currently running process.
        /// </summary>
        private bool IsCompleted { get; set; } = false;

        /// <summary>
        /// Gets or sets path to app's user directory.
        /// </summary>
        private string AppUserDir { get; set; }

        /// <summary>
        /// Gets or sets full path to Steam client directory.
        /// </summary>
        private string FullSteamPath { get; set; }

        /// <summary>
        /// Gets or sets instance of SourceGame class, selected in main window.
        /// </summary>
        private SourceGame SelectedGame { get; set; }

        /// <summary>
        /// Gets or sets instance of ReportManager class for managing generic
        /// report targets.
        /// </summary>
        private ReportManager RepMan { get; set; }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmRepBuilder_Load(object sender, EventArgs e)
        {
            RepMan = new ReportManager(AppUserDir);
        }

        /// <summary>
        /// Creates directories for storing generating report and working
        /// directory if does not exists.
        /// </summary>
        private void RepCreateDirectories()
        {
            if (!Directory.Exists(RepMan.ReportsDirectory))
            {
                Directory.CreateDirectory(RepMan.ReportsDirectory);
            }

            if (!Directory.Exists(RepMan.TempDirectory))
            {
                Directory.CreateDirectory(RepMan.TempDirectory);
            }
        }

        /// <summary>
        /// Creates compressed report and saves it to disk.
        /// </summary>
        private void RepCreateReport()
        {
            using (ZipFile ZBkUp = new ZipFile(RepMan.ReportArchiveName, Encoding.UTF8))
            {
                // Creating some counters...
                int TotalFiles = RepMan.ReportTargets.Count;
                int CurrentFile = 1, CurrentPercent;

                // Adding generic report files...
                foreach (ReportTarget RepTarget in RepMan.ReportTargets)
                {
                    ProcessManager.StartProcessAndWait(RepTarget.Program, String.Format(RepTarget.Parameters, RepTarget.OutputFileName));

                    CurrentPercent = (int)Math.Round(CurrentFile / (double)TotalFiles * 100.00d, 0); CurrentFile++;
                    if ((CurrentPercent >= 0) && (CurrentPercent <= 100))
                    {
                        BwGen.ReportProgress(CurrentPercent);
                    }

                    if (File.Exists(RepTarget.OutputFileName))
                    {
                        ZBkUp.AddFile(RepTarget.OutputFileName, RepTarget.ArchiveDirectoryName);
                    }
                    else
                    {
                        if (RepTarget.IsMandatory)
                        {
                            throw new FileNotFoundException(DebugStrings.AppDbgExRpMdrNotCreated);
                        }
                    }
                }

                // Adding game configs...
                if (Directory.Exists(SelectedGame.FullCfgPath))
                {
                    ZBkUp.AddDirectory(SelectedGame.FullCfgPath, "configs");
                }

                // Adding video file...
                if (SelectedGame.IsUsingVideoFile)
                {
                    string GameVideo = SelectedGame.GetActualVideoFile();
                    if (File.Exists(GameVideo))
                    {
                        ZBkUp.AddFile(GameVideo, "video");
                    }
                }

                // Adding Steam crash dumps...
                if (Directory.Exists(Path.Combine(FullSteamPath, "dumps")))
                {
                    ZBkUp.AddDirectory(Path.Combine(FullSteamPath, "dumps"), "dumps");
                }

                // Adding Steam logs...
                if (Directory.Exists(Path.Combine(FullSteamPath, "logs")))
                {
                    ZBkUp.AddDirectory(Path.Combine(FullSteamPath, "logs"), "logs");
                }

                // Adding Hosts file contents...
                if (File.Exists(Platform.HostsFileFullPath))
                {
                    ZBkUp.AddFile(Platform.HostsFileFullPath, "hosts");
                }

                // Adding application debug log...
                if (Directory.Exists(CurrentApp.LogDirectoryPath))
                {
                    ZBkUp.AddDirectory(CurrentApp.LogDirectoryPath, "debug");
                }

                // Saving Zip file to disk...
                ZBkUp.Save();
            }
        }

        /// <summary>
        /// Removes temporary working directory with all its contents.
        /// </summary>
        private void RepCleanupDirectories()
        {
            try
            {
                if (Directory.Exists(RepMan.TempDirectory))
                {
                    Directory.Delete(RepMan.TempDirectory, true);
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
            }
        }

        /// <summary>
        /// Removes generated report file if exists.
        /// </summary>
        private void RepRemoveArchive()
        {
            try
            {
                if (File.Exists(RepMan.ReportArchiveName))
                {
                    File.Delete(RepMan.ReportArchiveName);
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
            }
        }

        /// <summary>
        /// Enables status bar and hides some buttons.
        /// </summary>
        private void RepWindowStart()
        {
            RB_Progress.Visible = true;
            GenerateNow.Enabled = false;
            ControlBox = false;
        }

        /// <summary>
        /// Disables status bar and show all previously hidden buttons.
        /// </summary>
        private void RepWindowFinalize()
        {
            RB_Progress.Visible = false;
            GenerateNow.Text = AppStrings.RPB_CloseCpt;
            GenerateNow.Enabled = true;
            ControlBox = true;
            IsCompleted = true;
        }

        /// <summary>
        /// Generates report in a separate thread.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void BwGen_DoWork(object sender, DoWorkEventArgs e)
        {
            // Creating directories if does not exists...
            RepCreateDirectories();

            // Creating Zip archive...
            RepCreateReport();

            // Removing temporary files...
            RepCleanupDirectories();
        }

        /// <summary>
        /// Reports progress to progress bar on form.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Additional arguments.</param>
        private void BwGen_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RB_Progress.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Finalizes report generating procedure.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Completion arguments and results.</param>
        private void BwGen_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RepWindowFinalize();

            if (e.Error == null)
            {
                if (File.Exists(RepMan.ReportArchiveName))
                {
                    try
                    {
                        MessageBox.Show(String.Format(AppStrings.RPB_ComprGen, Path.GetFileName(RepMan.ReportArchiveName)), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Platform.OpenExplorer(RepMan.ReportArchiveName);
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, DebugStrings.AppDbgExRepFm);
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.PS_ArchFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.RPB_GenException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(e.Error, DebugStrings.AppDbgExRepPack);
                RepRemoveArchive();
            }
        }

        /// <summary>
        /// "Create/close" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void GenerateNow_Click(object sender, EventArgs e)
        {
            if (!IsCompleted)
            {
                RepWindowStart();

                if (!BwGen.IsBusy)
                {
                    BwGen.RunWorkerAsync();
                }
                else
                {
                    Logger.Warn(DebugStrings.AppDbgExRepWrkBusy);
                }
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// "Form close" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmRepBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Blocking ability to close form window during report generating process...
            e.Cancel = BwGen.IsBusy;
        }
    }
}
