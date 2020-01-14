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
        public List<String> SupportedGames { get; private set; }

        /// <summary>
        /// Gets or sets archive name inside of fps-config's archive.
        /// </summary>
        public string ArchiveDir { get; private set; }

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
        /// FPSConfig class constructor.
        /// </summary>
        /// <param name="CfName">FPS-config friendly name.</param>
        /// <param name="CfURI">FPS-config download URI.</param>
        /// <param name="CfURI">FPS-config download mirror URL.</param>
        /// <param name="CfDescription">FPS-config description.</param>
        /// <param name="CfGames">Array of supported by FPS-configs game IDs.</param>
        /// <param name="CfArchiveDIr">Working directory in download archive.</param>
        /// <param name="CfInstallDir">FPS-config installation directory.</param>
        /// <param name="CfFileHash">FPS-config download file checksum.</param>
        /// <param name="CfLocalFile">Path to downloaded FPS-config archive file on disk.</param>
        public FPSConfig(string CfName, string CfURI, string CfMirror, string CfDescription, string[] CfGames, string CfArchiveDIr, string CfInstallDir, string CfFileHash, string CfLocalFile)
        {
            // Setting class properties...
            Name = CfName;
            URI = CfURI;
            Mirror = CfMirror;
            Description = CfDescription;
            SupportedGames = new List<String>(CfGames);
            ArchiveDir = CfArchiveDIr;
            InstallDir = CfInstallDir;
            FileHash = CfFileHash;
            LocalFile = CfLocalFile;
        }
    }
}
