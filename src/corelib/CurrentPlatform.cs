﻿/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with platform-dependent functions.
    /// </summary>
    public abstract class CurrentPlatform : IPlatform
    {
        /// <summary>
        /// Create a platform-dependent instance. Factory method.
        /// </summary>
        public static CurrentPlatform Create()
        {
            switch (GetRunningOS())
            {
                case OSType.Windows:
                    return new PlatformWindows();
                case OSType.Linux:
                    return new PlatformLinux();
                case OSType.MacOSX:
                    return new PlatformMac();
                default:
                    throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// Codes and IDs of available platforms.
        /// </summary>
        public enum OSType
        {
            Windows = 0,
            MacOSX = 1,
            Linux = 2
        }

        /// <summary>
        /// Get name and ID of running operating system.
        /// </summary>
        /// <returns>Platform ID.</returns>
        private static OSType GetRunningOS()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    return Directory.Exists("/Applications") ? OSType.MacOSX : OSType.Linux;
                case PlatformID.MacOSX:
                    return OSType.MacOSX;
                default: return OSType.Windows;
            }
        }

        /// <summary>
        /// Add quotes to the path.
        /// </summary>
        /// <param name="Source">Source string with path.</param>
        /// <returns>Quoted string with path.</returns>
        protected static string AddQuotesToPath(string Source)
        {
            return string.Format(Properties.Resources.AppOpenHandlerEscapeTemplate, Source);
        }

        /// <summary>
        /// Return whether automatic updates are supported on this platform.
        /// </summary>
        public virtual bool AutoUpdateSupported => false;

        /// <summary>
        /// Return whether advanced features are supported on this platform.
        /// </summary>
        public virtual bool AdvancedFeaturesSupported => false;

        /// <summary>
        /// Immediately shut down application and return exit code.
        /// </summary>
        /// <param name="ReturnCode">Exit code.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public virtual void Exit(int ReturnCode)
        {
            if (Application.MessageLoop)
            {
                Environment.ExitCode = ReturnCode;
                Application.Exit();
            }
            else
            {
                Environment.Exit(ReturnCode);
            }
        }

        /// <summary>
        /// Open the specified URL in default Web browser.
        /// </summary>
        /// <param name="URI">Full URL.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public virtual void OpenWebPage(string URI)
        {
            Process.Start(URI);
        }

        /// <summary>
        /// Restart current application with admin user rights.
        /// </summary>
        /// <param name="OS">Operating system type.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public virtual void RestartApplicationAsAdmin()
        {
            StartElevatedProcess(CurrentApp.AssemblyLocation);
            Environment.Exit(ReturnCodes.Success);
        }

        /// <summary>
        /// Starts an external helper application.
        /// </summary>
        /// <param name="FileName">Full path to helper application.</param>
        /// <param name="IsElevated">Run with administrator privileges.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public virtual void StartExternalHelper(string FileName, bool IsElevated)
        {
            if (File.Exists(FileName))
            {
                if (IsElevated) { StartElevatedProcess(FileName); } else { StartRegularProcess(FileName); }
            }
            else
            {
                throw new FileNotFoundException(DebugStrings.AppDbgExCoreHelperNxExists, FileName);
            }
        }

        /// <summary>
        /// Start the required application as the current user.
        /// </summary>
        /// <param name="FileName">Full path to the executable.</param>
        /// <returns>PID of the newly created process.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public virtual int StartRegularProcess(string FileName)
        {
            return Process.Start(FileName).Id;
        }

        /// <summary>
        /// Start the required application as the current user with the specified
        /// command-line arguments.
        /// </summary>
        /// <param name="FileName">Full path to the executable.</param>
        /// <param name="Arguments">Command-line arguments.</param>
        /// <returns>PID of the newly created process.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public virtual int StartRegularProcess(string FileName, string Arguments)
        {
            ProcessStartInfo ST = new ProcessStartInfo
            {
                FileName = FileName,
                Arguments = Arguments
            };
            return Process.Start(ST).Id;
        }

        /// <summary>
        /// Start the required application as an administrator.
        /// </summary>
        /// <param name="FileName">Full path to the executable.</param>
        /// <returns>PID of the newly created process.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public virtual int StartElevatedProcess(string FileName)
        {
            return StartElevatedProcess(FileName, string.Empty);
        }

        /// <summary>
        /// Start the required application as an administrator with the specified
        /// command-line arguments and external helper tool.
        /// </summary>
        /// <param name="FileName">Full path to the executable.</param>
        /// <param name="Arguments">Command-line arguments.</param>
        /// <param name="ExternalHelper">External helper application for elevating permissions.</param>
        /// <returns>PID of the newly created process.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        protected virtual int StartElevatedProcess(string FileName, string Arguments, string ExternalHelper)
        {
            // Setting advanced properties...
            ProcessStartInfo ST = new ProcessStartInfo
            {
                FileName = FileName,
                Arguments = Arguments,
                Verb = ExternalHelper,
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = true
            };

            // Starting process...
            Process NewProcess = Process.Start(ST);

            // Returning PID of created process...
            return NewProcess.Id;
        }

        /// <summary>
        /// Backup Steam settings, stored in the registry.
        /// </summary>
        /// <param name="DestDir">Directory for saving backups.</param>
        public virtual void BackUpRegistrySettings(string DestDir)
        {
            throw new PlatformNotSupportedException(DebugStrings.AppDbgCoreRegNotSupported);
        }

        /// <summary>
        /// Remove Steam settings, stored in the registry.
        /// </summary>
        /// <param name="LangName">Steam language.</param>
        public virtual void CleanRegistrySettings(string LangName)
        {
            throw new PlatformNotSupportedException(DebugStrings.AppDbgCoreRegNotSupported);
        }

        /// <summary>
        /// Restore settings stored in the registry file.
        /// </summary>
        /// <param name="FileName">Full path to the registry file.</param>
        public virtual void RestoreRegistrySettings(string FileName)
        {
            throw new PlatformNotSupportedException(DebugStrings.AppDbgCoreRegNotSupported);
        }

        /// <summary>
        /// Start automatic service repair depending on the running platform.
        /// </summary>
        /// <param name="FullBinPath">Full path to the Steam binaries directory.</param>
        public virtual void StartServiceRepair(string FullBinPath)
        {
            throw new PlatformNotSupportedException(DebugStrings.AppDbgCoreServiceNotSupported);
        }

        /// <summary>
        /// Get current operating system ID.
        /// </summary>
        public abstract OSType OS { get; }

        /// <summary>
        /// Get current operating system friendly name for the HTTP_USER_AGENT header.
        /// </summary>
        public virtual string OSFriendlyName => OS.ToString();

        /// <summary>
        /// Get platform-dependent Steam installation folder (directory) name.
        /// </summary>
        public abstract string SteamFolderName { get; }

        /// <summary>
        /// Get platform-dependent Steam launcher file name.
        /// </summary>
        public abstract string SteamBinaryName { get; }

        /// <summary>
        /// Get platform-dependent SteamApps directory name.
        /// </summary>
        public abstract string SteamAppsFolderName { get; }

        /// <summary>
        /// Get platform-dependent Steam process name.
        /// </summary>
        public abstract string SteamProcName { get; }

        /// <summary>
        /// Return platform-dependent location of the Hosts file.
        /// </summary>
        public virtual string HostsFileLocation => "/etc";

        /// <summary>
        /// Get platform-dependent Steam installation path.
        /// </summary>
        public virtual string SteamInstallPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), SteamFolderName);

        /// <summary>
        /// Get platform-dependent Steam language.
        /// </summary>
        public virtual string SteamLanguage => "english";

        /// <summary>
        /// Return platform-dependent path to Hosts file.
        /// </summary>
        public virtual string HostsFileFullPath => Path.Combine(HostsFileLocation, "hosts");

        /// <summary>
        /// Open the specified text file in default (or overrided in application's
        /// settings (only on Windows platform)) text editor.
        /// </summary>
        /// <param name="FileName">Full path to text file.</param>
        /// <param name="EditorBin">External text editor (Windows only).</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public abstract void OpenTextEditor(string FileName, string EditorBin);

        /// <summary>
        /// Show the specified file in default file manager.
        /// </summary>
        /// <param name="FileName">Full path to file.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public abstract void OpenExplorer(string FileName);

        /// <summary>
        /// Start the required application as an administrator with the specified
        /// command-line arguments.
        /// </summary>
        /// <param name="FileName">Full path to the executable.</param>
        /// <param name="Arguments">Command-line arguments.</param>
        /// <returns>PID of the newly created process.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public abstract int StartElevatedProcess(string FileName, string Arguments);
    }
}
