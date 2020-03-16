/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2020 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2020 EasyCoding Team.
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
