/**
 * SPDX-FileCopyrightText: 2011-2021 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with Steam client.
    /// </summary>
    public sealed class SteamManager
    {
        /// <summary>
        /// Logger instance for SteamManager class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets or sets full path to Steam client directory.
        /// </summary>
        public string FullSteamPath { get; private set; }

        /// <summary>
        /// Gets list of available UserIDs.
        /// </summary>
        public List<String> SteamIDs { get; private set; }

        /// <summary>
        /// Gets selected or default UserID.
        /// </summary>
        public string SteamID { get; set; }

        /// <summary>
        /// Gets or sets full path to Steam userdata directory.
        /// </summary>
        private string UserDataPath { get; set; }

        /// <summary>
        /// Checks if specified UserID currently available. If not,
        /// returns first available value.
        /// </summary>
        /// <param name="SID">UserID to check.</param>
        /// <returns>Checked or default UserID.</returns>
        public string GetCurrentSteamID(string SID)
        {
            if (SteamIDs.Count < 1)
            {
                throw new ArgumentOutOfRangeException(DebugStrings.AppDbgExCoreStmManSidListEmpty);
            }
            return SteamIDs.IndexOf(SID) != -1 ? SID : SteamIDs[0];
        }

        /// <summary>
        /// Checks if specified path is a real Steam installation directory.
        /// </summary>
        /// <returns>Steam installation directory path.</returns>
        public static string TrySteamPath(string SteamPath)
        {
            if (Directory.Exists(SteamPath))
            {
                return SteamPath;
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }

        /// <summary>
        /// Gets Steam language from the specified internal ID.
        /// </summary>
        /// <param name="LangCode">Steam language ID.</param>
        /// <returns>Steam language name.</returns>
        public static string GetLanguageFromCode(int LangCode)
        {
            switch (LangCode)
            {
                case 1:
                    return "russian";
                default:
                    return "english";
            }
        }

        /// <summary>
        /// Gets Steam language internal ID from the specified public name.
        /// </summary>
        /// <param name="LangString">Steam language string.</param>
        /// <returns>Steam language internal ID.</returns>
        public static int GetCodeFromLanguage(string LangString)
        {
            switch (LangString)
            {
                case "russian":
                    return 1;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Gets full path to the main Steam config.vdf configuration file.
        /// </summary>
        /// <returns>Full path to config.vdf file.</returns>
        public string GetSteamConfig() => Path.Combine(FullSteamPath, "config", "config.vdf");

        /// <summary>
        /// Gets full path to the libraryfolders.vdf Steam configuration file.
        /// </summary>
        /// <returns>Full path to the libraryfolders.vdf file.</returns>
        public string GetLibraryFoldersConfig() => Path.Combine(FullSteamPath, "config", "libraryfolders.vdf");

        /// <summary>
        /// Gets full path to Steam localconfig.vdf configuration file.
        /// </summary>
        /// <returns>Full path to localconfig.vdf file.</returns>
        public List<String> GetSteamLocalConfig()
        {
            List<String> Result = new List<String>();
            foreach (string ID in SteamIDs)
            {
                Result.AddRange(FileManager.FindFiles(Path.Combine(UserDataPath, ID, "config"), "localconfig.vdf"));
            }
            return Result;
        }

        /// <summary>
        /// Gets list of available UserIDs.
        /// </summary>
        /// <returns>List of available UserIDs.</returns>
        private void GetUserIDs()
        {
            if (Directory.Exists(UserDataPath))
            {
                DirectoryInfo DInfo = new DirectoryInfo(UserDataPath);
                foreach (DirectoryInfo SubDir in DInfo.GetDirectories())
                {
                    if (SteamConv.ValidateUserID(SubDir.Name))
                    {
                        SteamIDs.Add(SubDir.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Removes Steam blob files (*.blob).
        /// </summary>
        public void CleanBlobsNow()
        {
            string FileName = Path.Combine(FullSteamPath, "AppUpdateStats.blob");
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }

            FileName = Path.Combine(FullSteamPath, "ClientRegistry.blob");
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
        }

        /// <summary>
        /// Removes Steam settings, stored in the Windows registry.
        /// </summary>
        /// <param name="LangCode">Steam language ID.</param>
        public void CleanRegistryNow(int LangCode)
        {
            PlatformWindows.CleanRegistrySettings(LangCode);
        }

        /// <summary>
        /// Reads and constructs a list of mount points from the specified
        /// configuration file.
        /// <param name="LibraryFoldersConfigFile">Full path to the libraryfolders.vdf file.</param>
        /// </summary>
        private List<String> ReadMountPointsFromFile(string LibraryFoldersConfigFile)
        {
            List<String> Result = new List<String>();
            using (StreamReader SteamConfig = new StreamReader(LibraryFoldersConfigFile, Encoding.UTF8))
            {
                string RdStr;
                while (SteamConfig.Peek() >= 0)
                {
                    RdStr = SteamConfig.ReadLine().Trim();
                    if (!String.IsNullOrWhiteSpace(RdStr))
                    {
                        if (RdStr.IndexOf("path", StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            RdStr = StringsManager.CleanString(RdStr, true, true);
                            RdStr = RdStr.Remove(0, RdStr.IndexOf(" ") + 1);
                            if (!String.IsNullOrWhiteSpace(RdStr) && Directory.Exists(RdStr)) { Result.Add(RdStr); }
                        }
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// Gets list of all additional mount points from main configuration file.
        /// </summary>
        private List<String> GetSteamMountPoints()
        {
            List<String> Result = new List<String> { FullSteamPath };
            try
            {
                string LibraryFoldersConfigFile = GetLibraryFoldersConfig();
                if (File.Exists(LibraryFoldersConfigFile))
                {
                    Result.AddRange(ReadMountPointsFromFile(LibraryFoldersConfigFile));
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExCoreStmManMountPointsFetchError);
            }
            return Result;
        }

        /// <summary>
        /// Gets list of all available game libraries.
        /// </summary>
        /// <param name="SteamAppsFolderName">Platform-dependent SteamApps directory name.</param>
        public List<String> FormatInstallDirs(string SteamAppsFolderName)
        {
            // Creating a new empty list...
            List<String> Result = new List<String>();

            // Getting additional mount points...
            List<String> MntPnts = GetSteamMountPoints();

            // Adding mandatory directories to paths...
            foreach (string MntPnt in MntPnts)
            {
                Result.Add(Path.Combine(MntPnt, SteamAppsFolderName, "common"));
            }

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Sets some private values of the SteamManager class.
        /// </summary>
        /// <param name="LastSteamID">Last used UserID.</param>
        private void SetValues(string LastSteamID)
        {
            SteamIDs = new List<String>();
            UserDataPath = Path.Combine(FullSteamPath, "userdata");
            GetUserIDs();
            SteamID = GetCurrentSteamID(LastSteamID);
        }

        /// <summary>
        /// SteamManager class constructor.
        /// </summary>
        /// <param name="LastSteamID">Last used UserID.</param>
        /// <param name="Platform">Instance of the CurrentPlatform class.</param>
        public SteamManager(string LastSteamID, CurrentPlatform Platform)
        {
            FullSteamPath = TrySteamPath(Platform.SteamInstallPath);
            SetValues(LastSteamID);
        }

        /// <summary>
        /// SteamManager class alternative constructor.
        /// </summary>
        /// <param name="SteamPath">Manually specified Steam installation path.</param>
        /// <param name="LastSteamID">Last used UserID.</param>
        public SteamManager(string SteamPath, string LastSteamID)
        {
            FullSteamPath = TrySteamPath(SteamPath);
            SetValues(LastSteamID);
        }
    }
}
