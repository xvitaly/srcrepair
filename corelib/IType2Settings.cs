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
    /// Interface with properties and methods for managing Type 2
    /// game video settings names.
    /// </summary>
    public interface IType2Settings : ICommonSettings
    {
        /// <summary>
        /// Gets standard effects video setting name.
        /// </summary>
        string EffectDetails { get; }

        /// <summary>
        /// Gets grain scale video setting name.
        /// </summary>
        string GrainScaleOverride { get; }

        /// <summary>
        /// Gets multicore rendering video setting name.
        /// </summary>
        string MCRendering { get; }

        /// <summary>
        /// Gets memory pool video setting name.
        /// </summary>
        string MemoryPoolType { get; }

        /// <summary>
        /// Gets screen aspect ratio video setting name.
        /// </summary>
        string ScreenRatio { get; }

        /// <summary>
        /// Gets shader effects level video setting name.
        /// </summary>
        string ShaderEffects { get; }

        /// <summary>
        /// Gets shadow effects quality video setting name.
        /// </summary>
        string ShadowQuality { get; }

        /// <summary>
        /// Gets texture quality video setting name.
        /// </summary>
        string TextureModelQuality { get; }

        /// <summary>
        /// Gets vertical synchronization quality video setting name.
        /// </summary>
        string VSyncMode { get; }
    }
}
