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
using System.Net;
using System.Reflection;
using System.Xml;

namespace srcrepair.core
{
    /// <summary>
    /// Class for checking and working with updates.
    /// </summary>
    public class UpdateManager
    {
        /// <summary>
        /// Gets or sets latest available version of application.
        /// </summary>
        public Version AppUpdateVersion { get; private set; }

        /// <summary>
        /// Gets or sets download URL of application update.
        /// </summary>
        public string AppUpdateURL { get; private set; }

        /// <summary>
        /// Gets or sets hash of application update file.
        /// </summary>
        public string AppUpdateHash { get; private set; }

        /// <summary>
        /// Gets or sets download URL of game database update.
        /// </summary>
        public string GameUpdateURL { get; private set; }

        /// <summary>
        /// Gets or sets hash of game database update file.
        /// </summary>
        public string GameUpdateHash { get; private set; }

        /// <summary>
        /// Gets or sets download URL of HUD database update.
        /// </summary>
        public string HUDUpdateURL { get; private set; }

        /// <summary>
        /// Gets or sets hash of HUD database update file.
        /// </summary>
        public string HUDUpdateHash { get; private set; }

        /// <summary>
        /// Gets or sets download URL of configs database update.
        /// </summary>
        public string CfgUpdateURL { get; private set; }

        /// <summary>
        /// Gets or sets hash of configs database update file.
        /// </summary>
        public string CfgUpdateHash { get; private set; }

        /// <summary>
        /// Stores full path to installed application.
        /// </summary>
        private readonly string FullAppPath;

        /// <summary>
        /// Stores HTTP UserAgent header, used during updates check.
        /// </summary>
        private readonly string UserAgent;

        /// <summary>
        /// Stores downloaded from server XML file with information
        /// about latest updates.
        /// </summary>
        private string UpdateXML;

        /// <summary>
        /// Downloads XML file with imformation about latest updates
        /// from server and stores it in class field.
        /// </summary>
        private void DownloadXML()
        {
            using (WebClient Downloader = new WebClient())
            {
                Downloader.Headers.Add("User-Agent", UserAgent);
                UpdateXML = Downloader.DownloadString(Properties.Resources.UpdateDBURL);
            }
        }

        /// <summary>
        /// Parses downloaded XML file with information about
        /// latest updates and fills our properties.
        /// </summary>
        private void ParseXML()
        {
            // Loading XML from variable...
            XmlDocument XMLD = new XmlDocument();
            XMLD.LoadXml(UpdateXML);

            // Parsing XML...
            foreach (XmlNode Node in XMLD.SelectNodes("Updates"))
            {
                foreach (XmlNode Child in Node.ChildNodes)
                {
                    switch (Child.Name)
                    {
                        case "Application":
                            AppUpdateVersion = new Version(Child.ChildNodes[0].InnerText);
                            AppUpdateURL = Child.ChildNodes[1].InnerText;
                            AppUpdateHash = Child.ChildNodes[2].InnerText;
                            break;
                        case "GameDB":
                            GameUpdateURL = Child.ChildNodes[1].InnerText;
                            GameUpdateHash = Child.ChildNodes[2].InnerText;
                            break;
                        case "HUDDB":
                            HUDUpdateURL = Child.ChildNodes[1].InnerText;
                            HUDUpdateHash = Child.ChildNodes[2].InnerText;
                            break;
                        case "CfgDB":
                            CfgUpdateURL = Child.ChildNodes[1].InnerText;
                            CfgUpdateHash = Child.ChildNodes[2].InnerText;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if application needs to be updated.
        /// </summary>
        /// <returns>Returns True if application update available.</returns>
        public bool CheckAppUpdate()
        {
            return AppUpdateVersion > Assembly.GetCallingAssembly().GetName().Version;
        }

        /// <summary>
        /// Checks hash of downloaded application update.
        /// </summary>
        /// <param name="Hash">Hash of downloaded file.</param>
        /// <returns>Returns True if hashes are equal.</returns>
        public bool CheckAppHash(string Hash)
        {
            return AppUpdateHash == Hash;
        }

        /// <summary>
        /// Checks if game database needs to be updated.
        /// </summary>
        /// <returns>Returns True if game database update available.</returns>
        public bool CheckGameDBUpdate()
        {
            return FileManager.CalculateFileMD5(Path.Combine(FullAppPath, Properties.Resources.GameListFile)) != GameUpdateHash;
        }

        /// <summary>
        /// Checks if HUD database needs to be updated.
        /// </summary>
        /// <returns>Returns True if HUD database update available.</returns>
        public bool CheckHUDUpdate()
        {
            return FileManager.CalculateFileMD5(Path.Combine(FullAppPath, Properties.Resources.HUDDbFile)) != HUDUpdateHash;
        }

        /// <summary>
        /// Checks if configs database needs to be updated.
        /// </summary>
        /// <returns>Returns True if configs database update available.</returns>
        public bool CheckCfgUpdate()
        {
            return FileManager.CalculateFileMD5(Path.Combine(FullAppPath, Properties.Resources.CfgDbFile)) != CfgUpdateHash;
        }

        /// <summary>
        /// Gets local application update file name.
        /// </summary>
        /// <param name="Url">Download URL.</param>
        /// <returns>Local file name.</returns>
        public static string GenerateUpdateFileName(string Url)
        {
            return Path.HasExtension(Url) ? Url : Path.ChangeExtension(Url, "exe");
        }

        /// <summary>
        /// UpdateManager class constructor.
        /// </summary>
        /// <param name="AppPath">Application installation directory.</param>
        /// <param name="UA">User-Agent header for outgoing HTTP queries.</param>
        public UpdateManager(string AppPath, string UA)
        {
            // Saving paths...
            FullAppPath = AppPath;
            UserAgent = UA;

            // Downloading and parsing XML...
            DownloadXML();
            ParseXML();
        }
    }
}
