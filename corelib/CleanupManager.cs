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
        /// Store full list of available cleanup targets.
        /// </summary>
        private readonly Dictionary<string, CleanupTarget> CleanupTargets;

        /// <summary>
        /// Overloading inxeding operator to return cleanup target instance
        /// by specified name.
        /// </summary>
        public CleanupTarget this[string key] => CleanupTargets[key];

        /// <summary>
        /// CleanupManager class constructor.
        /// </summary>
        /// <param name="FullAppPath">Path to SRC Repair installation directory.</param>
        public CleanupManager(string FullAppPath)
        {
            // Initializing empty dictionary...
            CleanupTargets = new Dictionary<string, CleanupTarget>();

            // Fetching list of available cleanup targets from XML database file...
            using (FileStream XMLFS = new FileStream(Path.Combine(FullAppPath, StringsManager.CleanupDatabaseName), FileMode.Open, FileAccess.Read))
            {
                // Loading XML file from file stream...
                XmlDocument XMLD = new XmlDocument();
                XMLD.Load(XMLFS);

                // Parsing XML and filling our structures...
                for (int i = 0; i < XMLD.GetElementsByTagName("Target").Count; i++)
                {
                    try
                    {
                        List<String> DirList = new List<String>();

                        foreach (XmlNode CtDir in XMLD.GetElementsByTagName("Directories")[i])
                        {
                            if (CtDir.Attributes["Class"].Value == "Safe")
                            {
                                DirList.Add(CtDir.InnerText);
                            }
                        }

                        CleanupTargets.Add(XMLD.GetElementsByTagName("ID")[i].InnerText, new CleanupTarget(XMLD.GetElementsByTagName("Name")[i].InnerText, DirList));
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, "Minor exception while while building CleanupTargets list object.");
                    }
                }
            }
        }
    }
}
