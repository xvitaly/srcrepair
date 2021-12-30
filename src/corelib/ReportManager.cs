﻿/**
 * SPDX-FileCopyrightText: 2011-2021 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
            ReportTargets.Add(new ReportTarget(ReportStrings.CmdIntExe, ReportStrings.BasicInfoParams, Path.Combine(TempDirectory, String.Format(ReportStrings.BasicInfoOutput, CurrentUnixTime)), ReportStrings.GenericArchDirectory));
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
