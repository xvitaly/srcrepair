/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with GNU/Linux specific functions.
    /// </summary>
    public class PlatformLinux : CurrentPlatform
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
            Process.Start(Properties.Resources.AppOpenHandlerLin, AddQuotesToPath(FileName));
        }

        /// <summary>
        /// Show the specified file in default file manager.
        /// </summary>
        /// <param name="FileName">Full path to file.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public override void OpenExplorer(string FileName)
        {
            Process.Start(Properties.Resources.AppOpenHandlerLin, string.Format("\"{0}\"", Path.GetDirectoryName(FileName)));
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
            return StartElevatedProcess(FileName, Arguments, "pkexec");
        }

        /// <summary>
        /// Restart current application with admin user rights.
        /// </summary>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public override void RestartApplicationAsAdmin()
        {
            StartElevatedProcess(Path.GetFileNameWithoutExtension(CurrentApp.AssemblyLocation));
            Environment.Exit(ReturnCodes.Success);
        }

        /// <summary>
        /// Get current operating system ID.
        /// </summary>
        public override OSType OS => OSType.Linux;

        /// <summary>
        /// Get platform-dependent Steam installation folder (directory) name.
        /// </summary>
        public override string SteamFolderName => Properties.Resources.SteamFolderNameLin;

        /// <summary>
        /// Get platform-dependent Steam launcher file name.
        /// </summary>
        public override string SteamBinaryName => Properties.Resources.SteamExecBinLin;

        /// <summary>
        /// Get platform-dependent SteamApps directory name.
        /// </summary>
        public override string SteamAppsFolderName => Properties.Resources.SteamAppsFolderNameLin;

        /// <summary>
        /// Get platform-dependent Steam process name.
        /// </summary>
        public override string SteamProcName => Properties.Resources.SteamProcNameLin;
    }
}
