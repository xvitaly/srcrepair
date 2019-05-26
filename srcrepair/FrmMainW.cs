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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
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

        #region HiDPI hacks

        /// <summary>
        /// Управляет масштабированием элементов управления на форме.
        /// </summary>
        /// <param name="ScalingFactor">Множитель масштабирования</param>
        /// <param name="Bounds">Границы элемента управления</param>
        protected override void ScaleControl(SizeF ScalingFactor, BoundsSpecified Bounds)
        {
            base.ScaleControl(ScalingFactor, Bounds);
            if (!DpiManager.CompareFloats(Math.Max(ScalingFactor.Width, ScalingFactor.Height), 1.0f))
            {
                DpiManager.ScaleColumnsInControl(CE_Editor, ScalingFactor);
                DpiManager.ScaleColumnsInControl(BU_LVTable, ScalingFactor);
                DpiManager.ScaleColumnsInControl(StatusBar, ScalingFactor);
            }
        }

        #endregion

        #region Internal Variables

        private string CFGFileName;
        private CurrentApp App;
        private Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Internal Methods

        /// <summary>
        /// Сохраняет содержимое таблицы в файл конфигурации, указанный в
        /// параметре. Используется в Save и SaveAs Редактора конфигов.
        /// </summary>
        /// <param name="Path">Полный путь к файлу конфига</param>
        private void WriteTableToFileNow(string Path)
        {
            // Начинаем сохранять содержимое редактора в файл...
            using (StreamWriter CFile = new StreamWriter(Path))
            {
                for (int i = 0; i < CE_Editor.Rows.Count; i++) // запускаем цикл
                {
                    CFile.WriteLine("{0} {1}", CE_Editor.Rows[i].Cells[0].Value, CE_Editor.Rows[i].Cells[1].Value);
                }
            }
        }

        /// <summary>
        /// Определяет установленные игры и заполняет комбо-бокс выбора
        /// доступных управляемых игр.
        /// </summary>
        private void DetectInstalledGames()
        {
            try
            {
                // Считаем базу данных и заполним таблицу...
                App.SourceGames = new GameManager(App, Properties.Settings.Default.HideUnsupportedGames);

                // Очистим список игр...
                AppSelector.Items.Clear();

                // Заполним селектор...
                AppSelector.Items.AddRange(App.SourceGames.InstalledGames.ToArray());

            }
            catch (Exception Ex) { Logger.Warn(Ex); }
        }

        /// <summary>
        /// Записывает настройки GCF-игры в реестр Windows.
        /// </summary>
        private void WriteType1VideoSettings()
        {
            // Создаём новый объект без получения данных из реестра...
            Type1Video Video = new Type1Video(App.SourceGames.SelectedGame.ConfDir, false)
            {
                // Записываем пользовательские настройки...
                ScreenWidth = (int)GT_ResHor.Value,
                ScreenHeight = (int)GT_ResVert.Value,
                DisplayMode = GT_ScreenType.SelectedIndex,
                ModelQuality = GT_ModelQuality.SelectedIndex,
                TextureQuality = GT_TextureQuality.SelectedIndex,
                ShaderQuality = GT_ShaderQuality.SelectedIndex,
                ReflectionsQuality = GT_WaterQuality.SelectedIndex,
                ShadowQuality = GT_ShadowQuality.SelectedIndex,
                ColorCorrection = GT_ColorCorrectionT.SelectedIndex,
                AntiAliasing = GT_AntiAliasing.SelectedIndex,
                FilteringMode = GT_Filtering.SelectedIndex,
                VSync = GT_VSync.SelectedIndex,
                MotionBlur = GT_MotionBlur.SelectedIndex,
                DirectXMode = GT_DxMode.SelectedIndex,
                HDRType = GT_HDR.SelectedIndex
            };

            // Записываем настройки в реестр...
            Video.WriteSettings();
        }

        /// <summary>
        /// Сохраняет настройки NCF игры в файл.
        /// </summary>
        private void WriteType2VideoSettings()
        {
            // Создаём новый объект без получения данных из файла...
            Type2Video Video = new Type2Video(App.SourceGames.SelectedGame.GetActualVideoFile(), App.SourceGames.SelectedGame.SourceType, false)
            {
                // Записываем пользовательские настройки...
                ScreenWidth = (int)GT_NCF_HorRes.Value,
                ScreenHeight = (int)GT_NCF_VertRes.Value,
                ScreenRatio = GT_NCF_Ratio.SelectedIndex,
                ScreenGamma = GT_NCF_Brightness.Text,
                ShadowQuality = GT_NCF_Shadows.SelectedIndex,
                MotionBlur = GT_NCF_MBlur.SelectedIndex,
                ScreenMode = GT_NCF_DispMode.SelectedIndex,
                AntiAliasing = GT_NCF_AntiAlias.SelectedIndex,
                FilteringMode = GT_NCF_Filtering.SelectedIndex,
                VSync = GT_NCF_VSync.SelectedIndex,
                RenderingMode = GT_NCF_Multicore.SelectedIndex,
                ShaderEffects = GT_NCF_ShaderE.SelectedIndex,
                Effects = GT_NCF_EffectD.SelectedIndex,
                MemoryPool = GT_NCF_MemPool.SelectedIndex,
                ModelQuality = GT_NCF_Quality.SelectedIndex
            };

            // Записываем настройки в файл...
            Video.WriteSettings();
        }

        /// <summary>
        /// Получает настройки первого типа игры из реестра и заполняет
        /// полученными данными страницу графического твикера.
        /// </summary>
        private void ReadType1VideoSettings()
        {
            try
            {
                // Получаем графические настройки...
                Type1Video Video = new Type1Video(App.SourceGames.SelectedGame.ConfDir, true);

                // Заполняем общие настройки...
                GT_ResHor.Value = Video.ScreenWidth;
                GT_ResVert.Value = Video.ScreenHeight;

                // Заполняем остальные настройки...
                GT_ScreenType.SelectedIndex = Video.DisplayMode;
                GT_ModelQuality.SelectedIndex = Video.ModelQuality;
                GT_TextureQuality.SelectedIndex = Video.TextureQuality;
                GT_ShaderQuality.SelectedIndex = Video.ShaderQuality;
                GT_WaterQuality.SelectedIndex = Video.ReflectionsQuality;
                GT_ShadowQuality.SelectedIndex = Video.ShadowQuality;
                GT_ColorCorrectionT.SelectedIndex = Video.ColorCorrection;
                GT_AntiAliasing.SelectedIndex = Video.AntiAliasing;
                GT_Filtering.SelectedIndex = Video.FilteringMode;
                GT_VSync.SelectedIndex = Video.VSync;
                GT_MotionBlur.SelectedIndex = Video.MotionBlur;
                GT_DxMode.SelectedIndex = Video.DirectXMode;
                GT_HDR.SelectedIndex = Video.HDRType;
            }
            catch (Exception Ex)
            {
                // Выводим сообщение об ошибке...
                MessageBox.Show(AppStrings.GT_RegOpenErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, AppStrings.AppDbgExT1LoadFail);
            }
        }

        /// <summary>
        /// Получает настройки второго типа игры из файла и заполняет ими
        /// таблицу графического твикера программы.
        /// </summary>
        private void ReadType2VideoSettings()
        {
            try
            {
                // Получаем актуальный файл с настройками видео...
                string VFileName = App.SourceGames.SelectedGame.GetActualVideoFile();

                // Загружаем содержимое если он существует...
                if (File.Exists(VFileName))
                {
                    // Получаем графические настройки...
                    Type2Video Video = new Type2Video(VFileName, App.SourceGames.SelectedGame.SourceType, true);

                    // Заполняем общие настройки...
                    GT_NCF_HorRes.Value = Video.ScreenWidth;
                    GT_NCF_VertRes.Value = Video.ScreenHeight;

                    // Заполняем остальные настройки...
                    GT_NCF_Ratio.SelectedIndex = Video.ScreenRatio;
                    GT_NCF_Brightness.Text = Video.ScreenGamma;
                    GT_NCF_Shadows.SelectedIndex = Video.ShadowQuality;
                    GT_NCF_MBlur.SelectedIndex = Video.MotionBlur;
                    GT_NCF_DispMode.SelectedIndex = Video.ScreenMode;
                    GT_NCF_AntiAlias.SelectedIndex = Video.AntiAliasing;
                    GT_NCF_Filtering.SelectedIndex = Video.FilteringMode;
                    GT_NCF_VSync.SelectedIndex = Video.VSync;
                    GT_NCF_Multicore.SelectedIndex = Video.RenderingMode;
                    GT_NCF_ShaderE.SelectedIndex = Video.ShaderEffects;
                    GT_NCF_EffectD.SelectedIndex = Video.Effects;
                    GT_NCF_MemPool.SelectedIndex = Video.MemoryPool;
                    GT_NCF_Quality.SelectedIndex = Video.ModelQuality;
                }
                else
                {
                    // Записываем в журнал сообщение об ошибке...
                    Logger.Warn(String.Format(AppStrings.AppVideoDbNotFound, App.SourceGames.SelectedGame.FullAppName, VFileName));
                }
            }
            catch (Exception Ex)
            {
                // Выводим сообщение об ошибке...
                MessageBox.Show(AppStrings.GT_NCFLoadFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, AppStrings.AppDbgExT2LoadFail);
            }
        }

        /// <summary>
        /// Проверяет наличие обновлений для программы. Используется главной формой.
        /// </summary>
        /// <returns>Возвращает true при обнаружении обновлений</returns>
        private bool AutoUpdateCheck()
        {
            UpdateManager UpMan = new UpdateManager(App.FullAppPath, App.UserAgent);
            return UpMan.CheckAppUpdate();
        }

        /// <summary>
        /// Открывает конфиг, имя которого передано в качестве параметра
        /// и заполняет им Редактор конфигов с одноимённой страницы.
        /// </summary>
        /// <param name="ConfFileName">Полный путь к файлу конфига</param>
        private void ReadConfigFromFile(string ConfFileName)
        {
            // Описываем буферные переменные...
            string ImpStr, CVarName, CVarContent;

            // Проверяем, существует ли файл...
            if (File.Exists(ConfFileName))
            {
                // Получаем имя открытого в Редакторе файла без пути...
                CFGFileName = ConfFileName;

                // Проверяем, не открыл ли пользователь файл config.cfg и, если да, то сообщаем об этом...
                if (Path.GetFileName(CFGFileName) == "config.cfg") { MessageBox.Show(AppStrings.CE_RestConfigOpenWarn, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                // Очищаем область редактирования...
                CE_Editor.Rows.Clear();

                // Загружаем содержимое конфига из файла...
                try
                {
                    // Открываем поток с нужным нам файлом...
                    using (StreamReader ConfigFile = new StreamReader(ConfFileName, Encoding.Default))
                    {
                        // Читаем файл в потоковом режиме от начала и до конца...
                        while (ConfigFile.Peek() >= 0)
                        {
                            // Почистим строку от лишних пробелов и табуляций...
                            ImpStr = CoreLib.CleanStrWx(ConfigFile.ReadLine());

                            // Проверяем, не пустая ли строка...
                            if (!(String.IsNullOrEmpty(ImpStr)))
                            {
                                // Проверяем, не комментарий ли...
                                if (ImpStr[0] != '/')
                                {
                                    // Строка почищена, продолжаем...
                                    if (ImpStr.IndexOf(" ") != -1)
                                    {
                                        // Выделяем переменную...
                                        CVarName = ImpStr.Substring(0, ImpStr.IndexOf(" "));
                                        ImpStr = ImpStr.Remove(0, ImpStr.IndexOf(" ") + 1);

                                        // Выделяем значение...
                                        CVarContent = ImpStr.IndexOf("//") >= 1 ? ImpStr.Substring(0, ImpStr.IndexOf("//") - 1) : ImpStr;

                                        // Вставляем в таблицу...
                                        CE_Editor.Rows.Add(CVarName, CVarContent);
                                    }
                                    else
                                    {
                                        CE_Editor.Rows.Add(ImpStr, String.Empty);
                                    }
                                }
                            }
                        }
                    }

                    // Изменяем содержимое строки статуса...
                    UpdateStatusBar();
                }
                catch (Exception Ex)
                {
                    // Произошло исключение...
                    MessageBox.Show(AppStrings.CE_ExceptionDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, AppStrings.AppDbgExCfgEdLoad);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.CE_OpenFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Генерирует удобочитаемое название для файла резервной копии.
        /// </summary>
        /// <param name="FileName">Ссылка на файл резервной копии</param>
        /// <returns>Возвращает пару "тип архива" и "удобочитаемое название"</returns>
        private Tuple<string, string> GenUserFriendlyBackupDesc(FileInfo FileName)
        {
            string ConfRow, ConfType = String.Empty;
            switch (FileName.Name.Substring(0, FileName.Name.LastIndexOf('_')))
            {
                case "Container":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_Bud, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Cont;
                    break;
                case "Config":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_Config, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Cfg;
                    break;
                case "VoiceBan":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_VChat, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_DB;
                    break;
                case "VideoCfg":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_GRGame, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Video;
                    break;
                case "VideoAutoCfg":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_GameAuto, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Video;
                    break;
                case "Game_Options":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_GRGame, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Reg;
                    break;
                case "Source_Options":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_SRCAll, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Reg;
                    break;
                case "Steam_BackUp":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_SteamAll, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Reg;
                    break;
                case "Game_AutoBackUp":
                    ConfRow = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_GameAuto, FileName.CreationTime);
                    ConfType = AppStrings.BU_BType_Reg;
                    break;
                default:
                    ConfRow = Path.GetFileNameWithoutExtension(FileName.Name);
                    ConfType = AppStrings.BU_BType_Unkn;
                    break;
            }
            return Tuple.Create(ConfType, ConfRow);
        }

        /// <summary>
        /// Считывает файлы резервных копий из указанного каталога и помещает в таблицу.
        /// </summary>
        private void ReadBackUpList2Table()
        {
            // Очистим таблицу...
            Invoke((MethodInvoker)delegate () { BU_LVTable.Items.Clear(); });

            // Открываем каталог...
            DirectoryInfo DInfo = new DirectoryInfo(App.SourceGames.SelectedGame.FullBackUpDirPath);

            // Считываем список файлов по заданной маске...
            FileInfo[] DirList = DInfo.GetFiles("*.*");

            // Начинаем обход массива...
            foreach (FileInfo DItem in DirList)
            {
                // Обрабатываем найденное...
                var Rs = GenUserFriendlyBackupDesc(DItem);

                // Добавляем в таблицу...
                ListViewItem LvItem = new ListViewItem(Rs.Item2);
                if (Properties.Settings.Default.HighlightOldBackUps) { if (DateTime.UtcNow - DItem.CreationTimeUtc > TimeSpan.FromDays(30)) { LvItem.BackColor = Color.LightYellow; } }
                LvItem.SubItems.Add(Rs.Item1);
                LvItem.SubItems.Add(GuiHelpers.SclBytes(DItem.Length));
                LvItem.SubItems.Add(DItem.CreationTime.ToString());
                LvItem.SubItems.Add(DItem.Name);
                Invoke((MethodInvoker)delegate () { BU_LVTable.Items.Add(LvItem); });
            }
        }

        /// <summary>
        /// Обнуляет контролы на странице графических настроек для Type 1 игры.
        /// </summary>
        private void NullType1Settings()
        {
            GT_ResHor.Value = 640;
            GT_ResVert.Value = 640;
            GT_ScreenType.SelectedIndex = -1;
            GT_ModelQuality.SelectedIndex = -1;
            GT_TextureQuality.SelectedIndex = -1;
            GT_ShaderQuality.SelectedIndex = -1;
            GT_WaterQuality.SelectedIndex = -1;
            GT_ShadowQuality.SelectedIndex = -1;
            GT_ColorCorrectionT.SelectedIndex = -1;
            GT_AntiAliasing.SelectedIndex = -1;
            GT_Filtering.SelectedIndex = -1;
            GT_VSync.SelectedIndex = -1;
            GT_MotionBlur.SelectedIndex = -1;
            GT_DxMode.SelectedIndex = -1;
            GT_HDR.SelectedIndex = -1;
        }

        /// <summary>
        /// Обнуляет контролы на странице графических настроек для Type 2 игры.
        /// </summary>
        private void NullType2Settings()
        {
            GT_NCF_HorRes.Value = 640;
            GT_NCF_VertRes.Value = 480;
            GT_NCF_Brightness.SelectedIndex = -1;
            GT_NCF_Shadows.SelectedIndex = -1;
            GT_NCF_MBlur.SelectedIndex = -1;
            GT_NCF_Ratio.SelectedIndex = -1;
            GT_NCF_DispMode.SelectedIndex = -1;
            GT_NCF_AntiAlias.SelectedIndex = -1;
            GT_NCF_Filtering.SelectedIndex = -1;
            GT_NCF_VSync.SelectedIndex = -1;
            GT_NCF_Multicore.SelectedIndex = -1;
            GT_NCF_ShaderE.SelectedIndex = -1;
            GT_NCF_EffectD.SelectedIndex = -1;
            GT_NCF_MemPool.SelectedIndex = -1;
            GT_NCF_Quality.SelectedIndex = -1;
        }

        /// <summary>
        /// Обнуляет контролы на странице графических настроек. Функция-заглушка.
        /// </summary>
        private void NullGraphSettings()
        {
            switch (App.SourceGames.SelectedGame.SourceType)
            {
                case "1":
                    if (App.Platform.OS == CurrentPlatform.OSType.Windows) { NullType1Settings(); } else { NullType2Settings(); }
                    break;
                case "2":
                    NullType2Settings();
                    break;
            }
        }

        /// <summary>
        /// Переключает состояние некоторых элементов управления на форме.
        /// </summary>
        private void HandleControlsOnSelGame()
        {
            // Включаем основные элементы управления (контролы)...
            MainTabControl.Enabled = true;

            // Очистим список FPS-конфигов и HUD-ов...
            FP_ConfigSel.Items.Clear();
            HD_HSel.Items.Clear();

            // Отключим кнопку редактирования FPS-конфигов...
            FP_OpenNotepad.Enabled = false;

            // Отключим контролы модуля управления FPS-конфигами...
            FP_Install.Enabled = false;
            FP_Comp.Visible = false;

            // Отключим контролы в менеджере HUD...
            HD_Install.Enabled = false;
            HD_Homepage.Enabled = false;
            HD_Uninstall.Enabled = false;
            HD_OpenDir.Enabled = false;
            HD_Warning.Visible = false;
            HD_GB_Pbx.Image = null;
            HD_LastUpdate.Visible = false;

            // Включаем заблокированные ранее контролы...
            MNUInstaller.Enabled = true;
        }

        /// <summary>
        /// Загружает настройки видео для выбранной игры.
        /// </summary>
        private void LoadGraphicSettings()
        {
            // Обнуляем графические настройки...
            NullGraphSettings();

            // Загружаем настройки графики согласно указанного движка...
            switch (App.SourceGames.SelectedGame.SourceType)
            {
                case "1": /* Source 1, Type 1 (ex. GCF). */
                    if (App.Platform.OS == CurrentPlatform.OSType.Windows) { ReadType1VideoSettings(); } else { ReadType2VideoSettings(); }
                    break;
                case "2": /* Source 1, Type 2 (ex. NCF). */
                    ReadType2VideoSettings();
                    break;
                default:
                    throw new NotSupportedException();
            }

            // Переключаем графический твикер в режим GCF/NCF...
            SelectGraphicWidget((App.Platform.OS != CurrentPlatform.OSType.Windows) && (App.SourceGames.SelectedGame.SourceType == "1") ? "2" : App.SourceGames.SelectedGame.SourceType);
        }

        /// <summary>
        /// Выполняет особые действия и начинает процесс сохранения настроек видео
        /// для Type 1 игры.
        /// </summary>
        private void PrepareWriteType1VideoSettings()
        {
            // Генерируем путь к ветке реестра с настройками...
            string GameRegKey = Type1Video.GetGameRegKey(App.SourceGames.SelectedGame.SmallAppName);

            // Создаём резервную копию если включена опция безопасной очистки...
            if (Properties.Settings.Default.SafeCleanup)
            {
                try
                {
                    Type1Video.BackUpVideoSettings(GameRegKey, "Game_AutoBackUp", App.SourceGames.SelectedGame.FullBackUpDirPath);
                }
                catch (Exception Ex) { Logger.Warn(Ex); }
            }

            // Запускаем процесс...
            try
            {
                // Проверяем существование ключа реестра и если его нет, создаём...
                if (!(Type1Video.CheckRegKeyExists(GameRegKey))) { Type1Video.CreateRegKey(GameRegKey); }

                // Записываем настройки в реестр...
                WriteType1VideoSettings();

                // Выводим сообщение об успехе...
                MessageBox.Show(AppStrings.GT_SaveSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex)
            {
                // Записываем в журнал и выводим сообщение об ошибке...
                MessageBox.Show(AppStrings.GT_SaveFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, AppStrings.AppDbgExT1SaveFail);
            }
        }

        /// <summary>
        /// Выполняет особые действия и начинает процесс сохранения настроек видео
        /// для Type 2 игры.
        /// </summary>
        private void PrepareWriteType2VideoSettings()
        {
            // Создаём резервную копию если включена опция безопасной очистки...
            if (Properties.Settings.Default.SafeCleanup)
            {
                try
                {
                    FileManager.CreateConfigBackUp(App.SourceGames.SelectedGame.VideoCfgFiles, App.SourceGames.SelectedGame.FullBackUpDirPath, Properties.Resources.BU_PrefixVidAuto);
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, AppStrings.AppDbgExT2AutoFail);
                }
            }

            // Запускаем процесс...
            try
            {
                // Записываем настройки в файл...
                WriteType2VideoSettings();

                // Выводим сообщение об успехе...
                MessageBox.Show(AppStrings.GT_SaveSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex)
            {
                // Записываем в журнал и выводим сообщение об ошибке...
                MessageBox.Show(AppStrings.GT_NCFFailure, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, AppStrings.AppDbgExT2SaveFail);
            }
        }

        /// <summary>
        /// Сохраняет настройки видео для выбранной игры.
        /// </summary>
        private void WriteGraphicSettings()
        {
            // Определим тип игры...
            switch (App.SourceGames.SelectedGame.SourceType)
            {
                case "1":
                    if (App.Platform.OS == CurrentPlatform.OSType.Windows) { PrepareWriteType1VideoSettings(); } else { PrepareWriteType2VideoSettings(); }
                    break;
                case "2":
                    PrepareWriteType2VideoSettings();
                    break;
            }
        }

        /// <summary>
        /// Переключает вид страницы графического твикера с в соответствие с выбранным
        /// движком.
        /// </summary>
        /// <param name="SType">Тип движка Source</param>
        private void SelectGraphicWidget(string SType)
        {
            // Переключаем виджеты...
            switch (SType)
            {
                case "1":
                    GT_GType1.Visible = true;
                    GT_GType2.Visible = false;
                    break;
                case "2":
                    GT_GType1.Visible = false;
                    GT_GType2.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// Изменяет вид значка и текст безопасной очистки в соответствии с её статусом
        /// в строке состояния программы.
        /// </summary>
        private void CheckSafeClnStatus()
        {
            if (Properties.Settings.Default.SafeCleanup)
            {
                SB_App.Text = AppStrings.AppSafeClnStTextOn;
                SB_App.Image = Properties.Resources.green_circle;
            }
            else
            {
                SB_App.Text = AppStrings.AppSafeClnStTextOff;
                SB_App.Image = Properties.Resources.red_circle;
            }
        }

        /// <summary>
        /// Требует указать путь к Steam вручную при невозможности автоопределения.
        /// </summary>
        private string GetPathByMEnter()
        {
            // Задаём начальное значение...
            string Result = String.Empty;

            // Указываем текст в диалоге поиска каталога...
            FldrBrwse.Description = AppStrings.SteamPathEnterText;

            // Отображаем стандартный диалог поиска каталога...
            if (FldrBrwse.ShowDialog() == DialogResult.OK) { if (!(File.Exists(Path.Combine(FldrBrwse.SelectedPath, App.Platform.SteamBinaryName)))) { throw new FileNotFoundException("Invalid Steam directory entered by user", Path.Combine(FldrBrwse.SelectedPath, App.Platform.SteamBinaryName)); } else { Result = FldrBrwse.SelectedPath; } } else { throw new OperationCanceledException("User closed opendir window"); }

            // Возвращаем результат...
            return Result;
        }

        /// <summary>
        /// Проверяет значение OldPath на наличие верного пути к клиенту Steam.
        /// </summary>
        /// <param name="OldPath">Проверяемый путь</param>
        private string CheckLastSteamPath(string OldPath)
        {
            return (!(String.IsNullOrWhiteSpace(OldPath)) && File.Exists(Path.Combine(OldPath, App.Platform.SteamBinaryName))) ? OldPath : GetPathByMEnter();
        }

        /// <summary>
        /// Получает путь и обрабатывает возможные исключения.
        /// </summary>
        private void ValidateAndHandle()
        {
            try
            {
                App.SteamClient.FullSteamPath = CheckLastSteamPath(Properties.Settings.Default.LastSteamPath);
            }
            catch (FileNotFoundException Ex)
            {
                MessageBox.Show(AppStrings.SteamPathEnterErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(Ex, AppStrings.AppDbgExSteamPath);
                Environment.Exit(7);
            }
            catch (OperationCanceledException Ex)
            {
                MessageBox.Show(AppStrings.SteamPathCancel, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(Ex, AppStrings.AppDbgExSteamPath);
                Environment.Exit(19);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.AppGenericError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(Ex, AppStrings.AppDbgExSteamPath);
                Environment.Exit(24);
            }
        }

        /// <summary>
        /// Устанавливает статус элементам управления, зависящим от платформы или спец. прав.
        /// </summary>
        /// <param name="State">Устанавливаемый статус</param>
        private void ChangePrvControlState(bool State)
        {
            // Проверяем платформу выполнения приложения...
            if (App.Platform.OS == CurrentPlatform.OSType.Windows)
            {
                // Платформа Windows, применяем стандартные ограничения...
                MNUWinMnuDisabler.Enabled = State;
            }
            else
            {
                // Платформа GNU/Linux или MacOS X, отключим ряд контролов...
                MNUReportBuilder.Enabled = false;
                MNUWinMnuDisabler.Enabled = false;
            }
        }

        /// <summary>
        /// Выполняет определение и вывод названия файловой системы на диске установки клиента игры.
        /// </summary>
        private void DetectFS()
        {
            try
            {
                PS_OSDrive.Text = String.Format(PS_OSDrive.Text, FileManager.DetectDriveFileSystem(Path.GetPathRoot(App.SourceGames.SelectedGame.FullGamePath)));
            }
            catch (Exception Ex)
            {
                PS_OSDrive.Text = String.Format(PS_OSDrive.Text, "Unknown");
                Logger.Warn(Ex);
            }
        }

        /// <summary>
        /// Проверяет количество найденных установленных игр и выполняет нужные действия.
        /// </summary>
        /// <param name="LoginCount">Количество найденных игр</param>
        private void CheckGames(int GamesCount)
        {
            switch (GamesCount)
            {
                case 0:
                    {
                        // Запишем в лог...
                        Logger.Warn(String.Format(AppStrings.AppNoGamesDLog, App.SteamClient.FullSteamPath));
                        // Нет, не нашлись, выведем сообщение...
                        MessageBox.Show(AppStrings.AppNoGamesDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Завершим работу приложения...
                        Environment.Exit(11);
                    }
                    break;
                case 1:
                    {
                        // При наличии единственной игры в списке, выберем её автоматически...
                        AppSelector.SelectedIndex = 0;
                        UpdateStatusBar();
                    }
                    break;
                default:
                    {
                        // Выберем последнюю использованную игру...
                        int Ai = AppSelector.Items.IndexOf(Properties.Settings.Default.LastGameName);
                        AppSelector.SelectedIndex = Ai != -1 ? Ai : 0;
                    }
                    break;
            }
        }

        /// <summary>
        /// Запускает проверку на наличие запрещённых символов в пути установки клиента Steam.
        /// </summary>
        private void CheckSymbolsSteam()
        {
            if (!(FileManager.CheckNonASCII(App.SteamClient.FullSteamPath)))
            {
                PS_PathSteam.ForeColor = Color.Red;
                PS_PathSteam.Image = Properties.Resources.upd_err;
                Logger.Warn(String.Format(AppStrings.AppRestrSymbLog, App.SteamClient.FullSteamPath));
            }
        }

        /// <summary>
        /// Запускает проверку на наличие запрещённых символов в пути установки игры.
        /// </summary>
        private void CheckSymbolsGame()
        {
            if (!(FileManager.CheckNonASCII(App.SourceGames.SelectedGame.FullGamePath)))
            {
                PS_PathGame.ForeColor = Color.Red;
                PS_PathGame.Image = Properties.Resources.upd_err;
                Logger.Warn(String.Format(AppStrings.AppRestrSymbLog, App.SourceGames.SelectedGame.FullGamePath));
            }
            else
            {
                PS_PathGame.ForeColor = Color.Green;
                PS_PathGame.Image = Properties.Resources.upd_nx;
            }
        }

        /// <summary>
        /// Управляет выводом значка активного FPS-конфига и кнопки их удаления.
        /// </summary>
        private void HandleConfigs()
        {
            App.SourceGames.SelectedGame.FPSConfigs = FileManager.ExpandFileList(ConfigManager.ListFPSConfigs(App.SourceGames.SelectedGame.FullGamePath, App.SourceGames.SelectedGame.IsUsingUserDir), true);
            GT_Warning.Visible = App.SourceGames.SelectedGame.FPSConfigs.Count > 0;
            FP_Uninstall.Enabled = App.SourceGames.SelectedGame.FPSConfigs.Count > 0;
        }

        /// <summary>
        /// Управляет выводом текущего SteamID.
        /// </summary>
        /// <param name="SID">Сохранённый SteamID</param>
        private void HandleSteamIDs(string SID)
        {
            try
            {
                string Result = App.SteamClient.GetCurrentSteamID(SID);
                SB_SteamID.Text = Result;
                Properties.Settings.Default.LastSteamID = Result;
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
                SB_SteamID.Text = String.Empty;
            }
        }

        /// <summary>
        /// Управляет видимостью специальных кнопок модуля Менеджер HUD.
        /// </summary>
        /// <param name="State">Статус выбранного HUD</param>
        private void SetHUDButtons(bool State)
        {
            HD_Install.Text = State ? AppStrings.HD_BtnUpdateText : AppStrings.HD_BtnInstallText;
            HD_Uninstall.Enabled = State;
            HD_OpenDir.Enabled = State;
        }

        /// <summary>
        /// Обновляет содержимое строки состояния в зависимости от контекста.
        /// </summary>
        private void UpdateStatusBar()
        {
            switch (MainTabControl.SelectedIndex)
            {
                case 1: // Открыта страница "Редактор конфигов"...
                    {
                        MNUShowEdHint.Enabled = true;
                        SB_Status.ForeColor = Color.Black;
                        SB_Status.Text = String.Format(AppStrings.StatusOpenedFile, String.IsNullOrEmpty(CFGFileName) ? AppStrings.UnnamedFileName : Path.GetFileName(CFGFileName));
                    }
                    break;
                case 4:
                    {
                        bool HUDDbStatus = HUDManager.CheckHUDDatabase(Properties.Settings.Default.LastHUDTime);
                        MNUShowEdHint.Enabled = false;
                        SB_Status.ForeColor = HUDDbStatus ? Color.Red : Color.Black;
                        SB_Status.Text = String.Format(AppStrings.HD_DynBarText, HUDDbStatus ? AppStrings.HD_StatusOutdated : AppStrings.HD_StatusUpdated, Properties.Settings.Default.LastHUDTime);
                    }
                    break;
                default: // Открыта другая страница...
                    {
                        MNUShowEdHint.Enabled = false;
                        SB_Status.ForeColor = Color.Black;
                        SB_Status.Text = AppStrings.StatusNormal;
                    }
                    break;
            }
        }

        /// <summary>
        /// Проверяет правильность заполнения графических настроек для Type 1 игр.
        /// </summary>
        private bool CheckType1Settings()
        {
            return (GT_ScreenType.SelectedIndex != -1) && (GT_ModelQuality.SelectedIndex != -1)
                && (GT_TextureQuality.SelectedIndex != -1) && (GT_ShaderQuality.SelectedIndex != -1)
                && (GT_WaterQuality.SelectedIndex != -1) && (GT_ShadowQuality.SelectedIndex != -1)
                && (GT_ColorCorrectionT.SelectedIndex != -1) && (GT_AntiAliasing.SelectedIndex != -1)
                && (GT_Filtering.SelectedIndex != -1) && (GT_VSync.SelectedIndex != -1)
                && (GT_MotionBlur.SelectedIndex != -1) && (GT_DxMode.SelectedIndex != -1)
                && (GT_HDR.SelectedIndex != -1);
        }

        /// <summary>
        /// Проверяет правильность заполнения графических настроек для Type 2 игр.
        /// </summary>
        private bool CheckType2Settings()
        {
            return (GT_NCF_Quality.SelectedIndex != -1) && (GT_NCF_MemPool.SelectedIndex != -1)
                && (GT_NCF_EffectD.SelectedIndex != -1) && (GT_NCF_ShaderE.SelectedIndex != -1)
                && (GT_NCF_Multicore.SelectedIndex != -1) && (GT_NCF_VSync.SelectedIndex != -1)
                && (GT_NCF_Filtering.SelectedIndex != -1) && (GT_NCF_AntiAlias.SelectedIndex != -1)
                && (GT_NCF_DispMode.SelectedIndex != -1) && (GT_NCF_Ratio.SelectedIndex != -1)
                && (GT_NCF_Brightness.SelectedIndex != -1) && (GT_NCF_Shadows.SelectedIndex != -1)
                && (GT_NCF_MBlur.SelectedIndex != -1);
        }

        /// <summary>
        /// Проверяет верность заполнения графических настроек
        /// </summary>
        private bool ValidateGameSettings()
        {
            bool Result = false;
            switch (App.SourceGames.SelectedGame.SourceType)
            {
                case "1":
                    Result = App.Platform.OS == CurrentPlatform.OSType.Windows ? CheckType1Settings() : CheckType2Settings();
                    break;
                case "2":
                    Result = CheckType2Settings();
                    break;
            }
            return Result;
        }

        /// <summary>
        /// Закрывает все открытые в Редакторе конфигов файлы и очищает таблицу...
        /// </summary>
        private void CloseEditorConfigs()
        {
            CFGFileName = String.Empty;
            CE_Editor.Rows.Clear();
        }

        /// <summary>
        /// Получает список резеервных копий и заносит их в таблицу...
        /// </summary>
        private void UpdateBackUpList()
        {
            try
            {
                // Создадим каталог для хранения резервных копий если его ещё нет...
                if (!Directory.Exists(App.SourceGames.SelectedGame.FullBackUpDirPath)) { Directory.CreateDirectory(App.SourceGames.SelectedGame.FullBackUpDirPath); }

                // Считываем и выводим в таблицу файлы резервных копий...
                ReadBackUpList2Table();
            }
            catch (Exception Ex)
            {
                // Произошло исключение. Запишем в журнал...
                Logger.Warn(Ex);
            }
        }

        /// <summary>
        /// Ищет установленные игры и выполняет ряд необходимых проверок.
        /// </summary>
        private void FindGames()
        {
            // Начинаем определять установленные игры...
            try
            {
                DetectInstalledGames();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.AppXMLParseError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(Ex, AppStrings.AppDbgExXmlParse);
                Environment.Exit(16);
            }

            // Проверим нашлись ли игры...
            CheckGames(App.SourceGames.InstalledGames.Count);
        }

        /// <summary>
        /// Переключает вид страницы модуля Менеджер HUD.
        /// </summary>
        /// <param name="Mode">Булево режима</param>
        private void HandleHUDMode(bool Mode)
        {
            HUD_Panel.Visible = Mode;
            HUD_NotAvailable.Visible = !Mode;
        }

        /// <summary>
        /// Запускает проверку наличия обновлений программы.
        /// </summary>
        private void CheckForUpdates()
        {
            // Запускаем проверку обновлений если это разрешено в настройках...
            if (Properties.Settings.Default.AutoUpdateCheck) { if (!BW_UpChk.IsBusy) { BW_UpChk.RunWorkerAsync(); } }
        }

        /// <summary>
        /// Генерирует ссылку онлайновой справочной системы на основе информации
        /// о текущей вкладке.
        /// </summary>
        /// <param name="TabIndex">Индекс текущей вкладки</param>
        /// <returns>Возвращает URL, пригодный для загрузки в веб-браузере</returns>
        private string GetHelpWebPage(int TabIndex)
        {
            // Создаём буферную переменную...
            string Result = "";

            // Генерируем ID для справочной системы сайта...
            switch (TabIndex)
            {
                case 0: /* графический твикер. */
                    Result = "graphic-tweaker";
                    break;
                case 1: /* Редактор конфигов. */
                    Result = "config-editor";
                    break;
                case 2: /* Устранение проблем и очистка. */
                    Result = "cleanup";
                    break;
                case 3: /* FPS-конфиги. */
                    Result = "fps-configs";
                    break;
                case 4: /* Менеджер HUD. */
                    Result = "hud-manager";
                    break;
                case 5: /* Резервные копии. */
                    Result = "backups";
                    break;
            }

            // Возвращаем финальный URL...
            return String.Format("{0}.html", Result);
        }


        /// <summary>
        /// Возвращает список строк для передачи в особые методы.
        /// </summary>
        /// <param name="Str">Строка для создания списка</param>
        /// <returns>Возвращает список строк</returns>
        private List<String> SingleToArray(string Str)
        {
            List<String> Result = new List<String> { Str };
            return Result;
        }

        /// <summary>
        /// Возвращает описание переданной в качестве параметра переменной, получая
        /// эту информацию из ресурса CVList с учётом локализации.
        /// </summary>
        /// <param name="CVar">Название переменной</param>
        /// <returns>Описание переменной с учётом локализации</returns>
        public string GetConVarDescription(string CVar)
        {
            ResourceManager DM = new ResourceManager(Properties.Resources.CE_CVResDf, typeof(FrmMainW).Assembly);
            return DM.GetString(CVar);
        }

        #endregion

        #region Internal Workers

        private void BW_UpChk_DoWork(object sender, DoWorkEventArgs e)
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

        private void BW_UpChk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Произошла ошибка во время проверки наличия обновлений. Запишем в журнал...
            if (e.Error != null)
            {
                Logger.Warn(e.Error, AppStrings.AppDbgExBgWChk);
            }
        }

        private void BW_FPRecv_DoWork(object sender, DoWorkEventArgs e)
        {
            // Получаем список установленных конфигов из БД...
            App.SourceGames.SelectedGame.CFGMan = new ConfigManager(Path.Combine(App.FullAppPath, StringsManager.ConfigDatabaseName), AppStrings.AppLangPrefix);

            // Выведем установленные в форму...
            foreach (string Str in App.SourceGames.SelectedGame.CFGMan.GetAllCfg())
            {
                Invoke((MethodInvoker)delegate () { FP_ConfigSel.Items.Add(Str); });
            }
        }

        private void BW_FPRecv_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // FPS-конфигов не найдено. Запишем в лог...
                Logger.Warn(e.Error);

                // Выводим текст об этом...
                FP_Description.Text = AppStrings.FP_NoCfgGame;
                FP_Description.ForeColor = Color.Red;

                // ...и блокируем контролы, отвечающие за установку...
                FP_Install.Enabled = false;
                FP_ConfigSel.Enabled = false;
                FP_OpenNotepad.Enabled = false;
            }
            else
            {
                // Проверяем, нашлись ли конфиги...
                if (FP_ConfigSel.Items.Count >= 1)
                {
                    FP_Description.Text = AppStrings.FP_SelectFromList;
                    FP_Description.ForeColor = Color.Black;
                    FP_ConfigSel.Enabled = true;
                }
            }
        }

        private void BW_BkUpRecv_DoWork(object sender, DoWorkEventArgs e)
        {
            // Получаем список резеверных копий...
            UpdateBackUpList();
        }

        private void BW_HUDList_DoWork(object sender, DoWorkEventArgs e)
        {
            // Получаем список доступных HUD...
            App.SourceGames.SelectedGame.HUDMan = new HUDManager(Path.Combine(App.FullAppPath, StringsManager.HudDatabaseName), App.SourceGames.SelectedGame.AppHUDDir, Properties.Settings.Default.HUDHideOutdated);

            // Вносим HUD текущей игры в форму...
            Invoke((MethodInvoker)delegate () { HD_HSel.Items.AddRange(App.SourceGames.SelectedGame.HUDMan.GetHUDNames(App.SourceGames.SelectedGame.SmallAppName).ToArray<object>()); });
        }


        private void BW_HUDList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.Warn(e.Error);
            }
        }

        private void BW_HUDScreen_DoWork(object sender, DoWorkEventArgs e)
        {
            // Сгенерируем путь к файлу со скриншотом...
            string ScreenFile = Path.Combine(App.SourceGames.SelectedGame.AppHUDDir, Path.GetFileName(App.SourceGames.SelectedGame.HUDMan.SelectedHUD.Preview));

            // Загрузим файл если не существует...
            if (!File.Exists(ScreenFile))
            {
                using (WebClient Downloader = new WebClient())
                {
                    Downloader.Headers.Add("User-Agent", App.UserAgent);
                    Downloader.DownloadFile(App.SourceGames.SelectedGame.HUDMan.SelectedHUD.Preview, ScreenFile);
                }
            }

            // Установим...
            Invoke((MethodInvoker)delegate () { HD_GB_Pbx.Image = Image.FromFile(ScreenFile); });
        }

        private void BW_HUDScreen_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string ScreenFile = Path.Combine(App.SourceGames.SelectedGame.AppHUDDir, Path.GetFileName(App.SourceGames.SelectedGame.HUDMan.SelectedHUD.Preview));

            if (e.Error != null)
            {
                Logger.Warn(e.Error);
                if (File.Exists(ScreenFile)) { File.Delete(ScreenFile); }
            }
        }

        private void BW_HudInstall_DoWork(object sender, DoWorkEventArgs e)
        {
            // Сохраняем предыдующий текст кнопки...
            string CaptText = HD_Install.Text;
            string InstallTmp = Path.Combine(App.SourceGames.SelectedGame.CustomInstallDir, "hudtemp");

            try
            {
                // Изменяем текст на "Идёт установка" и отключаем её...
                Invoke((MethodInvoker)delegate () { HD_Install.Text = AppStrings.HD_InstallBtnProgress; HD_Install.Enabled = false; });

                // Устанавливаем и очищаем временный каталог...
                try { Directory.Move(Path.Combine(InstallTmp, HUDManager.FormatIntDir(App.SourceGames.SelectedGame.HUDMan.SelectedHUD.ArchiveDir)), Path.Combine(App.SourceGames.SelectedGame.CustomInstallDir, App.SourceGames.SelectedGame.HUDMan.SelectedHUD.InstallDir)); }
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
            if (e.Error == null)
            {
                MessageBox.Show(AppStrings.HD_InstallSuccessfull, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(AppStrings.HD_InstallError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(e.Error, AppStrings.AppDbgExHUDInstall);
            }

            // Включаем кнопку удаления если HUD установлен...
            SetHUDButtons(HUDManager.CheckInstalledHUD(App.SourceGames.SelectedGame.CustomInstallDir, App.SourceGames.SelectedGame.HUDMan.SelectedHUD.InstallDir));
        }

        #endregion

        private void FrmMainW_Load(object sender, EventArgs e)
        {
            // Событие инициализации формы...
            App = new CurrentApp(Properties.Settings.Default.IsPortable, Properties.Resources.AppName);

            // Узнаем путь к установленному клиенту Steam...
            try
            {
                App.SteamClient = new SteamManager(Properties.Settings.Default.LastSteamID, App.Platform.OS);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show(AppStrings.AppNoSteamIDSetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(2);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, AppStrings.AppDbgExStmmInit);
                ValidateAndHandle();
            }
            
            // Начинаем платформо-зависимые процедуры...
            ChangePrvControlState(ProcessManager.IsCurrentUserAdmin());

            // Сохраним последний путь к Steam в файл конфигурации...
            Properties.Settings.Default.LastSteamPath = App.SteamClient.FullSteamPath;

            // Вставляем информацию о версии в заголовок формы...
            Text = String.Format(Text, Properties.Resources.AppName, App.Platform.OSFriendlyName, CurrentApp.AppVersion);

            // Укажем статус Безопасной очистки...
            CheckSafeClnStatus();

            // Укажем путь к Steam на странице "Устранение проблем"...
            PS_StPath.Text = String.Format(PS_StPath.Text, App.SteamClient.FullSteamPath);
            
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
                    switch (App.SteamClient.GetSteamLanguage())
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
                    Logger.Warn(Ex);
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
                            App.SteamClient.CleanBlobsNow();
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show(AppStrings.PS_CleanException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Logger.Error(Ex, AppStrings.AppDbgExClnBlobs);
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
                                App.SteamClient.CleanRegistryNow(PS_SteamLang.SelectedIndex);
                            }
                            else
                            {
                                // Пользователь не выбрал язык, поэтому будем использовать английский...
                                MessageBox.Show(AppStrings.PS_NoLangSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                App.SteamClient.CleanRegistryNow(0);
                            }
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show(AppStrings.PS_CleanException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Logger.Error(Ex, AppStrings.AppDbgExClnReg);
                        }
                    }

                    // Выполнение закончено, выведем сообщение о завершении...
                    MessageBox.Show(AppStrings.PS_SeqCompleted, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Запустим Steam...
                    if (File.Exists(Path.Combine(App.SteamClient.FullSteamPath, App.Platform.SteamBinaryName))) { Process.Start(Path.Combine(App.SteamClient.FullSteamPath, App.Platform.SteamBinaryName)); }
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
                App.SourceGames.Select(AppSelector.Text);

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
                HandleHUDMode(App.SourceGames.SelectedGame.IsHUDsAvailable);

                // Считаем список доступных HUD для данной игры...
                if (App.SourceGames.SelectedGame.IsHUDsAvailable) { if (!BW_HUDList.IsBusy) { BW_HUDList.RunWorkerAsync(); } }
                
                // Считаем список бэкапов...
                if (!BW_BkUpRecv.IsBusy) { BW_BkUpRecv.RunWorkerAsync(); }

                // Создадим каталоги кэшей для HUD...
                if (App.SourceGames.SelectedGame.IsHUDsAvailable && !Directory.Exists(App.SourceGames.SelectedGame.AppHUDDir)) { Directory.CreateDirectory(App.SourceGames.SelectedGame.AppHUDDir); }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.AppFailedToGetData, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, AppStrings.AppDbgExSelGame);
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
                switch (App.SourceGames.SelectedGame.SourceType)
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
                switch (App.SourceGames.SelectedGame.SourceType)
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
                try
                {
                    App.SourceGames.SelectedGame.CFGMan.Select(FP_ConfigSel.Text);
                }
                catch (Exception Ex) { Logger.Warn(Ex); }

                // Выводим описание...
                FP_Description.Text = App.SourceGames.SelectedGame.CFGMan.FPSConfig.Description;

                // Проверим совместимость конфига с игрой...
                FP_Comp.Visible = !App.SourceGames.SelectedGame.CFGMan.FPSConfig.CheckCompactibility(App.SourceGames.SelectedGame.GameInternalID);

                // Включаем кнопку открытия конфига в Блокноте...
                FP_OpenNotepad.Enabled = true;

                // Включаем кнопку установки конфига...
                FP_Install.Enabled = true;
            }
            catch (Exception Ex)
            {
                // Не получилось загрузить описание выбранного конфига. Выведем стандартное сообщение...
                Logger.Warn(Ex);
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
                        if (App.SourceGames.SelectedGame.FPSConfigs.Count > 0)
                        {
                            // Создаём резервную копию...
                            try
                            {
                                FileManager.CompressFiles(App.SourceGames.SelectedGame.FPSConfigs, FileManager.GenerateBackUpFileName(App.SourceGames.SelectedGame.FullBackUpDirPath, Properties.Resources.BU_PrefixCfg));
                            }
                            catch (Exception Ex)
                            {
                                Logger.Warn(Ex, AppStrings.AppDbgExFpsInstBackup);
                            }
                        }
                    }

                    try
                    {
                        // Устанавливаем...
                        ConfigManager.InstallConfigNow(App.SourceGames.SelectedGame.CFGMan.FPSConfig.FileName, App.FullAppPath, App.SourceGames.SelectedGame.FullGamePath, App.SourceGames.SelectedGame.IsUsingUserDir, Properties.Settings.Default.UserCustDirName);
                        
                        // Выводим сообщение об успешной установке...
                        MessageBox.Show(AppStrings.FP_InstallSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Перечитаем конфиги...
                        HandleConfigs();
                    }
                    catch (Exception Ex)
                    {
                        // Установка не удалась. Выводим сообщение об этом...
                        MessageBox.Show(AppStrings.FP_InstallFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Logger.Error(Ex, AppStrings.AppDbgExFpsInstall);
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
                if (App.SourceGames.SelectedGame.FPSConfigs.Count > 0)
                {
                    // Удаляем конфиги...
                    GuiHelpers.FormShowCleanup(App.SourceGames.SelectedGame.FPSConfigs, ((Button)sender).Text.ToLower(), AppStrings.FP_RemoveSuccessful, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile, false, false, false, Properties.Settings.Default.SafeCleanup);

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
                MessageBox.Show(AppStrings.FP_RemoveFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(Ex, AppStrings.AppDbgExFpsUninstall);
            }
        }

        private void GT_Warning_Click(object sender, EventArgs e)
        {
            try
            {
                // Предложим пользователю выбрать FPS-конфиг...
                string ConfigFile = GuiHelpers.FormShowCfgSelect(App.SourceGames.SelectedGame.FPSConfigs);

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
                MessageBox.Show(AppStrings.CS_FailedToOpenCfg, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, AppStrings.AppDbgExCfgSelection);
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
            CE_OpenCfgDialog.InitialDirectory = App.SourceGames.SelectedGame.FullCfgPath;

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
            CE_SaveCfgDialog.InitialDirectory = App.SourceGames.SelectedGame.FullCfgPath;

            // Проверяем, открыт ли какой-либо файл...
            if (!(String.IsNullOrEmpty(CFGFileName)))
            {
                // Будем бэкапить все файлы, сохраняемые в Редакторе...
                if (Properties.Settings.Default.SafeCleanup)
                {
                    // Создаём резервную копию...
                    if (File.Exists(CFGFileName))
                    {
                        try
                        {
                            FileManager.CreateConfigBackUp(CFGFileName, App.SourceGames.SelectedGame.FullBackUpDirPath, Properties.Resources.BU_PrefixCfg);
                        }
                        catch (Exception Ex)
                        {
                            Logger.Warn(Ex, AppStrings.AppDbgExCfgEdAutoBackup);
                        }
                    }
                }

                // Начинаем сохранение в тот же файл...
                try
                {
                    WriteTableToFileNow(CFGFileName);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.CE_CfgSVVEx, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, AppStrings.AppDbgExCfgEdSave);
                }
            }
            else
            {
                // Зададим стандартное имя (см. issue 21)...
                CE_SaveCfgDialog.FileName = File.Exists(Path.Combine(App.SourceGames.SelectedGame.FullCfgPath, "autoexec.cfg")) ? AppStrings.UnnamedFileName : "autoexec.cfg";

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
            CE_SaveCfgDialog.InitialDirectory = App.SourceGames.SelectedGame.FullCfgPath;

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
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "custom", "*.bsp"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "download", "*.bsp"),
                Path.Combine(App.SourceGames.SelectedGame.AppWorkshopDir, "*.bsp")
            };
            if (Properties.Settings.Default.AllowUnSafeCleanup) { CleanDirs.Add(Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "maps", "*.bsp")); }
            GuiHelpers.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile);
        }

        private void PS_RemDnlCache_Click(object sender, EventArgs e)
        {
            // Удаляем кэш загрузок...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "download", "*.*"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "downloads", "*.*"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "streams", "*.*")
            };
            GuiHelpers.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile);
        }

        private void PS_RemSoundCache_Click(object sender, EventArgs e)
        {
            // Удаляем звуковой кэш...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "maps", "graphs", "*.*"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "maps", "soundcache", "*.*"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "download", "sound", "*.*"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "*.cache")
            };
            GuiHelpers.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile);
        }

        private void PS_RemScreenShots_Click(object sender, EventArgs e)
        {
            // Удаляем все скриншоты...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "screenshots", "*.*")
            };
            GuiHelpers.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile, false, false, false);
        }

        private void PS_RemDemos_Click(object sender, EventArgs e)
        {
            // Удаляем все записанные демки...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "demos", "*.*"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "*.dem"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "*.mp4"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "*.tga"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "*.wav")
            };
            GuiHelpers.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile, false, false, false, false);
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
                    if (!App.SourceGames.SelectedGame.IsUsingVideoFile)
                    {
                        // Получаем полный путь к ветке реестра игры...
                        string GameRegKey = Type1Video.GetGameRegKey(App.SourceGames.SelectedGame.SmallAppName);

                        // Создаём резервную копию куста реестра...
                        if (Properties.Settings.Default.SafeCleanup)
                        {
                            try
                            {
                                Type1Video.BackUpVideoSettings(GameRegKey, "Game_AutoBackUp", App.SourceGames.SelectedGame.FullBackUpDirPath);
                            }
                            catch (Exception Ex) { Logger.Warn(Ex); }
                        }

                        // Удаляем ключ HKEY_CURRENT_USER\Software\Valve\Source\tf\Settings из реестра...
                        Type1Video.RemoveRegKey(GameRegKey);
                    }
                    else
                    {
                        // Создадим бэкап файла с графическими настройками...
                        if (Properties.Settings.Default.SafeCleanup)
                        {
                            try
                            {
                                FileManager.CreateConfigBackUp(App.SourceGames.SelectedGame.VideoCfgFiles, App.SourceGames.SelectedGame.FullBackUpDirPath, Properties.Resources.BU_PrefixVidAuto);
                            }
                            catch (Exception Ex)
                            {
                                Logger.Warn(Ex, AppStrings.AppDbgExRemVdAutoGs);
                            }
                        }

                        // Помечаем его на удаление...
                        CleanDirs.AddRange(App.SourceGames.SelectedGame.VideoCfgFiles);
                    }

                    // Создаём резервную копию...
                    if (Properties.Settings.Default.SafeCleanup)
                    {
                        try
                        {
                            FileManager.CreateConfigBackUp(App.SourceGames.SelectedGame.CloudConfigs, App.SourceGames.SelectedGame.FullBackUpDirPath, Properties.Resources.BU_PrefixCfg);
                        }
                        catch (Exception Ex)
                        {
                            Logger.Warn(Ex, AppStrings.AppDbgExRemVdAutoCfg);
                        }
                    }

                    // Помечаем конфиги игры на удаление...
                    CleanDirs.Add(Path.Combine(App.SourceGames.SelectedGame.FullCfgPath, "config.cfg"));
                    CleanDirs.AddRange(App.SourceGames.SelectedGame.CloudConfigs);

                    // Удаляем всю очередь...
                    GuiHelpers.FormShowRemoveFiles(CleanDirs);

                    // Выводим сообщение...
                    MessageBox.Show(AppStrings.PS_CleanupSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.PS_CleanupErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, AppStrings.AppDbgExRemVd);
                }
            }
        }

        private void PS_RemOldBin_Click(object sender, EventArgs e)
        {
            // Удаляем старые бинарники...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(App.SourceGames.SelectedGame.GamePath, Path.GetDirectoryName(App.SourceGames.SelectedGame.SmallAppName), "bin", "*.*"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "bin", "*.*"),
                Path.Combine(App.SourceGames.SelectedGame.GamePath, "*.exe")
            };
            if (Properties.Settings.Default.AllowUnSafeCleanup) { CleanDirs.Add(Path.Combine(App.SourceGames.SelectedGame.GamePath, "platform", "*.*")); }
            GuiHelpers.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CacheChkReq, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile);
        }

        private void PS_CheckCache_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format(AppStrings.AppQuestionTemplate, ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    Process.Start(String.Format("steam://validate/{0}", App.SourceGames.SelectedGame.GameInternalID));
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.AppStartSteamFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, AppStrings.AppDbgExValCache);
                }
            }
        }

        private void MNUReportBuilder_Click(object sender, EventArgs e)
        {
            if ((AppSelector.Items.Count > 0) && (AppSelector.SelectedIndex != -1))
            {
                // Запускаем форму создания отчёта для Техподдержки...
                GuiHelpers.FormShowRepBuilder(App.AppUserDir, App.SteamClient.FullSteamPath, App.SourceGames.SelectedGame);
            }
            else
            {
                MessageBox.Show(AppStrings.AppNoGamesSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUInstaller_Click(object sender, EventArgs e)
        {
            // Запускаем форму установщика спреев, демок и конфигов...
            GuiHelpers.FormShowInstaller(App.SourceGames.SelectedGame.FullGamePath, App.SourceGames.SelectedGame.IsUsingUserDir, App.SourceGames.SelectedGame.CustomInstallDir);
        }

        private void MNUExit_Click(object sender, EventArgs e)
        {
            // Завершаем работу программы...
            Environment.Exit(0);
        }

        private void MNUAbout_Click(object sender, EventArgs e)
        {
            // Отобразим форму "О программе"...
            GuiHelpers.FormShowAboutApp();
        }

        private void MNUReportBug_Click(object sender, EventArgs e)
        {
            // Перейдём в баг-трекер...
            try
            {
                ProcessManager.OpenWebPage(Properties.Resources.AppBtURL);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, AppStrings.AppDbgExBugRep);
            }
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
                                        Process.Start("regedit.exe", String.Format("/s \"{0}\"", Path.Combine(App.SourceGames.SelectedGame.FullBackUpDirPath, BU_Item.SubItems[4].Text)));
                                    }
                                    catch (Exception Ex)
                                    {
                                        // Произошло исключение...
                                        MessageBox.Show(AppStrings.BU_RestFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        Logger.Error(Ex, AppStrings.AppDbgExRegedit);
                                    }
                                    break;
                                case ".bud":
                                    // Распаковываем архив с выводом прогресса...
                                    GuiHelpers.FormShowArchiveExtract(Path.Combine(App.SourceGames.SelectedGame.FullBackUpDirPath, BU_Item.SubItems[4].Text), Path.GetPathRoot(App.SteamClient.FullSteamPath));

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
                                File.Delete(Path.Combine(App.SourceGames.SelectedGame.FullBackUpDirPath, BU_Item.SubItems[4].Text));

                                // Удаляем строку...
                                BU_LVTable.Items.Remove(BU_Item);
                            }
                            catch (Exception Ex)
                            {
                                // Произошло исключение при попытке удаления файла резервной копии...
                                MessageBox.Show(AppStrings.BU_DelFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                Logger.Error(Ex, AppStrings.AppDbgExBackupRem);
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
                    if (!App.SourceGames.SelectedGame.IsUsingVideoFile)
                    {
                        // Создаём конфиг ветки реестра...
                        Type1Video.BackUpVideoSettings(Type1Video.GetGameRegKey(App.SourceGames.SelectedGame.SmallAppName), "Game_Options", App.SourceGames.SelectedGame.FullBackUpDirPath);
                    }
                    else
                    {
                        // Проверяем существование файла с графическими настройками игры...
                        try
                        {
                            FileManager.CreateConfigBackUp(App.SourceGames.SelectedGame.VideoCfgFiles, App.SourceGames.SelectedGame.FullBackUpDirPath, Properties.Resources.BU_PrefixVideo);
                        }
                        catch (Exception Ex)
                        {
                            Logger.Warn(Ex, AppStrings.AppDbgExBkGsAuto);
                        }
                    }
                    
                    // Выводим сообщение об успехе...
                    MessageBox.Show(AppStrings.BU_RegDone, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Обновляем список резервных копий...
                    UpdateBackUpList();
                }
                catch (Exception Ex)
                {
                    // Выводим сообщение об ошибке и пишем в журнал отладки...
                    MessageBox.Show(AppStrings.BU_RegErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, AppStrings.AppDbgExBkSg);
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
                    Type1Video.CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve"), "Steam_BackUp", App.SourceGames.SelectedGame.FullBackUpDirPath);
                    
                    // Выводим сообщение...
                    MessageBox.Show(AppStrings.BU_RegDone, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Обновим список бэкапов...
                    UpdateBackUpList();
                }
                catch (Exception Ex)
                {
                    // Произошло исключение, уведомим пользователя...
                    MessageBox.Show(AppStrings.BU_RegErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, AppStrings.AppDbgExBkAllStm);
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
                    Type1Video.CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve", "Source"), "Source_Options", App.SourceGames.SelectedGame.FullBackUpDirPath);
                    MessageBox.Show(AppStrings.BU_RegDone, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateBackUpList();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.BU_RegErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, AppStrings.AppDbgExBkAllGames);
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
                if (!(String.IsNullOrEmpty(Buf))) { Buf = GetConVarDescription(Buf); if (!(String.IsNullOrEmpty(Buf))) { MessageBox.Show(Buf, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); } else { MessageBox.Show(AppStrings.CE_ClNoDescr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); } } else { MessageBox.Show(AppStrings.CE_ClSelErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            catch
            {
                MessageBox.Show(AppStrings.CE_ClSelErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUHelp_Click(object sender, EventArgs e)
        {
            // Сгенерируем путь к файлу CHM справочной системы...
            string CHMFile = Path.Combine(App.FullAppPath, "help", String.Format(Properties.Resources.AppHelpFileName, AppStrings.AppLangPrefix));

            // Проверим существование файла справки...
            if (File.Exists(CHMFile))
            {
                // Отобразим справочную систему в зависимости от контекста...
                Help.ShowHelp(this, CHMFile, HelpNavigator.Topic, GetHelpWebPage(MainTabControl.SelectedIndex));
            }
            else
            {
                // Скомпилированный файл справки не найден. Выводим сообщение об ошибке...
                MessageBox.Show(AppStrings.AppHelpCHMNotFound, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUOpinion_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessManager.OpenWebPage(Properties.Resources.AppURLReply);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, AppStrings.AppDbgExUrlHome);
            }
        }

        private void MNUSteamGroup_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Properties.Resources.AppURLSteamGrID);
            }
            catch
            {
                try
                {
                    ProcessManager.OpenWebPage(Properties.Resources.AppURLSteamGroup);
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, AppStrings.AppDbgExUrlGroup);
                }
            }
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
            catch (Exception Ex) { Logger.Warn(Ex, AppStrings.AppDbgExCfgEdRemRow); }
        }

        private void CE_Copy_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                foreach (DataGridViewCell DV in CE_Editor.SelectedCells) { if (DV.Value != null) { SB.AppendFormat("{0} ", DV.Value); } }
                Clipboard.SetText(SB.ToString().Trim());
            }
            catch (Exception Ex) { Logger.Warn(Ex, AppStrings.AppDbgExCfgEdCopy); }
        }

        private void CE_Cut_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                foreach (DataGridViewCell DV in CE_Editor.SelectedCells) { if (DV.Value != null) { SB.AppendFormat("{0} ", DV.Value); DV.Value = null; } }
                Clipboard.SetText(SB.ToString().Trim());
            }
            catch (Exception Ex) { Logger.Warn(Ex, AppStrings.AppDbgExCfgEdCut); }
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
            catch (Exception Ex) { Logger.Warn(Ex, AppStrings.AppDbgExCfgEdPaste); }
        }

        private void FP_OpenNotepad_Click(object sender, EventArgs e)
        {
            // Сгенерируем путь к файлу...
            string ConfigFile = Path.Combine(App.FullAppPath, "cfgs", App.SourceGames.SelectedGame.CFGMan.FPSConfig.FileName);
            
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
                try
                {
                    ProcessManager.OpenTextEditor(ConfigFile, Properties.Settings.Default.EditorBin, App.Platform.OS);
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, AppStrings.AppDbgExCfgEdExtEdt);
                }
            }
        }

        private void MNUUpdateCheck_Click(object sender, EventArgs e)
        {
            // Откроем форму модуля проверки обновлений...
            GuiHelpers.FormShowUpdater(App.UserAgent, App.FullAppPath, App.AppUserDir, App.Platform);
            
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
                    if (Regex.IsMatch(Path.GetExtension(BU_LVTable.SelectedItems[0].SubItems[4].Text), @"\.(txt|cfg|[0-9]|reg)"))
                    {
                        try
                        {
                            ProcessManager.OpenTextEditor(Path.Combine(App.SourceGames.SelectedGame.FullBackUpDirPath, BU_LVTable.SelectedItems[0].SubItems[4].Text), Properties.Settings.Default.EditorBin, App.Platform.OS);
                        }
                        catch (Exception Ex)
                        {
                            Logger.Warn(Ex, AppStrings.AppDbgExBkExtEdt);
                        }
                    }
                    else
                    {
                        MessageBox.Show(AppStrings.BU_BinaryFile, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void MNUAppOptions_Click(object sender, EventArgs e)
        {
            // Показываем форму настроек...
            GuiHelpers.FormShowOptions();
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
                    try
                    {
                        ProcessManager.OpenExplorer(Path.Combine(App.SourceGames.SelectedGame.FullBackUpDirPath, BU_LVTable.SelectedItems[0].SubItems[4].Text), App.Platform.OS);
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, AppStrings.AppDbgExBkFMan);
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

        private void FrmMainW_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Форма была закрыта, сохраняем настройки приложения...
            Properties.Settings.Default.Save();
        }

        private void MNUWinMnuDisabler_Click(object sender, EventArgs e)
        {
            // Показываем модуля отключения клавиш...
            GuiHelpers.FormShowKBHelper();
        }

        private void CE_OpenInNotepad_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(CFGFileName)))
            {
                try
                {
                    ProcessManager.OpenTextEditor(CFGFileName, Properties.Settings.Default.EditorBin, App.Platform.OS);
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, AppStrings.AppDbgExCfgEdExtEdt);
                }
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
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "replay", "*.*")
            };
            GuiHelpers.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile);
        }

        private void PS_RemTextures_Click(object sender, EventArgs e)
        {
            // Удаляем все кастомные текстуры...
            List<String> CleanDirs = new List<string>
            {
                // Чистим загруженные с серверов модели и текстуры...
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "download", "*.vt*"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "download", "*.vmt"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "download", "*.mdl"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "download", "*.phy"),
                
                // Чистим установленные пользователем модели и текстуры...
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "custom", "*.vt*"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "custom", "*.vmt"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "custom", "*.mdl"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "custom", "*.phy")
            };

            // Чистим базы игр со старой системой. Удалить после полного перехода на новую...
            if (Properties.Settings.Default.AllowUnSafeCleanup)
            {
                CleanDirs.Add(Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "materials", "*.*"));
                CleanDirs.Add(Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "models", "*.*"));
            }
            GuiHelpers.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile);
        }

        private void PS_RemSecndCache_Click(object sender, EventArgs e)
        {
            // Удаляем содержимое вторичного кэша загрузок...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "cache", "*.*"), // Кэш...
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "custom", "user_custom", "*.*"), // Кэш спреев игр с н.с.к...
                Path.Combine(App.SourceGames.SelectedGame.GamePath, "config", "html", "*.*") // Кэш MOTD...
            };
            GuiHelpers.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile);
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
            try
            {
                ProcessManager.OpenWebPage(AppStrings.AppCVListURL);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, AppStrings.AppDbgExUrlCvList);
            }
        }

        private void CE_ManualBackUpCfg_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(CFGFileName)))
            {
                if (File.Exists(CFGFileName))
                {
                    try
                    {
                        FileManager.CreateConfigBackUp(CFGFileName, App.SourceGames.SelectedGame.FullBackUpDirPath, Properties.Resources.BU_PrefixCfg);
                        MessageBox.Show(String.Format(AppStrings.CE_BackUpCreated, Path.GetFileName(CFGFileName)), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, AppStrings.AppDbgExCfgEdBkMan);
                    }
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
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "download", "*.mp3"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "download", "*.wav")
            };
            if (Properties.Settings.Default.AllowUnSafeCleanup) { CleanDirs.Add(Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "sound", "*.*")); }
            GuiHelpers.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile);
        }

        private void PS_RemCustDir_Click(object sender, EventArgs e)
        {
            // Удаляем пользовательного каталога...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "custom", "*.*"),
                Path.Combine(App.SourceGames.SelectedGame.AppWorkshopDir, "*.*")
            };
            GuiHelpers.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile);
        }

        private void PS_DeepCleanup_Click(object sender, EventArgs e)
        {
            // Проведём глубокую очистку...
            List<String> CleanDirs = new List<string>
            {
                // Удалим старые бинарники и лаунчеры...
                Path.Combine(App.SourceGames.SelectedGame.GamePath, Path.GetDirectoryName(App.SourceGames.SelectedGame.SmallAppName), "bin", "*.*"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "bin", "*.*"),
                Path.Combine(App.SourceGames.SelectedGame.GamePath, "*.exe"),

                // Удалим кэш загрузок...
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "download", "*.*"),

                // Удалим кастомные файлы...
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "custom", "*.*"),
                Path.Combine(App.SourceGames.SelectedGame.AppWorkshopDir, "*.*"),

                // Удалим другие кэши...
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "cache", "*.*"),

                // Удалим пользовательские конфиги...
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "cfg", "*.cfg")
            };

            // Конфиги их хранилища Steam Cloud...
            CleanDirs.AddRange(App.SourceGames.SelectedGame.CloudConfigs);

            // Данные платформы...
            if (Properties.Settings.Default.AllowUnSafeCleanup) { CleanDirs.Add(Path.Combine(App.SourceGames.SelectedGame.GamePath, "platform", "*.*")); }
            
            // Удаляем графические настройки NCF-игры...
            if (App.SourceGames.SelectedGame.IsUsingVideoFile) { CleanDirs.AddRange(App.SourceGames.SelectedGame.VideoCfgFiles); }

            // Запускаем процесс очистки...
            GuiHelpers.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CacheChkReq, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile);
        }

        private void PS_RemConfigs_Click(object sender, EventArgs e)
        {
            // Удаляем пользовательного каталога...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "cfg", "*.*"),
                Path.Combine(App.SourceGames.SelectedGame.FullGamePath, "custom", "*.cfg")
            };
            CleanDirs.AddRange(App.SourceGames.SelectedGame.CloudConfigs);
            GuiHelpers.FormShowCleanup(CleanDirs, ((Button)sender).Text.ToLower(), AppStrings.PS_CleanupSuccess, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile);
        }

        private void HD_HSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получим информацию о выбранном HUD...
            try
            {
                App.SourceGames.SelectedGame.HUDMan.Select(HD_HSel.Text);
            }
            catch (Exception Ex) { Logger.Warn(Ex, AppStrings.AppDbgExHudSelect); }
                
            // Проверяем результат...
            bool Success = !String.IsNullOrEmpty(App.SourceGames.SelectedGame.HUDMan.SelectedHUD.Name);

            // Переключаем статус элементов управления...
            HD_GB_Pbx.Image = Properties.Resources.LoadingFile;
            HD_Install.Enabled = Success;
            HD_Homepage.Enabled = Success;
            HD_Warning.Visible = Success && !App.SourceGames.SelectedGame.HUDMan.SelectedHUD.IsUpdated;

            // Выводим информацию о последнем обновлении HUD...
            HD_LastUpdate.Visible = Success;
            if (Success) { HD_LastUpdate.Text = String.Format(AppStrings.HD_LastUpdateInfo, FileManager.Unix2DateTime(App.SourceGames.SelectedGame.HUDMan.SelectedHUD.LastUpdate).ToLocalTime()); }

            // Проверяем установлен ли выбранный HUD...
            SetHUDButtons(HUDManager.CheckInstalledHUD(App.SourceGames.SelectedGame.CustomInstallDir, App.SourceGames.SelectedGame.HUDMan.SelectedHUD.InstallDir));

            // Загрузим скриншот выбранного HUD...
            if (Success && !BW_HUDScreen.IsBusy) { BW_HUDScreen.RunWorkerAsync(); }
        }

        private void HD_Install_Click(object sender, EventArgs e)
        {
            if (!HUDManager.CheckHUDDatabase(Properties.Settings.Default.LastHUDTime))
            {
                // Проверим поддерживает ли выбранный HUD последнюю версию игры...
                if (App.SourceGames.SelectedGame.HUDMan.SelectedHUD.IsUpdated)
                {
                    // Спросим пользователя о необходимости установки/обновления HUD...
                    if (MessageBox.Show(String.Format("{0}?", ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Начинаем загрузку архива с HUD...
                        GuiHelpers.FormShowDownloader(Properties.Settings.Default.HUDUseUpstream ? App.SourceGames.SelectedGame.HUDMan.SelectedHUD.UpURI : App.SourceGames.SelectedGame.HUDMan.SelectedHUD.URI, App.SourceGames.SelectedGame.HUDMan.SelectedHUD.LocalFile);

                        // Проверяем существует ли файл с архивом...
                        if (File.Exists(App.SourceGames.SelectedGame.HUDMan.SelectedHUD.LocalFile))
                        {
                            // Проверяем контрольную сумму загруженного архива...
                            if (Properties.Settings.Default.HUDUseUpstream || FileManager.CalculateFileMD5(App.SourceGames.SelectedGame.HUDMan.SelectedHUD.LocalFile) == App.SourceGames.SelectedGame.HUDMan.SelectedHUD.FileHash)
                            {
                                // Проверим установлен ли выбранный HUD...
                                if (HUDManager.CheckInstalledHUD(App.SourceGames.SelectedGame.CustomInstallDir, App.SourceGames.SelectedGame.HUDMan.SelectedHUD.InstallDir))
                                {
                                    // Удаляем уже установленные файлы HUD...
                                    GuiHelpers.FormShowRemoveFiles(SingleToArray(Path.Combine(App.SourceGames.SelectedGame.CustomInstallDir, App.SourceGames.SelectedGame.HUDMan.SelectedHUD.InstallDir)));
                                }

                                // Распаковываем загруженный архив с файлами HUD...
                                GuiHelpers.FormShowArchiveExtract(App.SourceGames.SelectedGame.HUDMan.SelectedHUD.LocalFile, Path.Combine(App.SourceGames.SelectedGame.CustomInstallDir, "hudtemp"));

                                // Запускаем установку пакета в отдельном потоке...
                                if (!BW_HudInstall.IsBusy) { BW_HudInstall.RunWorkerAsync(); }
                            }
                            else
                            {
                                // Проверка хеша загруженного файла не удалась. Выведем сообщение об этом...
                                MessageBox.Show(AppStrings.HD_HashError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            // Удаляем загруженный файл архива...
                            try
                            {
                                File.Delete(App.SourceGames.SelectedGame.HUDMan.SelectedHUD.LocalFile);
                            }
                            catch (Exception Ex) { Logger.Warn(Ex, AppStrings.AppDbgExHudArchRem); }
                        }
                        else
                        {
                            MessageBox.Show(AppStrings.HD_DownloadError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
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
                string HUDPath = Path.Combine(App.SourceGames.SelectedGame.CustomInstallDir, App.SourceGames.SelectedGame.HUDMan.SelectedHUD.InstallDir);

                // Воспользуемся модулем быстрой очистки для удаления выбранного HUD...
                GuiHelpers.FormShowRemoveFiles(SingleToArray(HUDPath));

                // Проверяем установлен ли выбранный HUD...
                bool IsInstalled = HUDManager.CheckInstalledHUD(App.SourceGames.SelectedGame.CustomInstallDir, App.SourceGames.SelectedGame.HUDMan.SelectedHUD.InstallDir);

                // При успешном удалении HUD выводим сообщение и сносим и его каталог...
                if (!IsInstalled) { MessageBox.Show(AppStrings.PS_CleanupSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); if (Directory.Exists(HUDPath)) { Directory.Delete(HUDPath); } }

                // Включаем / отключаем кнопки...
                SetHUDButtons(IsInstalled);
            }
        }

        private void HD_Homepage_Click(object sender, EventArgs e)
        {
            // Откроем домашнюю страницу выбранного HUD...
            if (!String.IsNullOrEmpty(App.SourceGames.SelectedGame.HUDMan.SelectedHUD.Site))
            {
                try
                {
                    ProcessManager.OpenWebPage(App.SourceGames.SelectedGame.HUDMan.SelectedHUD.Site);
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, AppStrings.AppDbgExUrlHudHome);
                }
            }
        }

        private void MNUExtClnAppCache_Click(object sender, EventArgs e)
        {
            // Очистим загруженные приложением файлы...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(App.AppUserDir, StringsManager.HudDirectoryName, "*.*")
            };
            GuiHelpers.FormShowCleanup(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", String.Empty), AppStrings.PS_CleanupSuccess, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile);
        }

        private void MNUExtClnTmpDir_Click(object sender, EventArgs e)
        {
            // Очистим каталоги с временными файлами системы...
            List<String> CleanDirs = new List<string>
            {
                Path.Combine(Path.GetTempPath(), "*.*")
            };
            GuiHelpers.FormShowCleanup(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", String.Empty), AppStrings.PS_CleanupSuccess, App.SourceGames.SelectedGame.FullBackUpDirPath, App.SourceGames.SelectedGame.GameBinaryFile);
        }

        private void MNUShowLog_Click(object sender, EventArgs e)
        {
            // Выведем на экран содержимое отладочного журнала...
            string DFile = App.LogFileName;
            if (File.Exists(DFile)) { GuiHelpers.FormShowLogViewer(DFile); } else { MessageBox.Show(AppStrings.AppNoDebugFile, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void HD_Warning_Click(object sender, EventArgs e)
        {
            // Выведем предупреждающие сообщения...
            if (!App.SourceGames.SelectedGame.HUDMan.SelectedHUD.IsUpdated) { MessageBox.Show(AppStrings.HD_NotTested, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void HD_OpenDir_Click(object sender, EventArgs e)
        {
            // Покажем файлы установленного HUD в Проводнике...
            try
            {
                ProcessManager.OpenExplorer(Path.Combine(App.SourceGames.SelectedGame.CustomInstallDir, App.SourceGames.SelectedGame.HUDMan.SelectedHUD.InstallDir), App.Platform.OS);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, AppStrings.AppDbgExHudExtFm);
            }
        }

        private void MNUExtClnSteam_Click(object sender, EventArgs e)
        {
            // Запустим модуль очистки кэшей Steam...
            GuiHelpers.FormShowStmCleaner(App.SteamClient.FullSteamPath, App.SourceGames.SelectedGame.FullBackUpDirPath, App.Platform.SteamAppsFolderName, App.Platform.SteamProcName);
        }

        private void MNUMuteMan_Click(object sender, EventArgs e)
        {
            // Запустим менеджер управления отключёнными игроками...
            GuiHelpers.FormShowMuteManager(App.SourceGames.SelectedGame.GetActualBanlistFile(), App.SourceGames.SelectedGame.FullBackUpDirPath);
        }

        private void SB_SteamID_Click(object sender, EventArgs e)
        {
            // Открываем диалог выбора SteamID и прописываем пользовательский выбор...
            try
            {
                string Result = GuiHelpers.FormShowIDSelect(App.SteamClient.SteamIDs);
                if (!String.IsNullOrWhiteSpace(Result))
                {
                    SB_SteamID.Text = Result;
                    Properties.Settings.Default.LastSteamID = Result;
                    FindGames();
                }
            } catch (Exception Ex) { Logger.Warn(Ex, AppStrings.AppDbgExUserIdSel); }
        }

        private void BU_LVTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Блокируем некоторые кнопки на панели инструментов модуля управления резервными копиями если выбрано более одной...
            bool IsSingle = BU_LVTable.SelectedItems.Count <= 1;
            BUT_OpenNpad.Enabled = IsSingle;
            BUT_ExploreBUp.Enabled = IsSingle;
        }
    }
}
