/**
 * SPDX-FileCopyrightText: 2011-2021 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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

        /// <summary>
        /// Hosts file does not exists.
        /// </summary>
        public const int HostsFileDoesNotExists = 9;

        /// <summary>
        /// Application update pending.
        /// </summary>
        public const int AppUpdatePending = 10;
    }
}
