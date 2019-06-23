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
    /// Class for working with Type 2 game video settings.
    /// </summary>
    public class Type2Video : ICommonVideo, IType2Video
    {
        /// <summary>
        /// Stores full path to video settings file.
        /// </summary>
        protected string VideoFileName;

        /// <summary>
        /// Stores full path to video settings file with default
        /// options for this system.
        /// </summary>
        protected string DefaultsFileName;

        /// <summary>
        /// Stores instance of Type2Settings class.
        /// </summary>
        protected Type2Settings VSettings;

        /// <summary>
        /// Stores contents of video settings file.
        /// </summary>
        protected List<String> VideoFile;

        /// <summary>
        /// Stores contents of video settings file with default
        /// options for this system.
        /// </summary>
        protected List<String> DefaultsFile;

        /// <summary>
        /// Stores screen width.
        /// </summary>
        protected int _ScreenWidth = 800;

        /// <summary>
        /// Stores screen height.
        /// </summary>
        protected int _ScreenHeight = 600;

        /// <summary>
        /// Stores screen aspect ratio: setting.aspectratiomode.
        /// </summary>
        protected int _ScreenRatio;

        /// <summary>
        /// Stores brightness value: setting.mat_monitorgamma.
        /// </summary>
        protected int _Brightness;

        /// <summary>
        /// Stores shadow effects quality: setting.csm_quality_level.
        /// </summary>
        protected int _ShadowQuality;

        /// <summary>
        /// Stores motion blur setting: setting.mat_motion_blur_enabled.
        /// </summary>
        protected int _MotionBlur;

        /// <summary>
        /// Stores display mode (fullscreen, windowed): setting.fullscreen.
        /// </summary>
        protected int _DisplayMode;

        /// <summary>
        /// Stores borderless window video setting: setting.nowindowborder.
        /// </summary>
        protected int _DisplayBorderless;

        /// <summary>
        /// Stores anti-aliasing setting: setting.mat_antialias.
        /// </summary>
        protected int _AntiAliasing;

        /// <summary>
        /// Stores anti-aliasing multiplier: setting.mat_aaquality.
        /// </summary>
        protected int _AntiAliasQuality;

        /// <summary>
        /// Stores filtering mode setting: setting.mat_forceaniso.
        /// </summary>
        protected int _FilteringMode;

        /// <summary>
        /// Stores vertical synchronization video setting: setting.mat_vsync.
        /// </summary>
        protected int _VSync;

        /// <summary>
        /// Stores vertical synchronization quality: setting.mat_triplebuffered.
        /// </summary>
        protected int _VSyncMode;

        /// <summary>
        /// Stores multicore rendering video setting: setting.mat_queue_mode.
        /// </summary>
        protected int _MCRendering;

        /// <summary>
        /// Stores shader effects level: setting.gpu_level.
        /// </summary>
        protected int _ShaderEffects;

        /// <summary>
        /// Stores standard effects level: setting.cpu_level.
        /// </summary>
        protected int _EffectDetails;

        /// <summary>
        /// Stores memory pool type: setting.mem_level.
        /// </summary>
        protected int _MemoryPoolType;

        /// <summary>
        /// Stores texture quality: setting.gpu_mem_level.
        /// </summary>
        protected int _TextureModelQuality;

        /// <summary>
        /// Gets or sets screen width video setting.
        /// </summary>
        public int ScreenWidth { get => _ScreenWidth; set { _ScreenWidth = value; } }

        /// <summary>
        /// Gets or sets screen height video setting.
        /// </summary>
        public int ScreenHeight { get => _ScreenHeight; set { _ScreenHeight = value; } }

        /// <summary>
        /// Gets or sets screen aspect ratio video setting.
        /// </summary>
        public int ScreenRatio
        {
            get
            {
                int res;

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
        /// Gets or sets screen gamma video setting..
        /// </summary>
        public string ScreenGamma { get => _Brightness.ToString(); set { _Brightness = Convert.ToInt32(value); } }

        /// <summary>
        /// Gets or sets shadow effects quality video setting.
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
        /// Gets or sets motion blur video setting.
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
        /// Gets or sets display mode (fullscreen, windowed) video setting.
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
        /// Gets or sets anti-aliasing video setting.
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
        /// Gets or sets filtering mode video setting.
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
        /// Gets or sets vertical synchronization video setting.
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
        /// Gets or sets multicore rendering video setting.
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
        /// Gets or sets shader effects level video setting.
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
        /// Gets or sets standard effects video setting.
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
        /// Gets or sets memory pool video setting.
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
        /// Gets or sets model quality video setting.
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
        /// Gets Cvar value from string.
        /// </summary>
        /// <param name="LineA">Source string.</param>
        /// <returns>Extracted from source string value.</returns>
        protected string ExtractCVFromLine(string LineA)
        {
            LineA = CoreLib.CleanStrWx(LineA, true);
            return LineA.Substring(LineA.LastIndexOf(" ")).Trim();
        }

        /// <summary>
        /// Gets Cvar value of integer type from video file.
        /// </summary>
        /// <param name="CVar">Cvar name.</param>
        /// <returns>Cvar value from video file.</returns>
        protected int GetNCFDWord(string CVar)
        {
            int Result;
            try
            {
                string StrRes = VideoFile.FirstOrDefault(s => s.Contains(CVar) && Regex.IsMatch(s, "setting."));
                if (String.IsNullOrEmpty(StrRes))
                {
                    StrRes = DefaultsFile.FirstOrDefault(s => s.Contains(CVar));
                }
                Result = Convert.ToInt32(ExtractCVFromLine(StrRes));
            }
            catch
            {
                Result = -1;
            }
            return Result;
        }

        /// <summary>
        /// Gets Cvar value of decimal type from video file.
        /// </summary>
        /// <param name="CVar">Cvar name.</param>
        /// <returns>Cvar value from video file.</returns>
        protected decimal GetNCFDble(string CVar)
        {
            decimal Result;
            CultureInfo CI = new CultureInfo("en-US");
            try
            {
                string StrRes = VideoFile.FirstOrDefault(s => s.Contains(CVar) && Regex.IsMatch(s, "setting."));
                if (String.IsNullOrEmpty(StrRes))
                {
                    StrRes = DefaultsFile.FirstOrDefault(s => s.Contains(CVar));
                }
                Result = Convert.ToDecimal(ExtractCVFromLine(StrRes), CI);
            }
            catch
            {
                Result = 2.2M;
            }
            return Result;
        }

        /// <summary>
        /// Reads Type 2 game video settings from file.
        /// </summary>
        protected void ReadSettings()
        {
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
        /// Writes Type 2 game video settings to file.
        /// </summary>
        public void WriteSettings()
        {
            // Checking if file exists. If not - create it...
            if (!(File.Exists(VideoFileName))) { FileManager.CreateFile(VideoFileName); }

            // Writing to file...
            using (StreamWriter CFile = new StreamWriter(VideoFileName))
            {
                // Generating template...
                string Templt = "\t\"setting.{0}\"\t\t\"{1}\"";

                // Explicitly setting en-US locale for writing floating point numbers...
                CultureInfo CI = new CultureInfo("en-US");

                // Adding standard header...
                CFile.WriteLine("\"VideoConfig\"");
                CFile.WriteLine("{");

                // Adding video settings...
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

                // Adding standard footer...
                CFile.WriteLine("}");
            }
        }

        /// <summary>
        /// Type2Video class constructor.
        /// </summary>
        /// <param name="VFile">Full path to video settings file.</param>
        /// <param name="ReadNow">Enable immediate reading of video settings.</param>
        public Type2Video(string VFile, bool ReadNow = true)
        {
            VSettings = new Type2Settings();
            VideoFileName = VFile;
            DefaultsFileName = Path.Combine(Path.GetDirectoryName(VideoFileName), "videodefaults.txt");
            VideoFile = new List<String>();
            DefaultsFile = new List<String>();

            if (ReadNow)
            {
                VideoFile.AddRange(File.ReadAllLines(VideoFileName));
                if (File.Exists(DefaultsFileName)) { DefaultsFile.AddRange(File.ReadAllLines(DefaultsFileName)); }
                ReadSettings();
            }
        }
    }
}
