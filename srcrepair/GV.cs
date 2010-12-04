using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace srcrepair
{
    class GV
    {
        /*
         * Эта константа хранит имя приложения для служебных целей
         */
        public const string AppName = "Source Repair";
        
        /*
         * В этой переменной мы будем хранить полный путь к каталогу установленного
         * клиента Steam.
         */
        public static string FullSteamPath;

        /*
         * В этой переменной будем хранить полный путь к каталогу с утилитой
         * SRCRepair для служебных целей.
         */
        public static string FullAppPath;

        /*
         * В этой переменной будем хранить полный путь к каталогу игры, которой
         * мы будем управлять данной утилитой.
         */
        public static string FullGamePath;
        
        /*
         * В этой переменной будем хранить полный путь к каталогу игры без
         * включения в путь GV.SmallAppName для служебных целей.
         */
        public static string GamePath;

        /*
         * В этой переменной будем хранить полное имя управляемого приложения
         * для служебных целей.
         */
        public static string FullAppName;

        /*
         * В этой переменной мы будем хранить краткое имя управляемого приложения
         * для служебных целей (SteamAlias).
         */
        public static string SmallAppName;

        /*
         * В этой переменной мы будем хранить полную информацию о версии
         * приложения для служебных целей.
         */
        public static string AppVersionInfo;

        /*
         * В этой переменной мы будем хранить полный путь до каталога с
         * файлами конфигурации управляемого приложения.
         */
        public static string FullCfgPath;

        /*
         * В этой переменной мы будем хранить полный путь до каталога с
         * резервными копиями управляемого приложения.
         */
        public static string FullBackUpDirPath;

        /*
         * Эта переменная будет указывать на тип управляемого приложения:
         * стандартное GCF, либо нестандартное NCF, с которым много проблем.
         */
        public static bool IsGCFApp;
    }
}
