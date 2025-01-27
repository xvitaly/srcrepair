/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;

namespace srcrepair.core
{
    public sealed class CleanupItem
    {
        public string CleanDirectory { get; private set; }

        public string CleanMask { get; private set; }

        public bool IsRecursive { get; private set; }

        public bool CleanEmpty { get; private set; }

        public CleanupItem(string Dir, string Mask, bool Recursive, bool Empty)
        {
            CleanDirectory = Dir;
            CleanMask = Mask;
            IsRecursive = Recursive;
            CleanEmpty = Empty;
        }
    }
}
