/*
 * Модуль обновления программы SRC Repair.
 * 
 * Copyright 2011 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2011 EasyCoding Team.
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace srcrepair
{
    public partial class frmUpdate : Form
    {
        public frmUpdate(string AvVersion, string URI)
        {
            InitializeComponent();
            AppAvailVersion = AvVersion;
            UpdateURI = URI;
        }

        private string AppAvailVersion;

        private string UpdateURI;

        private void frmUpdate_Load(object sender, EventArgs e)
        {
            // Заполняем...
            this.Text = String.Format(this.Text, GV.AppName);
            LWelcome.Text = String.Format(LWelcome.Text, GV.AppName);
            LCurrVersion.Text = String.Format(LCurrVersion.Text, GV.AppVersionInfo);
            LAvailVersion.Text = String.Format(LAvailVersion.Text, AppAvailVersion);
        }

        private void FileDownloader_Completed(object sender, AsyncCompletedEventArgs e)
        {
            // Проверим чтобы полученный файл существовал...
            if (File.Exists(Path.Combine(GV.AppUserDir, Path.GetFileName(UpdateURI))))
            {
                // Существует, покажем сообщение...
                MessageBox.Show(CoreLib.GetLocalizedString("UPD_UpdateSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Запустим...
                Process.Start(Path.Combine(GV.AppUserDir, Path.GetFileName(UpdateURI)));
                // Завершим работу программы...
                Environment.Exit(9);
            }
            else
            {
                // Файл не существует: ошибка обновления...
                MessageBox.Show(CoreLib.GetLocalizedString("UPD_UpdateFailure"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Закроем форму...
                this.Close();
            }
        }

        private void FileDownloader_ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Отрисовываем статус в прогресс-баре...
            DnlProgBar.Value = e.ProgressPercentage;
        }

        private void DnlInstall_Click(object sender, EventArgs e)
        {
            try
            {
                DnlInstall.Visible = false; // Прячем кнопку...
                DnlProgBar.Visible = true; // Отображаем диалог прогресса...
                using (WebClient FileDownloader = new WebClient())
                {
                    FileDownloader.Headers.Add("User-Agent", GV.UserAgent);
                    FileDownloader.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloader_Completed);
                    FileDownloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(FileDownloader_ProgressChanged);
                    // Скачиваем файл...
                    FileDownloader.DownloadFileAsync(new Uri(UpdateURI), Path.Combine(GV.AppUserDir, Path.GetFileName(UpdateURI)));
                }
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("UPD_DownloadException"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
        }
    }
}
