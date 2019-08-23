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
using System.IO;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with collection of custom report generators.
    /// </summary>
    public sealed class ReportManager
    {
        /// <summary>
        /// Gets or sets collection of custom report generators.
        /// </summary>
        public List<ReportTarget> ReportTargets { get; private set; }

        /// <summary>
        /// Stores current UnixTime as string.
        /// </summary>
        private readonly string CurrentUnixTime;

        /// <summary>
        /// Stores full path to temporary working directory.
        /// </summary>
        private readonly string TempDirectory;

        /// <summary>
        /// Adds custom report generators to collection.
        /// </summary>
        private void SetTargets()
        {
            ReportTargets.Add(new ReportTarget("msinfo32.exe", "/report \"{0}\"", "report_{0}.txt"));
            ReportTargets.Add(new ReportTarget("dxdiag.exe", "/t {0}", "dxdiag_{0}.log"));
            ReportTargets.Add(new ReportTarget("cmd.exe", "/C ping steampowered.com > \"{0}\"", "ping_{0}.log"));
            ReportTargets.Add(new ReportTarget("cmd.exe", "/C tracert steampowered.com > \"{0}\"", "traceroute_{0}.log"));
            ReportTargets.Add(new ReportTarget("cmd.exe", "/C ipconfig /all > \"{0}\"", "ipconfig_{0}.log"));
            ReportTargets.Add(new ReportTarget("cmd.exe", "/C netstat -a > \"{0}\"", "netstat_{0}.log"));
            ReportTargets.Add(new ReportTarget("cmd.exe", "/C route print > \"{0}\"", "routing_{0}.log"));
            ReportTargets.Add(new ReportTarget("cmd.exe", "/C net user > \"{0}\"", "users_{0}.log"));
        }

        /// <summary>
        /// ReportManager class constructor.
        /// </summary>
        public ReportManager()
        {
            ReportTargets = new List<ReportTarget>();
            TempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            CurrentUnixTime = FileManager.DateTime2Unix(DateTime.Now);
            SetTargets();
        }
    }
}
