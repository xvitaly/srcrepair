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
using System.Net;
using System.IO;
using NLog;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля загрузки файлов из Интернета.
    /// </summary>
    public partial class FrmDnWrk : Form
    {
        /// <summary>
        /// Управляет записью событий в журнал.
        /// </summary>
        private Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Хранит статус выполнения процесса загрузки.
        /// </summary>
        private bool IsRunning { get; set; } = true;

        /// <summary>
        /// Хранит URL файла, который нужно загрузить.
        /// </summary>
        private string RemoteURI { get; set; }

        /// <summary>
        /// Хранит путь к локальному файлу, куда нужно сохранить результат.
        /// </summary>
        private string LocalFile { get; set; }

        /// <summary>
        /// Конструктор класса формы модуля загрузки файлов из Интернета.
        /// </summary>
        /// <param name="R">URL файла загрузки</param>
        /// <param name="L">Путь сохранения файла</param>
        public FrmDnWrk(string R, string L)
        {
            InitializeComponent();
            RemoteURI = R;
            LocalFile = L;
        }

        /// <summary>
        /// Метод, срабатывающий при возникновении события "загрузка формы".
        /// </summary>
        private void FrmDnWrk_Load(object sender, EventArgs e)
        {
            // Начинаем процесс загрузки в отдельном потоке...
            DownloaderStart(RemoteURI, LocalFile);
        }

        /// <summary>
        /// Метод, информирующий основную форму о прогрессе загрузки файлов, который
        /// выполняется в отдельном потоке.
        /// </summary>
        private void DownloaderProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Отрисовываем статус в прогресс-баре...
            try { DN_PrgBr.Value = e.ProgressPercentage; } catch (Exception Ex) { Logger.Warn(Ex); }
        }

        /// <summary>
        /// Метод, срабатывающий по окончании работы механизма загрузки в отдельном потоке.
        /// </summary>
        private void DownloaderCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Загрузка завершена. Проверим скачалось ли что-то. Если нет, удалим пустой файл...
            try
            {
                if (File.Exists(LocalFile))
                {
                    FileInfo Fi = new FileInfo(LocalFile);
                    if (Fi.Length == 0)
                    {
                        File.Delete(LocalFile);
                    }
                }
            }
            catch (Exception Ex) { Logger.Warn(Ex); }

            // Закроем форму...
            IsRunning = false;
            Close();
        }

        /// <summary>
        /// Метод, инициирущий процесс загрузки файла из Интернета.
        /// </summary>
        private void DownloaderStart(string URI, string FileName)
        {
            try
            {
                // Проверим существование файла и удалим...
                if (File.Exists(FileName)) { File.Delete(FileName); }

                // Начинаем асинхронную загрузку файла...
                using (WebClient FileDownloader = new WebClient())
                {
                    FileDownloader.Headers.Add("User-Agent", Properties.Resources.AppDnlUA);
                    FileDownloader.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloaderCompleted);
                    FileDownloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloaderProgressChanged);
                    FileDownloader.DownloadFileAsync(new Uri(URI), FileName);
                }
            }
            catch (Exception Ex) { Logger.Warn(Ex); }
        }

        /// <summary>
        /// Метод, срабатывающий при попытке закрытия формы.
        /// </summary>
        private void FrmDnWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = IsRunning;
        }
    }
}
