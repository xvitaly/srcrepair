/*
 * Класс для определения запущенной платформы и работы с ней.
 * 
 * Copyright 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2017 EasyCoding Team.
 * 
 * Лицензия: GPL v3 (см. файл GPL.txt).
 * Лицензия контента: Creative Commons 3.0 BY.
 * 
 * Запрещается использовать этот файл при использовании любой
 * лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
 * 
 * Официальный блог EasyCoding Team: https://www.easycoding.org/
 * Официальная страница проекта: https://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/
using System;
using System.IO;

namespace srcrepair
{
    /// <summary>
    /// Класс для определения запущенной платформы и работы с ней.
    /// </summary>
    public sealed class CurrentPlatform
    {
        /// <summary>
        /// Содержит коды известных платформ.
        /// </summary>
        public enum OSType
        {
            Windows = 0,
            MacOSX = 1,
            Linux = 2
        }

        /// <summary>
        /// Хранит и возвращает код текущей платформы, на которой запущено приложение.
        /// </summary>
        public OSType OS { get; private set; }

        /// <summary>
        /// Возвращает название текущей платформы, на которой запущено приложение.
        /// </summary>
        public string OSFriendlyName => OS.ToString();

        /// <summary>
        /// Возвращает платформо-зависимое название бинарника Steam.
        /// </summary>
        public string SteamBinaryName
        {
            get
            {
                switch (OS)
                {
                    case OSType.Windows:
                        return Properties.Resources.SteamExecBinWin;
                    case OSType.Linux:
                        return Properties.Resources.SteamExecBinLin;
                    case OSType.MacOSX:
                        return Properties.Resources.SteamExecBinMac;
                    default: return String.Empty;
                }
            }
        }

        /// <summary>
        /// Возвращает платформо-зависимое название каталога SteamApps.
        /// </summary>
        public string SteamAppsFolderName
        {
            get
            {
                switch (OS)
                {
                    case OSType.Windows:
                        return Properties.Resources.SteamAppsFolderNameWin;
                    case OSType.Linux:
                        return Properties.Resources.SteamAppsFolderNameLin;
                    case OSType.MacOSX:
                        return Properties.Resources.SteamAppsFolderNameMac;
                    default: return String.Empty;
                }
            }
        }

        /// <summary>
        /// Возвращает платформо-зависимое имя процесса Steam.
        /// </summary>
        public string SteamProcName
        {
            get
            {
                switch (OS)
                {
                    case OSType.Windows:
                        return Properties.Resources.SteamProcNameWin;
                    case OSType.Linux:
                        return Properties.Resources.SteamProcNameLin;
                    case OSType.MacOSX:
                        return Properties.Resources.SteamProcNameMac;
                    default: return String.Empty;
                }
            }
        }

        /// <summary>
        /// Возвращает код текущей платформы, на которой запущено приложение.
        /// </summary>
        /// <returns>Код текущей платформы.</returns>
        private OSType GetRunningOS()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    return Directory.Exists("/Applications") ? OSType.MacOSX : OSType.Linux;
                case PlatformID.MacOSX:
                    return OSType.MacOSX;
                default: return OSType.Windows;
            }
        }

        /// <summary>
        /// Базовый конструктор класса CurrentPlatform.
        /// </summary>
        public CurrentPlatform()
        {
            // Получаем ID платформы...
            OS = GetRunningOS();
        }
    }
}
