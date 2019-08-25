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
        public string TempDirectory { get; private set; }

        /// <summary>
        /// Gets or sets full path to directory for saving generated reports.
        /// </summary>
        public string ReportsDirectory { get; private set; }

        /// <summary>
        /// Gets report file name with full path.
        /// </summary>
        public string ReportArchiveName => Path.Combine(ReportsDirectory, String.Format("report_{0}.zip", CurrentUnixTime));

        /// <summary>
        /// Adds custom report generators to collection.
        /// </summary>
        private void SetTargets()
        {
            ReportTargets.Add(new ReportTarget(ReportStrings.MsInfoExe, ReportStrings.MsInfoParams, Path.Combine(TempDirectory, String.Format(ReportStrings.MsInfoOutput, CurrentUnixTime)), ReportStrings.MsInfoArchDirectory, true));
            ReportTargets.Add(new ReportTarget(ReportStrings.DxDiagExe, ReportStrings.DxDiagParams, Path.Combine(TempDirectory, String.Format(ReportStrings.DxDiagOutput, CurrentUnixTime)), ReportStrings.GenericArchDirectory));
            ReportTargets.Add(new ReportTarget(ReportStrings.CmdIntExe, ReportStrings.PingScParams, Path.Combine(TempDirectory, String.Format(ReportStrings.PingScOutput, CurrentUnixTime)), ReportStrings.GenericArchDirectory));
            ReportTargets.Add(new ReportTarget(ReportStrings.CmdIntExe, ReportStrings.TracertScParams, Path.Combine(TempDirectory, String.Format(ReportStrings.TracertScOutput, CurrentUnixTime)), ReportStrings.GenericArchDirectory));
            ReportTargets.Add(new ReportTarget(ReportStrings.CmdIntExe, ReportStrings.IpConfigParams, Path.Combine(TempDirectory, String.Format(ReportStrings.IpConfigOutput, CurrentUnixTime)), ReportStrings.GenericArchDirectory));
            ReportTargets.Add(new ReportTarget(ReportStrings.CmdIntExe, ReportStrings.NetStatParams, Path.Combine(TempDirectory, String.Format(ReportStrings.NetStatOutput, CurrentUnixTime)), ReportStrings.GenericArchDirectory));
            ReportTargets.Add(new ReportTarget(ReportStrings.CmdIntExe, ReportStrings.RoutesParams, Path.Combine(TempDirectory, String.Format(ReportStrings.RoutesOutput, CurrentUnixTime)), ReportStrings.GenericArchDirectory));
            ReportTargets.Add(new ReportTarget(ReportStrings.CmdIntExe, ReportStrings.UsersParams, Path.Combine(TempDirectory, String.Format(ReportStrings.UsersOutput, CurrentUnixTime)), ReportStrings.GenericArchDirectory));
        }

        /// <summary>
        /// ReportManager class constructor.
        /// </summary>
        /// <param name="AppUserDir">Path to apps's user directory.</param>
        public ReportManager(string AppUserDir)
        {
            ReportTargets = new List<ReportTarget>();
            TempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            CurrentUnixTime = FileManager.DateTime2Unix(DateTime.Now);
            ReportsDirectory = Path.Combine(AppUserDir, ReportStrings.DirectoryName);
            SetTargets();
        }
    }
}
