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
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace srcrepair
{
    /// <summary>
    /// Класс главной формы приложения.
    /// </summary>
    public partial class FrmMainW : Form
    {
        /// <summary>
        /// Конструктор главной формы приложения.
        /// </summary>
        public FrmMainW()
        {
            // Инициализация...
            InitializeComponent();
            
            // Импортируем настройки из предыдущей версии...
            if (Properties.Settings.Default.CallUpgrade)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.CallUpgrade = false;
            }
        }

        #region Internal Variables

        private string CFGFileName;
        private CurrentApp App;
        private List<SourceGame> SourceGames;
        private SourceGame SelGame;

        #endregion

        private void FrmMainW_Load(object sender, EventArgs e)
        {
            // Событие инициализации формы...
            App = new CurrentApp();
            SourceGames = new List<SourceGame>();

            // Инкрементируем счётчик запусков программы...
            Properties.Settings.Default.LaunchCounter++;

            // Узнаем путь к установленному клиенту Steam...
            try { App.FullSteamPath = App.Platform.OS == CurrentPlatform.OSType.Windows ? SteamManager.GetSteamPath() : SteamManager.TrySteamPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Steam")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); ValidateAndHandle(); }

            // Начинаем платформо-зависимые процедуры...
            ChangePrvControlState(ProcessManager.IsCurrentUserAdmin());

            // Сохраним последний путь к Steam в файл конфигурации...
            Properties.Settings.Default.LastSteamPath = App.FullSteamPath;

            // Вставляем информацию о версии в заголовок формы...
            Text = String.Format(Text, Properties.Resources.AppName, App.Platform.OSFriendlyName, CurrentApp.AppVersion);

            // Укажем статус Безопасной очистки...
            CheckSafeClnStatus();

            // Укажем путь к Steam на странице "Устранение проблем"...
            PS_StPath.Text = String.Format(PS_StPath.Text, App.FullSteamPath);
            
            // Проверим на наличие запрещённых символов в пути к установленному клиенту Steam...
            CheckSymbolsSteam();

            // Запустим поиск установленных игр и проверим нашлось ли что-то...
            FindGames();

            // Проверим наличие обновлений программы...
            CheckForUpdates();
        }

        private void PS_CleanBlobs_CheckedChanged(object sender, EventArgs e)
        {
            // Управляем доступностью кнопки запуска очистки...
            PS_ExecuteNow.Enabled = PS_CleanBlobs.Checked || PS_CleanRegistry.Checked;
        }

        private void PS_CleanRegistry_CheckedChanged(object sender, EventArgs e)
        {
            // Включаем список с доступными языками клиента Steam...
            PS_SteamLang.Enabled = PS_CleanRegistry.Checked;

            // Получим значение текущего языка из реестра Windows...
            if (App.Platform.OS == CurrentPlatform.OSType.Windows)
            {
                try
                {
                    switch (SteamManager.GetSteamLanguage())
                    {
                        case "russian":
                            PS_SteamLang.SelectedIndex = 1;
                            break;
                        default:
                            PS_SteamLang.SelectedIndex = 0;
                            break;
                    }
                }
                catch (Exception Ex)
                {
                    CoreLib.WriteStringToLog(Ex.Message);
                    PS_SteamLang.SelectedIndex = 0;
                }
            }
            else
            {
                // Выбираем язык по умолчанию согласно языку приложения...
                PS_SteamLang.SelectedIndex = Convert.ToInt32(AppStrings.AppDefaultSteamLangID);
            }

            // Управляем доступностью кнопки запуска очистки...
            PS_ExecuteNow.Enabled = PS_CleanRegistry.Checked || PS_CleanBlobs.Checked;
        }

        private void PS_ExecuteNow_Click(object sender, EventArgs e)
        {
            // Начинаем очистку...

            // Запрашиваем подтверждение у пользователя...
            if (MessageBox.Show(AppStrings.PS_ExecuteMSG, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Подтверждение получено...
                if ((PS_CleanBlobs.Checked) || (PS_CleanRegistry.Checked))
                {
                    // Найдём и завершим работу клиента Steam...
                    if (ProcessManager.ProcessTerminate("Steam") != 0)
                    {
                        MessageBox.Show(AppStrings.PS_ProcessDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Проверяем нужно ли чистить блобы...
                    if (PS_CleanBlobs.Checked)
                    {
                        try
                        {
                            // Чистим блобы...
                            SteamManager.CleanBlobsNow(App.FullSteamPath);
                        }
                        catch (Exception Ex)
                        {
                            CoreLib.HandleExceptionEx(AppStrings.PS_CleanException, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                        }
                    }

                    // Проверяем нужно ли чистить реестр...
                    if (PS_CleanRegistry.Checked)
                    {
                        try
                        {
                            // Проверяем выбрал ли пользователь язык из выпадающего списка...
                            if (PS_SteamLang.SelectedIndex != -1)
                            {
                                // Всё в порядке, чистим реестр...
                                SteamManager.CleanRegistryNow(PS_SteamLang.SelectedIndex);
                            }
                            else
                            {
                                // Пользователь не выбрал язык, поэтому будем использовать английский...
                                MessageBox.Show(AppStrings.PS_NoLangSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                SteamManager.CleanRegistryNow(0);
                            }
                        }
                        catch (Exception Ex)
                        {
                            CoreLib.HandleExceptionEx(AppStrings.PS_CleanException, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                        }
                    }

                    // Выполнение закончено, выведем сообщение о завершении...
                    MessageBox.Show(AppStrings.PS_SeqCompleted, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Запустим Steam...
                    if (File.Exists(Path.Combine(App.FullSteamPath, App.Platform.SteamBinaryName))) { Process.Start(Path.Combine(App.FullSteamPath, App.Platform.SteamBinaryName)); }
                }
            }
        }

        private void FrmMainW_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Запрашиваем подтверждение у пользователя на закрытие формы...
                e.Cancel = ((Properties.Settings.Default.ConfirmExit && !(MessageBox.Show(String.Format(AppStrings.FrmCloseQuery, Properties.Resources.AppName), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)) || (BW_BkUpRecv.IsBusy || BW_FPRecv.IsBusy || BW_HudInstall.IsBusy || BW_HUDList.IsBusy || BW_HUDScreen.IsBusy || BW_UpChk.IsBusy));
            }
        }

        private void AppSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Получаем нужные значения...
                SelGame = SourceGames.Find(Item => String.Equals(Item.FullAppName, AppSelector.Text, StringComparison.CurrentCultureIgnoreCase));

                // Переключаем состояние некоторых контролов...
                HandleControlsOnSelGame();
                
                // Проверим наличие запрещённых символов в пути...
                CheckSymbolsGame();

                // Распознаем файловую систему на диске с игрой...
                DetectFS();

                // Считаем настройки графики...
                try { LoadGraphicSettings(); } catch (NotSupportedException) { MessageBox.Show(AppStrings.AppIncorrectSrcVersion, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); }

                // Получаем текущий SteamID...
                HandleSteamIDs(Properties.Settings.Default.LastSteamID);

                // Проверим, установлен ли FPS-конфиг...
                HandleConfigs();

                // Закроем открытые конфиги в редакторе...
                if (!(String.IsNullOrEmpty(CFGFileName))) { CloseEditorConfigs(); }

                // Считаем имеющиеся FPS-конфиги...
                if (!BW_FPRecv.IsBusy) { BW_FPRecv.RunWorkerAsync(); }

                // Обновляем статус...
                UpdateStatusBar();

                // Сохраним ID последней выбранной игры...
                Properties.Settings.Default.LastGameName = AppSelector.Text;

                // Переключаем вид страницы менеджера HUD...
                HandleHUDMode(SelGame.IsHUDsAvailable);

                // Считаем список доступных HUD для данной игры...
                if (SelGame.IsHUDsAvailable) { if (!BW_HUDList.IsBusy) { BW_HUDList.RunWorkerAsync(); } }
                
                // Считаем список бэкапов...
                if (!BW_BkUpRecv.IsBusy) { BW_BkUpRecv.RunWorkerAsync(); }

                // Создадим каталоги кэшей для HUD...
                if (SelGame.IsHUDsAvailable && !Directory.Exists(SelGame.AppHUDDir)) { Directory.CreateDirectory(SelGame.AppHUDDir); }
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(AppStrings.AppFailedToGetData, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
        }

        private void AppRefresh_Click(object sender, EventArgs e)
        {
            // Попробуем обновить список игр...
            FindGames();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Максимальное качество".
        /// Устанавливает графические настройки на рекомендуемый максимум.
        /// </summary>
        private void GT_Maximum_Graphics_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppStrings.GT_MaxPerfMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                switch (SelGame.SourceType)
                {
                    case "1":
                        GT_ScreenType.SelectedIndex = 0;
                        GT_ModelQuality.SelectedIndex = 2;
                        GT_TextureQuality.SelectedIndex = 2;
                        GT_ShaderQuality.SelectedIndex = 1;
                        GT_WaterQuality.SelectedIndex = 1;
                        GT_ShadowQuality.SelectedIndex = 1;
                        GT_ColorCorrectionT.SelectedIndex = 1;
                        GT_AntiAliasing.SelectedIndex = 5;
                        GT_Filtering.SelectedIndex = 5;
                        GT_VSync.SelectedIndex = 0;
                        GT_MotionBlur.SelectedIndex = 0;
                        GT_DxMode.SelectedIndex = 3;
                        GT_HDR.SelectedIndex = 2;
                        break;
                    case "2":
                        GT_NCF_DispMode.SelectedIndex = 0;
                        GT_NCF_Ratio.SelectedIndex = 1;
                        GT_NCF_Brightness.Text = "22";
                        GT_NCF_AntiAlias.SelectedIndex = 5;
                        GT_NCF_Filtering.SelectedIndex = 5;
                        GT_NCF_Shadows.SelectedIndex = 3;
                        GT_NCF_MBlur.SelectedIndex = 1;
                        GT_NCF_VSync.SelectedIndex = 0;
                        GT_NCF_Multicore.SelectedIndex = 1;
                        GT_NCF_ShaderE.SelectedIndex = 3;
                        GT_NCF_EffectD.SelectedIndex = 2;
                        GT_NCF_MemPool.SelectedIndex = 2;
                        GT_NCF_Quality.SelectedIndex = 2;
                        break;
                }
                MessageBox.Show(AppStrings.GT_PerfSet, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Максимальная производительность".
        /// Устанавливает графические настройки на рекомендуемый минимум.
        /// </summary>
        private void GT_Maximum_Performance_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppStrings.GT_MinPerfMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                switch (SelGame.SourceType)
                {
                    case "1":
                        GT_ScreenType.SelectedIndex = 0;
                        GT_ModelQuality.SelectedIndex = 0;
                        GT_TextureQuality.SelectedIndex = 0;
                        GT_ShaderQuality.SelectedIndex = 0;
                        GT_WaterQuality.SelectedIndex = 0;
                        GT_ShadowQuality.SelectedIndex = 0;
                        GT_ColorCorrectionT.SelectedIndex = 0;
                        GT_AntiAliasing.SelectedIndex = 0;
                        GT_Filtering.SelectedIndex = 1;
                        GT_VSync.SelectedIndex = 0;
                        GT_MotionBlur.SelectedIndex = 0;
                        GT_DxMode.SelectedIndex = MessageBox.Show(AppStrings.GT_DxLevelMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes ? 0 : 3;
                        GT_HDR.SelectedIndex = 0;
                        break;
                    case "2":
                        GT_NCF_DispMode.SelectedIndex = 0;
                        GT_NCF_Ratio.SelectedIndex = 1;
                        GT_NCF_Brightness.Text = "22";
                        GT_NCF_AntiAlias.SelectedIndex = 0;
                        GT_NCF_Filtering.SelectedIndex = 1;
                        GT_NCF_Shadows.SelectedIndex = 0;
                        GT_NCF_MBlur.SelectedIndex = 0;
                        GT_NCF_VSync.SelectedIndex = 0;
                        GT_NCF_Multicore.SelectedIndex = 1;
                        GT_NCF_ShaderE.SelectedIndex = 0;
                        GT_NCF_EffectD.SelectedIndex = 0;
                        GT_NCF_MemPool.SelectedIndex = 0;
                        GT_NCF_Quality.SelectedIndex = 0;
                        break;
                }
                MessageBox.Show(AppStrings.GT_PerfSet, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки сохранения графических настроек.
        /// </summary>
        private void GT_SaveApply_Click(object sender, EventArgs e)
        {
            // Сохраняем изменения в графических настройках...
            if (ValidateGameSettings())
            {
                // Запрашиваем подтверждение у пользователя...
                if (MessageBox.Show(AppStrings.GT_SaveMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Сохраняем настройки графики...
                    WriteGraphicSettings();
                }
            }
            else
            {
                // Пользователь заполнил не все поля. Сообщаем ему об этом...
                MessageBox.Show(AppStrings.GT_NCFNReady, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FP_ConfigSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Загружаем данные выбранного конфига...
                try { SelGame.CFGMan.Select(FP_ConfigSel.Text); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Выводим описание...
                FP_Description.Text = SelGame.CFGMan.FPSConfig.Description;

                // Проверим совместимость конфига с игрой...
                FP_Comp.Visible = !SelGame.CFGMan.FPSConfig.CheckCompactibility(SelGame.GameInternalID);

                // Включаем кнопку открытия конфига в Блокноте...
                FP_OpenNotepad.Enabled = true;

                // Включаем кнопку установки конфига...
                FP_Install.Enabled = true;
            }
            catch (Exception Ex)
            {
                // Не получилось загрузить описание выбранного конфига. Выведем стандартное сообщение...
                CoreLib.WriteStringToLog(Ex.Message);
                FP_Description.Text = AppStrings.FP_NoDescr;
            }
        }

        private void FP_Install_Click(object sender, EventArgs e)
        {
            // Начинаем устанавливать FPS-конфиг в управляемое приложение...
            if (FP_ConfigSel.SelectedIndex != -1)
            {
                if (MessageBox.Show(AppStrings.FP_InstallQuestion, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Проверим, не нужно ли создавать резервную копию...
                    if (Properties.Settings.Default.SafeCleanup)
                    {
                        // Проверяем есть ли установленные конфиги...
                        if (SelGame.FPSConfigs.Count > 0)
                        {
                            // Создаём резервную копию...
                            FileManager.CompressFiles(SelGame.FPSConfigs, FileManager.GenerateBackUpFileName(SelGame.FullBackUpDirPath, Properties.Resources.BU_PrefixCfg));
                        }
                    }

                    try
                    {
                        // Устанавливаем...
                        ConfigManager.InstallConfigNow(SelGame.CFGMan.FPSConfig.FileName, App.FullAppPath, SelGame.FullGamePath, SelGame.IsUsingUserDir);
                        
                        // Выводим сообщение об успешной установке...
                        MessageBox.Show(AppStrings.FP_InstallSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Перечитаем конфиги...
                        HandleConfigs();
                    }
                    catch (Exception Ex)
                    {
                        // Установка не удалась. Выводим сообщение об этом...
                        CoreLib.HandleExceptionEx(AppStrings.FP_InstallFailed, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                // Пользователь не выбрал конфиг. Сообщим об этом...
                MessageBox.Show(AppStrings.FP_NothingSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FP_Uninstall_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверим есть ли кандидаты на удаление...
                if (SelGame.FPSConfigs.Count > 0)
                {
                    // Удаляем конфиги...
                    FormManager.FormShowCleanup(SelGame.FPSConfigs, ((Button)sender).Text.ToLower(), AppStrings.FP_RemoveSuccessful, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile, false, false, false, Properties.Settings.Default.SafeCleanup);

                    // Перечитаем список конфигов...
                    HandleConfigs();
                }
                else
                {
                    MessageBox.Show(AppStrings.FP_RemoveNotExists, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(AppStrings.FP_RemoveFailed, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
            }
        }

        private void GT_Warning_Click(object sender, EventArgs e)
        {
            try
            {
                // Предложим пользователю выбрать FPS-конфиг...
                string ConfigFile = FormManager.FormShowCfgSelect(SelGame.FPSConfigs);

                // Проверим выбрал ли что-то пользователь в специальной форме...
                if (!(String.IsNullOrWhiteSpace(ConfigFile)))
                {
                    // Загрузим выбранный конфиг в Редактор конфигов...
                    ReadConfigFromFile(ConfigFile);

                    // Переключимся на него...
                    MainTabControl.SelectedIndex = 1;
                }
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(AppStrings.CS_FailedToOpenCfg, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
        }

        private void CE_New_Click(object sender, EventArgs e)
        {
            // Закрываем все открытые конфиги в Редакторе конфигов и создаём новый пустой файл...
            CloseEditorConfigs();

            // Обновляем содержимое строки статуса...
            UpdateStatusBar();
        }

        private void CE_Open_Click(object sender, EventArgs e)
        {
            // Прочитаем конфиг и заполним его содержимым нашу таблицу редактора...
            
            // Указываем стартовый каталог в диалоге открытия файла на каталог с конфигами игры...
            CE_OpenCfgDialog.InitialDirectory = SelGame.FullCfgPath;

            // Считывает файл конфига и помещает записи в таблицу
            if (CE_OpenCfgDialog.ShowDialog() == DialogResult.OK) // Отображаем стандартный диалог открытия файла...
            {
                // Считываем...
                ReadConfigFromFile(CE_OpenCfgDialog.FileName);
            }
        }

        private void CE_Save_Click(object sender, EventArgs e)
        {
            // Указываем путь по умолчанию к конфигам управляемого приложения...
            CE_SaveCfgDialog.InitialDirectory = SelGame.FullCfgPath;

            // Проверяем, открыт ли какой-либо файл...
            if (!(String.IsNullOrEmpty(CFGFileName)))
            {
                // Будем бэкапить все файлы, сохраняемые в Редакторе...
                if (Properties.Settings.Default.SafeCleanup)
                {
                    // Создаём резервную копию...
                    if (File.Exists(CFGFileName)) { FileManager.CreateConfigBackUp(CFGFileName, SelGame.FullBackUpDirPath, Properties.Resources.BU_PrefixCfg); }
                }

                // Начинаем сохранение в тот же файл...
                try { WriteTableToFileNow(CFGFileName); } catch (Exception Ex) { CoreLib.HandleExceptionEx(AppStrings.CE_CfgSVVEx, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
            }
            else
            {
                // Зададим стандартное имя (см. issue 21)...
                CE_SaveCfgDialog.FileName = File.Exists(Path.Combine(SelGame.FullCfgPath, "autoexec.cfg")) ? AppStrings.UnnamedFileName : "autoexec.cfg";

                // Файл не был открыт. Отображаем стандартный диалог сохранения файла...
                if (CE_SaveCfgDialog.ShowDialog() == DialogResult.OK)
                {
                    WriteTableToFileNow(CE_SaveCfgDialog.FileName);
                    CFGFileName = CE_SaveCfgDialog.FileName;
                    UpdateStatusBar();
                }
            }
        }

        private void CE_SaveAs_Click(object sender, EventArgs e)
        {
            // Сохраняем файл с другим, выбранным пользователем, именем...
            CE_SaveCfgDialog.InitialDirectory = SelGame.FullCfgPath;

            // Отображаем стандартный диалог сохранения файла...
            if (CE_SaveCfgDialog.ShowDialog() == DialogResult.OK)
            {
                WriteTableToFileNow(CE_SaveCfgDialog.FileName);
            }
        }

        private void PS_RemCustMaps_Click(object sender, EventArgs e)
        {
            // Удаляем кастомные (нестандартные) карты...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(SelGame.FullGamePath, "custom", "*.bsp"),
                Path.Combine(SelGame.FullGamePath, "download", "*.bsp"),
                Path.Combine(SelGame.AppWorkshopDir, "*.bsp")
            };
            if (Properties.Settings.Default.AllowUnSafeCleanup) { CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "maps", "*.bsp")); }
            FormManager.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemDnlCache_Click(object sender, EventArgs e)
        {
            // Удаляем кэш загрузок...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(SelGame.FullGamePath, "download", "*.*"),
                Path.Combine(SelGame.FullGamePath, "downloads", "*.*"),
                Path.Combine(SelGame.FullGamePath, "streams", "*.*")
            };
            FormManager.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemSoundCache_Click(object sender, EventArgs e)
        {
            // Удаляем звуковой кэш...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(SelGame.FullGamePath, "maps", "graphs", "*.*"),
                Path.Combine(SelGame.FullGamePath, "maps", "soundcache", "*.*"),
                Path.Combine(SelGame.FullGamePath, "download", "sound", "*.*"),
                Path.Combine(SelGame.FullGamePath, "*.cache")
            };
            FormManager.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemScreenShots_Click(object sender, EventArgs e)
        {
            // Удаляем все скриншоты...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(SelGame.FullGamePath, "screenshots", "*.*")
            };
            FormManager.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile, false, false, false);
        }

        private void PS_RemDemos_Click(object sender, EventArgs e)
        {
            // Удаляем все записанные демки...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(SelGame.FullGamePath, "demos", "*.*"),
                Path.Combine(SelGame.FullGamePath, "*.dem"),
                Path.Combine(SelGame.FullGamePath, "*.mp4"),
                Path.Combine(SelGame.FullGamePath, "*.tga"),
                Path.Combine(SelGame.FullGamePath, "*.wav")
            };
            FormManager.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile, false, false, false, false);
        }

        private void PS_RemGameOpts_Click(object sender, EventArgs e)
        {
            // Создаём список файлов для удаления...
            List<String> CleanDirs = new List<string>();

            // Запрашиваем у пользователя подтверждение удаления...
            if (MessageBox.Show(String.Format(AppStrings.AppQuestionTemplate, ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    // Удаляем графические настройки...
                    if (!SelGame.IsUsingVideoFile)
                    {
                        // Получаем полный путь к ветке реестра игры...
                        string GameRegKey = Type1Video.GetGameRegKey(SelGame.SmallAppName);

                        // Создаём резервную копию куста реестра...
                        if (Properties.Settings.Default.SafeCleanup) { try { Type1Video.BackUpVideoSettings(GameRegKey, "Game_AutoBackUp", SelGame.FullBackUpDirPath); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); } }

                        // Удаляем ключ HKEY_CURRENT_USER\Software\Valve\Source\tf\Settings из реестра...
                        Type1Video.RemoveRegKey(GameRegKey);
                    }
                    else
                    {
                        // Создадим бэкап файла с графическими настройками...
                        if (Properties.Settings.Default.SafeCleanup) { FileManager.CreateConfigBackUp(SelGame.VideoCfgFiles, SelGame.FullBackUpDirPath, Properties.Resources.BU_PrefixVidAuto); }

                        // Помечаем его на удаление...
                        CleanDirs.AddRange(SelGame.VideoCfgFiles);
                    }

                    // Создаём резервную копию...
                    if (Properties.Settings.Default.SafeCleanup) { FileManager.CreateConfigBackUp(SelGame.CloudConfigs, SelGame.FullBackUpDirPath, Properties.Resources.BU_PrefixCfg); }

                    // Помечаем конфиги игры на удаление...
                    CleanDirs.Add(Path.Combine(SelGame.FullCfgPath, "config.cfg"));
                    CleanDirs.AddRange(SelGame.CloudConfigs);

                    // Удаляем всю очередь...
                    FormManager.FormShowRemoveFiles(CleanDirs);

                    // Выводим сообщение...
                    MessageBox.Show(AppStrings.PS_CleanupSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(AppStrings.PS_CleanupErr, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        private void PS_RemOldBin_Click(object sender, EventArgs e)
        {
            // Удаляем старые бинарники...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(SelGame.GamePath, Path.GetDirectoryName(SelGame.SmallAppName), "bin", "*.*"),
                Path.Combine(SelGame.FullGamePath, "bin", "*.*"),
                Path.Combine(SelGame.GamePath, "*.exe")
            };
            if (Properties.Settings.Default.AllowUnSafeCleanup) { CleanDirs.Add(Path.Combine(SelGame.GamePath, "platform", "*.*")); }
            FormManager.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CacheChkReq, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_CheckCache_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format(AppStrings.AppQuestionTemplate, ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try { Process.Start(String.Format("steam://validate/{0}", SelGame.GameInternalID)); } catch (Exception Ex) { CoreLib.HandleExceptionEx(AppStrings.AppStartSteamFailed, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
            }
        }

        private void MNUReportBuilder_Click(object sender, EventArgs e)
        {
            if ((AppSelector.Items.Count > 0) && (AppSelector.SelectedIndex != -1))
            {
                // Запускаем форму создания отчёта для Техподдержки...
                FormManager.FormShowRepBuilder(App.AppUserDir, App.FullSteamPath, SelGame);
            }
            else
            {
                MessageBox.Show(AppStrings.AppNoGamesSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUInstaller_Click(object sender, EventArgs e)
        {
            // Запускаем форму установщика спреев, демок и конфигов...
            FormManager.FormShowInstaller(SelGame.FullGamePath, SelGame.IsUsingUserDir, SelGame.CustomInstallDir);
        }

        private void MNUExit_Click(object sender, EventArgs e)
        {
            // Завершаем работу программы...
            Environment.Exit(0);
        }

        private void MNUAbout_Click(object sender, EventArgs e)
        {
            // Отобразим форму "О программе"...
            FormManager.FormShowAboutApp();
        }

        private void MNUReportBug_Click(object sender, EventArgs e)
        {
            // Перейдём в баг-трекер...
            ProcessManager.OpenWebPage(Properties.Resources.AppBtURL);
        }

        private void BUT_Refresh_Click(object sender, EventArgs e)
        {
            // Обновим список резервных копий...
            UpdateBackUpList();
        }

        private void BUT_RestoreB_Click(object sender, EventArgs e)
        {
            // Восстановим выделенный бэкап...
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    // Запрашиваем подтверждение...
                    if (MessageBox.Show(AppStrings.BU_QMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Обходим выбранные бэкапы в цикле...
                        foreach (ListViewItem BU_Item in BU_LVTable.SelectedItems)
                        {
                            // Проверяем что восстанавливать: конфиг или реестр...
                            switch (Path.GetExtension(BU_Item.SubItems[4].Text))
                            {
                                case ".reg":
                                    // Восстанавливаем файл реестра...
                                    try
                                    {
                                        // Восстанавливаем...
                                        Process.Start("regedit.exe", String.Format("/s \"{0}\"", Path.Combine(SelGame.FullBackUpDirPath, BU_Item.SubItems[4].Text)));
                                    }
                                    catch (Exception Ex)
                                    {
                                        // Произошло исключение...
                                        CoreLib.HandleExceptionEx(AppStrings.BU_RestFailed, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                                    }
                                    break;
                                case ".bud":
                                    // Распаковываем архив с выводом прогресса...
                                    FormManager.FormShowArchiveExtract(Path.Combine(SelGame.FullBackUpDirPath, BU_Item.SubItems[4].Text), Path.GetPathRoot(App.FullSteamPath));

                                    // Обновляем список FPS-конфигов...
                                    HandleConfigs();
                                    break;
                                default:
                                    // Выводим сообщение о неизвестном формате резервной копии...
                                    MessageBox.Show(AppStrings.BU_UnknownType, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    break;
                            }

                            // Выводим сообщение об успехе...
                            MessageBox.Show(AppStrings.BU_RestSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.BU_NoSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.BU_NoFiles, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BUT_DelB_Click(object sender, EventArgs e)
        {
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    // Запросим подтверждение...
                    if (MessageBox.Show(AppStrings.BU_DelMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Обходим выбранные бэкапы в цикле...
                        foreach (ListViewItem BU_Item in BU_LVTable.SelectedItems)
                        {
                            try
                            {
                                // Удаляем файл...
                                File.Delete(Path.Combine(SelGame.FullBackUpDirPath, BU_Item.SubItems[4].Text));

                                // Удаляем строку...
                                BU_LVTable.Items.Remove(BU_Item);
                            }
                            catch (Exception Ex)
                            {
                                // Произошло исключение при попытке удаления файла резервной копии...
                                CoreLib.HandleExceptionEx(AppStrings.BU_DelFailed, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                            }
                        }

                        // Показываем сообщение об успешном удалении...
                        MessageBox.Show(AppStrings.BU_DelSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.BU_NoSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.BU_NoFiles, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BUT_CrBkupReg_ButtonClick(object sender, EventArgs e)
        {
            // Отображаем выпадающее меню...
            BUT_CrBkupReg.ShowDropDown();
        }

        private void BUT_L_GameSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppStrings.BU_RegCreate, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Создадим резервную копию графических настроек игры...
                try
                {
                    if (!SelGame.IsUsingVideoFile)
                    {
                        // Создаём конфиг ветки реестра...
                        Type1Video.BackUpVideoSettings(Type1Video.GetGameRegKey(SelGame.SmallAppName), "Game_Options", SelGame.FullBackUpDirPath);
                    }
                    else
                    {
                        // Проверяем существование файла с графическими настройками игры...
                        FileManager.CreateConfigBackUp(SelGame.VideoCfgFiles, SelGame.FullBackUpDirPath, Properties.Resources.BU_PrefixVideo);
                    }
                    
                    // Выводим сообщение об успехе...
                    MessageBox.Show(AppStrings.BU_RegDone, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Обновляем список резервных копий...
                    UpdateBackUpList();
                }
                catch (Exception Ex)
                {
                    // Выводим сообщение об ошибке и пишем в журнал отладки...
                    CoreLib.HandleExceptionEx(AppStrings.BU_RegErr, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        private void BUT_L_AllSteam_Click(object sender, EventArgs e)
        {
            // Создадим резервную копию всех настроек Steam...
            if (MessageBox.Show(AppStrings.BU_RegCreate, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // Создаём...
                    Type1Video.CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve"), "Steam_BackUp", SelGame.FullBackUpDirPath);
                    
                    // Выводим сообщение...
                    MessageBox.Show(AppStrings.BU_RegDone, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Обновим список бэкапов...
                    UpdateBackUpList();
                }
                catch (Exception Ex)
                {
                    // Произошло исключение, уведомим пользователя...
                    CoreLib.HandleExceptionEx(AppStrings.BU_RegErr, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        private void BUT_L_AllSRC_Click(object sender, EventArgs e)
        {
            // Созданим резервную копию графических настроек всех Source-игр...
            if (MessageBox.Show(AppStrings.BU_RegCreate, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Type1Video.CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve", "Source"), "Source_Options", SelGame.FullBackUpDirPath);
                    MessageBox.Show(AppStrings.BU_RegDone, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateBackUpList();
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(AppStrings.BU_RegErr, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Пользователь переключил вкладку. Обновляем содержимое строки статуса...
            UpdateStatusBar();
        }

        private void CE_ShowHint_Click(object sender, EventArgs e)
        {
            try
            {
                string Buf = CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[0].Value.ToString();
                if (!(String.IsNullOrEmpty(Buf))) { Buf = CurrentApp.GetConVarDescription(Buf); if (!(String.IsNullOrEmpty(Buf))) { MessageBox.Show(Buf, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); } else { MessageBox.Show(AppStrings.CE_ClNoDescr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); } } else { MessageBox.Show(AppStrings.CE_ClSelErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            catch
            {
                MessageBox.Show(AppStrings.CE_ClSelErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUHelp_Click(object sender, EventArgs e)
        {
            // Отобразим справочную систему в зависимости от контекста...
            ProcessManager.OpenWebPage(GetHelpWebPage(MainTabControl.SelectedIndex));
        }

        private void MNUOpinion_Click(object sender, EventArgs e)
        {
            ProcessManager.OpenWebPage(Properties.Resources.AppURLReply);
        }

        private void MNUSteamGroup_Click(object sender, EventArgs e)
        {
            try { Process.Start(Properties.Resources.AppURLSteamGrID); } catch { ProcessManager.OpenWebPage(Properties.Resources.AppURLSteamGroup); }
        }

        private void MNULnkEasyCoding_Click(object sender, EventArgs e)
        {
            ProcessManager.OpenWebPage(Properties.Resources.AppURLOffSite);
        }

        private void MNULnkTFRU_Click(object sender, EventArgs e)
        {
            ProcessManager.OpenWebPage(Properties.Resources.AppURLSpnTFSU);
        }

        private void MNUHEd_Click(object sender, EventArgs e)
        {
            // Отобразим форму редактора файла hosts...
            FormManager.FormShowHostsEditor();
        }

        private void CE_RmRow_Click(object sender, EventArgs e)
        {
            try
            {
                if (CE_Editor.Rows.Count > 0)
                {
                    CE_Editor.Rows.Remove(CE_Editor.CurrentRow);
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void CE_Copy_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                foreach (DataGridViewCell DV in CE_Editor.SelectedCells) { if (DV.Value != null) { SB.AppendFormat("{0} ", DV.Value); } }
                Clipboard.SetText(SB.ToString().Trim());
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void CE_Cut_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                foreach (DataGridViewCell DV in CE_Editor.SelectedCells) { if (DV.Value != null) { SB.AppendFormat("{0} ", DV.Value); DV.Value = null; } }
                Clipboard.SetText(SB.ToString().Trim());
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void CE_Paste_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value = Clipboard.GetText();
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void FP_OpenNotepad_Click(object sender, EventArgs e)
        {
            // Сгенерируем путь к файлу...
            string ConfigFile = Path.Combine(App.FullAppPath, "cfgs", SelGame.CFGMan.FPSConfig.FileName);
            
            // Проверим зажал ли пользователь Shift перед тем, как кликнуть по кнопке...
            if (Control.ModifierKeys == Keys.Shift)
            {
                // Загрузим выбранный конфиг в Редактор конфигов...
                ReadConfigFromFile(ConfigFile);

                // Переключимся на него...
                MainTabControl.SelectedIndex = 1;
            }
            else
            {
                // Загрузим файл в Блокноте...
                ProcessManager.OpenTextEditor(ConfigFile, App.Platform.OS);
            }
        }

        private void MNUUpdateCheck_Click(object sender, EventArgs e)
        {
            // Откроем форму модуля проверки обновлений...
            FormManager.FormShowUpdater(App.UserAgent, App.FullAppPath, App.AppUserDir, App.Platform);
            
            // Перечитаем базу игр...
            FindGames();
        }

        private void BUT_OpenNpad_Click(object sender, EventArgs e)
        {
            // Откроем выбранный бэкап в Блокноте Windows...
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    if (Regex.IsMatch(Path.GetExtension(BU_LVTable.SelectedItems[0].SubItems[4].Text), @"\.(txt|cfg|[0-9]|reg)")) { ProcessManager.OpenTextEditor(Path.Combine(SelGame.FullBackUpDirPath, BU_LVTable.SelectedItems[0].SubItems[4].Text), App.Platform.OS); } else { MessageBox.Show(AppStrings.BU_BinaryFile, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
                else
                {
                    MessageBox.Show(AppStrings.BU_NoSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.BU_NoFiles, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUAppOptions_Click(object sender, EventArgs e)
        {
            // Показываем форму настроек...
            FormManager.FormShowOptions();
        }

        private void BU_LVTable_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            // Запрещаем изменение размеров столбцов таблицы...
            e.NewWidth = BU_LVTable.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void BUT_ExploreBUp_Click(object sender, EventArgs e)
        {
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    // Откроем выбранный бэкап в Проводнике Windows...
                    ProcessManager.OpenExplorer(Path.Combine(SelGame.FullBackUpDirPath, BU_LVTable.SelectedItems[0].SubItems[4].Text), App.Platform.OS);
                }
                else
                {
                    MessageBox.Show(AppStrings.BU_NoSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.BU_NoFiles, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FrmMainW_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Форма была закрыта, сохраняем настройки приложения...
            Properties.Settings.Default.Save();
        }

        private void MNUWinMnuDisabler_Click(object sender, EventArgs e)
        {
            // Показываем модуля отключения клавиш...
            FormManager.FormShowKBHelper();
        }

        private void CE_OpenInNotepad_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(CFGFileName)))
            {
                ProcessManager.OpenTextEditor(CFGFileName, App.Platform.OS);
            }
            else
            {
                MessageBox.Show(AppStrings.CE_NoFileOpened, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PS_PathDetector_Click(object sender, EventArgs e)
        {
            if (((Label)sender).ForeColor == Color.Red) { MessageBox.Show(AppStrings.SteamNonASCIIDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); } else { MessageBox.Show(AppStrings.SteamNonASCIINotDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void PS_PathGame_Click(object sender, EventArgs e)
        {
            if (((Label)sender).ForeColor == Color.Red) { MessageBox.Show(AppStrings.GameNonASCIIDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); } else { MessageBox.Show(AppStrings.GameNonASCIINotDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void PS_RemReplays_Click(object sender, EventArgs e)
        {
            // Удаляем все реплеи...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(SelGame.FullGamePath, "replay", "*.*")
            };
            FormManager.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemTextures_Click(object sender, EventArgs e)
        {
            // Удаляем все кастомные текстуры...
            List<String> CleanDirs = new List<string>
            {
                // Чистим загруженные с серверов модели и текстуры...
                Path.Combine(SelGame.FullGamePath, "download", "*.vt*"),
                Path.Combine(SelGame.FullGamePath, "download", "*.vmt"),
                Path.Combine(SelGame.FullGamePath, "download", "*.mdl"),
                Path.Combine(SelGame.FullGamePath, "download", "*.phy"),
                
                // Чистим установленные пользователем модели и текстуры...
                Path.Combine(SelGame.FullGamePath, "custom", "*.vt*"),
                Path.Combine(SelGame.FullGamePath, "custom", "*.vmt"),
                Path.Combine(SelGame.FullGamePath, "custom", "*.mdl"),
                Path.Combine(SelGame.FullGamePath, "custom", "*.phy")
            };

            // Чистим базы игр со старой системой. Удалить после полного перехода на новую...
            if (Properties.Settings.Default.AllowUnSafeCleanup)
            {
                CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "materials", "*.*"));
                CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "models", "*.*"));
            }
            FormManager.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemSecndCache_Click(object sender, EventArgs e)
        {
            // Удаляем содержимое вторичного кэша загрузок...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(SelGame.FullGamePath, "cache", "*.*"), // Кэш...
                Path.Combine(SelGame.FullGamePath, "custom", "user_custom", "*.*"), // Кэш спреев игр с н.с.к...
                Path.Combine(SelGame.GamePath, "config", "html", "*.*") // Кэш MOTD...
            };
            FormManager.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void SB_App_DoubleClick(object sender, EventArgs e)
        {
            // Переключим статус безопасной очистки...
            Properties.Settings.Default.SafeCleanup = !Properties.Settings.Default.SafeCleanup;
            
            // Сообщим пользователю если он отключил безопасную очистку...
            if (!Properties.Settings.Default.SafeCleanup)
            {
                MessageBox.Show(AppStrings.AppSafeClnDisabled, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            // Обновим статусную строку...
            CheckSafeClnStatus();
        }

        private void CE_OpenCVList_Click(object sender, EventArgs e)
        {
            ProcessManager.OpenWebPage(AppStrings.AppCVListURL);
        }

        private void CE_ManualBackUpCfg_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(CFGFileName)))
            {
                if (File.Exists(CFGFileName))
                {
                    FileManager.CreateConfigBackUp(CFGFileName, SelGame.FullBackUpDirPath, Properties.Resources.BU_PrefixCfg);
                    MessageBox.Show(String.Format(AppStrings.CE_BackUpCreated, Path.GetFileName(CFGFileName)), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.CE_NoFileOpened, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PS_RemSounds_Click(object sender, EventArgs e)
        {
            // Удаляем кастомные звуки...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(SelGame.FullGamePath, "download", "*.mp3"),
                Path.Combine(SelGame.FullGamePath, "download", "*.wav")
            };
            if (Properties.Settings.Default.AllowUnSafeCleanup) { CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "sound", "*.*")); }
            FormManager.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemCustDir_Click(object sender, EventArgs e)
        {
            // Удаляем пользовательного каталога...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(SelGame.FullGamePath, "custom", "*.*"),
                Path.Combine(SelGame.AppWorkshopDir, "*.*")
            };
            FormManager.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_DeepCleanup_Click(object sender, EventArgs e)
        {
            // Проведём глубокую очистку...
            List<String> CleanDirs = new List<string>
            {
                // Удалим старые бинарники и лаунчеры...
                Path.Combine(SelGame.GamePath, Path.GetDirectoryName(SelGame.SmallAppName), "bin", "*.*"),
                Path.Combine(SelGame.FullGamePath, "bin", "*.*"),
                Path.Combine(SelGame.GamePath, "*.exe"),

                // Удалим кэш загрузок...
                Path.Combine(SelGame.FullGamePath, "download", "*.*"),

                // Удалим кастомные файлы...
                Path.Combine(SelGame.FullGamePath, "custom", "*.*"),
                Path.Combine(SelGame.AppWorkshopDir, "*.*"),

                // Удалим другие кэши...
                Path.Combine(SelGame.FullGamePath, "cache", "*.*"),

                // Удалим пользовательские конфиги...
                Path.Combine(SelGame.FullGamePath, "cfg", "*.cfg")
            };

            // Конфиги их хранилища Steam Cloud...
            CleanDirs.AddRange(SelGame.CloudConfigs);

            // Данные платформы...
            if (Properties.Settings.Default.AllowUnSafeCleanup) { CleanDirs.Add(Path.Combine(SelGame.GamePath, "platform", "*.*")); }
            
            // Удаляем графические настройки NCF-игры...
            if (SelGame.IsUsingVideoFile) { CleanDirs.AddRange(SelGame.VideoCfgFiles); }

            // Запускаем процесс очистки...
            FormManager.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CacheChkReq, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemConfigs_Click(object sender, EventArgs e)
        {
            // Удаляем пользовательного каталога...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(SelGame.FullGamePath, "cfg", "*.*"),
                Path.Combine(SelGame.FullGamePath, "custom", "*.cfg")
            };
            CleanDirs.AddRange(SelGame.CloudConfigs);
            FormManager.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void HD_HSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получим информацию о выбранном HUD...
            try { SelGame.HUDMan.Select(HD_HSel.Text); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                
            // Проверяем результат...
            bool Success = !String.IsNullOrEmpty(SelGame.HUDMan.SelectedHUD.Name);

            // Переключаем статус элементов управления...
            HD_GB_Pbx.Image = Properties.Resources.LoadingFile;
            HD_Install.Enabled = Success;
            HD_Homepage.Enabled = Success;
            HD_Warning.Visible = Success && !SelGame.HUDMan.SelectedHUD.IsUpdated;

            // Выводим информацию о последнем обновлении HUD...
            HD_LastUpdate.Visible = Success;
            if (Success) { HD_LastUpdate.Text = String.Format(AppStrings.HD_LastUpdateInfo, FileManager.Unix2DateTime(SelGame.HUDMan.SelectedHUD.LastUpdate).ToLocalTime()); }

            // Проверяем установлен ли выбранный HUD...
            SetHUDButtons(HUDManager.CheckInstalledHUD(SelGame.CustomInstallDir, SelGame.HUDMan.SelectedHUD.InstallDir));

            // Загрузим скриншот выбранного HUD...
            if (Success && !BW_HUDScreen.IsBusy) { BW_HUDScreen.RunWorkerAsync(); }
        }

        private void HD_Install_Click(object sender, EventArgs e)
        {
            if (!HUDManager.CheckHUDDatabase(Properties.Settings.Default.LastHUDTime))
            {
                // Проверим поддерживает ли выбранный HUD последнюю версию игры...
                if (SelGame.HUDMan.SelectedHUD.IsUpdated)
                {
                    // Спросим пользователя о необходимости установки/обновления HUD...
                    if (MessageBox.Show(String.Format("{0}?", ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Проверим установлен ли выбранный HUD...
                        if (HUDManager.CheckInstalledHUD(SelGame.CustomInstallDir, SelGame.HUDMan.SelectedHUD.InstallDir))
                        {
                            // Удаляем уже установленные файлы HUD...
                            FormManager.FormShowRemoveFiles(SingleToArray(Path.Combine(SelGame.CustomInstallDir, SelGame.HUDMan.SelectedHUD.InstallDir)));
                        }

                        // Начинаем загрузку архива с HUD...
                        FormManager.FormShowDownloader(Properties.Settings.Default.HUDUseUpstream ? SelGame.HUDMan.SelectedHUD.UpURI : SelGame.HUDMan.SelectedHUD.URI, SelGame.HUDMan.SelectedHUD.LocalFile);

                        // Проверяем контрольную сумму загруженного архива...
                        if (Properties.Settings.Default.HUDUseUpstream || FileManager.CalculateFileMD5(SelGame.HUDMan.SelectedHUD.LocalFile) == SelGame.HUDMan.SelectedHUD.FileHash)
                        {
                            // Распаковываем загруженный архив с файлами HUD...
                            FormManager.FormShowArchiveExtract(SelGame.HUDMan.SelectedHUD.LocalFile, Path.Combine(SelGame.CustomInstallDir, "hudtemp"));

                            // Запускаем установку пакета в отдельном потоке...
                            if (!BW_HudInstall.IsBusy) { BW_HudInstall.RunWorkerAsync(); }
                        }
                        else
                        {
                            // Проверка хеша загруженного файла не удалась. Выведем сообщение об этом...
                            MessageBox.Show(AppStrings.HD_HashError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        // Проверяем существует ли файл с архивом. Если да, то удаляем...
                        try { if (File.Exists(SelGame.HUDMan.SelectedHUD.LocalFile)) { File.Delete(SelGame.HUDMan.SelectedHUD.LocalFile); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    }
                }
                else
                {
                    // Выбран устаревший HUD. Выведем сообщение об этом...
                    MessageBox.Show(AppStrings.HD_Outdated, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // База HUD устарела. Требуется обновление. Выведем сообщение...
                MessageBox.Show(AppStrings.HD_DbOutdated, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void HD_Uninstall_Click(object sender, EventArgs e)
        {
            // Спросим пользователя о необходимости удаления HUD...
            if (MessageBox.Show(String.Format("{0}?", ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Сгенерируем полный путь к установленному HUD...
                string HUDPath = Path.Combine(SelGame.CustomInstallDir, SelGame.HUDMan.SelectedHUD.InstallDir);

                // Воспользуемся модулем быстрой очистки для удаления выбранного HUD...
                FormManager.FormShowRemoveFiles(SingleToArray(HUDPath));

                // Проверяем установлен ли выбранный HUD...
                bool IsInstalled = HUDManager.CheckInstalledHUD(SelGame.CustomInstallDir, SelGame.HUDMan.SelectedHUD.InstallDir);

                // При успешном удалении HUD выводим сообщение и сносим и его каталог...
                if (!IsInstalled) { MessageBox.Show(AppStrings.PS_CleanupSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); if (Directory.Exists(HUDPath)) { Directory.Delete(HUDPath); } }

                // Включаем / отключаем кнопки...
                SetHUDButtons(IsInstalled);
            }
        }

        private void HD_Homepage_Click(object sender, EventArgs e)
        {
            // Откроем домашнюю страницу выбранного HUD...
            if (!String.IsNullOrEmpty(SelGame.HUDMan.SelectedHUD.Site)) { ProcessManager.OpenWebPage(SelGame.HUDMan.SelectedHUD.Site); }
        }

        private void MNUExtClnAppCache_Click(object sender, EventArgs e)
        {
            // Очистим загруженные приложением файлы...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(App.AppUserDir, Properties.Resources.HUDLocalDir, "*.*")
            };
            FormManager.FormShowCleanup(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", String.Empty), AppStrings.PS_CleanupSuccess, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void MNUExtClnTmpDir_Click(object sender, EventArgs e)
        {
            // Очистим каталоги с временными файлами системы...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(Path.GetTempPath(), "*.*")
            };
            FormManager.FormShowCleanup(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", String.Empty), AppStrings.PS_CleanupSuccess, SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void MNUShowLog_Click(object sender, EventArgs e)
        {
            // Выведем на экран содержимое отладочного журнала...
            if (Properties.Settings.Default.EnableDebugLog)
            {
                string DFile = Path.Combine(App.AppUserDir, Properties.Resources.DebugLogFileName);
                if (File.Exists(DFile)) { FormManager.FormShowLogViewer(DFile); } else { MessageBox.Show(AppStrings.AppNoDebugFile, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            else
            {
                MessageBox.Show(AppStrings.AppDebugDisabled, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void HD_Warning_Click(object sender, EventArgs e)
        {
            // Выведем предупреждающие сообщения...
            if (!SelGame.HUDMan.SelectedHUD.IsUpdated) { MessageBox.Show(AppStrings.HD_NotTested, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void HD_OpenDir_Click(object sender, EventArgs e)
        {
            // Покажем файлы установленного HUD в Проводнике...
            ProcessManager.OpenExplorer(Path.Combine(SelGame.CustomInstallDir, SelGame.HUDMan.SelectedHUD.InstallDir), App.Platform.OS);
        }

        private void MNUExtClnSteam_Click(object sender, EventArgs e)
        {
            // Запустим модуль очистки кэшей Steam...
            FormManager.FormShowStmCleaner(App.FullSteamPath, SelGame.FullBackUpDirPath, App.Platform.SteamAppsFolderName, App.Platform.SteamProcName);
        }

        private void MNUMuteMan_Click(object sender, EventArgs e)
        {
            // Запустим менеджер управления отключёнными игроками...
            FormManager.FormShowMuteManager(SelGame.GetActualBanlistFile(), SelGame.FullBackUpDirPath);
        }

        private void MNUSupportChat_Click(object sender, EventArgs e)
        {
            // Откроем канал поддержки в клиенте Telegram для десктопа, а если он не установлен - в браузере...
            try { Process.Start(Properties.Resources.AppTgChannel); } catch { ProcessManager.OpenWebPage(Properties.Resources.AppTgChannelURL); }
        }

        private void SB_SteamID_Click(object sender, EventArgs e)
        {
            // Открываем диалог выбора SteamID и прописываем пользовательский выбор...
            try { string Result = FormManager.FormShowIDSelect(SelGame.SteamIDs); if (!(String.IsNullOrWhiteSpace(Result))) { SB_SteamID.Text = Result; Properties.Settings.Default.LastSteamID = Result; FindGames(); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void BU_LVTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Блокируем некоторые кнопки на панели инструментов модуля управления резервными копиями если выбрано более одной...
            bool IsSingle = BU_LVTable.SelectedItems.Count <= 1;
            BUT_OpenNpad.Enabled = IsSingle;
            BUT_ExploreBUp.Enabled = IsSingle;
        }

        private void MNUDonate_Click(object sender, EventArgs e)
        {
            // Откроем веб-страницу с реквизитами...
            ProcessManager.OpenWebPage(Properties.Resources.AppURLDonate);
        }
    }
}
