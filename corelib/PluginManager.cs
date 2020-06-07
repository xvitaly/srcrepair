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

using System.Collections.Generic;
using System.IO;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with external plugins.
    /// </summary>
    public sealed class PluginManager
    {
        /// <summary>
        /// Stores path to SRC Repair installation directory.
        /// </summary>
        private readonly string FullAppPath;

        /// <summary>
        /// Gets or sets collection of available plugins.
        /// </summary>
        public Dictionary<string, PluginTarget> AvailablePlugins { get; private set; }

        /// <summary>
        /// Find and add plugins to collection.
        /// TODO: automatically search for new plugins.
        /// </summary>
        private void FindPlugins()
        {
            AvailablePlugins.Add("kbhelper", new PluginTarget("System buttons disabler", Path.Combine(FullAppPath, "kbhelper.exe"), true));
        }

        /// <summary>
        /// PluginManager class constructor.
        /// </summary>
        /// <param name="CfFullAppPath">Path to SRC Repair installation directory.</param>
        public PluginManager(string CfFullAppPath)
        {
            AvailablePlugins = new Dictionary<string, PluginTarget>();
            FullAppPath = CfFullAppPath;
            FindPlugins();
        }
    }
}
