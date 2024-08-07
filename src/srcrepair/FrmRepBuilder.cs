﻿/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        /// Stores full path to the local logs directory.
        /// </summary>
        private readonly string AppLogDir;

        /// <summary>
        /// Stores full path to Steam client crash dumps directory.
        /// </summary>
        private readonly string FullSteamDumpsDir;

        /// <summary>
        /// Stores full path to Steam client logs directory.
        /// </summary>
        private readonly string FullSteamLogsDir;

        /// <summary>
        /// Stores an instance of the SourceGame class, selected in main window.
        /// </summary>
        private readonly SourceGame SelectedGame;

        /// <summary>
        /// Stores an instance of the ReportManager class for managing generic
        /// report targets.
        /// </summary>
        private readonly ReportManager RepMan;

        /// <summary>
        /// Stores status of currently running process.
        /// </summary>
        private bool IsRunning = false;

        /// <summary>
        /// Stores status of process completion event.
        /// </summary>
        private bool IsCompleted = false;

        /// <summary>
        /// FrmRepBuilder class constructor.
        /// </summary>
        /// <param name="A">Full path to the local reports directory.</param>
        /// <param name="L">Full path to the local logs directory.</param>
        /// <param name="SD">Full path to Steam crash dumps directory.</param>
        /// <param name="SL">Full path to Steam logs directory.</param>
        /// <param name="SG">Instance of SourceGame class, selected in main window.</param>
        public FrmRepBuilder(string A, string L, string SD, string SL, SourceGame SG)
        {
            InitializeComponent();
            RepMan = new ReportManager(A);
            AppLogDir = L;
            FullSteamDumpsDir = SD;
            FullSteamLogsDir = SL;
            SelectedGame = SG;
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
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private void RepCreateReport(IProgress<int> Progress)
        {
            using (ZipArchive ZBkUp = ZipFile.Open(RepMan.ReportArchiveName, ZipArchiveMode.Create, Encoding.UTF8))
            {
                // Creating some counters...
                int TotalFiles = RepMan.ReportTargets.Count;
                int CurrentFile = 1, CurrentPercent, PreviousPercent = 0;

                // Adding generic report files...
                foreach (ReportTarget RepTarget in RepMan.ReportTargets)
                {
                    ProcessManager.StartProcessAndWait(RepTarget.Program, string.Format(RepTarget.Parameters, RepTarget.OutputFileName));

                    CurrentPercent = (int)Math.Round(CurrentFile / (double)TotalFiles * 100.00d, 0); CurrentFile++;
                    if ((CurrentPercent >= 0) && (CurrentPercent <= 100) && (CurrentPercent > PreviousPercent))
                    {
                        PreviousPercent = CurrentPercent;
                        Progress.Report(CurrentPercent);
                    }

                    if (File.Exists(RepTarget.OutputFileName))
                    {
                        ZBkUp.CreateEntryFromFile(RepTarget.OutputFileName, Path.Combine(RepTarget.ArchiveDirectoryName, Path.GetFileName(RepTarget.OutputFileName)), CompressionLevel.Optimal);
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
                    ZBkUp.CreateEntryFromDirectory(SelectedGame.FullCfgPath, "configs", CompressionLevel.Optimal);
                }

                // Adding video file...
                if (SelectedGame.IsUsingVideoFile)
                {
                    string GameVideo = SelectedGame.GetActualVideoFile();
                    if (File.Exists(GameVideo))
                    {
                        ZBkUp.CreateEntryFromFile(GameVideo, Path.Combine("video", Path.GetFileName(GameVideo)), CompressionLevel.Optimal);
                    }
                }

                // Adding Steam crash dumps...
                if (Directory.Exists(FullSteamDumpsDir))
                {
                    ZBkUp.CreateEntryFromDirectory(FullSteamDumpsDir, "dumps", CompressionLevel.Optimal);
                }

                // Adding Steam logs...
                if (Directory.Exists(FullSteamLogsDir))
                {
                    ZBkUp.CreateEntryFromDirectory(FullSteamLogsDir, "logs", CompressionLevel.Optimal);
                }

                // Adding Hosts file contents...
                if (File.Exists(Platform.HostsFileFullPath))
                {
                    ZBkUp.CreateEntryFromFile(Platform.HostsFileFullPath, Path.Combine("hosts", Path.GetFileName(Platform.HostsFileFullPath)), CompressionLevel.Optimal);
                }

                // Adding application debug log...
                if (Directory.Exists(AppLogDir))
                {
                    ZBkUp.CreateEntryFromDirectory(AppLogDir, "debug", CompressionLevel.Optimal);
                }
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
                Logger.Warn(Ex, DebugStrings.AppDbgExRepDirRem);
            }
        }

        /// <summary>
        /// Removes generated report file if exists.
        /// </summary>
        private void RepRemoveArchive()
        {
            try
            {
                FileManager.RemoveFile(RepMan.ReportArchiveName);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExRepArchRem);
            }
        }

        /// <summary>
        /// Enables status bar and hides some buttons.
        /// </summary>
        private void RepWindowStart()
        {
            IsRunning = true;
            RB_Progress.Visible = true;
            GenerateNow.Enabled = false;
            ControlBox = false;
        }

        /// <summary>
        /// Disables status bar and show all previously hidden buttons.
        /// </summary>
        private void RepWindowFinalize()
        {
            IsRunning = false;
            RB_Progress.Visible = false;
            GenerateNow.Text = AppStrings.RPB_CloseCpt;
            GenerateNow.Enabled = true;
            ControlBox = true;
            IsCompleted = true;
        }

        /// <summary>
        /// Handles errors during report generation.
        /// </summary>
        private void RepWindowError()
        {
            RepWindowFinalize();
            RepRemoveArchive();
            Close();
        }

        /// <summary>
        /// Asynchronously generates reports.
        /// </summary>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private async Task CreateReportTask(IProgress<int> Progress)
        {
            await Task.Run(() =>
            {
                RepCreateDirectories();
                RepCreateReport(Progress);
                RepCleanupDirectories();
            });
        }

        /// <summary>
        /// Reports progress to progress bar on form.
        /// </summary>
        /// <param name="Progress">Current progress percentage.</param>
        private void ReportProgress(int Progress)
        {
            RB_Progress.Value = Progress;
        }

        /// <summary>
        /// "Create/close" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private async void GenerateNow_Click(object sender, EventArgs e)
        {
            if (!IsCompleted)
            {
                try
                {
                    RepWindowStart();
                    await CreateReportTask(new Progress<int>(ReportProgress));
                    RepWindowFinalize();
                    if (File.Exists(RepMan.ReportArchiveName))
                    {
                        try
                        {
                            MessageBox.Show(string.Format(AppStrings.RPB_ComprGen, Path.GetFileName(RepMan.ReportArchiveName)), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                catch (Exception Ex)
                {
                    Logger.Error(Ex, DebugStrings.AppDbgExRepPack);
                    MessageBox.Show(AppStrings.RPB_GenException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    RepWindowError();
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
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && IsRunning;
        }
    }
}
