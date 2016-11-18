/*
 * Класс, предназначенный для работы с графическими настройками NCF игр.
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
using System.Globalization;
using System.IO;
using System.Linq;

namespace srcrepair
{
    /// <summary>
    /// Управляет графическими настройками NCF приложений.
    /// </summary>
    public class NCFVideo : VideoSettings
    {
        /// <summary>
        /// Хранит путь к актуальному файлу с графическими настройками игры.
        /// </summary>
        protected string VideoFileName;

        /// <summary>
        /// Хранит путь к файлу со стандартными настройками для текущего ПК.
        /// </summary>
        protected string DefaultsFileName;

        /// <summary>
        /// Хранит содержимое файла с графическими настройками игры.
        /// </summary>
        protected List<String> VideoFile;

        /// <summary>
        /// Хранит содержимое файла со стандартными настройками игры для текущего ПК.
        /// </summary>
        protected List<String> DefaultsFile;

        /// <summary>
        /// Хранит настройки соотношения сторон NCF-игры на движке Source 1: setting.aspectratiomode.
        /// </summary>
        protected int _ScreenRatio;

        /// <summary>
        /// Хранит настройки контрастности и цветовой гаммы NCF-игры на движке Source 1: setting.mat_monitorgamma.
        /// </summary>
        protected int _Brightness;

        /// <summary>
        /// Хранит настройки качества теней NCF-игры на движке Source 1: setting.csm_quality_level.
        /// </summary>
        protected int _ShadowQuality;

        /// <summary>
        /// Хранит настройки размытия движений NCF-игры на движке Source 1: setting.mat_motion_blur_enabled.
        /// </summary>
        protected int _MotionBlur;

        /// <summary>
        /// Хранит настройки графического режима NCF-игры на движке Source 1: setting.fullscreen.
        /// </summary>
        protected int _DisplayMode;

        /// <summary>
        /// Хранит настройки графического режима NCF-игры на движке Source 1: setting.nowindowborder.
        /// </summary>
        protected int _DisplayBorderless;

        /// <summary>
        /// Хранит настройки полноэкранного сглаживания NCF-игры на движке Source 1: setting.mat_antialias.
        /// </summary>
        protected int _AntiAliasing;

        /// <summary>
        /// Хранит значение глубины полноэкранного сглаживания NCF-игры на движке Source 1: setting.mat_aaquality.
        /// </summary>
        protected int _AntiAliasQuality;

        /// <summary>
        /// Хранит настройки качества фильтрации текстур NCF-игры на движке Source 1: setting.mat_forceaniso.
        /// </summary>
        protected int _FilteringMode;

        /// <summary>
        /// Хранит настройки вертикальной синхронизации NCF-игры на движке Source 1: setting.mat_vsync.
        /// </summary>
        protected int _VSync;

        /// <summary>
        /// Хранит настройки качества вертикальной синхронизации NCF-игры на движке Source 1: setting.mat_triplebuffered.
        /// </summary>
        protected int _VSyncMode;

        /// <summary>
        /// Хранит настройки многоядерного рендеринга NCF-игры на движке Source 1: setting.mat_queue_mode.
        /// </summary>
        protected int _MCRendering;

        /// <summary>
        /// Хранит настройки качества шейдерных эффектов NCF-игры на движке Source 1: setting.gpu_level.
        /// </summary>
        protected int _ShaderEffects;

        /// <summary>
        /// Хранит настройки качества обычных эффектов NCF-игры на движке Source 1: setting.cpu_level.
        /// </summary>
        protected int _EffectDetails;

        /// <summary>
        /// Хранит настройки выделенного игре пула памяти NCF-игры на движке Source 1: setting.mem_level.
        /// </summary>
        protected int _MemoryPoolType;

        /// <summary>
        /// Хранит настройки качества детализации моделей и текстур NCF-игры на движке Source 1: setting.gpu_mem_level.
        /// </summary>
        protected int _TextureModelQuality;

        /// <summary>
        /// Возвращает / задаёт настройки соотношения сторон NCF-игры на движке Source 1.
        /// </summary>
        public int ScreenRatio
        {
            get
            {
                int res = -1;

                switch (_ScreenRatio)
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
                        _ScreenRatio = 0;
                        break;
                    case 1:
                        _ScreenRatio = 1;
                        break;
                    case 2:
                        _ScreenRatio = 2;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт настройки контрастности и цветовой гаммы NCF-игры на движке Source 1.
        /// </summary>
        public string ScreenGamma { get { return _Brightness.ToString(); } set { _Brightness = Convert.ToInt32(value); } }

        /// <summary>
        /// Возвращает / задаёт настройки качества теней NCF-игры на движке Source 1.
        /// </summary>
        public int ShadowQuality
        {
            get
            {
                int res = -1;

                switch (_ShadowQuality)
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
                    case 3:
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
                        _ShadowQuality = 0;
                        break;
                    case 1:
                        _ShadowQuality = 1;
                        break;
                    case 2:
                        _ShadowQuality = 2;
                        break;
                    case 3:
                        _ShadowQuality = 3;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт настройки размытия движений NCF-игры на движке Source 1.
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
        /// Возвращает / задаёт настройки графического режима NCF-игры на движке Source 1.
        /// </summary>
        public int ScreenMode
        {
            get
            {
                int res = -1;

                switch (_DisplayMode)
                {
                    case 0:
                        switch (_DisplayBorderless)
                        {
                            case 0:
                                res = 1;
                                break;
                            case 1:
                                res = 2;
                                break;
                        }
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
                        _DisplayMode = 1;
                        _DisplayBorderless = 0;
                        break;
                    case 1:
                        _DisplayMode = 0;
                        _DisplayBorderless = 0;
                        break;
                    case 2:
                        _DisplayMode = 1;
                        _DisplayBorderless = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт настройки полноэкранного сглаживания NCF-игры на движке Source 1.
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
                    case 0:
                        _AntiAliasing = 1;
                        _AntiAliasQuality = 0;
                        break;
                    case 1:
                        _AntiAliasing = 2;
                        _AntiAliasQuality = 0;
                        break;
                    case 2:
                        _AntiAliasing = 4;
                        _AntiAliasQuality = 0;
                        break;
                    case 3:
                        _AntiAliasing = 4;
                        _AntiAliasQuality = 2;
                        break;
                    case 4:
                        _AntiAliasing = 4;
                        _AntiAliasQuality = 4;
                        break;
                    case 5:
                        _AntiAliasing = 8;
                        _AntiAliasQuality = 0;
                        break;
                    case 6:
                        _AntiAliasing = 8;
                        _AntiAliasQuality = 2;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт настройки качества фильтрации текстур NCF-игры на движке Source 1.
        /// </summary>
        public int FilteringMode
        {
            get
            {
                int res = -1;

                switch (_FilteringMode)
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
                    case 0:
                        _FilteringMode = 0;
                        break;
                    case 1:
                        _FilteringMode = 1;
                        break;
                    case 2:
                        _FilteringMode = 2;
                        break;
                    case 3:
                        _FilteringMode = 4;
                        break;
                    case 4:
                        _FilteringMode = 8;
                        break;
                    case 5:
                        _FilteringMode = 16;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт настройки вертикальной синхронизации NCF-игры на движке Source 1.
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
                        switch (_VSyncMode)
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
                        _VSync = 0;
                        _VSyncMode = 0;
                        break;
                    case 1:
                        _VSync = 1;
                        _VSyncMode = 0;
                        break;
                    case 2:
                        _VSync = 1;
                        _VSyncMode = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт настройки многоядерного рендеринга NCF-игры на движке Source 1.
        /// </summary>
        public int RenderingMode
        {
            get
            {
                int res = -1;

                switch (_MCRendering)
                {
                    case -1:
                        res = 1;
                        break;
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                    case 2:
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
                        _MCRendering = 0;
                        break;
                    case 1:
                        _MCRendering = -1;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт настройки качества шейдерных эффектов NCF-игры на движке Source 1.
        /// </summary>
        public int ShaderEffects
        {
            get
            {
                int res = -1;

                switch (_ShaderEffects)
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
                    case 3:
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
                        _ShaderEffects = 0;
                        break;
                    case 1:
                        _ShaderEffects = 1;
                        break;
                    case 2:
                        _ShaderEffects = 2;
                        break;
                    case 3:
                        _ShaderEffects = 3;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт настройки качества обычных эффектов NCF-игры на движке Source 1.
        /// </summary>
        public int Effects
        {
            get
            {
                int res = -1;

                switch (_EffectDetails)
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
                        _EffectDetails = 0;
                        break;
                    case 1:
                        _EffectDetails = 1;
                        break;
                    case 2:
                        _EffectDetails = 2;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт настройки выделенного игре пула памяти NCF-игры на движке Source 1.
        /// </summary>
        public int MemoryPool
        {
            get
            {
                int res = -1;

                switch (_MemoryPoolType)
                {
                    case -1:
                        res = 2;
                        break;
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
                        _MemoryPoolType = 0;
                        break;
                    case 1:
                        _MemoryPoolType = 1;
                        break;
                    case 2:
                        _MemoryPoolType = 2;
                        break;
                }
            }
        }

        /// <summary>
        /// Возвращает / задаёт настройки качества детализации моделей и текстур NCF-игры на движке Source 1.
        /// </summary>
        public int ModelQuality
        {
            get
            {
                int res = -1;

                switch (_TextureModelQuality)
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
                        _TextureModelQuality = 0;
                        break;
                    case 1:
                        _TextureModelQuality = 1;
                        break;
                    case 2:
                        _TextureModelQuality = 2;
                        break;
                }
            }
        }

        /// <summary>
        /// Извлекает значение переменной из строки.
        /// </summary>
        /// <param name="LineA">Строка для извлечения</param>
        private string ExtractCVFromLine(string LineA)
        {
            LineA = CoreLib.CleanStrWx(LineA, true);
            return LineA.Substring(LineA.LastIndexOf(" ")).Trim();
        }

        /// <summary>
        /// Возвращает значение переменной, переданной в параметре, хранящейся в файле.
        /// </summary>
        /// <param name="CVar">Переменная</param>
        private int GetNCFDWord(string CVar)
        {
            int res; try { res = Convert.ToInt32(ExtractCVFromLine(VideoFile.FirstOrDefault(s => s.Contains(CVar)))); } catch { res = Convert.ToInt32(ExtractCVFromLine(DefaultsFile.FirstOrDefault(s => s.Contains(CVar)))); } return res;
        }

        /// <summary>
        /// Возвращает значение переменной типа double, переданной в параметре, хранящейся в файле.
        /// </summary>
        /// <param name="CVar">Переменная</param>
        private decimal GetNCFDble(string CVar)
        {
            decimal res; CultureInfo CI = new CultureInfo("en-US"); try { res = Convert.ToDecimal(ExtractCVFromLine(VideoFile.FirstOrDefault(s => s.Contains(CVar))), CI); } catch { res = Convert.ToDecimal(ExtractCVFromLine(DefaultsFile.FirstOrDefault(s => s.Contains(CVar))), CI); } return res;
        }

        /// <summary>
        /// Считывает графические настройки игры из файла.
        /// </summary>
        private void ReadSettings()
        {
            // Считываем настройки графики из файла...
            _ScreenWidth = GetNCFDWord("setting.defaultres");
            _ScreenHeight = GetNCFDWord("setting.defaultresheight");
            _ScreenRatio = GetNCFDWord("setting.aspectratiomode");
            _Brightness = Convert.ToInt32(GetNCFDble("setting.mat_monitorgamma") * 10);
            _ShadowQuality = GetNCFDWord("setting.csm_quality_level");
            _MotionBlur = GetNCFDWord("setting.mat_motion_blur_enabled");
            _DisplayMode = GetNCFDWord("setting.fullscreen");
            _DisplayBorderless = GetNCFDWord("setting.nowindowborder");
            _AntiAliasing = GetNCFDWord("setting.mat_antialias");
            _AntiAliasQuality = GetNCFDWord("setting.mat_aaquality");
            _FilteringMode = GetNCFDWord("setting.mat_forceaniso");
            _VSync = GetNCFDWord("setting.mat_vsync");
            _VSyncMode = GetNCFDWord("setting.mat_triplebuffered");
            _MCRendering = GetNCFDWord("setting.mat_queue_mode");
            _ShaderEffects = GetNCFDWord("setting.gpu_level");
            _EffectDetails = GetNCFDWord("setting.cpu_level");
            _MemoryPoolType = GetNCFDWord("setting.mem_level");
            _TextureModelQuality = GetNCFDWord("setting.gpu_mem_level");
        }

        /// <summary>
        /// Сохраняет графические настройки игры в файл.
        /// </summary>
        public void WriteSettings()
        {
            // Проверим существует ли файл...
            if (!(File.Exists(VideoFileName))) { FileManager.CreateFile(VideoFileName); }

            // Начинаем сохранять содержимое объекта в файл...
            using (StreamWriter CFile = new StreamWriter(VideoFileName))
            {
                // Генерируем шаблон...
                string Templt = "\t\"{0}\"\t\t\"{1}\"";

                // Явно указываем локаль для записи вещественных чисел...
                CultureInfo CI = new CultureInfo("en-US");

                // Вставляем стандартный заголовок...
                CFile.WriteLine("\"VideoConfig\"");
                CFile.WriteLine("{");

                // Вставляем настройки графики...
                CFile.WriteLine(String.Format(Templt, "setting.cpu_level", _EffectDetails));
                CFile.WriteLine(String.Format(Templt, "setting.gpu_level", _ShaderEffects));
                CFile.WriteLine(String.Format(Templt, "setting.mat_antialias", _AntiAliasing));
                CFile.WriteLine(String.Format(Templt, "setting.mat_aaquality", _AntiAliasQuality));
                CFile.WriteLine(String.Format(Templt, "setting.mat_forceaniso", _FilteringMode));
                CFile.WriteLine(String.Format(Templt, "setting.mat_vsync", _VSync));
                CFile.WriteLine(String.Format(Templt, "setting.mat_triplebuffered", _VSyncMode));
                CFile.WriteLine(String.Format(Templt, "setting.mat_grain_scale_override", "1"));
                CFile.WriteLine(String.Format(Templt, "setting.mat_monitorgamma", (_Brightness / 10.0).ToString(CI)));
                CFile.WriteLine(String.Format(Templt, "setting.csm_quality_level", _ShadowQuality));
                CFile.WriteLine(String.Format(Templt, "setting.mat_motion_blur_enabled", _MotionBlur));
                CFile.WriteLine(String.Format(Templt, "setting.gpu_mem_level", _TextureModelQuality));
                CFile.WriteLine(String.Format(Templt, "setting.mem_level", _MemoryPoolType));
                CFile.WriteLine(String.Format(Templt, "setting.mat_queue_mode", _MCRendering));
                CFile.WriteLine(String.Format(Templt, "setting.defaultres", _ScreenWidth));
                CFile.WriteLine(String.Format(Templt, "setting.defaultresheight", _ScreenHeight));
                CFile.WriteLine(String.Format(Templt, "setting.aspectratiomode", _ScreenRatio));
                CFile.WriteLine(String.Format(Templt, "setting.fullscreen", _DisplayMode));
                CFile.WriteLine(String.Format(Templt, "setting.nowindowborder", _DisplayBorderless));

                // Вставляем закрывающую скобку...
                CFile.WriteLine("}");
            }
        }

        /// <summary>
        /// Базовый конструктор класса.
        /// </summary>
        /// <param name="VFile">Путь к файлу с настройками графики</param>
        /// <param name="ReadNow">Включает автоматическое считывание настроек из файла</param>
        public NCFVideo(string VFile, bool ReadNow = true)
        {
            // Сохраним путь к файлу с графическими настройками...
            VideoFileName = VFile;

            // Сгенерируем путь к файлу со стандартными настройками графики текущей системы...
            DefaultsFileName = Path.Combine(Path.GetDirectoryName(VideoFileName), "videodefaults.txt");

            // Создаём массивы...
            VideoFile = new List<String>();
            DefaultsFile = new List<String>();

            // Считываем настройки из файла если зто разрешено...
            if (ReadNow)
            {
                // Получаем содержимое файла графических настроек...
                VideoFile.AddRange(File.ReadAllLines(VideoFileName));

                // Получаем содержимое файла стандартных настроек (если он существует)...
                if (File.Exists(DefaultsFileName)) { DefaultsFile.AddRange(File.ReadAllLines(DefaultsFileName)); }

                // Запускаем непосредственно процесс...
                ReadSettings();
            }
        }
    }
}
