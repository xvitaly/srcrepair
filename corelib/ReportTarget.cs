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

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with a single target, used for
    /// generating reports.
    /// </summary>
    public sealed class ReportTarget
    {
        /// <summary>
        /// Gets or sets full path to application executable.
        /// </summary>
        public string Program { get; private set; }

        /// <summary>
        /// Gets or sets additional command-line arguments.
        /// </summary>
        public string Parameters { get; private set; }

        /// <summary>
        /// Gets or sets output file name with full path.
        /// </summary>
        public string OutputFileName { get; private set; }

        /// <summary>
        /// Gets or sets directory name inside of the final archive.
        /// </summary>
        public string ArchiveDirectoryName { get; private set; }

        /// <summary>
        /// ReportTarget class constructor.
        /// </summary>
        /// <param name="RtProgram">Full path to application executable.</param>
        /// <param name="RtParameters">Additional command-line arguments.</param>
        /// <param name="RtOutputFileName">Output file name with full path.</param>
        /// <param name="RtArchiveDirectoryName">Directory name inside of the final archive.</param>
        public ReportTarget(string RtProgram, string RtParameters, string RtOutputFileName, string RtArchiveDirectoryName)
        {
            Program = RtProgram;
            Parameters = RtParameters;
            OutputFileName = RtOutputFileName;
            ArchiveDirectoryName = RtArchiveDirectoryName;
        }
    }
}
