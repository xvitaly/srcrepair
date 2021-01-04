/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 *
 * Copyright (c) 2011 - 2020 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2020 EasyCoding Team.
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
