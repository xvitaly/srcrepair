/*
 * Модуль обновления программы SRC Repair.
 * 
 * Copyright 2011 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2014 EasyCoding Team.
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
        public frmUpdate()
        {
            InitializeComponent();
        }

        private string NewVersion;
        private string UpdateURI;
        private string UpdateFileName;
        private string DBHash;
        private string NewHash;
        private bool AppAvailable;
        private bool DbAvailable;

        private void frmUpdate_Load(object sender, EventArgs e)
        {
            // Заполняем...
            this.Text = String.Format(this.Text, GV.AppName);

            // Запускаем функции проверки обновлений...
            if (!WrkChkApp.IsBusy) { WrkChkApp.RunWorkerAsync(); }

            // Проверяем наличие прав на запись в каталог и если они есть запускаем проверку...
            if (CoreLib.IsDirectoryWritable(GV.FullAppPath))
            {
                if (!WrkChkDb.IsBusy) { WrkChkDb.RunWorkerAsync(); }
            }
        }

        private void FileDownloader_Completed(object sender, AsyncCompletedEventArgs e)
        {
            // Проверим чтобы полученный файл существовал...
            if (File.Exists(this.UpdateFileName))
            {
                // Существует, покажем сообщение...
                MessageBox.Show(CoreLib.GetLocalizedString("UPD_UpdateSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Запустим...
                try { Process.Start(this.UpdateFileName); } catch (Exception Ex) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("UPD_UpdateFailure"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error); }
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
            try { DnlProgBar.Value = e.ProgressPercentage; } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private string GenerateUpdateFileName(string Url)
        {
            return Path.HasExtension(Url) ? Url : Path.ChangeExtension(Url, "exe");
        }

        private void DownloadUpdate(string UpdateURI, string FileName)
        {
            using (WebClient FileDownloader = new WebClient())
            {
                // Скачиваем файл...
                FileDownloader.Headers.Add("User-Agent", Properties.Resources.AppDnlUA);
                FileDownloader.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloader_Completed);
                FileDownloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(FileDownloader_ProgressChanged);
                FileDownloader.DownloadFileAsync(new Uri(UpdateURI), FileName);
            }
        }

        private void DnlInstall_Click(object sender, EventArgs e)
        {
            try
            {
                if (!WrkChkDb.IsBusy && !WrkChkApp.IsBusy && (this.AppAvailable || this.DbAvailable))
                {
                    DnlInstall.Visible = false; // Прячем кнопку...
                    DnlProgBar.Visible = true; // Отображаем диалог прогресса...
                    this.UpdateFileName = GenerateUpdateFileName(Path.Combine(GV.AppUserDir, Path.GetFileName(UpdateURI)));
                    DownloadUpdate(this.UpdateURI, this.UpdateFileName);
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("UPD_LatestInstalled"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("UPD_DownloadException"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
        }

        private void WrkChkApp_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Установим значок проверки обновлений...
                this.Invoke((MethodInvoker)delegate() { UpdAppImg.Image = Properties.Resources.upd_chk; });
                
                // Опишем буферные переменные...
                string DnlStr;

                // Получаем файл с номером версии и ссылкой на новую...
                using (WebClient Downloader = new WebClient())
                {
                    Downloader.Headers.Add("User-Agent", GV.UserAgent);
                    DnlStr = Downloader.DownloadString(Properties.Settings.Default.UpdateChURI);
                }

                // Установим дату последней проверки обновлений...
                Properties.Settings.Default.LastUpdateTime = DateTime.Now;

                // Мы получили URL и версию...
                NewVersion = DnlStr.Substring(0, DnlStr.IndexOf("!")); // Получаем версию...
                UpdateURI = DnlStr.Remove(0, DnlStr.IndexOf("!") + 1); // Получаем URL...
            }
            catch (Exception Ex)
            {
                // Произошло исключение...
                CoreLib.WriteStringToLog(Ex.Message);
            }
        }

        private void WrkChkApp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Проверим, является ли версия на сервере новее, чем текущая...
            if (CoreLib.CompareVersions(GV.AppVersionInfo, NewVersion))
            {
                // Доступна новая версия...
                this.Invoke((MethodInvoker)delegate()
                {
                    UpdAppImg.Image = Properties.Resources.upd_av;
                    UpdAppStatus.Text = String.Format(CoreLib.GetLocalizedString("UPD_AppUpdateAvail"), this.NewVersion);
                });
                this.AppAvailable = true;
            }
            else
            {
                // Новых версий не обнаружено...
                this.Invoke((MethodInvoker)delegate()
                {
                    UpdAppImg.Image = Properties.Resources.upd_nx;
                    UpdAppStatus.Text = CoreLib.GetLocalizedString("UPD_AppNoUpdates");
                });
                this.AppAvailable = false;
            }
        }

        private void WrkChkDb_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Установим значок проверки обновлений...
                this.Invoke((MethodInvoker)delegate() { UpdDBImg.Image = Properties.Resources.upd_chk; });

                // Получаем файл с номером версии и ссылкой на новую...
                using (WebClient Downloader = new WebClient())
                {
                    // Получим хеш...
                    Downloader.Headers.Add("User-Agent", GV.UserAgent);
                    this.NewHash = Downloader.DownloadString(Properties.Settings.Default.UpdateGameDBHash);
                }

                // Рассчитаем хеш текущего файла...
                this.DBHash = CoreLib.CalculateFileMD5(Path.Combine(GV.FullAppPath, Properties.Settings.Default.GameListFile));
            }
            catch (Exception Ex)
            {
                // Произошло исключение...
                CoreLib.WriteStringToLog(Ex.Message);
            }
        }

        private void WrkChkDb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Проверим хеши...
            if (this.DBHash != this.NewHash)
            {
                // Хеши не совпадают, будем обновлять...
                this.Invoke((MethodInvoker)delegate() { UpdDBImg.Image = Properties.Resources.upd_av; });
                this.DbAvailable = true;
            }
            else
            {
                // Хеши совпали, обновление не требуется...
                this.Invoke((MethodInvoker)delegate() { UpdDBImg.Image = Properties.Resources.upd_nx; });
                this.DbAvailable = false;
            }
        }
    }
}
