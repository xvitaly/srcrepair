/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
        private static readonly long Multi = 76561197960265728;

        /// <summary>
        /// Gets UserID from SteamID32.
        /// </summary>
        /// <param name="Sid32">SteamID32.</param>
        /// <returns>UserID.</returns>
        public static long GetUserID(string Sid32)
        {
            string[] SidArr = Sid32.Split(':');
            return (Convert.ToInt64(SidArr[2]) * 2) + Convert.ToInt64(SidArr[1]);
        }

        /// <summary>
        /// Validates UserID.
        /// </summary>
        /// <param name="Sidv3">UserID.</param>
        /// <returns>Returns True if UserID has correct format.</returns>
        public static bool ValidateUserID(string UserID)
        {
            return Regex.IsMatch(UserID, Properties.Resources.UserIDValidateRegex);
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
            return Int64.Parse(Regex.Match(Sidv3, Properties.Resources.UserIDParseRegex).Value) + Multi;
        }
    }
}
