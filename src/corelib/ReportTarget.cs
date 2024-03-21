/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
        /// Gets or sets if current target is mandatory.
        /// </summary>
        public bool IsMandatory { get; private set; }

        /// <summary>
        /// ReportTarget class constructor.
        /// </summary>
        /// <param name="RtProgram">Full path to application executable.</param>
        /// <param name="RtParameters">Additional command-line arguments.</param>
        /// <param name="RtOutputFileName">Output file name with full path.</param>
        /// <param name="RtArchiveDirectoryName">Directory name inside of the final archive.</param>
        /// <param name="RtIsMandatory">If current target is mandatory.</param>
        public ReportTarget(string RtProgram, string RtParameters, string RtOutputFileName, string RtArchiveDirectoryName, bool RtIsMandatory)
        {
            Program = RtProgram;
            Parameters = RtParameters;
            OutputFileName = RtOutputFileName;
            ArchiveDirectoryName = RtArchiveDirectoryName;
            IsMandatory = RtIsMandatory;
        }
    }
}
