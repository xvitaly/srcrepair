/**
 * SPDX-FileCopyrightText: 2011-2023 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
