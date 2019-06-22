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
using System.Text.RegularExpressions;

namespace srcrepair.core
{
    /// <summary>
    /// Class with different converters of SteamID formats.
    /// </summary>
    public static class SteamConv
    {
        /// <summary>
        /// Special pre-defined magic constant, using in all conversions.
        /// </summary>
        public const long Multi = 76561197960265728;

        /// <summary>
        /// Gets UserID from SteamID32.
        /// </summary>
        /// <param name="Sid32">SteamID32.</param>
        /// <returns>UserID.</returns>
        public static long GetUserID(string Sid32)
        {
            string[] SidArr = Sid32.Split(':');
            return Convert.ToInt64(SidArr[2]) * 2 + Convert.ToInt64(SidArr[1]);
        }
        
        /// <summary>
        /// Gets SteamIDv3 from SteamID32.
        /// </summary>
        /// <param name="Sid32">SteamID32.</param>
        /// <returns>SteamIDv3.</returns>
        public static string ConvSid32Sidv3(string Sid32)
        {
            return String.Format("[U:1:{0}]", GetUserID(Sid32));
        }

        /// <summary>
        /// Gets SteamID64 from SteamID32.
        /// </summary>
        /// <param name="Sid32">SteamID32.</param>
        /// <returns>SteamID64.</returns>
        public static long ConvSid32Sid64(string Sid32)
        {
            return GetUserID(Sid32) + Multi;
        }

        /// <summary>
        /// Gets SteamID64 from SteamIDv3.
        /// </summary>
        /// <param name="Sidv3">SteamIDv3.</param>
        /// <returns>SteamID64.</returns>
        public static long ConvSidv3Sid64(string Sidv3)
        {
            return Int64.Parse(Regex.Match(Sidv3, @"\d{2,12}").Value) + Multi;
        }
    }
}
