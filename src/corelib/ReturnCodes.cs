/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
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
        public static int Success => 0;

        /// <summary>
        /// Wrong path to Steam client installation directory entered.
        /// </summary>
        public static int StmWrongPath => 1;

        /// <summary>
        /// User refused to find Steam client installation directory.
        /// </summary>
        public static int StmPathCancel => 2;

        /// <summary>
        /// Exception occured while trying to detect Steam client
        /// installation directory.
        /// </summary>
        public static int StmPathException => 3;

        /// <summary>
        /// No supported installed games detected.
        /// </summary>
        public static int NoGamesDetected => 4;

        /// <summary>
        /// Exception during parsing XML game database and trying
        /// to find installed supported games.
        /// </summary>
        public static int GameDbParseError => 5;

        /// <summary>
        /// No Steam UserIDs detected.
        /// </summary>
        public static int NoUserIdsDetected => 6;

        /// <summary>
        /// Application is already running.
        /// </summary>
        public static int AppAlreadyRunning => 7;

        /// <summary>
        /// Core library version missmatch.
        /// </summary>
        public static int CoreLibVersionMissmatch => 8;

        /// <summary>
        /// Hosts file does not exists.
        /// </summary>
        public static int HostsFileDoesNotExists => 9;

        /// <summary>
        /// Application update pending.
        /// </summary>
        public static int AppUpdatePending => 10;

        /// <summary>
        /// Current platform is not supported.
        /// </summary>
        public static int PlatformNotSupported => 11;

        /// <summary>
        /// The requested action requires administrator rights, but the
        /// program is running as a regular user.
        /// </summary>
        public static int AppNoAdminRights => 12;
    }
}
