/*
 * Класс для взаимодействия с клиентом Steam.
 * 
 * Copyright 2011 - 2016 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2016 EasyCoding Team.
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
    public static class SteamManager
    {
        /// <summary>
        /// Возвращает путь к главному VDF конфигу Steam.
        /// </summary>
        /// <param name="SteamPath">Каталог установки Steam</param>
        /// <returns>Путь к VDF конфигу</returns>
        public static string GetSteamConfig(string SteamPath)
        {
            return Path.Combine(SteamPath, "config", "config.vdf");
        }

        /// <summary>
        /// Возвращает путь к локально хранящемуся VDF конфигу Steam.
        /// </summary>
        /// <param name="SteamPath">Каталог установки Steam</param>
        /// <returns>Путь к локальному VDF конфигу</returns>
        public static List<String> GetSteamLocalConfig(string SteamPath)
        {
            List<String> Result = new List<String>();
            foreach (string ID in GetUserIDs(SteamPath))
            {
                Result.AddRange(CoreLib.FindFiles(Path.Combine(SteamPath, "userdata", ID, "config"), "localconfig.vdf"));
            }
            return Result;
        }

        /// <summary>
        /// Возвращает список используемых на данном компьютере SteamID.
        /// </summary>
        /// <param name="SteamPath">Каталог установки Steam</param>
        /// <returns>Список Steam user ID</returns>
        public static List<String> GetUserIDs(string SteamPath)
        {
            // Создаём новый список...
            List<String> Result = new List<String>();

            // Получаем список каталогов...
            string DDir = Path.Combine(SteamPath, "userdata");
            if (Directory.Exists(DDir))
            {
                DirectoryInfo DInfo = new DirectoryInfo(DDir);
                foreach (DirectoryInfo SubDir in DInfo.GetDirectories())
                {
                    Result.Add(SubDir.Name);
                }
            }

            // Возвращаем результат...
            return Result;
        }

        /// <summary>
        /// Получает и возвращает параметры запуска указанного приложения.
        /// </summary>
        /// <param name="SteamPath">Каталог установки Steam</param>
        /// <param name="GameID">ID приложения, параметры запуска которого нужно определить</param>
        /// <returns>Параметры запуска приложения</returns>
        public static string GetLaunchOptions(string SteamPath, string GameID)
        {
            // Возвращаем результат...
            return String.Empty;
        }
    }
}
