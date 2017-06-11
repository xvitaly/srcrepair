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
    public class CurrentPlatform
    {
        public enum OSType
        {
            Windows = 0,
            MacOSX = 1,
            Linux = 2
        }

        public static OSType GetRunningOS()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    return (Directory.Exists("/Applications") & Directory.Exists("/System") & Directory.Exists("/Users") & Directory.Exists("/Volumes")) ? OSType.MacOSX : OSType.Linux;
                case PlatformID.MacOSX:
                    return OSType.MacOSX;
                default: return OSType.Windows;
            }
        }
    }
}
