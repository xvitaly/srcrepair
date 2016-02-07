/*
 * Основной модуль программы SRC Repair.
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing; // аналогично Forms...
using System.Linq;
using System.Text;
using System.Windows.Forms; // для работы с формами...
using System.IO; // для работы с файлами...
using System.Diagnostics; // для управления процессами...
using Microsoft.Win32; // для работы с реестром...
using System.Resources; // для управления ресурсами...
using System.Net; // для скачивания файлов...
using System.Xml; // для разбора (парсинга) XML...
using System.Text.RegularExpressions; // для работы с регулярными выражениями...

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

        private string CFGFileName;
        private CurrentApp App;
        private List<SourceGame> SourceGames;
        private SourceGame SelGame;
        private HUDTlx SelHUD;

        #endregion

        #region Internal Functions

        /// <summary>
        /// Устанавливает требуемый FPS-конфиг.
        /// </summary>
        /// <param name="ConfName">Имя конфига</param>
        /// <param name="AppPath">Путь к программе SRC Repair</param>
        /// <param name="GameDir">Путь к каталогу игры</param>
        /// <param name="CustmDir">Флаг использования игрой н. с. к.</param>
        private void InstallConfigNow(string ConfName, string AppPath, string GameDir, bool CustmDir)
        {
            // Генерируем путь к каталогу установки конфига...
            string DestPath = Path.Combine(GameDir, CustmDir ? Path.Combine("custom", Properties.Settings.Default.UserCustDirName) : "", "cfg");

            // Проверяем существование каталога и если его не существует - создаём...
            if (!Directory.Exists(DestPath)) { Directory.CreateDirectory(DestPath); }

            // Устанавливаем...
            File.Copy(Path.Combine(AppPath, "cfgs", ConfName), Path.Combine(DestPath, "autoexec.cfg"), true);
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
            // Удаляем ключ HKEY_LOCAL_MACHINE\Software\Valve рекурсивно (если есть права администратора)...
            if (CoreLib.IsCurrentUserAdmin()) { Registry.LocalMachine.DeleteSubKeyTree(Path.Combine("Software", "Valve"), false); }

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
        private void WriteTableToFileNow(string Path)
        {
            // Начинаем сохранять содержимое редактора в файл...
            using (StreamWriter CFile = new StreamWriter(Path))
            {
                CFile.WriteLine(); // вставляем пустую строку
                for (int i = 0; i < CE_Editor.Rows.Count; i++) // запускаем цикл
                {
                    CFile.Write(CE_Editor.Rows[i].Cells[0].Value); // вставляем содержимое первого столбца (название переменной)
                    CFile.Write(" "); // вставляем пробел
                    CFile.WriteLine(CE_Editor.Rows[i].Cells[1].Value); // вставляем содержимое второго столбца (значение переменной)
                }
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
            string Params = String.Format("/ea \"{0}\" {1}", Path.Combine(DestDir, String.Format("{0}_{1}.reg", FileName, CoreLib.DateTime2Unix(DateTime.Now))), RKey);
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
        /// Считывает из главного файла конфигурации Steam пути к дополнительным точкам монтирования.
        /// </summary>
        /// <param name="SteamPath">Путь к клиенту Steam</param>
        private List<String> GetSteamMountPoints(string SteamPath)
        {
            // Создаём массив, в который будем помещать найденные пути...
            List<String> Result = new List<String>();

            // Добавляем каталог установки Steam...
            Result.Add(SteamPath);

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
                            if (RdStr.IndexOf("BaseInstallFolder", StringComparison.CurrentCultureIgnoreCase) != -1)
                            {
                                RdStr = CoreLib.CleanStrWx(RdStr, true, true);
                                RdStr = RdStr.Remove(0, RdStr.IndexOf(" ") + 1);
                                if (!(String.IsNullOrWhiteSpace(RdStr))) { Result.Add(RdStr); }
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
        /// Формирует полные пути к библиотекам с установленными играми.
        /// </summary>
        /// <param name="SteamPath">Путь установки Steam</param>
        private List<String> FormatInstallDirs(string SteamPath)
        {
            // Создаём массив, в который будем помещать найденные пути...
            List<String> Result = new List<String>();

            // Считываем все возможные расположения локальных библиотек игр...
            List<String> MntPnts = GetSteamMountPoints(SteamPath);

            // Начинаем обход каталога и получение поддиректорий...
            foreach (string MntPnt in MntPnts)
            {
                Result.Add(Path.Combine(MntPnt, Properties.Resources.SteamAppsFolderName, "common"));
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
            List<String> GameDirs = FormatInstallDirs(App.FullSteamPath);
            
            // Формируем список для поддерживаемых игр...
            List<String> AvailableGames = new List<String>();

            try
            {
                // Создаём поток с XML-файлом...
                using (FileStream XMLFS = new FileStream(Path.Combine(App.FullAppPath, Properties.Settings.Default.GameListFile), FileMode.Open, FileAccess.Read))
                {
                    // Создаём объект документа XML...
                    XmlDocument XMLD = new XmlDocument();
                    
                    // Загружаем поток в объект XML документа...
                    XMLD.Load(XMLFS);
                    
                    // Обходим полученный список в цикле...
                    XmlNodeList XMLNode = XMLD.GetElementsByTagName("Game");
                    for (int i = 0; i < XMLNode.Count; i++)
                    {
                        AvailableGames.Add(XMLD.GetElementsByTagName("DirName")[i].InnerText);
                        try
                        {
                            SourceGame SG = new SourceGame(XMLNode[i].Attributes["Name"].Value, XMLD.GetElementsByTagName("DirName")[i].InnerText, XMLD.GetElementsByTagName("SmallName")[i].InnerText, XMLD.GetElementsByTagName("Executable")[i].InnerText, XMLD.GetElementsByTagName("SID")[i].InnerText, XMLD.GetElementsByTagName("VFDir")[i].InnerText, XMLD.GetElementsByTagName("HasVF")[i].InnerText == "1", XMLD.GetElementsByTagName("UserDir")[i].InnerText == "1", XMLD.GetElementsByTagName("HUDsAvail")[i].InnerText == "1", App.FullAppPath, App.AppUserDir, App.FullSteamPath, GameDirs);
                            if (SG.IsInstalled)
                            {
                                SourceGames.Add(SG);
                                AppSelector.Items.Add(XMLNode[i].Attributes["Name"].Value);
                            }
                        }
                        catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    }
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Извлекает значение переменной из строки.
        /// </summary>
        /// <param name="LineA">Строка для извлечения</param>
        private string ExtractCVFromLine(string LineA)
        {
            LineA = CoreLib.CleanStrWx(LineA, true);
            return LineA.Substring(LineA.LastIndexOf(" ")).Trim();
        }

        /// <summary>
        /// Возвращает значение переменной, переданной в параметре, хранящейся в файле.
        /// </summary>
        /// <param name="CVar">Переменная</param>
        /// <param name="VFile">Массив с содержимым конфига</param>
        private int GetNCFDWord(string CVar, List<String> VFile)
        {
            string Result = VFile.FirstOrDefault(s => s.Contains(CVar));
            return Convert.ToInt32(ExtractCVFromLine(Result));
        }

        /// <summary>
        /// Возвращает значение переменной типа double, переданной в параметре, хранящейся в файле.
        /// </summary>
        /// <param name="CVar">Переменная</param>
        /// <param name="VFile">Массив с содержимым конфига</param>
        private double GetNCFDble(string CVar, List<String> VFile)
        {
            string Result = VFile.FirstOrDefault(s => s.Contains(CVar)).Replace(".", ",");
            return Double.Parse(ExtractCVFromLine(Result));
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
            // Проверим существует ли файл...
            if (!(File.Exists(VFileName))) { CoreLib.CreateFile(VFileName); }
            
            // Начинаем сохранять содержимое редактора в файл...
            using (StreamWriter CFile = new StreamWriter(VFileName))
            {
                // Генерируем шаблон...
                string Templt = "\t\"{0}\"\t\t\"{1}\"";
                
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
                CFile.WriteLine(String.Format(Templt, "setting.mat_monitorgamma", (Convert.ToDouble(GT_NCF_Brightness.Text) / 10).ToString().Replace(",", ".")));
                
                // Настройки качества теней...
                CFile.WriteLine(String.Format(Templt, "setting.csm_quality_level", GT_NCF_Shadows.SelectedIndex.ToString()));
                
                // Настройки размытия движения...
                CFile.WriteLine(String.Format(Templt, "setting.mat_motion_blur_enabled", GT_NCF_MBlur.SelectedIndex.ToString()));
                
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
                MessageBox.Show(CoreLib.GetLocalizedString("GT_RegOpenErr"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Получает настройки NCF-игры из файла и заполняет ими таблицу
        /// графического твикера программы.
        /// </summary>
        /// <param name="VFileName">Путь к файлу с настройками</param>
        private void ReadNCFGameSettings(string VFileName)
        {
            // Получаем содержимое файла...
            List<String> VideoFile = new List<String>(File.ReadAllLines(VFileName));
            
            // Получаем значение разрешения по горизонтали...
            try { GT_NCF_HorRes.Value = GetNCFDWord("setting.defaultres", VideoFile); } catch { GT_NCF_HorRes.Value = 800; }
            
            // Получаем значение разрешения по вертикали...
            try { GT_NCF_VertRes.Value = GetNCFDWord("setting.defaultresheight", VideoFile); } catch { GT_NCF_VertRes.Value = 600; }
            
            // Получаем настройки соотношения сторон...
            try { GT_NCF_Ratio.SelectedIndex = GetNCFDWord("setting.aspectratiomode", VideoFile); } catch { GT_NCF_Ratio.SelectedIndex = -1; }
            
            // Получаем настройки яркости...
            try { GT_NCF_Brightness.Text = (GetNCFDble("setting.mat_monitorgamma", VideoFile) * 10).ToString(); } catch { GT_NCF_Brightness.Text = "22"; }

            // Получаем настройки качества теней...
            try { GT_NCF_Shadows.SelectedIndex = GetNCFDWord("setting.csm_quality_level", VideoFile); } catch { GT_NCF_Shadows.SelectedIndex = -1; }

            // Получаем настройки размытия движения...
            try { GT_NCF_MBlur.SelectedIndex = GetNCFDWord("setting.mat_motion_blur_enabled", VideoFile); } catch { GT_NCF_MBlur.SelectedIndex = -1; }

            // Получаем настройки режима...
            try
            {
                switch (GetNCFDWord("setting.fullscreen", VideoFile))
                {
                    case 0:
                        switch (GetNCFDWord("setting.nowindowborder", VideoFile))
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
                switch (GetNCFDWord("setting.mat_antialias", VideoFile))
                {
                    case 0: GT_NCF_AntiAlias.SelectedIndex = 0;
                        break;
                    case 1: GT_NCF_AntiAlias.SelectedIndex = 0;
                        break;
                    case 2: GT_NCF_AntiAlias.SelectedIndex = 1;
                        break;
                    case 4:
                        switch (GetNCFDWord("setting.mat_aaquality", VideoFile))
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
                        switch (GetNCFDWord("setting.mat_aaquality", VideoFile))
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
                switch (GetNCFDWord("setting.mat_forceaniso", VideoFile))
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
                switch (GetNCFDWord("setting.mat_vsync", VideoFile))
                {
                    case 0: GT_NCF_VSync.SelectedIndex = 0;
                        break;
                    case 1:
                        switch (GetNCFDWord("setting.mat_triplebuffered", VideoFile))
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
                switch (GetNCFDWord("setting.mat_queue_mode", VideoFile))
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
            try { GT_NCF_ShaderE.SelectedIndex = GetNCFDWord("setting.gpu_level", VideoFile); } catch { GT_NCF_ShaderE.SelectedIndex = -1; }
            
            // Получаем настройки эффектов...
            try { GT_NCF_EffectD.SelectedIndex = GetNCFDWord("setting.cpu_level", VideoFile); } catch { GT_NCF_EffectD.SelectedIndex = -1; }
            
            // Получаем настройки пула памяти...
            try { GT_NCF_MemPool.SelectedIndex = GetNCFDWord("setting.mem_level", VideoFile); } catch { GT_NCF_MemPool.SelectedIndex = -1; }
            
            // Получаем настройки качества моделей и текстур...
            try { GT_NCF_Quality.SelectedIndex = GetNCFDWord("setting.gpu_mem_level", VideoFile); } catch { GT_NCF_Quality.SelectedIndex = -1; }
        }

        /// <summary>
        /// Проверяет наличие обновлений для программы. Используется в модуле автообновления.
        /// </summary>
        /// <param name="CurrentVersion">Текущая версия</param>
        /// <param name="ChURI">URL проверки обновлений</param>
        /// <param name="UserAgent">Заголовок HTTP UserAgent</param>
        /// <returns>Возвращает true при обнаружении обновлений</returns>
        private bool AutoUpdateCheck(string CurrentVersion, string ChURI, string UserAgent)
        {
            string NewVersion, DnlStr;
            using (WebClient Downloader = new WebClient())
            {
                Downloader.Headers.Add("User-Agent", UserAgent);
                DnlStr = Downloader.DownloadString(ChURI);
            }
            NewVersion = DnlStr.Substring(0, DnlStr.IndexOf("!"));
            return CoreLib.CompareVersions(CurrentVersion, NewVersion);
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
                if (Path.GetFileName(CFGFileName) == "config.cfg") { MessageBox.Show(CoreLib.GetLocalizedString("CE_RestConfigOpenWarn"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }

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
                                        CE_Editor.Rows.Add(ImpStr, "");
                                    }
                                }
                            }
                        }
                    }

                    // Изменяем содержимое строки статуса...
                    UpdateStatusBar(MainTabControl.SelectedIndex);
                }
                catch (Exception Ex)
                {
                    // Произошло исключение...
                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("CE_ExceptionDetected"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("CE_OpenFailed"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            string Buf = "";

            switch (FileName.Extension)
            {
                case ".reg":
                    Buf = CoreLib.GetLocalizedString("BU_BType_Reg");
                    if (BufName.IndexOf("Game_Options", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, CoreLib.GetLocalizedString("BU_BName_GRGame"), FileName.CreationTime);
                    }
                    else if (BufName.IndexOf("Source_Options", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, CoreLib.GetLocalizedString("BU_BName_SRCAll"), FileName.CreationTime);
                    }
                    else if (BufName.IndexOf("Steam_BackUp", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, CoreLib.GetLocalizedString("BU_BName_SteamAll"), FileName.CreationTime);
                    }
                    else if (BufName.IndexOf("Game_AutoBackUp", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, CoreLib.GetLocalizedString("BU_BName_GameAuto"), FileName.CreationTime);
                    }
                    break;
                case ".bud":
                    Buf = CoreLib.GetLocalizedString("BU_BType_Cont");
                    if (BufName.IndexOf(Properties.Resources.BU_PrefixDef, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, CoreLib.GetLocalizedString("BU_BName_Bud"), FileName.CreationTime);
                    }
                    else if (BufName.IndexOf(Properties.Resources.BU_PrefixCfg, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        Buf = CoreLib.GetLocalizedString("BU_BType_Cfg");
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, CoreLib.GetLocalizedString("BU_BName_Config"), FileName.CreationTime);
                    }
                    else if (BufName.IndexOf(Properties.Resources.BU_PrefixVChat, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        Buf = CoreLib.GetLocalizedString("BU_BType_DB");
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, CoreLib.GetLocalizedString("BU_BName_VChat"), FileName.CreationTime);
                    }
                    else if (BufName.IndexOf(Properties.Resources.BU_PrefixVideo, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        Buf = CoreLib.GetLocalizedString("BU_BType_Video");
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, CoreLib.GetLocalizedString("BU_BName_GRGame"), FileName.CreationTime);
                    }
                    else if (BufName.IndexOf(Properties.Resources.BU_PrefixVidAuto, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        Buf = CoreLib.GetLocalizedString("BU_BType_Video");
                        BufName = String.Format(Properties.Resources.BU_TablePrefix, CoreLib.GetLocalizedString("BU_BName_GameAuto"), FileName.CreationTime);
                    }
                    break;
                default:
                    Buf = CoreLib.GetLocalizedString("BU_BType_Unkn");
                    break;
            }

            return Tuple.Create(Buf, BufName);
        }

        /// <summary>
        /// Считывает файлы резервных копий из указанного каталога и помещает в таблицу.
        /// </summary>
        /// <param name="BUpDir">Путь к каталогу с резервными копиями</param>
        private void ReadBackUpList2Table(string BUpDir)
        {
            // Очистим таблицу...
            Invoke((MethodInvoker)delegate() { BU_LVTable.Items.Clear(); });
            
            // Открываем каталог...
            DirectoryInfo DInfo = new DirectoryInfo(BUpDir);
            
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
                Invoke((MethodInvoker)delegate() { BU_LVTable.Items.Add(LvItem); });
            }
        }

        /// <summary>
        /// Обнуляет контролы на странице графических настроек. Функция-заглушка.
        /// </summary>
        private void NullGraphOptions()
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

            // Отключим кнопку установки FPS-конфигов...
            FP_Install.Enabled = false;

            // Отключим контролы в менеджере HUD...
            HD_Install.Enabled = false;
            HD_Homepage.Enabled = false;
            HD_Uninstall.Enabled = false;
            HD_OpenDir.Enabled = false;
            HD_Warning.Visible = false;
            HD_GB_Pbx.Image = null;

            // Включаем заблокированные ранее контролы...
            MNUFPSWizard.Enabled = true;
            MNUInstaller.Enabled = true;
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
                App.FullSteamPath = CheckLastSteamPath(Properties.Settings.Default.LastSteamPath);
            }
            catch (FileNotFoundException Ex)
            {
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("SteamPathEnterErr"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
                Environment.Exit(7);
            }
            catch (OperationCanceledException Ex)
            {
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("SteamPathCancel"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
                Environment.Exit(19);
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("AppGenericError"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
                Environment.Exit(24);
            }
        }

        /// <summary>
        /// Устанавливает статус элементам управления, зависящим от платформы или спец. прав.
        /// </summary>
        /// <param name="State">Устанавливаемый статус</param>
        private void ChangePrvControlState(bool State)
        {
            MNUWinMnuDisabler.Enabled = State;
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
                        CoreLib.WriteStringToLog(String.Format(CoreLib.GetLocalizedString("AppNoGamesDLog"), App.FullSteamPath));
                        // Нет, не нашлись, выведем сообщение...
                        MessageBox.Show(CoreLib.GetLocalizedString("AppNoGamesDetected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Завершим работу приложения...
                        Environment.Exit(11);
                    }
                    break;
                case 1:
                    {
                        // При наличии единственной игры в списке, выберем её автоматически...
                        AppSelector.SelectedIndex = 0;
                        UpdateStatusBar(MainTabControl.SelectedIndex);
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
        private void CheckSymbolsSteam(string SteamPath)
        {
            if (!(CoreLib.CheckNonASCII(SteamPath)))
            {
                PS_PathSteam.ForeColor = Color.Red;
                PS_PathSteam.Image = Properties.Resources.upd_err;
                CoreLib.WriteStringToLog(String.Format(CoreLib.GetLocalizedString("AppRestrSymbLog"), SteamPath));
            }
        }

        /// <summary>
        /// Запускает проверку на наличие запрещённых символов в пути установки игры.
        /// </summary>
        /// <param name="GamePath">Каталог установки игры</param>
        private void CheckSymbolsGame(string GamePath)
        {
            if (!(CoreLib.CheckNonASCII(GamePath)))
            {
                PS_PathGame.ForeColor = Color.Red;
                PS_PathGame.Image = Properties.Resources.upd_err;
                CoreLib.WriteStringToLog(String.Format(CoreLib.GetLocalizedString("AppRestrSymbLog"), GamePath));
            }
        }

        /// <summary>
        /// Генерирует массив, содержащий пути к FPS-конфигам.
        /// </summary>
        /// <param name="GamePath">Каталог управляемого приложения</param>
        /// <param name="UserDir">Указывает использует ли управляемое приложение пользовательский каталог</param>
        /// <returns>Возвращает массив с сгенерированными путями до FPS-конфигов</returns>
        private List<String> ListFPSConfigs(string GamePath, bool UserDir)
        {
            List<String> Result = new List<String>();
            Result.Add(Path.Combine(GamePath, "cfg", "autoexec.cfg"));
            if (UserDir) { Result.Add(Path.Combine(GamePath, "custom", "autoexec.cfg")); }
            return Result;
        }

        /// <summary>
        /// Ищет файлы по указанным маскам в указанных каталогах
        /// </summary>
        /// <param name="CleanDirs">Каталоги для выполнения очистки с маской имени</param>
        /// <param name="IsRecursive">Включает / отключает рекурсивный поиск</param>
        /// <returns>Возвращает массив с именами файлов и полными путями</returns>
        private List<String> ExpandFileList(List<String> CleanDirs, bool IsRecursive)
        {
            List<String> Result = new List<String>();
            foreach (string DirMs in CleanDirs)
            {
                string CleanDir = Path.GetDirectoryName(DirMs);
                string CleanMask = Path.GetFileName(DirMs);
                if (Directory.Exists(CleanDir))
                {
                    try
                    {
                        DirectoryInfo DInfo = new DirectoryInfo(CleanDir);
                        FileInfo[] DirList = DInfo.GetFiles(CleanMask);
                        foreach (FileInfo DItem in DirList) { Result.Add(DItem.FullName); }

                        if (IsRecursive)
                        {
                            try
                            {
                                List<String> SubDirs = new List<string>();
                                foreach (DirectoryInfo Dir in DInfo.GetDirectories()) { SubDirs.Add(Path.Combine(Dir.FullName, CleanMask)); }
                                if (SubDirs.Count > 0) { Result.AddRange(ExpandFileList(SubDirs, true)); }
                            }
                            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                        }
                    }
                    catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                }
            }
            return Result;
        }

        /// <summary>
        /// Управляет выводом значка активного FPS-конфига и кнопки их удаления.
        /// </summary>
        /// <param name="GameDir">Полный путь к каталогу игры</param>
        /// <param name="UserDir">Флаг использования кастомного каталога</param>
        private void HandleConfigs(string GameDir, bool UserDir)
        {
            SelGame.FPSConfigs = ExpandFileList(ListFPSConfigs(GameDir, UserDir), true);
            GT_Warning.Visible = SelGame.FPSConfigs.Count > 0;
            FP_Uninstall.Enabled = SelGame.FPSConfigs.Count > 0;
        }

        /// <summary>
        /// Управляет видимостью специальных кнопок модуля Менеджер HUD.
        /// </summary>
        /// <param name="State">Статус выбранного HUD</param>
        private void SetHUDButtons(bool State)
        {
            HD_Install.Text = CoreLib.GetLocalizedString(State ? "HD_BtnUpdateText" : "HD_BtnInstallText");
            HD_Uninstall.Enabled = State;
            HD_OpenDir.Enabled = State;
        }

        /// <summary>
        /// Проверяет актуальность базы HUD.
        /// </summary>
        /// <param name="LastHUDUpdate">Дата последней проверки обновлений базы HUD</param>
        /// <returns>Булево актуальности базы HUD</returns>
        private bool CheckHUDDatabase(DateTime LastHUDUpdate)
        {
            return (DateTime.Now - LastHUDUpdate).Days >= 7;
        }

        /// <summary>
        /// Обновляет содержимое строки состояния в зависимости от контекста.
        /// </summary>
        /// <param name="Index">ID текущей вкладки</param>
        private void UpdateStatusBar(int Index)
        {
            switch (Index)
            {
                case 1: // Открыта страница "Редактор конфигов"...
                    {
                        MNUShowEdHint.Enabled = true;
                        SB_Status.ForeColor = Color.Black;
                        SB_Status.Text = String.Format(CoreLib.GetLocalizedString("StatusOpenedFile"), String.IsNullOrEmpty(CFGFileName) ? CoreLib.GetLocalizedString("UnnamedFileName") : Path.GetFileName(CFGFileName));
                    }
                    break;
                case 4:
                    {
                        bool HUDDbStatus = CheckHUDDatabase(Properties.Settings.Default.LastHUDTime);
                        MNUShowEdHint.Enabled = false;
                        SB_Status.ForeColor = HUDDbStatus ? Color.Red : Color.Black;
                        SB_Status.Text = String.Format(CoreLib.GetLocalizedString("HD_DynBarText"), CoreLib.GetLocalizedString(HUDDbStatus ? "HD_StatusOutdated" : "HD_StatusUpdated"), Properties.Settings.Default.LastHUDTime);
                    }
                    break;
                default: // Открыта другая страница...
                    {
                        MNUShowEdHint.Enabled = false;
                        SB_Status.ForeColor = Color.Black;
                        SB_Status.Text = CoreLib.GetLocalizedString("StatusNormal");
                    }
                    break;
            }
        }

        /// <summary>
        /// Проверяет верность заполнения графических настроек
        /// </summary>
        /// <param name="GameType">Тип игры: GCF или NCF</param>
        private bool ValidateGameSettings(bool GameType)
        {
            return !GameType ? ((GT_ScreenType.SelectedIndex != -1) && (GT_ModelQuality.SelectedIndex != -1)
                && (GT_TextureQuality.SelectedIndex != -1) && (GT_ShaderQuality.SelectedIndex != -1)
                && (GT_WaterQuality.SelectedIndex != -1) && (GT_ShadowQuality.SelectedIndex != -1)
                && (GT_ColorCorrectionT.SelectedIndex != -1) && (GT_AntiAliasing.SelectedIndex != -1)
                && (GT_Filtering.SelectedIndex != -1) && (GT_VSync.SelectedIndex != -1)
                && (GT_MotionBlur.SelectedIndex != -1) && (GT_DxMode.SelectedIndex != -1)
                && (GT_HDR.SelectedIndex != -1)) : ((GT_NCF_Quality.SelectedIndex != -1)
                && (GT_NCF_MemPool.SelectedIndex != -1) && (GT_NCF_EffectD.SelectedIndex != -1)
                && (GT_NCF_ShaderE.SelectedIndex != -1) && (GT_NCF_Multicore.SelectedIndex != -1)
                && (GT_NCF_VSync.SelectedIndex != -1) && (GT_NCF_Filtering.SelectedIndex != -1)
                && (GT_NCF_AntiAlias.SelectedIndex != -1) && (GT_NCF_DispMode.SelectedIndex != -1)
                && (GT_NCF_Ratio.SelectedIndex != -1) && (GT_NCF_Brightness.SelectedIndex != -1)
                && (GT_NCF_Shadows.SelectedIndex != -1) && (GT_NCF_MBlur.SelectedIndex != -1));
        }

        /// <summary>
        /// Закрывает все открытые в Редакторе конфигов файлы и очищает таблицу...
        /// </summary>
        private void CloseEditorConfigs()
        {
            CFGFileName = "";
            CE_Editor.Rows.Clear();
        }

        /// <summary>
        /// Получает список резеервных копий и заносит их в таблицу...
        /// </summary>
        private void UpdateBackUpList(string BackUpDir)
        {
            try
            {
                // Считываем и выводим в таблицу файлы резервных копий...
                ReadBackUpList2Table(BackUpDir);
            }
            catch (Exception Ex)
            {
                // Произошло исключение. Запишем в журнал...
                CoreLib.WriteStringToLog(Ex.Message);

                // Создадим каталог для хранения резервных копий если его ещё нет...
                if (!Directory.Exists(BackUpDir)) { Directory.CreateDirectory(BackUpDir); }
            }
        }

        /// <summary>
        /// Ищет установленные игры и выполняет ряд необходимых проверок.
        /// </summary>
        /// <param name="SteamDir">Путь к каталогу установки Steam</param>
        /// <param name="SteamApps">Название каталога SteamApps в зависимости от платформы</param>
        /// <param name="ErrMsg">Текст сообщения об ошибке</param>
        private void FindGames(string SteamDir, string SteamApps, string ErrMsg)
        {
            // Начинаем определять установленные игры...
            try { DetectInstalledGames(SteamDir, SteamApps); } catch (Exception Ex) { CoreLib.HandleExceptionEx(ErrMsg, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error); Environment.Exit(16); }

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
                    if (AutoUpdateCheck(App.AppVersionInfo, Properties.Resources.UpdateChURI, App.UserAgent))
                    {
                        // Доступны обновления...
                        MessageBox.Show(String.Format(CoreLib.GetLocalizedString("AppUpdateAvailable"), Properties.Resources.AppName), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                DirectoryInfo DInfo = new DirectoryInfo(Path.Combine(App.FullAppPath, "cfgs"));
                
                // Считываем список файлов по заданной маске...
                FileInfo[] DirList = DInfo.GetFiles("*.cfg");
                
                // Начинаем обход массива...
                foreach (FileInfo DItem in DirList)
                {
                    // Обрабатываем найденное...
                    if (DItem.Name != "config_default.cfg")
                    {
                        Invoke((MethodInvoker)delegate() { FP_ConfigSel.Items.Add((string)DItem.Name); });
                    }
                }
            }
            catch (Exception Ex)
            {
                // FPS-конфигов не найдено. Запишем в лог...
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
            // Получаем список резеверных копий...
            UpdateBackUpList(SelGame.FullBackUpDirPath);
        }

        private void BW_HUDList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Получаем полный список доступных HUD для данной игры. Открываем поток...
                using (FileStream XMLFS = new FileStream(Path.Combine(App.FullAppPath, Properties.Settings.Default.HUDDbFile), FileMode.Open, FileAccess.Read))
                {
                    // Загружаем XML из потока...
                    XmlDocument XMLD = new XmlDocument();
                    XMLD.Load(XMLFS);
                    
                    // Разбираем XML файл и обходим его в цикле...
                    for (int i = 0; i < XMLD.GetElementsByTagName("HUD").Count; i++)
                    {
                        // Выводим только HUD для выбранной в главном окне игры...
                        if (String.Compare(XMLD.GetElementsByTagName("Game")[i].InnerText, SelGame.SmallAppName, true) == 0)
                        {
                            Invoke((MethodInvoker)delegate() { HD_HSel.Items.Add(XMLD.GetElementsByTagName("Name")[i].InnerText); });
                        }
                    }
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void BW_HUDScreen_DoWork(object sender, DoWorkEventArgs e)
        {
            // Сгенерируем путь к файлу со скриншотом...
            string ScreenFile = Path.Combine(SelGame.AppHUDDir, Path.GetFileName(SelHUD.Preview));

            try
            {
                // Загрузим файл если не существует...
                if (!File.Exists(ScreenFile))
                {
                    using (WebClient Downloader = new WebClient())
                    {
                        Downloader.Headers.Add("User-Agent", App.UserAgent);
                        Downloader.DownloadFile(SelHUD.Preview, ScreenFile);
                    }
                }

                // Установим...
                Invoke((MethodInvoker)delegate() { HD_GB_Pbx.Image = Image.FromFile(ScreenFile); });
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
                Invoke((MethodInvoker)delegate() { HD_Install.Text = CoreLib.GetLocalizedString("HD_InstallBtnProgress"); HD_Install.Enabled = false; });

                // Устанавливаем и очищаем временный каталог...
                try { Directory.Move(Path.Combine(InstallTmp, SelHUD.FormatIntDir(SelHUD.ArchiveDir)), Path.Combine(SelGame.CustomInstallDir, SelHUD.InstallDir)); }
                finally { if (Directory.Exists(InstallTmp)) { Directory.Delete(InstallTmp, true); } }

                // Сохраняем или удаляем загруженный архив в зависимости от настроек приложения...
                if (!Properties.Settings.Default.HUDSaveArchives) { if (File.Exists(SelHUD.LocalFile)) { File.Delete(SelHUD.LocalFile); } }
            }
            finally
            {
                // Возвращаем сохранённый...
                Invoke((MethodInvoker)delegate() { HD_Install.Text = CaptText; HD_Install.Enabled = true; });
            }
        }

        private void BW_HudInstall_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Выводим сообщение...
            if (e.Error == null) { MessageBox.Show(CoreLib.GetLocalizedString("HD_InstallSuccessfull"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); } else { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("HD_InstallError"), Properties.Resources.AppName, e.Error.Message, e.Error.Source, MessageBoxIcon.Error); }

            // Включаем кнопку удаления если HUD установлен...
            SetHUDButtons(SelHUD.CheckInstalledHUD(SelGame.CustomInstallDir, SelHUD.InstallDir));
        }

        #endregion

        private void frmMainW_Load(object sender, EventArgs e)
        {
            // Событие инициализации формы...
            App = new CurrentApp();
            SourceGames = new List<SourceGame>();

            // Узнаем путь к установленному клиенту Steam...
            try { App.FullSteamPath = CoreLib.GetSteamPath(); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); ValidateAndHandle(); }

            // Начинаем платформо-зависимые процедуры...
            ChangePrvControlState(CoreLib.IsCurrentUserAdmin());

            // При работе отладочной версии запишем в лог путь к найденному Steam...
            #if DEBUG
            CoreLib.WriteStringToLog(String.Format(CoreLib.GetLocalizedString("AppSteamLocLog"), App.FullSteamPath));
            #endif

            // Сохраним последний путь к Steam в файл конфигурации...
            Properties.Settings.Default.LastSteamPath = App.FullSteamPath;

            // Вставляем информацию о версии в заголовок формы...
            #if DEBUG
            Text = String.Format(Text, Properties.Resources.AppName, Properties.Resources.PlatformFriendlyName, App.AppVersionInfo + " (debug)", CoreLib.GetSystemArch());
            #else
            Text = String.Format(Text, Properties.Resources.AppName, Properties.Resources.PlatformFriendlyName, App.AppVersionInfo, CoreLib.GetSystemArch());
            #endif

            // Укажем статус Безопасной очистки...
            CheckSafeClnStatus();

            // Укажем путь к Steam на странице "Устранение проблем"...
            PS_StPath.Text = String.Format(PS_StPath.Text, App.FullSteamPath);
            
            // Проверим на наличие запрещённых символов в пути к установленному клиенту Steam...
            CheckSymbolsSteam(App.FullSteamPath);

            // Распознаем файловую систему на диске со Steam...
            DetectFS(App.FullSteamPath);

            // Запустим поиск установленных игр и проверим нашлось ли что-то...
            FindGames(App.FullSteamPath, Properties.Resources.SteamAppsFolderName, CoreLib.GetLocalizedString("AppXMLParseError"));

            try
            {
                // Проверим наличие обновлений программы (если разрешено в настройках)...
                if (Properties.Settings.Default.EnableAutoUpdate)
                {
                    if (!BW_UpChk.IsBusy) { BW_UpChk.RunWorkerAsync(); }
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
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

            // Выбираем язык по умолчанию согласно языку приложения...
            PS_SteamLang.SelectedIndex = Convert.ToInt32(CoreLib.GetLocalizedString("AppDefaultSteamLangID"));

            // Управляем доступностью кнопки запуска очистки...
            PS_ExecuteNow.Enabled = PS_CleanRegistry.Checked || PS_CleanBlobs.Checked;
        }

        private void PS_ExecuteNow_Click(object sender, EventArgs e)
        {
            // Начинаем очистку...

            // Запрашиваем подтверждение у пользователя...
            if (MessageBox.Show(CoreLib.GetLocalizedString("PS_ExecuteMSG"), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Подтверждение получено...
                if ((PS_CleanBlobs.Checked) || (PS_CleanRegistry.Checked))
                {
                    // Найдём и завершим работу клиента Steam...
                    if (CoreLib.ProcessTerminate("Steam") != 0)
                    {
                        MessageBox.Show(CoreLib.GetLocalizedString("PS_ProcessDetected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Проверяем нужно ли чистить блобы...
                    if (PS_CleanBlobs.Checked)
                    {
                        try
                        {
                            // Чистим блобы...
                            CleanBlobsNow(App.FullSteamPath);
                        }
                        catch (Exception Ex)
                        {
                            CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("PS_CleanException"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
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
                                MessageBox.Show(CoreLib.GetLocalizedString("PS_NoLangSelected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                CleanRegistryNow(0);
                            }
                        }
                        catch (Exception Ex)
                        {
                            CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("PS_CleanException"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                        }
                    }

                    // Выполнение закончено, выведем сообщение о завершении...
                    MessageBox.Show(CoreLib.GetLocalizedString("PS_SeqCompleted"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Запустим Steam...
                    if (File.Exists(Path.Combine(App.FullSteamPath, Properties.Resources.SteamExecBin))) { Process.Start(Path.Combine(App.FullSteamPath, Properties.Resources.SteamExecBin)); }
                }
            }
        }

        private void frmMainW_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Запрашиваем подтверждение у пользователя на закрытие формы...
                e.Cancel = ((Properties.Settings.Default.ConfirmExit && !(MessageBox.Show(String.Format(CoreLib.GetLocalizedString("FrmCloseQuery"), Properties.Resources.AppName), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)) || (BW_BkUpRecv.IsBusy || BW_FPRecv.IsBusy || BW_HudInstall.IsBusy || BW_HUDList.IsBusy || BW_HUDScreen.IsBusy || BW_UpChk.IsBusy));
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
                CheckSymbolsGame(SelGame.FullGamePath);

                // Считаем настройки графики...
                if (SelGame.IsUsingVideoFile) { SelGame.UpdateVideoFilesList(SelGame.GameInternalID, App.FullSteamPath, SelGame.GamePath, SelGame.ConfDir); string VideoFile = SelGame.GetActualVideoFile(); if (File.Exists(VideoFile)) { ReadNCFGameSettings(VideoFile); } else { CoreLib.WriteStringToLog(String.Format(CoreLib.GetLocalizedString("AppVideoDbNotFound"), SelGame.FullAppName, VideoFile)); NullGraphOptions(); } } else { ReadGCFGameSettings(SelGame.SmallAppName); }

                // Переключаем графический твикер в режим GCF/NCF...
                SetGTOptsType(!SelGame.IsUsingVideoFile);

                // Проверим, установлен ли FPS-конфиг...
                HandleConfigs(SelGame.FullGamePath, SelGame.IsUsingUserDir);

                // Закроем открытые конфиги в редакторе...
                if (!(String.IsNullOrEmpty(CFGFileName))) { CloseEditorConfigs(); }

                // Считаем имеющиеся FPS-конфиги...
                if (!BW_FPRecv.IsBusy) { BW_FPRecv.RunWorkerAsync(); }

                // Обновляем статус...
                UpdateStatusBar(MainTabControl.SelectedIndex);

                // Сохраним ID последней выбранной игры...
                Properties.Settings.Default.LastGameName = AppSelector.Text;

                // Переключаем вид страницы менеджера HUD...
                HandleHUDMode(SelGame.IsHUDsAvailable);

                // Считаем список доступных HUD для данной игры...
                if (SelGame.IsHUDsAvailable) { if (!BW_HUDList.IsBusy) { BW_HUDList.RunWorkerAsync(); } }
                
                // Считаем список бэкапов...
                if (!BW_BkUpRecv.IsBusy) { BW_BkUpRecv.RunWorkerAsync(); }

                // Создадим каталоги...
                if (!Directory.Exists(SelGame.AppHUDDir)) { Directory.CreateDirectory(SelGame.AppHUDDir); }
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("AppFailedToGetData"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
        }

        private void AppRefresh_Click(object sender, EventArgs e)
        {
            // Попробуем обновить список игр...
            FindGames(App.FullSteamPath, Properties.Resources.SteamAppsFolderName, CoreLib.GetLocalizedString("AppXMLParseError"));
        }

        private void GT_Maximum_Graphics_Click(object sender, EventArgs e)
        {
            // Нажатие этой кнопки устанавливает графические настройки на рекомендуемый максимум...
            // Зададим вопрос, а нужно ли это юзеру?
            if (MessageBox.Show(CoreLib.GetLocalizedString("GT_MaxPerfMsg"), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!SelGame.IsUsingVideoFile)
                {
                    // Пользователь согласился, продолжаем...
                    GT_ScreenType.SelectedIndex = 0; // полноэкранный режим
                    GT_ModelQuality.SelectedIndex = 2; // высокая детализация моделей
                    GT_TextureQuality.SelectedIndex = 2; // высокая детализация текстур
                    GT_ShaderQuality.SelectedIndex = 1; // высокое качество шейдерных эффектов
                    GT_WaterQuality.SelectedIndex = 1; // отражать мир в воде
                    GT_ShadowQuality.SelectedIndex = 1; // высокое качество теней
                    GT_ColorCorrectionT.SelectedIndex = 1; // корренкция цвета включена
                    GT_AntiAliasing.SelectedIndex = 5; // сглаживание MSAA 8x
                    GT_Filtering.SelectedIndex = 5; // анизотропная фильтрация 16x
                    GT_VSync.SelectedIndex = 0; // вертикальная синхронизация выключена
                    GT_MotionBlur.SelectedIndex = 0; // размытие движения выключено
                    GT_DxMode.SelectedIndex = 3; // режим DirecX 9.0c
                    GT_HDR.SelectedIndex = 2; // HDR полные
                }
                else
                {
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
                }
                MessageBox.Show(CoreLib.GetLocalizedString("GT_PerfSet"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GT_Maximum_Performance_Click(object sender, EventArgs e)
        {
            // Нажатие этой кнопки устанавливает графические настройки на рекомендуемый минимум...
            // Спросим пользователя.
            if (MessageBox.Show(CoreLib.GetLocalizedString("GT_MinPerfMsg"), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!SelGame.IsUsingVideoFile)
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
                    if (MessageBox.Show(CoreLib.GetLocalizedString("GT_DxLevelMsg"), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                }
                MessageBox.Show(CoreLib.GetLocalizedString("GT_PerfSet"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GT_SaveApply_Click(object sender, EventArgs e)
        {
            // Сохраняем изменения в графических настройках...
            if (ValidateGameSettings(SelGame.IsUsingVideoFile))
            {
                // Запрашиваем подтверждение у пользователя...
                if (MessageBox.Show(CoreLib.GetLocalizedString("GT_SaveMsg"), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Определим тип игры...
                    if (!SelGame.IsUsingVideoFile)
                    {
                        // Это GCF-приложение, будем писать настройки в реестр...
                        // Создаём резервную копию...
                        if (Properties.Settings.Default.SafeCleanup)
                        {
                            try { CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve", "Source", SelGame.SmallAppName, "Settings"), "Game_AutoBackUp", SelGame.FullBackUpDirPath); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                        }

                        try
                        {
                            // Проверим существование ключа реестра и в случае необходимости создадим...
                            if (!(CoreLib.CheckIfHKCUSKeyExists(Path.Combine("Software", "Valve", "Source", SelGame.SmallAppName, "Settings")))) { Registry.CurrentUser.CreateSubKey(Path.Combine("Software", "Valve", "Source", SelGame.SmallAppName, "Settings")); }
                            
                            // Записываем выбранные настройки в реестр...
                            WriteGCFGameSettings(SelGame.SmallAppName);
                            
                            // Выводим подтверждающее сообщение...
                            MessageBox.Show(CoreLib.GetLocalizedString("GT_SaveSuccess"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception Ex) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("GT_SaveFailure"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
                    }
                    else
                    {
                        // Создадим бэкап файла с графическими настройками...
                        if (Properties.Settings.Default.SafeCleanup)
                        {
                            CoreLib.CreateConfigBackUp(SelGame.VideoCfgFiles, SelGame.FullBackUpDirPath, Properties.Resources.BU_PrefixVidAuto);
                        }

                        try
                        {
                            // Записываем...
                            WriteNCFGameSettings(SelGame.GetActualVideoFile());
                            
                            // Выводим сообщение...
                            MessageBox.Show(CoreLib.GetLocalizedString("GT_SaveSuccess"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception Ex)
                        {
                            // Произошла ошибка...
                            CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("GT_NCFFailure"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("GT_NCFNReady"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FP_ConfigSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Получаем описание выбранного пользователем FPS-конфига...
                FP_Description.Text = File.ReadAllText(Path.Combine(App.FullAppPath, "cfgs", String.Format("{0}_{1}.txt", Path.GetFileNameWithoutExtension(FP_ConfigSel.Text), CoreLib.GetLocalizedString("AppLangPrefix"))));
            }
            catch (Exception Ex)
            {
                // Не получилось загрузить описание выбранного конфига. Выведем стандартное сообщение...
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
                if (MessageBox.Show(CoreLib.GetLocalizedString("FP_InstallQuestion"), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Проверим, не нужно ли создавать резервную копию...
                    if (Properties.Settings.Default.SafeCleanup)
                    {
                        // Проверяем есть ли установленные конфиги...
                        if (SelGame.FPSConfigs.Count > 0)
                        {
                            // Создаём резервную копию...
                            CoreLib.CompressFiles(SelGame.FPSConfigs, CoreLib.GenerateBackUpFileName(SelGame.FullBackUpDirPath, Properties.Resources.BU_PrefixCfg));
                        }
                    }

                    try
                    {
                        // Устанавливаем...
                        InstallConfigNow(FP_ConfigSel.Text, App.FullAppPath, SelGame.FullGamePath, SelGame.IsUsingUserDir);
                        
                        // Выводим сообщение об успешной установке...
                        MessageBox.Show(CoreLib.GetLocalizedString("FP_InstallSuccessful"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Перечитаем конфиги...
                        HandleConfigs(SelGame.FullGamePath, SelGame.IsUsingUserDir);
                    }
                    catch (Exception Ex)
                    {
                        // Установка не удалась. Выводим сообщение об этом...
                        CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("FP_InstallFailed"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                // Пользователь не выбрал конфиг. Сообщим об этом...
                MessageBox.Show(CoreLib.GetLocalizedString("FP_NothingSelected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    CoreLib.OpenCleanupWindow(SelGame.FPSConfigs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("FP_RemoveSuccessful"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile, false, false, false, Properties.Settings.Default.SafeCleanup);

                    // Перечитаем список конфигов...
                    HandleConfigs(SelGame.FullGamePath, SelGame.IsUsingUserDir);
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("FP_RemoveNotExists"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("FP_RemoveFailed"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
            }
        }

        private void GT_Warning_Click(object sender, EventArgs e)
        {
            // Выдадим сообщение о наличии FPS-конфига...
            MessageBox.Show(CoreLib.GetLocalizedString("GT_FPSCfgDetected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void CE_New_Click(object sender, EventArgs e)
        {
            // Закрываем все открытые конфиги в Редакторе конфигов и создаём новый пустой файл...
            CloseEditorConfigs();

            // Обновляем содержимое строки статуса...
            UpdateStatusBar(MainTabControl.SelectedIndex);
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
                    if (File.Exists(CFGFileName)) { CoreLib.CreateConfigBackUp(CoreLib.SingleToArray(CFGFileName), SelGame.FullBackUpDirPath, Properties.Resources.BU_PrefixCfg); }
                }

                // Начинаем сохранение в тот же файл...
                try { WriteTableToFileNow(CFGFileName); } catch (Exception Ex) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("CE_CfgSVVEx"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
            }
            else
            {
                // Зададим стандартное имя (см. issue 21)...
                CE_SaveCfgDialog.FileName = File.Exists(Path.Combine(SelGame.FullCfgPath, "autoexec.cfg")) ? CoreLib.GetLocalizedString("UnnamedFileName") : "autoexec.cfg";

                // Файл не был открыт. Отображаем стандартный диалог сохранения файла...
                if (CE_SaveCfgDialog.ShowDialog() == DialogResult.OK)
                {
                    WriteTableToFileNow(CE_SaveCfgDialog.FileName);
                    CFGFileName = CE_SaveCfgDialog.FileName;
                    UpdateStatusBar(MainTabControl.SelectedIndex);
                }
            }
        }

        private void CE_SaveAs_Click(object sender, EventArgs e)
        {
            // Сохраняем файл с другим, выбранным пользователем, именем...
            CE_SaveCfgDialog.InitialDirectory = SelGame.FullCfgPath;
            if (CE_SaveCfgDialog.ShowDialog() == DialogResult.OK)
            {
                WriteTableToFileNow(CE_SaveCfgDialog.FileName);
            }
        }

        private void PS_RemCustMaps_Click(object sender, EventArgs e)
        {
            // Удаляем кастомные (нестандартные) карты...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "custom", "*.bsp"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "download", "*.bsp"));
            CleanDirs.Add(Path.Combine(SelGame.AppWorkshopDir, "*.bsp"));
            if (Properties.Settings.Default.AllowUnSafeCleanup) { CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "maps", "*.bsp")); }
            CoreLib.OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("PS_CleanupSuccess"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemDnlCache_Click(object sender, EventArgs e)
        {
            // Удаляем кэш загрузок...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "download", "*.*"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "downloads", "*.*"));
            CoreLib.OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("PS_CleanupSuccess"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemSoundCache_Click(object sender, EventArgs e)
        {
            // Удаляем звуковой кэш...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "maps", "graphs", "*.*"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "maps", "soundcache", "*.*"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "download", "sound", "*.*"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "*.cache"));
            CoreLib.OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("PS_CleanupSuccess"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemScreenShots_Click(object sender, EventArgs e)
        {
            // Удаляем все скриншоты...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "screenshots", "*.*"));
            CoreLib.OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("PS_CleanupSuccess"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile, false, false, false);
        }

        private void PS_RemDemos_Click(object sender, EventArgs e)
        {
            // Удаляем все записанные демки...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "*.dem"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "*.mp4"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "*.tga"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "*.wav"));
            CoreLib.OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("PS_CleanupSuccess"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile, false, false, false, false);
        }

        private void PS_RemGraphOpts_Click(object sender, EventArgs e)
        {
            // Удаляем графические настройки...
            if (MessageBox.Show(((Button)sender).Text + "?", Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (!SelGame.IsUsingVideoFile)
                {
                    // Создаём резервную копию...
                    if (Properties.Settings.Default.SafeCleanup)
                    {
                        try { CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve", "Source", SelGame.SmallAppName, "Settings"), "Game_AutoBackUp", SelGame.FullBackUpDirPath); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    }

                    // Работаем...
                    try
                    {
                        // Удаляем ключ HKEY_CURRENT_USER\Software\Valve\Source\tf\Settings из реестра...
                        Registry.CurrentUser.DeleteSubKeyTree(Path.Combine("Software", "Valve", "Source", SelGame.SmallAppName, "Settings"), false);
                        MessageBox.Show(CoreLib.GetLocalizedString("PS_CleanupSuccess"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception Ex)
                    {
                        CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("PS_CleanupErr"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // Создадим бэкап файла с графическими настройками...
                    if (Properties.Settings.Default.SafeCleanup)
                    {
                        CoreLib.CreateConfigBackUp(SelGame.VideoCfgFiles, SelGame.FullBackUpDirPath, Properties.Resources.BU_PrefixVidAuto);
                    }

                    // Удаляем файлы...
                    CoreLib.RemoveFiles(SelGame.VideoCfgFiles);

                    // Выводим сообщение...
                    MessageBox.Show(CoreLib.GetLocalizedString("PS_CleanupSuccess"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void PS_RemOldBin_Click(object sender, EventArgs e)
        {
            // Удаляем старые бинарники...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(SelGame.GamePath, "bin", "*.*"));
            CleanDirs.Add(Path.Combine(SelGame.GamePath, "platform", "*.*"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "bin", "*.*"));
            CleanDirs.Add(Path.Combine(SelGame.GamePath, "*.exe"));
            CoreLib.OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("PS_CacheChkReq"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_ResetSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(((Button)sender).Text + "?", Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try { Process.Start(String.Format("steam://validate/{0}", SelGame.GameInternalID)); } catch (Exception Ex) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("AppStartSteamFailed"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
            }
        }

        private void MNUReportBuilder_Click(object sender, EventArgs e)
        {
            if ((AppSelector.Items.Count > 0) && (AppSelector.SelectedIndex != -1))
            {
                // Запускаем форму создания отчёта для Техподдержки...
                using (frmRepBuilder RBF = new frmRepBuilder(App.AppUserDir, App.FullSteamPath, SelGame.FullCfgPath))
                {
                    RBF.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("AppNoGamesSelected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUInstaller_Click(object sender, EventArgs e)
        {
            // Запускаем форму установщика спреев, демок и конфигов...
            using (frmInstaller InstF = new frmInstaller(SelGame.FullGamePath, SelGame.IsUsingUserDir, SelGame.CustomInstallDir))
            {
                InstF.ShowDialog();
            }
        }

        private void MNUExit_Click(object sender, EventArgs e)
        {
            // Завершаем работу программы...
            Environment.Exit(0);
        }

        private void MNUFPSWizard_Click(object sender, EventArgs e)
        {
            // Очистим Редактор конфигов...
            CloseEditorConfigs();
            
            // Запускаем форму мастера FPS-конфигов...
            using (frmFPGen FPFrm = new frmFPGen(new CoreLib.CFGEdDelegate(AddRowToTable)))
            {
                FPFrm.ShowDialog();
            }

            // Переключаемся на вкладку Редактор конфигов...
            MainTabControl.SelectedIndex = 1;
        }

        private void MNUAbout_Click(object sender, EventArgs e)
        {
            // Отобразим форму "О программе"...
            using (frmAbout AboutFrm = new frmAbout())
            {
                AboutFrm.ShowDialog();
            }
        }

        private void MNUReportBug_Click(object sender, EventArgs e)
        {
            // Отобразим форму сообщения об ошибках...
            using (frmBugReporter BgRepFrm = new frmBugReporter(App.UserAgent))
            {
                BgRepFrm.ShowDialog();
            }
        }

        private void BUT_Refresh_Click(object sender, EventArgs e)
        {
            // Обновим список резервных копий...
            UpdateBackUpList(SelGame.FullBackUpDirPath);
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
                    if (MessageBox.Show(String.Format(CoreLib.GetLocalizedString("BU_QMsg"), Path.GetFileNameWithoutExtension(FName), BU_LVTable.SelectedItems[0].SubItems[3].Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Проверяем что восстанавливать: конфиг или реестр...
                        switch (Path.GetExtension(FName))
                        {
                            case ".reg":
                                // Восстанавливаем файл реестра...
                                try
                                {
                                    // Восстанавливаем...
                                    Process.Start("regedit.exe", String.Format("/s \"{0}\"", Path.Combine(SelGame.FullBackUpDirPath, FName)));
                                    
                                    // Показываем сообщение об успешном восстановлении...
                                    MessageBox.Show(CoreLib.GetLocalizedString("BU_RestSuccessful"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                catch (Exception Ex)
                                {
                                    // Произошло исключение...
                                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("BU_RestFailed"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                                }
                                break;
                            case ".bud":
                                // Распаковываем архив с выводом прогресса...
                                CoreLib.ExtractFiles(Path.Combine(SelGame.FullBackUpDirPath, FName), Path.GetPathRoot(App.FullSteamPath));
                                
                                // Обновляем список FPS-конфигов...
                                HandleConfigs(SelGame.FullGamePath, SelGame.IsUsingUserDir);
                                
                                // Выводим сообщение об успехе...
                                MessageBox.Show(CoreLib.GetLocalizedString("BU_RestSuccessful"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            default:
                                // Выводим сообщение о неизвестном формате резервной копии...
                                MessageBox.Show(CoreLib.GetLocalizedString("BU_UnknownType"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_NoSelected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("BU_NoFiles"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    if (MessageBox.Show(CoreLib.GetLocalizedString("BU_DelMsg"), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        try
                        {
                            // Удаляем файл...
                            File.Delete(Path.Combine(SelGame.FullBackUpDirPath, FName));
                            
                            // Удаляем строку...
                            BU_LVTable.Items.Remove(BU_LVTable.SelectedItems[0]);
                            
                            // Показываем сообщение об успешном удалении...
                            MessageBox.Show(CoreLib.GetLocalizedString("BU_DelSuccessful"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception Ex)
                        {
                            // Произошло исключение при попытке удаления файла резервной копии...
                            CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("BU_DelFailed"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_NoSelected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("BU_NoFiles"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BUT_CrBkupReg_ButtonClick(object sender, EventArgs e)
        {
            BUT_CrBkupReg.ShowDropDown();
        }

        private void BUT_L_GameSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(CoreLib.GetLocalizedString("BU_RegCreate"), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Создадим резервную копию графических настроек игры...
                try
                {
                    if (!SelGame.IsUsingVideoFile)
                    {
                        // Создаём конфиг ветки реестра...
                        CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve", "Source", SelGame.SmallAppName, "Settings"), "Game_Options", SelGame.FullBackUpDirPath);
                    }
                    else
                    {
                        // Проверяем существование файла с графическими настройками игры...
                        CoreLib.CreateConfigBackUp(SelGame.VideoCfgFiles, SelGame.FullBackUpDirPath, Properties.Resources.BU_PrefixVideo);
                    }
                    
                    // Выводим сообщение об успехе...
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_RegDone"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Обновляем список резервных копий...
                    UpdateBackUpList(SelGame.FullBackUpDirPath);
                }
                catch (Exception Ex)
                {
                    // Выводим сообщение об ошибке и пишем в журнал отладки...
                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("BU_RegErr"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        private void BUT_L_AllSteam_Click(object sender, EventArgs e)
        {
            // Создадим резервную копию всех настроек Steam...
            if (MessageBox.Show(CoreLib.GetLocalizedString("BU_RegCreate"), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // Создаём...
                    CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve"), "Steam_BackUp", SelGame.FullBackUpDirPath);
                    
                    // Выводим сообщение...
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_RegDone"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Обновим список бэкапов...
                    UpdateBackUpList(SelGame.FullBackUpDirPath);
                }
                catch (Exception Ex)
                {
                    // Произошло исключение, уведомим пользователя...
                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("BU_RegErr"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        private void BUT_L_AllSRC_Click(object sender, EventArgs e)
        {
            // Созданим резервную копию графических настроек всех Source-игр...
            if (MessageBox.Show(CoreLib.GetLocalizedString("BU_RegCreate"), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", "Software", "Valve", "Source"), "Source_Options", SelGame.FullBackUpDirPath);
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_RegDone"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateBackUpList(SelGame.FullBackUpDirPath);
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("BU_RegErr"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStatusBar(((TabControl)sender).SelectedIndex);
        }

        private void CE_ShowHint_Click(object sender, EventArgs e)
        {
            try
            {
                string Buf = CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[0].Value.ToString();
                if (!(String.IsNullOrEmpty(Buf))) { Buf = GetCVDescription(Buf); if (!(String.IsNullOrEmpty(Buf))) { MessageBox.Show(Buf, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); } else { MessageBox.Show(CoreLib.GetLocalizedString("CE_ClNoDescr"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); } } else { MessageBox.Show(CoreLib.GetLocalizedString("CE_ClSelErr"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            catch
            {
                MessageBox.Show(CoreLib.GetLocalizedString("CE_ClSelErr"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUHelp_Click(object sender, EventArgs e)
        {
            CoreLib.OpenWebPage(Properties.Resources.AppURLHelpSite);
        }

        private void MNUOpinion_Click(object sender, EventArgs e)
        {
            CoreLib.OpenWebPage(Properties.Resources.AppURLReply);
        }

        private void MNUSteamGroup_Click(object sender, EventArgs e)
        {
            try { Process.Start(Properties.Resources.AppURLSteamGrID); } catch { CoreLib.OpenWebPage(Properties.Resources.AppURLSteamGroup); }
        }

        private void MNULnkEasyCoding_Click(object sender, EventArgs e)
        {
            CoreLib.OpenWebPage(Properties.Resources.AppURLOffSite);
        }

        private void MNULnkTFRU_Click(object sender, EventArgs e)
        {
            CoreLib.OpenWebPage(Properties.Resources.AppURLSpnTFSU);
        }

        private void MNUHEd_Click(object sender, EventArgs e)
        {
            // Отобразим форму редактора файла hosts...
            using (frmHEd HEdFrm = new frmHEd())
            {
                HEdFrm.ShowDialog();
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
            // Сгенерируем путь к файлу...
            string ConfigFile = Path.Combine(App.FullAppPath, "cfgs", FP_ConfigSel.Text);
            
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
                CoreLib.OpenTextEditor(ConfigFile);
            }
        }

        private void MNUUpdateCheck_Click(object sender, EventArgs e)
        {
            // Откроем форму модуля проверки обновлений...
            using (frmUpdate UpdFrm = new frmUpdate(App.UserAgent, App.FullAppPath, App.AppVersionInfo, App.AppUserDir))
            {
                UpdFrm.ShowDialog();
            }
            
            // Перечитаем базу игр...
            FindGames(App.FullSteamPath, Properties.Resources.SteamAppsFolderName, CoreLib.GetLocalizedString("AppXMLParseError"));
        }

        private void BUT_OpenNpad_Click(object sender, EventArgs e)
        {
            // Откроем выбранный бэкап в Блокноте Windows...
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    if (Regex.IsMatch(Path.GetExtension(BU_LVTable.SelectedItems[0].SubItems[4].Text), @"\.(txt|cfg|[0-9]|reg)")) { CoreLib.OpenTextEditor(Path.Combine(SelGame.FullBackUpDirPath, BU_LVTable.SelectedItems[0].SubItems[4].Text)); } else { MessageBox.Show(CoreLib.GetLocalizedString("BU_BinaryFile"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_NoSelected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("BU_NoFiles"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUAppOptions_Click(object sender, EventArgs e)
        {
            // Показываем форму настроек...
            using (frmOptions OptsFrm = new frmOptions())
            {
                OptsFrm.ShowDialog();
            }
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
                    CoreLib.OpenExplorer(Path.Combine(SelGame.FullBackUpDirPath, BU_LVTable.SelectedItems[0].SubItems[4].Text));
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("BU_NoSelected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("BU_NoFiles"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            using (frmKBHelper KBHlp = new frmKBHelper())
            {
                KBHlp.ShowDialog();
            }
        }

        private void CE_OpenInNotepad_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(CFGFileName)))
            {
                CoreLib.OpenTextEditor(CFGFileName);
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("CE_NoFileOpened"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PS_PathDetector_Click(object sender, EventArgs e)
        {
            if (((Label)sender).ForeColor == Color.Red) { MessageBox.Show(CoreLib.GetLocalizedString("SteamNonASCIIDetected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); } else { MessageBox.Show(CoreLib.GetLocalizedString("SteamNonASCIINotDetected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void PS_PathGame_Click(object sender, EventArgs e)
        {
            if (((Label)sender).ForeColor == Color.Red) { MessageBox.Show(CoreLib.GetLocalizedString("GameNonASCIIDetected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); } else { MessageBox.Show(CoreLib.GetLocalizedString("GameNonASCIINotDetected"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void PS_RemReplays_Click(object sender, EventArgs e)
        {
            // Удаляем все реплеи...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "replay", "*.*"));
            CoreLib.OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("PS_CleanupSuccess"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemTextures_Click(object sender, EventArgs e)
        {
            // Удаляем все кастомные текстуры...
            List<String> CleanDirs = new List<string>();
            
            // Чистим базы игр со старой системой. Удалить после полного перехода на новую...
            if (Properties.Settings.Default.AllowUnSafeCleanup)
            {
                CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "materials", "*.*"));
                CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "models", "*.*"));
            }
            
            // Чистим загруженные с серверов модели и текстуры...
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "download", "*.vt*"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "download", "*.vmt"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "download", "*.mdl"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "download", "*.phy"));
            
            // Чистим установленные пользователем модели и текстуры...
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "custom", "*.vt*"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "custom", "*.vmt"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "custom", "*.mdl"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "custom", "*.phy"));
            CoreLib.OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("PS_CleanupSuccess"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemSecndCache_Click(object sender, EventArgs e)
        {
            // Удаляем содержимое вторичного кэша загрузок...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "cache", "*.*")); // Кэш...
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "custom", "user_custom", "*.*")); // Кэш спреев игр с н.с.к...
            CleanDirs.Add(Path.Combine(SelGame.GamePath, "config", "html", "*.*")); // Кэш MOTD...
            CoreLib.OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("PS_CleanupSuccess"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void SB_App_DoubleClick(object sender, EventArgs e)
        {
            // Переключим статус безопасной очистки...
            Properties.Settings.Default.SafeCleanup = !Properties.Settings.Default.SafeCleanup;
            
            // Сообщим пользователю если он отключил безопасную очистку...
            if (!Properties.Settings.Default.SafeCleanup)
            {
                MessageBox.Show(CoreLib.GetLocalizedString("AppSafeClnDisabled"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            // Обновим статусную строку...
            CheckSafeClnStatus();
        }

        private void CE_OpenCVList_Click(object sender, EventArgs e)
        {
            CoreLib.OpenWebPage(CoreLib.GetLocalizedString("AppCVListURL"));
        }

        private void CE_ManualBackUpCfg_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(CFGFileName)))
            {
                if (File.Exists(CFGFileName))
                {
                    CoreLib.CreateConfigBackUp(CoreLib.SingleToArray(CFGFileName), SelGame.FullBackUpDirPath, Properties.Resources.BU_PrefixCfg);
                    MessageBox.Show(String.Format(CoreLib.GetLocalizedString("CE_BackUpCreated"), Path.GetFileName(CFGFileName)), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("CE_NoFileOpened"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PS_RemSounds_Click(object sender, EventArgs e)
        {
            // Удаляем кастомные звуки...
            List<String> CleanDirs = new List<string>();
            if (Properties.Settings.Default.AllowUnSafeCleanup) { CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "sound", "*.*")); }
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "download", "*.mp3"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "download", "*.wav"));
            CoreLib.OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("PS_CleanupSuccess"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemCustDir_Click(object sender, EventArgs e)
        {
            // Удаляем пользовательного каталога...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "custom", "*.*"));
            CleanDirs.Add(Path.Combine(SelGame.AppWorkshopDir, "*.*"));
            CoreLib.OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("PS_CleanupSuccess"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_DeepCleanup_Click(object sender, EventArgs e)
        {
            // Проведём глубокую очистку...
            List<String> CleanDirs = new List<string>();
            
            // Удалим старые бинарники и лаунчеры...
            CleanDirs.Add(Path.Combine(SelGame.GamePath, "bin", "*.*"));
            CleanDirs.Add(Path.Combine(SelGame.GamePath, "platform", "*.*"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "bin", "*.*"));
            CleanDirs.Add(Path.Combine(SelGame.GamePath, "*.exe"));
            
            // Удалим кэш загрузок...
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "download", "*.*"));
            
            // Удалим кастомные файлы...
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "custom", "*.*"));
            CleanDirs.Add(Path.Combine(SelGame.AppWorkshopDir, "*.*"));
            
            // Удалим другие кэши...
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "cache", "*.*"));
            
            // Удалим кэш MOTD...
            CleanDirs.Add(Path.Combine(SelGame.GamePath, "config", "html", "*.*"));
            
            // Удалим пользовательские конфиги...
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "cfg", "*.*"));
            CoreLib.OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("PS_CacheChkReq"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void PS_RemConfigs_Click(object sender, EventArgs e)
        {
            // Удаляем пользовательного каталога...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "cfg", "*.*"));
            CleanDirs.Add(Path.Combine(SelGame.FullGamePath, "custom", "*.cfg"));
            CoreLib.OpenCleanupWindow(CleanDirs, ((Button)sender).Text.ToLower(), CoreLib.GetLocalizedString("PS_CleanupSuccess"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void GT_ResAvailable_Btn_Click(object sender, EventArgs e)
        {
            // Получим список доступных разрешений...
            List<String> Resolutions = CoreLib.GetDesktopResolutions();

            // Очистим список...
            GT_ResAvailable.Items.Clear();

            // Пройдём массив в цикле...
            foreach (string CRes in Resolutions)
            {
                GT_ResAvailable.Items.Add(CRes);
            }

            // Если нашли, включим контрол выбора...
            GT_ResAvailable.Enabled = GT_ResAvailable.Items.Count > 0;
        }

        private void GT_ResAvailable_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((ComboBox)sender).Items.Count > 0)
                {
                    string[] CR = ((ComboBox)sender).Text.Substring(0, ((ComboBox)sender).Text.IndexOf('@')).Split('x');
                    if (CR.Length >= 2) { if (!SelGame.IsUsingVideoFile) { GT_ResHor.Value = Convert.ToInt32(CR[0]); GT_ResVert.Value = Convert.ToInt32(CR[1]); } else { GT_NCF_HorRes.Value = Convert.ToInt32(CR[0]); GT_NCF_VertRes.Value = Convert.ToInt32(CR[1]); } }
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void HD_HSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получим информацию о выбранном HUD...
            try { SelHUD = new HUDTlx(HD_HSel.Text, App.FullAppPath, SelGame.AppHUDDir); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                
            // Проверяем результат...
            bool Success = !String.IsNullOrEmpty(SelHUD.Name);

            // Переключаем статус элементов управления...
            HD_GB_Pbx.Image = Properties.Resources.LoadingFile;
            HD_Install.Enabled = Success;
            HD_Homepage.Enabled = Success;
            HD_Warning.Visible = Success && !SelHUD.IsUpdated;

            // Проверяем установлен ли выбранный HUD...
            SetHUDButtons(SelHUD.CheckInstalledHUD(SelGame.CustomInstallDir, SelHUD.InstallDir));

            // Загрузим скриншот выбранного HUD...
            if (Success && !BW_HUDScreen.IsBusy) { BW_HUDScreen.RunWorkerAsync(); }
        }

        private void HD_Install_Click(object sender, EventArgs e)
        {
            if (!CheckHUDDatabase(Properties.Settings.Default.LastHUDTime))
            {
                // Проверим поддерживает ли выбранный HUD последнюю версию игры...
                if (SelHUD.IsUpdated)
                {
                    // Спросим пользователя о необходимости установки/обновления HUD...
                    if (MessageBox.Show(String.Format("{0}?", ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Проверим установлен ли выбранный HUD...
                        if (SelHUD.CheckInstalledHUD(SelGame.CustomInstallDir, SelHUD.InstallDir))
                        {
                            // Удаляем уже установленные файлы HUD...
                            CoreLib.RemoveDirectoryEx(CoreLib.SingleToArray(Path.Combine(SelGame.CustomInstallDir, SelHUD.InstallDir)));
                        }

                        // Начинаем загрузку если файл не существует...
                        if (!File.Exists(SelHUD.LocalFile)) { CoreLib.DownloadFileEx(Properties.Settings.Default.HUDUseUpstream ? SelHUD.UpURI : SelHUD.URI, SelHUD.LocalFile); }

                        // Распаковываем загруженный архив с файлами HUD...
                        CoreLib.ExtractFiles(SelHUD.LocalFile, Path.Combine(SelGame.CustomInstallDir, "hudtemp"));

                        // Запускаем установку пакета в отдельном потоке...
                        if (!BW_HudInstall.IsBusy) { BW_HudInstall.RunWorkerAsync(); }
                    }
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("HD_Outdated"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("HD_DbOutdated"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void HD_Uninstall_Click(object sender, EventArgs e)
        {
            // Спросим пользователя о необходимости удаления HUD...
            if (MessageBox.Show(String.Format("{0}?", ((Button)sender).Text), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Сгенерируем полный путь к установленному HUD...
                string HUDPath = Path.Combine(SelGame.CustomInstallDir, SelHUD.InstallDir);

                // Воспользуемся модулем быстрой очистки для удаления выбранного HUD...
                CoreLib.RemoveDirectoryEx(CoreLib.SingleToArray(HUDPath));

                // Проверяем установлен ли выбранный HUD...
                bool IsInstalled = SelHUD.CheckInstalledHUD(SelGame.CustomInstallDir, SelHUD.InstallDir);

                // При успешном удалении HUD выводим сообщение и сносим и его каталог...
                if (!IsInstalled) { MessageBox.Show(CoreLib.GetLocalizedString("PS_CleanupSuccess"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); if (Directory.Exists(HUDPath)) { Directory.Delete(HUDPath); } }

                // Включаем / отключаем кнопки...
                SetHUDButtons(IsInstalled);
            }
        }

        private void HD_Homepage_Click(object sender, EventArgs e)
        {
            // Откроем домашнюю страницу выбранного HUD...
            if (!String.IsNullOrEmpty(SelHUD.Site)) { CoreLib.OpenWebPage(SelHUD.Site); }
        }

        private void MNUExtClnAppCache_Click(object sender, EventArgs e)
        {
            // Очистим загруженные приложением файлы...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(App.AppUserDir, Properties.Settings.Default.HUDLocalDir, "*.*"));
            CoreLib.OpenCleanupWindow(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", ""), CoreLib.GetLocalizedString("PS_CleanupSuccess"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void MNUExtClnTmpDir_Click(object sender, EventArgs e)
        {
            // Очистим каталоги с временными файлами системы...
            List<String> CleanDirs = new List<string>();
            CleanDirs.Add(Path.Combine(Path.GetTempPath(), "*.*"));
            CoreLib.OpenCleanupWindow(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", ""), CoreLib.GetLocalizedString("PS_CleanupSuccess"), SelGame.FullBackUpDirPath, SelGame.GameBinaryFile);
        }

        private void MNUShowLog_Click(object sender, EventArgs e)
        {
            // Выведем на экран содержимое отладочного журнала...
            if (Properties.Settings.Default.EnableDebugLog)
            {
                string DFile = Path.Combine(CoreLib.GetApplicationPath(), Properties.Settings.Default.DebugLogFileName);
                if (File.Exists(DFile)) { using (frmLogView Lv = new frmLogView(DFile)) { Lv.ShowDialog(); } } else { MessageBox.Show(CoreLib.GetLocalizedString("AppNoDebugFile"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("AppDebugDisabled"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void HD_Warning_Click(object sender, EventArgs e)
        {
            // Выведем предупреждающие сообщения...
            if (!SelHUD.IsUpdated) { MessageBox.Show(CoreLib.GetLocalizedString("HD_NotTested"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void HD_OpenDir_Click(object sender, EventArgs e)
        {
            // Покажем файлы установленного HUD в Проводнике...
            CoreLib.OpenExplorer(Path.Combine(SelGame.CustomInstallDir, SelHUD.InstallDir));
        }

        private void MNUExtClnSteam_Click(object sender, EventArgs e)
        {
            // Запустим модуль очистки кэшей Steam...
            using (FrmStmClean StmCln = new FrmStmClean(App.FullSteamPath, SelGame.FullBackUpDirPath))
            {
                StmCln.ShowDialog();
            }
        }

        private void MNUMuteMan_Click(object sender, EventArgs e)
        {
            // Запустим менеджер управления отключёнными игроками...
            using (FrmMute FMm = new FrmMute(SelGame.BanlistFileName, SelGame.FullBackUpDirPath))
            {
                FMm.ShowDialog();
            }
        }
    }
}
