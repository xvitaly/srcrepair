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
        /// Get current operating system friendly name.
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
        /// Get platform-dependent Steam process name.
        /// </summary>
        string SteamProcName { get; }

        /// <summary>
        /// Get platform-dependent suffix for HTTP_USER_AGENT header.
        /// </summary>
        string UASuffix { get; }

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
        /// Start the required application from administrator.
        /// </summary>
        /// <param name="FileName">Full path to the executable.</param>
        /// <returns>PID of the newly created process.</returns>
        int StartElevatedProcess(string FileName);

        /// <summary>
        /// Start an external helper application.
        /// </summary>
        /// <param name="FileName">Full path to helper application.</param>
        /// <param name="Elevated">Run with administrator privileges.</param>
        void StartExternalHelper(string FileName, bool Elevated = false);
    }
}
