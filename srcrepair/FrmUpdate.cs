﻿/*
 * Модуль обновления программы SRC Repair.
 * 
 * Copyright 2011 - 2015 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2015 EasyCoding Team.
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
using System.Diagnostics;
using System.IO;

namespace srcrepair
{
    public partial class frmUpdate : Form
    {
        public frmUpdate(string UA, string A, string V, string U)
        {
            InitializeComponent();
            UserAgent = UA;
            FullAppPath = A;
            AppVersionInfo = V;
            AppUserDir = U;
        }

        private string NewVersion;
        private string UpdateURI;
        private string UpdateFileName;
        private string DBHash;
        private string NewHash;
        private string FullAppPath;
        private string UserAgent;
        private string AppVersionInfo;
        private string AppUserDir;
        private bool AppAvailable;
        private bool DbAvailable;

        private void frmUpdate_Load(object sender, EventArgs e)
        {
            // Заполняем...
            Text = String.Format(Text, Properties.Resources.AppName);

            // Запускаем функции проверки обновлений программы...
            if (!WrkChkApp.IsBusy) { WrkChkApp.RunWorkerAsync(); }

            // Запускаем функции проверки обновлений базы данных игр...
            if (!WrkChkDb.IsBusy) { WrkChkDb.RunWorkerAsync(); }
        }

        private string GenerateUpdateFileName(string Url)
        {
            return Path.HasExtension(Url) ? Url : Path.ChangeExtension(Url, "exe");
        }

        private void WrkChkApp_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Установим значок проверки обновлений...
                Invoke((MethodInvoker)delegate() { UpdAppImg.Image = Properties.Resources.upd_chk; });
                
                // Опишем буферные переменные...
                string DnlStr;

                // Получаем файл с номером версии и ссылкой на новую...
                using (WebClient Downloader = new WebClient())
                {
                    Downloader.Headers.Add("User-Agent", UserAgent);
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
            try
            {
                // Проверим, является ли версия на сервере новее, чем текущая...
                if (CoreLib.CompareVersions(AppVersionInfo, NewVersion))
                {
                    // Доступна новая версия...
                    Invoke((MethodInvoker)delegate()
                    {
                        UpdAppImg.Image = Properties.Resources.upd_av;
                        UpdAppImg.Cursor = Cursors.Hand;
                        UpdAppStatus.Cursor = Cursors.Hand;
                        UpdAppStatus.Text = String.Format(CoreLib.GetLocalizedString("UPD_AppUpdateAvail"), NewVersion);
                    });
                    AppAvailable = true;
                }
                else
                {
                    // Новых версий не обнаружено...
                    Invoke((MethodInvoker)delegate()
                    {
                        UpdAppImg.Image = Properties.Resources.upd_nx;
                        UpdAppStatus.Text = CoreLib.GetLocalizedString("UPD_AppNoUpdates");
                    });
                    AppAvailable = false;
                }
            }
            catch (Exception Ex)
            {
                CoreLib.WriteStringToLog(Ex.Message);
            }
        }

        private void WrkChkDb_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Установим значок проверки обновлений...
                Invoke((MethodInvoker)delegate() { UpdDBImg.Image = Properties.Resources.upd_chk; });

                // Получаем файл с номером версии и ссылкой на новую...
                using (WebClient Downloader = new WebClient())
                {
                    // Получим хеш...
                    Downloader.Headers.Add("User-Agent", UserAgent);
                    NewHash = Downloader.DownloadString(Properties.Settings.Default.UpdateGameDBHash);
                }

                // Рассчитаем хеш текущего файла...
                DBHash = CoreLib.CalculateFileMD5(Path.Combine(FullAppPath, Properties.Settings.Default.GameListFile));
            }
            catch (Exception Ex)
            {
                // Произошло исключение...
                CoreLib.WriteStringToLog(Ex.Message);
            }
        }

        private void WrkChkDb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                // Проверим хеши...
                if (DBHash != NewHash)
                {
                    // Хеши не совпадают, будем обновлять...
                    Invoke((MethodInvoker)delegate()
                    {
                        UpdDBImg.Image = Properties.Resources.upd_av;
                        UpdDBImg.Cursor = Cursors.Hand;
                        UpdDBStatus.Cursor = Cursors.Hand;
                        UpdDBStatus.Text = String.Format(CoreLib.GetLocalizedString("UPD_DbUpdateAvail"), NewHash);
                    });
                    DbAvailable = true;
                }
                else
                {
                    // Хеши совпали, обновление не требуется...
                    Invoke((MethodInvoker)delegate()
                    {
                        UpdDBImg.Image = Properties.Resources.upd_nx;
                        UpdDBStatus.Text = CoreLib.GetLocalizedString("UPD_DbNoUpdates");
                    });
                    DbAvailable = false;
                }
            }
            catch (Exception Ex)
            {
                CoreLib.WriteStringToLog(Ex.Message);
            }
        }

        private void frmUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && WrkChkApp.IsBusy && WrkChkDb.IsBusy;
        }

        private void UpdDBStatus_Click(object sender, EventArgs e)
        {
            if (!WrkChkDb.IsBusy)
            {
                if (DbAvailable && CoreLib.IsDirectoryWritable(FullAppPath))
                {
                    UpdateFileName = GenerateUpdateFileName(Path.Combine(FullAppPath, Properties.Settings.Default.GameListFile));
                    CoreLib.DownloadFileEx(Properties.Settings.Default.UpdateGameDBFile, UpdateFileName);
                    if (File.Exists(UpdateFileName)) { MessageBox.Show(CoreLib.GetLocalizedString("UPD_GamL_Updated"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); } else { MessageBox.Show(CoreLib.GetLocalizedString("UPD_UpdateFailure"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    Close();
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("UPD_LatestInstalled"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void UpdAppStatus_Click(object sender, EventArgs e)
        {
            if (!WrkChkApp.IsBusy)
            {
                if (AppAvailable)
                {
                    UpdateFileName = GenerateUpdateFileName(Path.Combine(AppUserDir, Path.GetFileName(UpdateURI)));
                    CoreLib.DownloadFileEx(UpdateURI, UpdateFileName);
                    if (File.Exists(UpdateFileName))
                    {
                        MessageBox.Show(CoreLib.GetLocalizedString("UPD_UpdateSuccessful"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        try { Process.Start(UpdateFileName); } catch (Exception Ex) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("UPD_UpdateFailure"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error); }
                        Environment.Exit(9);
                    }
                    else
                    {
                        MessageBox.Show(CoreLib.GetLocalizedString("UPD_UpdateFailure"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("UPD_LatestInstalled"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
