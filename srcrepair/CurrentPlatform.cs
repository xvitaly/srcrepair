/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2019 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2019 EasyCoding Team.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
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
                    default:
                        throw new PlatformNotSupportedException();
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
                    default:
                        throw new PlatformNotSupportedException();
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
                    default:
                        throw new PlatformNotSupportedException();
                }
            }
        }

        /// <summary>
        /// Возвращает платформо-зависимый суффикс для заголовка HTTP_USER_AGENT.
        /// </summary>
        public string UASuffix
        {
            get
            {
                switch (OS)
                {
                    case OSType.Windows:
                        return Properties.Resources.AppUASuffixWin;
                    case OSType.Linux:
                        return Properties.Resources.AppUASuffixOther;
                    case OSType.MacOSX:
                        return Properties.Resources.AppUASuffixOther;
                    default:
                        throw new PlatformNotSupportedException();
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
