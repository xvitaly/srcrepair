/**
 * SPDX-FileCopyrightText: 2011-2023 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
        /// Gets HTTP_USER_AGENT header template.
        /// </summary>
        public static string HTTPUserAgentTemplate => Properties.Resources.AppDefUA;

        /// <summary>
        /// Gets local updates directory name.
        /// </summary>
        public static string UpdateLocalDirectoryName => Properties.Resources.UpdateLocalDir;

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
        /// Gets plugins database file name.
        /// </summary>
        public static string PluginsDatabaseName => Properties.Resources.PluginsDbFile;

        /// <summary>
        /// Remove tabulations from the source string.
        /// </summary>
        /// <param name="SrcStr">Source string for cleanup.</param>
        /// <returns>String with tabulations replaced with spaces.</returns>
        private static string RemoveTabs(string SrcStr)
        {
            while (SrcStr.IndexOf("\t", StringComparison.InvariantCulture) != -1)
            {
                SrcStr = SrcStr.Replace("\t", " ");
            }
            return SrcStr;
        }

        /// <summary>
        /// Remove NUL-bytes from the source string.
        /// </summary>
        /// <param name="SrcStr">Source string for cleanup.</param>
        /// <returns>String with NUL-bytes removed.</returns>
        private static string RemoveNullBytes(string SrcStr)
        {
            while (SrcStr.IndexOf("\0", StringComparison.InvariantCulture) != -1)
            {
                SrcStr = SrcStr.Replace("\0", " ");
            }
            return SrcStr;
        }

        /// <summary>
        /// Remove multiple spaces from the source string.
        /// </summary>
        /// <param name="SrcStr">Source string for cleanup.</param>
        /// <returns>String with multiple spaces removed.</returns>
        private static string RemoveMultipleSpaces(string SrcStr)
        {
            while (SrcStr.IndexOf("  ", StringComparison.InvariantCulture) != -1)
            {
                SrcStr = SrcStr.Replace("  ", " ");
            }
            return SrcStr;
        }

        /// <summary>
        /// Remove different special characters from specified string.
        /// </summary>
        /// <param name="RecvStr">Source string for cleanup.</param>
        /// <param name="CleanQuotes">Enable removal of quotes.</param>
        /// <param name="CleanSlashes">Enable removal of double slashes.</param>
        /// <returns>Clean string with removed special characters.</returns>
        public static string CleanString(string RecvStr, bool CleanQuotes, bool CleanSlashes)
        {
            // Removing tabulations...
            RecvStr = RemoveTabs(RecvStr);

            // Replacing all NUL symbols with spaces...
            RecvStr = RemoveNullBytes(RecvStr);

            // Removing multiple spaces...
            RecvStr = RemoveMultipleSpaces(RecvStr);

            // Removing quotes if enabled...
            if (CleanQuotes)
            {
                while (RecvStr.IndexOf(@"""", StringComparison.InvariantCulture) != -1)
                {
                    RecvStr = RecvStr.Replace(@"""", string.Empty);
                }
            }

            // Removing double slashes if enabled...
            if (CleanSlashes)
            {
                while (RecvStr.IndexOf(@"\\", StringComparison.InvariantCulture) != -1)
                {
                    RecvStr = RecvStr.Replace(@"\\", @"\");
                }
            }

            // Return result with removal of leading and trailing white-spaces...
            return RecvStr.Trim();
        }

        /// <summary>
        /// Remove different special characters from specified string.
        /// </summary>
        /// <param name="RecvStr">Source string for cleanup.</param>
        /// <returns>Clean string with removed special characters.</returns>
        public static string CleanString(string RecvStr)
        {
            return CleanString(RecvStr, false, false);
        }

        /// <summary>
        /// Return contents of text file from internal resource section
        /// of the calling assembly.
        /// </summary>
        /// <param name="FileName">Internal resource file name.</param>
        /// <returns>Contents of bundled in resource text file.</returns>
        public static string GetTemplateFromResource(string FileName)
        {
            string Result = string.Empty;
            using (StreamReader Reader = new StreamReader(Assembly.GetCallingAssembly().GetManifestResourceStream(FileName)))
            {
                Result = Reader.ReadToEnd();
            }
            return Result;
        }
    }
}
