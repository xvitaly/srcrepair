/*
 * Модуль обновления программы SRC Repair.
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
using System.Diagnostics;
using System.IO;

namespace srcrepair
{
    public partial class frmUpdate : Form
    {
        public frmUpdate(string UA, string A, string U)
        {
            InitializeComponent();
            UserAgent = UA;
            FullAppPath = A;
            AppUserDir = U;
        }

        private UpdateManager UpMan;
        private string UserAgent;
        private string AppUserDir;
        private string FullAppPath;

        /// <summary>
        /// Устанавливает дату последней проверки обновлений приложения.
        /// </summary>
        private void UpdateTimeSetApp()
        {
            Properties.Settings.Default.LastUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// Устанавливает дату последней проверки обновлений базы HUD.
        /// </summary>
        private void UpdateTimeSetHUD()
        {
            Properties.Settings.Default.LastHUDTime = DateTime.Now;
        }

        /// <summary>
        /// Начинает процесс поиска обновлений в отдельном потоке.
        /// </summary>
        private void CheckForUpdates()
        {
            if (!WrkChkApp.IsBusy) { WrkChkApp.RunWorkerAsync(); }
        }

        private void frmUpdate_Load(object sender, EventArgs e)
        {
            // Заполняем...
            Text = String.Format(Text, Properties.Resources.AppName);

            // Запускаем функцию проверки обновлений...
            CheckForUpdates();
        }

        private void WrkChkApp_DoWork(object sender, DoWorkEventArgs e)
        {
            // Установим значок проверки обновлений...
            Invoke((MethodInvoker)delegate()
            {
                UpdAppImg.Image = Properties.Resources.upd_chk;
                UpdDBImg.Image = Properties.Resources.upd_chk;
                UpdHUDDbImg.Image = Properties.Resources.upd_chk;
            });

            // Запускаем механизм проверки обновлений асинхронно...
            UpMan = new UpdateManager(FullAppPath, UserAgent);
        }

        private void WrkChkApp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                // Проверим статус проверки...
                if (e.Error == null)
                {
                    // Проверим наличие обновлений для приложения...
                    if (UpMan.CheckAppUpdate()) { UpdAppImg.Image = Properties.Resources.upd_av; UpdAppStatus.Text = String.Format(AppStrings.UPD_AppUpdateAvail, UpMan.AppUpdateVersion); } else { UpdAppImg.Image = Properties.Resources.upd_nx; UpdAppStatus.Text = AppStrings.UPD_AppNoUpdates; UpdateTimeSetApp(); }

                    // Проверим наличие обновлений для базы игр...
                    if (UpMan.CheckGameDBUpdate()) { UpdDBImg.Image = Properties.Resources.upd_av; UpdDBStatus.Text = String.Format(AppStrings.UPD_DbUpdateAvail, UpMan.GameUpdateHash.Substring(0, 7)); } else { UpdDBImg.Image = Properties.Resources.upd_nx; UpdDBStatus.Text = AppStrings.UPD_DbNoUpdates; }

                    // Проверим наличие обновлений для базы HUD...
                    if (UpMan.CheckHUDUpdate()) { UpdHUDDbImg.Image = Properties.Resources.upd_av; UpdHUDStatus.Text = String.Format(AppStrings.UPD_HUDUpdateAvail, UpMan.HUDUpdateHash.Substring(0, 7)); } else { UpdHUDDbImg.Image = Properties.Resources.upd_nx; UpdHUDStatus.Text = AppStrings.UPD_HUDNoUpdates; UpdateTimeSetHUD(); }
                }
                else
                {
                    // Произошла ошибка...
                    UpdAppImg.Image = Properties.Resources.upd_err;
                    UpdAppStatus.Text = AppStrings.UPD_AppCheckFailure;
                    UpdDBImg.Image = Properties.Resources.upd_err;
                    UpdDBStatus.Text = AppStrings.UPD_DbCheckFailure;
                    UpdHUDDbImg.Image = Properties.Resources.upd_err;
                    UpdHUDStatus.Text = AppStrings.UPD_HUDCheckFailure;

                    // Запишем в журнал...
                    CoreLib.WriteStringToLog(e.Error.Message);
                }

            }
            catch (Exception Ex)
            {
                CoreLib.WriteStringToLog(Ex.Message);
            }
        }

        private void frmUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && WrkChkApp.IsBusy;
        }

        private void UpdAppStatus_Click(object sender, EventArgs e)
        {
            // Проверяем наличие обновлений программы...
            if (UpMan.CheckAppUpdate())
            {
                // Генерируем имя файла обновления...
                string UpdateFileName = UpdateManager.GenerateUpdateFileName(Path.Combine(AppUserDir, Path.GetFileName(UpMan.AppUpdateURL)));

                // Загружаем файл асинхронно...
                CoreLib.DownloadFileEx(UpMan.AppUpdateURL, UpdateFileName);

                // Выполняем проверки и устанавливаем обновление...
                if (File.Exists(UpdateFileName)) { if (CoreLib.CalculateFileMD5(UpdateFileName) == UpMan.AppUpdateHash) { UpdateTimeSetApp(); MessageBox.Show(AppStrings.UPD_UpdateSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); try { Process.Start(UpdateFileName); } catch (Exception Ex) { CoreLib.HandleExceptionEx(AppStrings.UPD_UpdateFailure, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error); } Environment.Exit(9); } else { try { File.Delete(UpdateFileName); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); } MessageBox.Show(AppStrings.UPD_HashFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); } } else { MessageBox.Show(AppStrings.UPD_UpdateFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); CheckForUpdates(); }
            }
            else
            {
                // Обновление не требуется, поэтому просто выводим сообщение об этом...
                MessageBox.Show(AppStrings.UPD_LatestInstalled, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdHUDStatus_Click(object sender, EventArgs e)
        {
            // Проверяем наличие обновлений...
            if (UpMan.CheckHUDUpdate())
            {
                // Проверяем наличие прав на запись в каталог...
                if (CoreLib.IsDirectoryWritable(FullAppPath))
                {
                    // Генерируем пути к файлам...
                    string UpdateFileName = UpdateManager.GenerateUpdateFileName(Path.Combine(FullAppPath, Properties.Resources.HUDDbFile));
                    string UpdateTempFile = Path.GetTempFileName();

                    // Загружаем файл с сервера...
                    CoreLib.DownloadFileEx(UpMan.HUDUpdateURL, UpdateTempFile);

                    try
                    {
                        // Проверяем контрольную сумму...
                        if (CoreLib.CalculateFileMD5(UpdateTempFile) == UpMan.HUDUpdateHash)
                        {
                            // Копируем загруженный файл...
                            File.Copy(UpdateTempFile, UpdateFileName, true);

                            // Обновляем дату последней проверки обновлений...
                            UpdateTimeSetHUD();

                            // Выводим сообщение об успехе...
                            MessageBox.Show(AppStrings.UPD_HUDDb_Updated, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Выводим сообщение о несовпадении хешей...
                            MessageBox.Show(AppStrings.UPD_HashFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception Ex)
                    {
                        // Выводим сообщение об ошибке...
                        CoreLib.HandleExceptionEx(AppStrings.UPD_UpdateFailure, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
                    }
                    
                    // Удаляем загруженный файл если он существует...
                    if (File.Exists(UpdateTempFile)) { File.Delete(UpdateTempFile); }

                    // Повторяем поиск обновлений...
                    CheckForUpdates();
                }
                else
                {
                    // Выводим сообщение об отсутствии прав на запись в каталог...
                    MessageBox.Show(AppStrings.UPD_NoWritePermissions, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Обновление не требуется. Выводим соответствующее сообщение...
                MessageBox.Show(AppStrings.UPD_HUDDb_Latest, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdDBStatus_Click(object sender, EventArgs e)
        {
            if (UpMan.CheckGameDBUpdate())
            {
                if (CoreLib.IsDirectoryWritable(FullAppPath))
                {
                    string UpdateFileName = UpdateManager.GenerateUpdateFileName(Path.Combine(FullAppPath, Properties.Resources.GameListFile)); string UpdateTempFile = Path.GetTempFileName(); CoreLib.DownloadFileEx(UpMan.GameUpdateURL, UpdateTempFile); try { if (CoreLib.CalculateFileMD5(UpdateTempFile) == UpMan.GameUpdateHash) { File.Copy(UpdateTempFile, UpdateFileName, true); MessageBox.Show(AppStrings.UPD_GamL_Updated, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); } else { MessageBox.Show(AppStrings.UPD_HashFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); } } catch (Exception Ex) { CoreLib.HandleExceptionEx(AppStrings.UPD_UpdateFailure, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error); } if (File.Exists(UpdateTempFile)) { File.Delete(UpdateTempFile); } CheckForUpdates();
                }
                else
                {
                    MessageBox.Show(AppStrings.UPD_NoWritePermissions, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.UPD_GamL_Latest, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
