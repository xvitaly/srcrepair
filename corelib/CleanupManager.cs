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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using NLog;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with collection of cleanup targets.
    /// </summary>
    public sealed class CleanupManager
    {
        /// <summary>
        /// Logger instance for CleanupManager class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Stores full list of available cleanup targets.
        /// </summary>
        private readonly Dictionary<string, CleanupTarget> CleanupTargets;

        /// <summary>
        /// Stores path to installation directory without SmallAppName.
        /// </summary>
        private readonly string GamePath;

        /// <summary>
        /// Stores full path to installation directory.
        /// </summary>
        private readonly string FullGamePath;

        /// <summary>
        /// Stores full path of Workshop directory.
        /// </summary>
        private readonly string AppWorkshopDir;

        /// <summary>
        /// Stores full path to cloud screenshots directory.
        /// </summary>
        private readonly string CloudScreenshotsPath;

        /// <summary>
        /// Overloading inxeding operator to return cleanup target instance
        /// by specified name.
        /// </summary>
        public CleanupTarget this[string key] => CleanupTargets[key];

        /// <summary>
        /// Fill templates with real application paths.
        /// </summary>
        /// <param name="Row">String with templates to be filled.</param>
        /// <returns>Fully qualified string with path.</returns>
        private string ParseRow(string Row)
        {
            StringBuilder Result = new StringBuilder(Row);

            Result.Replace("$GamePath$", GamePath);
            Result.Replace("$FullGamePath$", FullGamePath);
            Result.Replace("$AppWorkshopDir$", AppWorkshopDir);
            Result.Replace("$CloudScreenshotsPath$", CloudScreenshotsPath);
            Result.Replace('/', Path.DirectorySeparatorChar);

            return Result.ToString();
        }

        /// <summary>
        /// Gets fully qualified path from specified source string.
        /// </summary>
        /// <param name="Row">Source string.</param>
        /// <returns>Fully qualified path.</returns>
        private string GetFullPath(string Row)
        {
            return ParseRow(Row);
        }

        /// <summary>
        /// CleanupManager class constructor.
        /// </summary>
        /// <param name="FullAppPath">Path to SRC Repair installation directory.</param>
        /// <param name="SelectedGame">Instance of SourceGame class with selected in main window game.</param>
        /// <param name="AllowUnsafe">Allow or disallow to use unsafe cleanup methods.</param>
        public CleanupManager(string FullAppPath, SourceGame SelectedGame, bool AllowUnsafe = false)
        {
            // Filling some private fields...
            GamePath = SelectedGame.GamePath;
            FullGamePath = SelectedGame.FullGamePath;
            AppWorkshopDir = SelectedGame.AppWorkshopDir;
            CloudScreenshotsPath = SelectedGame.CloudScreenshotsPath;

            // Initializing empty dictionary...
            CleanupTargets = new Dictionary<string, CleanupTarget>();

            // Fetching list of available cleanup targets from XML database file...
            using (FileStream XMLFS = new FileStream(Path.Combine(FullAppPath, StringsManager.CleanupDatabaseName), FileMode.Open, FileAccess.Read))
            {
                // Loading XML file from file stream...
                XmlDocument XMLD = new XmlDocument();
                XMLD.Load(XMLFS);

                // Parsing XML and filling our structures...
                foreach (XmlNode XmlItem in XMLD.SelectNodes("Targets/Target"))
                {
                    try
                    {
                        List<String> DirList = new List<String>();

                        foreach (XmlNode CtDir in XmlItem.SelectSingleNode("Directories"))
                        {
                            if (CtDir.Attributes["Class"].Value == "Safe" || AllowUnsafe)
                            {
                                DirList.Add(GetFullPath(CtDir.InnerText));
                            }
                        }

                        CleanupTargets.Add(XmlItem.SelectSingleNode("ID").InnerText, new CleanupTarget(XmlItem.SelectSingleNode("Name").InnerText, DirList));
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, DebugStrings.AppDbgExCoreClnManConstructor);
                    }
                }
            }
        }
    }
}
