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
        /// HUDSingle class constructor.
        /// </summary>
        /// <param name="HDName">Value Name from database.</param>
        /// <param name="HDGame">Value Game from database.</param>
        /// <param name="HDURI">Value URI from database.</param>
        /// <param name="HDUpURI">Value UpURI from database.</param>
        /// <param name="HDIsUp">Value IsUpdated from database.</param>
        /// <param name="HDPreview">Value Preview from database.</param>
        /// <param name="HDSite">Value Site from database.</param>
        /// <param name="HDAd">Value ArchiveDir from database.</param>
        /// <param name="HDId">Value InstallDir from database.</param>
        /// <param name="UPDHash">Value Hash from database.</param>
        /// <param name="HDLocal">Full path to HUD archive file on disk.</param>
        public HUDSingle(string HDName, string HDGame, string HDURI, string HDUpURI, bool HDIsUp, string HDPreview, string HDUpTime, string HDSite, string HDAd, string HDId, string UPDHash, string HDLocal)
        {
            Name = HDName;
            Game = HDGame;
            URI = HDURI;
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
