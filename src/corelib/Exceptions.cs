/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;

namespace srcrepair.core
{
    /// <summary>
    /// Class of custom VideoConfigException type.
    /// </summary>
    [Serializable]
    public class VideoConfigException : Exception
    {
        /// <summary>
        /// Parameter-less constructor of the VideoConfigException class.
        /// </summary>
        public VideoConfigException() { }

        /// <summary>
        /// Constructor of the VideoConfigException class with optional message.
        /// </summary>
        /// <param name="Message">Exception message.</param>
        public VideoConfigException(string Message) : base(Message) { }

        /// <summary>
        /// Constructor of the VideoConfigException class with optional message and a
        /// reference to the inner exception.
        /// </summary>
        /// <param name="Message">Exception message.</param>
        /// <param name="InnerException">Reference to the inner exception.</param>
        public VideoConfigException(string Message, Exception InnerException) : base(Message, InnerException) { }
    }

    /// <summary>
    /// Class of custom SteamPathNotFoundException type.
    /// </summary>
    [Serializable]
    public class SteamPathNotFoundException : Exception
    {
        /// <summary>
        /// Parameter-less constructor of the SteamPathNotFoundException class.
        /// </summary>
        public SteamPathNotFoundException() { }

        /// <summary>
        /// Constructor of the SteamPathNotFoundException class with optional message.
        /// </summary>
        /// <param name="Message">Exception message.</param>
        public SteamPathNotFoundException(string Message) : base(Message) { }

        /// <summary>
        /// Constructor of the SteamPathNotFoundException class with optional message and a
        /// reference to the inner exception.
        /// </summary>
        /// <param name="Message">Exception message.</param>
        /// <param name="InnerException">Reference to the inner exception.</param>
        public SteamPathNotFoundException(string Message, Exception InnerException) : base(Message, InnerException) { }
    }

    /// <summary>
    /// Class of custom SteamLangNameNotFoundException type.
    /// </summary>
    [Serializable]
    public class SteamLangNameNotFoundException : Exception
    {
        /// <summary>
        /// Parameter-less constructor of the SteamLangNameNotFoundException class.
        /// </summary>
        public SteamLangNameNotFoundException() { }

        /// <summary>
        /// Constructor of the SteamLangNameNotFoundException class with optional message.
        /// </summary>
        /// <param name="Message">Exception message.</param>
        public SteamLangNameNotFoundException(string Message) : base(Message) { }

        /// <summary>
        /// Constructor of the SteamLangNameNotFoundException class with optional message and
        /// a reference to the inner exception.
        /// </summary>
        /// <param name="Message">Exception message.</param>
        /// <param name="InnerException">Reference to the inner exception.</param>
        public SteamLangNameNotFoundException(string Message, Exception InnerException) : base(Message, InnerException) { }
    }
}
