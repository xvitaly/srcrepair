/**
 * SPDX-FileCopyrightText: 2011-2023 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

namespace srcrepair.core
{
    /// <summary>
    /// Interface with properties and methods for managing Type 1
    /// game video settings names.
    /// </summary>
    public interface IType1Settings : ICommonSettings
    {
        /// <summary>
        /// Gets anti-aliasing (MSAA) video setting name.
        /// </summary>
        string AntiAliasingMSAA { get; }

        /// <summary>
        /// Gets anti-aliasing multiplier (MSAA) video setting name.
        /// </summary>
        string AntiAliasQualityMSAA { get; }

        /// <summary>
        /// Gets color correction video setting name.
        /// </summary>
        string ColorCorrection { get; }

        /// <summary>
        /// Gets DirectX mode (version) video setting name.
        /// </summary>
        string DirectXMode { get; }

        /// <summary>
        /// Gets trilinear filtering mode video setting name.
        /// </summary>
        string FilteringTrilinear { get; }

        /// <summary>
        /// Gets HDR level video setting name.
        /// </summary>
        string HDRMode { get; }

        /// <summary>
        /// Gets model quality video setting name.
        /// </summary>
        string ModelDetail { get; }

        /// <summary>
        /// Gets shader effects level video setting name.
        /// </summary>
        string ShaderDetail { get; }

        /// <summary>
        /// Gets shadow effects quality video setting name.
        /// </summary>
        string ShadowDetail { get; }

        /// <summary>
        /// Gets texture quality video setting name.
        /// </summary>
        string TextureDetail { get; }

        /// <summary>
        /// Gets water quality video setting name.
        /// </summary>
        string WaterDetail { get; }

        /// <summary>
        /// Gets water reflections level video setting name.
        /// </summary>
        string WaterReflections { get; }
    }
}
