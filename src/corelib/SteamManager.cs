/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
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
        /// Gets list of available UserIDs.
        /// </summary>
        public List<string> SteamIDs { get; private set; }

        /// <summary>
        /// Gets selected or default UserID.
        /// </summary>
        public string SteamID { get; set; }

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
                case 0: return "arabic"; case 1: return "bulgarian"; case 2: return "schinese";
                case 3: return "tchinese"; case 4: return "czech"; case 5: return "danish";
                case 6: return "dutch"; case 7: return "english"; case 8: return "finnish";
                case 9: return "french"; case 10: return "german"; case 11: return "greek";
                case 12: return "hungarian"; case 13: return "italian"; case 14: return "japanese";
                case 15: return "koreana"; case 16: return "norwegian"; case 17: return "polish";
                case 18: return "portuguese"; case 19: return "brazilian"; case 20: return "romanian";
                case 21: return "russian"; case 22: return "spanish"; case 23: return "latam";
                case 24: return "swedish"; case 25: return "thai"; case 26: return "turkish";
                case 27: return "ukrainian"; case 28: return "vietnamese"; default: return "english";
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
                case "arabic": return 0; case "bulgarian": return 1; case "schinese": return 2;
                case "tchinese": return 3; case "czech": return 4; case "danish": return 5;
                case "dutch": return 6; case "english": return 7; case "finnish": return 8;
                case "french": return 9; case "german": return 10; case "greek": return 11;
                case "hungarian": return 12; case "italian": return 13; case "japanese": return 14;
                case "koreana": return 15; case "norwegian": return 16; case "polish": return 17;
                case "portuguese": return 18; case "brazilian": return 19; case "romanian": return 20;
                case "russian": return 21; case "spanish": return 22; case "latam": return 23;
                case "swedish": return 24; case "thai": return 25; case "turkish": return 26;
                case "ukrainian": return 27; case "vietnamese": return 28; default: return 7;
            }
        }

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
                    if (!string.IsNullOrWhiteSpace(RdStr))
                    {
                        if (RdStr.IndexOf("path", StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            RdStr = StringsManager.CleanString(RdStr, true, true);
                            RdStr = RdStr.Remove(0, RdStr.IndexOf(" ", StringComparison.CurrentCulture) + 1);
                            if (!string.IsNullOrWhiteSpace(RdStr) && Directory.Exists(RdStr)) { Result.Add(RdStr); }
                        }
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
