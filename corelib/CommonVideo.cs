﻿/*
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
    /// Abstract class with common properties and methods for
    /// all types of games.
    /// </summary>
    public abstract class CommonVideo : ICommonVideo
    {
        /// <summary>
        /// Stores anti-aliasing setting: mat_antialias.
        /// </summary>
        protected int _AntiAliasing;

        /// <summary>
        /// Stores anti-aliasing multiplier: mat_aaquality.
        /// </summary>
        protected int _AntiAliasQuality;

        /// <summary>
        /// Stores motion blur setting: MotionBlur.
        /// </summary>
        protected int _MotionBlur;

        /// <summary>
        /// Stores screen width.
        /// </summary>
        protected int _ScreenWidth = 800;

        /// <summary>
        /// Stores screen height.
        /// </summary>
        protected int _ScreenHeight = 600;

        /// <summary>
        /// Gets or sets anti-aliasing video setting.
        /// </summary>
        public int AntiAliasing
        {
            get
            {
                int res = -1;

                switch (_AntiAliasing)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 0;
                        break;
                    case 2:
                        res = 1;
                        break;
                    case 4:
                        switch (_AntiAliasQuality)
                        {
                            case 0:
                                res = 2;
                                break;
                            case 2:
                                res = 3;
                                break;
                            case 4:
                                res = 4;
                                break;
                        }
                        break;
                    case 8:
                        switch (_AntiAliasQuality)
                        {
                            case 0:
                                res = 5;
                                break;
                            case 2:
                                res = 6;
                                break;
                        }
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0: // No AA
                        _AntiAliasing = 1;
                        _AntiAliasQuality = 0;
                        break;
                    case 1: // 2x MSAA
                        _AntiAliasing = 2;
                        _AntiAliasQuality = 0;
                        break;
                    case 2: // 4x MSAA
                        _AntiAliasing = 4;
                        _AntiAliasQuality = 0;
                        break;
                    case 3: // 8x CSAA
                        _AntiAliasing = 4;
                        _AntiAliasQuality = 2;
                        break;
                    case 4: // 16x CSAA
                        _AntiAliasing = 4;
                        _AntiAliasQuality = 4;
                        break;
                    case 5: // 8x MSAA
                        _AntiAliasing = 8;
                        _AntiAliasQuality = 0;
                        break;
                    case 6: // 16xQ CSAA
                        _AntiAliasing = 8;
                        _AntiAliasQuality = 2;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets motion blur video setting.
        /// </summary>
        public int MotionBlur
        {
            get
            {
                int res = -1;

                switch (_MotionBlur)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _MotionBlur = 0;
                        break;
                    case 1:
                        _MotionBlur = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets shadow effects quality video setting.
        /// </summary>
        public abstract int ShadowQuality { get; set; }

        /// <summary>
        /// Gets or sets screen width video setting.
        /// </summary>
        public int ScreenWidth { get => _ScreenWidth; set { _ScreenWidth = value; } }

        /// <summary>
        /// Gets or sets screen height video setting.
        /// </summary>
        public int ScreenHeight { get => _ScreenHeight; set { _ScreenHeight = value; } }

        /// <summary>
        /// Gets or sets vertical synchronization video setting.
        /// </summary>
        public abstract int VSync { get; set; }

        /// <summary>
        /// Reads game video settings.
        /// </summary>
        public abstract void ReadSettings();

        /// <summary>
        /// Writes game video settings.
        /// </summary>
        public abstract void WriteSettings();

        /// <summary>
        /// Gets Cvar value from string.
        /// </summary>
        /// <param name="LineA">Source string.</param>
        /// <returns>Extracted from source string value.</returns>
        protected static string ExtractCVFromLine(string LineA)
        {
            LineA = StringsManager.CleanString(LineA, true);
            return LineA.Substring(LineA.LastIndexOf(" ")).Trim();
        }
    }
}