/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
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
            Process.Start(Properties.Resources.ShBinWin, String.Format("{0} \"{1}\"", Properties.Resources.ShParamWin, FileName));
        }

        /// <summary>
        /// Start the required application from administrator.
        /// </summary>
        /// <param name="FileName">Full path to the executable.</param>
        /// <returns>PID of the newly created process.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public override int StartElevatedProcess(string FileName)
        {
            // Setting advanced properties...
            ProcessStartInfo ST = new ProcessStartInfo
            {
                FileName = FileName,
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = true
            };

            // Starting process...
            Process NewProcess = Process.Start(ST);

            // Returning PID of created process...
            return NewProcess.Id;
        }

        /// <summary>
        /// Get platform-dependent suffix for HTTP_USER_AGENT header.
        /// </summary>
        public override string UASuffix => Properties.Resources.AppUASuffixWin;

        /// <summary>
        /// Get current operating system ID.
        /// </summary>
        public override OSType OS => OSType.Windows;

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
        /// Removes Steam settings, stored in the Windows registry.
        /// </summary>
        /// <param name="LangCode">Steam language ID.</param>
        public static void CleanRegistrySettings(int LangCode)
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
                    RegLangKey.SetValue("language", SteamManager.GetLanguageFromCode(LangCode));
                }
            }
        }

        /// <summary>
        /// Return platform-dependent location of the Hosts file.
        /// </summary>
        public override string HostsFileLocation
        {
            get
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
        }

        /// <summary>
        /// Return Steam installation directory from the Windows registry.
        /// </summary>
        public override string SteamInstallPath
        {
            get
            {
                // Creating an empty string for storing result...
                string ResString = String.Empty;

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
        }

        /// <summary>
        /// Get platform-dependent Steam language name from the Windows registry.
        /// </summary>
        public override string SteamLanguage
        {
            get
            {
                // Creating an empty string for storing result...
                string Result = String.Empty;

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
        }
    }
}
