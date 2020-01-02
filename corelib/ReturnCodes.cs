/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2020 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2020 EasyCoding Team.
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

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with application return codes.
    /// </summary>
    public static class ReturnCodes
    {
        /// <summary>
        /// Successful exit without any errors.
        /// </summary>
        public const int Success = 0;

        /// <summary>
        /// Wrong path to Steam client installation directory entered.
        /// </summary>
        public const int StmWrongPath = 1;

        /// <summary>
        /// User refused to find Steam client installation directory.
        /// </summary>
        public const int StmPathCancel = 2;

        /// <summary>
        /// Exception occured while trying to detect Steam client
        /// installation directory.
        /// </summary>
        public const int StmPathException = 3;

        /// <summary>
        /// No supported installed games detected.
        /// </summary>
        public const int NoGamesDetected = 4;

        /// <summary>
        /// Exception during parsing XML game database and trying
        /// to find installed supported games.
        /// </summary>
        public const int GameDbParseError = 5;

        /// <summary>
        /// No Steam UserIDs detected.
        /// </summary>
        public const int NoUserIdsDetected = 6;

        /// <summary>
        /// Application is already running.
        /// </summary>
        public const int AppAlreadyRunning = 7;

        /// <summary>
        /// Core library version missmatch.
        /// </summary>
        public const int CoreLibVersionMissmatch = 8;
    }
}
