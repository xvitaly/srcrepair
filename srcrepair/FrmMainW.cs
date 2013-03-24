/*
 * Основной модуль программы SRC Repair.
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
using System.Drawing; // аналогично Forms...
using System.Linq;
using System.Text;
using System.Windows.Forms; // для работы с формами...
using System.IO; // для работы с файлами...
using System.Diagnostics; // для управления процессами...
using Microsoft.Win32; // для работы с реестром...
using System.Reflection; // для управления сборками...
using System.Globalization; // для управления локализациями...
using System.Resources; // для управления ресурсами...
using System.Net; // для скачивания файлов...
using System.Xml; // для разбора (парсинга) XML...
using Ionic.Zip; // для работы с Zip архивами...

namespace srcrepair
{
    public partial class frmMainW : Form
    {
        public frmMainW()
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

        /* В этой переменной будем хранить имя открытого конфига для служебных целей. */
        private string CFGFileName;

        #endregion

        #region Internal Functions

        /// <summary>
        /// Устанавливает требуемый FPS-конфиг.
        /// </summary>
        /// <param name="ConfName">Имя конфига</param>
        /// <param name="AppPath">Путь к программе SRC Repair</param>
        /// <param name="DestPath">Каталог назначения</param>
        private void InstallConfigNow(string ConfName, string AppPath, string DestPath)
        {
            // Устанавливаем...
            File.Copy(Path.Combine(AppPath, "cfgs", ConfName), Path.Combine(DestPath, "autoexec.cfg"), true);
        }

        /// <summary>
        /// Создаёт резервную копию конфига, имя которого передано в параметре.
        /// </summary>
        /// <param name="ConfName">Имя конфига без пути</param>
        /// <param name="ConfigDir">Путь к каталогу с конфигом</param>
        /// <param name="BackUpDir">Путь к каталогу с резервными копиями</param>
        private void CreateBackUpNow(string ConfName, string ConfigDir, string BackUpDir)
        {
            // Сначала проверим, существует ли запрошенный файл...
            if (File.Exists(Path.Combine(ConfigDir, ConfName)))
            {
                // Проверяем чтобы каталог для бэкапов существовал...
                if (!(Directory.Exists(BackUpDir)))
                {
                    // Каталоги не существуют. Создадим общий каталог для резервных копий...
                    Directory.CreateDirectory(BackUpDir);
                }

                try
                {
                    // Копируем оригинальный файл в файл бэкапа...
                    File.Copy(Path.Combine(ConfigDir, ConfName), Path.Combine(BackUpDir, ConfName + "." + CoreLib.WriteDateToString(DateTime.Now, true)), true);
                }
                catch (Exception Ex)
                {
                    // Произошло исключение. Уведомим пользователя об этом...
                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("BackUpCreationFailed"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Очищает блобы (файлы с расширением *.blob) из каталога Steam.
        /// </summary>
        /// <param name="SteamPath">Полный путь к каталогу Steam</param>
        private void CleanBlobsNow(string SteamPath)
        {
            // Инициализируем буферную переменную, в которой будем хранить имя файла...
            string FileName;

            // Генерируем имя первого кандидата на удаление с полным путём до него...
            FileName = Path.Combine(SteamPath, "AppUpdateStats.blob");

            // Проверяем существует ли данный файл...
            if (File.Exists(FileName))
            {
                // Удаляем...
                File.Delete(FileName);
            }

            // Аналогично генерируем имя второго кандидата...
            FileName = Path.Combine(SteamPath, "ClientRegistry.blob");

            // Проверяем, существует ли файл...
            if (File.Exists(FileName))
            {
                // Удаляем...
                File.Delete(FileName);
            }
        }

        /// <summary>
        /// Удаляет значения реестра, отвечающие за настройки клиента Steam,
        /// а также записывает значение языка.
        /// </summary>
        /// <param name="LangCode">ID языка Steam</param>
        private void CleanRegistryNow(int LangCode)
        {
            // Удаляем ключ HKEY_LOCAL_MACHINE\Software\Valve рекурсивно...
            Registry.LocalMachine.DeleteSubKeyTree(Path.Combine("Software", "Valve"), false);

            // Удаляем ключ HKEY_CURRENT_USER\Software\Valve рекурсивно...
            Registry.CurrentUser.DeleteSubKeyTree(Path.Combine("Software", "Valve"), false);

            // Начинаем вставлять значение языка клиента Steam...
            // Инициализируем буферную переменную для хранения названия языка...
            string XLang;

            // Генерируем...
            switch (LangCode)
            {
                case 0:
                    XLang = "english";
                    break;
                case 1:
                    XLang = "russian";
                    break;
                default:
                    XLang = "english";
                    break;
            }

            // Подключаем реестр и создаём ключ HKEY_CURRENT_USER\Software\Valve\Steam...
            RegistryKey RegLangKey = Registry.CurrentUser.CreateSubKey(Path.Combine("Software", "Valve", "Steam"));

            // Если не было ошибок, записываем значение...
            if (RegLangKey != null)
            {
                // Записываем значение в реестр...
                RegLangKey.SetValue("language", XLang);
            }

            // Закрываем ключ...
            RegLangKey.Close();
        }

        /// <summary>
        /// Сохраняет содержимое таблицы в файл конфигурации, указанный в
        /// параметре. Используется в Save и SaveAs Редактора конфигов.
        /// </summary>
        /// <param name="Path">Полный путь к файлу конфига</param>
        /// <param name="XAppName">Название программы</param>
        private void WriteTableToFileNow(string Path, string XAppName)
        {
            // Начинаем сохранять содержимое редактора в файл...
            using (StreamWriter CFile = new StreamWriter(Path))
            {
                CFile.WriteLine("// Generated by " + XAppName + " //"); // Вставляем сообщение ;-).
                CFile.WriteLine(); // вставляем пустую строку
                for (int i = 0; i < CE_Editor.Rows.Count; i++) // запускаем цикл
                {
                    CFile.Write(CE_Editor.Rows[i].Cells[0].Value); // вставляем содержимое первого столбца (название переменной)
                    CFile.Write(" "); // вставляем пробел
                    CFile.WriteLine(CE_Editor.Rows[i].Cells[1].Value); // вставляем содержимое второго столбца (значение переменной)
                }
                CFile.Close(); // закрываем файл
            }
        }

        /// <summary>
        /// Используется для записи значений в таблицу Редактора конфигов.
        /// Используется делегатом. Прямой вызов не допускается.
        /// </summary>
        /// <param name="Cv">Название переменной</param>
        /// <param name="Cn">Значение переменной</param>
        private void AddRowToTable(string Cv, string Cn)
        {
            CE_Editor.Rows.Add(Cv, Cn);
        }

        /// <summary>
        /// Используется для создания резервной копии выбранной ветки
        /// реестра в переданный в параметре файл.
        /// </summary>
        /// <param name="RKey">Ветка реестра для резервирования</param>
        /// <param name="FileName">Имя файла резервной копии</param>
        /// <param name="DestDir">Каталог с резервными копиями</param>
        private void CreateRegBackUpNow(string RKey, string FileName, string DestDir)
        {
            // Генерируем строку с параметрами...
            string Params = String.Format("/ea \"{0}\" {1}", Path.Combine(DestDir, String.Format("{0}_{1}.reg", FileName, CoreLib.WriteDateToString(DateTime.Now, true))), RKey);
            // Запускаем и ждём завершения...
            CoreLib.StartProcessAndWait("regedit.exe", Params);
        }

        /// <summary>
        /// Возвращает описание переданной в качестве параметра переменной, получая
        /// эту информацию из ресурса CVList с учётом локализации.
        /// </summary>
        /// <param name="CVar">Название переменной</param>
        /// <returns>Описание переменной с учётом локализации</returns>
        private string GetCVDescription(string CVar)
        {
            ResourceManager DM = new ResourceManager("srcrepair.CVList", typeof(frmMainW).Assembly);
            return DM.GetString(CVar);
        }

        /// <summary>
        /// Разрешает/блокирует кнопки, отвечающие за очистку (актуально
        /// для NCF-приложений).
        /// </summary>
        /// <param name="BStatus">Новый статус</param>
        private void EnableCleanButtons(bool BStatus)
        {
            PS_RemCustMaps.Enabled = BStatus;
            //PS_RemDnlCache.Enabled = BStatus;
            //PS_RemOldSpray.Enabled = BStatus;
            PS_RemSounds.Enabled = BStatus;
            PS_RemGraphCache.Enabled = BStatus;
            PS_RemSoundCache.Enabled = BStatus;
            PS_RemSecndCache.Enabled = BStatus;
            //PS_RemScreenShots.Enabled = BStatus;
            //PS_RemDemos.Enabled = BStatus;
            //PS_RemGraphOpts.Enabled = BStatus;
            //PS_RemOldBin.Enabled = BStatus;
            PS_RemTextures.Enabled = BStatus;
            PS_RemModels.Enabled = BStatus;
            PS_RemReplays.Enabled = BStatus;
        }

        /// <summary>
        /// Отображает диалоговое окно менеджера быстрой очистки.
        /// </summary>
        /// <param name="Path">Путь к каталогу очистки</param>
        /// <param name="Mask">Маска файлов, подлежащих очистке</param>
        /// <param name="LText">Текст заголовка</param>
        /// <param name="ReadOnly">Пользователю будет запрещено изменять выбор удаляемых файлов</param>
        /// <param name="NoAuto">Включает / отключает автовыбор файлов флажками</param>
        private void OpenCleanupWindow(List<String> Paths, string LText, bool ReadOnly = false, bool NoAuto = false)
        {
            frmCleaner FCl = new frmCleaner(Paths, LText, ReadOnly, NoAuto);
            FCl.ShowDialog();
        }

        /// <summary>
        /// Считывает из главного файла конфигурации Steam пути к установленным играм.
        /// </summary>
        /// <param name="SteamPath">Путь к клиенту Steam</param>
        private List<String> GetInstalledDirsFromFile(string SteamPath)
        {
            // Создаём массив, в который будем помещать найденные пути...
            List<String> Result = new List<String>();

            // Начинаем чтение главного файла конфигурации...
            try
            {
                // Открываем файл как поток...
                using (StreamReader SteamConfig = new StreamReader(Path.Combine(SteamPath, "config", "config.vdf"), Encoding.Default))
                {
                    // Инициализируем буферную переменную...
                    string RdStr;

                    // Читаем поток построчно...
                    while (SteamConfig.Peek() >= 0)
                    {
                        // Считываем строку и сразу очищаем от лишнего...
                        RdStr = SteamConfig.ReadLine().Trim();

                        // Проверяем наличие данных в строке...
                        if (!(String.IsNullOrWhiteSpace(RdStr)))
                        {
                            // Ищем в строке путь установки...
                            if (RdStr.IndexOf("installdir") != -1)
                            {
                                RdStr = CoreLib.CleanStrWx(RdStr, true);
                                RdStr = RdStr.Remove(0, RdStr.IndexOf(" ") + 1);
                                if (!(String.IsNullOrWhiteSpace(RdStr))) { try { DirectoryInfo DInfo = new DirectoryInfo(RdStr); Result.Add(DInfo.FullName); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); } }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                CoreLib.WriteStringToLog(Ex.Message);
            }

            // Возвращаем сформированный массив...
            return Result;
        }

        /// <summary>
        /// Определяет установленные игры и заполняет комбо-бокс выбора
        /// доступных управляемых игр.
        /// </summary>
        /// <param name="SteamPath">Путь к клиенту Steam</param>
        /// <param name="SteamAppsDir">Имя каталога SteamApps</param>
        private void DetectInstalledGames(string SteamPath, string SteamAppsDir)
        {
            // Очистим список игр...
            AppSelector.Items.Clear();

            // При использовании нового метода поиска установленных игр, считаем их из конфига Steam...
            GV.GameDirs = GetInstalledDirsFromFile(GV.FullSteamPath);
            
            // Формируем список для поддерживаемых игр...
            List<String> AvailableGames = new List<String>();

            try
            {
                XmlDocument XMLD = new XmlDocument(); // Создаём объект документа XML...
                FileStream XMLFS = new FileStream(Path.Combine(GV.FullAppPath, Properties.Settings.Default.GameListFile), FileMode.Open, FileAccess.Read); // Создаём поток с XML-файлом...
                XMLD.Load(XMLFS); // Загружаем поток в объект XML документа...
                for (int i = 0; i < XMLD.GetElementsByTagName("Game").Count; i++) // Обходим полученный список в цикле...
                {
                    AvailableGames.Add(XMLD.GetElementsByTagName("DirName")[i].InnerText);
                }
                XMLFS.Close(); // Закрываем файловый поток...
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            try
            {
                // Обойдём полученный список каталогов в массиве...
                foreach (string StrX in GV.GameDirs)
                {
                    if (AvailableGames.Exists(s => s.IndexOf(Path.GetFileName(StrX), StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        AppSelector.Items.Add(StrX);
                    }
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Записывает настройки GCF-игры в реестр Windows.
        /// </summary>
        /// <param name="SAppName">Краткое имя игры</param>
        private void WriteGCFGameSettings(string SAppName)
        {            
            // Открываеам ключ реестра для записи...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Valve", "Source", SAppName, "Settings"), true);

            // Запишем в реестр настройки разрешения экрана...
            // По горизонтали (ScreenWidth):
            try { ResKey.SetValue("ScreenWidth", (int)GT_ResHor.Value, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // По вертикали (ScreenHeight):
            try { ResKey.SetValue("ScreenHeight", (int)GT_ResVert.Value, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки режима запуска приложения (ScreenWindowed):
            switch (GT_ScreenType.SelectedIndex)
            {
                case 0: try { ResKey.SetValue("ScreenWindowed", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 1: try { ResKey.SetValue("ScreenWindowed", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
            }

            // Запишем в реестр настройки детализации моделей (r_rootlod):
            switch (GT_ModelQuality.SelectedIndex)
            {
                case 0: try { ResKey.SetValue("r_rootlod", 2, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 1: try { ResKey.SetValue("r_rootlod", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 2: try { ResKey.SetValue("r_rootlod", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
            }

            // Запишем в реестр настройки детализации текстур (mat_picmip):
            switch (GT_TextureQuality.SelectedIndex)
            {
                case 0: try { ResKey.SetValue("mat_picmip", 2, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 1: try { ResKey.SetValue("mat_picmip", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 2: try { ResKey.SetValue("mat_picmip", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 3: try { ResKey.SetValue("mat_picmip", -1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
            }

            // Запишем в реестр настройки качества шейдерных эффектов (mat_reducefillrate):
            switch (GT_ShaderQuality.SelectedIndex)
            {
                case 0: try { ResKey.SetValue("mat_reducefillrate", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 1: try { ResKey.SetValue("mat_reducefillrate", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
            }

            // Запишем в реестр настройки отражений в воде (r_waterforceexpensive и r_waterforcereflectentities):
            switch (GT_WaterQuality.SelectedIndex)
            {
                case 0: // Simple reflections
                    try { ResKey.SetValue("r_waterforceexpensive", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("r_waterforcereflectentities", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 1: // Reflect world
                    try { ResKey.SetValue("r_waterforceexpensive", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("r_waterforcereflectentities", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 2: // Reflect all
                    try { ResKey.SetValue("r_waterforceexpensive", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("r_waterforcereflectentities", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
            }

            // Запишем в реестр настройки прорисовки теней (r_shadowrendertotexture):
            switch (GT_ShadowQuality.SelectedIndex)
            {
                case 0: try { ResKey.SetValue("r_shadowrendertotexture", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 1: try { ResKey.SetValue("r_shadowrendertotexture", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
            }

            // Запишем в реестр настройки коррекции цвета (mat_colorcorrection):
            switch (GT_ColorCorrectionT.SelectedIndex)
            {
                case 0: try { ResKey.SetValue("mat_colorcorrection", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 1: try { ResKey.SetValue("mat_colorcorrection", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
            }

            // Запишем в реестр настройки сглаживания (mat_antialias и mat_aaquality):
            switch (GT_AntiAliasing.SelectedIndex)
            {
                case 0: // Нет сглаживания
                    try { ResKey.SetValue("mat_antialias", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("mat_aaquality", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAA", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAAQuality", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 1: // 2x MSAA
                    try { ResKey.SetValue("mat_antialias", 2, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("mat_aaquality", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAA", 2, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAAQuality", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 2: // 4x MSAA
                    try { ResKey.SetValue("mat_antialias", 4, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("mat_aaquality", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAA", 4, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAAQuality", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 3: // 8x CSAA
                    try { ResKey.SetValue("mat_antialias", 4, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("mat_aaquality", 2, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAA", 4, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAAQuality", 2, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 4: // 16x CSAA
                    try { ResKey.SetValue("mat_antialias", 4, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("mat_aaquality", 4, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAA", 4, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAAQuality", 4, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 5: // 8x MSAA
                    try { ResKey.SetValue("mat_antialias", 8, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("mat_aaquality", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAA", 8, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAAQuality", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 6: // 16xQ CSAA
                    try { ResKey.SetValue("mat_antialias", 8, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("mat_aaquality", 2, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAA", 8, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("ScreenMSAAQuality", 2, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
            }

            // Запишем в реестр настройки фильтрации (mat_forceaniso):
            switch (GT_Filtering.SelectedIndex)
            {
                case 0: // Билинейная
                    try { ResKey.SetValue("mat_forceaniso", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("mat_trilinear", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 1: // Трилинейная
                    try { ResKey.SetValue("mat_forceaniso", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("mat_trilinear", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 2: // Анизотропная 2x
                    try { ResKey.SetValue("mat_forceaniso", 2, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("mat_trilinear", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 3: // Анизотропная 4x
                    try { ResKey.SetValue("mat_forceaniso", 4, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("mat_trilinear", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 4: // Анизотропная 8x
                    try { ResKey.SetValue("mat_forceaniso", 8, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("mat_trilinear", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 5: // Анизотропная 16x
                    try { ResKey.SetValue("mat_forceaniso", 16, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { ResKey.SetValue("mat_trilinear", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
            }

            // Запишем в реестр настройки вертикальной синхронизации (mat_vsync):
            switch (GT_VSync.SelectedIndex)
            {
                case 0: try { ResKey.SetValue("mat_vsync", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 1: try { ResKey.SetValue("mat_vsync", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
            }

            // Запишем в реестр настройки размытия движения (MotionBlur):
            switch (GT_MotionBlur.SelectedIndex)
            {
                case 0: try { ResKey.SetValue("MotionBlur", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 1: try { ResKey.SetValue("MotionBlur", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
            }

            // Запишем в реестр настройки режима DirectX (DXLevel_V1):
            switch (GT_DxMode.SelectedIndex)
            {
                case 0: // DirectX 8.0
                    try { ResKey.SetValue("DXLevel_V1", 80, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 1: // DirectX 8.1
                    try { ResKey.SetValue("DXLevel_V1", 81, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 2: // DirectX 9.0
                    try { ResKey.SetValue("DXLevel_V1", 90, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 3: // DirectX 9.0c
                    try { ResKey.SetValue("DXLevel_V1", 95, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
            }

            // Запишем в реестр настройки HDR (mat_hdr_level):
            switch (GT_HDR.SelectedIndex)
            {
                case 0: try { ResKey.SetValue("mat_hdr_level", 0, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 1: try { ResKey.SetValue("mat_hdr_level", 1, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
                case 2: try { ResKey.SetValue("mat_hdr_level", 2, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    break;
            }

            // Закрываем открытый ранее ключ реестра...
            ResKey.Close();
        }

        /// <summary>
        /// Сохраняет графические настройки игры в файл.
        /// </summary>
        /// <param name="VFileName">Имя файла опций</param>
        private void WriteNCFGameSettings(string VFileName)
        {
            // Начинаем сохранять содержимое редактора в файл...
            using (StreamWriter CFile = new StreamWriter(VFileName))
            {
                string Templt = "\t" + @"""" + "{0}" + @"""" + "\t\t" + @"""" + "{1}" + @"""";
                // Вставляем стандартный заголовок...
                CFile.WriteLine(@"""" + "VideoConfig" + @"""");
                CFile.WriteLine("{");
                // Вставляем параметры...
                // Обычные эффекты...
                CFile.WriteLine(String.Format(Templt, "setting.cpu_level", GT_NCF_EffectD.SelectedIndex.ToString()));
                // Шейдерные эффекты...
                CFile.WriteLine(String.Format(Templt, "setting.gpu_level", GT_NCF_ShaderE.SelectedIndex.ToString()));
                // Настройки сглаживания...
                switch (GT_NCF_AntiAlias.SelectedIndex)
                {
                    case 0: CFile.WriteLine(String.Format(Templt, "setting.mat_antialias", "1"));
                        CFile.WriteLine(String.Format(Templt, "setting.mat_aaquality", "0"));
                        break;
                    case 1: CFile.WriteLine(String.Format(Templt, "setting.mat_antialias", "2"));
                        CFile.WriteLine(String.Format(Templt, "setting.mat_aaquality", "0"));
                        break;
                    case 2: CFile.WriteLine(String.Format(Templt, "setting.mat_antialias", "4"));
                        CFile.WriteLine(String.Format(Templt, "setting.mat_aaquality", "0"));
                        break;
                    case 3: CFile.WriteLine(String.Format(Templt, "setting.mat_antialias", "4"));
                        CFile.WriteLine(String.Format(Templt, "setting.mat_aaquality", "2"));
                        break;
                    case 4: CFile.WriteLine(String.Format(Templt, "setting.mat_antialias", "4"));
                        CFile.WriteLine(String.Format(Templt, "setting.mat_aaquality", "4"));
                        break;
                    case 5: CFile.WriteLine(String.Format(Templt, "setting.mat_antialias", "8"));
                        CFile.WriteLine(String.Format(Templt, "setting.mat_aaquality", "0"));
                        break;
                    case 6: CFile.WriteLine(String.Format(Templt, "setting.mat_antialias", "8"));
                        CFile.WriteLine(String.Format(Templt, "setting.mat_aaquality", "2"));
                        break;
                }
                // Фильтрация...
                switch (GT_NCF_Filtering.SelectedIndex)
                {
                    case 0: CFile.WriteLine(String.Format(Templt, "setting.mat_forceaniso", "0"));
                        break;
                    case 1: CFile.WriteLine(String.Format(Templt, "setting.mat_forceaniso", "1"));
                        break;
                    case 2: CFile.WriteLine(String.Format(Templt, "setting.mat_forceaniso", "2"));
                        break;
                    case 3: CFile.WriteLine(String.Format(Templt, "setting.mat_forceaniso", "4"));
                        break;
                    case 4: CFile.WriteLine(String.Format(Templt, "setting.mat_forceaniso", "8"));
                        break;
                    case 5: CFile.WriteLine(String.Format(Templt, "setting.mat_forceaniso", "16"));
                        break;
                }
                // Вертикальная синхронизация...
                switch (GT_NCF_VSync.SelectedIndex)
                {
                    case 0: CFile.WriteLine(String.Format(Templt, "setting.mat_vsync", "0"));
                        CFile.WriteLine(String.Format(Templt, "setting.mat_triplebuffered", "0"));
                        break;
                    case 1: CFile.WriteLine(String.Format(Templt, "setting.mat_vsync", "1"));
                        CFile.WriteLine(String.Format(Templt, "setting.mat_triplebuffered", "0"));
                        break;
                    case 2: CFile.WriteLine(String.Format(Templt, "setting.mat_vsync", "1"));
                        CFile.WriteLine(String.Format(Templt, "setting.mat_triplebuffered", "1"));
                        break;
                }
                // Настройки зернистости...
                CFile.WriteLine(String.Format(Templt, "setting.mat_grain_scale_override", "1"));
                // Настройки гаммы...
                CFile.WriteLine(String.Format(Templt, "setting.mat_monitorgamma", (((float)GT_NCF_Brightness.Value / 10).ToString() + "00000")).Replace(",", "."));
                // Настройки качества моделей и текстур...
                CFile.WriteLine(String.Format(Templt, "setting.gpu_mem_level", GT_NCF_Quality.SelectedIndex.ToString()));
                // Настройки пула памяти...
                CFile.WriteLine(String.Format(Templt, "setting.mem_level", GT_NCF_MemPool.SelectedIndex.ToString()));
                // Настройки многоядерного рендеринга...
                switch (GT_NCF_Multicore.SelectedIndex)
                {
                    case 0: CFile.WriteLine(String.Format(Templt, "setting.mat_queue_mode", "0"));
                        break;
                    case 1: CFile.WriteLine(String.Format(Templt, "setting.mat_queue_mode", "-1"));
                        break;
                }
                // Настройки разрешения...
                CFile.WriteLine(String.Format(Templt, "setting.defaultres", GT_NCF_HorRes.Value.ToString()));
                CFile.WriteLine(String.Format(Templt, "setting.defaultresheight", GT_NCF_VertRes.Value.ToString()));
                // Настройки соотношения сторон...
                CFile.WriteLine(String.Format(Templt, "setting.aspectratiomode", GT_NCF_Ratio.SelectedIndex.ToString()));
                // Настройки режима...
                switch (GT_NCF_DispMode.SelectedIndex)
                {
                    case 0: CFile.WriteLine(String.Format(Templt, "setting.fullscreen", "1"));
                        CFile.WriteLine(String.Format(Templt, "setting.nowindowborder", "0"));
                        break;
                    case 1: CFile.WriteLine(String.Format(Templt, "setting.fullscreen", "0"));
                        CFile.WriteLine(String.Format(Templt, "setting.nowindowborder", "0"));
                        break;
                    case 2: CFile.WriteLine(String.Format(Templt, "setting.fullscreen", "1"));
                        CFile.WriteLine(String.Format(Templt, "setting.nowindowborder", "1"));
                        break;
                }
                // Завершающая скобка...
                CFile.WriteLine("}");
                // Закрываем файл...
                CFile.Close();
            }
        }

        /// <summary>
        /// Получает настройки GCF-игры из реестра и заполняет полученными
        /// данными страницу графического твикера.
        /// </summary>
        /// <param name="SAppName">Краткое имя игры</param>
        private void ReadGCFGameSettings(string SAppName)
        {
            // Открываем ключ реестра для чтения...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Valve", "Source", SAppName, "Settings"), false);

            // Проверяем открылся ли ключ...
            if (ResKey != null)
            {
                // Получаем значение разрешения по горизонтали...
                try { GT_ResHor.Value = Convert.ToInt32(ResKey.GetValue("ScreenWidth")); } catch { GT_ResHor.Value = 800; }

                // Получаем значение разрешения по вертикали...
                try { GT_ResVert.Value = Convert.ToInt32(ResKey.GetValue("ScreenHeight")); } catch { GT_ResVert.Value = 600; }

                // Получаем режим окна (ScreenWindowed): 1-window, 0-fullscreen...
                try { GT_ScreenType.SelectedIndex = Convert.ToInt32(ResKey.GetValue("ScreenWindowed")); } catch { GT_ScreenType.SelectedIndex = -1; }

                // Получаем детализацию моделей (r_rootlod): 0-high, 1-med, 2-low...
                try
                {
                    switch (Convert.ToInt32(ResKey.GetValue("r_rootlod")))
                    {
                        case 0: GT_ModelQuality.SelectedIndex = 2;
                            break;
                        case 1: GT_ModelQuality.SelectedIndex = 1;
                            break;
                        case 2: GT_ModelQuality.SelectedIndex = 0;
                            break;
                    }
                }
                catch
                {
                    GT_ModelQuality.SelectedIndex = -1;
                }

                // Получаем детализацию текстур (mat_picmip): 0-high, 1-med, 2-low...
                try
                {
                    switch (Convert.ToInt32(ResKey.GetValue("mat_picmip")))
                    {
                        case -1: GT_TextureQuality.SelectedIndex = 3;
                            break;
                        case 0: GT_TextureQuality.SelectedIndex = 2;
                            break;
                        case 1: GT_TextureQuality.SelectedIndex = 1;
                            break;
                        case 2: GT_TextureQuality.SelectedIndex = 0;
                            break;
                    }
                }
                catch
                {
                    GT_TextureQuality.SelectedIndex = -1;
                }

                // Получаем настройки шейдеров (mat_reducefillrate): 0-high, 1-low...
                try
                {
                    switch (Convert.ToInt32(ResKey.GetValue("mat_reducefillrate")))
                    {
                        case 0: GT_ShaderQuality.SelectedIndex = 1;
                            break;
                        case 1: GT_ShaderQuality.SelectedIndex = 0;
                            break;
                    }
                }
                catch
                {
                    GT_ShaderQuality.SelectedIndex = -1;
                }

                // Начинаем работать над отражениями (здесь сложнее)...
                try
                {
                    switch (Convert.ToInt32(ResKey.GetValue("r_waterforceexpensive")))
                    {
                        case 0: GT_WaterQuality.SelectedIndex = 0;
                            break;
                        case 1:
                            switch (Convert.ToInt32(ResKey.GetValue("r_waterforcereflectentities")))
                            {
                                case 0: GT_WaterQuality.SelectedIndex = 1;
                                    break;
                                case 1: GT_WaterQuality.SelectedIndex = 2;
                                    break;
                            }
                            break;
                    }
                }
                catch
                {
                    GT_WaterQuality.SelectedIndex = -1;
                }

                // Получаем настройки теней (r_shadowrendertotexture): 0-low, 1-high...
                try { GT_ShadowQuality.SelectedIndex = Convert.ToInt32(ResKey.GetValue("r_shadowrendertotexture")); } catch { GT_ShadowQuality.SelectedIndex = -1; }

                // Получаем настройки коррекции цвета (mat_colorcorrection): 0-off, 1-on...
                try { GT_ColorCorrectionT.SelectedIndex = Convert.ToInt32(ResKey.GetValue("mat_colorcorrection")); } catch { GT_ColorCorrectionT.SelectedIndex = -1; }

                // Получаем настройки сглаживания (mat_antialias): 1-off, 2-2x, 4-4x, etc...
                // 2x MSAA - 2:0; 4xMSAA - 4:0; 8xCSAA - 4:2; 16xCSAA - 4:4; 8xMSAA - 8:0; 16xQ CSAA - 8:2.
                try
                {
                    switch (Convert.ToInt32(ResKey.GetValue("mat_antialias")))
                    {
                        case 0: GT_AntiAliasing.SelectedIndex = 0;
                            break;
                        case 1: GT_AntiAliasing.SelectedIndex = 0;
                            break;
                        case 2: GT_AntiAliasing.SelectedIndex = 1;
                            break;
                        case 4:
                            switch (Convert.ToInt32(ResKey.GetValue("mat_aaquality")))
                            {
                                case 0: GT_AntiAliasing.SelectedIndex = 2;
                                    break;
                                case 2: GT_AntiAliasing.SelectedIndex = 3;
                                    break;
                                case 4: GT_AntiAliasing.SelectedIndex = 4;
                                    break;
                            }
                            break;
                        case 8:
                            switch (Convert.ToInt32(ResKey.GetValue("mat_aaquality")))
                            {
                                case 0: GT_AntiAliasing.SelectedIndex = 5;
                                    break;
                                case 2: GT_AntiAliasing.SelectedIndex = 6;
                                    break;
                            }
                            break;
                    }
                }
                catch
                {
                    GT_AntiAliasing.SelectedIndex = -1;
                }

                // Получаем настройки анизотропии (mat_forceaniso): 1-off, etc...
                try
                {
                    switch (Convert.ToInt32(ResKey.GetValue("mat_forceaniso")))
                    {
                        case 1:
                            switch (Convert.ToInt32(ResKey.GetValue("mat_trilinear")))
                            {
                                case 0: GT_Filtering.SelectedIndex = 0;
                                    break;
                                case 1: GT_Filtering.SelectedIndex = 1;
                                    break;
                            }
                            break;
                        case 2: GT_Filtering.SelectedIndex = 2;
                            break;
                        case 4: GT_Filtering.SelectedIndex = 3;
                            break;
                        case 8: GT_Filtering.SelectedIndex = 4;
                            break;
                        case 16: GT_Filtering.SelectedIndex = 5;
                            break;
                    }
                }
                catch
                {
                    GT_Filtering.SelectedIndex = -1;
                }

                // Получаем настройки вертикальной синхронизации (mat_vsync): 0-off, 1-on...
                try
                {
                    switch (Convert.ToInt32(ResKey.GetValue("mat_vsync")))
                    {
                        case 0: GT_VSync.SelectedIndex = 0;
                            break;
                        case 1: GT_VSync.SelectedIndex = 1;
                            break;
                    }
                }
                catch
                {
                    GT_VSync.SelectedIndex = -1;
                }

                // Получаем настройки размытия движения (MotionBlur): 0-off, 1-on...
                try
                {
                    switch (Convert.ToInt32(ResKey.GetValue("MotionBlur")))
                    {
                        case 0: GT_MotionBlur.SelectedIndex = 0;
                            break;
                        case 1: GT_MotionBlur.SelectedIndex = 1;
                            break;
                    }
                }
                catch
                {
                    GT_MotionBlur.SelectedIndex = -1;
                }

                // Получаем настройки режима рендера (DXLevel_V1):
                // 80-DirectX 8.0; 81-DirectX 8.1; 90-DirectX 9.0; 95-DirectX 9.0c...
                try
                {
                    switch (Convert.ToInt32(ResKey.GetValue("DXLevel_V1")))
                    {
                        case 80: GT_DxMode.SelectedIndex = 0;
                            break;
                        case 81: GT_DxMode.SelectedIndex = 1;
                            break;
                        case 90: GT_DxMode.SelectedIndex = 2;
                            break;
                        case 95: GT_DxMode.SelectedIndex = 3;
                            break;
                    }
                }
                catch
                {
                    GT_DxMode.SelectedIndex = -1;
                }

                // Получаем настройки HDR (mat_hdr_level): 0-off, 1-bloom, 2-full...
                try
                {
                    switch (Convert.ToInt32(ResKey.GetValue("mat_hdr_level")))
                    {
                        case 0: GT_HDR.SelectedIndex = 0;
                            break;
                        case 1: GT_HDR.SelectedIndex = 1;
                            break;
                        case 2: GT_HDR.SelectedIndex = 2;
                            break;
                    }
                }
                catch
                {
                    GT_HDR.SelectedIndex = -1;
                }

                // Закрываем ключ реестра...
                ResKey.Close();
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("GT_RegOpenErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Получает настройки NCF-игры из файла и заполняет ими таблицу
        /// графического твикера программы.
        /// </summary>
        /// <param name="VFileName">Путь к файлу с настройками</param>
        private void ReadNCFGameSettings(string VFileName)
        {
            // Получаем значение разрешения по горизонтали...
            try
            {
                GT_NCF_HorRes.Value = CoreLib.GetNCFDWord("setting.defaultres", VFileName);
            }
            catch
            {
                GT_NCF_HorRes.Value = 800;
            }
            // Получаем значение разрешения по вертикали...
            try
            {
                GT_NCF_VertRes.Value = CoreLib.GetNCFDWord("setting.defaultresheight", VFileName);
            }
            catch
            {
                GT_NCF_VertRes.Value = 600;
            }
            // Получаем настройки соотношения сторон...
            try
            {
                GT_NCF_Ratio.SelectedIndex = CoreLib.GetNCFDWord("setting.aspectratiomode", VFileName);
            }
            catch
            {
                GT_NCF_Ratio.SelectedIndex = -1;
            }
            // Получаем настройки яркости...
            try
            {
                GT_NCF_Brightness.Value = Convert.ToInt32(CoreLib.GetNCFDble("setting.mat_monitorgamma", VFileName) * 10);
            }
            catch
            {
                GT_NCF_Brightness.Value = 18;
            }
            // Получаем настройки режима...
            try
            {
                switch (CoreLib.GetNCFDWord("setting.fullscreen", VFileName))
                {
                    case 0:
                        switch (CoreLib.GetNCFDWord("setting.nowindowborder", VFileName))
                        {
                            case 0: GT_NCF_DispMode.SelectedIndex = 1;
                                break;
                            case 1: GT_NCF_DispMode.SelectedIndex = 2;
                                break;
                        }
                        break;
                    case 1: GT_NCF_DispMode.SelectedIndex = 0;
                        break;
                }
            }
            catch
            {
                GT_NCF_DispMode.SelectedIndex = -1;
            }
            // Получаем настройки сглаживания текстур...
            try
            {
                switch (CoreLib.GetNCFDWord("setting.mat_antialias", VFileName))
                {
                    case 0: GT_NCF_AntiAlias.SelectedIndex = 0;
                        break;
                    case 1: GT_NCF_AntiAlias.SelectedIndex = 0;
                        break;
                    case 2: GT_NCF_AntiAlias.SelectedIndex = 1;
                        break;
                    case 4:
                        switch (CoreLib.GetNCFDWord("setting.mat_aaquality", VFileName))
                        {
                            case 0: GT_NCF_AntiAlias.SelectedIndex = 2;
                                break;
                            case 2: GT_NCF_AntiAlias.SelectedIndex = 3;
                                break;
                            case 4: GT_NCF_AntiAlias.SelectedIndex = 4;
                                break;
                        }
                        break;
                    case 8:
                        switch (CoreLib.GetNCFDWord("setting.mat_aaquality", VFileName))
                        {
                            case 0: GT_NCF_AntiAlias.SelectedIndex = 5;
                                break;
                            case 2: GT_NCF_AntiAlias.SelectedIndex = 6;
                                break;
                        }
                        break;
                }
            }
            catch
            {
                GT_NCF_AntiAlias.SelectedIndex = -1;
            }
            // Получаем настройки фильтрации текстур...
            try
            {
                switch (CoreLib.GetNCFDWord("setting.mat_forceaniso", VFileName))
                {
                    case 0: GT_NCF_Filtering.SelectedIndex = 0;
                        break;
                    case 1: GT_NCF_Filtering.SelectedIndex = 1;
                        break;
                    case 2: GT_NCF_Filtering.SelectedIndex = 2;
                        break;
                    case 4: GT_NCF_Filtering.SelectedIndex = 3;
                        break;
                    case 8: GT_NCF_Filtering.SelectedIndex = 4;
                        break;
                    case 16: GT_NCF_Filtering.SelectedIndex = 5;
                        break;
                }
            }
            catch
            {
                GT_NCF_Filtering.SelectedIndex = -1;
            }
            // Получаем настройки вертикальной синхронизации...
            try
            {
                switch (CoreLib.GetNCFDWord("setting.mat_vsync", VFileName))
                {
                    case 0: GT_NCF_VSync.SelectedIndex = 0;
                        break;
                    case 1:
                        switch (CoreLib.GetNCFDWord("setting.mat_triplebuffered", VFileName))
                        {
                            case 0: GT_NCF_VSync.SelectedIndex = 1;
                                break;
                            case 1: GT_NCF_VSync.SelectedIndex = 2;
                                break;
                        }
                        break;
                }
            }
            catch
            {
                GT_NCF_VSync.SelectedIndex = -1;
            }
            // Получаем настройки многоядерного рендеринга...
            try
            {
                switch (CoreLib.GetNCFDWord("setting.mat_queue_mode", VFileName))
                {
                    case -1: GT_NCF_Multicore.SelectedIndex = 1;
                        break;
                    case 0: GT_NCF_Multicore.SelectedIndex = 0;
                        break;
                    case 1: GT_NCF_Multicore.SelectedIndex = 1;
                        break;
                    case 2: GT_NCF_Multicore.SelectedIndex = 1;
                        break;
                }
            }
            catch
            {
                GT_NCF_Multicore.SelectedIndex = -1;
            }
            // Получаем настройки качества шейдерных эффектов...
            try
            {
                GT_NCF_ShaderE.SelectedIndex = CoreLib.GetNCFDWord("setting.gpu_level", VFileName);
            }
            catch
            {
                GT_NCF_ShaderE.SelectedIndex = -1;
            }
            // Получаем настройки эффектов...
            try
            {
                GT_NCF_EffectD.SelectedIndex = CoreLib.GetNCFDWord("setting.cpu_level", VFileName);
            }
            catch
            {
                GT_NCF_EffectD.SelectedIndex = -1;
            }
            // Получаем настройки пула памяти...
            try
            {
                GT_NCF_MemPool.SelectedIndex = CoreLib.GetNCFDWord("setting.mem_level", VFileName);
            }
            catch
            {
                GT_NCF_MemPool.SelectedIndex = -1;
            }
            // Получаем настройки качества моделей и текстур...
            try
            {
                GT_NCF_Quality.SelectedIndex = CoreLib.GetNCFDWord("setting.gpu_mem_level", VFileName);
            }
            catch
            {
                GT_NCF_Quality.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Открывает конфиг, имя которого передано в качестве параметра
        /// и заполняет им Редактор конфигов с одноимённой страницы.
        /// </summary>
        /// <param name="ConfFileName">Полный путь к файлу конфига</param>
        private void ReadConfigFromFile(string ConfFileName)
        {
            string Buf = ConfFileName; // Получаем имя файла с полным путём...
            string ImpStr; // Строка для парсинга...
            string CVarName, CVarContent;
            if (File.Exists(Buf)) // Проверяем, существует ли файл...
            {
                // Файл существует. Продолжаем...
                CFGFileName = Path.GetFileName(Buf); // Получаем имя открытого в Редакторе файла без пути...
                if (CFGFileName == "config.cfg") // Проверяем, не открыл ли пользователь файл config.cfg и, если да, то сообщаем об этом...
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("CE_RestConfigOpenWarn"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                CE_Editor.Rows.Clear(); // Очищаем область редактирования...
                try
                {
                    using (StreamReader OpenedConfig = new StreamReader(@Buf, Encoding.Default)) // Открываем файл таким способом...
                    {
                        // Будем читать поток построчно...
                        while (OpenedConfig.Peek() >= 0)
                        {
                            // Начинаем работу...
                            ImpStr = OpenedConfig.ReadLine(); // считали строку...
                            ImpStr = ImpStr.Trim(); // почистим строку от лишних пробелов...
                            // Начинаем парсить считанную строку...
                            if (!(String.IsNullOrEmpty(ImpStr))) // проверяем, не пустая ли строка...
                            {
                                if ((ImpStr[1] != '/') && (ImpStr.Length >= 4) && (ImpStr.Substring(0, 4) != "echo")) // проверяем, не комментарий ли или сообщение...
                                {
                                    // Почистим строку от лишних пробелов и табуляций...
                                    ImpStr = CoreLib.CleanStrWx(ImpStr);

                                    // Строка почищена, продолжаем...
                                    if (ImpStr.IndexOf(" ") != -1)
                                    {
                                        Buf = ImpStr.Substring(0, ImpStr.IndexOf(" ")); // мы получили переменную...
                                        ImpStr = ImpStr.Remove(0, ImpStr.IndexOf(" ") + 1); // удаляем полученное...
                                        //ImpStr = ImpStr.Replace(Buf, "");
                                        // Buf теперь содержит всё до пробела. Нужно чистить...
                                        if ((Buf.IndexOf("/") == -1) && (Buf.IndexOf(" ") == -1) && (Buf != ""))
                                        {
                                            CVarName = Buf; // заполняем имя переменной...
                                            // Отлично, имя переменной мы получили и храним в CVarName. Осталось получить значение...
                                            if (ImpStr.IndexOf("//") != -1) // ищем в строке комментарии...
                                            {
                                                Buf = ImpStr.Substring(0, ImpStr.IndexOf("/") - 1); // копируем всё до комментария...
                                                CVarContent = Buf; // возвращаем значение...
                                            }
                                            else
                                            {
                                                CVarContent = ImpStr; // комментариев нет, сразу возвращаем значение...
                                            }
                                            // Пишем в нашу таблицу...
                                            CE_Editor.Rows.Add(CVarName, CVarContent);
                                        }
                                    }
                                    else
                                    {
                                        CE_Editor.Rows.Add(ImpStr, "");
                                    }
                                }
                            }
                        }
                    }
                    SB_Status.Text = CoreLib.GetLocalizedString("StatusOpenedFile") + " " + CFGFileName;
                }
                catch (Exception Ex)
                {
                    // Произошло исключение...
                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("CE_ExceptionDetected"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("CE_OpenFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Считывает файлы резервных копий из указанного каталога и помещает в таблицу.
        /// </summary>
        /// <param name="BUpDir">Путь к каталогу с резервными копиями</param>
        private void ReadBackUpList2Table(string BUpDir)
        {
            // Считаем список резервных копий и заполним таблицу...
            // Очистим таблицу...
            if (BU_LVTable.InvokeRequired) { this.Invoke((MethodInvoker)delegate() { BU_LVTable.Items.Clear(); }); } else { BU_LVTable.Items.Clear(); }
            // Открываем каталог...
            DirectoryInfo DInfo = new DirectoryInfo(BUpDir);
            // Считываем список файлов по заданной маске...
            FileInfo[] DirList = DInfo.GetFiles("*.*");
            // Инициализируем буферные переменные...
            string Buf, BufName;
            // Начинаем обход массива...
            foreach (FileInfo DItem in DirList)
            {
                // Обрабатываем найденное...
                BufName = Path.GetFileNameWithoutExtension(DItem.Name);
                switch (DItem.Extension)
                {
                    case ".reg":
                        // Бэкап реестра...
                        Buf = CoreLib.GetLocalizedString("BU_BType_Reg");
                        // Заполним человеческое описание...
                        if (BufName.IndexOf("Game_Options") != -1)
                        {
                            BufName = CoreLib.GetLocalizedString("BU_BName_GRGame") + " " + DItem.CreationTime;
                        }
                        if (BufName.IndexOf("Source_Options") != -1)
                        {
                            BufName = CoreLib.GetLocalizedString("BU_BName_SRCAll") + " " + DItem.CreationTime;
                        }
                        if (BufName.IndexOf("Steam_BackUp") != -1)
                        {
                            BufName = CoreLib.GetLocalizedString("BU_BName_SteamAll") + " " + DItem.CreationTime;
                        }
                        if (BufName.IndexOf("Game_AutoBackUp") != -1)
                        {
                            BufName = CoreLib.GetLocalizedString("BU_BName_GameAuto") + " " + DItem.CreationTime;
                        }
                        break;
                    case ".bud":
                        Buf = CoreLib.GetLocalizedString("BU_BType_Cont");
                        if (BufName.IndexOf("Container") != -1)
                        {
                            BufName = CoreLib.GetLocalizedString("BU_BName_Bud") + " " + DItem.CreationTime;
                        }
                        break;
                    default:
                        switch (Path.GetExtension(Path.GetFileNameWithoutExtension(DItem.FullName)))
                        {
                            case ".cfg":
                                Buf = CoreLib.GetLocalizedString("BU_BType_Cfg");
                                break;
                            case ".txt":
                                Buf = CoreLib.GetLocalizedString("BU_BType_Video");
                                break;
                            default: Buf = CoreLib.GetLocalizedString("BU_BType_Cfg");
                                break;
                        }
                        break;
                }
                // Добавляем в таблицу...
                ListViewItem LvItem = new ListViewItem(BufName);
                LvItem.SubItems.Add(Buf);
                LvItem.SubItems.Add(CoreLib.SclBytes(DItem.Length));
                LvItem.SubItems.Add(CoreLib.WriteDateToString(DItem.CreationTime, false));
                LvItem.SubItems.Add(DItem.Name);
                if (BU_LVTable.InvokeRequired) { this.Invoke((MethodInvoker)delegate() { BU_LVTable.Items.Add(LvItem); }); } else { BU_LVTable.Items.Add(LvItem); }
            }
        }

        /// <summary>
        /// Обнуляет контролы на странице графических настроек. Функция-заглушка.
        /// </summary>
        private void NullGraphOptions()
        {
            GT_NCF_HorRes.Value = 640;
            GT_NCF_VertRes.Value = 480;
            GT_NCF_Brightness.Value = 18;
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
        /// Переключает вид страницы графического твикера с GCF на NCF приложение
        /// и наоборот.
        /// </summary>
        /// <param name="GCFGame">Тип управляемого приложения</param>
        private void SetGTOptsType(bool GCFGame)
        {
            GT_GCF_Group.Visible = GCFGame;
            GT_NCF_Group.Visible = !GCFGame;
        }

        /// <summary>
        /// Изменяет вид значка и текст безопасной очистки в соответствии с её статусом
        /// в строке состояния программы.
        /// </summary>
        private void CheckSafeClnStatus()
        {
            if (Properties.Settings.Default.SafeCleanup)
            {
                SB_App.Text = CoreLib.GetLocalizedString("AppSafeClnStTextOn");
                SB_App.Image = Properties.Resources.green_circle;
            }
            else
            {
                SB_App.Text = CoreLib.GetLocalizedString("AppSafeClnStTextOff");
                SB_App.Image = Properties.Resources.red_circle;
            }
        }

        /// <summary>
        /// Требует указать путь к Steam вручную при невозможности автоопределения.
        /// </summary>
        private string GetPathByMEnter()
        {
            string Result = null;
            FldrBrwse.Description = CoreLib.GetLocalizedString("SteamPathEnterText"); // Указываем текст в диалоге поиска каталога...
            if (FldrBrwse.ShowDialog() == DialogResult.OK) // Отображаем стандартный диалог поиска каталога...
            {
                if (!(File.Exists(Path.Combine(FldrBrwse.SelectedPath, Properties.Resources.SteamExecBin))))
                {
                    throw new FileNotFoundException("Invalid Steam directory entered by user", Path.Combine(FldrBrwse.SelectedPath, Properties.Resources.SteamExecBin));
                }
                else
                {
                    Result = FldrBrwse.SelectedPath;
                }
            }
            else
            {
                throw new OperationCanceledException("User closed opendir window");
            }
            return Result;
        }

        /// <summary>
        /// Проверяет значение OldPath на наличие верного пути к клиенту Steam.
        /// </summary>
        /// <param name="OldPath">Проверяемый путь</param>
        private string CheckLastSteamPath(string OldPath)
        {
            return (!(String.IsNullOrWhiteSpace(OldPath)) && File.Exists(Path.Combine(OldPath, Properties.Resources.SteamExecBin))) ? OldPath : GetPathByMEnter();
        }

        /// <summary>
        /// Получает путь и обрабатывает возможные исключения.
        /// </summary>
        private void ValidateAndHandle()
        {
            try
            {
                GV.FullSteamPath = CheckLastSteamPath(Properties.Settings.Default.LastSteamPath);
            }
            catch (FileNotFoundException Ex)
            {
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("SteamPathEnterErr"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
                Environment.Exit(7);
            }
            catch (OperationCanceledException Ex)
            {
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("SteamPathCancel"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
                Environment.Exit(19);
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("AppGenericError"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
                Environment.Exit(24);
            }
        }

        /// <summary>
        /// Устанавливает статус элементам управления, зависящим от платформы или спец. прав.
        /// </summary>
        /// <param name="State">Устанавливаемый статус</param>
        private void ChangePrvControlState(bool State)
        {
            PS_CleanRegistry.Enabled = State;
            PS_SteamLang.Enabled = State;
            //MNUHEd.Enabled = State;
            MNUUpdateCheck.Enabled = State;
            MNUWinMnuDisabler.Enabled = State;
            MNUUpGameDB.Enabled = State;
        }

        /// <summary>
        /// Восстанавливает файл резервной копии согласно правилам в указанный каталог.
        /// </summary>
        /// <param name="FName">Имя файла резервной копии</param>
        /// <param name="DestDirW">Каталог назначения</param>
        private void RestoreBackUpFile(string FName, string DestDirW)
        {
            // Сохраняем оригинальное имя файла резервной копии для функции копирования...
            string OrigName = FName;
            // Отбрасываем двойное расширение...
            FName = Path.GetFileNameWithoutExtension(FName);
            try
            {
                // Копируем файл...
                File.Copy(Path.Combine(GV.FullBackUpDirPath, OrigName), Path.Combine(DestDirW, FName), true);
                // Если восстановили autoexec.cfg, отображаем значок на странице графического твикера...
                if (FName == "autoexec.cfg") { GT_Warning.Visible = true; }
                // Показываем сообщение об успешном восстановлении...
                MessageBox.Show(CoreLib.GetLocalizedString("BU_RestSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex)
            {
                // Произошло исключение...
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("BU_RestFailed"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Выполняет действия для данной платформы.
        /// </summary>
        private void DoPlatformRoutines()
        {
            // Проверяем, запущена ли программа с правами администратора...
            if (!(CoreLib.IsCurrentUserAdmin()))
            {
                // Программа запущена с правами пользователя, поэтому принимаем меры...
                // Выводим сообщение об этом...
                if (Properties.Settings.Default.AllowNonAdmDialog)
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("AppLaunchedNotAdmin"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Properties.Settings.Default.AllowNonAdmDialog = false;
                }
                // Блокируем контролы, требующие для своей работы прав админа...
                ChangePrvControlState(false);
            }

            // Узнаем путь к установленному клиенту Steam...
            try { GV.FullSteamPath = CoreLib.GetSteamPath(); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); ValidateAndHandle(); }
        }

        /// <summary>
        /// Выполняет определение и вывод названия файловой системы на диске установки клиента Steam.
        /// </summary>
        /// <param name="SteamPath">Каталог установки Steam</param>
        private void DetectFS(string SteamPath)
        {
            try
            {
                PS_OSDrive.Text = String.Format(PS_OSDrive.Text, CoreLib.DetectDriveFileSystem(Path.GetPathRoot(SteamPath)));
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
                        CoreLib.WriteStringToLog(String.Format(CoreLib.GetLocalizedString("AppNoGamesDLog"), GV.FullSteamPath));
                        // Нет, не нашлись, выведем сообщение...
                        MessageBox.Show(CoreLib.GetLocalizedString("AppNoGamesDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Завершим работу приложения...
                        Environment.Exit(11);
                    }
                    break;
                case 1:
                    {
                        // При наличии единственной игры в списке, выберем её автоматически...
                        AppSelector.SelectedIndex = 0;
                        SB_Status.Text = CoreLib.GetLocalizedString("StatusNormal");
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
        /// <param name="SteamPath">Каталог установки Steam</param>
        private void CheckSymbols(string SteamPath)
        {
            if (!(CoreLib.CheckNonASCII(SteamPath)))
            {
                // Запрещённые символы найдены!
                PS_PathDetector.Text = CoreLib.GetLocalizedString("SteamNonASCIITitle");
                PS_PathDetector.ForeColor = Color.Red;
                MessageBox.Show(CoreLib.GetLocalizedString("SteamNonASCIIDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CoreLib.WriteStringToLog(String.Format(CoreLib.GetLocalizedString("AppRestrSymbLog"), GV.FullSteamPath));
            }
        }

        #endregion

        #region Internal Workers
        
        private void BW_UpChk_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Вычисляем разницу между текущей датой и датой последнего обновления...
                TimeSpan TS = DateTime.Now - Properties.Settings.Default.LastUpdateTime;
                if (TS.Days >= 7) // Проверяем не прошла ли неделя с момента последней прверки...
                {
                    // Требуется проверка обновлений...
                    if (CoreLib.AutoUpdateCheck(GV.AppVersionInfo, Properties.Settings.Default.UpdateChURI))
                    {
                        // Доступны обновления...
                        MessageBox.Show(String.Format(CoreLib.GetLocalizedString("AppUpdateAvailable"), GV.AppName), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void BW_FPRecv_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Открываем каталог...
                DirectoryInfo DInfo = new DirectoryInfo(Path.Combine(GV.FullAppPath, "cfgs"));
                // Считываем список файлов по заданной маске...
                FileInfo[] DirList = DInfo.GetFiles("*.cfg");
                // Начинаем обход массива...
                foreach (FileInfo DItem in DirList)
                {
                    // Обрабатываем найденное...
                    if (DItem.Name != "config_default.cfg")
                    {
                        if (FP_ConfigSel.InvokeRequired)
                        {
                            this.Invoke((MethodInvoker)delegate() { FP_ConfigSel.Items.Add((string)DItem.Name); });
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                // FPS-конфигов для выбранного приложения не найдено.
                // Запишем в лог...
                CoreLib.WriteStringToLog(Ex.Message);
                // Выводим текст об этом...
                FP_Description.Text = CoreLib.GetLocalizedString("FP_NoCfgGame");
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
                FP_Description.Text = CoreLib.GetLocalizedString("FP_SelectFromList");
                FP_Description.ForeColor = Color.Black;
                FP_ConfigSel.Enabled = true;
            }
        }

        private void BW_BkUpRecv_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ReadBackUpList2Table(GV.FullBackUpDirPath);
            }
            catch (Exception Ex)
            {
                CoreLib.WriteStringToLog(Ex.Message);
                Directory.CreateDirectory(GV.FullBackUpDirPath);
            }
        }

        #endregion

        private void frmMainW_Load(object sender, EventArgs e)
        {
            // Событие инициализации формы...

            // Получаем путь к каталогу приложения...
            Assembly Assmbl = Assembly.GetEntryAssembly();
            GV.FullAppPath = Path.GetDirectoryName(Assmbl.Location);

            // Укажем путь к пользовательским данным и создадим если не существует...
            GV.AppUserDir = Properties.Settings.Default.IsPortable ? Path.Combine(GV.FullAppPath, "portable") : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GV.AppName);
            
            // Проверим существование каталога пользовательских данных и при необходимости создадим...
            if (!(Directory.Exists(GV.AppUserDir)))
            {
                Directory.CreateDirectory(GV.AppUserDir);
            }

            // Начинаем платформо-зависимые процедуры...
            DoPlatformRoutines();

            // При работе отладочной версии запишем в лог путь к найденному Steam...
            #if DEBUG
            CoreLib.WriteStringToLog(String.Format(CoreLib.GetLocalizedString("AppSteamLocLog"), GV.FullSteamPath));
            #endif

            // Сохраним последний путь к Steam в файл конфигурации...
            Properties.Settings.Default.LastSteamPath = GV.FullSteamPath;

            // Получаем информацию о версии нашего приложения...
            GV.AppVersionInfo = Assmbl.GetName().Version.ToString();

            // Вставляем информацию о версии в заголовок формы...
            //this.Text += " (version " + GV.AppVersionInfo + ")";
            #if DEBUG
            this.Text = String.Format(this.Text, GV.AppName, Properties.Resources.PlatformFriendlyName, GV.AppVersionInfo + " (debug)", Assmbl.GetName().ProcessorArchitecture.ToString());
            #else
            this.Text = String.Format(this.Text, GV.AppName, Properties.Resources.PlatformFriendlyName, GV.AppVersionInfo, Assmbl.GetName().ProcessorArchitecture.ToString());
            #endif

            // Генерируем User-Agent для SRC Repair...
            GV.UserAgent = String.Format(Properties.Resources.AppDefUA, Properties.Resources.PlatformFriendlyName, GV.AppVersionInfo, GV.AppName, Assmbl.GetName().ProcessorArchitecture.ToString());

            // Найдём и завершим в памяти процесс Steam...
            CoreLib.ProcessTerminate("Steam", CoreLib.GetLocalizedString("ST_KillMessage"));

            // Применим некоторые настройки...
            AppSelector.Sorted = Properties.Settings.Default.SortGamesList;

            // Укажем статус Безопасной очистки...
            CheckSafeClnStatus();

            // Выводим сообщение в строку статуса...
            SB_Status.Text = CoreLib.GetLocalizedString("StatusSLogin");

            // Укажем путь к Steam на странице "Устранение проблем"...
            PS_StPath.Text = String.Format(PS_StPath.Text, GV.FullSteamPath);
            
            // Проверим на наличие запрещённых символов в пути к установленному клиенту Steam...
            CheckSymbols(GV.FullSteamPath);

            // Распознаем файловую систему на диске со Steam...
            DetectFS(GV.FullSteamPath);

            // Начинаем определять установленные игры...
            try
            {
                DetectInstalledGames(GV.FullSteamPath, Properties.Resources.SteamAppsFolderName);
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("AppXMLParseError"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
                Environment.Exit(16);
            }

            // Проверим нашлись ли игры...
            CheckGames(AppSelector.Items.Count);

            try
            {
                // Проверим наличие обновлений программы (если разрешено в настройках)...
                if (Properties.Settings.Default.EnableAutoUpdate && (Properties.Settings.Default.LastUpdateTime != null))
                {
                    if (!BW_UpChk.IsBusy) { BW_UpChk.RunWorkerAsync(); }
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void PS_CleanBlobs_CheckedChanged(object sender, EventArgs e)
        {
            PS_ExecuteNow.Enabled = PS_CleanBlobs.Checked || PS_CleanRegistry.Checked;
        }

        private void PS_CleanRegistry_CheckedChanged(object sender, EventArgs e)
        {
            // Включаем список с доступными языками клиента Steam...
            PS_SteamLang.Enabled = PS_CleanRegistry.Checked;

            // Выбираем язык по умолчанию согласно языку приложения...
            PS_SteamLang.SelectedIndex = Convert.ToInt32(CoreLib.GetLocalizedString("AppDefaultSteamLangID"));

            PS_ExecuteNow.Enabled = PS_CleanRegistry.Checked || PS_CleanBlobs.Checked;
        }

        private void PS_ExecuteNow_Click(object sender, EventArgs e)
        {
            // Начинаем очистку...

            // Запрашиваем подтверждение у пользователя...
            if (MessageBox.Show(CoreLib.GetLocalizedString("PS_ExecuteMSG"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Подтверждение получено...
                if ((PS_CleanBlobs.Checked) || (PS_CleanRegistry.Checked))
                {
                    // Найдём и завершим работу клиента Steam...
                    if (CoreLib.ProcessTerminate("Steam") != 0)
                    {
                        MessageBox.Show(CoreLib.GetLocalizedString("PS_ProcessDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Проверяем нужно ли чистить блобы...
                    if (PS_CleanBlobs.Checked)
                    {
                        try
                        {
                            // Чистим блобы...
                            CleanBlobsNow(GV.FullSteamPath);
                        }
                        catch (Exception Ex)
                        {
                            CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("PS_CleanException"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
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
                                CleanRegistryNow(PS_SteamLang.SelectedIndex);
                            }
                            else
                            {
                                // Пользователь не выбрал язык, поэтому будем использовать английский...
                                MessageBox.Show(CoreLib.GetLocalizedString("PS_NoLangSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                CleanRegistryNow(0);
                            }
                        }
                        catch (Exception Ex)
                        {
                            CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("PS_CleanException"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                        }
                    }

                    // Выполнение закончено, выведем сообщение о завершении...
                    MessageBox.Show(CoreLib.GetLocalizedString("PS_SeqCompleted"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Запустим Steam...
                    if (File.Exists(Path.Combine(GV.FullSteamPath, Properties.Resources.SteamExecBin))) { Process.Start(Path.Combine(GV.FullSteamPath, Properties.Resources.SteamExecBin)); }
                }
            }
        }

        private void frmMainW_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (Properties.Settings.Default.ConfirmExit)
                {
                    // Запрашиваем подтверждение у пользователя на закрытие формы...
                    e.Cancel = !(MessageBox.Show(String.Format(CoreLib.GetLocalizedString("FrmCloseQuery"), GV.AppName), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
                }
            }
        }

        private void AppSelector_SelectedIndexChanged(object sender, EventArgs e)
        {            
            // Начинаем определять нужные нам значения переменных...
            try
            {
                XmlDocument XMLD = new XmlDocument();
                FileStream XMLFS = new FileStream(Path.Combine(GV.FullAppPath, Properties.Settings.Default.GameListFile), FileMode.Open, FileAccess.Read);
                XMLD.Load(XMLFS);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("Game");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    if (XMLD.GetElementsByTagName("DirName")[i].InnerText.ToLower() == Path.GetFileName(AppSelector.Text))
                    {
                        GV.FullAppName = XMLD.GetElementsByTagName("DirName")[i].InnerText;
                        GV.SmallAppName = XMLD.GetElementsByTagName("SmallName")[i].InnerText;
                        GV.GameInternalID = XMLD.GetElementsByTagName("SID")[i].InnerText;
                        GV.ConfDir = XMLD.GetElementsByTagName("VFDir")[i].InnerText;
                        GV.IsUsingVideoFile = XMLD.GetElementsByTagName("HasVF")[i].InnerText == "1";
                        break;
                    }
                }
                XMLFS.Close();
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("AppXMLParseError"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
            }

            // Генерируем полный путь до каталога управляемого приложения...
            GV.GamePath = AppSelector.Text;
            GV.FullGamePath = Path.Combine(GV.GamePath, GV.SmallAppName);

            // Заполняем другие служебные переменные...
            GV.FullCfgPath = Path.Combine(GV.FullGamePath, "cfg");
            GV.FullBackUpDirPath = Path.Combine(GV.AppUserDir, "backups", GV.SmallAppName);
            GV.VideoCfgFile = Path.Combine(GV.GamePath, GV.ConfDir, "cfg", "video.txt");
            
            // Включаем основные элементы управления (контролы)...
            MainTabControl.Enabled = true;

            // Управляем доступностью модуля создания отчётов для Техподдержки...
            MNUReportBuilder.Enabled = ((AppSelector.Items.Count > 0) && (AppSelector.SelectedIndex != -1));

            // Отключим модули очистки...
            if (!(Properties.Settings.Default.AllowNCFUnsafeOps)) { EnableCleanButtons(false); }

            // Считаем настройки графики...
            if (GV.IsUsingVideoFile)
            {
                if (File.Exists(GV.VideoCfgFile))
                {
                    ReadNCFGameSettings(GV.VideoCfgFile);
                }
                else
                {
                    NullGraphOptions();
                }
            }
            else
            {
                ReadGCFGameSettings(GV.SmallAppName);
            }

            // Переключаем графический твикер в режим GCF/NCF...
            SetGTOptsType(!GV.IsUsingVideoFile);

            // Проверим, установлен ли FPS-конфиг...
            GT_Warning.Visible = File.Exists(Path.Combine(GV.FullCfgPath, "autoexec.cfg"));
            
            // Очистим список FPS-конфигов...
            FP_ConfigSel.Items.Clear();

            // Отключим кнопку редактирования FPS-конфигов...
            FP_OpenNotepad.Enabled = false;

            // Отключим кнопку установки FPS-конфигов...
            FP_Install.Enabled = false;

            // Закроем открытые конфиги в редакторе...
            if (!(String.IsNullOrEmpty(CFGFileName))) { CE_New.PerformClick(); }

            // Считаем имеющиеся FPS-конфиги...
            if (!BW_FPRecv.IsBusy) { BW_FPRecv.RunWorkerAsync(); }

            // Включаем заблокированные ранее контролы...
            MNUFPSWizard.Enabled = true;
            MNUInstaller.Enabled = true;
            
            // Выводим сообщение о завершении считывания в статус-бар...
            SB_Status.Text = CoreLib.GetLocalizedString("StatusNormal");

            // Сохраним ID последней выбранной игры...
            Properties.Settings.Default.LastGameName = AppSelector.Text;

            // Считаем список бэкапов...
            if (!BW_BkUpRecv.IsBusy) { BW_BkUpRecv.RunWorkerAsync(); }
        }

        private void GT_Maximum_Graphics_Click(object sender, EventArgs e)
        {
            // Нажатие этой кнопки устанавливает графические настройки на рекомендуемый максимум...
            // Зададим вопрос, а нужно ли это юзеру?
            if (MessageBox.Show(CoreLib.GetLocalizedString("GT_MaxPerfMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (GV.IsUsingVideoFile)
                {
                    // Пользователь согласился, продолжаем...
                    GT_ScreenType.SelectedIndex = 0; // полноэкранный режим
                    GT_ModelQuality.SelectedIndex = 2; // высокая детализация моделей
                    GT_TextureQuality.SelectedIndex = 2; // высокая детализация текстур
                    GT_ShaderQuality.SelectedIndex = 1; // высокое качество шейдерных эффектов
                    GT_WaterQuality.SelectedIndex = 1; // отражать мир в воде
                    GT_ShadowQuality.SelectedIndex = 1; // высокое качество теней
                    GT_ColorCorrectionT.SelectedIndex = 1; // корренкция цвета включена
                    GT_AntiAliasing.SelectedIndex = 0; // сглаживание выключено
                    GT_Filtering.SelectedIndex = 3; // анизотропная фильтрация 4x
                    GT_VSync.SelectedIndex = 0; // вертикальная синхронизация выключена
                    GT_MotionBlur.SelectedIndex = 0; // размытие движения выключено
                    GT_DxMode.SelectedIndex = 3; // режим DirecX 9.0c
                    GT_HDR.SelectedIndex = 2; // HDR полные
                }
                else
                {
                    GT_NCF_DispMode.SelectedIndex = 0;
                    GT_NCF_AntiAlias.SelectedIndex = 2;
                    GT_NCF_Filtering.SelectedIndex = 3;
                    GT_NCF_VSync.SelectedIndex = 0;
                    GT_NCF_Multicore.SelectedIndex = 1;
                    GT_NCF_ShaderE.SelectedIndex = 3;
                    GT_NCF_EffectD.SelectedIndex = 2;
                    GT_NCF_MemPool.SelectedIndex = 2;
                    GT_NCF_Quality.SelectedIndex = 2;
                }
                MessageBox.Show(CoreLib.GetLocalizedString("GT_PerfSet"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GT_Maximum_Performance_Click(object sender, EventArgs e)
        {
            // Нажатие этой кнопки устанавливает графические настройки на рекомендуемый минимум...
            // Спросим пользователя.
            if (MessageBox.Show(CoreLib.GetLocalizedString("GT_MinPerfMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (GV.IsUsingVideoFile)
                {
                    // Пользователь согласился, продолжаем...
                    GT_ScreenType.SelectedIndex = 0; // полноэкранный режим
                    GT_ModelQuality.SelectedIndex = 0; // низкая детализация моделей
                    GT_TextureQuality.SelectedIndex = 0; // низкая детализация текстур
                    GT_ShaderQuality.SelectedIndex = 0; // низкое качество шейдерных эффектов
                    GT_WaterQuality.SelectedIndex = 0; // простые отражения в воде
                    GT_ShadowQuality.SelectedIndex = 0; // низкое качество теней
                    GT_ColorCorrectionT.SelectedIndex = 0; // корренкция цвета выключена
                    GT_AntiAliasing.SelectedIndex = 0; // сглаживание выключено
                    GT_Filtering.SelectedIndex = 1; // трилинейная фильтрация текстур
                    GT_VSync.SelectedIndex = 0; // вертикальная синхронизация выключена
                    GT_MotionBlur.SelectedIndex = 0; // размытие движения выключено
                    // Спросим у пользователя о режиме DirectX...
                    if (MessageBox.Show(CoreLib.GetLocalizedString("GT_DxLevelMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        GT_DxMode.SelectedIndex = 0; // режим DirecX 8.0
                    }
                    else
                    {
                        GT_DxMode.SelectedIndex = 3; // режим DirecX 9.0c
                    }
                    GT_HDR.SelectedIndex = 0; // эффекты HDR выключены
                }
                else
                {
                    GT_NCF_DispMode.SelectedIndex = 0;
                    GT_NCF_AntiAlias.SelectedIndex = 0;
                    GT_NCF_Filtering.SelectedIndex = 1;
                    GT_NCF_VSync.SelectedIndex = 0;
                    GT_NCF_Multicore.SelectedIndex = 1;
                    GT_NCF_ShaderE.SelectedIndex = 0;
                    GT_NCF_EffectD.SelectedIndex = 0;
                    GT_NCF_MemPool.SelectedIndex = 0;
                    GT_NCF_Quality.SelectedIndex = 0;
                }
                MessageBox.Show(CoreLib.GetLocalizedString("GT_PerfSet"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GT_SaveApply_Click(object sender, EventArgs e)
        {
            // Сохраняем изменения в графических настройках...
            // Запрашиваем подтверждение у пользователя...
            if (MessageBox.Show(CoreLib.GetLocalizedString("GT_SaveMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!GV.IsUsingVideoFile)
                {
                    // Создаём резервную копию...
                    if (Properties.Settings.Default.SafeCleanup)
                    {
                        try
                        {
                            CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve", "Source", GV.SmallAppName, "Settings"), "Game_AutoBackUp", GV.FullBackUpDirPath);
                        }
                        catch (Exception Ex)
                        {
                            CoreLib.WriteStringToLog(Ex.Message);
                        }
                    }

                    try
                    {
                        // Проверим существование ключа реестра и в случае необходимости создадим...
                        if (!(CoreLib.CheckIfHKCUSKeyExists(Path.Combine("Software", "Valve", "Source", GV.SmallAppName, "Settings")))) { Registry.CurrentUser.CreateSubKey(Path.Combine("Software", "Valve", "Source", GV.SmallAppName, "Settings")); }
                        // Записываем выбранные настройки в реестр...
                        WriteGCFGameSettings(GV.SmallAppName);
                        // Выводим подтверждающее сообщение...
                        MessageBox.Show(CoreLib.GetLocalizedString("GT_SaveSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception Ex)
                    {
                        CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("GT_SaveFailure"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // Это NCF-приложение, поэтому будем записывать настройки в файл...
                    if ((GT_NCF_Quality.SelectedIndex != -1) && (GT_NCF_MemPool.SelectedIndex != -1)
                        && (GT_NCF_EffectD.SelectedIndex != -1) && (GT_NCF_ShaderE.SelectedIndex != -1)
                        && (GT_NCF_Multicore.SelectedIndex != -1) && (GT_NCF_VSync.SelectedIndex != -1)
                        && (GT_NCF_Filtering.SelectedIndex != -1) && (GT_NCF_AntiAlias.SelectedIndex != -1)
                        && (GT_NCF_DispMode.SelectedIndex != -1) && (GT_NCF_Ratio.SelectedIndex != -1))
                    {
                        // Создадим бэкап файла с графическими настройками...
                        if (Properties.Settings.Default.SafeCleanup)
                        {
                            if (File.Exists(GV.VideoCfgFile))
                            {
                                CreateBackUpNow(Path.GetFileName(GV.VideoCfgFile), Path.GetDirectoryName(GV.VideoCfgFile), GV.FullBackUpDirPath);
                            }
                        }
                        
                        // Записываем...
                        try
                        {
                            WriteNCFGameSettings(GV.VideoCfgFile);
                            MessageBox.Show(CoreLib.GetLocalizedString("GT_SaveSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception Ex)
                        {
                            CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("GT_NCFFailure"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show(CoreLib.GetLocalizedString("GT_NCFNReady"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // Запишем в реестр пользовательскую строку запуска TF2...
                // TODO: реализовать возможность записывать параметры строки запуска...

                // Закончили запись основных настроек, указанных пользователем или выбранных по умолчанию...
            }
        }

        private void FP_ConfigSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получаем описание выбранного пользователем конфига...
            try
            {
                FP_Description.Text = File.ReadAllText(Path.Combine(GV.FullAppPath, "cfgs", String.Format("{0}_{1}.txt", Path.GetFileNameWithoutExtension(FP_ConfigSel.Text), CoreLib.GetLocalizedString("AppLangPrefix"))));
            }
            catch (Exception Ex)
            {
                CoreLib.WriteStringToLog(Ex.Message);
                FP_Description.Text = CoreLib.GetLocalizedString("FP_NoDescr");
            }
            // Включаем кнопку открытия конфига в Блокноте...
            FP_OpenNotepad.Enabled = true;
            // Включаем кнопку установки конфига...
            FP_Install.Enabled = true;
        }

        private void FP_Install_Click(object sender, EventArgs e)
        {
            // Начинаем устанавливать FPS-конфиг в управляемое приложение...
            if (FP_ConfigSel.SelectedIndex != -1)
            {
                if (MessageBox.Show(CoreLib.GetLocalizedString("FP_InstallQuestion"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Проверим, не нужно ли создавать резервную копию...
                    if (Properties.Settings.Default.SafeCleanup)
                    {
                        // Создаём резервную копию...
                        CreateBackUpNow("autoexec.cfg", GV.FullCfgPath, GV.FullBackUpDirPath);
                    }

                    try
                    {
                        // Создадим каталог если он не существует...
                        if (!Directory.Exists(GV.FullCfgPath)) { Directory.CreateDirectory(GV.FullCfgPath); }
                        // Устанавливаем...
                        InstallConfigNow(FP_ConfigSel.Text, GV.FullAppPath, GV.FullCfgPath);
                        // Выводим сообщение об успешной установке...
                        MessageBox.Show(CoreLib.GetLocalizedString("FP_InstallSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Отобразим значок предупреждения на странице графических настроек...
                        GT_Warning.Visible = true;
                    }
                    catch (Exception Ex)
                    {
                        // Установка не удалась. Выводим сообщение об этом...
                        CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("FP_InstallFailed"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                // Пользователь не выбрал конфиг. Сообщим об этом...
                MessageBox.Show(CoreLib.GetLocalizedString("FP_NothingSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FP_Uninstall_Click(object sender, EventArgs e)
        {
            // Начинаем удаление установленного конфига...
            // Генерируем имя файла с полным путём до него...
            string CfgFile = Path.Combine(GV.FullCfgPath, "autoexec.cfg");
            // Проверяем, существует ли файл...
            if (File.Exists(CfgFile))
            {
                // Файл существует. Запросим подтверждение на удаление...
                if (MessageBox.Show(CoreLib.GetLocalizedString("FP_RemoveQuestion"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    // Создадим бэкап если включена безопасная очистка...
                    if (Properties.Settings.Default.SafeCleanup)
                    {
                        // Создаём резервную копию...
                        CreateBackUpNow("autoexec.cfg", GV.FullCfgPath, GV.FullBackUpDirPath);
                    }

                    try
                    {
                        // Удалим файл...
                        File.Delete(CfgFile);
                        // Выводим сообщение об успешном удалении...
                        MessageBox.Show(CoreLib.GetLocalizedString("FP_RemoveSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Скрываем значок предупреждения на странице графических настроек...
                        GT_Warning.Visible = false;
                    }
                    catch (Exception Ex)
                    {
                        // Произошло исключение при попытке удаления. Уведомим пользователя об этом...
                        CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("FP_RemoveFailed"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("FP_RemoveNotExists"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GT_Warning_Click(object sender, EventArgs e)
        {
            // Выдадим сообщение о наличии FPS-конфига...
            MessageBox.Show(CoreLib.GetLocalizedString("GT_FPSCfgDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void CE_New_Click(object sender, EventArgs e)
        {
            // Создаём новый файл...
            CE_Editor.Rows.Clear();
            CFGFileName = "";
            SB_Status.Text = CoreLib.GetLocalizedString("StatusOpenedFile") + " " + CoreLib.GetLocalizedString("UnnamedFileName");
        }

        private void CE_Open_Click(object sender, EventArgs e)
        {
            // Прочитаем конфиг и заполним его содержимым нашу таблицу редактора...
            
            // Указываем стартовый каталог в диалоге открытия файла на каталог с конфигами игры...
            CE_OpenCfgDialog.InitialDirectory = GV.FullCfgPath;

            // Считывает файл конфига и помещает записи в таблицу
            if (CE_OpenCfgDialog.ShowDialog() == DialogResult.OK) // Отображаем стандартный диалог открытия файла...
            {
                // Считываем...
                ReadConfigFromFile(CE_OpenCfgDialog.FileName);
            }
        }

        private void CE_Save_Click(object sender, EventArgs e)
        {
            CE_SaveCfgDialog.InitialDirectory = GV.FullCfgPath; // Указываем путь по умолчанию к конфигам управляемого приложения...
            if (!(String.IsNullOrEmpty(CFGFileName))) // Проверяем, открыт ли какой-либо файл...
            {
                // Будем бэкапировать только файлы, находящиеся в каталоге /cfg/
                // управляемоего приложения. Остальные - нет.
                if (Properties.Settings.Default.SafeCleanup)
                {
                    if (File.Exists(Path.Combine(GV.FullCfgPath, CFGFileName)))
                    {
                        // Создаём резервную копию...
                        CreateBackUpNow(CFGFileName, GV.FullCfgPath, GV.FullBackUpDirPath);
                    }
                }

                // Начинаем сохранение...
                try
                {
                    //WriteTableToFileNow(CFGFileName);
                    WriteTableToFileNow(CE_OpenCfgDialog.FileName, GV.AppName);
                }
                catch (Exception Ex)
                {
                    // Произошла ошибка при сохранении файла...
                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("CE_CfgSVVEx"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Зададим стандартное имя (см. issue 21)...
                if (!(File.Exists(Path.Combine(GV.FullCfgPath, "autoexec.cfg"))))
                {
                    // Файл autoexec.cfg не существует, поэтому предложим это имя...
                    CE_SaveCfgDialog.FileName = "autoexec.cfg";
                }
                else
                {
                    // Файл существует, поэтому предложим стандартное имя безымянного конфига...
                    CE_SaveCfgDialog.FileName = CoreLib.GetLocalizedString("UnnamedFileName");
                }

                // Файл не был открыт. Нужно сохранить и дать имя...
                if (CE_SaveCfgDialog.ShowDialog() == DialogResult.OK) // Отображаем стандартный диалог сохранения файла...
                {
                    WriteTableToFileNow(CE_SaveCfgDialog.FileName, GV.AppName);
                    CFGFileName = Path.GetFileName(CE_SaveCfgDialog.FileName);
                    CE_OpenCfgDialog.FileName = CE_SaveCfgDialog.FileName;
                    SB_Status.Text = CoreLib.GetLocalizedString("StatusOpenedFile") + " " + CFGFileName;
                }
            }
        }

        private void CE_SaveAs_Click(object sender, EventArgs e)
        {
            CE_SaveCfgDialog.InitialDirectory = GV.FullCfgPath;
            if (CE_SaveCfgDialog.ShowDialog() == DialogResult.OK)
            {
                WriteTableToFileNow(CE_SaveCfgDialog.FileName, GV.AppName);
            }
        }

        private void PS_RemCustMaps_Click(object sender, EventArgs e)
        {
            // Удаляем кастомные (нестандартные) карты...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullGamePath, "maps", "*.bsp"));
            OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), false, true);
        }

        private void PS_RemDnlCache_Click(object sender, EventArgs e)
        {
            // Удаляем кэш загрузок...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullGamePath, "downloads", "*.*"));
            OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower());
        }

        private void PS_RemOldSpray_Click(object sender, EventArgs e)
        {
            // Удаляем кэш спреев...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullGamePath, "materials", "temp", "*.vtf"));
            OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower());
        }

        private void PS_RemGraphCache_Click(object sender, EventArgs e)
        {
            // Удаляем графический кэш...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullGamePath, "maps", "graphs", "*.*"));
            OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower());
        }

        private void PS_RemSoundCache_Click(object sender, EventArgs e)
        {
            // Удаляем звуковой кэш...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullGamePath, "maps", "soundcache", "*.*"));
            OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower());
        }

        private void PS_RemScreenShots_Click(object sender, EventArgs e)
        {
            // Удаляем все скриншоты...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullGamePath, "screenshots", "*.*"));
            OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower());
        }

        private void PS_RemDemos_Click(object sender, EventArgs e)
        {
            // Удаляем все записанные демки...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullGamePath, "*.dem"));
            OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower());
        }

        private void PS_RemGraphOpts_Click(object sender, EventArgs e)
        {
            // Удаляем графические настройки...
            if (MessageBox.Show(((Button)sender).Text + "?", GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (GV.IsUsingVideoFile)
                {
                    // Создаём резервную копию...
                    if (Properties.Settings.Default.SafeCleanup)
                    {
                        try
                        {
                            CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve", "Source", GV.SmallAppName, "Settings"), "Game_AutoBackUp", GV.FullBackUpDirPath);
                        }
                        catch (Exception Ex)
                        {
                            CoreLib.WriteStringToLog(Ex.Message);
                        }
                    }

                    // Работаем...
                    try
                    {
                        // Удаляем ключ HKEY_CURRENT_USER\Software\Valve\Source\tf\Settings из реестра...
                        Registry.CurrentUser.DeleteSubKeyTree(Path.Combine("Software", "Valve", "Source", GV.SmallAppName, "Settings"), false);
                        MessageBox.Show(CoreLib.GetLocalizedString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception Ex)
                    {
                        CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("PS_CleanupErr"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    try
                    {
                        if (File.Exists(GV.VideoCfgFile))
                        {
                            if (Properties.Settings.Default.SafeCleanup)
                            {
                                CreateBackUpNow(Path.GetFileName(GV.VideoCfgFile), Path.GetDirectoryName(GV.VideoCfgFile), GV.FullBackUpDirPath);
                            }
                            File.Delete(GV.VideoCfgFile);
                            MessageBox.Show(CoreLib.GetLocalizedString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception Ex)
                    {
                        CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("PS_CleanupErr"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void PS_RemOldBin_Click(object sender, EventArgs e)
        {
            // Удаляем старые бинарники...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.GamePath, "bin", "*.*"));
            CleanDirs.Add(Path.Combine(GV.GamePath, "platform", "*.*"));
            CleanDirs.Add(Path.Combine(GV.FullGamePath, "bin", "*.*"));
            CleanDirs.Add(Path.Combine(GV.GamePath, "*.exe"));
            OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower());
        }

        private void GT_ResHor_Btn_Click(object sender, EventArgs e)
        {
            // Выводим краткую справку о вводе разрешения по горизонтали...
            MessageBox.Show(String.Format(CoreLib.GetLocalizedString("GT_ResMsg"), CoreLib.GetLocalizedString("GT_ResMsgHor")), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GT_ResVert_Btn_Click(object sender, EventArgs e)
        {
            // Выводим краткую справку о вводе разрешения по вертикали...
            MessageBox.Show(String.Format(CoreLib.GetLocalizedString("GT_ResMsg"), CoreLib.GetLocalizedString("GT_ResMsgVert")), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GT_LaunchOptions_Btn_Click(object sender, EventArgs e)
        {
            // Выводим краткую справку о строке параметров запуска...
            MessageBox.Show(CoreLib.GetLocalizedString("NotImplementedYet"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PS_ResetSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(((Button)sender).Text + "?", GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    Process.Start(String.Format("steam://validate/{0}", GV.GameInternalID));
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("AppStartSteamFailed"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        private void MNUShowEdHint_Click(object sender, EventArgs e)
        {
            // Покажем подсказку...
            CE_ShowHint.PerformClick();
        }

        private void MNUReportBuilder_Click(object sender, EventArgs e)
        {
            if ((AppSelector.Items.Count > 0) && (AppSelector.SelectedIndex != -1))
            {
                // Запускаем форму создания отчёта для Техподдержки...
                frmRepBuilder RBF = new frmRepBuilder();
                RBF.ShowDialog();
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("AppNoGamesSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUInstaller_Click(object sender, EventArgs e)
        {
            // Запускаем форму установщика спреев, демок и конфигов...
            frmInstaller InstF = new frmInstaller();
            InstF.ShowDialog();
        }

        private void MNUExit_Click(object sender, EventArgs e)
        {
            // Завершаем работу программы...
            Environment.Exit(0);
        }

        private void MNUFPSWizard_Click(object sender, EventArgs e)
        {
            // Очистим Редактор конфигов...
            CE_New.PerformClick();
            // Запускаем форму мастера FPS-конфигов...
            frmFPGen FPFrm = new frmFPGen(new CoreLib.CFGEdDelegate(AddRowToTable));
            FPFrm.ShowDialog();
            MainTabControl.SelectedIndex = 1;
        }

        private void MNUAbout_Click(object sender, EventArgs e)
        {
            // Отобразим форму "О программе"...
            frmAbout AboutFrm = new frmAbout();
            AboutFrm.ShowDialog();
        }

        private void MNUReportBug_Click(object sender, EventArgs e)
        {
            // Открываем в браузере страницу сообщения об ошибке в багтрекере гуглкода...
            Process.Start("http://code.google.com/p/srcrepair/issues/entry");
        }

        private void BUT_Refresh_Click(object sender, EventArgs e)
        {            
            try
            {
                ReadBackUpList2Table(GV.FullBackUpDirPath);
            }
            catch (Exception Ex)
            {
                // Произошло исключение...
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("BU_ListLdFailed"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                // Создадим каталог для резервных копий...
                Directory.CreateDirectory(GV.FullBackUpDirPath);
            }
        }

        private void BUT_RestoreB_Click(object sender, EventArgs e)
        {
            // Восстановим выделенный бэкап...
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    // Получаем имя файла...
                    string FName = BU_LVTable.SelectedItems[0].SubItems[4].Text;

                    // Запрашиваем подтверждение...
                    if (MessageBox.Show(String.Format(CoreLib.GetLocalizedString("BU_QMsg"), Path.GetFileNameWithoutExtension(FName), BU_LVTable.SelectedItems[0].SubItems[3].Text), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Проверяем что восстанавливать: конфиг или реестр...
                        switch (Path.GetExtension(FName))
                        {
                            case ".reg":
                                // Восстанавливаем файл реестра...
                                try
                                {
                                    // Восстанавливаем...
                                    Process.Start("regedit.exe", String.Format("/s \"{0}\"", Path.Combine(GV.FullBackUpDirPath, FName)));
                                    // Показываем сообщение об успешном восстановлении...
                                    MessageBox.Show(CoreLib.GetLocalizedString("BU_RestSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                catch (Exception Ex)
                                {
                                    // Произошло исключение...
                                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("BU_RestFailed"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                                }
                                break;
                            case ".bud":
                                using (ZipFile Zip = ZipFile.Read(Path.Combine(GV.FullBackUpDirPath, FName)))
                                {
                                    foreach (ZipEntry ZFile in Zip)
                                    {
                                        try
                                        {
                                            ZFile.Extract(Path.GetPathRoot(GV.FullSteamPath), ExtractExistingFileAction.OverwriteSilently);
                                        }
                                        catch (Exception Ex)
                                        {
                                            CoreLib.WriteStringToLog(Ex.Message);
                                        }
                                    }
                                }
                                MessageBox.Show(CoreLib.GetLocalizedString("BU_RestSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            default:
                                switch (Path.GetExtension(Path.GetFileNameWithoutExtension(FName)))
                                {
                                    case ".cfg":
                                        RestoreBackUpFile(FName, GV.FullCfgPath);
                                        break;
                                    case ".txt":
                                        RestoreBackUpFile(FName, Path.Combine(GV.GamePath, GV.ConfDir, "cfg"));
                                        break;
                                    default:
                                        MessageBox.Show(CoreLib.GetLocalizedString("BU_UnknownType"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        break;
                                }
                                break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_NoSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("BU_NoFiles"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BUT_DelB_Click(object sender, EventArgs e)
        {
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    // Удалим выбранный бэкап...
                    string FName = BU_LVTable.SelectedItems[0].SubItems[4].Text;
                    // Запросим подтверждение...
                    if (MessageBox.Show(CoreLib.GetLocalizedString("BU_DelMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        try
                        {
                            // Удаляем файл...
                            File.Delete(Path.Combine(GV.FullBackUpDirPath, FName));
                            // Удаляем строку...
                            BU_LVTable.Items.Remove(BU_LVTable.SelectedItems[0]);
                            // Показываем сообщение об успешном удалении...
                            MessageBox.Show(CoreLib.GetLocalizedString("BU_DelSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception Ex)
                        {
                            // Произошло исключение при попытке удаления файла резервной копии...
                            CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("BU_DelFailed"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_NoSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("BU_NoFiles"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BUT_CrBkupReg_ButtonClick(object sender, EventArgs e)
        {
            BUT_CrBkupReg.ShowDropDown();
        }

        private void BUT_L_GameSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(CoreLib.GetLocalizedString("BU_RegCreate"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Создадим резервную копию графических настроек игры...
                try
                {
                    if (GV.IsUsingVideoFile)
                    {
                        CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve", "Source", GV.SmallAppName, "Settings"), "Game_Options", GV.FullBackUpDirPath);
                    }
                    else
                    {
                        if (File.Exists(GV.VideoCfgFile))
                        {
                            CreateBackUpNow(Path.GetFileName(GV.VideoCfgFile), Path.GetDirectoryName(GV.VideoCfgFile), GV.FullBackUpDirPath);
                        }
                    }
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_RegDone"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BUT_Refresh.PerformClick();
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("BU_RegErr"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        private void BUT_L_AllSteam_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(CoreLib.GetLocalizedString("BU_RegCreate"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Создадим резервную копию всех настроек Steam...
                try
                {
                    // Создаём...
                    CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve"), "Steam_BackUp", GV.FullBackUpDirPath);
                    // Выводим сообщение...
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_RegDone"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Обновим список бэкапов...
                    BUT_Refresh.PerformClick();
                }
                catch (Exception Ex)
                {
                    // Произошло исключение, уведомим пользователя...
                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("BU_RegErr"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        private void BUT_L_AllSRC_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(CoreLib.GetLocalizedString("BU_RegCreate"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Созданим резервную копию графических настроек всех Source-игр...
                try
                {
                    CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve", "Source"), "Source_Options", GV.FullBackUpDirPath);
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_RegDone"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BUT_Refresh.PerformClick();
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("BU_RegErr"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Проверяем открыта ли страница "Редактор конфигов"...
            if (MainTabControl.SelectedIndex == 1)
            {
                // Включаем заблокированный контрол...
                MNUShowEdHint.Enabled = true;
                // Проверяем открыт ли файл в Редакторе конфигов...
                SB_Status.Text = String.IsNullOrEmpty(CFGFileName) ? CoreLib.GetLocalizedString("StatusOpenedFile") + " " + CoreLib.GetLocalizedString("UnnamedFileName") : SB_Status.Text = CoreLib.GetLocalizedString("StatusOpenedFile") + " " + CFGFileName;
            }
            else
            {
                // Открыта другая страница...
                // Блокируем контрол подсказки...
                MNUShowEdHint.Enabled = false;
                // ...и выводим стандартное сообщение в статус-бар...
                SB_Status.Text = CoreLib.GetLocalizedString("StatusNormal");
            }
        }

        private void CE_ShowHint_Click(object sender, EventArgs e)
        {
            try
            {
                string Buf = CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[0].Value.ToString();
                if (!(String.IsNullOrEmpty(Buf)))
                {
                    Buf = GetCVDescription(Buf);
                    if (!(String.IsNullOrEmpty(Buf)))
                    {
                        MessageBox.Show(Buf, GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(CoreLib.GetLocalizedString("CE_ClNoDescr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("CE_ClSelErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show(CoreLib.GetLocalizedString("CE_ClSelErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUHelp_Click(object sender, EventArgs e)
        {
            // Проверим наличие локальной справки в формате CHM...
            string HelpFilePath = String.Format(GV.FullAppPath + "srcrepair_{0}.chm", CoreLib.GetLocalizedString("AppLangPrefix"));
            if (File.Exists(HelpFilePath))
            {
                Process.Start(HelpFilePath);
            }
            else
            {
                // Выбираем какую онлайновую справочную систему использовать.
                // Используем Switch, т.к. их может быть больше двух.
                switch (Properties.Settings.Default.PreferedHelpSystem)
                {
                    case 0: Process.Start("http://www.easycoding.org/projects/srcrepair/help");
                        break;
                    case 1: Process.Start(String.Format("http://code.google.com/p/srcrepair/wiki/UserManual_{0}", CoreLib.GetLocalizedString("AppLangPrefix")));
                        break;
                }
            }
        }

        private void MNUOpinion_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("mailto:srcrepair@easycoding.org?subject=SRC Repair Opinion");
            }
            catch
            {
                Process.Start("http://www.easycoding.org/projects/srcrepair#respond");
            }
        }

        private void MNUSteamGroup_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("steam://url/GroupSteamIDPage/103582791431662552");
            }
            catch
            {
                Process.Start("http://steamcommunity.com/groups/srcrepair");
            }
        }

        private void MNULnkEasyCoding_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.easycoding.org/");
        }

        private void MNULnkTFRU_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.team-fortress.ru/");
        }

        private void MNUHEd_Click(object sender, EventArgs e)
        {
            // Отобразим форму редактора файла hosts...
            frmHEd HEdFrm = new frmHEd();
            HEdFrm.ShowDialog();
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
            catch (Exception Ex)
            {
                CoreLib.WriteStringToLog(Ex.Message);
            }
        }

        private void CE_Copy_Click(object sender, EventArgs e)
        {
            if (CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value != null)
            {
                Clipboard.SetText(CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value.ToString());
            }
        }

        private void CE_Cut_Click(object sender, EventArgs e)
        {
            if (CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value != null)
            {
                // Копируем в буфер...
                Clipboard.SetText(CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value.ToString());
                // Удаляем из ячейки...
                CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value = null;
            }
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
            catch (Exception Ex)
            {
                CoreLib.WriteStringToLog(Ex.Message);
            }
        }

        private void FP_OpenNotepad_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Settings.Default.EditorBin, Path.Combine(GV.FullAppPath, "cfgs", FP_ConfigSel.Text));
        }

        private void MNUUpdateCheck_Click(object sender, EventArgs e)
        {
            // Сохраним текущее содержимое статусной строки...
            string StatusBarCurrText = SB_Status.Text;
            // Выведем сообщение о проверке обновлений...
            SB_Status.Text = CoreLib.GetLocalizedString("AppCheckingForUpdates");
            // Начинаем проверку...
            try
            {
                string NewVersion, UpdateURI, DnlStr;
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
                // Проверим, является ли версия на сервере новее, чем текущая...
                if (CoreLib.CompareVersions(GV.AppVersionInfo, NewVersion))
                {
                    // Доступна новая версия, отобразим модуль обновления...
                    frmUpdate UpdFrm = new frmUpdate(NewVersion, UpdateURI);
                    UpdFrm.ShowDialog();
                }
                else
                {
                    // Новых версий не обнаружено.
                    MessageBox.Show(CoreLib.GetLocalizedString("UPD_LatestInstalled"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception Ex)
            {
                // Произошло исключение...
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("UPD_ExceptionDetected"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
            // Вернём предыдущее содержимое строки статуса...
            SB_Status.Text = StatusBarCurrText;
        }

        private void BUT_OpenNpad_Click(object sender, EventArgs e)
        {
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    // Откроем выбранный бэкап в Блокноте Windows...
                    Process.Start(Properties.Settings.Default.EditorBin, Path.Combine(GV.FullBackUpDirPath, BU_LVTable.SelectedItems[0].SubItems[4].Text));
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_NoSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("BU_NoFiles"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUAppOptions_Click(object sender, EventArgs e)
        {
            // Показываем форму настроек...
            frmOptions OptsFrm = new frmOptions();
            OptsFrm.ShowDialog();
        }

        private void BU_LVTable_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
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
                    Process.Start(Properties.Settings.Default.ShBin, String.Format("{0} \"{1}\"", Properties.Settings.Default.ShParam, Path.Combine(GV.FullBackUpDirPath, BU_LVTable.SelectedItems[0].SubItems[4].Text)));
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_NoSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("BU_NoFiles"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmMainW_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Форма была закрыта, сохраняем настройки приложения...
            Properties.Settings.Default.Save();
        }

        private void MNUWinMnuDisabler_Click(object sender, EventArgs e)
        {
            // Показываем модуля отключения клавиш...
            frmKBHelper KBHlp = new frmKBHelper();
            KBHlp.ShowDialog();
        }

        private void MNUDonate_Click(object sender, EventArgs e)
        {
            Process.Start("http://code.google.com/p/srcrepair/wiki/Donate");
        }

        private void CE_OpenInNotepad_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(CFGFileName)))
            {
                Process.Start(Properties.Settings.Default.EditorBin, CE_OpenCfgDialog.FileName);
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("CE_NoFileOpened"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PS_PathDetector_Click(object sender, EventArgs e)
        {
            if (((Label)sender).ForeColor == Color.Red)
            {
                MessageBox.Show(CoreLib.GetLocalizedString("SteamNonASCIISmall"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("SteamNonASCIINotDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void PS_RemModels_Click(object sender, EventArgs e)
        {
            // Удаляем все кастомные модели...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullGamePath, "models", "*.*"));
            OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower());
        }

        private void PS_RemReplays_Click(object sender, EventArgs e)
        {
            // Удаляем все реплеи...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullGamePath, "replay", "*.*"));
            OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower());
        }

        private void PS_RemTextures_Click(object sender, EventArgs e)
        {
            // Удаляем все кастомные текстуры...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullGamePath, "materials", "*.*"));
            OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower());
        }

        private void PS_RemSecndCache_Click(object sender, EventArgs e)
        {
            // Удаляем содержимое вторичного кэша загрузок...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullGamePath, "cache", "*.*"));
            CleanDirs.Add(Path.Combine(GV.GamePath, "config", "html", "*.*"));
            OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower());
        }

        private void SB_App_DoubleClick(object sender, EventArgs e)
        {
            // Переключим статус безопасной очистки...
            Properties.Settings.Default.SafeCleanup = !Properties.Settings.Default.SafeCleanup;
            // Сообщим пользователю если он отключил безопасную очистку...
            if (!Properties.Settings.Default.SafeCleanup)
            {
                MessageBox.Show(CoreLib.GetLocalizedString("AppSafeClnDisabled"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Обновим статусную строку...
            CheckSafeClnStatus();
        }

        private void CE_OpenCVList_Click(object sender, EventArgs e)
        {
            Process.Start(CoreLib.GetLocalizedString("AppCVListURL"));
        }

        private void MNUExtClnCache_Click(object sender, EventArgs e)
        {
            // Очистим HTML-кэш внутреннего браузера Steam...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullSteamPath, "config", "htmlcache", "*.*"));
            OpenCleanupWindow(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", ""));
        }

        private void MNUExtClnOverlay_Click(object sender, EventArgs e)
        {
            // Очистим HTML-кэш браузера оверлея...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullSteamPath, "config", "overlayhtmlcache", "*.*"));
            OpenCleanupWindow(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", ""));
        }

        private void MNUExtClnOverlayHTCache_Click(object sender, EventArgs e)
        {
            // Очистим HTTP-кэш...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullSteamPath, "appcache", "httpcache", "*.*"));
            OpenCleanupWindow(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", ""));
        }

        private void CE_ManualBackUpCfg_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(CFGFileName)))
            {
                if (File.Exists(Path.Combine(GV.FullCfgPath, CFGFileName)))
                {
                    CreateBackUpNow(CFGFileName, GV.FullCfgPath, GV.FullBackUpDirPath);
                    MessageBox.Show(String.Format(CoreLib.GetLocalizedString("CE_BackUpCreated"), CFGFileName), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("CE_NoFileOpened"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUExtClnLogs_Click(object sender, EventArgs e)
        {
            // Очистим логи клиента Steam...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullSteamPath, "logs", "*.*"));
            CleanDirs.Add(Path.Combine(GV.FullSteamPath, "*.log"));
            OpenCleanupWindow(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", ""));
        }

        private void MNUExtClnGIcons_Click(object sender, EventArgs e)
        {
            // Очистим кэшированные значки игр...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullSteamPath, "steam", "games", "*.*"));
            OpenCleanupWindow(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", ""));
        }

        private void MNUExtClnGameStats_Click(object sender, EventArgs e)
        {
            // Очистим кэшированную статистику игр...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullSteamPath, "appcache", "stats", "*.*"));
            OpenCleanupWindow(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", ""));
        }

        private void MNUExtClnErrDumps_Click(object sender, EventArgs e)
        {
            // Очистим краш-дампы...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullSteamPath, "dumps", "*.*"));
            OpenCleanupWindow(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", ""));
        }

        private void MNUExtClnCloudLocal_Click(object sender, EventArgs e)
        {
            // Очистим локальное зеркало Cloud...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullSteamPath, "userdata", "*.*"));
            OpenCleanupWindow(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", ""));
        }

        private void PS_RemSounds_Click(object sender, EventArgs e)
        {
            // Удаляем кастомные звуки...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullGamePath, "sound", "*.*"));
            OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), false, true);
        }

        private void MNUUpGameDB_Click(object sender, EventArgs e)
        {
            // Обновим базу поддерживаемых программой игр...
            // Сохраним текущее содержимое статусной строки...
            string StatusBarCurrText = SB_Status.Text;
            // Выведем сообщение о проверке обновлений...
            SB_Status.Text = CoreLib.GetLocalizedString("AppCheckingForUpdates");
            // Начинаем проверку...
            try
            {
                // Получаем файл с номером версии и ссылкой на новую...
                string DBHash;
                using (WebClient Downloader = new WebClient())
                {
                    // Получим хеш...
                    Downloader.Headers.Add("User-Agent", GV.UserAgent);
                    DBHash = Downloader.DownloadString(Properties.Settings.Default.UpdateGameDBHash);
                    CoreLib.WriteStringToLog(String.Format(CoreLib.GetLocalizedString("UPD_RecvHashStr"), DBHash));
                }
                // Рассчитаем хеш текущего файла...
                string CurrHash = CoreLib.CalculateFileMD5(Path.Combine(GV.FullAppPath, Properties.Settings.Default.GameListFile));
                CoreLib.WriteStringToLog(String.Format(CoreLib.GetLocalizedString("UPD_ExtHashSum"), Properties.Settings.Default.GameListFile, CurrHash));
                if (CurrHash != DBHash)
                {
                    // Хеши не совпадают, будем обновлять...
                    using (WebClient Downloader = new WebClient())
                    {
                        // Получаем свежую базу данных...
                        Downloader.Headers.Add("User-Agent", GV.UserAgent);
                        CoreLib.WriteStringToLog(CoreLib.GetLocalizedString("UPD_ProcStartMsg"));
                        Downloader.DownloadFile(new Uri(Properties.Settings.Default.UpdateGameDBFile), Path.Combine(GV.FullAppPath, Properties.Settings.Default.GameListFile));
                        CoreLib.WriteStringToLog(CoreLib.GetLocalizedString("UPD_ProcFinMsg"));
                    }
                    // Выводим сообщение об успешном обновлении...
                    MessageBox.Show(CoreLib.GetLocalizedString("UPD_GamL_Updated"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Хеши совпали, обновление не требуется...
                    MessageBox.Show(CoreLib.GetLocalizedString("UPD_GamL_Latest"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception Ex)
            {
                // Произошло исключение...
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("UPD_ExceptionDetected"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
            // Вернём предыдущее содержимое строки статуса...
            SB_Status.Text = StatusBarCurrText;
        }

        private void MNUExtClnBuildCache_Click(object sender, EventArgs e)
        {
            // Очистим кэш сборки обновлений игр с новой системой контента...
            if (MessageBox.Show(String.Format(CoreLib.GetLocalizedString("PS_CleanupFull"), ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", "")), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    string ClDir = Path.Combine(GV.FullSteamPath, Properties.Resources.SteamAppsFolderName, "downloading");
                    if (Directory.Exists(ClDir)) { Directory.Delete(ClDir, true); }
                    ClDir = Path.Combine(GV.FullSteamPath, Properties.Resources.SteamAppsFolderName, "temp");
                    if (Directory.Exists(ClDir)) { Directory.Delete(ClDir, true); }
                    MessageBox.Show(CoreLib.GetLocalizedString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("PS_CleanupErr"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        private void MNUExtClnUpdCch_Click(object sender, EventArgs e)
        {
            // Очистим кэш обновлений Steam...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(GV.FullSteamPath, "package", "*.*"));
            OpenCleanupWindow(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", ""));
        }
    }
}
