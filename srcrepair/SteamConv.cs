/*
 * Класс со статическими методами для преобразования форматов SteamID.
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
using System.Text.RegularExpressions;

namespace srcrepair
{
    public sealed class SteamConv
    {
        /// <summary>
        /// Магическая константа, используемая для преобразований форматов.
        /// </summary>
        public const long Multi = 76561197960265728;

        /// <summary>
        /// Получает значение UserID из старого формата SteamID32.
        /// </summary>
        /// <param name="Sid32">SteamID32</param>
        /// <returns>UserID</returns>
        public static long GetUserID(string Sid32)
        {
            string[] SidArr = Sid32.Split(':');
            return Convert.ToInt64(SidArr[2]) * 2 + Convert.ToInt64(SidArr[1]);
        }
        
        /// <summary>
        /// Преобразовывает старый формат SteamID32 в новый SteamIDv3.
        /// </summary>
        /// <param name="Sid32">SteamID32</param>
        /// <returns>SteamIDv3</returns>
        public static string ConvSid32Sidv3(string Sid32)
        {
            return String.Format("[U:1:{0}]", GetUserID(Sid32));
        }

        /// <summary>
        /// Преобразовывает старый формат SteamID32 в универсальный SteamID64.
        /// </summary>
        /// <param name="Sid32">SteamID32</param>
        /// <returns>SteamID64</returns>
        public static long ConvSid32Sid64(string Sid32)
        {
            return GetUserID(Sid32) + Multi;
        }

        /// <summary>
        /// Преобразовывает новый формат SteamIDv3 в универсальный SteamID64.
        /// </summary>
        /// <param name="Sidv3">SteamIDv3</param>
        /// <returns>SteamID64</returns>
        public static long ConvSidv3Sid64(string Sidv3)
        {
            return Int64.Parse(Regex.Match(Sidv3, @"\d{2,12}").Value) + Multi;
        }
    }
}
