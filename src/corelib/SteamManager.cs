/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// Gets or sets full path to Steam binaries directory.
        /// </summary>
        public string FullBinPath { get; private set; }

        /// <summary>
        /// Gets or sets full path to Steam configuration files directory.
        /// </summary>
        public string FullConfigsPath { get; private set; }

        /// <summary>
        /// Gets or sets full path to Steam userdata directory.
        /// </summary>
        public string FullUserDataPath { get; private set; }

        /// <summary>
        /// Gets or sets full path to Steam crash dumps directory.
        /// </summary>
        public string FullDumpsPath { get; private set; }

        /// <summary>
        /// Gets or sets full path to Steam logs directory.
        /// </summary>
        public string FullLogsPath { get; private set; }

        /// <summary>
        /// Gets or sets the list of available UserIDs.
        /// </summary>
        public List<string> SteamIDs { get; private set; }

        /// <summary>
        /// Gets or sets selected or default UserID.
        /// </summary>
        public string SteamID { get; private set; }

        /// <summary>
        /// Gets or sets full path to the main Steam config.vdf configuration file.
        /// </summary>
        /// <returns>Full path to config.vdf file.</returns>
        public string SteamConfigFile { get; private set; }

        /// <summary>
        /// Gets or sets full path to the libraryfolders.vdf Steam configuration file.
        /// </summary>
        /// <returns>Full path to the libraryfolders.vdf file.</returns>
        public string LibraryFoldersConfigFile { get; private set; }

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
        /// Gets full path to Steam localconfig.vdf configuration file.
        /// </summary>
        /// <returns>Full path to localconfig.vdf file.</returns>
        public List<string> GetSteamLocalConfig()
        {
            List<string> Result = new List<string>();
            foreach (string ID in SteamIDs)
            {
                Result.AddRange(FileManager.FindFiles(Path.Combine(FullUserDataPath, ID, "config"), "localconfig.vdf"));
            }
            return Result;
        }

        /// <summary>
        /// Gets list of available UserIDs.
        /// </summary>
        /// <returns>List of available UserIDs.</returns>
        private void GetUserIDs()
        {
            if (Directory.Exists(FullUserDataPath))
            {
                DirectoryInfo DInfo = new DirectoryInfo(FullUserDataPath);
                DirectoryInfo[] SubDirs = DInfo.GetDirectories();
                foreach (DirectoryInfo SubDir in SubDirs.Where(e => SteamConv.ValidateUserID(e.Name)))
                {
                    SteamIDs.Add(SubDir.Name);
                }
            }
        }

        /// <summary>
        /// Creates a backup copy of Steam settings, stored in the Windows registry.
        /// </summary>
        /// <param name="DestDir">Directory for saving backups.</param>
        public void BackUpRegistryNow(string DestDir)
        {
            PlatformWindows.BackUpRegistrySettings(DestDir);
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
        /// <param name="LangCode">Steam language.</param>
        public void CleanRegistryNow(string LangName)
        {
            PlatformWindows.CleanRegistrySettings(LangName);
        }

        /// <summary>
        /// Reads and constructs a list of mount points from the specified
        /// configuration file.
        /// </summary>
        private List<string> ReadMountPointsFromFile()
        {
            List<string> Result = new List<string>();
            using (StreamReader SteamConfig = new StreamReader(LibraryFoldersConfigFile, Encoding.UTF8))
            {
                string RdStr;
                while (SteamConfig.Peek() >= 0)
                {
                    RdStr = SteamConfig.ReadLine().Trim();
                    if (!string.IsNullOrWhiteSpace(RdStr) && RdStr.IndexOf("path", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        RdStr = StringsManager.CleanString(RdStr, true, true, false);
                        RdStr = RdStr.Remove(0, RdStr.IndexOf(" ", StringComparison.CurrentCulture) + 1);
                        if (!string.IsNullOrWhiteSpace(RdStr) && Directory.Exists(RdStr)) { Result.Add(RdStr); }
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// Gets list of all additional mount points from main configuration file.
        /// </summary>
        private List<string> GetSteamMountPoints()
        {
            List<string> Result = new List<string> { FullSteamPath };
            try
            {
                if (File.Exists(LibraryFoldersConfigFile))
                {
                    Result.AddRange(ReadMountPointsFromFile());
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
        public List<string> FormatInstallDirs(string SteamAppsFolderName)
        {
            // Creating a new empty list...
            List<string> Result = new List<string>();

            // Getting additional mount points...
            List<string> MntPnts = GetSteamMountPoints();

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
            SteamIDs = new List<string>();
            FullBinPath = Path.Combine(FullSteamPath, "bin");
            FullConfigsPath = Path.Combine(FullSteamPath, "config");
            FullDumpsPath = Path.Combine(FullSteamPath, "dumps");
            FullLogsPath = Path.Combine(FullSteamPath, "logs");
            FullUserDataPath = Path.Combine(FullSteamPath, "userdata");
            SteamConfigFile = Path.Combine(FullConfigsPath, "config.vdf");
            LibraryFoldersConfigFile = Path.Combine(FullConfigsPath, "libraryfolders.vdf");
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
