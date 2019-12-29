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
    /// Class for working with collection of FPS-configs.
    /// </summary>
    public sealed class ConfigManager
    {
        /// <summary>
        /// Logger instance for ConfigManager class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Store full list of available FPS-configs.
        /// </summary>
        private readonly Dictionary<string, FPSConfig> Configs;

        /// <summary>
        /// Gets list of all available FPS-config names.
        /// </summary>
        public List<String> ConfigNames
        {
            get
            {
                List<String> Result = new List<String>();
                foreach (string Cfg in Configs.Keys) { Result.Add(Cfg); }
                return Result;
            }
        }

        /// <summary>
        /// Overloading inxeding operator to return FPS-config by specified name.
        /// </summary>
        public FPSConfig this[string key] => Configs[key];

        /// <summary>
        /// Gets or sets full path to FPS-config installation directory.
        /// </summary>
        public string FPSConfigInstallPath { get; private set; }

        /// <summary>
        /// Get list of common FPS-config paths installation.
        /// </summary>
        /// <param name="GamePath">Game installation directory.</param>
        /// <param name="UserDir">If game use custom user directories for custom stuff.</param>
        /// <returns>List of common FPS-config paths.</returns>
        public static List<String> ListFPSConfigs(string GamePath, bool UserDir)
        {
            List<String> Result = new List<String> { Path.Combine(GamePath, "cfg", "autoexec.cfg") };
            if (UserDir) { Result.Add(Path.Combine(GamePath, "custom", "autoexec.cfg")); }
            return Result;
        }

        /// <summary>
        /// Checks if specified FPS-config is installed.
        /// </summary>
        /// <param name="ConfigInstallDirectory">HUD installation directory name.</param>
        /// <returns>Return True if specified FPS-config is installed.</returns>
        public bool CheckInstalledConfig(string ConfigInstallDirectory)
        {
            // Creating some local variables...
            bool Result = false;
            string FullInstallPath = Path.Combine(FPSConfigInstallPath, ConfigInstallDirectory);

            // Checks if directory exists...
            if (Directory.Exists(FullInstallPath))
            {
                // Checks if any files exists in this directory...
                using (IEnumerator<String> StrEn = Directory.EnumerateFileSystemEntries(FullInstallPath).GetEnumerator())
                {
                    Result = StrEn.MoveNext();
                }
            }

            // Returning result...
            return Result;
        }

        /// <summary>
        /// ConfigManager class constructor.
        /// </summary>
        /// <param name="FullAppPath">Path to SRC Repair installation directory.</param>
        /// <param name="AppCfgDir">Full path to the local FPS-configs download directory.</param>
        /// <param name="Destination">Full path FPS-config installation directory.</param>
        /// <param name="LangPrefix">SRC Repair language code.</param>
        public ConfigManager(string FullAppPath, string AppCfgDir, string Destination, string LangPrefix)
        {
            // Initializing empty dictionary...
            Configs = new Dictionary<string, FPSConfig>();
            FPSConfigInstallPath = Destination;

            // Fetching list of available FPS-configs from XML database file...
            using (FileStream XMLFS = new FileStream(Path.Combine(FullAppPath, StringsManager.ConfigDatabaseName), FileMode.Open, FileAccess.Read))
            {
                // Loading XML file from file stream...
                XmlDocument XMLD = new XmlDocument();
                XMLD.Load(XMLFS);

                // Parsing XML and filling our structures...
                foreach (XmlNode XmlItem in XMLD.SelectNodes("Configs/Config"))
                {
                    try
                    {
                        Configs.Add(XmlItem.SelectSingleNode("Name").InnerText, new FPSConfig(XmlItem.SelectSingleNode("Name").InnerText, XmlItem.SelectSingleNode("URI").InnerText, XmlItem.SelectSingleNode(LangPrefix).InnerText, XmlItem.SelectSingleNode("SupportedGames").InnerText.Split(';'), XmlItem.SelectSingleNode("ArchiveDir").InnerText, XmlItem.SelectSingleNode("InstallDir").InnerText, XmlItem.SelectSingleNode("Hash2").InnerText, Path.Combine(AppCfgDir, Path.GetFileName(XmlItem.SelectSingleNode("URI").InnerText))));
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, DebugStrings.AppDbgExCoreConfManConstructor);
                    }
                }
            }
        }
    }
}
