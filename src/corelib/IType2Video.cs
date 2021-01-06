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
    /// Interface with properties and methods for managing Type 2
    /// game video settings.
    /// </summary>
    public interface IType2Video : ICommonVideo
    {
        /// <summary>
        /// Gets or sets standard effects video setting.
        /// </summary>
        int Effects { get; set; }

        /// <summary>
        /// Gets or sets filtering mode video setting.
        /// </summary>
        int FilteringMode { get; set; }

        /// <summary>
        /// Gets or sets memory pool video setting.
        /// </summary>
        int MemoryPool { get; set; }

        /// <summary>
        /// Gets or sets model quality video setting.
        /// </summary>
        int ModelQuality { get; set; }

        /// <summary>
        /// Gets or sets multicore rendering video setting.
        /// </summary>
        int RenderingMode { get; set; }

        /// <summary>
        /// Gets or sets screen gamma video setting.
        /// </summary>
        string ScreenGamma { get; set; }

        /// <summary>
        /// Gets or sets display mode (fullscreen, windowed) video setting.
        /// </summary>
        int ScreenMode { get; set; }

        /// <summary>
        /// Gets or sets screen aspect ratio video setting.
        /// </summary>
        int ScreenRatio { get; set; }

        /// <summary>
        /// Gets or sets shader effects level video setting.
        /// </summary>
        int ShaderEffects { get; set; }
    }
}
