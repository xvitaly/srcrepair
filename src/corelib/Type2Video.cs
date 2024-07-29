/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with Type 2 game video settings.
    /// </summary>
    public class Type2Video : CommonVideo, IType2Video
    {
        /// <summary>
        /// Stores instance of Type2Settings class.
        /// </summary>
        private readonly Type2Settings VSettings;

        /// <summary>
        /// Stores full path to video settings file.
        /// </summary>
        protected readonly string VideoFileName;

        /// <summary>
        /// Stores full path to video settings file with default
        /// options for this system.
        /// </summary>
        protected readonly string DefaultsFileName;

        /// <summary>
        /// Stores contents of video settings file.
        /// </summary>
        protected List<string> VideoFile;

        /// <summary>
        /// Stores contents of video settings file with default
        /// options for this system.
        /// </summary>
        protected List<string> DefaultsFile;

        /// <summary>
        /// Stores screen aspect ratio: setting.aspectratiomode.
        /// </summary>
        protected int _ScreenRatio;

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
        /// Gets or sets screen aspect ratio video setting.
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
                        Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _ScreenRatio, nameof(_ScreenRatio));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreSetterOutOfRange, value, nameof(ScreenRatio));
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
        public override int ShadowQuality
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _ShadowQuality, nameof(_ShadowQuality));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreSetterOutOfRange, value, nameof(ShadowQuality));
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
                            default:
                                Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _DisplayBorderless, nameof(_DisplayBorderless));
                                break;
                        }
                        break;
                    case 1:
                        res = 0;
                        break;
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _DisplayMode, nameof(_DisplayMode));
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
                        _DisplayBorderless = 1;
                        break;
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreSetterOutOfRange, value, nameof(ScreenMode));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _FilteringMode, nameof(_FilteringMode));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreSetterOutOfRange, value, nameof(FilteringMode));
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets vertical synchronization video setting.
        /// </summary>
        public override int VSync
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
                            default:
                                Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _VSyncMode, nameof(_VSyncMode));
                                break;
                        }
                        break;
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _VSync, nameof(_VSync));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreSetterOutOfRange, value, nameof(VSync));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _MCRendering, nameof(_MCRendering));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreSetterOutOfRange, value, nameof(RenderingMode));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _ShaderEffects, nameof(_ShaderEffects));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreSetterOutOfRange, value, nameof(ShaderEffects));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _EffectDetails, nameof(_EffectDetails));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreSetterOutOfRange, value, nameof(Effects));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _MemoryPoolType, nameof(_MemoryPoolType));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreSetterOutOfRange, value, nameof(MemoryPool));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _TextureModelQuality, nameof(_TextureModelQuality));
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
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreSetterOutOfRange, value, nameof(ModelQuality));
                        break;
                }
            }
        }

        /// <summary>
        /// Gets Cvar value as string from video file.
        /// </summary>
        /// <param name="CVar">Cvar name.</param>
        /// <returns>Cvar value as string from video file.</returns>
        protected override string GetRawValue(string CVar)
        {
            string CVarRegex = string.Format("setting.{0}", CVar);
            string StrRes = VideoFile.FirstOrDefault(s => Regex.IsMatch(s, CVarRegex));
            if (string.IsNullOrEmpty(StrRes))
            {
                StrRes = DefaultsFile.FirstOrDefault(s => s.Contains(CVar));
            }
            return ExtractCVFromLine(StrRes);
        }

        /// <summary>
        /// Reads contents of video config file.
        /// </summary>
        private void ReadVideoFile()
        {
            VideoFile = File.ReadAllLines(VideoFileName).ToList<string>();
            if (File.Exists(DefaultsFileName))
            {
                DefaultsFile = File.ReadAllLines(DefaultsFileName).ToList<string>();
            }
        }

        /// <summary>
        /// Parses video config file and sets values.
        /// </summary>
        private void SetVideoValues()
        {
            _ScreenWidth = GetNCFDWord(VSettings.ScreenWidth, 800);
            _ScreenHeight = GetNCFDWord(VSettings.ScreenHeight, 600);
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
        /// Creates a backup copy of the game video settings.
        /// </summary>
        /// <param name="DestDir">Directory for saving backups.</param>
        /// <param name="IsManual">Determines whether the backup was initiated by the user or not.</param>
        public override void BackUpSettings(string DestDir, bool IsManual)
        {
            List<string> VideoCfgFiles = new List<string> { VideoFileName, DefaultsFileName };
            FileManager.CreateConfigBackUp(VideoCfgFiles, DestDir, IsManual ? Properties.Resources.VideoFileManualPrefix : Properties.Resources.VideoFileAutoPrefix);
        }

        /// <summary>
        /// Removes game video settings.
        /// </summary>
        public override void RemoveSettings()
        {
            List<string> VideoCfgFiles = new List<string> { VideoFileName, DefaultsFileName };
            FileManager.RemoveFiles(VideoCfgFiles);
        }

        /// <summary>
        /// Reads Type 2 game video settings from file.
        /// </summary>
        public override void ReadSettings()
        {
            ReadVideoFile();
            SetVideoValues();
        }

        /// <summary>
        /// Writes Type 2 game video settings to file.
        /// </summary>
        public override void WriteSettings()
        {
            // Checking if file exists. If not - create it...
            if (!File.Exists(VideoFileName)) { FileManager.CreateFile(VideoFileName); }

            // Writing to file...
            using (StreamWriter CFile = new StreamWriter(VideoFileName))
            {
                // Generating template...
                string Templt = "\t\"setting.{0}\"\t\t\"{1}\"";

                // Adding standard header...
                CFile.WriteLine("\"VideoConfig\"");
                CFile.WriteLine("{");

                // Adding video settings...
                CFile.WriteLine(string.Format(Templt, VSettings.EffectDetails, _EffectDetails));
                CFile.WriteLine(string.Format(Templt, VSettings.ShaderEffects, _ShaderEffects));
                CFile.WriteLine(string.Format(Templt, VSettings.AntiAliasing, _AntiAliasing));
                CFile.WriteLine(string.Format(Templt, VSettings.AntiAliasQuality, _AntiAliasQuality));
                CFile.WriteLine(string.Format(Templt, VSettings.FilteringMode, _FilteringMode));
                CFile.WriteLine(string.Format(Templt, VSettings.VSync, _VSync));
                CFile.WriteLine(string.Format(Templt, VSettings.VSyncMode, _VSyncMode));
                CFile.WriteLine(string.Format(Templt, VSettings.GrainScaleOverride, "1"));
                CFile.WriteLine(string.Format(Templt, VSettings.Brightness, (_Brightness / 10.0).ToString(CI)));
                CFile.WriteLine(string.Format(Templt, VSettings.ShadowQuality, _ShadowQuality));
                CFile.WriteLine(string.Format(Templt, VSettings.MotionBlur, _MotionBlur));
                CFile.WriteLine(string.Format(Templt, VSettings.TextureModelQuality, _TextureModelQuality));
                CFile.WriteLine(string.Format(Templt, VSettings.MemoryPoolType, _MemoryPoolType));
                CFile.WriteLine(string.Format(Templt, VSettings.MCRendering, _MCRendering));
                CFile.WriteLine(string.Format(Templt, VSettings.ScreenWidth, _ScreenWidth));
                CFile.WriteLine(string.Format(Templt, VSettings.ScreenHeight, _ScreenHeight));
                CFile.WriteLine(string.Format(Templt, VSettings.ScreenRatio, _ScreenRatio));
                CFile.WriteLine(string.Format(Templt, VSettings.DisplayMode, _DisplayMode));
                CFile.WriteLine(string.Format(Templt, VSettings.DisplayBorderless, _DisplayBorderless));

                // Adding standard footer...
                CFile.WriteLine("}");
            }
        }

        /// <summary>
        /// Type2Video class constructor.
        /// </summary>
        /// <param name="VFile">Full path to video settings file.</param>
        public Type2Video(string VFile)
        {
            VSettings = new Type2Settings();
            VideoFileName = VFile;
            DefaultsFileName = Path.Combine(Path.GetDirectoryName(VideoFileName), "videodefaults.txt");
            VideoFile = new List<string>();
            DefaultsFile = new List<string>();
        }
    }
}
