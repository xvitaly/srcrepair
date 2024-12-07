/**
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
    /// Class of the Report builder module.
    /// </summary>
    public partial class FrmRepBuilder : Form
    {
        /// <summary>
        /// Logger instance for the FrmRepBuilder class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// CurrentPlatform instance for the FrmRepBuilder class.
        /// </summary>
        private readonly CurrentPlatform Platform = CurrentPlatform.Create();

        /// <summary>
        /// Stores the full path to the application logs directory.
        /// </summary>
        private readonly string AppLogDir;

        /// <summary>
        /// Stores the full path to the Steam client crash dumps directory.
        /// </summary>
        private readonly string FullSteamDumpsDir;

        /// <summary>
        /// Stores the full path to the Steam client logs directory.
        /// </summary>
        private readonly string FullSteamLogsDir;

        /// <summary>
        /// Stores an instance of the SourceGame class, selected in the
        /// main window.
        /// </summary>
        private readonly SourceGame SelectedGame;

        /// <summary>
        /// Stores an instance of the ReportManager class for working with the
        /// report targets.
        /// </summary>
        private readonly ReportManager RepMan;

        /// <summary>
        /// Stores the status of the currently running process.
        /// </summary>
        private bool IsRunning = false;

        /// <summary>
        /// Stores the status of the process completion event.
        /// </summary>
        private bool IsCompleted = false;

        /// <summary>
        /// FrmRepBuilder class constructor.
        /// </summary>
        /// <param name="A">Full path to the application reports directory.</param>
        /// <param name="L">Full path to the application logs directory.</param>
        /// <param name="SD">Full path to the Steam crash dumps directory.</param>
        /// <param name="SL">Full path to the Steam logs directory.</param>
        /// <param name="SG">Instance of the SourceGame class, selected in main window.</param>
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
        /// Creates a directory for storing generated reports and a working
        /// directory for temporary files.
        /// </summary>
        private void CreateDirectories()
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
        /// Creates a report file and saves it to disk.
        /// </summary>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private void CreateReportFile(IProgress<int> Progress)
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
        /// Removes a working directory with temporary files.
        /// </summary>
        private void CleanupDirectories()
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
                Logger.Warn(Ex, DebugStrings.AppDbgExRpCleanTempDir);
            }
        }

        /// <summary>
        /// Removes a generated report file.
        /// </summary>
        private void RemoveReportFile()
        {
            try
            {
                FileManager.RemoveFile(RepMan.ReportArchiveName);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExRpRemoveReportFile);
            }
        }

        /// <summary>
        /// Shows the generated report file in a default file manager.
        /// </summary>
        private void ShowReportFile()
        {
            try
            {
                Platform.OpenExplorer(RepMan.ReportArchiveName);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExRpShowReportFile);
            }
        }

        /// <summary>
        /// Enables the progress bar, hides some buttons and the form control box.
        /// </summary>
        private void FormStart()
        {
            IsRunning = true;
            RP_Progress.Visible = true;
            RP_Generate.Enabled = false;
            ControlBox = false;
        }

        /// <summary>
        /// Disables the progress bar, shows all previously hidden buttons and
        /// the form control box.
        /// </summary>
        private void FormFinalize()
        {
            IsRunning = false;
            RP_Progress.Visible = false;
            RP_Generate.Text = AppStrings.RP_CloseButtonText;
            RP_Generate.Enabled = true;
            ControlBox = true;
            IsCompleted = true;
        }

        /// <summary>
        /// Checks whether the report has been successfully generated and
        /// performs some actions.
        /// </summary>
        private void FormComplete()
        {
            if (File.Exists(RepMan.ReportArchiveName))
            {
                MessageBox.Show(string.Format(AppStrings.RP_ReportFileCreated, Path.GetFileName(RepMan.ReportArchiveName)), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowReportFile();
            }
            else
            {
                MessageBox.Show(AppStrings.RP_ReportFileNotCreated, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles errors during the report generation.
        /// </summary>
        private void FormError()
        {
            FormFinalize();
            RemoveReportFile();
            Close();
        }

        /// <summary>
        /// Asynchronously generates a report.
        /// </summary>
        /// <param name="Progress">Instance of IProgress interface for reporting progress.</param>
        private async Task CreateReport(IProgress<int> Progress)
        {
            await Task.Run(() =>
            {
                CreateDirectories();
                CreateReportFile(Progress);
                CleanupDirectories();
            });
        }

        /// <summary>
        /// Reports progress to the progress bar on the form.
        /// </summary>
        /// <param name="Progress">Current progress percentage.</param>
        private void ReportProgress(int Progress)
        {
            RP_Progress.Value = Progress;
        }

        /// <summary>
        /// "Generate/Close" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private async void RP_Generate_Click(object sender, EventArgs e)
        {
            if (!IsCompleted)
            {
                try
                {
                    FormStart();
                    await CreateReport(new Progress<int>(ReportProgress));
                    FormFinalize();
                    FormComplete();
                }
                catch (Exception Ex)
                {
                    Logger.Error(Ex, DebugStrings.AppDbgExRpGenerate);
                    MessageBox.Show(AppStrings.RP_GenerateReportError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FormError();
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
