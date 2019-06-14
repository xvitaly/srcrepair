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
    /// Class with different helper methods.
    /// </summary>
    public static class CoreLib
    {
        /// <summary>
        /// Remove different special characters from specified string.
        /// </summary>
        /// <param name="RecvStr">Source string for cleanup.</param>
        /// <param name="CleanQuotes">Enable removal of quotes.</param>
        /// <param name="CleanSlashes">Enable removal of double slashes.</param>
        /// <returns>Clean string with removed special characters.</returns>
        public static string CleanStrWx(string RecvStr, bool CleanQuotes = false, bool CleanSlashes = false)
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
        /// Return contents of text file from internal resource section of calling assembly.
        /// </summary>
        /// <param name="FileName">Internal resource file name.</param>
        /// <returns>Contents of bundled in resource text file.</returns>
        public static string GetTemplateFromResource(string FileName)
        {
            string Result = String.Empty;
            using (StreamReader Reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(FileName)))
            {
                Result = Reader.ReadToEnd();
            }
            return Result;
        }
    }
}
