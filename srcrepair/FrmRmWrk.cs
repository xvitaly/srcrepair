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
using System.Windows.Forms;
using System.IO;
using NLog;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля быстрой очистки.
    /// </summary>
    public partial class FrmRmWrk : Form
    {
        /// <summary>
        /// Управляет записью событий в журнал.
        /// </summary>
        private Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Хранит статус выполнения процесса очистки.
        /// </summary>
        private bool IsRunning { get; set; } = true;

        /// <summary>
        /// Хранит список каталогов для очистки.
        /// </summary>
        private List<String> RemDirs { get; set; }

        /// <summary>
        /// Конструктор класса формы модуля быстрой очистки.
        /// </summary>
        /// <param name="SL">Список каталогов для очистки</param>
        public FrmRmWrk(List<String> SL)
        {
            InitializeComponent();
            RemDirs = SL;
        }

        /// <summary>
        /// Генерирует список кандидатов на удаление.
        /// </summary>
        /// <param name="CleanDirs">Список каталогов для очистки</param>
        /// <returns>Возвращает список файлов для удаления</returns>
        private List<String> DetectFilesForCleanup(List<String> CleanDirs)
        {
            // Создаём массив, в котором будем хранить имена файлов...
            List<String> Result = new List<String>();

            // Заполняем наш массив...
            foreach (string CleanCnd in CleanDirs)
            {
                // Проверяем существование каталога...
                if (Directory.Exists(CleanCnd))
                {
                    // Получаем содержимое каталога и добавляем их в очередь для удаления...
                    DirectoryInfo DInfo = new DirectoryInfo(CleanCnd);
                    FileInfo[] DirList = DInfo.GetFiles("*.*");
                    foreach (FileInfo DItem in DirList)
                    {
                        Result.Add(DItem.FullName);
                    }

                    // Получаем список вложенных каталогов...
                    List<String> SubDirs = new List<string>();
                    foreach (DirectoryInfo Dir in DInfo.GetDirectories())
                    {
                        SubDirs.Add(Path.Combine(Dir.FullName));
                    }

                    // Обходим полученные подкаталоги рекурсивно...
                    if (SubDirs.Count > 0)
                    {
                        Result.AddRange(DetectFilesForCleanup(SubDirs));
                    }
                }
                else
                {
                    // Если это не каталог, значит может быть обычным файлом. Проверим...
                    if (File.Exists(CleanCnd))
                    {
                        Result.Add(CleanCnd);
                    }
                }
            }

            // Выводим результат...
            return Result;
        }

        /// <summary>
        /// Метод, срабатывающий при возникновении события "загрузка формы".
        /// </summary>
        private void FrmRmWrk_Load(object sender, EventArgs e)
        {
            // Запускаем удаление асинхронно...
            if (!RW_Wrk.IsBusy) { RW_Wrk.RunWorkerAsync(); }
        }

        /// <summary>
        /// Метод, работающий в отдельном потоке при запуске механизма поиска и удаления.
        /// </summary>
        private void RW_Wrk_DoWork(object sender, DoWorkEventArgs e)
        {
            // Создаём список файлов для удаления...
            List<string> DeleteQueue = DetectFilesForCleanup(RemDirs);

            // Формируем счётчики...
            int TotalFiles = DeleteQueue.Count;
            int i = 1, j = 0;

            // Удаляем файлы из очереди очистки...
            foreach (string Fl in DeleteQueue)
            {
                try { j = (int)Math.Round(((double)i / (double)TotalFiles * (double)100.00), 0); i++; if ((j >= 0) && (j <= 100)) { RW_Wrk.ReportProgress(j); } } catch (Exception Ex) { Logger.Warn(Ex); }
                try { if (File.Exists(Fl)) { File.SetAttributes(Fl, FileAttributes.Normal); File.Delete(Fl); } } catch (Exception Ex) { Logger.Warn(Ex); }
            }

            // Удаляем пустые каталоги...
            foreach (string Dir in RemDirs) { FileManager.RemoveEmptyDirectories(Path.GetDirectoryName(Dir)); }

        }

        /// <summary>
        /// Метод, информирующий основную форму о прогрессе удаления файлов, который
        /// выполняется в отдельном потоке.
        /// </summary>
        private void RW_Wrk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Отображаем прогресс удаления файлов...
            RW_PrgBr.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Метод, срабатывающий по окончании работы механизма удаления файлов
        /// в отдельном потоке.
        /// </summary>
        private void RW_Wrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Удаление завершено. Закроем форму...
            IsRunning = false;

            if (e.Error != null)
            {
                MessageBox.Show(AppStrings.RW_RmException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(e.Error, "Exception while removing files.");
            }

            Close();
        }

        /// <summary>
        /// Метод, срабатывающий при попытке закрытия формы.
        /// </summary>
        private void FrmRmWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Блокируем возможность закрытия формы при работающем процессе...
            e.Cancel = IsRunning;
        }
    }
}
