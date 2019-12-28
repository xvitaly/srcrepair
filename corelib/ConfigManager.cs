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
        /// Install FPS-config to game.
        /// </summary>
        /// <param name="ConfName">FPS-config name.</param>
        /// <param name="AppPath">Path to SRC Repair installation directory.</param>
        /// <param name="GameDir">Path to game installation directory.</param>
        /// <param name="UseCustmDir">If game use custom user directories for custom stuff.</param>
        /// <param name="CustmDir">Name of custom game stuff directory.</param>
        public static void InstallConfigNow(string ConfName, string AppPath, string GameDir, bool UseCustmDir, string CustmDir)
        {
            // Generating full path to destination...
            string DestPath = Path.Combine(GameDir, UseCustmDir ? Path.Combine("custom", CustmDir) : String.Empty, "cfg");

            // Checking if destination exists. If not, creating...
            if (!Directory.Exists(DestPath)) { Directory.CreateDirectory(DestPath); }

            // Installing FPS-config by copying it's file to destination...
            File.Copy(Path.Combine(AppPath, "cfgs", ConfName), Path.Combine(DestPath, "autoexec.cfg"), true);
        }

        /// <summary>
        /// ConfigManager class constructor.
        /// </summary>
        /// <param name="FullAppPath">Path to SRC Repair installation directory.</param>
        /// <param name="LangPrefix">SRC Repair language code.</param>
        public ConfigManager(string FullAppPath, string LangPrefix)
        {
            // Initializing empty dictionary...
            Configs = new Dictionary<string, FPSConfig>();

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
                        Configs.Add(XmlItem.SelectSingleNode("Name").InnerText, new FPSConfig(XmlItem.SelectSingleNode("Name").InnerText, XmlItem.SelectSingleNode("URI").InnerText, XmlItem.SelectSingleNode(LangPrefix).InnerText, XmlItem.SelectSingleNode("SupportedGames").InnerText.Split(';'), XmlItem.SelectSingleNode("ArchiveDir").InnerText, XmlItem.SelectSingleNode("InstallDir").InnerText));
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
