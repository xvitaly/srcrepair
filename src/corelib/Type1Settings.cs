/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with Type 1 game video settings names.
    /// </summary>
    public class Type1Settings : CommonSettings, IType1Settings
    {
        /// <summary>
        /// Gets or sets model quality video setting name.
        /// </summary>
        public string ModelDetail { get; protected set; }

        /// <summary>
        /// Gets or sets texture quality video setting name.
        /// </summary>
        public string TextureDetail { get; protected set; }

        /// <summary>
        /// Gets or sets shader effects level video setting name.
        /// </summary>
        public string ShaderDetail { get; protected set; }

        /// <summary>
        /// Gets or sets water quality video setting name.
        /// </summary>
        public string WaterDetail { get; protected set; }

        /// <summary>
        /// Gets or sets water reflections level video setting name.
        /// </summary>
        public string WaterReflections { get; protected set; }

        /// <summary>
        /// Gets or sets shadow effects quality video setting name.
        /// </summary>
        public string ShadowDetail { get; protected set; }

        /// <summary>
        /// Gets or sets color correction video setting name.
        /// </summary>
        public string ColorCorrection { get; protected set; }

        /// <summary>
        /// Gets or sets anti-aliasing (MSAA) video setting name.
        /// </summary>
        public string AntiAliasingMSAA { get; protected set; }

        /// <summary>
        /// Gets or sets anti-aliasing multiplier (MSAA) video setting name.
        /// </summary>
        public string AntiAliasQualityMSAA { get; protected set; }

        /// <summary>
        /// Gets or sets trilinear filtering mode video setting name.
        /// </summary>
        public string FilteringTrilinear { get; protected set; }

        /// <summary>
        /// Gets or sets DirectX mode (version) video setting name.
        /// </summary>
        public string DirectXMode { get; protected set; }

        /// <summary>
        /// Gets or sets HDR level video setting name.
        /// </summary>
        public string HDRMode { get; protected set; }

        /// <summary>
        /// Sets properties data.
        /// </summary>
        private void SetSettings()
        {
            ScreenWidth = "ScreenWidth";
            ScreenHeight = "ScreenHeight";
            DisplayMode = "ScreenWindowed";
            DisplayBorderless = "ScreenNoBorder";
            ModelDetail = "r_rootlod";
            TextureDetail = "mat_picmip";
            ShaderDetail = "mat_reducefillrate";
            WaterDetail = "r_waterforceexpensive";
            WaterReflections = "r_waterforcereflectentities";
            ShadowDetail = "r_shadowrendertotexture";
            ColorCorrection = "mat_colorcorrection";
            AntiAliasing = "mat_antialias";
            AntiAliasingMSAA = "ScreenMSAA";
            AntiAliasQuality = "mat_aaquality";
            AntiAliasQualityMSAA = "ScreenMSAAQuality";
            FilteringMode = "mat_forceaniso";
            FilteringTrilinear = "mat_trilinear";
            VSync = "mat_vsync";
            MotionBlur = "MotionBlur";
            DirectXMode = "DXLevel_V1";
            HDRMode = "mat_hdr_level";
            VendorID = "VendorID";
            DeviceID = "DeviceID";
        }

        /// <summary>
        /// Type1Settings class constructor.
        /// </summary>
        public Type1Settings()
        {
            SetSettings();
        }
    }
}
