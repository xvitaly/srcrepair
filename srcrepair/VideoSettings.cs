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
        protected int ScreenWidth = 800;
        protected int ScreenHeight = 600;

        public int GetScreenWidth()
        {
            return ScreenWidth;
        }

        public int GetScreenHeight()
        {
            return ScreenHeight;
        }
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

        public int GetDisplayMode()
        {
            int res = -1;

            switch (DisplayMode)
            {
                case 0: res = 0;
                    break;
                case 1: res = 1;
                    break;
            }

            return res;
        }

        public int GetModelQuality()
        {
            int res = -1;

            switch (ModelDetail)
            {
                case 0: res = 2;
                    break;
                case 1: res = 1;
                    break;
                case 2: res = 0;
                    break;
            }

            return res;
        }

        public int GetTextureQuality()
        {
            int res = -1;

            switch (TextureDetail)
            {
                case -1: res = 3;
                    break;
                case 0: res = 2;
                    break;
                case 1: res = 1;
                    break;
                case 2: res = 0;
                    break;
            }

            return res;
        }

        public int GetShaderQuality()
        {
            int res = -1;

            switch (ShaderDetail)
            {
                case 0: res = 1;
                    break;
                case 1: res = 0;
                    break;
            }

            return res;
        }

        public int GetReflectionsQuality()
        {
            int res = -1;

            switch (WaterDetail)
            {
                case 0: res = 0;
                    break;
                case 1:
                    switch (WaterReflections)
                    {
                        case 0: res = 1;
                            break;
                        case 1: res = 2;
                            break;
                    }
                    break;
            }

            return res;
        }

        public int GetShadowQuality()
        {
            int res = -1;

            switch (ShadowDetail)
            {
                case 0: res = 0;
                    break;
                case 1: res = 1;
                    break;
            }

            return res;
        }

        public int GetColorCorrection()
        {
            int res = -1;

            switch (ColorCorrection)
            {
                case 0: res = 0;
                    break;
                case 1: res = 1;
                    break;
            }

            return res;
        }

        public int GetAntiAliasing()
        {
            int res = -1;

            switch (AntiAliasing)
            {
                case 0: res = 0;
                    break;
                case 1: res = 0;
                    break;
                case 2: res = 1;
                    break;
                case 4:
                    switch (AntiAliasQuality)
                    {
                        case 0: res = 2;
                            break;
                        case 2: res = 3;
                            break;
                        case 4: res = 4;
                            break;
                    }
                    break;
                case 8:
                    switch (AntiAliasQuality)
                    {
                        case 0: res = 5;
                            break;
                        case 2: res = 6;
                            break;
                    }
                    break;
            }

            return res;
        }

        public int GetFilteringMode()
        {
            int res = -1;

            switch (FilteringMode)
            {
                case 1:
                    switch (FilteringTrilinear)
                    {
                        case 0: res = 0;
                            break;
                        case 1: res = 1;
                            break;
                    }
                    break;
                case 2: res = 2;
                    break;
                case 4: res = 3;
                    break;
                case 8: res = 4;
                    break;
                case 16: res = 5;
                    break;
            }

            return res;
        }

        public int GetVSync()
        {
            int res = -1;

            switch (VSync)
            {
                case 0: res = 0;
                    break;
                case 1: res = 1;
                    break;
            }

            return res;
        }

        public int GetMotionBlur()
        {
            int res = -1;

            switch (MotionBlur)
            {
                case 0: res = 0;
                    break;
                case 1: res = 1;
                    break;
            }

            return res;
        }

        public int GetDirectXMode()
        {
            int res = -1;

            switch (DirectXMode)
            {
                case 80: res = 0;
                    break;
                case 81: res = 1;
                    break;
                case 90: res = 2;
                    break;
                case 95: res = 3;
                    break;
            }

            return res;
        }

        public int GetHDRType()
        {
            int res = -1;

            switch (HDRMode)
            {
                case 0: res = 0;
                    break;
                case 1: res = 1;
                    break;
                case 2: res = 2;
                    break;
            }

            return res;
        }

        public GCFVideo(string SAppName)
        {
            // Открываем ключ реестра для чтения...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(Path.Combine("Software", "Valve", "Source", SAppName, "Settings"), false);

            // Проверяем открылся ли ключ...
            if (ResKey != null)
            {
                // Получаем значение разрешения по горизонтали...
                try { ScreenWidth = Convert.ToInt32(ResKey.GetValue("ScreenWidth")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем значение разрешения по вертикали...
                try { ScreenHeight = Convert.ToInt32(ResKey.GetValue("ScreenHeight")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем режим окна (ScreenWindowed): 1-window, 0-fullscreen...
                try { DisplayMode = Convert.ToInt32(ResKey.GetValue("ScreenWindowed")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем детализацию моделей (r_rootlod): 0-high, 1-med, 2-low...
                try { ModelDetail = Convert.ToInt32(ResKey.GetValue("r_rootlod")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем детализацию текстур (mat_picmip): 0-high, 1-med, 2-low...
                try { TextureDetail = Convert.ToInt32(ResKey.GetValue("mat_picmip")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки шейдеров (mat_reducefillrate): 0-high, 1-low...
                try { ShaderDetail = Convert.ToInt32(ResKey.GetValue("mat_reducefillrate")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Начинаем работать над отражениями (здесь сложнее)...
                try { WaterDetail = Convert.ToInt32(ResKey.GetValue("r_waterforceexpensive")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                try { WaterReflections = Convert.ToInt32(ResKey.GetValue("r_waterforcereflectentities")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки теней (r_shadowrendertotexture): 0-low, 1-high...
                try { ShadowDetail = Convert.ToInt32(ResKey.GetValue("r_shadowrendertotexture")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки коррекции цвета (mat_colorcorrection): 0-off, 1-on...
                try { ColorCorrection = Convert.ToInt32(ResKey.GetValue("mat_colorcorrection")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки сглаживания (mat_antialias): 1-off, 2-2x, 4-4x, etc...
                // 2x MSAA - 2:0; 4xMSAA - 4:0; 8xCSAA - 4:2; 16xCSAA - 4:4; 8xMSAA - 8:0; 16xQ CSAA - 8:2.
                try { AntiAliasing = Convert.ToInt32(ResKey.GetValue("mat_antialias")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }                
                try { AntiAliasQuality = Convert.ToInt32(ResKey.GetValue("mat_aaquality")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки анизотропии (mat_forceaniso): 1-off, etc...
                try { FilteringMode = Convert.ToInt32(ResKey.GetValue("mat_forceaniso")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                try { FilteringTrilinear = Convert.ToInt32(ResKey.GetValue("mat_trilinear")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки вертикальной синхронизации (mat_vsync): 0-off, 1-on...
                try { VSync = Convert.ToInt32(ResKey.GetValue("mat_vsync")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки размытия движения (MotionBlur): 0-off, 1-on...
                try { MotionBlur = Convert.ToInt32(ResKey.GetValue("MotionBlur")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки режима рендера (DXLevel_V1):
                // 80-DirectX 8.0; 81-DirectX 8.1; 90-DirectX 9.0; 95-DirectX 9.0c...
                try { DirectXMode = Convert.ToInt32(ResKey.GetValue("DXLevel_V1")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки HDR (mat_hdr_level): 0-off, 1-bloom, 2-full...
                try { HDRMode = Convert.ToInt32(ResKey.GetValue("mat_hdr_level")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

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
