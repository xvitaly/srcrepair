/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with Type 3 game video settings names.
    /// </summary>
    public class Type3Settings : Type2Settings
    {
        /// <summary>
        /// Gets or sets shader effects level video setting name.
        /// </summary>
        public string SchemaVersion { get; protected set; }

        /// <summary>
        /// Gets or sets video adapter device knowledge video setting name.
        /// </summary>
        public string KnownDevice { get; protected set; }

        /// <summary>
        /// Gets or sets display mode (fullscreen, windowed) video setting name.
        /// </summary>
        public string FullScreenMode { get; protected set; }

        /// <summary>
        /// Gets or sets display mode (fullscreen, windowed) in co-op mode
        /// video setting name.
        /// </summary>
        public string FullScreenModeCoop { get; protected set; }

        /// <summary>
        /// Gets or sets display mode on focus loss video setting name.
        /// </summary>
        public string DisplayModeFocusLoss { get; protected set; }

        /// <summary>
        /// Gets or sets cheap water reflections video setting name.
        /// </summary>
        public string CheapWaterReflections { get; protected set; }

        /// <summary>
        /// Gets or sets fog of war height video setting name.
        /// </summary>
        public string FogHeight { get; protected set; }

        /// <summary>
        /// Gets or sets simple lighting effects video setting name.
        /// </summary>
        public string SimpleLight { get; protected set; }

        /// <summary>
        /// Gets or sets SSAO lighting effects video setting name.
        /// </summary>
        public string SSAOLights { get; protected set; }

        /// <summary>
        /// Gets or sets global shadow effects video setting name.
        /// </summary>
        public string GlobalShadowMode { get; protected set; }

        /// <summary>
        /// Gets or sets portrait animations video setting name.
        /// </summary>
        public string PortraitAnimation { get; protected set; }

        /// <summary>
        /// Gets or sets specular shading video setting name.
        /// </summary>
        public string SpecularShading { get; protected set; }

        /// <summary>
        /// Gets or sets specular bloom shading video setting name.
        /// </summary>
        public string SpecularBloomShading { get; protected set; }

        /// <summary>
        /// Gets or sets ambient creatures video setting name.
        /// </summary>
        public string AmbientCreatures { get; protected set; }

        /// <summary>
        /// Gets or sets texture streaming video setting name.
        /// </summary>
        public string TextureStreaming { get; protected set; }

        /// <summary>
        /// Gets or sets normal maps video setting name.
        /// </summary>
        public string NormalMaps { get; protected set; }

        /// <summary>
        /// Gets or sets dashboard quality video setting name.
        /// </summary>
        public string DashboardQuality { get; protected set; }

        /// <summary>
        /// Gets or sets shader effects level video setting name.
        /// </summary>
        public string ShaderQuality { get; protected set; }

        /// <summary>
        /// Gets or sets recommended height video setting name.
        /// </summary>
        public string RecommendedHeight { get; protected set; }

        /// <summary>
        /// Gets or sets viewport scale video setting name.
        /// </summary>
        public string ViewportScale { get; protected set; }

        /// <summary>
        /// Gets or sets advanced options video setting name.
        /// </summary>
        public string UseAdvanced { get; protected set; }

        /// <summary>
        /// Gets or sets ambient cloth quality video setting name.
        /// </summary>
        public string AmbientCloth { get; protected set; }

        /// <summary>
        /// Gets or sets grass quality video setting name.
        /// </summary>
        public string GrassQuality { get; protected set; }

        /// <summary>
        /// Gets or sets wind effects quality video setting name.
        /// </summary>
        public string WindEffects { get; protected set; }

        /// <summary>
        /// Gets or sets parallax mapping video setting name.
        /// </summary>
        public string ParallaxMapping { get; protected set; }

        /// <summary>
        /// Gets or sets particle fallback video setting name.
        /// </summary>
        public string ParticleFallback { get; protected set; }

        /// <summary>
        /// Gets or sets particle fallback multiplier video setting name.
        /// </summary>
        public string ParticleFallbackMultiplier { get; protected set; }

        /// <summary>
        /// Gets or sets reset to defaults video setting name.
        /// </summary>
        public string ResetToDefaults { get; protected set; }

        /// <summary>
        /// Sets properties data.
        /// </summary>
        private void SetSettings()
        {
            VendorID = "VendorID";
            DeviceID = "DeviceID";
            EffectDetails = "setting.cpu_level";
            MemoryPoolType = "setting.mem_level";
            TextureModelQuality = "setting.gpu_mem_level";
            ScreenWidth = "setting.defaultres";
            ScreenHeight = "setting.defaultresheight";
            ShaderEffects = "setting.gpu_level";
            SchemaVersion = "setting.version";
            KnownDevice = "setting.knowndevice";
            FullScreenMode = "setting.fullscreen";
            FullScreenModeCoop = "setting.coop_fullscreen";
            DisplayBorderless = "setting.nowindowborder";
            VSync = "setting.mat_vsync";
            DisplayModeFocusLoss = "setting.fullscreen_min_on_focus_loss";
            CheapWaterReflections = "setting.dota_cheap_water";
            FogHeight = "setting.r_deferred_height_fog";
            SimpleLight = "setting.r_deferred_simple_light";
            SSAOLights = "setting.r_ssao";
            GlobalShadowMode = "setting.cl_globallight_shadow_mode";
            AntiAliasing = "setting.r_dota_fxaa";
            AntiAliasQuality = "setting.r_deferred_additive_pass";
            PortraitAnimation = "setting.dota_portrait_animate";
            SpecularShading = "setting.r_deferred_specular";
            SpecularBloomShading = "setting.r_deferred_specular_bloom";
            AmbientCreatures = "setting.dota_ambient_creatures";
            TextureStreaming = "setting.r_texture_stream_mip_bias";
            NormalMaps = "setting.r_dota_normal_maps";
            DashboardQuality = "setting.r_dashboard_render_quality";
            ShaderQuality = "setting.shaderquality";
            RecommendedHeight = "setting.recommendedheight";
            ViewportScale = "setting.mat_viewportscale";
            Brightness = "setting.r_fullscreen_gamma";
            UseAdvanced = "setting.useadvanced";
            AmbientCloth = "setting.dota_ambient_cloth";
            GrassQuality = "setting.r_grass_quality";
            WindEffects = "setting.r_dota_allow_wind_on_trees";
            ParallaxMapping = "setting.r_dota_allow_parallax_mapping";
            ParticleFallback = "setting.cl_particle_fallback_base";
            ParticleFallbackMultiplier = "setting.cl_particle_fallback_multiplier";
            ResetToDefaults = "setting.resettodefaults";
            ScreenRatio = "setting.aspectratiomode";
        }

        /// <summary>
        /// Type3Settings class constructor.
        /// </summary>
        public Type3Settings()
        {
            SetSettings();
        }
    }
}
