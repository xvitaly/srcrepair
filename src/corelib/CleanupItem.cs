﻿/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

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
