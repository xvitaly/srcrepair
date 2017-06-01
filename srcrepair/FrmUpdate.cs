/*
 * Модуль обновления программы SRC Repair.
 * 
 * Copyright 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2017 EasyCoding Team.
 * 
 * Лицензия: GPL v3 (см. файл GPL.txt).
 * Лицензия контента: Creative Commons 3.0 BY.
 * 
 * Запрещается использовать этот файл при использовании любой
 * лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
 * 
 * Официальный блог EasyCoding Team: https://www.easycoding.org/
 * Официальная страница проекта: https://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля обновления программы SRC Repair.
    /// </summary>
    public partial class FrmUpdate : Form
    {
        /// <summary>
        /// Конструктор класса формы модуля обновления программы SRC Repair.
        /// </summary>
        /// <param name="UA">Заголовок UserAgent</param>
        /// <param name="A">Путь к каталогу программы</param>
        /// <param name="U">Путь к пользовательскому каталогу</param>
        public FrmUpdate(string UA, string A, string U)
        {
            InitializeComponent();
            UserAgent = UA;
            FullAppPath = A;
            AppUserDir = U;
        }

        /// <summary>
        /// Менеджер обновлений: управляет процессом поиска и установки обновлений.
        /// </summary>
        private UpdateManager UpMan { get; set; }

        /// <summary>
        /// Хранит полученный UserAgent для HTTP запросов.
        /// </summary>
        private string UserAgent { get; set; }

        /// <summary>
        /// Хранит путь к пользовательскому каталогу SRC Repair.
        /// </summary>
        private string AppUserDir { get; set; }

        /// <summary>
        /// Хранит путь установки SRC Repair.
        /// </summary>
        private string FullAppPath { get; set; }

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

        /// <summary>
        /// Устанавливает обновление базы данных.
        /// </summary>
        /// <param name="ResFileName">Имя файла для обновления</param>
        /// <param name="UpdateURL">URL загрузки обновления</param>
        /// <param name="UpdateHash">Контрольная сумма файла обновления</param>
        /// <returns>Возвращает true при успешной установке обновления, иначе - false.</returns>
        private bool InstallDatabaseUpdate(string ResFileName, string UpdateURL, string UpdateHash)
        {
            // Задаём значения переменных по умолчанию...
            bool Result = false;

            // Проверяем наличие прав на запись в каталог...
            if (FileManager.IsDirectoryWritable(FullAppPath))
            {
                // Генерируем пути к файлам...
                string UpdateFileName = UpdateManager.GenerateUpdateFileName(Path.Combine(FullAppPath, ResFileName));
                string UpdateTempFile = Path.GetTempFileName();

                // Загружаем файл с сервера...
                FormManager.FormShowDownloader(UpdateURL, UpdateTempFile);

                try
                {
                    // Проверяем контрольную сумму...
                    if (FileManager.CalculateFileMD5(UpdateTempFile) == UpdateHash)
                    {
                        // Копируем загруженный файл...
                        File.Copy(UpdateTempFile, UpdateFileName, true);

                        // Выводим сообщение об успехе...
                        MessageBox.Show(AppStrings.UPD_UpdateDBSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Возвращаем положительный результат...
                        Result = true;
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

            // Возвращаем результат...
            return Result;
        }

        /// <summary>
        /// Устанавливает обновление в виде отдельного исполняемого файла.
        /// </summary>
        /// <param name="UpdateURL">URL загрузки обновления</param>
        /// <param name="UpdateHash">Контрольная сумма файла обновления</param>
        /// <returns>Возвращает true при успешной установке обновления, иначе - false.</returns>
        private bool InstallBinaryUpdate(string UpdateURL, string UpdateHash)
        {
            // Задаём значения переменных по умолчанию...
            bool Result = false;

            // Генерируем имя файла обновления...
            string UpdateFileName = UpdateManager.GenerateUpdateFileName(Path.Combine(AppUserDir, Path.GetFileName(UpdateURL)));

            // Загружаем файл асинхронно...
            FormManager.FormShowDownloader(UpMan.AppUpdateURL, UpdateFileName);

            // Выполняем проверки и устанавливаем обновление...
            if (File.Exists(UpdateFileName))
            {
                // Проверяем хеш загруженного файла с эталоном...
                if (FileManager.CalculateFileMD5(UpdateFileName) == UpdateHash)
                {
                    // Обновляем дату последней проверки обновлений...
                    UpdateTimeSetApp();

                    // Выводим сообщение об успешном окончании загрузки и готовности к установке обновления...
                    MessageBox.Show(AppStrings.UPD_UpdateSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Запускаем установку standalone-обновления...
                    try { if (FileManager.IsDirectoryWritable(FullAppPath)) { Process.Start(UpdateFileName); } else { ProcessManager.StartWithUAC(UpdateFileName); } Result = true; } catch (Exception Ex) { CoreLib.HandleExceptionEx(AppStrings.UPD_UpdateFailure, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error); }
                }
                else
                {
                    // Хеш-сумма не совпала, поэтому файл скорее всего повреждён. Удаляем...
                    try { File.Delete(UpdateFileName); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                    // Выводим сообщение о несовпадении контрольной суммы...
                    MessageBox.Show(AppStrings.UPD_HashFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Не удалось загрузить файл обновления. Выводим сообщение об ошибке...
                MessageBox.Show(AppStrings.UPD_UpdateFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Повторно запускаем проверку обновлений...
            CheckForUpdates();

            // Возвращаем результат...
            return Result;
        }

        private void FrmUpdate_Load(object sender, EventArgs e)
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

        private void FrmUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && WrkChkApp.IsBusy;
        }

        private void UpdAppStatus_Click(object sender, EventArgs e)
        {
            if (!WrkChkApp.IsBusy)
            {
                // Проверяем наличие обновлений программы...
                if (UpMan.CheckAppUpdate())
                {
                    // Устанавливаем доступное обновление...
                    if (InstallBinaryUpdate(UpMan.AppUpdateURL, UpMan.AppUpdateHash))
                    {
                        // Загрузка завершилась успешно. Завершаем работу приложения для установки...
                        Environment.Exit(9);
                    }
                }
                else
                {
                    // Обновление не требуется, поэтому просто выводим сообщение об этом...
                    MessageBox.Show(AppStrings.UPD_LatestInstalled, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.DB_WrkInProgress, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdHUDStatus_Click(object sender, EventArgs e)
        {
            if (!WrkChkApp.IsBusy)
            {
                // Проверяем наличие обновлений...
                if (UpMan.CheckHUDUpdate())
                {
                    // Запускаем процесс установки обновления...
                    if (InstallDatabaseUpdate(Properties.Resources.HUDDbFile, UpMan.HUDUpdateURL, UpMan.HUDUpdateHash))
                    {
                        // Обновляем дату последней проверки обновлений...
                        UpdateTimeSetHUD();
                    }
                }
                else
                {
                    // Обновление не требуется. Выводим соответствующее сообщение...
                    MessageBox.Show(AppStrings.UPD_LatestDBInstalled, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.DB_WrkInProgress, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdDBStatus_Click(object sender, EventArgs e)
        {
            if (!WrkChkApp.IsBusy)
            {
                if (UpMan.CheckGameDBUpdate())
                {
                    InstallDatabaseUpdate(Properties.Resources.GameListFile, UpMan.GameUpdateURL, UpMan.GameUpdateHash);
                }
                else
                {
                    MessageBox.Show(AppStrings.UPD_LatestDBInstalled, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.DB_WrkInProgress, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
