/**
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
        public List<String> InstalledGameNames
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
                foreach (XmlNode XmlItem in XMLD.SelectNodes("Games/Game"))
                {
                    try
                    {
                        if (XmlItem.SelectSingleNode("Enabled").InnerText == "1" || !HideUnsupported)
                        {
                            SourceGame SG = new SourceGame(XmlItem.Attributes["Name"].Value, XmlItem.SelectSingleNode("DirName").InnerText, XmlItem.SelectSingleNode("SmallName").InnerText, XmlItem.SelectSingleNode("Executable").InnerText, XmlItem.SelectSingleNode("SID").InnerText, Convert.ToInt32(XmlItem.SelectSingleNode("SVer").InnerText), XmlItem.SelectSingleNode("VFDir").InnerText, XmlItem.SelectSingleNode("UserDir").InnerText == "1", XmlItem.SelectSingleNode("HUDsAvail").InnerText == "1", App.AppUserDir, App.SteamClient.FullSteamPath, App.Platform.SteamAppsFolderName, App.SteamClient.SteamID, GameDirs, App.Platform.OS);
                            if (SG.IsInstalled)
                            {
                                SourceGames.Add(XmlItem.Attributes["Name"].Value, SG);
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, DebugStrings.AppDbgExCoreGameManConstructor);
                    }
                }
            }
        }
    }
}
