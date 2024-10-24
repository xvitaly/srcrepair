/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

namespace srcrepair.core
{
    /// <summary>
    /// Interface for working with platform-dependent functions.
    /// </summary>
    public interface IPlatform
    {
        /// <summary>
        /// Return platform-dependent path to Hosts file.
        /// </summary>
        string HostsFileFullPath { get; }

        /// <summary>
        /// Return platform-dependent location of the Hosts file.
        /// </summary>
        string HostsFileLocation { get; }

        /// <summary>
        /// Codes and IDs of available platforms.
        /// </summary>
        CurrentPlatform.OSType OS { get; }

        /// <summary>
        /// Return whether automatic updates are supported on this platform.
        /// </summary>
        bool AutoUpdateSupported { get; }

        /// <summary>
        /// Return whether advanced features are supported on this platform.
        /// </summary>
        bool AdvancedFeaturesSupported { get; }

        /// <summary>
        /// Immediately shut down application and return exit code.
        /// </summary>
        /// <param name="ReturnCode">Exit code.</param>
        void Exit(int ReturnCode);

        /// <summary>
        /// Get information about operating system architecture for the HTTP_USER_AGENT header.
        /// </summary>
        string OSArchitecture { get; }

        /// <summary>
        /// Get current operating system friendly name for the HTTP_USER_AGENT header.
        /// </summary>
        string OSFriendlyName { get; }

        /// <summary>
        /// Get platform-dependent SteamApps directory name.
        /// </summary>
        string SteamAppsFolderName { get; }

        /// <summary>
        /// Get platform-dependent Steam launcher file name.
        /// </summary>
        string SteamBinaryName { get; }

        /// <summary>
        /// Get platform-dependent Steam installation folder (directory) name.
        /// </summary>
        string SteamFolderName { get; }

        /// <summary>
        /// Get platform-dependent Steam installation path.
        /// </summary>
        string SteamInstallPath { get; }

        /// <summary>
        /// Get platform-dependent Steam language.
        /// </summary>
        string SteamLanguage { get; }

        /// <summary>
        /// Get platform-dependent Steam process name.
        /// </summary>
        string SteamProcName { get; }

        /// <summary>
        /// Show the specified file in default file manager.
        /// </summary>
        /// <param name="FileName">Full path to file.</param>
        void OpenExplorer(string FileName);

        /// <summary>
        /// Open the specified text file in default (or overrided in application's
        /// settings (only on Windows platform)) text editor.
        /// </summary>
        /// <param name="FileName">Full path to text file.</param>
        /// <param name="EditorBin">External text editor (Windows only).</param>
        void OpenTextEditor(string FileName, string EditorBin);

        /// <summary>
        /// Open the specified URL in default Web browser.
        /// </summary>
        /// <param name="URI">Full URL.</param>
        void OpenWebPage(string URI);

        /// <summary>
        /// Restart current application with admin user rights.
        /// </summary>
        void RestartApplicationAsAdmin();

        /// <summary>
        /// Start the required application as an administrator with the specified
        /// command-line arguments.
        /// </summary>
        /// <param name="FileName">Full path to the executable.</param>
        /// <param name="Arguments">Command-line arguments.</param>
        /// <returns>PID of the newly created process.</returns>
        int StartElevatedProcess(string FileName, string Arguments);

        /// <summary>
        /// Start the required application from an administrator.
        /// </summary>
        /// <param name="FileName">Full path to the executable.</param>
        /// <returns>PID of the newly created process.</returns>
        int StartElevatedProcess(string FileName);

        /// <summary>
        /// Start an external helper application.
        /// </summary>
        /// <param name="FileName">Full path to helper application.</param>
        /// <param name="IsElevated">Run with administrator privileges.</param>
        void StartExternalHelper(string FileName, bool IsElevated);

        /// <summary>
        /// Start the required application as the current user with the specified
        /// command-line arguments.
        /// </summary>
        /// <param name="FileName">Full path to the executable.</param>
        /// <param name="Arguments">Command-line arguments.</param>
        /// <returns>PID of the newly created process.</returns>
        int StartRegularProcess(string FileName, string Arguments);

        /// <summary>
        /// Start the required application as the current user.
        /// </summary>
        /// <param name="FileName">Full path to the executable.</param>
        /// <returns>PID of the newly created process.</returns>
        int StartRegularProcess(string FileName);

        /// <summary>
        /// Backup Steam settings, stored in the registry.
        /// </summary>
        /// <param name="DestDir">Directory for saving backups.</param>
        void BackUpRegistrySettings(string DestDir);

        /// <summary>
        /// Remove Steam settings, stored in the registry.
        /// </summary>
        /// <param name="LangName">Steam language.</param>
        void CleanRegistrySettings(string LangName);

        /// <summary>
        /// Restore settings stored in the registry file.
        /// </summary>
        /// <param name="FileName">Full path to the registry file.</param>
        void RestoreRegistrySettings(string FileName);

        /// <summary>
        /// Start automatic service repair depending on the running platform.
        /// </summary>
        /// <param name="FullBinPath">Full path to the Steam binaries directory.</param>
        void StartServiceRepair(string FullBinPath);
    }
}
