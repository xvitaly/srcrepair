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
using System.IO;
using System.Xml;
using NLog;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with external plugins.
    /// </summary>
    public sealed class PluginManager
    {
        /// <summary>
        /// Logger instance for PluginManager class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Stores the list of available plugins.
        /// </summary>
        private readonly Dictionary<string, PluginTarget> Plugins;

        /// <summary>
        /// Overloading inxeding operator to return plugin target instance
        /// by specified name.
        /// </summary>
        public PluginTarget this[string key] => Plugins[key];

        /// <summary>
        /// PluginManager class constructor.
        /// </summary>
        /// <param name="FullAppPath">Path to SRC Repair installation directory.</param>
        public PluginManager(string FullAppPath)
        {
            // Initializing an empty dictionary...
            Plugins = new Dictionary<string, PluginTarget>();

            // Fetching the list of available plugins from the XML database file...
            using (FileStream XMLFS = new FileStream(Path.Combine(FullAppPath, StringsManager.PluginsDatabaseName), FileMode.Open, FileAccess.Read))
            {
                // Loading XML file from file stream...
                XmlDocument XMLD = new XmlDocument();
                XMLD.Load(XMLFS);

                // Parsing XML and filling our structures...
                foreach (XmlNode XmlItem in XMLD.SelectNodes("Plugins/Plugin"))
                {
                    try
                    {
                        Plugins.Add(XmlItem.SelectSingleNode("IntName").InnerText, new PluginTarget(XmlItem.SelectSingleNode("Name").InnerText, Path.Combine(FullAppPath, XmlItem.SelectSingleNode("ExeName").InnerText), XmlItem.SelectSingleNode("ElevationRequired").InnerText == "1"));
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, DebugStrings.AppDbgExCorePluginManConstructor);
                    }
                }
            }
        }
    }
}
