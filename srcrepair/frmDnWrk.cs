/*
 * Модуль загрузки файлов из сети Интернет с индикатором прогресса.
 * 
 * Copyright 2011 - 2016 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2016 EasyCoding Team.
 * 
 * Лицензия: GPL v3 (см. файл GPL.txt).
 * Лицензия контента: Creative Commons 3.0 BY.
 * 
 * Запрещается использовать этот файл при использовании любой
 * лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
 * 
 * Официальный блог EasyCoding Team: http://www.easycoding.org/
 * Официальная страница проекта: http://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля загрузки файлов из Интернета.
    /// </summary>
    public partial class frmDnWrk : Form
    {
        private bool IsRunning = true;
        private string RemoteURI;
        private string LocalFile;

        /// <summary>
        /// Конструктор класса формы модуля загрузки файлов из Интернета.
        /// </summary>
        public frmDnWrk(string R, string L)
        {
            InitializeComponent();
            RemoteURI = R;
            LocalFile = L;
        }

        private void frmDnWrk_Load(object sender, EventArgs e)
        {
            // Начинаем процесс загрузки в отдельном потоке...
            DownloaderStart(RemoteURI, LocalFile);
        }

        private void DownloaderProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Отрисовываем статус в прогресс-баре...
            try { DN_PrgBr.Value = e.ProgressPercentage; } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

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
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Закроем форму...
            IsRunning = false;
            Close();
        }

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
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void frmDnWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = IsRunning;
        }
    }
}
