/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
