/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 *
 * Copyright (c) 2011 - 2021 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2021 EasyCoding Team.
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

            // Extracting information about application update...
            XmlNode AppNode = XMLD.SelectSingleNode("Updates/Application");
            AppUpdateVersion = new Version(AppNode.SelectSingleNode("Version").InnerText);
            AppUpdateURL = AppNode.SelectSingleNode("URL").InnerText;
            AppUpdateHash = AppNode.SelectSingleNode("Hash2").InnerText;
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
        /// <param name="UA">User-Agent header for outgoing HTTP queries.</param>
        public UpdateManager(string UA)
        {
            // Saving paths...
            UserAgent = UA;

            // Downloading and parsing XML...
            DownloadXML();
            ParseXML();
        }
    }
}
