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
    /// Interface with properties and methods for managing Type 1
    /// game video settings.
    /// </summary>
    public interface IType1Video : ICommonVideo
    {
        /// <summary>
        /// Gets or sets color correction video setting.
        /// </summary>
        int ColorCorrection { get; set; }

        /// <summary>
        /// Gets or sets DirectX mode (version) video setting.
        /// </summary>
        int DirectXMode { get; set; }

        /// <summary>
        /// Gets or sets display mode (fullscreen, windowed) video setting.
        /// </summary>
        int DisplayMode { get; set; }

        /// <summary>
        /// Gets or sets filtering mode video setting.
        /// </summary>
        int FilteringMode { get; set; }

        /// <summary>
        /// Gets or sets HDR level video setting.
        /// </summary>
        int HDRType { get; set; }

        /// <summary>
        /// Gets or sets model quality video setting.
        /// </summary>
        int ModelQuality { get; set; }

        /// <summary>
        /// Gets or sets water reflections quality video setting.
        /// </summary>
        int ReflectionsQuality { get; set; }

        /// <summary>
        /// Gets or sets shader effects quality video setting.
        /// </summary>
        int ShaderQuality { get; set; }

        /// <summary>
        /// Gets or sets texture quality video setting.
        /// </summary>
        int TextureQuality { get; set; }
    }
}
