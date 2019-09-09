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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with loaded shared libraries.
    /// </summary>
    public static class LibraryManager
    {
        /// <summary>
        /// Checks if required ABI version is equal to current library ABI version.
        /// </summary>
        /// <param name="RequiredVersion">Required ABI version.</param>
        /// <returns>Check results.</returns>
        public static bool CheckABIVersion(string RequiredVersion)
        {
            return RequiredVersion == Properties.Resources.LibABIVersion;
        }

        /// <summary>
        /// Checks if required library version is equal to current library version.
        /// </summary>
        /// <param name="RequiredVersion">Required ABI version.</param>
        /// <returns>Check results.</returns>
        public static bool CheckLibraryVersion()
        {
            return Assembly.GetCallingAssembly().GetName().Version == Assembly.GetExecutingAssembly().GetName().Version;
        }

        /// <summary>
        /// Checks if required library version is equal to current library version.
        /// </summary>
        /// <param name="RequiredVersion">Required library version.</param>
        /// <returns>Check results.</returns>
        public static bool CheckLibraryVersion(string RequiredVersion)
        {
            return Assembly.GetCallingAssembly().GetName().Version == new Version(RequiredVersion);
        }
    }
}
