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
    /// Class for working with collection of available games.
    /// </summary>
    public sealed class GameManager
    {
        /// <summary>
        /// Logger instance for GameManager class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Store full list of available games.
        /// </summary>
        private readonly Dictionary<string, SourceGame> SourceGames;

        /// <summary>
        /// Get names of installed games.
        /// </summary>
        public List<String> InstalledGames
        {
            get
            {
                List<String> Result = new List<String>();
                foreach (string IG in SourceGames.Keys) { Result.Add(IG); }
                return Result;
            }
        }

        /// <summary>
        /// Overloaded inxeding operator, returning SourceGame instance by specified name.
        /// </summary>
        public SourceGame this[string key] => SourceGames[key];

        /// <summary>
        /// GameManager class constructor.
        /// </summary>
        /// <param name="App">CurrentApp class instance.</param>
        /// <param name="HideUnsupported">Enable or disable adding unsupported games to list.</param>
        public GameManager(CurrentApp App, bool HideUnsupported = true)
        {
            // Creating empty dictionary...
            SourceGames = new Dictionary<string, SourceGame>();

            // Fetching game libraries...
            List<String> GameDirs = App.SteamClient.FormatInstallDirs(App.Platform.SteamAppsFolderName);

            // Creating FileStream for XML database...
            using (FileStream XMLFS = new FileStream(Path.Combine(App.FullAppPath, Properties.Resources.GameListFile), FileMode.Open, FileAccess.Read))
            {
                // Loading XML file from file stream...
                XmlDocument XMLD = new XmlDocument();
                XMLD.Load(XMLFS);

                // Parsing XML and filling our structures...
                XmlNodeList XMLNode = XMLD.GetElementsByTagName("Game");
                for (int i = 0; i < XMLNode.Count; i++)
                {
                    try
                    {
                        if (XMLD.GetElementsByTagName("Enabled")[i].InnerText == "1" || !HideUnsupported)
                        {
                            SourceGame SG = new SourceGame(XMLNode[i].Attributes["Name"].Value, XMLD.GetElementsByTagName("DirName")[i].InnerText, XMLD.GetElementsByTagName("SmallName")[i].InnerText, XMLD.GetElementsByTagName("Executable")[i].InnerText, XMLD.GetElementsByTagName("SID")[i].InnerText, XMLD.GetElementsByTagName("SVer")[i].InnerText, XMLD.GetElementsByTagName("VFDir")[i].InnerText, App.Platform.OS == CurrentPlatform.OSType.Windows ? XMLD.GetElementsByTagName("HasVF")[i].InnerText == "1" : true, XMLD.GetElementsByTagName("UserDir")[i].InnerText == "1", XMLD.GetElementsByTagName("HUDsAvail")[i].InnerText == "1", App.FullAppPath, App.AppUserDir, App.SteamClient.FullSteamPath, App.Platform.SteamAppsFolderName, App.SteamClient.SteamID, GameDirs, App.Platform.OS);
                            if (SG.IsInstalled)
                            {
                                SourceGames.Add(XMLNode[i].Attributes["Name"].Value, SG);
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, "Minor exception while building games list object.");
                    }
                }
            }
        }
    }
}
