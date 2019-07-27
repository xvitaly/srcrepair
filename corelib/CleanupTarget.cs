/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2019 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2019 EasyCoding Team.
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

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with a single cleanup target.
    /// </summary>
    public sealed class CleanupTarget
    {
        /// <summary>
        /// Get or sets cleanup target friendly name.
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Gets or sets list of directories for cleanup.
        /// </summary>
        public List<String> Directories { get; private set; }

        /// <summary>
        /// Adds custom directory for cleanup.
        /// </summary>
        /// <param name="Target">Custom directory to be added to list.</param>
        public void AddTarget(string Target)
        {
            Directories.Add(Target);
        }

        /// <summary>
        /// Adds collection of custom directories for cleanup.
        /// </summary>
        /// <param name="Targets">Collection of custom directories to be added to list.</param>
        public void AddTargets(List<String> Targets)
        {
            Directories.AddRange(Targets);
        }

        /// <summary>
        /// CleanupTarget class constructor.
        /// </summary>
        /// <param name="CtName">Cleanup target friendly name.</param>
        /// <param name="CtDirectories">List of directories for cleanup.</param>
        public CleanupTarget(string CtName, List<String> CtDirectories)
        {
            Name = CtName;
            Directories = CtDirectories;
        }
    }
}
