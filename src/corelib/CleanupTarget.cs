﻿/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

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
        public List<CleanupItem> Items { get; private set; }

        /// <summary>
        /// CleanupTarget class constructor.
        /// </summary>
        /// <param name="CtName">Cleanup target friendly name.</param>
        /// <param name="CtDirectories">List of directories for cleanup.</param>
        public CleanupTarget(string CtName, List<CleanupItem> CtItems)
        {
            Name = CtName;
            Items = CtItems;
        }
    }
}
