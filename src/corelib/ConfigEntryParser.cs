/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;

namespace srcrepair.core
{
    public sealed class ConfigEntryParser
    {
        public string Variable { get; private set; }
        public string Value { get; private set; }
        public string Comment { get; private set; }

        private static ConfigEntryParser InternalParse(string Value, bool TryParse)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(string SrcStr, out ConfigEntryParser Parser)
        {
            Parser = InternalParse(SrcStr, true);
            return !(Parser is null);
        }

        public static ConfigEntryParser Parse(string SrcStr)
        {
            return InternalParse(SrcStr, false);
        }

        private ConfigEntryParser(string VariableStr, string ValueStr, string CommentStr)
        {
            Variable = VariableStr;
            Value = ValueStr;
            Comment = CommentStr;
        }
    }
}
