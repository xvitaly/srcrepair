﻿/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
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
        /// Checks if specified FPS-config is installed (with support of custom game directory).
        /// </summary>
        /// <param name="ConfigInstallDirectory">HUD installation directory name.</param>
        /// <returns>Return True if specified FPS-config is installed.</returns>
        private bool CheckInstalledConfigCustom(string ConfigInstallDirectory)
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
        /// Checks if specified FPS-config is installed (legacy games without custom user
        /// directory support).
        /// </summary>
        /// <returns>Return True if specified FPS-config is installed.</returns>
        private bool CheckInstalledConfigLegacy()
        {
            // Returning result...
            return File.Exists(Path.Combine(FPSConfigInstallPath, "cfg", "autoexec.cfg"));
        }

        /// <summary>
        /// Checks if specified FPS-config is installed.
        /// </summary>
        /// <param name="ConfigInstallDirectory">FPS-config installation directory name.</param>
        /// <param name="IsUsingUserDir">If current game is using a custom user stuff directory.</param>
        /// <returns>Return True if specified FPS-config is installed.</returns>
        public bool CheckInstalledConfig(string ConfigInstallDirectory, bool IsUsingUserDir)
        {
            // Returning result...
            return IsUsingUserDir ? CheckInstalledConfigCustom(ConfigInstallDirectory) : CheckInstalledConfigLegacy();
        }

        /// <summary>
        /// Moves installed FPS-configs to legacy location for games
        /// without custom user directory support.
        /// </summary>
        /// <param name="ConfigInstallDirectory">FPS-config installation directory name.</param>
        /// <param name="FullCfgPath">Full path to directory with game configs.</param>
        public void MoveLegacyConfig(string ConfigInstallDirectory, string FullCfgPath)
        {
            // Generating source directory full path...
            string SourcePath = Path.Combine(FPSConfigInstallPath, ConfigInstallDirectory);

            // Moving FPS-config files...
            FileManager.MoveDirectoryContents(Path.Combine(SourcePath, "cfg"), FullCfgPath);

            // Performing cleanup...
            if (Directory.Exists(SourcePath))
            {
                Directory.Delete(SourcePath, true);
            }
        }

        /// <summary>
        /// Extracts the list of strings from the XML node.
        /// </summary>
        /// <param name="XmlItem">Source XML node item.</param>
        /// <returns>List of strings.</returns>
        private List<String> GetFilesListFromNode(XmlNode XmlItem)
        {
            List<String> Result = new List<String>();

            foreach (XmlNode CtFiles in XmlItem.SelectSingleNode("Files"))
            {
                Result.Add(FileManager.NormalizeDirectorySeparators(CtFiles.InnerText));
            }

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
                        Configs.Add(XmlItem.SelectSingleNode("Name").InnerText, new FPSConfig(XmlItem.SelectSingleNode("Name").InnerText, XmlItem.SelectSingleNode("URI").InnerText, XmlItem.SelectSingleNode("Mirror").InnerText, XmlItem.SelectSingleNode(LangPrefix).InnerText, XmlItem.SelectSingleNode("SupportedGames").InnerText.Split(';'), XmlItem.SelectSingleNode("ArchiveDir").InnerText, GetFilesListFromNode(XmlItem), XmlItem.SelectSingleNode("InstallDir").InnerText, XmlItem.SelectSingleNode("Hash2").InnerText, Path.Combine(AppCfgDir, Path.GetFileName(XmlItem.SelectSingleNode("URI").InnerText))));
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
