/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 *
 * Copyright (c) 2011 - 2021 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2021 EasyCoding Team.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
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
