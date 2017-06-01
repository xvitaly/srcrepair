/*
 * Служебный класс работы с настройками графики Type 1 игр.
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
 * Официальный блог EasyCoding Team: http://www.easycoding.org/
 * Официальная страница проекта: http://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/

namespace srcrepair
{
    /// <summary>
    /// Служебный класс работы с настройками графики Type 1 игр.
    /// </summary>
    public class Type1Settings
    {
        /// <summary>
        /// Хранит имя настройки разрешения по горизонтали.
        /// </summary>
        public string ScreenWidth { get; private set; }

        /// <summary>
        /// Хранит имя настройки разрешения по вертикали.
        /// </summary>
        public string ScreenHeight { get; private set; }

        /// <summary>
        /// Хранит имя настройки соотношения сторон.
        /// </summary>
        public string DisplayMode { get; private set; }

        /// <summary>
        /// Хранит имя настройки детализации моделей.
        /// </summary>
        public string ModelDetail { get; private set; }

        /// <summary>
        /// Хранит имя настройки детализации текстур.
        /// </summary>
        public string TextureDetail { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества шейдерных эффектов.
        /// </summary>
        public string ShaderDetail { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества прорисовки воды.
        /// </summary>
        public string WaterDetail { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества отражений в воде.
        /// </summary>
        public string WaterReflections { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества теней.
        /// </summary>
        public string ShadowDetail { get; private set; }

        /// <summary>
        /// Хранит имя настройки коррекции цвета.
        /// </summary>
        public string ColorCorrection { get; private set; }

        /// <summary>
        /// Хранит имя настройки настроек полноэкранного сглаживания.
        /// </summary>
        public string AntiAliasing { get; private set; }

        /// <summary>
        /// Хранит имя настройки настроек полноэкранного сглаживания (MSAA).
        /// </summary>
        public string AntiAliasingMSAA { get; private set; }

        /// <summary>
        /// Хранит имя настройки глубины полноэкранного сглаживания.
        /// </summary>
        public string AntiAliasQuality { get; private set; }

        /// <summary>
        /// Хранит имя настройки глубины полноэкранного сглаживания (MSAA).
        /// </summary>
        public string AntiAliasQualityMSAA { get; private set; }

        /// <summary>
        /// Хранит имя настройки анизотропной фильтрации текстур.
        /// </summary>
        public string FilteringMode { get; private set; }

        /// <summary>
        /// Хранит имя настройки трилинейной фильтрации текстур.
        /// </summary>
        public string FilteringTrilinear { get; private set; }

        /// <summary>
        /// Хранит имя настройки вертикальной синхронизации.
        /// </summary>
        public string VSync { get; private set; }

        /// <summary>
        /// Хранит имя настройки размытия движения.
        /// </summary>
        public string MotionBlur { get; private set; }

        /// <summary>
        /// Хранит имя настройки режима DirectX.
        /// </summary>
        public string DirectXMode { get; private set; }

        /// <summary>
        /// Хранит имя настройки HDR.
        /// </summary>
        public string HDRMode { get; private set; }

        /// <summary>
        /// Заполняет свойства класса настройками для базы версии 1 (Source 1; хранение в реестре).
        /// </summary>
        private void SetSettingsV1()
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
        /// Базовый конструктор класса.
        /// </summary>
        public Type1Settings()
        {
            // Заполняем значения...
            SetSettingsV1();
        }
    }
}
