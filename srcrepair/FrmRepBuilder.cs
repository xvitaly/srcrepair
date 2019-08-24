/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2019 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2019 EasyCoding Team.
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
using System.IO;
using System.Text;
using System.Windows.Forms;
using Ionic.Zip;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Класс формы модуля создания отчётов для техподдержки.
    /// </summary>
    public partial class FrmRepBuilder : Form
    {
        /// <summary>
        /// Управляет записью событий в журнал.
        /// </summary>
        private Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Конструктор класса формы модуля создания отчётов для техподдержки.
        /// </summary>
        /// <param name="A">Путь к каталогу пользователя</param>
        /// <param name="FS">Путь к каталогу установки Steam</param>
        /// <param name="SG">Конфигурация выбранной в главном окне игры</param>
        public FrmRepBuilder(string A, string FS, SourceGame SG)
        {
            InitializeComponent();
            AppUserDir = A;
            FullSteamPath = FS;
            SelectedGame = SG;
        }

        /// <summary>
        /// Хранит название модуля для внутренних целей.
        /// </summary>
        private const string PluginName = "Report Builder";

        /// <summary>
        /// Хранит статус выполнения процесса создания отчёта.
        /// </summary>
        private bool IsCompleted { get; set; } = false;

        /// <summary>
        /// Хранит путь к пользовательскому каталогу приложения.
        /// </summary>
        private string AppUserDir { get; set; }

        /// <summary>
        /// Хранит путь к каталогу установки клиента Steam.
        /// </summary>
        private string FullSteamPath { get; set; }

        /// <summary>
        /// Хранит конфигурацию выбранной в главном окне игры.
        /// </summary>
        private SourceGame SelectedGame { get; set; }

        private ReportManager RepMan { get; set; }

        /// <summary>
        /// Метод, срабатывающий при возникновении события "загрузка формы".
        /// </summary>
        private void FrmRepBuilder_Load(object sender, EventArgs e)
        {
            RepMan = new ReportManager(AppUserDir);
        }

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
                            throw new FileNotFoundException("Mandatory report was not created.");
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
                if (File.Exists(FileManager.GetHostsFileFullPath(CurrentPlatform.OSType.Windows)))
                {
                    ZBkUp.AddFile(FileManager.GetHostsFileFullPath(CurrentPlatform.OSType.Windows), "hosts");
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

        private void RepWindowStart()
        {
            RB_Progress.Visible = true;
            GenerateNow.Enabled = false;
            ControlBox = false;
        }

        private void RepWindowFinalize()
        {
            RB_Progress.Visible = false;
            GenerateNow.Text = AppStrings.RPB_CloseCpt;
            GenerateNow.Enabled = true;
            ControlBox = true;
            IsCompleted = true;
        }

        /// <summary>
        /// Метод, работающий в отдельном потоке при запуске механизма создания
        /// отчёта.
        /// </summary>
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
        /// Метод, срабатывающий по окончании работы механизма создания отчёта
        /// в отдельном потоке.
        /// </summary>
        private void BwGen_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RepWindowFinalize();

            if (e.Error == null)
            {
                // Открываем каталог с отчётами в оболочке и выделяем созданный файл...
                if (File.Exists(RepMan.ReportArchiveName))
                {
                    try
                    {
                        MessageBox.Show(String.Format(AppStrings.RPB_ComprGen, Path.GetFileName(RepMan.ReportArchiveName)), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ProcessManager.OpenExplorer(RepMan.ReportArchiveName, CurrentPlatform.OSType.Windows);
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
        /// Метод, срабатывающий при нажатии на кнопку "Создать/Закрыть".
        /// </summary>
        private void GenerateNow_Click(object sender, EventArgs e)
        {
            // Проверим необходим ли нам запуск очистки или закрытие формы...
            if (!IsCompleted)
            {
                // Отключим контролы...
                RepWindowStart();

                // Запускаем асинхронный обработчик...
                if (!BwGen.IsBusy) { BwGen.RunWorkerAsync(); } else { Logger.Warn(DebugStrings.AppDbgExRepWrkBusy); }
            }
            else
            {
                // Закрываем форму...
                Close();
            }
        }

        /// <summary>
        /// Метод, срабатывающий при попытке закрытия формы.
        /// </summary>
        private void FrmRepBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Блокируем возможность закрытия формы во время работы модуля...
            e.Cancel = BwGen.IsBusy;
        }

        private void BwGen_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RB_Progress.Value = e.ProgressPercentage;
        }
    }
}
