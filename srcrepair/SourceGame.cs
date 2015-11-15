/*
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
using System.Xml;
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
        /// Конструктор класса. Получает информацию о выбранном приложении.
        /// </summary>
        /// <param name="GameName">Название выбранного приложения</param>
        /// <param name="AppPath">Путь к каталогу SRC Repair</param>
        /// <param name="UserDir">Путь к пользовательскому каталогу SRC Repair</param>
        /// <param name="SteamDir">Путь к установленному клиенту Steam</param>
        public SourceGame(string AppName, string AppPath, string UserDir, string SteamDir)
        {
            // Начинаем определять нужные нам значения переменных...
            using (FileStream XMLFS = new FileStream(Path.Combine(AppPath, Properties.Settings.Default.GameListFile), FileMode.Open, FileAccess.Read))
            {
                XmlDocument XMLD = new XmlDocument();
                XMLD.Load(XMLFS);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("Game");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    if (String.Compare(XMLD.GetElementsByTagName("DirName")[i].InnerText, Path.GetFileName(AppName), true) == 0)
                    {
                        FullAppName = XMLD.GetElementsByTagName("DirName")[i].InnerText;
                        SmallAppName = XMLD.GetElementsByTagName("SmallName")[i].InnerText;
                        GameBinaryFile = XMLD.GetElementsByTagName("Executable")[i].InnerText;
                        GameInternalID = XMLD.GetElementsByTagName("SID")[i].InnerText;
                        ConfDir = XMLD.GetElementsByTagName("VFDir")[i].InnerText;
                        IsUsingVideoFile = XMLD.GetElementsByTagName("HasVF")[i].InnerText == "1";
                        IsUsingUserDir = XMLD.GetElementsByTagName("UserDir")[i].InnerText == "1";
                        break;
                    }
                }
            }

            // Генерируем полный путь до каталога управляемого приложения...
            GamePath = AppName;
            FullGamePath = Path.Combine(GamePath, SmallAppName);

            // Заполняем другие служебные переменные...
            FullCfgPath = Path.Combine(FullGamePath, "cfg");
            FullBackUpDirPath = Path.Combine(UserDir, "backups", SmallAppName);
            VideoCfgFile = Path.Combine(GamePath, ConfDir, "cfg", "video.txt");
            AppHUDDir = Path.Combine(UserDir, Properties.Settings.Default.HUDLocalDir, SmallAppName);
            CustomInstallDir = Path.Combine(FullGamePath, IsUsingUserDir ? "custom" : "");
            AppWorkshopDir = Path.Combine(SteamDir, Properties.Resources.SteamAppsFolderName, Properties.Resources.WorkshopFolderName, "content", GameInternalID);
        }
    }
}
