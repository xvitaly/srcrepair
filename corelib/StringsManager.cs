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
using System.IO;
using System.Reflection;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with strings and strings, stored in
    /// resource section of this shared library.
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
        /// Gets HUD local directory name.
        /// </summary>
        public static string ConfigDirectoryName => Properties.Resources.CfgLocalDir;

        /// <summary>
        /// Gets cleanup targets database file name.
        /// </summary>
        public static string CleanupDatabaseName => Properties.Resources.CleanupDbFile;

        /// <summary>
        /// Remove different special characters from specified string.
        /// </summary>
        /// <param name="RecvStr">Source string for cleanup.</param>
        /// <param name="CleanQuotes">Enable removal of quotes.</param>
        /// <param name="CleanSlashes">Enable removal of double slashes.</param>
        /// <returns>Clean string with removed special characters.</returns>
        public static string CleanString(string RecvStr, bool CleanQuotes = false, bool CleanSlashes = false)
        {
            // Removing tabulations...
            while (RecvStr.IndexOf("\t") != -1)
            {
                RecvStr = RecvStr.Replace("\t", " ");
            }

            // Replacing all NUL symbols with spaces...
            while (RecvStr.IndexOf("\0") != -1)
            {
                RecvStr = RecvStr.Replace("\0", " ");
            }

            // Removing multiple spaces...
            while (RecvStr.IndexOf("  ") != -1)
            {
                RecvStr = RecvStr.Replace("  ", " ");
            }

            // Removing quotes if enabled...
            if (CleanQuotes)
            {
                while (RecvStr.IndexOf('"') != -1)
                {
                    RecvStr = RecvStr.Replace(@"""", String.Empty);
                }
            }

            // Removing double slashes if enabled...
            if (CleanSlashes)
            {
                while (RecvStr.IndexOf(@"\\") != -1)
                {
                    RecvStr = RecvStr.Replace(@"\\", @"\");
                }
            }

            // Return result with removal of leading and trailing white-spaces...
            return RecvStr.Trim();
        }

        /// <summary>
        /// Return contents of text file from internal resource section
        /// of the calling assembly.
        /// </summary>
        /// <param name="FileName">Internal resource file name.</param>
        /// <returns>Contents of bundled in resource text file.</returns>
        public static string GetTemplateFromResource(string FileName)
        {
            string Result = String.Empty;
            using (StreamReader Reader = new StreamReader(Assembly.GetCallingAssembly().GetManifestResourceStream(FileName)))
            {
                Result = Reader.ReadToEnd();
            }
            return Result;
        }
    }
}
