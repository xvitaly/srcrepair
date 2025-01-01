/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with Microsoft Windows specific functions.
    /// </summary>
    public class PlatformWindows : CurrentPlatform
    {
        /// <summary>
        /// Open the specified text file in default (or overrided in application's
        /// settings (only on Windows platform)) text editor.
        /// </summary>
        /// <param name="FileName">Full path to text file.</param>
        /// <param name="EditorBin">External text editor (Windows only).</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public override void OpenTextEditor(string FileName, string EditorBin)
        {
            Process.Start(EditorBin, AddQuotesToPath(FileName));
        }

        /// <summary>
        /// Show the specified file in default file manager.
        /// </summary>
        /// <param name="FileName">Full path to file.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public override void OpenExplorer(string FileName)
        {
            Process.Start(Properties.Resources.ShBinWin, string.Format("{0} \"{1}\"", Properties.Resources.ShParamWin, FileName));
        }

        /// <summary>
        /// Start the required application as an administrator with the specified
        /// command-line arguments.
        /// </summary>
        /// <param name="FileName">Full path to the executable.</param>
        /// <param name="Arguments">Command-line arguments.</param>
        /// <returns>PID of the newly created process.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public override int StartElevatedProcess(string FileName, string Arguments)
        {
            return StartElevatedProcess(FileName, Arguments, "runas");
        }

        /// <summary>
        /// Get current operating system ID.
        /// </summary>
        public override OSType OS => OSType.Windows;

        /// <summary>
        /// Get current operating system friendly name for the HTTP_USER_AGENT header.
        /// </summary>
        public override string OSFriendlyName => string.Format(Properties.Resources.OSFriendlyNameWin, OS);

        /// <summary>
        /// Return whether automatic updates are supported on this platform.
        /// </summary>
        public override bool AutoUpdateSupported => true;

        /// <summary>
        /// Return whether advanced features are supported on this platform.
        /// </summary>
        public override bool AdvancedFeaturesSupported => true;

        /// <summary>
        /// Get platform-dependent Steam installation folder (directory) name.
        /// </summary>
        public override string SteamFolderName => Properties.Resources.SteamFolderNameWin;

        /// <summary>
        /// Get platform-dependent Steam launcher file name.
        /// </summary>
        public override string SteamBinaryName => Properties.Resources.SteamExecBinWin;

        /// <summary>
        /// Get platform-dependent SteamApps directory name.
        /// </summary>
        public override string SteamAppsFolderName => Properties.Resources.SteamAppsFolderNameWin;

        /// <summary>
        /// Get platform-dependent Steam process name.
        /// </summary>
        public override string SteamProcName => Properties.Resources.SteamProcNameWin;

        /// <summary>
        /// Backup Steam settings, stored in the Windows registry.
        /// </summary>
        /// <param name="DestDir">Directory for saving backups.</param>
        public override void BackUpRegistrySettings(string DestDir)
        {
            ProcessManager.StartProcessAndWait(Properties.Resources.RegExecutable, string.Format(Properties.Resources.RegExportCmdLine, @"HKEY_CURRENT_USER\Software\Valve", Path.Combine(DestDir, string.Format(Properties.Resources.RegOutFilePattern, "Steam_BackUp", FileManager.DateTime2Unix(DateTime.Now)))));
        }

        /// <summary>
        /// Remove Steam settings, stored in the Windows registry.
        /// </summary>
        /// <param name="LangName">Steam language.</param>
        public override void CleanRegistrySettings(string LangName)
        {
            // Removing key HKEY_CURRENT_USER\Software\Valve recursive...
            Registry.CurrentUser.DeleteSubKeyTree(@"Software\Valve", false);

            // Creating a new registry key HKEY_CURRENT_USER\Software\Valve\Steam...
            using (RegistryKey RegLangKey = Registry.CurrentUser.CreateSubKey(@"Software\Valve\Steam"))
            {
                // Saving Steam language name...
                RegLangKey?.SetValue("language", LangName);
            }
        }

        /// <summary>
        /// Restore settings stored in the registry file.
        /// </summary>
        /// <param name="FileName">Full path to the registry file.</param>
        public override void RestoreRegistrySettings(string FileName)
        {
            ProcessManager.StartProcessAndWait(Properties.Resources.RegExecutable, string.Format(Properties.Resources.RegImportCmdLine, FileName));
        }

        /// <summary>
        /// Start automatic service repair depending on the running platform.
        /// </summary>
        /// <param name="FullBinPath">Full path to the Steam binaries directory.</param>
        public override void StartServiceRepair(string FullBinPath)
        {
            string ServiceBinary = Path.Combine(FullBinPath, "steamservice.exe");
            if (!File.Exists(ServiceBinary)) { throw new FileNotFoundException(DebugStrings.AppDbgCoreServiceBinaryNotFound, ServiceBinary); }
            ProcessManager.StartProcessAndWait(ServiceBinary, "/repair");
        }

        /// <summary>
        /// Get platform-dependent location of the Hosts file.
        /// </summary>
        /// <returns>Platform-dependent location of the Hosts file.</returns>
        private string GetHostsFileLocation()
        {
            string HostsDirectory;

            try
            {
                using (RegistryKey ResKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", false))
                {
                    HostsDirectory = (string)ResKey.GetValue("DataBasePath");
                }
            }
            catch
            {
                HostsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "drivers", "etc");
            }

            return HostsDirectory;
        }

        /// <summary>
        /// Get platform-dependent Steam installation directory from the Windows
        /// registry.
        /// </summary>
        /// <returns>Steam installation directory from the Windows registry.</returns>
        private string GetSteamInstallPath()
        {
            // Creating an empty string for storing result...
            string ResString = string.Empty;

            // Opening registry key as read only...
            using (RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam", false))
            {
                // Checking if registry key exists and available for reading...
                if (ResKey != null)
                {
                    // Getting SteamPath value from previously opened key...
                    object ResObj = ResKey.GetValue("SteamPath");

                    // Checking if value exists...
                    if (ResObj != null)
                    {
                        // Extracting result...
                        ResString = Path.GetFullPath(Convert.ToString(ResObj));
                    }
                    else
                    {
                        // Does not exists. Throwing exception...
                        throw new SteamPathNotFoundException(DebugStrings.AppDbgExCoreStmManNoInstallPathDetected);
                    }
                }
            }

            // Returning result...
            return ResString;
        }

        /// <summary>
        /// Get platform-dependent Steam language name from the Windows registry.
        /// </summary>
        /// <returns>Steam language name from the Windows registry.</returns>
        private string GetSteamLanguage()
        {
            // Creating an empty string for storing result...
            string Result = string.Empty;

            // Opening registry key as read only...
            using (RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam", false))
            {
                // Checking if registry key exists and available for reading...
                if (ResKey != null)
                {
                    // Getting SteamPath value from previously opened key...
                    object ResObj = ResKey.GetValue("Language");

                    // Checking if value exists...
                    if (ResObj != null)
                    {
                        // Extracting result...
                        Result = Convert.ToString(ResObj);
                    }
                    else
                    {
                        // Does not exists. Throwing exception...
                        throw new SteamLangNameNotFoundException(DebugStrings.AppDbgExCoreStmManNoLangNameDetected);
                    }
                }
            }

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Return platform-dependent location of the Hosts file.
        /// </summary>
        public override string HostsFileLocation => GetHostsFileLocation();

        /// <summary>
        /// Return platform-dependent Steam installation directory from the Windows
        /// registry.
        /// </summary>
        public override string SteamInstallPath => GetSteamInstallPath();

        /// <summary>
        /// Return platform-dependent Steam language name from the Windows registry.
        /// </summary>
        public override string SteamLanguage => GetSteamLanguage();
    }
}
