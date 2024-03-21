/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
