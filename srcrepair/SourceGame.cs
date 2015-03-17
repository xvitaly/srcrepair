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
using System.Linq;
using System.Text;

namespace srcrepair
{
    class SourceGame
    {
        /// <summary>
        /// Хранит User-Agent, которым представляется удалённым службам...
        /// </summary>
        public string UserAgent;

        /// <summary>
        /// В этой переменной мы будем хранить полный путь к каталогу установленного
        /// клиента Steam.
        /// </summary>
        public string FullSteamPath;

        /// <summary>
        /// В этой переменной будем хранить полный путь к каталогу с утилитой
        /// SRCRepair для служебных целей.
        /// </summary>
        public string FullAppPath;

        /// <summary>
        /// В этой переменной будем хранить путь до каталога пользователя
        /// программы. Используется для служебных целей.
        /// </summary>
        public string AppUserDir;

        /// <summary>
        /// В этой переменной будем хранить путь до каталога локального хранения
        /// загруженных файлов HUD и их мета-информации.
        /// </summary>
        public string AppHUDDir;

        /// <summary>
        /// В этой переменной будем хранить путь установки кастомных файлов.
        /// </summary>
        public string CustomInstallDir;

        /// <summary>
        /// В этой переменной будем хранить полный путь к каталогу игры, которой
        /// мы будем управлять данной утилитой.
        /// </summary>
        public static string FullGamePath;

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
        /// В этой переменной мы будем хранить полную информацию о версии
        /// приложения для служебных целей.
        /// </summary>
        public string AppVersionInfo;

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
        /// Содержит пути к установленным FPS-конфигам управляемой игры.
        /// </summary>
        public List<String> FPSConfigs;
    }
}
