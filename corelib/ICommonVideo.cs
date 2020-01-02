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
    /// Interface with common properties and methods for
    /// all types of games.
    /// </summary>
    interface ICommonVideo
    {
        /// <summary>
        /// Gets and sets anti-aliasing video setting.
        /// </summary>
        int AntiAliasing { get; set; }

        /// <summary>
        /// Gets and sets motion blur video setting.
        /// </summary>
        int MotionBlur { get; set; }

        /// <summary>
        /// Gets and sets shadow quality video setting.
        /// </summary>
        int ShadowQuality { get; set; }

        /// <summary>
        /// Gets and sets screen width video setting.
        /// </summary>
        int ScreenWidth { get; set; }

        /// <summary>
        /// Gets and sets screen height video setting.
        /// </summary>
        int ScreenHeight { get; set; }

        /// <summary>
        /// Gets and sets vertical synchronization video setting.
        /// </summary>
        int VSync { get; set; }

        /// <summary>
        /// Saves video settings to file or registry.
        /// </summary>
        void WriteSettings();
    }
}
