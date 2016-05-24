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
        private string VideoFileName;

        /// <summary>
        /// Хранит путь к файлу со стандартными настройками для текущего ПК.
        /// </summary>
        private string DefaultsFileName;

        /// <summary>
        /// Хранит содержиме файла с графическими настройками игры.
        /// </summary>
        private List<String> VideoFile;

        /// <summary>
        /// Хранит содержиме файла со стандартными настройками игры для текущего ПК.
        /// </summary>
        private List<String> DefaultsFile;

        /// <summary>
        /// Хранит настройки соотношения сторон NCF-игры на движке Source 1: setting.aspectratiomode.
        /// </summary>
        private int ScreenRatio;

        /// <summary>
        /// Хранит настройки контрастности и цветовой гаммы NCF-игры на движке Source 1: setting.mat_monitorgamma.
        /// </summary>
        private int Brightness;

        /// <summary>
        /// Хранит настройки качества теней NCF-игры на движке Source 1: setting.csm_quality_level.
        /// </summary>
        private int ShadowQuality;

        /// <summary>
        /// Хранит настройки размытия движений NCF-игры на движке Source 1: setting.mat_motion_blur_enabled.
        /// </summary>
        private int MotionBlur;

        /// <summary>
        /// Хранит настройки графического режима NCF-игры на движке Source 1: setting.fullscreen.
        /// </summary>
        private int DisplayMode;

        /// <summary>
        /// Хранит настройки графического режима NCF-игры на движке Source 1: setting.nowindowborder.
        /// </summary>
        private int DisplayBorderless;

        /// <summary>
        /// Хранит настройки полноэкранного сглаживания NCF-игры на движке Source 1: setting.mat_antialias.
        /// </summary>
        private int AntiAliasing;

        /// <summary>
        /// Хранит значение глубины полноэкранного сглаживания NCF-игры на движке Source 1: setting.mat_aaquality.
        /// </summary>
        private int AntiAliasQuality;

        /// <summary>
        /// Хранит настройки качества фильтрации текстур NCF-игры на движке Source 1: setting.mat_forceaniso.
        /// </summary>
        private int FilteringMode;

        /// <summary>
        /// Хранит настройки вертикальной синхронизации NCF-игры на движке Source 1: setting.mat_vsync.
        /// </summary>
        private int VSync;

        /// <summary>
        /// Хранит настройки качества вертикальной синхронизации NCF-игры на движке Source 1: setting.mat_triplebuffered.
        /// </summary>
        private int VSyncMode;

        /// <summary>
        /// Хранит настройки многоядерного рендеринга NCF-игры на движке Source 1: setting.mat_queue_mode.
        /// </summary>
        private int MCRendering;

        /// <summary>
        /// Хранит настройки качества шейдерных эффектов NCF-игры на движке Source 1: setting.gpu_level.
        /// </summary>
        private int ShaderEffects;

        /// <summary>
        /// Хранит настройки качества обычных эффектов NCF-игры на движке Source 1: setting.cpu_level.
        /// </summary>
        private int EffectDetails;

        /// <summary>
        /// Хранит настройки выделенного игре пула памяти NCF-игры на движке Source 1: setting.mem_level.
        /// </summary>
        private int MemoryPoolType;

        /// <summary>
        /// Хранит настройки качества детализации моделей и текстур NCF-игры на движке Source 1: setting.gpu_mem_level.
        /// </summary>
        private int TextureModelQuality;

        /// <summary>
        /// Возвращает настройки соотношения сторон NCF-игры на движке Source 1.
        /// </summary>
        public int GetScreenRatio()
        {
            int res = -1;

            switch (ScreenRatio)
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

        /// <summary>
        /// Задаёт настройки соотношения сторон NCF-игры на движке Source 1.
        /// </summary>
        /// <param name="Value">Текущий индекс контрола</param>
        public void SetScreenRatio(int Value)
        {
            switch (Value)
            {
                case 0: ScreenRatio = 0;
                    break;
                case 1: ScreenRatio = 1;
                    break;
                case 2: ScreenRatio = 2;
                    break;
            }
        }

        /// <summary>
        /// Возвращает настройки контрастности и цветовой гаммы NCF-игры на движке Source 1.
        /// </summary>
        public string GetScreenGamma()
        {
            return Brightness.ToString();
        }

        /// <summary>
        /// Задаёт настройки контрастности и цветовой гаммы NCF-игры на движке Source 1.
        /// </summary>
        /// <param name="Value">Текущий индекс контрола</param>
        public void SetScreenGamma(string Value)
        {
            Brightness = Convert.ToInt32(Value);
        }

        /// <summary>
        /// Возвращает настройки качества теней NCF-игры на движке Source 1.
        /// </summary>
        public int GetShadowQuality()
        {
            int res = -1;

            switch (ShadowQuality)
            {
                case 0: res = 0;
                    break;
                case 1: res = 1;
                    break;
                case 2: res = 2;
                    break;
                case 3: res = 3;
                    break;
            }

            return res;
        }

        /// <summary>
        /// Задаёт настройки качества теней NCF-игры на движке Source 1.
        /// </summary>
        /// <param name="Value">Текущий индекс контрола</param>
        public void SetShadowQuality(int Value)
        {
            switch (Value)
            {
                case 0: ShadowQuality = 0;
                    break;
                case 1: ShadowQuality = 1;
                    break;
                case 2: ShadowQuality = 2;
                    break;
                case 3: ShadowQuality = 3;
                    break;
            }
        }

        /// <summary>
        /// Возвращает настройки размытия движений NCF-игры на движке Source 1.
        /// </summary>
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

        /// <summary>
        /// Задаёт настройки размытия движений NCF-игры на движке Source 1.
        /// </summary>
        /// <param name="Value">Текущий индекс контрола</param>
        public void SetMotionBlur(int Value)
        {
            switch (Value)
            {
                case 0: MotionBlur = 0;
                    break;
                case 1: MotionBlur = 1;
                    break;
            }
        }

        /// <summary>
        /// Возвращает настройки графического режима NCF-игры на движке Source 1.
        /// </summary>
        public int GetScreenMode()
        {
            int res = -1;

            switch (DisplayMode)
            {
                case 0:
                    switch (DisplayBorderless)
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

        /// <summary>
        /// Задаёт настройки графического режима NCF-игры на движке Source 1.
        /// </summary>
        /// <param name="Value">Текущий индекс контрола</param>
        public void SetScreenMode(int Value)
        {
            switch (Value)
            {
                case 0:
                    DisplayMode = 1;
                    DisplayBorderless = 0;
                    break;
                case 1:
                    DisplayMode = 0;
                    DisplayBorderless = 0;
                    break;
                case 2:
                    DisplayMode = 1;
                    DisplayBorderless = 0;
                    break;
            }
        }

        /// <summary>
        /// Возвращает настройки полноэкранного сглаживания NCF-игры на движке Source 1.
        /// </summary>
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

        /// <summary>
        /// Задаёт настройки полноэкранного сглаживания NCF-игры на движке Source 1.
        /// </summary>
        /// <param name="Value">Текущий индекс контрола</param>
        public void SetAntiAliasing(int Value)
        {
            switch (Value)
            {
                case 0:
                    AntiAliasing = 1;
                    AntiAliasQuality = 0;
                    break;
                case 1:
                    AntiAliasing = 2;
                    AntiAliasQuality = 0;
                    break;
                case 2:
                    AntiAliasing = 4;
                    AntiAliasQuality = 0;
                    break;
                case 3:
                    AntiAliasing = 4;
                    AntiAliasQuality = 2;
                    break;
                case 4:
                    AntiAliasing = 4;
                    AntiAliasQuality = 4;
                    break;
                case 5:
                    AntiAliasing = 8;
                    AntiAliasQuality = 0;
                    break;
                case 6:
                    AntiAliasing = 8;
                    AntiAliasQuality = 2;
                    break;
            }
        }

        /// <summary>
        /// Возвращает настройки качества фильтрации текстур NCF-игры на движке Source 1.
        /// </summary>
        public int GetFilteringMode()
        {
            int res = -1;

            switch (FilteringMode)
            {
                case 0: res = 0;
                    break;
                case 1: res = 1;
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

        /// <summary>
        /// Задаёт настройки качества фильтрации текстур NCF-игры на движке Source 1.
        /// </summary>
        /// <param name="Value">Текущий индекс контрола</param>
        public void SetFilteringMode(int Value)
        {
            switch (Value)
            {
                case 0: FilteringMode = 0;
                    break;
                case 1: FilteringMode = 1;
                    break;
                case 2: FilteringMode = 2;
                    break;
                case 3: FilteringMode = 4;
                    break;
                case 4: FilteringMode = 8;
                    break;
                case 5: FilteringMode = 16;
                    break;
            }
        }

        /// <summary>
        /// Возвращает настройки вертикальной синхронизации NCF-игры на движке Source 1.
        /// </summary>
        public int GetVSync()
        {
            int res = -1;

            switch (VSync)
            {
                case 0: res = 0;
                    break;
                case 1:
                    switch (VSyncMode)
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

        /// <summary>
        /// Задаёт настройки вертикальной синхронизации NCF-игры на движке Source 1.
        /// </summary>
        /// <param name="Value">Текущий индекс контрола</param>
        public void SetVSync(int Value)
        {
            switch (Value)
            {
                case 0:
                    VSync = 0;
                    VSyncMode = 0;
                    break;
                case 1:
                    VSync = 1;
                    VSyncMode = 0;
                    break;
                case 2:
                    VSync = 1;
                    VSyncMode = 1;
                    break;
            }
        }

        /// <summary>
        /// Возвращает настройки многоядерного рендеринга NCF-игры на движке Source 1.
        /// </summary>
        public int GetRenderingMode()
        {
            int res = -1;

            switch (MCRendering)
            {
                case -1: res = 1;
                    break;
                case 0: res = 0;
                    break;
                case 1: res = 1;
                    break;
                case 2: res = 1;
                    break;
            }

            return res;
        }

        /// <summary>
        /// Задаёт настройки многоядерного рендеринга NCF-игры на движке Source 1.
        /// </summary>
        /// <param name="Value">Текущий индекс контрола</param>
        public void SetRenderingMode(int Value)
        {
            switch (Value)
            {
                case 0: MCRendering = 0;
                    break;
                case 1: MCRendering = -1;
                    break;
            }
        }

        /// <summary>
        /// Возвращает настройки качества шейдерных эффектов NCF-игры на движке Source 1.
        /// </summary>
        public int GetShaderEffects()
        {
            int res = -1;

            switch (ShaderEffects)
            {
                case 0: res = 0;
                    break;
                case 1: res = 1;
                    break;
                case 2: res = 2;
                    break;
                case 3: res = 3;
                    break;
            }

            return res;
        }

        /// <summary>
        /// Задаёт настройки качества шейдерных эффектов NCF-игры на движке Source 1.
        /// </summary>
        /// <param name="Value">Текущий индекс контрола</param>
        public void SetShaderEffects(int Value)
        {
            switch (Value)
            {
                case 0: ShaderEffects = 0;
                    break;
                case 1: ShaderEffects = 1;
                    break;
                case 2: ShaderEffects = 2;
                    break;
                case 3: ShaderEffects = 3;
                    break;
            }
        }

        /// <summary>
        /// Возвращает настройки качества обычных эффектов NCF-игры на движке Source 1.
        /// </summary>
        public int GetEffects()
        {
            int res = -1;

            switch (EffectDetails)
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

        /// <summary>
        /// Задаёт настройки качества обычных эффектов NCF-игры на движке Source 1.
        /// </summary>
        /// <param name="Value">Текущий индекс контрола</param>
        public void SetEffects(int Value)
        {
            switch (Value)
            {
                case 0: EffectDetails = 0;
                    break;
                case 1: EffectDetails = 1;
                    break;
                case 2: EffectDetails = 2;
                    break;
            }
        }

        /// <summary>
        /// Возвращает настройки выделенного игре пула памяти NCF-игры на движке Source 1.
        /// </summary>
        public int GetMemoryPool()
        {
            int res = -1;

            switch (MemoryPoolType)
            {
                case -1: res = 2;
                    break;
                case 0: res = 0;
                    break;
                case 1: res = 1;
                    break;
                case 2: res = 2;
                    break;
            }

            return res;
        }

        /// <summary>
        /// Задаёт настройки выделенного игре пула памяти NCF-игры на движке Source 1.
        /// </summary>
        /// <param name="Value">Текущий индекс контрола</param>
        public void SetMemoryPool(int Value)
        {
            switch (Value)
            {
                case 0: MemoryPoolType = 0;
                    break;
                case 1: MemoryPoolType = 1;
                    break;
                case 2: MemoryPoolType = 2;
                    break;
            }
        }

        /// <summary>
        /// Возвращает настройки качества детализации моделей и текстур NCF-игры на движке Source 1.
        /// </summary>
        public int GetModelQuality()
        {
            int res = -1;

            switch (TextureModelQuality)
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

        /// <summary>
        /// Задаёт настройки качества детализации моделей и текстур NCF-игры на движке Source 1.
        /// </summary>
        /// <param name="Value">Текущий индекс контрола</param>
        public void SetModelQuality(int Value)
        {
            switch (Value)
            {
                case 0: TextureModelQuality = 0;
                    break;
                case 1: TextureModelQuality = 1;
                    break;
                case 2: TextureModelQuality = 2;
                    break;
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
            ScreenHeight = GetNCFDWord("setting.defaultres");
            ScreenWidth = GetNCFDWord("setting.defaultresheight");
            ScreenRatio = GetNCFDWord("setting.aspectratiomode");
            Brightness = Convert.ToInt32(GetNCFDble("setting.mat_monitorgamma") * 10);
            ShadowQuality = GetNCFDWord("setting.csm_quality_level");
            MotionBlur = GetNCFDWord("setting.mat_motion_blur_enabled");
            DisplayMode = GetNCFDWord("setting.fullscreen");
            DisplayBorderless = GetNCFDWord("setting.nowindowborder");
            AntiAliasing = GetNCFDWord("setting.mat_antialias");
            AntiAliasQuality = GetNCFDWord("setting.mat_aaquality");
            FilteringMode = GetNCFDWord("setting.mat_forceaniso");
            VSync = GetNCFDWord("setting.mat_vsync");
            VSyncMode = GetNCFDWord("setting.mat_triplebuffered");
            MCRendering = GetNCFDWord("setting.mat_queue_mode");
            ShaderEffects = GetNCFDWord("setting.gpu_level");
            EffectDetails = GetNCFDWord("setting.cpu_level");
            MemoryPoolType = GetNCFDWord("setting.mem_level");
            TextureModelQuality = GetNCFDWord("setting.gpu_mem_level");
        }

        /// <summary>
        /// Сохраняет графические настройки игры в файл.
        /// </summary>
        public void WriteSettings()
        {
            // Проверим существует ли файл...
            if (!(File.Exists(VideoFileName))) { CoreLib.CreateFile(VideoFileName); }

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
                CFile.WriteLine(String.Format(Templt, "setting.cpu_level", EffectDetails));
                CFile.WriteLine(String.Format(Templt, "setting.gpu_level", ShaderEffects));
                CFile.WriteLine(String.Format(Templt, "setting.mat_antialias", AntiAliasing));
                CFile.WriteLine(String.Format(Templt, "setting.mat_aaquality", AntiAliasQuality));
                CFile.WriteLine(String.Format(Templt, "setting.mat_forceaniso", FilteringMode));
                CFile.WriteLine(String.Format(Templt, "setting.mat_vsync", VSync));
                CFile.WriteLine(String.Format(Templt, "setting.mat_triplebuffered", VSyncMode));
                CFile.WriteLine(String.Format(Templt, "setting.mat_grain_scale_override", "1"));
                CFile.WriteLine(String.Format(Templt, "setting.mat_monitorgamma", (Brightness / 10.0).ToString(CI)));
                CFile.WriteLine(String.Format(Templt, "setting.csm_quality_level", ShadowQuality));
                CFile.WriteLine(String.Format(Templt, "setting.mat_motion_blur_enabled", MotionBlur));
                CFile.WriteLine(String.Format(Templt, "setting.gpu_mem_level", TextureModelQuality));
                CFile.WriteLine(String.Format(Templt, "setting.mem_level", MemoryPoolType));
                CFile.WriteLine(String.Format(Templt, "setting.mat_queue_mode", MCRendering));
                CFile.WriteLine(String.Format(Templt, "setting.defaultres", ScreenWidth));
                CFile.WriteLine(String.Format(Templt, "setting.defaultresheight", ScreenHeight));
                CFile.WriteLine(String.Format(Templt, "setting.aspectratiomode", ScreenRatio));
                CFile.WriteLine(String.Format(Templt, "setting.fullscreen", DisplayMode));
                CFile.WriteLine(String.Format(Templt, "setting.nowindowborder", DisplayBorderless));

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

            // Считываем настройки из файла если зто разрешено...
            if (ReadNow)
            {
                // Получаем содержимое файла графических настроек...
                VideoFile = new List<String>(File.ReadAllLines(VideoFileName));

                // Получаем содержимое файла стандартных настроек (если он существует)...
                DefaultsFile = new List<String>();
                if (File.Exists(DefaultsFileName)) { DefaultsFile.AddRange(File.ReadAllLines(DefaultsFileName)); }

                // Запускаем непосредственно процесс...
                ReadSettings();
            }
        }
    }
}
