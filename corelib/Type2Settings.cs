/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2019 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2019 EasyCoding Team.
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
    /// Class for working with Type 2 game video settings names.
    /// </summary>
    public class Type2Settings : IType2Settings
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
        /// Gets or sets screen aspect ratio video setting name.
        /// </summary>
        public string ScreenRatio { get; protected set; }

        /// <summary>
        /// Gets or sets brightness video setting name.
        /// </summary>
        public string Brightness { get; protected set; }

        /// <summary>
        /// Gets or sets shadow effects quality video setting name.
        /// </summary>
        public string ShadowQuality { get; protected set; }

        /// <summary>
        /// Gets or sets motion blur video setting name.
        /// </summary>
        public string MotionBlur { get; protected set; }

        /// <summary>
        /// Gets or sets display mode (fullscreen, windowed) video setting name.
        /// </summary>
        public string DisplayMode { get; protected set; }

        /// <summary>
        /// Gets or sets borderless window video setting name.
        /// </summary>
        public string DisplayBorderless { get; protected set; }

        /// <summary>
        /// Gets or sets anti-aliasing video setting name.
        /// </summary>
        public string AntiAliasing { get; protected set; }

        /// <summary>
        /// Gets or sets anti-aliasing multiplier video setting name.
        /// </summary>
        public string AntiAliasQuality { get; protected set; }

        /// <summary>
        /// Gets or sets filtering mode video setting name.
        /// </summary>
        public string FilteringMode { get; protected set; }

        /// <summary>
        /// Gets or sets vertical synchronization video setting name.
        /// </summary>
        public string VSync { get; protected set; }

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
        protected void SetSettingsV2()
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
        }

        /// <summary>
        /// Type2Settings class constructor.
        /// </summary>
        public Type2Settings()
        {
            SetSettingsV2();
        }
    }
}
