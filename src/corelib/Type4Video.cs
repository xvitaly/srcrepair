/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with Type 4 game video settings.
    /// </summary>
    public class Type4Video : Type1Video, IType1Video
    {
        /// <summary>
        /// Stores instance of Type4Settings class.
        /// </summary>
        private readonly Type4Settings VSettings;

        /// <summary>
        /// Stores brightness value: ShadowDepthTexture.
        /// </summary>
        protected int _ShadowDepth;

        /// <summary>
        /// Stores full path to video settings file.
        /// </summary>
        protected string VideoFileName;

        /// <summary>
        /// Stores contents of video settings file.
        /// </summary>
        protected List<string> VideoFile;

        /// <summary>
        /// Gets or sets shadow effects quality video setting.
        /// </summary>
        public override int ShadowQuality
        {
            get
            {
                return base.ShadowQuality;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _ShadowQuality = 0;
                        _ShadowDepth = 0;
                        break;
                    case 1:
                        _ShadowQuality = 1;
                        _ShadowDepth = 1;
                        break;
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreSetterOutOfRange, value, "ShadowQuality");
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
            return ExtractCVFromLine(VideoFile.FirstOrDefault(s => s.Contains(CVar)));
        }

        /// <summary>
        /// Reads contents of video config file.
        /// </summary>
        private void ReadVideoFile()
        {
            VideoFile = File.ReadAllLines(VideoFileName).ToList<string>();
        }

        /// <summary>
        /// Parses video config file and sets values.
        /// </summary>
        private void SetVideoValues()
        {
            _ScreenWidth = GetNCFDWord(VSettings.ScreenWidth, 800);
            _ScreenHeight = GetNCFDWord(VSettings.ScreenHeight, 600);
            _DisplayMode = GetNCFDWord(VSettings.DisplayMode);
            _Brightness = Convert.ToInt32(GetNCFDble(VSettings.Brightness) * 10);
            _ModelDetail = GetNCFDWord(VSettings.ModelDetail);
            _TextureDetail = GetNCFDWord(VSettings.TextureDetail);
            _ShaderDetail = GetNCFDWord(VSettings.ShaderDetail);
            _WaterDetail = GetNCFDWord(VSettings.WaterDetail);
            _WaterReflections = GetNCFDWord(VSettings.WaterReflections);
            _ShadowQuality = GetNCFDWord(VSettings.ShadowDetail);
            _ColorCorrection = GetNCFDWord(VSettings.ColorCorrection);
            _AntiAliasing = GetNCFDWord(VSettings.AntiAliasing);
            _AntiAliasQuality = GetNCFDWord(VSettings.AntiAliasQuality);
            _FilteringMode = GetNCFDWord(VSettings.FilteringMode);
            _FilteringTrilinear = GetNCFDWord(VSettings.FilteringTrilinear);
            _VSync = GetNCFDWord(VSettings.VSync);
            _MotionBlur = GetNCFDWord(VSettings.MotionBlur);
            _DirectXMode = GetNCFDWord(VSettings.DirectXMode);
            _HDRMode = GetNCFDWord(VSettings.HDRMode);
            _DisplayBorderless = GetNCFDWord(VSettings.DisplayBorderless);
            _ShadowDepth = GetNCFDWord(VSettings.ShadowDepth);
            _VendorID = GetNCFDWord(VSettings.VendorID);
            _DeviceID = GetNCFDWord(VSettings.DeviceID);
        }

        /// <summary>
        /// Creates a backup copy of the game video settings.
        /// </summary>
        /// <param name="FileName">Full path to the backup file.</param>
        /// <param name="DestDir">Directory for saving backups.</param>
        public override void BackUpSettings(string FileName, string DestDir)
        {
            List<string> VideoCfgFiles = new List<string> { VideoFileName };
            FileManager.CreateConfigBackUp(VideoCfgFiles, DestDir, FileName);
        }

        /// <summary>
        /// Restores a backup copy of the game video settings.
        /// </summary>
        /// <param name="FileName">Full path to the backup file.</param>
        public override void RestoreSettings(string FileName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes game video settings.
        /// </summary>
        public override void RemoveSettings()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads Type 4 game video settings from config file.
        /// </summary>
        public override void ReadSettings()
        {
            ReadVideoFile();
            SetVideoValues();
        }

        /// <summary>
        /// Writes Type 4 game video settings to file.
        /// </summary>
        public override void WriteSettings()
        {
            // Checking if file exists. If not - create it...
            if (!File.Exists(VideoFileName)) { FileManager.CreateFile(VideoFileName); }

            // Writing to file...
            using (StreamWriter CFile = new StreamWriter(VideoFileName))
            {
                // Generating template...
                string Templt = "\t\"{0}\"\t\t\"{1}\"";

                // Adding standard header...
                CFile.WriteLine("\"videoconfig\"");
                CFile.WriteLine("{");

                // Adding video settings...
                CFile.WriteLine(string.Format(Templt, VSettings.ScreenWidth, _ScreenWidth));
                CFile.WriteLine(string.Format(Templt, VSettings.ScreenHeight, _ScreenHeight));
                CFile.WriteLine(string.Format(Templt, VSettings.DisplayMode, _DisplayMode));
                CFile.WriteLine(string.Format(Templt, VSettings.ModelDetail, _ModelDetail));
                CFile.WriteLine(string.Format(Templt, VSettings.TextureDetail, _TextureDetail));
                CFile.WriteLine(string.Format(Templt, VSettings.ShaderDetail, _ShaderDetail));
                CFile.WriteLine(string.Format(Templt, VSettings.WaterDetail, _WaterDetail));
                CFile.WriteLine(string.Format(Templt, VSettings.WaterReflections, _WaterReflections));
                CFile.WriteLine(string.Format(Templt, VSettings.ShadowDetail, _ShadowQuality));
                CFile.WriteLine(string.Format(Templt, VSettings.Brightness, (_Brightness / 10.0).ToString(CI)));
                CFile.WriteLine(string.Format(Templt, VSettings.ColorCorrection, _ColorCorrection));
                CFile.WriteLine(string.Format(Templt, VSettings.AntiAliasing, _AntiAliasing));
                CFile.WriteLine(string.Format(Templt, VSettings.AntiAliasQuality, _AntiAliasQuality));
                CFile.WriteLine(string.Format(Templt, VSettings.AntiAliasingMSAA, _AntiAliasing));
                CFile.WriteLine(string.Format(Templt, VSettings.AntiAliasQualityMSAA, _AntiAliasQuality));
                CFile.WriteLine(string.Format(Templt, VSettings.FilteringMode, _FilteringMode));
                CFile.WriteLine(string.Format(Templt, VSettings.FilteringTrilinear, _FilteringTrilinear));
                CFile.WriteLine(string.Format(Templt, VSettings.VSync, _VSync));
                CFile.WriteLine(string.Format(Templt, VSettings.MotionBlur, _MotionBlur));
                CFile.WriteLine(string.Format(Templt, VSettings.DirectXMode, _DirectXMode));
                CFile.WriteLine(string.Format(Templt, VSettings.HDRMode, _HDRMode));
                CFile.WriteLine(string.Format(Templt, VSettings.DisplayBorderless, _DisplayBorderless));
                CFile.WriteLine(string.Format(Templt, VSettings.ShadowDepth, _ShadowDepth));
                CFile.WriteLine(string.Format(Templt, VSettings.VendorID, _VendorID));
                CFile.WriteLine(string.Format(Templt, VSettings.DeviceID, _DeviceID));

                // Adding standard footer...
                CFile.WriteLine("}");
            }
        }

        /// <summary>
        /// Type4Video class constructor.
        /// </summary>
        /// <param name="SAppName">The name of registry subkey, used for storing video settings.</param>
        public Type4Video(string VFile) : base (string.Empty)
        {
            VSettings = new Type4Settings();
            VideoFileName = VFile;
            VideoFile = new List<string>();
        }
    }
}
