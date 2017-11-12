/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2017 EasyCoding Team.
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace srcrepair
{
    public partial class FrmMainW
    {
        private void BW_UpChk_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Вычисляем разницу между текущей датой и датой последнего обновления...
                TimeSpan TS = DateTime.Now - Properties.Settings.Default.LastUpdateTime;
                if (TS.Days >= 7) // Проверяем не прошла ли неделя с момента последней прверки...
                {
                    // Требуется проверка обновлений...
                    if (AutoUpdateCheck())
                    {
                        // Доступны обновления...
                        MessageBox.Show(String.Format(AppStrings.AppUpdateAvailable, Properties.Resources.AppName), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Установим время последней проверки обновлений...
                        Properties.Settings.Default.LastUpdateTime = DateTime.Now;
                    }
                }
            }
            catch (Exception Ex)
            {
                CoreLib.WriteStringToLog(Ex.Message);
            }
        }

        private void BW_UpChk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Произошла ошибка во время проверки наличия обновлений. Запишем в журнал...
            if (e.Error != null)
            {
                CoreLib.WriteStringToLog(e.Error.Message);
            }
        }

        private void BW_FPRecv_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Получаем список установленных конфигов из БД...
                SelGame.CFGMan = new ConfigManager(Path.Combine(App.FullAppPath, Properties.Resources.CfgDbFile), AppStrings.AppLangPrefix);

                // Выведем установленные в форму...
                foreach (string Str in SelGame.CFGMan.GetAllCfg())
                {
                    Invoke((MethodInvoker)delegate () { FP_ConfigSel.Items.Add(Str); });
                }
            }
            catch (Exception Ex)
            {
                // FPS-конфигов не найдено. Запишем в лог...
                CoreLib.WriteStringToLog(Ex.Message);

                // Выводим текст об этом...
                FP_Description.Text = AppStrings.FP_NoCfgGame;
                FP_Description.ForeColor = Color.Red;

                // ...и блокируем контролы, отвечающие за установку...
                FP_Install.Enabled = false;
                FP_ConfigSel.Enabled = false;
                FP_OpenNotepad.Enabled = false;
            }
        }

        private void BW_FPRecv_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Проверяем, нашлись ли конфиги...
            if (FP_ConfigSel.Items.Count >= 1)
            {
                FP_Description.Text = AppStrings.FP_SelectFromList;
                FP_Description.ForeColor = Color.Black;
                FP_ConfigSel.Enabled = true;
            }
        }

        private void BW_BkUpRecv_DoWork(object sender, DoWorkEventArgs e)
        {
            // Получаем список резеверных копий...
            UpdateBackUpList();
        }

        private void BW_HUDList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Получаем список доступных HUD...
                SelGame.HUDMan = new HUDManager(Path.Combine(App.FullAppPath, Properties.Resources.HUDDbFile), SelGame.AppHUDDir);

                // Вносим HUD текущей игры в форму...
                Invoke((MethodInvoker)delegate () { HD_HSel.Items.AddRange(SelGame.HUDMan.GetHUDNames(SelGame.SmallAppName).ToArray<object>()); });
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void BW_HUDScreen_DoWork(object sender, DoWorkEventArgs e)
        {
            // Сгенерируем путь к файлу со скриншотом...
            string ScreenFile = Path.Combine(SelGame.AppHUDDir, Path.GetFileName(SelGame.HUDMan.SelectedHUD.Preview));

            try
            {
                // Загрузим файл если не существует...
                if (!File.Exists(ScreenFile))
                {
                    using (WebClient Downloader = new WebClient())
                    {
                        Downloader.Headers.Add("User-Agent", App.UserAgent);
                        Downloader.DownloadFile(SelGame.HUDMan.SelectedHUD.Preview, ScreenFile);
                    }
                }

                // Установим...
                Invoke((MethodInvoker)delegate () { HD_GB_Pbx.Image = Image.FromFile(ScreenFile); });
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); if (File.Exists(ScreenFile)) { File.Delete(ScreenFile); } }
        }

        private void BW_HudInstall_DoWork(object sender, DoWorkEventArgs e)
        {
            // Сохраняем предыдующий текст кнопки...
            string CaptText = HD_Install.Text;
            string InstallTmp = Path.Combine(SelGame.CustomInstallDir, "hudtemp");

            try
            {
                // Изменяем текст на "Идёт установка" и отключаем её...
                Invoke((MethodInvoker)delegate () { HD_Install.Text = AppStrings.HD_InstallBtnProgress; HD_Install.Enabled = false; });

                // Устанавливаем и очищаем временный каталог...
                try { Directory.Move(Path.Combine(InstallTmp, HUDManager.FormatIntDir(SelGame.HUDMan.SelectedHUD.ArchiveDir)), Path.Combine(SelGame.CustomInstallDir, SelGame.HUDMan.SelectedHUD.InstallDir)); }
                finally { if (Directory.Exists(InstallTmp)) { Directory.Delete(InstallTmp, true); } }
            }
            finally
            {
                // Возвращаем сохранённый...
                Invoke((MethodInvoker)delegate () { HD_Install.Text = CaptText; HD_Install.Enabled = true; });
            }
        }

        private void BW_HudInstall_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Выводим сообщение...
            if (e.Error == null) { MessageBox.Show(AppStrings.HD_InstallSuccessfull, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); } else { CoreLib.HandleExceptionEx(AppStrings.HD_InstallError, Properties.Resources.AppName, e.Error.Message, e.Error.Source, MessageBoxIcon.Error); }

            // Включаем кнопку удаления если HUD установлен...
            SetHUDButtons(HUDManager.CheckInstalledHUD(SelGame.CustomInstallDir, SelGame.HUDMan.SelectedHUD.InstallDir));
        }
    }
}
