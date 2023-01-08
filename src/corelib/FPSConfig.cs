/**
 * SPDX-FileCopyrightText: 2011-2023 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with a single FPS-config.
    /// </summary>
    public sealed class FPSConfig
    {
        /// <summary>
        /// Get or sets FPS-config friendly name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets FPS-config download URL.
        /// </summary>
        public string URI { get; private set; }

        /// <summary>
        /// Gets or sets FPS-config download mirror URL.
        /// </summary>
        public string Mirror { get; private set; }

        /// <summary>
        /// Gets or sets FPS-config user-friendly description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets or sets list of supported by FPS-config games.
        /// </summary>
        public List<string> SupportedGames { get; private set; }

        /// <summary>
        /// Gets or sets archive name inside of fps-config's archive.
        /// </summary>
        public string ArchiveDir { get; private set; }

        /// <summary>
        /// Gets or sets the list of fps-config's files.
        /// </summary>
        public List<string> Files { get; private set; }

        /// <summary>
        /// Gets or sets FPS-config installation directory name if
        /// the game support custom user directories.
        /// </summary>
        public string InstallDir { get; private set; }

        /// <summary>
        /// Gets FPS-config download file checksum.
        /// </summary>
        public string FileHash { get; private set; }

        /// <summary>
        /// Gets full path to downloaded FPS-config archive file on disk.
        /// </summary>
        public string LocalFile { get; private set; }

        /// <summary>
        /// Checks compatibility with specified game ID.
        /// </summary>
        /// <param name="GameID">Game ID.</param>
        public bool CheckCompatibility(string GameID)
        {
            // Check if GameID exists in list...
            return SupportedGames.Contains(GameID);
        }

        /// <summary>
        /// Checks hash sum of the locally downloaded file with the etalon.
        /// </summary>
        public bool CheckHash()
        {
            return FileManager.CalculateFileSHA512(LocalFile) == FileHash;
        }

        /// <summary>
        /// FPSConfig class constructor.
        /// </summary>
        /// <param name="CfName">FPS-config friendly name.</param>
        /// <param name="CfURI">FPS-config download URI.</param>
        /// <param name="CfURI">FPS-config download mirror URL.</param>
        /// <param name="CfDescription">FPS-config description.</param>
        /// <param name="CfGames">Array of supported by FPS-configs game IDs.</param>
        /// <param name="CfArchiveDIr">Working directory in download archive.</param>
        /// <param name="CfFiles">List of FPS-config's files.</param>
        /// <param name="CfInstallDir">FPS-config installation directory.</param>
        /// <param name="CfFileHash">FPS-config download file checksum.</param>
        /// <param name="CfLocalFile">Path to downloaded FPS-config archive file on disk.</param>
        public FPSConfig(string CfName, string CfURI, string CfMirror, string CfDescription, string[] CfGames, string CfArchiveDIr, List<string> CfFiles, string CfInstallDir, string CfFileHash, string CfLocalFile)
        {
            // Setting class properties...
            Name = CfName;
            URI = CfURI;
            Mirror = CfMirror;
            Description = CfDescription;
            SupportedGames = new List<string>(CfGames);
            ArchiveDir = CfArchiveDIr;
            Files = CfFiles;
            InstallDir = CfInstallDir;
            FileHash = CfFileHash;
            LocalFile = CfLocalFile;
        }
    }
}
