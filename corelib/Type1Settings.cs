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
    /// Class for working with Type 1 game video settings names.
    /// </summary>
    public class Type1Settings : IType1Settings
    {
        /// <summary>
        /// Gets or sets screen width video setting name.
        /// </summary>
        public string ScreenWidth { get; protected set; }

        /// <summary>
        /// Gets or sets screen height video setting name.
        /// </summary>
        public string ScreenHeight { get; protected set; }

        /// <summary>
        /// Gets or sets display mode (fullscreen, windowed) video setting name.
        /// </summary>
        public string DisplayMode { get; protected set; }

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
        /// Gets or sets anti-aliasing video setting name.
        /// </summary>
        public string AntiAliasing { get; protected set; }

        /// <summary>
        /// Gets or sets anti-aliasing (MSAA) video setting name.
        /// </summary>
        public string AntiAliasingMSAA { get; protected set; }

        /// <summary>
        /// Gets or sets anti-aliasing multiplier video setting name.
        /// </summary>
        public string AntiAliasQuality { get; protected set; }

        /// <summary>
        /// Gets or sets anti-aliasing multiplier (MSAA) video setting name.
        /// </summary>
        public string AntiAliasQualityMSAA { get; protected set; }

        /// <summary>
        /// Gets or sets filtering mode video setting name.
        /// </summary>
        public string FilteringMode { get; protected set; }

        /// <summary>
        /// Gets or sets trilinear filtering mode video setting name.
        /// </summary>
        public string FilteringTrilinear { get; protected set; }

        /// <summary>
        /// Gets or sets vertical synchronization video setting name.
        /// </summary>
        public string VSync { get; protected set; }

        /// <summary>
        /// Gets or sets motion blur video setting name.
        /// </summary>
        public string MotionBlur { get; protected set; }

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
        protected void SetSettingsV1()
        {
            ScreenWidth = "ScreenWidth";
            ScreenHeight = "ScreenHeight";
            DisplayMode = "ScreenWindowed";
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
        }

        /// <summary>
        /// Type1Settings class constructor.
        /// </summary>
        public Type1Settings()
        {
            SetSettingsV1();
        }
    }
}
