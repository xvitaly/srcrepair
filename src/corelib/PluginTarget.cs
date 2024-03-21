/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System.IO;
using System.Security.Permissions;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with a single external plugin.
    /// </summary>
    public sealed class PluginTarget
    {
        /// <summary>
        /// Gets or sets plugin's user-friendly name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets fully-qualified path to plugin's executable.
        /// </summary>
        private string Executable { get; set; }

        /// <summary>
        /// Gets or sets if elevation is required to run this plugin.
        /// </summary>
        private bool ElevationRequired { get; set; }

        /// <summary>
        /// Checks if the plugin is installed and available to launch.
        /// </summary>
        public bool Installed => File.Exists(Executable);

        /// <summary>
        /// Run plugin if installed.
        /// </summary>
        /// <param name="Platform">An instance of the CurrentPlatform class for working
        /// with platform-dependent functions.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public void Run(CurrentPlatform Platform)
        {
            if (!Installed) { throw new FileNotFoundException(DebugStrings.AppDbgExCorePluginNotFound, Executable); }
            Platform.StartExternalHelper(Executable, ElevationRequired);
        }

        /// <summary>
        /// PluginTarget class constructor.
        /// </summary>
        /// <param name="CfName">Plugin's user-friendly name.</param>
        /// <param name="CfExecutable">Full path to plugin's executable.</param>
        /// <param name="CfElevationRequired">If elevation is required to run this plugin..</param>
        public PluginTarget(string CfName, string CfExecutable, bool CfElevationRequired)
        {
            Name = CfName;
            Executable = CfExecutable;
            ElevationRequired = CfElevationRequired;
        }
    }
}
