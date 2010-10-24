using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace srcrepair
{
    class GV
    {
        /* В этой переменной мы будем хранить полный путь к каталогу установленного
         * клиента Steam. */
        public static string FullSteamPath;

        /* В этой переменной будем хранить полный путь к каталогу с утилитой
         * SRCRepair для служебных целей. */
        public static string FullAppPath;

        /* В этой переменной будем хранить полный путь к каталогу игры, которой
         * мы будем управлять данной утилитой. */
        public static string FullGamePath;

        /* В этой переменной будем хранить полное имя управляемого приложения
         * для служебных целей. */
        public static string FullAppName;

        /* В этой переменной мы будем хранить краткое имя управляемого приложения
         * для служебных целей (SteamAlias). */
        public static string SmallAppName;
    }
}
