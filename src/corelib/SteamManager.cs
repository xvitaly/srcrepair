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
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Win32;
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
            string Result;
            switch (LangCode)
            {
                case 1:
                    Result = "russian";
                    break;
                default:
                    Result = "english";
                    break;
            }
            return Result;
        }

        /// <summary>
        /// Gets full path to the main Steam config.vdf configuration file.
        /// </summary>
        /// <returns>Full path to config.vdf file.</returns>
        public string GetSteamConfig() => Path.Combine(FullSteamPath, "config", "config.vdf");

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
        /// Removes Steam settings, stored in Windows registry.
        /// </summary>
        /// <param name="LangCode">Steam language ID.</param>
        public void CleanRegistryNow(int LangCode)
        {
            // Removing key HKEY_LOCAL_MACHINE\Software\Valve recursive if we have admin rights...
            if (ProcessManager.IsCurrentUserAdmin())
            {
                Registry.LocalMachine.DeleteSubKeyTree(Path.Combine("Software", "Valve"), false);
            }

            // Removing key HKEY_CURRENT_USER\Software\Valve recursive...
            Registry.CurrentUser.DeleteSubKeyTree(Path.Combine("Software", "Valve"), false);

            // Creating a new registry key HKEY_CURRENT_USER\Software\Valve\Steam...
            using (RegistryKey RegLangKey = Registry.CurrentUser.CreateSubKey(Path.Combine("Software", "Valve", "Steam")))
            {
                // Saving Steam language name...
                if (RegLangKey != null)
                {
                    RegLangKey.SetValue("language", GetLanguageFromCode(LangCode));
                }
            }
        }

        /// <summary>
        /// Gets list of all additional mount points from main configuration file.
        /// </summary>
        private List<String> GetSteamMountPoints()
        {
            // Creating a new list for storing result...
            List<String> Result = new List<String> { FullSteamPath };

            // Trying to get additional mount points by reading them from config...
            try
            {
                // Opening file stream...
                using (StreamReader SteamConfig = new StreamReader(GetSteamConfig(), Encoding.Default))
                {
                    // Creating buffer variable...
                    string RdStr;

                    // Reading up to the end...
                    while (SteamConfig.Peek() >= 0)
                    {
                        // Reading row and cleaning it up...
                        RdStr = SteamConfig.ReadLine().Trim();

                        // Checking if string is not empty...
                        if (!(String.IsNullOrWhiteSpace(RdStr)))
                        {
                            // Finding additional game libraries...
                            if (RdStr.IndexOf("BaseInstallFolder", StringComparison.CurrentCultureIgnoreCase) != -1)
                            {
                                RdStr = StringsManager.CleanString(RdStr, true, true);
                                RdStr = RdStr.Remove(0, RdStr.IndexOf(" ") + 1);
                                if (!(String.IsNullOrWhiteSpace(RdStr))) { Result.Add(RdStr); }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExCoreStmManMountPointsFetchError);
            }

            // Returning final list...
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
