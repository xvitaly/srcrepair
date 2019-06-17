/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2019 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2019 EasyCoding Team.
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
    /// Interface with properties and methods for managing Type 2
    /// game video settings names.
    /// </summary>
    public interface IType2Settings
    {
        /// <summary>
        /// Gets and sets anti-aliasing video setting name.
        /// </summary>
        string AntiAliasing { get; }

        /// <summary>
        /// Gets and sets anti-aliasing multiplier video setting name.
        /// </summary>
        string AntiAliasQuality { get; }

        /// <summary>
        /// Gets and sets brightness video setting name.
        /// </summary>
        string Brightness { get; }

        /// <summary>
        /// Gets and sets borderless window video setting name.
        /// </summary>
        string DisplayBorderless { get; }

        /// <summary>
        /// Gets and sets display mode (fullscreen, windowed) video setting name.
        /// </summary>
        string DisplayMode { get; }

        /// <summary>
        /// Gets and sets standard effects video setting name.
        /// </summary>
        string EffectDetails { get; }

        /// <summary>
        /// Gets and sets filtering mode video setting name.
        /// </summary>
        string FilteringMode { get; }

        /// <summary>
        /// Gets and sets grain scale video setting name.
        /// </summary>
        string GrainScaleOverride { get; }

        /// <summary>
        /// Gets and sets multicore rendering video setting name.
        /// </summary>
        string MCRendering { get; }

        /// <summary>
        /// Gets and sets memory pool video setting name.
        /// </summary>
        string MemoryPoolType { get; }

        /// <summary>
        /// Gets and sets motion blur video setting name.
        /// </summary>
        string MotionBlur { get; }

        /// <summary>
        /// Gets and sets screen height video setting name.
        /// </summary>
        string ScreenHeight { get; }

        /// <summary>
        /// Gets and sets screen aspect ratio video setting name.
        /// </summary>
        string ScreenRatio { get; }

        /// <summary>
        /// Gets and sets screen width video setting name.
        /// </summary>
        string ScreenWidth { get; }

        /// <summary>
        /// Gets and sets shader effects level video setting name.
        /// </summary>
        string ShaderEffects { get; }

        /// <summary>
        /// Gets and sets shadow effects quality video setting name.
        /// </summary>
        string ShadowQuality { get; }

        /// <summary>
        /// Gets and sets texture quality video setting name.
        /// </summary>
        string TextureModelQuality { get; }

        /// <summary>
        /// Gets and sets vertical synchronization video setting name.
        /// </summary>
        string VSync { get; }

        /// <summary>
        /// Gets and sets vertical synchronization quality video setting name.
        /// </summary>
        string VSyncMode { get; }
    }
}
