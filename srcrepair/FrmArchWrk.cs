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
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Ionic.Zip;
using NLog;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля распаковки файлов из архивов.
    /// </summary>
    public partial class FrmArchWrk : Form
    {
        /// <summary>
        /// Управляет записью событий в журнал.
        /// </summary>
        private Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Хранит статус выполнения процесса распаковки.
        /// </summary>
        private bool IsRunning { get; set; } = true;

        /// <summary>
        /// Хранит имя архива, который мы распаковываем.
        /// </summary>
        private string ArchName { get; set; }

        /// <summary>
        /// Хранит каталог назначения.
        /// </summary>
        private string DestDir { get; set; }

        /// <summary>
        /// Конструктор класса формы модуля распаковки файлов из архивов.
        /// </summary>
        /// <param name="A">Архив для распаковки</param>
        /// <param name="D">Каталог назначения</param>
        public FrmArchWrk(string A, string D)
        {
            InitializeComponent();
            ArchName = A;
            DestDir = D;
        }

        /// <summary>
        /// Метод, срабатывающий при возникновении события "загрузка формы".
        /// </summary>
        private void FrmArchWrk_Load(object sender, EventArgs e)
        {
            // Начинаем процесс распаковки асинхронно...
            if (!AR_Wrk.IsBusy) { AR_Wrk.RunWorkerAsync(); }
        }

        /// <summary>
        /// Метод, работающий в отдельном потоке при запуске механизма распаковки.
        /// </summary>
        private void AR_Wrk_DoWork(object sender, DoWorkEventArgs e)
        {
            // Начинаем процесс распаковки с выводом индикатора прогресса...
            if (File.Exists(ArchName))
            {
                using (ZipFile Zip = ZipFile.Read(ArchName))
                {
                    // Формируем счётчики...
                    int TotalFiles = Zip.Count;
                    int i = 1, j = 0;
                    
                    // Начинаем распаковку архива...
                    foreach (ZipEntry ZFile in Zip)
                    {
                        try { ZFile.Extract(DestDir, ExtractExistingFileAction.OverwriteSilently); j = (int)Math.Round(((double)i / (double)TotalFiles * (double)100.00), 0); i++; if ((j >= 0) && (j <= 100)) { AR_Wrk.ReportProgress(j); } } catch (Exception Ex) { Logger.Warn(Ex); }
                    }
                }
            }
            else
            {
                throw new FileNotFoundException(AppStrings.AR_BkgWrkExText, ArchName);
            }
        }

        /// <summary>
        /// Метод, информирующий основную форму о прогрессе распаковки файлов, который
        /// выполняется в отдельном потоке.
        /// </summary>
        private void AR_Wrk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Отображаем прогресс распаковки архива в баре...
            AR_PrgBr.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Метод, срабатывающий по окончании работы механизма распаковки в отдельном потоке.
        /// </summary>
        private void AR_Wrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Работа завершена. Закроем форму...
            IsRunning = false;

            if (e.Error != null)
            {
                MessageBox.Show(AppStrings.AR_UnpackException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(e.Error, AppStrings.AppDbgExArWrkUnpack);
            }

            Close();
        }

        /// <summary>
        /// Метод, срабатывающий при попытке закрытия формы.
        /// </summary>
        private void FrmArchWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Блокируем возможность закрытия формы при работающем процессе...
            e.Cancel = IsRunning;
        }
    }
}
