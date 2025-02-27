/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using NLog;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with collection of cleanup targets.
    /// </summary>
    public sealed class CleanupManager
    {
        /// <summary>
        /// Logger instance for CleanupManager class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Stores full list of available cleanup targets.
        /// </summary>
        private readonly Dictionary<int, CleanupTarget> CleanupTargets;

        /// <summary>
        /// Stores whether unsafe cleanup methods are allowed.
        /// </summary>
        private readonly bool AllowUnsafe;

        /// <summary>
        /// Stores path to installation directory without SmallAppName.
        /// </summary>
        private readonly string GamePath;

        /// <summary>
        /// Stores full path to installation directory.
        /// </summary>
        private readonly string FullGamePath;

        /// <summary>
        /// Stores full path of Workshop directory.
        /// </summary>
        private readonly string AppWorkshopDir;

        /// <summary>
        /// Stores full path to cloud screenshots directory.
        /// </summary>
        private readonly string CloudScreenshotsPath;

        /// <summary>
        /// Overloading inxeding operator to return cleanup target instance
        /// by specified name.
        /// </summary>
        public CleanupTarget this[int key] => CleanupTargets[key];

        /// <summary>
        /// Fill templates with real application paths.
        /// </summary>
        /// <param name="Row">String with templates to be filled.</param>
        /// <returns>Fully qualified string with path.</returns>
        private string ParseRow(string Row)
        {
            StringBuilder Result = new StringBuilder(Row);

            Result.Replace("$GamePath$", GamePath);
            Result.Replace("$FullGamePath$", FullGamePath);
            Result.Replace("$AppWorkshopDir$", AppWorkshopDir);
            Result.Replace("$CloudScreenshotsPath$", CloudScreenshotsPath);
            Result.Replace('/', Path.DirectorySeparatorChar);

            return Result.ToString();
        }

        /// <summary>
        /// Gets fully qualified path from specified source string.
        /// </summary>
        /// <param name="Row">Source string.</param>
        /// <returns>Fully qualified path.</returns>
        private string GetFullPath(string Row)
        {
            return ParseRow(Row);
        }

        /// <summary>
        /// Extracts the list of cleanup items from the XML node.
        /// </summary>
        /// <param name="XmlItem">Source XML node item.</param>
        /// <returns>The list of cleanup items.</returns>
        private List<CleanupItem> GetDirListFromNode(XmlNode XmlItem)
        {
            List<CleanupItem> Result = new List<CleanupItem>();

            foreach (XmlNode Item in XmlItem.SelectNodes("Directories/Directory"))
            {
                if (Item.SelectSingleNode("Class").InnerText == "Safe" || AllowUnsafe)
                {
                    Result.Add(new CleanupItem(GetFullPath(Item.SelectSingleNode("Path").InnerText), Item.SelectSingleNode("Extension").InnerText, Item.SelectSingleNode("Recursive").InnerText == "1", Item.SelectSingleNode("CleanEmpty").InnerText == "1"));
                }
            }

            return Result;
        }

        /// <summary>
        /// CleanupManager class constructor.
        /// </summary>
        /// <param name="FullAppPath">Path to SRC Repair installation directory.</param>
        /// <param name="SelectedGame">Instance of SourceGame class with selected in main window game.</param>
        /// <param name="UnsafeCleanup">Allow or disallow to use unsafe cleanup methods.</param>
        public CleanupManager(string FullAppPath, SourceGame SelectedGame, bool UnsafeCleanup)
        {
            // Filling some private fields...
            AllowUnsafe = UnsafeCleanup;
            GamePath = SelectedGame.GamePath;
            FullGamePath = SelectedGame.FullGamePath;
            AppWorkshopDir = SelectedGame.AppWorkshopDir;
            CloudScreenshotsPath = SelectedGame.CloudScreenshotsPath;

            // Initializing empty dictionary...
            CleanupTargets = new Dictionary<int, CleanupTarget>();

            // Fetching list of available cleanup targets from XML database file...
            using (FileStream XMLFS = new FileStream(Path.Combine(FullAppPath, Properties.Resources.CleanupDbFile), FileMode.Open, FileAccess.Read))
            {
                // Loading XML file from file stream...
                XmlDocument XMLD = new XmlDocument();
                XMLD.Load(XMLFS);

                // Parsing XML and filling our structures...
                foreach (XmlNode XmlItem in XMLD.SelectNodes("Targets/Target"))
                {
                    try
                    {
                        CleanupTargets.Add(int.Parse(XmlItem.SelectSingleNode("ID").InnerText), new CleanupTarget(XmlItem.SelectSingleNode("Name").InnerText, GetDirListFromNode(XmlItem)));
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, DebugStrings.AppDbgExCoreClnManConstructor);
                    }
                }
            }
        }
    }
}
