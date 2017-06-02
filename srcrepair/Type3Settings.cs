/*
 * Служебный класс работы с настройками графики Type 3 игр.
 * 
 * Copyright 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2017 EasyCoding Team.
 * 
 * Лицензия: GPL v3 (см. файл GPL.txt).
 * Лицензия контента: Creative Commons 3.0 BY.
 * 
 * Запрещается использовать этот файл при использовании любой
 * лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
 * 
 * Официальный блог EasyCoding Team: https://www.easycoding.org/
 * Официальная страница проекта: https://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/

namespace srcrepair
{
    /// <summary>
    /// Служебный класс работы с настройками графики Type 3 игр.
    /// </summary>
    public class Type3Settings
    {
        /// <summary>
        /// Хранит имя настройки ID производителя видеокарты.
        /// </summary>
        public string VendorID { get; private set; }

        /// <summary>
        /// Хранит имя настройки ID видеокарты.
        /// </summary>
        public string DeviceID { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества обычных эффектов.
        /// </summary>
        public string EffectDetails { get; private set; }

        /// <summary>
        /// Хранит имя настройки выделенного игре пула памяти.
        /// </summary>
        public string MemoryPoolType { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества детализации моделей и текстур.
        /// </summary>
        public string TextureModelQuality { get; private set; }

        /// <summary>
        /// Хранит имя настройки разрешения по горизонтали.
        /// </summary>
        public string ScreenWidth { get; private set; }

        /// <summary>
        /// Хранит имя настройки разрешения по вертикали.
        /// </summary>
        public string ScreenHeight { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества шейдерных эффектов.
        /// </summary>
        public string ShaderEffects { get; private set; }

        /// <summary>
        /// Хранит имя настройки версии конфига настроек графики.
        /// </summary>
        public string SchemaVersion { get; private set; }

        /// <summary>
        /// Хранит имя настройки известности устройства.
        /// </summary>
        public string KnownDevice { get; private set; }

        /// <summary>
        /// Хранит имя настройки параметров полноэкранного режима.
        /// </summary>
        public string FullScreenMode { get; private set; }

        /// <summary>
        /// Хранит имя настройки параметров полноэкранного режима в
        /// кооперативном режиме.
        /// </summary>
        public string FullScreenModeCoop { get; private set; }

        /// <summary>
        /// Хранит имя настройки других параметров графического режима (без рамки).
        /// </summary>
        public string DisplayBorderless { get; private set; }

        /// <summary>
        /// Хранит имя настройки вертикальной синхронизации.
        /// </summary>
        public string VSync { get; private set; }

        /// <summary>
        /// Хранит имя настройки параметров полноэкранного режима при потере фокуса.
        /// </summary>
        public string DisplayModeFocusLoss { get; private set; }

        /// <summary>
        /// Хранит имя настройки параметров рендеринга водных поверхностей.
        /// </summary>
        public string CheapWaterReflections { get; private set; }

        /// <summary>
        /// Хранит имя настройки высоты и качества эффектов тумана.
        /// </summary>
        public string FogHeight { get; private set; }

        /// <summary>
        /// Хранит имя настройки, позволяющей использовать только простые
        /// источники освещения.
        /// </summary>
        public string SimpleLight { get; private set; }

        /// <summary>
        /// Хранит имя настройки SSAO для освещения.
        /// </summary>
        public string SSAOLights { get; private set; }

        /// <summary>
        /// Хранит имя настройки глобальной карты теней.
        /// </summary>
        public string GlobalShadowMode { get; private set; }

        /// <summary>
        /// Хранит имя настройки полноэкранного сглаживания.
        /// </summary>
        public string AntiAliasing { get; private set; }

        /// <summary>
        /// Хранит имя настройки глубины полноэкранного сглаживания.
        /// </summary>
        public string AntiAliasQuality { get; private set; }

        /// <summary>
        /// Хранит имя настройки портретной анимации.
        /// </summary>
        public string PortraitAnimation { get; private set; }

        /// <summary>
        /// Хранит имя настройки Specular Shading.
        /// </summary>
        public string SpecularShading { get; private set; }

        /// <summary>
        /// Хранит имя настройки Specular Bloom Shading.
        /// </summary>
        public string SpecularBloomShading { get; private set; }

        /// <summary>
        /// Хранит имя настройки рендеринга второстепенных предметов на карте.
        /// </summary>
        public string AmbientCreatures { get; private set; }

        /// <summary>
        /// Хранит имя настройки стриминга текстур.
        /// </summary>
        public string TextureStreaming { get; private set; }

        /// <summary>
        /// Хранит имя настройки карт нормалей.
        /// </summary>
        public string NormalMaps { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества детализации таблицы результатов.
        /// </summary>
        public string DashboardQuality { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества шейдерных эффектов.
        /// </summary>
        public string ShaderQuality { get; private set; }

        /// <summary>
        /// Хранит имя настройки рекомендованного разрешения по вертикали.
        /// </summary>
        public string RecommendedHeight { get; private set; }

        /// <summary>
        /// Хранит имя настройки области рендеринга.
        /// </summary>
        public string ViewportScale { get; private set; }

        /// <summary>
        /// Хранит имя настройки контрастности и цветовой гаммы.
        /// </summary>
        public string Brightness { get; private set; }

        /// <summary>
        /// Хранит имя настройки активации расширенных опций видео.
        /// </summary>
        public string UseAdvanced { get; private set; }

        /// <summary>
        /// Хранит имя настройки рендеринга ambient cloth.
        /// </summary>
        public string AmbientCloth { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества детализации травы.
        /// </summary>
        public string GrassQuality { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества детализации эффекта "ветер".
        /// </summary>
        public string WindEffects { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества эффектов Parallax Mapping.
        /// </summary>
        public string ParallaxMapping { get; private set; }

        /// <summary>
        /// Хранит имя настройки particle fallback.
        /// </summary>
        public string ParticleFallback { get; private set; }

        /// <summary>
        /// Хранит имя настройки множителя particle fallback.
        /// </summary>
        public string ParticleFallbackMultiplier { get; private set; }

        /// <summary>
        /// Хранит имя настройки сброса графических настроек.
        /// </summary>
        public string ResetToDefaults { get; private set; }

        /// <summary>
        /// Хранит имя настройки соотношения сторон.
        /// </summary>
        public string ScreenRatio { get; private set; }

        /// <summary>
        /// Заполняет свойства класса настройками для базы версии 3 (Source 2, DotA 2: Reborn).
        /// </summary>
        private void SetSettingsV3()
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
        /// Базовый конструктор класса.
        /// </summary>
        public Type3Settings()
        {
            // Заполняем свойства класса...
            SetSettingsV3();
        }
    }
}
