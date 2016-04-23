/*
 * Классы, связанные с работой с графическими настройками.
 * 
 * Copyright 2011 - 2016 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2016 EasyCoding Team.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace srcrepair
{
    public class VideoSettings
    {
        protected bool GameType;

        protected int ScreenWidth;
        protected int ScreenHeight;
    }

    public class NCFVideo : VideoSettings
    {
    }

    public class GCFVideo: VideoSettings
    {
        private int DisplayMode;
        private int ModelDetail;
        private int TextureDetail;
        private int ShaderDetail;
        private int WaterDetail;
        private int WaterReflections;
        private int ShadowDetail;
        private int ColorCorrection;
        private int AntiAliasing;
        private int AntiAliasQuality;
        private int FilteringMode;
        private int FilteringTrilinear;
        private int VSync;
        private int MotionBlur;
        private int DirectXMode;
        private int HDRMode;

        public GCFVideo(string SAppName)
        {
            // Открываем ключ реестра для чтения...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Valve", "Source", SAppName, "Settings"), false);

            // Проверяем открылся ли ключ...
            if (ResKey != null)
            {
                // Получаем значение разрешения по горизонтали...
                ScreenWidth = Convert.ToInt32(ResKey.GetValue("ScreenWidth"));

                // Получаем значение разрешения по вертикали...
                ScreenHeight = Convert.ToInt32(ResKey.GetValue("ScreenHeight"));

                // Получаем режим окна (ScreenWindowed): 1-window, 0-fullscreen...
                DisplayMode = Convert.ToInt32(ResKey.GetValue("ScreenWindowed"));

                // Получаем детализацию моделей (r_rootlod): 0-high, 1-med, 2-low...
                ModelDetail = Convert.ToInt32(ResKey.GetValue("r_rootlod"));

                // Получаем детализацию текстур (mat_picmip): 0-high, 1-med, 2-low...
                TextureDetail = Convert.ToInt32(ResKey.GetValue("mat_picmip"));

                // Получаем настройки шейдеров (mat_reducefillrate): 0-high, 1-low...
                ShaderDetail = Convert.ToInt32(ResKey.GetValue("mat_reducefillrate"));

                // Начинаем работать над отражениями (здесь сложнее)...
                WaterDetail = Convert.ToInt32(ResKey.GetValue("r_waterforceexpensive"));
                WaterReflections = Convert.ToInt32(ResKey.GetValue("r_waterforcereflectentities"));

                // Получаем настройки теней (r_shadowrendertotexture): 0-low, 1-high...
                ShadowDetail = Convert.ToInt32(ResKey.GetValue("r_shadowrendertotexture"));

                // Получаем настройки коррекции цвета (mat_colorcorrection): 0-off, 1-on...
                ColorCorrection = Convert.ToInt32(ResKey.GetValue("mat_colorcorrection"));

                // Получаем настройки сглаживания (mat_antialias): 1-off, 2-2x, 4-4x, etc...
                // 2x MSAA - 2:0; 4xMSAA - 4:0; 8xCSAA - 4:2; 16xCSAA - 4:4; 8xMSAA - 8:0; 16xQ CSAA - 8:2.
                AntiAliasing = Convert.ToInt32(ResKey.GetValue("mat_antialias"));
                AntiAliasQuality = Convert.ToInt32(ResKey.GetValue("mat_aaquality"));

                // Получаем настройки анизотропии (mat_forceaniso): 1-off, etc...
                FilteringMode = Convert.ToInt32(ResKey.GetValue("mat_forceaniso"));
                FilteringTrilinear = Convert.ToInt32(ResKey.GetValue("mat_trilinear"));

                // Получаем настройки вертикальной синхронизации (mat_vsync): 0-off, 1-on...
                VSync = Convert.ToInt32(ResKey.GetValue("mat_vsync"));

                // Получаем настройки размытия движения (MotionBlur): 0-off, 1-on...
                MotionBlur = Convert.ToInt32(ResKey.GetValue("MotionBlur"));

                // Получаем настройки режима рендера (DXLevel_V1):
                // 80-DirectX 8.0; 81-DirectX 8.1; 90-DirectX 9.0; 95-DirectX 9.0c...
                DirectXMode = Convert.ToInt32(ResKey.GetValue("DXLevel_V1"));

                // Получаем настройки HDR (mat_hdr_level): 0-off, 1-bloom, 2-full...
                HDRMode = Convert.ToInt32(ResKey.GetValue("mat_hdr_level"));

                // Закрываем ключ реестра...
                ResKey.Close();
            }
            else
            {
                // Произошла ошибка при открытии ключа реестра. Выбросим исключение...
                throw new Exception(AppStrings.GT_RegOpenErr);
            }
        }
    }
}
