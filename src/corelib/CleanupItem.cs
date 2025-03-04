/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System.Collections.Generic;
using System.IO;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with a single cleanup item.
    /// </summary>
    public sealed class CleanupItem
    {
        /// <summary>
        /// Gets or sets the full path to the directory for cleanup.
        /// </summary>
        public string CleanDirectory { get; private set; }

        /// <summary>
        /// Gets or sets the file mask for cleanup.
        /// </summary>
        public string CleanMask { get; private set; }

        /// <summary>
        /// Gets or sets whether the recursive cleanup is enabled.
        /// </summary>
        public bool IsRecursive { get; private set; }

        /// <summary>
        /// Gets or sets whether the empty directories cleanup is enabled.
        /// </summary>
        public bool CleanEmpty { get; private set; }

        /// <summary>
        /// Creates a modern cleanup file list from a legacy list format.
        /// </summary>
        /// <param name="Items">List of files and directories for deletion in legacy format.</param>
        /// <param name="Recursive">Enable or disable the recursive cleanup.</param>
        /// <param name="Empty">Enable or disable the empty directories cleanup.</param>
        /// <returns>List of files and directories for deletion.</returns>
        public static List<CleanupItem> CreateFromLegacyList(List<string> Items, bool Recursive, bool Empty)
        {
            List<CleanupItem> Result = new List<CleanupItem>();
            foreach (string Item in Items)
            {
                Result.Add(new CleanupItem(Path.GetDirectoryName(Item), Path.GetFileName(Item), Recursive, Empty));
            }
            return Result;
        }

        /// <summary>
        /// CleanupItem class constructor with all files file mask,
        /// recursive cleanup and cleanup of empty directories options
        /// enabled.
        /// </summary>
        /// <param name="Dir">Full path to the directory for cleanup.</param>
        public CleanupItem(string Dir)
        {
            CleanDirectory = Dir;
            CleanMask = Properties.Resources.AllFilesMask;
            IsRecursive = true;
            CleanEmpty = true;
        }

        /// <summary>
        /// CleanupItem class constructor with recursive cleanup
        /// and cleanup of empty directories options enabled.
        /// </summary>
        /// <param name="Dir">Full path to the directory for cleanup.</param>
        /// <param name="Mask">File mask for cleanup.</param>
        public CleanupItem(string Dir, string Mask)
        {
            CleanDirectory = Dir;
            CleanMask = Mask;
            IsRecursive = true;
            CleanEmpty = true;
        }

        /// <summary>
        /// CleanupItem class constructor with all files file mask.
        /// </summary>
        /// <param name="Dir">Full path to the directory for cleanup.</param>
        /// <param name="Recursive">Enable or disable the recursive cleanup.</param>
        /// <param name="Empty">Enable or disable the empty directories cleanup.</param>
        public CleanupItem(string Dir, bool Recursive, bool Empty)
        {
            CleanDirectory = Dir;
            CleanMask = Properties.Resources.AllFilesMask;
            IsRecursive = Recursive;
            CleanEmpty = Empty;
        }

        /// <summary>
        /// CleanupItem class constructor.
        /// </summary>
        /// <param name="Dir">Full path to the directory for cleanup.</param>
        /// <param name="Mask">File mask for cleanup.</param>
        /// <param name="Recursive">Enable or disable the recursive cleanup.</param>
        /// <param name="Empty">Enable or disable the empty directories cleanup.</param>
        public CleanupItem(string Dir, string Mask, bool Recursive, bool Empty)
        {
            CleanDirectory = Dir;
            CleanMask = Mask;
            IsRecursive = Recursive;
            CleanEmpty = Empty;
        }
    }
}
