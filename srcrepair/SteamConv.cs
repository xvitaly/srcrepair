using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
