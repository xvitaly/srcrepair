/**
 * SPDX-FileCopyrightText: 2011-2021 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

namespace srcrepair.core
{
    /// <summary>
    /// Class with properties and methods for managing
    /// game video settings names.
    /// </summary>
    public abstract class CommonSettings : ICommonSettings
    {
        /// <summary>
        /// Gets or sets screen width video setting name.
        /// </summary>
        public string ScreenWidth { get; protected set; }

        /// <summary>
        /// Gets or sets screen height video setting name.
        /// </summary>
        public string ScreenHeight { get; protected set; }

        /// <summary>
        /// Gets or sets display mode (fullscreen, windowed) video setting name.
        /// </summary>
        public string DisplayMode { get; protected set; }

        /// <summary>
        /// Gets or sets borderless window video setting name.
        /// </summary>
        public string DisplayBorderless { get; protected set; }

        /// <summary>
        /// Gets or sets anti-aliasing video setting name.
        /// </summary>
        public string AntiAliasing { get; protected set; }

        /// <summary>
        /// Gets or sets anti-aliasing multiplier video setting name.
        /// </summary>
        public string AntiAliasQuality { get; protected set; }

        /// <summary>
        /// Gets or sets filtering mode video setting name.
        /// </summary>
        public string FilteringMode { get; protected set; }

        /// <summary>
        /// Gets or sets vertical synchronization video setting name.
        /// </summary>
        public string VSync { get; protected set; }

        /// <summary>
        /// Gets or sets motion blur video setting name.
        /// </summary>
        public string MotionBlur { get; protected set; }

        /// <summary>
        /// Gets or sets brightness video setting name.
        /// </summary>
        public string Brightness { get; protected set; }

        /// <summary>
        /// Gets or sets hardware vendor ID video setting name.
        /// </summary>
        public string VendorID { get; protected set; }

        /// <summary>
        /// Gets or sets video card ID video setting name.
        /// </summary>
        public string DeviceID { get; protected set; }
    }
}
