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
        /// Extract the variable name from the game config file entry.
        /// </summary>
        /// <param name="Value">Game config entry string for parsing.</param>
        /// <param name="SplitIndex">Separator index (space or quote).</param>
        /// <returns>Extracted variable name.</returns>
        private static string ExtractVariable(string Value, int SplitIndex)
        {
            return Value.Substring(0, SplitIndex).Trim();
        }

        /// <summary>
        /// Extract the value of the variable from the game config file entry.
        /// </summary>
        /// <param name="Value">Game config entry string for parsing.</param>
        /// <param name="SplitIndex">Separator index (space or quote).</param>
        /// <param name="CommentIndex">Position of the comment character in the source string.</param>
        /// <param name="SplitOffset">Separator size (1 for space; 0 for quote).</param>
        /// <returns>Extracted value of the variable.</returns>
        private static string ExtractValue(string Value, int SplitIndex, int CommentIndex, int SplitOffset)
        {
            return (CommentIndex > SplitIndex ? Value.Substring(SplitIndex + SplitOffset, CommentIndex - SplitIndex - SplitOffset) : Value.Remove(0, SplitIndex + SplitOffset)).Trim();
        }

        /// <summary>
        /// Extract the comment from the game config file entry.
        /// </summary>
        /// <param name="Value">Game config entry string for parsing.</param>
        /// <param name="CommentIndex">Position of the comment character in the source string.</param>
        /// <returns>Extracted comment.</returns>
        private static string ExtractComment(string Value, int CommentIndex)
        {
            return CommentIndex > 0 ? Value.Substring(CommentIndex + 2).Trim() : string.Empty;
        }

        /// <summary>
        /// An internal implementation of the game config entries strings parser.
        /// Creates an object from the string.
        /// </summary>
        /// <param name="Value">Game config entry string for parsing.</param>
        /// <param name="TryParse">Disable exceptions. Return null instead.</param>
        /// <returns>Returns the ConfigEntryParser object, or null if exceptions are disabled.</returns>
        private static ConfigEntryParser InternalParse(string Value, bool TryParse)
        {
            // Checking the source string for null...
            if (string.IsNullOrWhiteSpace(Value))
            {
                if (TryParse) { return null; } else { throw new ArgumentException("Game config entry string cannot be null, empty or contain only spaces.", nameof(Value)); }
            }

            // Calculating the indices of the first space and the double slash character...
            int SpaceIndex = Value.IndexOf(" ", StringComparison.InvariantCulture);
            int QuoteIndex = Value.IndexOf(@"""", StringComparison.InvariantCulture);
            int CommentIndex = Value.IndexOf("//", StringComparison.InvariantCulture);

            // If the source string contains no spaces, quotes and comments, return it as is...
            if ((SpaceIndex == -1) && (QuoteIndex == -1) && (CommentIndex == -1))
            {
                return new ConfigEntryParser(Value, string.Empty, string.Empty);
            }

            // If the source string contains only a name and a comment, skip the value parsing...
            if ((CommentIndex > 0) && ((SpaceIndex == -1) || (CommentIndex < SpaceIndex)))
            {
                try
                {
                    return new ConfigEntryParser(ExtractVariable(Value, CommentIndex), string.Empty, ExtractComment(Value, CommentIndex));
                }
                catch
                {
                    if (TryParse) { return null; } else { throw; }
                }
            }

            // If the source string contains quotes we will use them instead of spaces...
            if ((QuoteIndex > 0) && ((SpaceIndex == -1) || (QuoteIndex < SpaceIndex)))
            {
                try
                {
                    return new ConfigEntryParser(ExtractVariable(Value, QuoteIndex), ExtractValue(Value, QuoteIndex, CommentIndex, 0), ExtractComment(Value, CommentIndex));
                }
                catch
                {
                    if (TryParse) { return null; } else { throw; }
                }
            }

            // Parsing the source string...
            try
            {
                return new ConfigEntryParser(ExtractVariable(Value, SpaceIndex), ExtractValue(Value, SpaceIndex, CommentIndex, 1), ExtractComment(Value, CommentIndex));
            }
            catch
            {
                if (TryParse) { return null; } else { throw; }
            }
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
