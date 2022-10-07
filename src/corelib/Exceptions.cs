/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
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
        public VideoConfigException() { }

        public VideoConfigException(string Message) : base(Message) { }

        public VideoConfigException(string Message, Exception InnerException) : base(Message, InnerException) { }
    }
}
