/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with Type 2 game video settings names.
    /// </summary>
    public class Type2Settings : CommonSettings, IType2Settings
    {
        /// <summary>
        /// Gets or sets screen aspect ratio video setting name.
        /// </summary>
        public string ScreenRatio { get; protected set; }

        /// <summary>
        /// Gets or sets shadow effects quality video setting name.
        /// </summary>
        public string ShadowQuality { get; protected set; }

        /// <summary>
        /// Gets or sets vertical synchronization quality video setting name.
        /// </summary>
        public string VSyncMode { get; protected set; }

        /// <summary>
        /// Gets or sets multicore rendering video setting name.
        /// </summary>
        public string MCRendering { get; protected set; }

        /// <summary>
        /// Gets or sets shader effects level video setting name.
        /// </summary>
        public string ShaderEffects { get; protected set; }

        /// <summary>
        /// Gets or sets standard effects video setting name.
        /// </summary>
        public string EffectDetails { get; protected set; }

        /// <summary>
        /// Gets or sets memory pool video setting name.
        /// </summary>
        public string MemoryPoolType { get; protected set; }

        /// <summary>
        /// Gets or sets texture quality video setting name.
        /// </summary>
        public string TextureModelQuality { get; protected set; }

        /// <summary>
        /// Gets or sets grain scale video setting name.
        /// </summary>
        public string GrainScaleOverride { get; protected set; }

        /// <summary>
        /// Sets properties data.
        /// </summary>
        private void SetSettings()
        {
            ScreenWidth = "defaultres";
            ScreenHeight = "defaultresheight";
            ScreenRatio = "aspectratiomode";
            Brightness = "mat_monitorgamma";
            ShadowQuality = "csm_quality_level";
            MotionBlur = "mat_motion_blur_enabled";
            DisplayMode = "fullscreen";
            DisplayBorderless = "nowindowborder";
            AntiAliasing = "mat_antialias";
            AntiAliasQuality = "mat_aaquality";
            FilteringMode = "mat_forceaniso";
            VSync = "mat_vsync";
            VSyncMode = "mat_triplebuffered";
            MCRendering = "mat_queue_mode";
            ShaderEffects = "gpu_level";
            EffectDetails = "cpu_level";
            MemoryPoolType = "mem_level";
            TextureModelQuality = "gpu_mem_level";
            GrainScaleOverride = "mat_grain_scale_override";
            VendorID = "VendorID";
            DeviceID = "DeviceID";
        }

        /// <summary>
        /// Type2Settings class constructor.
        /// </summary>
        public Type2Settings()
        {
            SetSettings();
        }
    }
}
