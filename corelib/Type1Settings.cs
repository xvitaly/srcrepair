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
    /// Служебный класс работы с настройками графики Type 1 игр.
    /// </summary>
    public class Type1Settings : IType1Settings
    {
        /// <summary>
        /// Хранит имя настройки разрешения по горизонтали.
        /// </summary>
        public string ScreenWidth { get; protected set; }

        /// <summary>
        /// Хранит имя настройки разрешения по вертикали.
        /// </summary>
        public string ScreenHeight { get; protected set; }

        /// <summary>
        /// Хранит имя настройки соотношения сторон.
        /// </summary>
        public string DisplayMode { get; protected set; }

        /// <summary>
        /// Хранит имя настройки детализации моделей.
        /// </summary>
        public string ModelDetail { get; protected set; }

        /// <summary>
        /// Хранит имя настройки детализации текстур.
        /// </summary>
        public string TextureDetail { get; protected set; }

        /// <summary>
        /// Хранит имя настройки качества шейдерных эффектов.
        /// </summary>
        public string ShaderDetail { get; protected set; }

        /// <summary>
        /// Хранит имя настройки качества прорисовки воды.
        /// </summary>
        public string WaterDetail { get; protected set; }

        /// <summary>
        /// Хранит имя настройки качества отражений в воде.
        /// </summary>
        public string WaterReflections { get; protected set; }

        /// <summary>
        /// Хранит имя настройки качества теней.
        /// </summary>
        public string ShadowDetail { get; protected set; }

        /// <summary>
        /// Хранит имя настройки коррекции цвета.
        /// </summary>
        public string ColorCorrection { get; protected set; }

        /// <summary>
        /// Хранит имя настройки настроек полноэкранного сглаживания.
        /// </summary>
        public string AntiAliasing { get; protected set; }

        /// <summary>
        /// Хранит имя настройки настроек полноэкранного сглаживания (MSAA).
        /// </summary>
        public string AntiAliasingMSAA { get; protected set; }

        /// <summary>
        /// Хранит имя настройки глубины полноэкранного сглаживания.
        /// </summary>
        public string AntiAliasQuality { get; protected set; }

        /// <summary>
        /// Хранит имя настройки глубины полноэкранного сглаживания (MSAA).
        /// </summary>
        public string AntiAliasQualityMSAA { get; protected set; }

        /// <summary>
        /// Хранит имя настройки анизотропной фильтрации текстур.
        /// </summary>
        public string FilteringMode { get; protected set; }

        /// <summary>
        /// Хранит имя настройки трилинейной фильтрации текстур.
        /// </summary>
        public string FilteringTrilinear { get; protected set; }

        /// <summary>
        /// Хранит имя настройки вертикальной синхронизации.
        /// </summary>
        public string VSync { get; protected set; }

        /// <summary>
        /// Хранит имя настройки размытия движения.
        /// </summary>
        public string MotionBlur { get; protected set; }

        /// <summary>
        /// Хранит имя настройки режима DirectX.
        /// </summary>
        public string DirectXMode { get; protected set; }

        /// <summary>
        /// Хранит имя настройки HDR.
        /// </summary>
        public string HDRMode { get; protected set; }

        /// <summary>
        /// Заполняет свойства класса настройками для базы версии 1 (Source 1; хранение в реестре).
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
        /// Базовый конструктор класса.
        /// </summary>
        public Type1Settings()
        {
            // Заполняем значения...
            SetSettingsV1();
        }
    }
}
