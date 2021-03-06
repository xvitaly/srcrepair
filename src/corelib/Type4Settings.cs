﻿/*
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
    /// Class for working with Type 4 game video settings names.
    /// </summary>
    public class Type4Settings : Type1Settings
    {
        /// <summary>
        /// Gets or sets shadow delpth video setting name.
        /// </summary>
        public string ShadowDepth { get; protected set; }

        /// <summary>
        /// Sets properties data for Type 4 game.
        /// </summary>
        private void SetSettings()
        {
            DisplayBorderless = "ScreenWindoNoBorder";
            Brightness = "ScreenMonitorGamma";
            ShadowDepth = "ShadowDepthTexture";
        }

        /// <summary>
        /// Type4Settings class constructor.
        /// </summary>
        public Type4Settings()
        {
            SetSettings();
        }
    }
}
