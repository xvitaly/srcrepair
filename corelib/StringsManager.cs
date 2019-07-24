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

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with strings, stored in resource
    /// section of this shared library.
    /// </summary>
    public static class StringsManager
    {
        /// <summary>
        /// Gets config database file name.
        /// </summary>
        public static string ConfigDatabaseName => Properties.Resources.CfgDbFile;

        /// <summary>
        /// Gets game database file name.
        /// </summary>
        public static string GameDatabaseName => Properties.Resources.GameListFile;

        /// <summary>
        /// Gets HUD database file name.
        /// </summary>
        public static string HudDatabaseName => Properties.Resources.HUDDbFile;

        /// <summary>
        /// Gets full update checker URL.
        /// </summary>
        public static string UpdateDatabaseUrl => Properties.Resources.UpdateDBURL;

        /// <summary>
        /// Gets HUD local directory name.
        /// </summary>
        public static string HudDirectoryName => Properties.Resources.HUDLocalDir;

        /// <summary>
        /// Gets cleanup targets database file name.
        /// </summary>
        public static string CleanupDatabaseName => Properties.Resources.CleanupDbFile;
    }
}
