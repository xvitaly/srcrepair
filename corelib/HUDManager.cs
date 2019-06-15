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
using System.Xml;
using System.Collections.Generic;
using NLog;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with collection of HUDs.
    /// </summary>
    public sealed class HUDManager
    {
        /// <summary>
        /// Logger instance for HUDManager class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Store full list of available HUDs.
        /// </summary>
        private readonly List<HUDTlx> HUDsAvailable;

        /// <summary>
        /// Returns HUD by it's name.
        /// </summary>
        /// <param name="HUDName">HUD name.</param>
        private HUDTlx GetHUDByName(string HUDName)
        {
            return HUDsAvailable.Find(Item => String.Equals(Item.Name, HUDName, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Overloaded inxeding operator, returning HUD by specified name.
        /// </summary>
        public HUDTlx this[string key] => GetHUDByName(key);

        /// <summary>
        /// Gets list of available HUD names for specified game ID.
        /// </summary>
        /// <param name="GameName">Game ID.</param>
        /// <returns>List of available HUD names for specified game ID.</returns>
        public List<String> GetHUDNames(string GameName)
        {
            // Creating an empty list...
            List<String> Result = new List<String>();

            // Executing LINQ query...
            foreach (HUDTlx HUD in HUDsAvailable.FindAll(Item => String.Equals(Item.Game, GameName, StringComparison.CurrentCultureIgnoreCase)))
            {
                Result.Add(HUD.Name);
            }

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Checks if specified HUD installed.
        /// </summary>
        /// <param name="CustomInstallDir">Full path to custom game stuff directory.</param>
        /// <param name="HUDDir">HUD installation directory name.</param>
        /// <returns>Return True if specified config is installed.</returns>
        public static bool CheckInstalledHUD(string CustomInstallDir, string HUDDir)
        {
            // Creating some local variables...
            bool Result = false;
            string HUDPath = Path.Combine(CustomInstallDir, HUDDir);

            // Checks if directory exists...
            if (Directory.Exists(HUDPath))
            {
                // Checks if any files exists in this directory...
                using (IEnumerator<String> StrEn = Directory.EnumerateFileSystemEntries(HUDPath).GetEnumerator())
                {
                    Result = StrEn.MoveNext();
                }
            }

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Formats path with platform-dependent trailing path separator.
        /// </summary>
        /// <param name="IntDir">Source string with full path.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatIntDir(string IntDir)
        {
            return IntDir.Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Checks if HUD database need to be updated.
        /// </summary>
        /// <param name="LastHUDUpdate">Date of the last HUD database update.</param>
        /// <returns>Returns True if database need to be updated.</returns>
        public static bool CheckHUDDatabase(DateTime LastHUDUpdate)
        {
            return (DateTime.Now - LastHUDUpdate).Days >= 7;
        }

        /// <summary>
        /// ConfigManager class constructor.
        /// </summary>
        /// <param name="FullAppPath">Full path to application's directory</param>
        /// <param name="AppHUDDir">Full HUD directory installation path</param>
        /// <param name="HideOutdated">Enable hiding of outdated HUDs.</param>
        public HUDManager(string FullAppPath, string AppHUDDir, bool HideOutdated = true)
        {
            // Initializing empty list...
            HUDsAvailable = new List<HUDTlx>();

            // Fetching list of available HUDs from XML database file...
            using (FileStream XMLFS = new FileStream(Path.Combine(FullAppPath, StringsManager.HudDatabaseName), FileMode.Open, FileAccess.Read))
            {
                // Loading XML file from file stream...
                XmlDocument XMLD = new XmlDocument();
                XMLD.Load(XMLFS);

                // Parsing XML and filling our structures...
                for (int i = 0; i < XMLD.GetElementsByTagName("HUD").Count; i++)
                {
                    try
                    {
                        if (!HideOutdated || XMLD.GetElementsByTagName("IsUpdated")[i].InnerText == "1")
                        {
                            HUDsAvailable.Add(new HUDTlx(AppHUDDir, XMLD.GetElementsByTagName("Name")[i].InnerText, XMLD.GetElementsByTagName("Game")[i].InnerText, XMLD.GetElementsByTagName("URI")[i].InnerText, XMLD.GetElementsByTagName("UpURI")[i].InnerText, XMLD.GetElementsByTagName("IsUpdated")[i].InnerText == "1", XMLD.GetElementsByTagName("Preview")[i].InnerText, XMLD.GetElementsByTagName("LastUpdate")[i].InnerText, XMLD.GetElementsByTagName("Site")[i].InnerText, XMLD.GetElementsByTagName("ArchiveDir")[i].InnerText, XMLD.GetElementsByTagName("InstallDir")[i].InnerText, XMLD.GetElementsByTagName("Hash")[i].InnerText, Path.Combine(AppHUDDir, Path.ChangeExtension(Path.GetFileName(XMLD.GetElementsByTagName("Name")[i].InnerText), ".zip"))));
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, "Minor exception while building HUD list object.");
                    }
                }
            }
        }
    }
}
