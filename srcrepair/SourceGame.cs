﻿/*
 * Класс SourceEngine SRC Repair.
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
using System.Collections.Generic;
using System.IO;

namespace srcrepair
{
    public sealed class SourceGame
    {
        /// <summary>
        /// В этой переменной будем хранить путь установки кастомных файлов.
        /// </summary>
        public string CustomInstallDir;

        /// <summary>
        /// В этой переменной будем хранить полный путь к каталогу игры, которой
        /// мы будем управлять данной утилитой.
        /// </summary>
        public string FullGamePath;

        /// <summary>
        /// В этой переменной будем хранить полный путь к каталогу игры без
        /// включения в путь GV.SmallAppName для служебных целей.
        /// </summary>
        public string GamePath;

        /// <summary>
        /// В этой переменной будем хранить полное имя управляемого приложения
        /// для служебных целей.
        /// </summary>
        public string FullAppName;

        /// <summary>
        /// В этой переменной мы будем хранить краткое имя управляемого приложения
        /// для служебных целей (SteamAlias).
        /// </summary>
        public string SmallAppName;

        /// <summary>
        /// В этой переменной мы будем хранить имя главного процесса игры.
        /// </summary>
        public string GameBinaryFile;

        /// <summary>
        /// В этой переменной мы будем хранить полный путь до каталога с
        /// файлами конфигурации управляемого приложения.
        /// </summary>
        public string FullCfgPath;

        /// <summary>
        /// В этой переменной мы будем хранить полный путь до каталога с
        /// резервными копиями управляемого приложения.
        /// </summary>
        public string FullBackUpDirPath;

        /// <summary>
        /// Указывает использует ли игра файл video.txt для хранения
        /// своих настроек.
        /// </summary>
        public bool IsUsingVideoFile;

        /// <summary>
        /// Определяет использует ли игра специальный каталог для хранения
        /// пользовательских настроек и скриптов.
        /// </summary>
        public bool IsUsingUserDir;

        /// <summary>
        /// Эта переменная хранит ID игры по базе данных Steam. Используется
        /// для служебных целей.
        /// </summary>
        public string GameInternalID;

        /// <summary>
        /// В этой переменной хранится путь к файлу с настройками видео,
        /// используется в NCF-играх.
        /// </summary>
        public string VideoCfgFile;

        /// <summary>
        /// В этом списке хранятся пути ко всем найденным файлами
        /// с графическими настройками.
        /// </summary>
        public List<String> VideoCfgFiles;

        /// <summary>
        /// Содержит имя каталога с конфигами. Используется в последних
        /// играх.
        /// </summary>
        public string ConfDir;

        /// <summary>
        /// В этой переменной будем хранить путь до каталога локального хранения
        /// загруженных файлов HUD и их мета-информации.
        /// </summary>
        public string AppHUDDir;

        /// <summary>
        /// Содержит пути к установленным FPS-конфигам управляемой игры.
        /// </summary>
        public List<String> FPSConfigs;

        /// <summary>
        /// Содержит путь к каталогу с загруженными данными из Steam Workshop.
        /// </summary>
        public string AppWorkshopDir;

        /// <summary>
        /// Содержит путь к файлу со списком заглушенных пользователей.
        /// </summary>
        public string BanlistFileName;

        /// <summary>
        /// Указывает установлено ли данное приложение.
        /// </summary>
        public bool IsInstalled;

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
        /// Ищет все доступные файлы с графическими настройками в локальном хранилище.
        /// </summary>
        /// <param name="SteamIDs">Steam User IDs</param>
        /// <param name="AppID">ID выбранного приложения</param>
        /// <param name="SteamPath">Каталог установки Steam</param>
        /// <returns>Возвращает список найденных файлов с графическими настройками</returns>
        private List<String> GetVideoConfigs(List<String> SteamIDs, string AppID, string SteamPath)
        {
            List<String> Result = new List<String>();
            foreach (string ID in SteamIDs)
            {
                string FullDir = Path.Combine(SteamPath, "userdata", ID, AppID, "local", "cfg", "video.txt");
                if (File.Exists(FullDir)) { Result.Add(FullDir); }
            }
            return Result;
        }

        /// <summary>
        /// Конструктор класса. Заполняет информацию о выбранном приложении.
        /// </summary>
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
        public SourceGame(string DirName, string SmallName, string Executable, string SID, string VFDir, bool HasVF, bool UserDir, string AppPath, string AUserDir, string SteamDir, List<String> GameDirs)
        {
            // Начинаем определять нужные нам значения переменных...
            FullAppName = DirName;
            SmallAppName = SmallName;
            GameBinaryFile = Executable;
            GameInternalID = SID;
            ConfDir = VFDir;
            IsUsingVideoFile = HasVF;
            IsUsingUserDir = UserDir;

            // Получаем полный путь до каталога управляемого приложения...
            GamePath = GetGameDirectory(DirName, SmallName, GameDirs);
            
            // Проверяем установлено ли приложение...
            IsInstalled = !String.IsNullOrWhiteSpace(GamePath);

            // Заполняем остальные свойства класса если приложение установлено...
            if (IsInstalled)
            {
                FullGamePath = Path.Combine(GamePath, SmallAppName);
                FullCfgPath = Path.Combine(FullGamePath, "cfg");
                FullBackUpDirPath = Path.Combine(AUserDir, "backups", SmallAppName);
                BanlistFileName = Path.Combine(FullGamePath, "voice_ban.dt");
                VideoCfgFiles = GetVideoConfigs(CoreLib.GetUserIDs(SteamDir), SID, SteamDir);
                VideoCfgFile = VideoCfgFiles.Count >= 1 ? CoreLib.FindNewerestFile(VideoCfgFiles) : Path.Combine(GamePath, ConfDir, "cfg", "video.txt");
                AppHUDDir = Path.Combine(AUserDir, Properties.Settings.Default.HUDLocalDir, SmallAppName);
                CustomInstallDir = Path.Combine(FullGamePath, IsUsingUserDir ? "custom" : "");
                AppWorkshopDir = Path.Combine(SteamDir, Properties.Resources.SteamAppsFolderName, Properties.Resources.WorkshopFolderName, "content", GameInternalID);
            }
        }
    }
}
