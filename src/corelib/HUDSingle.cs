/**
 * SPDX-FileCopyrightText: 2011-2023 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with a single HUD.
    /// </summary>
    public sealed class HUDSingle
    {
        /// <summary>
        /// Gets HUD user-friendly name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets small game name.
        /// </summary>
        public string Game { get; private set; }

        /// <summary>
        /// Gets HUD download URL.
        /// </summary>
        public string URI { get; private set; }

        /// <summary>
        /// Gets HUD alternative download URL.
        /// </summary>
        public string Mirror { get; private set; }

        /// <summary>
        /// Gets HUD upstream download URL.
        /// </summary>
        public string UpURI { get; private set; }

        /// <summary>
        /// Gets information if HUD supported latest version of game.
        /// </summary>
        public bool IsUpdated { get; private set; }

        /// <summary>
        /// Gets HUD screenshot download URL.
        /// </summary>
        public string Preview { get; private set; }

        /// <summary>
        /// Gets HUD last update time in Unixtime format.
        /// </summary>
        public long LastUpdate { get; private set; }

        /// <summary>
        /// Gets link to HUD's official website.
        /// </summary>
        public string Site { get; private set; }

        /// <summary>
        /// Gets archive name inside of HUD's archive.
        /// </summary>
        public string ArchiveDir { get; private set; }

        /// <summary>
        /// Gets HUD installation directory name.
        /// </summary>
        public string InstallDir { get; private set; }

        /// <summary>
        /// Gets HUD download file checksum.
        /// </summary>
        public string FileHash { get; private set; }

        /// <summary>
        /// Gets full path to downloaded HUD archive file on disk.
        /// </summary>
        public string LocalFile { get; private set; }

        /// <summary>
        /// Checks hash sum of the locally downloaded file with the etalon.
        /// </summary>
        public bool CheckHash()
        {
            return FileManager.CalculateFileSHA512(LocalFile) == FileHash;
        }

        /// <summary>
        /// HUDSingle class constructor.
        /// </summary>
        /// <param name="HDName">Value Name from database.</param>
        /// <param name="HDGame">Value Game from database.</param>
        /// <param name="HDURI">Value URI from database.</param>
        /// <param name="HDMirror">Value URI from database.</param>
        /// <param name="HDUpURI">Value UpURI from database.</param>
        /// <param name="HDIsUp">Value IsUpdated from database.</param>
        /// <param name="HDPreview">Value Preview from database.</param>
        /// <param name="HDSite">Value Site from database.</param>
        /// <param name="HDAd">Value ArchiveDir from database.</param>
        /// <param name="HDId">Value InstallDir from database.</param>
        /// <param name="UPDHash">Value Hash from database.</param>
        /// <param name="HDLocal">Full path to HUD archive file on disk.</param>
        public HUDSingle(string HDName, string HDGame, string HDURI, string HDMirror, string HDUpURI, bool HDIsUp, string HDPreview, string HDUpTime, string HDSite, string HDAd, string HDId, string UPDHash, string HDLocal)
        {
            Name = HDName;
            Game = HDGame;
            URI = HDURI;
            Mirror = HDMirror;
            UpURI = HDUpURI;
            IsUpdated = HDIsUp;
            Preview = HDPreview;
            LastUpdate = Convert.ToInt64(HDUpTime);
            Site = HDSite;
            ArchiveDir = HDAd;
            InstallDir = HDId;
            LocalFile = HDLocal;
            FileHash = UPDHash;
        }
    }
}
