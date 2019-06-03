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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace srcrepair.core
{
    /// <summary>
    /// Управляет графическими настройками Type 2 приложений.
    /// </summary>
    public class Type2Video : ICommonVideo, IType2Video
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
        /// Хранит и определяет названия переменных внутри базы настроек графики
        /// в зависимости от версии движка Source Engine.
        /// </summary>
        protected Type2Settings VSettings;

        /// <summary>
        /// Хранит содержимое файла с графическими настройками игры.
        /// </summary>
        protected List<String> VideoFile;

        /// <summary>
        /// Хранит содержимое файла со стандартными настройками игры для текущего ПК.
        /// </summary>
        protected List<String> DefaultsFile;

        /// <summary>
        /// Хранит разрешение по горизонтали.
        /// </summary>
        protected int _ScreenWidth = 800;

        /// <summary>
        /// Хранит разрешение по вертикали.
        /// </summary>
        protected int _ScreenHeight = 600;

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
        /// Возвращает / задаёт разрешение по горизонтали.
        /// </summary>
        public int ScreenWidth { get { return _ScreenWidth; } set { _ScreenWidth = value; } }

        /// <summary>
        /// Возвращает / задаёт разрешение по вертикали.
        /// </summary>
        public int ScreenHeight { get { return _ScreenHeight; } set { _ScreenHeight = value; } }

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
                    default:
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
                    case -1:
                        res = 3;
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
            int Result; try { string StrRes = VideoFile.FirstOrDefault(s => s.Contains(CVar) && Regex.IsMatch(s, "setting.")); if (String.IsNullOrEmpty(StrRes)) { StrRes = DefaultsFile.FirstOrDefault(s => s.Contains(CVar)); } Result = Convert.ToInt32(ExtractCVFromLine(StrRes)); } catch { Result = -1; } return Result;
        }

        /// <summary>
        /// Возвращает значение переменной типа double, переданной в параметре, хранящейся в файле.
        /// </summary>
        /// <param name="CVar">Переменная</param>
        private decimal GetNCFDble(string CVar)
        {
            decimal Result; CultureInfo CI = new CultureInfo("en-US"); try { string StrRes = VideoFile.FirstOrDefault(s => s.Contains(CVar) && Regex.IsMatch(s, "setting.")); if (String.IsNullOrEmpty(StrRes)) { StrRes = DefaultsFile.FirstOrDefault(s => s.Contains(CVar)); } Result = Convert.ToDecimal(ExtractCVFromLine(StrRes), CI); } catch { Result = 2.2M; } return Result;
        }

        /// <summary>
        /// Считывает графические настройки игры из файла.
        /// </summary>
        private void ReadSettings()
        {
            // Считываем настройки графики из файла...
            _ScreenWidth = GetNCFDWord(VSettings.ScreenWidth);
            _ScreenHeight = GetNCFDWord(VSettings.ScreenHeight);
            _ScreenRatio = GetNCFDWord(VSettings.ScreenRatio);
            _Brightness = Convert.ToInt32(GetNCFDble(VSettings.Brightness) * 10);
            _ShadowQuality = GetNCFDWord(VSettings.ShadowQuality);
            _MotionBlur = GetNCFDWord(VSettings.MotionBlur);
            _DisplayMode = GetNCFDWord(VSettings.DisplayMode);
            _DisplayBorderless = GetNCFDWord(VSettings.DisplayBorderless);
            _AntiAliasing = GetNCFDWord(VSettings.AntiAliasing);
            _AntiAliasQuality = GetNCFDWord(VSettings.AntiAliasQuality);
            _FilteringMode = GetNCFDWord(VSettings.FilteringMode);
            _VSync = GetNCFDWord(VSettings.VSync);
            _VSyncMode = GetNCFDWord(VSettings.VSyncMode);
            _MCRendering = GetNCFDWord(VSettings.MCRendering);
            _ShaderEffects = GetNCFDWord(VSettings.ShaderEffects);
            _EffectDetails = GetNCFDWord(VSettings.EffectDetails);
            _MemoryPoolType = GetNCFDWord(VSettings.MemoryPoolType);
            _TextureModelQuality = GetNCFDWord(VSettings.TextureModelQuality);
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
                string Templt = "\t\"setting.{0}\"\t\t\"{1}\"";

                // Явно указываем локаль для записи вещественных чисел...
                CultureInfo CI = new CultureInfo("en-US");

                // Вставляем стандартный заголовок...
                CFile.WriteLine("\"VideoConfig\"");
                CFile.WriteLine("{");

                // Вставляем настройки графики...
                CFile.WriteLine(String.Format(Templt, VSettings.EffectDetails, _EffectDetails));
                CFile.WriteLine(String.Format(Templt, VSettings.ShaderEffects, _ShaderEffects));
                CFile.WriteLine(String.Format(Templt, VSettings.AntiAliasing, _AntiAliasing));
                CFile.WriteLine(String.Format(Templt, VSettings.AntiAliasQuality, _AntiAliasQuality));
                CFile.WriteLine(String.Format(Templt, VSettings.FilteringMode, _FilteringMode));
                CFile.WriteLine(String.Format(Templt, VSettings.VSync, _VSync));
                CFile.WriteLine(String.Format(Templt, VSettings.VSyncMode, _VSyncMode));
                CFile.WriteLine(String.Format(Templt, VSettings.GrainScaleOverride, "1"));
                CFile.WriteLine(String.Format(Templt, VSettings.Brightness, (_Brightness / 10.0).ToString(CI)));
                CFile.WriteLine(String.Format(Templt, VSettings.ShadowQuality, _ShadowQuality));
                CFile.WriteLine(String.Format(Templt, VSettings.MotionBlur, _MotionBlur));
                CFile.WriteLine(String.Format(Templt, VSettings.TextureModelQuality, _TextureModelQuality));
                CFile.WriteLine(String.Format(Templt, VSettings.MemoryPoolType, _MemoryPoolType));
                CFile.WriteLine(String.Format(Templt, VSettings.MCRendering, _MCRendering));
                CFile.WriteLine(String.Format(Templt, VSettings.ScreenWidth, _ScreenWidth));
                CFile.WriteLine(String.Format(Templt, VSettings.ScreenHeight, _ScreenHeight));
                CFile.WriteLine(String.Format(Templt, VSettings.ScreenRatio, _ScreenRatio));
                CFile.WriteLine(String.Format(Templt, VSettings.DisplayMode, _DisplayMode));
                CFile.WriteLine(String.Format(Templt, VSettings.DisplayBorderless, _DisplayBorderless));

                // Вставляем закрывающую скобку...
                CFile.WriteLine("}");
            }
        }

        /// <summary>
        /// Базовый конструктор класса.
        /// </summary>
        /// <param name="VFile">Путь к файлу с настройками графики</param>
        /// <param name="SVID">Тип механизма хранения настроек движка Source</param>
        /// <param name="ReadNow">Включает автоматическое считывание настроек из файла</param>
        public Type2Video(string VFile, string SVID, bool ReadNow = true)
        {
            // Подготовим базу с названиями переменных базы нужной версии...
            VSettings = new Type2Settings(SVID);
            
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
