/*
 * Класс, предназначенный для работы с графическими настройками GCF игр.
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
using System.IO;
using Microsoft.Win32;

namespace srcrepair
{
    /// <summary>
    /// Управляет графическими настройками GCF приложений.
    /// </summary>
    public class GCFVideo : VideoSettings
    {
        /// <summary>
        /// Хранит значеение режима окна: ScreenWidth.
        /// </summary>
        protected int _DisplayMode;

        /// <summary>
        /// Хранит значение детализации моделей: r_rootlod.
        /// </summary>
        protected int _ModelDetail;

        /// <summary>
        /// Хранит значение детализации текстур: mat_picmip.
        /// </summary>
        protected int _TextureDetail;

        /// <summary>
        /// Хранит значение качества шейдерных эффектов: mat_reducefillrate.
        /// </summary>
        protected int _ShaderDetail;

        /// <summary>
        /// Хранит значение качества отражений в воде: r_waterforceexpensive.
        /// </summary>
        protected int _WaterDetail;

        /// <summary>
        /// Хранит значение качества отражений в воде: r_waterforcereflectentities.
        /// </summary>
        protected int _WaterReflections;

        /// <summary>
        /// Хранит значение качества теней: r_shadowrendertotexture.
        /// </summary>
        protected int _ShadowDetail;

        /// <summary>
        /// Хранит значение настроек коррекции цвета: mat_colorcorrection.
        /// </summary>
        protected int _ColorCorrection;

        /// <summary>
        /// Хранит значение настроек полноэкранного сглаживания: mat_antialias.
        /// </summary>
        protected int _AntiAliasing;

        /// <summary>
        /// Хранит значение глубины полноэкранного сглаживания: mat_aaquality.
        /// </summary>
        protected int _AntiAliasQuality;

        /// <summary>
        /// Хранит значение настроек анизотропной фильтрации текстур: mat_forceaniso.
        /// </summary>
        protected int _FilteringMode;

        /// <summary>
        /// Хранит значение настроек трилинейной фильтрации текстур: mat_trilinear.
        /// </summary>
        protected int _FilteringTrilinear;

        /// <summary>
        /// Хранит значение настроек вертикальной синхронизации: mat_vsync.
        /// </summary>
        protected int _VSync;

        /// <summary>
        /// Хранит значение настроек размытия движения: MotionBlur.
        /// </summary>
        protected int _MotionBlur;

        /// <summary>
        /// Хранит значение настроек режима DirectX: DXLevel_V1.
        /// </summary>
        protected int _DirectXMode;

        /// <summary>
        /// Хранит значение настроек HDR: mat_hdr_level.
        /// </summary>
        protected int _HDRMode;

        /// <summary>
        /// Хранит путь к ветке реестра с графическими настройками игры.
        /// </summary>
        protected string RegKey;

        /// <summary>
        /// Возвращает / задаёт значение режима отображения.
        /// </summary>
        public int DisplayMode
        {
            get
            {
                int res = -1;

                switch (_DisplayMode)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _DisplayMode = 0;
                        break;
                    case 1:
                        _DisplayMode = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт значение качества моделей.
        /// </summary>
        public int ModelQuality
        {
            get
            {
                int res = -1;

                switch (_ModelDetail)
                {
                    case 0:
                        res = 2;
                        break;
                    case 1:
                        res = 1;
                        break;
                    case 2:
                        res = 0;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _ModelDetail = 2;
                        break;
                    case 1:
                        _ModelDetail = 1;
                        break;
                    case 2:
                        _ModelDetail = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт значение качества текстур.
        /// </summary>
        public int TextureQuality
        {
            get
            {
                int res = -1;

                switch (_TextureDetail)
                {
                    case -1:
                        res = 3;
                        break;
                    case 0:
                        res = 2;
                        break;
                    case 1:
                        res = 1;
                        break;
                    case 2:
                        res = 0;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _TextureDetail = 2;
                        break;
                    case 1:
                        _TextureDetail = 1;
                        break;
                    case 2:
                        _TextureDetail = 0;
                        break;
                    case 3:
                        _TextureDetail = -1;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт значение качества шейдерных эффектов.
        /// </summary>
        public int ShaderQuality
        {
            get
            {
                int res = -1;

                switch (_ShaderDetail)
                {
                    case 0:
                        res = 1;
                        break;
                    case 1:
                        res = 0;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _ShaderDetail = 1;
                        break;
                    case 1:
                        _ShaderDetail = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт значение качества отражений в воде.
        /// </summary>
        public int ReflectionsQuality
        {
            get
            {
                int res = -1;

                switch (_WaterDetail)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        switch (_WaterReflections)
                        {
                            case 0:
                                res = 1;
                                break;
                            case 1:
                                res = 2;
                                break;
                        }
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _WaterDetail = 0;
                        _WaterReflections = 0;
                        break;
                    case 1:
                        _WaterDetail = 1;
                        _WaterReflections = 0;
                        break;
                    case 2:
                        _WaterDetail = 1;
                        _WaterReflections = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт значение качества теней.
        /// </summary>
        public int ShadowQuality
        {
            get
            {
                int res = -1;

                switch (_ShadowDetail)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _ShadowDetail = 0;
                        break;
                    case 1:
                        _ShadowDetail = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт значение качества коррекции цвета.
        /// </summary>
        public int ColorCorrection
        {
            get
            {
                int res = -1;

                switch (_ColorCorrection)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _ColorCorrection = 0;
                        break;
                    case 1:
                        _ColorCorrection = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт значение настроек полноэкранного сглаживания.
        /// </summary>
        public int AntiAliasing
        {
            get
            {
                int res = -1;

                switch (_AntiAliasing)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 0;
                        break;
                    case 2:
                        res = 1;
                        break;
                    case 4:
                        switch (_AntiAliasQuality)
                        {
                            case 0:
                                res = 2;
                                break;
                            case 2:
                                res = 3;
                                break;
                            case 4:
                                res = 4;
                                break;
                        }
                        break;
                    case 8:
                        switch (_AntiAliasQuality)
                        {
                            case 0:
                                res = 5;
                                break;
                            case 2:
                                res = 6;
                                break;
                        }
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0: // Нет сглаживания
                        _AntiAliasing = 1;
                        _AntiAliasQuality = 0;
                        break;
                    case 1: // 2x MSAA
                        _AntiAliasing = 2;
                        _AntiAliasQuality = 0;
                        break;
                    case 2: // 4x MSAA
                        _AntiAliasing = 4;
                        _AntiAliasQuality = 0;
                        break;
                    case 3: // 8x CSAA
                        _AntiAliasing = 4;
                        _AntiAliasQuality = 2;
                        break;
                    case 4: // 16x CSAA
                        _AntiAliasing = 4;
                        _AntiAliasQuality = 4;
                        break;
                    case 5: // 8x MSAA
                        _AntiAliasing = 8;
                        _AntiAliasQuality = 0;
                        break;
                    case 6: // 16xQ CSAA
                        _AntiAliasing = 8;
                        _AntiAliasQuality = 2;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт значение настроек фильтрации текстур.
        /// </summary>
        public int FilteringMode
        {
            get
            {
                int res = -1;

                switch (_FilteringMode)
                {
                    case 1:
                        switch (_FilteringTrilinear)
                        {
                            case 0:
                                res = 0;
                                break;
                            case 1:
                                res = 1;
                                break;
                        }
                        break;
                    case 2:
                        res = 2;
                        break;
                    case 4:
                        res = 3;
                        break;
                    case 8:
                        res = 4;
                        break;
                    case 16:
                        res = 5;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0: // Билинейная
                        _FilteringMode = 1;
                        _FilteringTrilinear = 0;
                        break;
                    case 1: // Трилинейная
                        _FilteringMode = 1;
                        _FilteringTrilinear = 1;
                        break;
                    case 2: // Анизотропная 2x
                        _FilteringMode = 2;
                        _FilteringTrilinear = 0;
                        break;
                    case 3: // Анизотропная 4x
                        _FilteringMode = 4;
                        _FilteringTrilinear = 0;
                        break;
                    case 4: // Анизотропная 8x
                        _FilteringMode = 8;
                        _FilteringTrilinear = 0;
                        break;
                    case 5: // Анизотропная 16x
                        _FilteringMode = 16;
                        _FilteringTrilinear = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт значение настроек вертикальной синхронизации.
        /// </summary>
        public int VSync
        {
            get
            {
                int res = -1;

                switch (_VSync)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _VSync = 0;
                        break;
                    case 1:
                        _VSync = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт значение настроек размытия движения.
        /// </summary>
        public int MotionBlur
        {
            get
            {
                int res = -1;

                switch (_MotionBlur)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _MotionBlur = 0;
                        break;
                    case 1:
                        _MotionBlur = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт значение настроек режима DirectX.
        /// </summary>
        public int DirectXMode
        {
            get
            {
                int res = -1;

                switch (_DirectXMode)
                {
                    case 80:
                        res = 0;
                        break;
                    case 81:
                        res = 1;
                        break;
                    case 90:
                        res = 2;
                        break;
                    case 95:
                        res = 3;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _DirectXMode = 80;
                        break;
                    case 1:
                        _DirectXMode = 81;
                        break;
                    case 2:
                        _DirectXMode = 90;
                        break;
                    case 3:
                        _DirectXMode = 95;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт значение настроек HDR.
        /// </summary>
        public int HDRType
        {
            get
            {
                int res = -1;

                switch (_HDRMode)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                    case 2:
                        res = 2;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _HDRMode = 0;
                        break;
                    case 1:
                        _HDRMode = 1;
                        break;
                    case 2:
                        _HDRMode = 2;
                        break;
                }
            }
        }

        /// <summary>
        /// Считывает графические настройки игры из реестра.
        /// </summary>
        private void ReadSettings()
        {
            // Открываем ключ реестра для чтения...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(RegKey, false);

            // Проверяем открылся ли ключ...
            if (ResKey != null)
            {
                // Получаем значение разрешения по горизонтали...
                try { _ScreenWidth = Convert.ToInt32(ResKey.GetValue("ScreenWidth")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем значение разрешения по вертикали...
                try { _ScreenHeight = Convert.ToInt32(ResKey.GetValue("ScreenHeight")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем режим окна (ScreenWindowed): 1-window, 0-fullscreen...
                try { _DisplayMode = Convert.ToInt32(ResKey.GetValue("ScreenWindowed")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем детализацию моделей (r_rootlod): 0-high, 1-med, 2-low...
                try { _ModelDetail = Convert.ToInt32(ResKey.GetValue("r_rootlod")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем детализацию текстур (mat_picmip): 0-high, 1-med, 2-low...
                try { _TextureDetail = Convert.ToInt32(ResKey.GetValue("mat_picmip")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки шейдеров (mat_reducefillrate): 0-high, 1-low...
                try { _ShaderDetail = Convert.ToInt32(ResKey.GetValue("mat_reducefillrate")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Начинаем работать над отражениями (здесь сложнее)...
                try { _WaterDetail = Convert.ToInt32(ResKey.GetValue("r_waterforceexpensive")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                try { _WaterReflections = Convert.ToInt32(ResKey.GetValue("r_waterforcereflectentities")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки теней (r_shadowrendertotexture): 0-low, 1-high...
                try { _ShadowDetail = Convert.ToInt32(ResKey.GetValue("r_shadowrendertotexture")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки коррекции цвета (mat_colorcorrection): 0-off, 1-on...
                try { _ColorCorrection = Convert.ToInt32(ResKey.GetValue("mat_colorcorrection")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки сглаживания (mat_antialias): 1-off, 2-2x, 4-4x, etc...
                // 2x MSAA - 2:0; 4xMSAA - 4:0; 8xCSAA - 4:2; 16xCSAA - 4:4; 8xMSAA - 8:0; 16xQ CSAA - 8:2.
                try { _AntiAliasing = Convert.ToInt32(ResKey.GetValue("mat_antialias")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                try { _AntiAliasQuality = Convert.ToInt32(ResKey.GetValue("mat_aaquality")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки анизотропии (mat_forceaniso): 1-off, etc...
                try { _FilteringMode = Convert.ToInt32(ResKey.GetValue("mat_forceaniso")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                try { _FilteringTrilinear = Convert.ToInt32(ResKey.GetValue("mat_trilinear")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки вертикальной синхронизации (mat_vsync): 0-off, 1-on...
                try { _VSync = Convert.ToInt32(ResKey.GetValue("mat_vsync")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки размытия движения (MotionBlur): 0-off, 1-on...
                try { _MotionBlur = Convert.ToInt32(ResKey.GetValue("MotionBlur")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки режима рендера (DXLevel_V1):
                // 80-DirectX 8.0; 81-DirectX 8.1; 90-DirectX 9.0; 95-DirectX 9.0c...
                try { _DirectXMode = Convert.ToInt32(ResKey.GetValue("DXLevel_V1")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Получаем настройки HDR (mat_hdr_level): 0-off, 1-bloom, 2-full...
                try { _HDRMode = Convert.ToInt32(ResKey.GetValue("mat_hdr_level")); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

                // Закрываем ключ реестра...
                ResKey.Close();
            }
            else
            {
                // Произошла ошибка при открытии ключа реестра. Выбросим исключение...
                throw new Exception(AppStrings.GT_RegOpenErr);
            }
        }

        /// <summary>
        /// Сохраняет графические настройки игры в реестр.
        /// </summary>
        public void WriteSettings()
        {
            // Открываеам ключ реестра для записи...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(RegKey, true);

            // Запишем в реестр настройки разрешения экрана...
            try { ResKey.SetValue("ScreenWidth", _ScreenWidth, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
            try { ResKey.SetValue("ScreenHeight", _ScreenHeight, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки режима запуска приложения (ScreenWindowed)...
            try { ResKey.SetValue("ScreenWindowed", _DisplayMode, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки детализации моделей...
            try { ResKey.SetValue("r_rootlod", _ModelDetail, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки детализации текстур...
            try { ResKey.SetValue("mat_picmip", _TextureDetail, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки качества шейдерных эффектов...
            try { ResKey.SetValue("mat_reducefillrate", _ShaderDetail, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки отражений в воде...
            try { ResKey.SetValue("r_waterforceexpensive", _WaterDetail, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
            try { ResKey.SetValue("r_waterforcereflectentities", _WaterReflections, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки прорисовки теней...
            try { ResKey.SetValue("r_shadowrendertotexture", _ShadowDetail, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки коррекции цвета...
            try { ResKey.SetValue("mat_colorcorrection", _ColorCorrection, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки сглаживания...
            try { ResKey.SetValue("mat_antialias", _AntiAliasing, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
            try { ResKey.SetValue("mat_aaquality", _AntiAliasQuality, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
            try { ResKey.SetValue("ScreenMSAA", _AntiAliasing, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
            try { ResKey.SetValue("ScreenMSAAQuality", _AntiAliasQuality, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки фильтрации...
            try { ResKey.SetValue("mat_forceaniso", _FilteringMode, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки вертикальной синхронизации...
            try { ResKey.SetValue("mat_vsync", _VSync, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки размытия движения...
            try { ResKey.SetValue("MotionBlur", _MotionBlur, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки режима DirectX...
            try { ResKey.SetValue("DXLevel_V1", _DirectXMode, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Запишем в реестр настройки HDR...
            try { ResKey.SetValue("mat_hdr_level", _HDRMode, RegistryValueKind.DWord); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }

            // Закрываем открытый ранее ключ реестра...
            ResKey.Close();
        }

        /// <summary>
        /// Возвращает ключ реестра c графическими настройками для выбранной игры.
        /// </summary>
        /// <param name="SAppName">Короткое название игры (из БД)</param>
        /// <returns>Возвращает полный путь к ключу реестра</returns>
        public static string GetGameRegKey(string SAppName)
        {
            return Path.Combine("Software", "Valve", "Source", SAppName, "Settings");
        }

        /// <summary>
        /// Проверяет существование в реестре требуемого ключа. При отсутствии оного
        /// возвращает false.
        /// </summary>
        /// <param name="Subkey">Подключ реестра для проверки</param>
        /// <returns>Возвращает булево существует ли указанный ключ реестра</returns>
        public static bool CheckRegKeyExists(string Subkey)
        {
            // Открываем проверяемый ключ реестра... При ошибке вернёт null.
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(Subkey, false);
            
            // Получаем результат проверки...
            bool Result = ResKey != null;
            
            // Если ключ был успешно открыт, закрываем.
            if (Result) { ResKey.Close(); }
            
            // Возвращаем результат функции...
            return Result;
        }

        /// <summary>
        /// Создаёт в реестре указанный ключ.
        /// </summary>
        /// <param name="Subkey">Подключ реестра для создания</param>
        public static void CreateRegKey(string Subkey)
        {
            Registry.CurrentUser.CreateSubKey(Subkey);
        }

        /// <summary>
        /// Удаляет из реестра указанный ключ.
        /// </summary>
        /// <param name="Subkey">Подключ реестра для удаления</param>
        public static void RemoveRegKey(string Subkey)
        {
            Registry.CurrentUser.DeleteSubKeyTree(Subkey, false);
        }

        /// <summary>
        /// Используется для создания резервной копии выбранной ветки
        /// реестра в переданный в параметре файл.
        /// </summary>
        /// <param name="RegKey">Ключ реестра</param>
        /// <param name="FileName">Имя файла резервной копии</param>
        /// <param name="DestDir">Каталог с резервными копиями</param>
        public static void CreateRegBackUpNow(string RegKey, string FileName, string DestDir)
        {
            // Запускаем и ждём завершения...
            ProcessManager.StartProcessAndWait("regedit.exe", String.Format("/ea \"{0}\" {1}", Path.Combine(DestDir, String.Format("{0}_{1}.reg", FileName, FileManager.DateTime2Unix(DateTime.Now))), RegKey));
        }

        /// <summary>
        /// Используется для создания резервной копии графических настроек
        /// GCF игр в файл.
        /// </summary>
        /// <param name="RegKey">Ключ реестра</param>
        /// <param name="FileName">Имя файла резервной копии</param>
        /// <param name="DestDir">Каталог с резервными копиями</param>
        public static void BackUpVideoSettings(string RegKey, string FileName, string DestDir)
        {
            CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", RegKey), FileName, DestDir);
        }

        /// <summary>
        /// Базовый конструктор класса.
        /// </summary>
        /// <param name="SAppName">Короткое название игры (из БД)</param>
        /// <param name="ReadNow">Включает автоматическое считывание настроек из реестра</param>
        public GCFVideo(string SAppName, bool ReadNow = true)
        {
            // Сгенерируем путь к ключу реестра...
            RegKey = GetGameRegKey(SAppName);

            // Считываем настройки из реестра...
            if (ReadNow) { ReadSettings(); }
        }
    }
}
