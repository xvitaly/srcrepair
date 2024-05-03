/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;

namespace srcrepair.core
{
    /// <summary>
    /// Class for parsing game config file entries.
    /// </summary>
    public sealed class ConfigEntryParser
    {
        /// <summary>
        /// Gets or sets the variable name as a string.
        /// </summary>
        public string Variable { get; private set; }

        /// <summary>
        /// Gets or sets the value of a variable as a string.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Gets or sets the comment of a variable as a string.
        /// </summary>
        public string Comment { get; private set; }

        /// <summary>
        /// An internal implementation of the game config entries strings parser.
        /// Creates an object from the string.
        /// </summary>
        /// <param name="Value">Game config entry string for parsing.</param>
        /// <param name="TryParse">Disable exceptions. Return null instead.</param>
        /// <returns>Returns the ConfigEntryParser object, or null if exceptions are disabled.</returns>
        private static ConfigEntryParser InternalParse(string Value, bool TryParse)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parse game config entry string to an object without throwing any exceptions.
        /// </summary>
        /// <param name="SrcStr">Game config entry string for parsing.</param>
        /// <param name="Parser">An instance of the ConfigEntryParser object with result.</param>
        /// <returns>Returns if the ConfigEntryParser object was successfully created.</returns>
        public static bool TryParse(string SrcStr, out ConfigEntryParser Parser)
        {
            Parser = InternalParse(SrcStr, true);
            return !(Parser is null);
        }

        /// <summary>
        /// Parse game config entry string to an object.
        /// </summary>
        /// <param name="SrcStr">Game config entry string for parsing.</param>
        /// <returns>Returns an instance of the ConfigEntryParser object.</returns>
        public static ConfigEntryParser Parse(string SrcStr)
        {
            return InternalParse(SrcStr, false);
        }

        /// <summary>
        /// Main constructor of the ConfigEntryParser class. Should not be used directly.
        /// Use the Parse() or TryParse() methods to create instances.
        /// </summary>
        /// <param name="VariableStr">Variable field as a string.</param>
        /// <param name="ValueStr">Value field as a string.</param>
        /// <param name="CommentStr">Comment field as a string.</param>
        private ConfigEntryParser(string VariableStr, string ValueStr, string CommentStr)
        {
            Variable = VariableStr;
            Value = ValueStr;
            Comment = CommentStr;
        }
    }
}
