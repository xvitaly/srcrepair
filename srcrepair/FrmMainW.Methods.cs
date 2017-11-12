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
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace srcrepair
{
    partial class FrmMainW
    {
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
            // Очистим список игр...
            AppSelector.Items.Clear();
            SourceGames.Clear();

            // При использовании нового метода поиска установленных игр, считаем их из конфига Steam...
            List<String> GameDirs = SteamManager.FormatInstallDirs(App.FullSteamPath, App.Platform.SteamAppsFolderName);

            try
            {
                // Создаём поток с XML-файлом...
                using (FileStream XMLFS = new FileStream(Path.Combine(App.FullAppPath, Properties.Resources.GameListFile), FileMode.Open, FileAccess.Read))
                {
                    // Создаём объект документа XML...
                    XmlDocument XMLD = new XmlDocument();

                    // Загружаем поток в объект XML документа...
                    XMLD.Load(XMLFS);

                    // Обходим полученный список в цикле...
                    XmlNodeList XMLNode = XMLD.GetElementsByTagName("Game");
                    for (int i = 0; i < XMLNode.Count; i++)
                    {
                        try
                        {
                            if (XMLD.GetElementsByTagName("Enabled")[i].InnerText == "1" || !Properties.Settings.Default.HideUnsupportedGames)
                            {
                                SourceGame SG = new SourceGame(XMLNode[i].Attributes["Name"].Value, XMLD.GetElementsByTagName("DirName")[i].InnerText, XMLD.GetElementsByTagName("SmallName")[i].InnerText, XMLD.GetElementsByTagName("Executable")[i].InnerText, XMLD.GetElementsByTagName("SID")[i].InnerText, XMLD.GetElementsByTagName("SVer")[i].InnerText, XMLD.GetElementsByTagName("VFDir")[i].InnerText, App.Platform.OS == CurrentPlatform.OSType.Windows ? XMLD.GetElementsByTagName("HasVF")[i].InnerText == "1" : true, XMLD.GetElementsByTagName("UserDir")[i].InnerText == "1", XMLD.GetElementsByTagName("HUDsAvail")[i].InnerText == "1", App.FullAppPath, App.AppUserDir, App.FullSteamPath, App.Platform.SteamAppsFolderName, GameDirs);
                                if (SG.IsInstalled)
                                {
                                    SourceGames.Add(SG);
                                    AppSelector.Items.Add(XMLNode[i].Attributes["Name"].Value);
                                }
                            }
                        }
                        catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    }
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Записывает настройки GCF-игры в реестр Windows.
        /// </summary>
        private void WriteType1VideoSettings()
        {
            // Создаём новый объект без получения данных из реестра...
            Type1Video Video = new Type1Video(SelGame.SmallAppName, false)
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
            Type2Video Video = new Type2Video(SelGame.GetActualVideoFile(), SelGame.SourceType, false)
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
                Type1Video Video = new Type1Video(SelGame.SmallAppName, true);

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
                CoreLib.HandleExceptionEx(AppStrings.GT_RegOpenErr, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
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
                string VFileName = SelGame.GetActualVideoFile();

                // Загружаем содержимое если он существует...
                if (File.Exists(VFileName))
                {
                    // Получаем графические настройки...
                    Type2Video Video = new Type2Video(VFileName, SelGame.SourceType, true);

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
                    CoreLib.WriteStringToLog(String.Format(AppStrings.AppVideoDbNotFound, SelGame.FullAppName, VFileName));
                }
            }
            catch (Exception Ex)
            {
                // Выводим сообщение об ошибке...
                CoreLib.HandleExceptionEx(AppStrings.GT_NCFLoadFailure, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
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
                    CoreLib.HandleExceptionEx(AppStrings.CE_ExceptionDetected, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
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
        /// <param name="FileName">Указатель на файл резервной копии</param>
        /// <returns>Возвращает пару "тип архива" и "удобочитаемое название"</returns>
        private Tuple<string, string> GenUserFriendlyBackupDesc(FileInfo FileName)
        {
            string BufName = Path.GetFileNameWithoutExtension(FileName.Name);
            string Buf = String.Empty;

            switch (FileName.Extension)
            {
                case ".reg":
                    Buf = AppStrings.BU_BType_Reg;
                    if (BufName.IndexOf("Game_Options", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_GRGame, FileName.CreationTime);
                    }
                    else if (BufName.IndexOf("Source_Options", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_SRCAll, FileName.CreationTime);
                    }
                    else if (BufName.IndexOf("Steam_BackUp", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_SteamAll, FileName.CreationTime);
                    }
                    else if (BufName.IndexOf("Game_AutoBackUp", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_GameAuto, FileName.CreationTime);
                    }
                    break;
                case ".bud":
                    Buf = AppStrings.BU_BType_Cont;
                    if (BufName.IndexOf(Properties.Resources.BU_PrefixDef, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_Bud, FileName.CreationTime);
                    }
                    else if (BufName.IndexOf(Properties.Resources.BU_PrefixCfg, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        Buf = AppStrings.BU_BType_Cfg;
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_Config, FileName.CreationTime);
                    }
                    else if (BufName.IndexOf(Properties.Resources.BU_PrefixVChat, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        Buf = AppStrings.BU_BType_DB;
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_VChat, FileName.CreationTime);
                    }
                    else if (BufName.IndexOf(Properties.Resources.BU_PrefixVideo, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        Buf = AppStrings.BU_BType_Video;
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_GRGame, FileName.CreationTime);
                    }
                    else if (BufName.IndexOf(Properties.Resources.BU_PrefixVidAuto, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        Buf = AppStrings.BU_BType_Video;
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, AppStrings.BU_BName_GameAuto, FileName.CreationTime);
                    }
                    break;
                default:
                    Buf = AppStrings.BU_BType_Unkn;
                    break;
            }

            return Tuple.Create(Buf, BufName);
        }

        /// <summary>
        /// Считывает файлы резервных копий из указанного каталога и помещает в таблицу.
        /// </summary>
        private void ReadBackUpList2Table()
        {
            // Очистим таблицу...
            Invoke((MethodInvoker)delegate () { BU_LVTable.Items.Clear(); });

            // Открываем каталог...
            DirectoryInfo DInfo = new DirectoryInfo(SelGame.FullBackUpDirPath);

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
                LvItem.SubItems.Add(CoreLib.SclBytes(DItem.Length));
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
            switch (SelGame.SourceType)
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
            switch (SelGame.SourceType)
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
            SelectGraphicWidget((App.Platform.OS != CurrentPlatform.OSType.Windows) && (SelGame.SourceType == "1") ? "2" : SelGame.SourceType);
        }

        /// <summary>
        /// Выполняет особые действия и начинает процесс сохранения настроек видео
        /// для Type 1 игры.
        /// </summary>
        private void PrepareWriteType1VideoSettings()
        {
            // Генерируем путь к ветке реестра с настройками...
            string GameRegKey = Type1Video.GetGameRegKey(SelGame.SmallAppName);

            // Создаём резервную копию если включена опция безопасной очистки...
            if (Properties.Settings.Default.SafeCleanup)
            {
                try { Type1Video.BackUpVideoSettings(GameRegKey, "Game_AutoBackUp", SelGame.FullBackUpDirPath); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
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
                CoreLib.HandleExceptionEx(AppStrings.GT_SaveFailure, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
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
                FileManager.CreateConfigBackUp(SelGame.VideoCfgFiles, SelGame.FullBackUpDirPath, Properties.Resources.BU_PrefixVidAuto);
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
                CoreLib.HandleExceptionEx(AppStrings.GT_NCFFailure, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Сохраняет настройки видео для выбранной игры.
        /// </summary>
        private void WriteGraphicSettings()
        {
            // Определим тип игры...
            switch (SelGame.SourceType)
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
                App.FullSteamPath = CheckLastSteamPath(Properties.Settings.Default.LastSteamPath);
            }
            catch (FileNotFoundException Ex)
            {
                CoreLib.HandleExceptionEx(AppStrings.SteamPathEnterErr, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
                Environment.Exit(7);
            }
            catch (OperationCanceledException Ex)
            {
                CoreLib.HandleExceptionEx(AppStrings.SteamPathCancel, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
                Environment.Exit(19);
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(AppStrings.AppGenericError, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
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
                PS_OSDrive.Text = String.Format(PS_OSDrive.Text, FileManager.DetectDriveFileSystem(Path.GetPathRoot(SelGame.FullGamePath)));
            }
            catch (Exception Ex)
            {
                PS_OSDrive.Text = String.Format(PS_OSDrive.Text, "Unknown");
                CoreLib.WriteStringToLog(Ex.Message);
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
                        CoreLib.WriteStringToLog(String.Format(AppStrings.AppNoGamesDLog, App.FullSteamPath));
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
            if (!(FileManager.CheckNonASCII(App.FullSteamPath)))
            {
                PS_PathSteam.ForeColor = Color.Red;
                PS_PathSteam.Image = Properties.Resources.upd_err;
                CoreLib.WriteStringToLog(String.Format(AppStrings.AppRestrSymbLog, App.FullSteamPath));
            }
        }

        /// <summary>
        /// Запускает проверку на наличие запрещённых символов в пути установки игры.
        /// </summary>
        private void CheckSymbolsGame()
        {
            if (!(FileManager.CheckNonASCII(SelGame.FullGamePath)))
            {
                PS_PathGame.ForeColor = Color.Red;
                PS_PathGame.Image = Properties.Resources.upd_err;
                CoreLib.WriteStringToLog(String.Format(AppStrings.AppRestrSymbLog, SelGame.FullGamePath));
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
            SelGame.FPSConfigs = FileManager.ExpandFileList(ConfigManager.ListFPSConfigs(SelGame.FullGamePath, SelGame.IsUsingUserDir), true);
            GT_Warning.Visible = SelGame.FPSConfigs.Count > 0;
            FP_Uninstall.Enabled = SelGame.FPSConfigs.Count > 0;
        }

        /// <summary>
        /// Управляет выводом текущего SteamID.
        /// </summary>
        /// <param name="SID">Сохранённый SteamID</param>
        private void HandleSteamIDs(string SID)
        {
            try
            {
                string Result = SelGame.GetCurrentSteamID(SID);
                SB_SteamID.Text = Result;
                Properties.Settings.Default.LastSteamID = Result;
            }
            catch (Exception Ex)
            {
                CoreLib.WriteStringToLog(Ex.Message);
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
            switch (SelGame.SourceType)
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
                // Считываем и выводим в таблицу файлы резервных копий...
                ReadBackUpList2Table();
            }
            catch (Exception Ex)
            {
                // Произошло исключение. Запишем в журнал...
                CoreLib.WriteStringToLog(Ex.Message);

                // Создадим каталог для хранения резервных копий если его ещё нет...
                if (!Directory.Exists(SelGame.FullBackUpDirPath)) { Directory.CreateDirectory(SelGame.FullBackUpDirPath); }
            }
        }

        /// <summary>
        /// Ищет установленные игры и выполняет ряд необходимых проверок.
        /// </summary>
        private void FindGames()
        {
            // Начинаем определять установленные игры...
            try { DetectInstalledGames(); } catch (Exception Ex) { CoreLib.HandleExceptionEx(AppStrings.AppXMLParseError, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error); Environment.Exit(16); }

            // Проверим нашлись ли игры...
            CheckGames(AppSelector.Items.Count);
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
                    Result = "gtweaker";
                    break;
                case 1: /* Редактор конфигов. */
                    Result = "cfgeditor";
                    break;
                case 2: /* Устранение проблем и очистка. */
                    Result = "cleanup";
                    break;
                case 3: /* FPS-конфиги. */
                    Result = "fpscfgs";
                    break;
                case 4: /* Менеджер HUD. */
                    Result = "hudman";
                    break;
                case 5: /* Резервные копии. */
                    Result = "backups";
                    break;
            }

            // Возвращаем финальный URL...
            return String.Format(Properties.Resources.AppURLHelpSystem, Result);
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
    }
}
