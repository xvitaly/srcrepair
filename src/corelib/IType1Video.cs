/**
 * SPDX-FileCopyrightText: 2011-2023 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
