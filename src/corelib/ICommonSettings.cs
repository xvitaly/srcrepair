/**
 * SPDX-FileCopyrightText: 2011-2023 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

namespace srcrepair.core
{
    /// <summary>
    /// Interface with properties and methods for managing
    /// game video settings names.
    /// </summary>
    public interface ICommonSettings
    {
        /// <summary>
        /// Gets or sets anti-aliasing video setting name.
        /// </summary>
        string AntiAliasing { get; }

        /// <summary>
        /// Gets or sets anti-aliasing multiplier video setting name.
        /// </summary>
        string AntiAliasQuality { get; }

        /// <summary>
        /// Gets or sets brightness video setting name.
        /// </summary>
        string Brightness { get; }

        /// <summary>
        /// Gets or sets video card ID video setting name.
        /// </summary>
        string DeviceID { get; }

        /// <summary>
        /// Gets or sets borderless window video setting name.
        /// </summary>
        string DisplayBorderless { get; }

        /// <summary>
        /// Gets or sets display mode (fullscreen, windowed) video setting name.
        /// </summary>
        string DisplayMode { get; }

        /// <summary>
        /// Gets or sets filtering mode video setting name.
        /// </summary>
        string FilteringMode { get; }

        /// <summary>
        /// Gets or sets motion blur video setting name.
        /// </summary>
        string MotionBlur { get; }

        /// <summary>
        /// Gets or sets screen height video setting name.
        /// </summary>
        string ScreenHeight { get; }

        /// <summary>
        /// Gets or sets screen width video setting name.
        /// </summary>
        string ScreenWidth { get; }

        /// <summary>
        /// Gets or sets hardware vendor ID video setting name.
        /// </summary>
        string VendorID { get; }

        /// <summary>
        /// Gets or sets vertical synchronization video setting name.
        /// </summary>
        string VSync { get; }
    }
}
