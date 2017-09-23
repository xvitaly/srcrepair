/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2017 EasyCoding Team.
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
namespace srcrepair
{
    /// <summary>
    /// Служебный класс работы с настройками графики Type 2 игр.
    /// </summary>
    public class Type2Settings
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
        public string ScreenRatio { get; private set; }

        /// <summary>
        /// Хранит имя настройки контрастности и цветовой гаммы.
        /// </summary>
        public string Brightness { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества теней.
        /// </summary>
        public string ShadowQuality { get; private set; }

        /// <summary>
        /// Хранит имя настройки размытия движений.
        /// </summary>
        public string MotionBlur { get; private set; }

        /// <summary>
        /// Хранит имя настройки параметров графического режима.
        /// </summary>
        public string DisplayMode { get; private set; }

        /// <summary>
        /// Хранит имя настройки других параметров графического режима (без рамки).
        /// </summary>
        public string DisplayBorderless { get; private set; }

        /// <summary>
        /// Хранит имя настройки полноэкранного сглаживания.
        /// </summary>
        public string AntiAliasing { get; private set; }

        /// <summary>
        /// Хранит имя настройки глубины полноэкранного сглаживания.
        /// </summary>
        public string AntiAliasQuality { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества фильтрации текстур.
        /// </summary>
        public string FilteringMode { get; private set; }

        /// <summary>
        /// Хранит имя настройки вертикальной синхронизации.
        /// </summary>
        public string VSync { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества вертикальной синхронизации.
        /// </summary>
        public string VSyncMode { get; private set; }

        /// <summary>
        /// Хранит имя настройки многоядерного рендеринга.
        /// </summary>
        public string MCRendering { get; private set; }

        /// <summary>
        /// Хранит имя настройки качества шейдерных эффектов.
        /// </summary>
        public string ShaderEffects { get; private set; }

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
        /// Хранит имя настройки зернистости.
        /// </summary>
        public string GrainScaleOverride { get; private set; }

        /// <summary>
        /// Заполняет свойства класса настройками для базы версии 2 (Source 1; до CS:GO включительно).
        /// </summary>
        private void SetSettingsV2()
        {
            ScreenWidth = "setting.defaultres";
            ScreenHeight = "setting.defaultresheight";
            ScreenRatio = "setting.aspectratiomode";
            Brightness = "setting.mat_monitorgamma";
            ShadowQuality = "setting.csm_quality_level";
            MotionBlur = "setting.mat_motion_blur_enabled";
            DisplayMode = "setting.fullscreen";
            DisplayBorderless = "setting.nowindowborder";
            AntiAliasing = "setting.mat_antialias";
            AntiAliasQuality = "setting.mat_aaquality";
            FilteringMode = "setting.mat_forceaniso";
            VSync = "setting.mat_vsync";
            VSyncMode = "setting.mat_triplebuffered";
            MCRendering = "setting.mat_queue_mode";
            ShaderEffects = "setting.gpu_level";
            EffectDetails = "setting.cpu_level";
            MemoryPoolType = "setting.mem_level";
            TextureModelQuality = "setting.gpu_mem_level";
            GrainScaleOverride = "setting.mat_grain_scale_override";
        }

        /// <summary>
        /// Базовый конструктор класса.
        /// </summary>
        /// <param name="ID">Тип механизма хранения настроек движка Source</param>
        public Type2Settings(string ID)
        {
            // Временно заполним без указания ID...
            SetSettingsV2();
        }
    }
}
