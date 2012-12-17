using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace srcrepair
{
    /// <summary>
    /// Специальный класс для хранения особых переменных и констант.
    /// </summary>
    public sealed class GV
    {
        /// <summary>
        /// Эта константа хранит имя приложения для служебных целей.
        /// </summary>
        public const string AppName = "SRC Repair";

        /// <summary>
        /// Этот массив содержит имена службных каталогов Steam, которые не будет
        /// отображаться в списке доступных для выбора логинов.
        /// </summary>
        public static string[] InternalDirs = { "common", "sourcemods", "media", "staging", "temp", "build", "downloading" };

        /// <summary>
        /// Хранит код платформы запуска приложения...
        /// </summary>
        public static int RunningPlatform;

        /// <summary>
        /// Хранит имя платформы запуска приложения...
        /// </summary>
        public static string PlatformFriendlyName;

        /// <summary>
        /// Хранит имя бинарного файла клиента Steam в зависимости от платформы...
        /// </summary>
        public static string SteamExecuttable;

        /// <summary>
        /// Хранит User-Agent, которым представляется удалённым службам...
        /// </summary>
        public static string UserAgent;

        /// <summary>
        /// Хранит имя каталога SteamApps в зависимости от платформы...
        /// </summary>
        public static string SteamAppsFolderName;

        /// <summary>
        /// В этой переменной мы будем хранить полный путь к каталогу установленного
        /// клиента Steam.
        /// </summary>
        public static string FullSteamPath;

        /// <summary>
        /// В этой переменной будем хранить полный путь к каталогу с утилитой
        /// SRCRepair для служебных целей.
        /// </summary>
        public static string FullAppPath;

        /// <summary>
        /// В этой переменной будем хранить путь до каталога пользователя
        /// программы. Используется для служебных целей.
        /// </summary>
        public static string AppUserDir;

        /// <summary>
        /// В этой переменной будем хранить полный путь к каталогу игры, которой
        /// мы будем управлять данной утилитой.
        /// </summary>
        public static string FullGamePath;
        
        /// <summary>
        /// В этой переменной будем хранить полный путь к каталогу игры без
        /// включения в путь GV.SmallAppName для служебных целей.
        /// </summary>
        public static string GamePath;

        /// <summary>
        /// В этой переменной будем хранить полное имя управляемого приложения
        /// для служебных целей.
        /// </summary>
        public static string FullAppName;

        /// <summary>
        /// В этой переменной мы будем хранить краткое имя управляемого приложения
        /// для служебных целей (SteamAlias).
        /// </summary>
        public static string SmallAppName;

        /// <summary>
        /// В этой переменной мы будем хранить полную информацию о версии
        /// приложения для служебных целей.
        /// </summary>
        public static string AppVersionInfo;

        /// <summary>
        /// В этой переменной мы будем хранить полный путь до каталога с
        /// файлами конфигурации управляемого приложения.
        /// </summary>
        public static string FullCfgPath;

        /// <summary>
        /// В этой переменной мы будем хранить полный путь до каталога с
        /// резервными копиями управляемого приложения.
        /// </summary>
        public static string FullBackUpDirPath;

        /// <summary>
        /// Эта переменная будет указывать на тип управляемого приложения:
        /// стандартное GCF, либо нестандартное NCF, с которым много проблем.
        /// </summary>
        public static bool IsGCFApp;

        /// <summary>
        /// Эта переменная хранит ID игры по базе данных Steam. Используется
        /// для служебных целей.
        /// </summary>
        public static string GameInternalID;

        /// <summary>
        /// Служебная переменная. Используется для указания доступности
        /// редактора чёрного списка серверов.
        /// </summary>
        public static string GameBLEnabled;

        /// <summary>
        /// В этой переменной хранится путь к файлу с настройками видео,
        /// используется в NCF-играх.
        /// </summary>
        public static string VideoCfgFile;

        /// <summary>
        /// Содержит имя каталога с конфигами. Используется в последних
        /// играх.
        /// </summary>
        public static string ConfDir;
    }
}
