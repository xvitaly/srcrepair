/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
