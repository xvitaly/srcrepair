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

        public OSType OS { get; private set; }

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
        /// Возвращает название текущей платформы, на которой запущено приложение.
        /// </summary>
        /// <returns>Название текущей платформы.</returns>
        public string GetOSFriendlyName()
        {
            return OS.ToString();
        }

        /// <summary>
        /// Базовый конструктор класса CurrentPlatform.
        /// </summary>
        public CurrentPlatform()
        {
            OS = GetRunningOS();
        }
    }
}
