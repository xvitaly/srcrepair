/*
 * Класс SourceGame SRC Repair.
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
using System.IO;

namespace srcrepair
{
    public sealed class SourceGame
    {
        /// <summary>
        /// В этой переменной будем хранить путь установки кастомных файлов.
        /// </summary>
        public string CustomInstallDir { get; private set; }

        /// <summary>
        /// В этой переменной будем хранить полный путь к каталогу игры, которой
        /// мы будем управлять данной утилитой.
        /// </summary>
        public string FullGamePath { get; private set; }

        /// <summary>
        /// В этой переменной будем хранить полный путь к каталогу игры без
        /// включения в путь GV.SmallAppName для служебных целей.
        /// </summary>
        public string GamePath { get; private set; }

        /// <summary>
        /// В этой переменной будем хранить полное имя управляемого приложения
        /// для служебных целей.
        /// </summary>
        public string FullAppName { get; private set; }

        /// <summary>
        /// В этой переменной мы будем хранить краткое имя управляемого приложения
        /// для служебных целей (SteamAlias).
        /// </summary>
        public string SmallAppName { get; private set; }

        /// <summary>
        /// В этой переменной мы будем хранить имя главного процесса игры.
        /// </summary>
        public string GameBinaryFile { get; private set; }

        /// <summary>
        /// В этой переменной мы будем пути к каталогам с облачными конфигами.
        /// </summary>
        public List<String> CloudConfigs { get; private set; }

        /// <summary>
        /// В этой переменной мы будем хранить полный путь до каталога с
        /// файлами конфигурации управляемого приложения.
        /// </summary>
        public string FullCfgPath { get; private set; }

        /// <summary>
        /// В этой переменной мы будем хранить полный путь до каталога с
        /// резервными копиями управляемого приложения.
        /// </summary>
        public string FullBackUpDirPath { get; private set; }

        /// <summary>
        /// Указывает использует ли игра файл video.txt для хранения
        /// своих настроек.
        /// </summary>
        public bool IsUsingVideoFile { get; private set; }

        /// <summary>
        /// Определяет использует ли игра специальный каталог для хранения
        /// пользовательских настроек и скриптов.
        /// </summary>
        public bool IsUsingUserDir { get; private set; }

        /// <summary>
        /// Указывает поддерживает ли конкретная игра кастомные HUD и имеется
        /// ли их поддержка в SRC Repair.
        /// </summary>
        public bool IsHUDsAvailable { get; private set; }

        /// <summary>
        /// Эта переменная хранит ID игры по базе данных Steam. Используется
        /// для служебных целей.
        /// </summary>
        public string GameInternalID { get; private set; }

        /// <summary>
        /// В этом списке хранятся пути ко всем найденным файлами
        /// с графическими настройками.
        /// </summary>
        public List<String> VideoCfgFiles { get; private set; }

        /// <summary>
        /// Содержит имя каталога с графическими настройками NCF игр. Используется
        /// в последних играх.
        /// </summary>
        public string ConfDir { get; private set; }

        /// <summary>
        /// В этой переменной будем хранить путь до каталога локального хранения
        /// загруженных файлов HUD и их мета-информации.
        /// </summary>
        public string AppHUDDir { get; private set; }

        /// <summary>
        /// Содержит пути к установленным FPS-конфигам управляемой игры.
        /// </summary>
        public List<String> FPSConfigs { get; set; }

        /// <summary>
        /// Содержит список доступных HUD для управляемой игры.
        /// </summary>
        public HUDManager HUDMan { get; set; }

        /// <summary>
        /// Содержит список доступных HUD для управляемой игры.
        /// </summary>
        public ConfigManager CFGMan { get; set; }

        /// <summary>
        /// Содержит путь к каталогу с загруженными данными из Steam Workshop.
        /// </summary>
        public string AppWorkshopDir { get; private set; }

        /// <summary>
        /// Содержит путь к файлу со списком заглушенных пользователей.
        /// </summary>
        public string BanlistFileName { get; private set; }

        /// <summary>
        /// Указывает установлено ли данное приложение.
        /// </summary>
        public bool IsInstalled { get; private set; }

        private string SteamPath;

        /// <summary>
        /// Генерирует путь к каталогу установки игры.
        /// </summary>
        /// <param name="AppName">Имя каталога приложения</param>
        /// <param name="GameDirs">Возможные каталоги установки</param>
        /// <returns>Возвращает путь к каталогу игры или пустую строку</returns>
        private string GetGameDirectory(string AppName, string SmallAppName, List<String> GameDirs)
        {
            foreach (string Dir in GameDirs)
            {
                string GamePath = Path.Combine(Dir, AppName);
                if (Directory.Exists(Path.Combine(GamePath, SmallAppName)))
                {
                    return GamePath;
                }
            }
            return null;
        }

        /// <summary>
        /// Ищет все доступные конфиги, хранящиеся в Cloud или его локальной копии.
        /// </summary>
        /// <param name="SteamIDs">Steam User IDs</param>
        /// <param name="AppID">ID выбранного приложения</param>
        /// <param name="SteamPath">Каталог установки Steam</param>
        /// <param name="Mask">Маска файлов для поиска</param>
        /// <returns>Возвращает список найденных файлов с графическими настройками</returns>
        private List<String> GetCloudConfigs(List<String> SteamIDs, string AppID, string SteamPath, string Mask = "*.*cfg")
        {
            List<String> Result = new List<String>();
            foreach (string ID in SteamIDs)
            {
                // Сгенерируем путь к конфигам в локальной копии Cloud...
                Result.AddRange(CoreLib.FindFiles(Path.Combine(SteamPath, "userdata", ID, AppID), Mask));
            }
            return Result;
        }

        /// <summary>
        /// Обновляет список файлов с графическими настройками выбранной игры.
        /// </summary>
        public void UpdateVideoFilesList()
        {
            // Ищем файлы с графическими настройками из локального хранилища...
            VideoCfgFiles = GetCloudConfigs(CoreLib.GetUserIDs(SteamPath), GameInternalID, SteamPath, "video.txt");
            
            // Добавляем в базу Legacy конфиг...
            VideoCfgFiles.Add(Path.Combine(GamePath, ConfDir, "cfg", "video.txt"));
        }

        /// <summary>
        /// Возвращает актуальный файл графических настроек игры.
        /// </summary>
        public string GetActualVideoFile()
        {
            return CoreLib.FindNewerestFile(VideoCfgFiles);
        }

        /// <summary>
        /// Конструктор класса. Заполняет информацию о выбранном приложении.
        /// </summary>
        /// <param name="AppName">Название приложения (из БД)</param>
        /// <param name="DirName">Каталог приложения (из БД)</param>
        /// <param name="SmallName">Внутренний каталог приложения (из БД)</param>
        /// <param name="Executable">Имя главного бинарника (из БД)</param>
        /// <param name="SID">Внутренний ID приложения в Steam (из БД)</param>
        /// <param name="VFDir">Каталог хранения графических настроек (из БД)</param>
        /// <param name="HasVF">Задаёт формат приложения: GCF/NCF (из БД)</param>
        /// <param name="UserDir">Указывает использует ли приложение кастомный каталог (из БД)</param>
        /// <param name="AppPath">Путь к каталогу SRC Repair</param>
        /// <param name="UserDir">Путь к пользовательскому каталогу SRC Repair</param>
        /// <param name="SteamDir">Путь к установленному клиенту Steam</param>
        public SourceGame(string AppName, string DirName, string SmallName, string Executable, string SID, string VFDir, bool HasVF, bool UserDir, bool HUDAv, string AppPath, string AUserDir, string SteamDir, List<String> GameDirs)
        {
            // Начинаем определять нужные нам значения переменных...
            FullAppName = AppName;
            SmallAppName = SmallName;
            GameBinaryFile = Executable;
            GameInternalID = SID;
            ConfDir = VFDir;
            IsUsingVideoFile = HasVF;
            IsUsingUserDir = UserDir;
            IsHUDsAvailable = HUDAv;
            SteamPath = SteamDir;

            // Получаем полный путь до каталога управляемого приложения...
            GamePath = GetGameDirectory(DirName, SmallName, GameDirs);
            
            // Проверяем установлено ли приложение...
            IsInstalled = !String.IsNullOrWhiteSpace(GamePath);

            // Заполняем остальные свойства класса если приложение установлено...
            if (IsInstalled)
            {
                FullGamePath = Path.Combine(GamePath, SmallAppName);
                FullCfgPath = Path.Combine(FullGamePath, "cfg");
                FullBackUpDirPath = Path.Combine(AUserDir, "backups", Path.GetFileName(SmallAppName));
                BanlistFileName = Path.Combine(FullGamePath, "voice_ban.dt");
                AppHUDDir = Path.Combine(AUserDir, Properties.Settings.Default.HUDLocalDir, SmallAppName);
                CustomInstallDir = Path.Combine(FullGamePath, IsUsingUserDir ? "custom" : String.Empty);
                AppWorkshopDir = Path.Combine(SteamDir, Properties.Resources.SteamAppsFolderName, Properties.Resources.WorkshopFolderName, "content", GameInternalID);
                CloudConfigs = GetCloudConfigs(CoreLib.GetUserIDs(SteamDir), SID, SteamDir);
            }
        }
    }
}
