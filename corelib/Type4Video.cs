/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 *
 * Copyright (c) 2011 - 2020 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2020 EasyCoding Team.
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
        protected List<String> VideoFile;

        /// <summary>
        /// Stores instance of Type4Settings class.
        /// </summary>
        protected new Type4Settings VSettings;

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
            VideoFile = File.ReadAllLines(VideoFileName).ToList<String>();
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
                CFile.WriteLine(String.Format(Templt, VSettings.ScreenWidth, _ScreenWidth));
                CFile.WriteLine(String.Format(Templt, VSettings.ScreenHeight, _ScreenHeight));
                CFile.WriteLine(String.Format(Templt, VSettings.DisplayMode, _DisplayMode));
                CFile.WriteLine(String.Format(Templt, VSettings.ModelDetail, _ModelDetail));
                CFile.WriteLine(String.Format(Templt, VSettings.TextureDetail, _TextureDetail));
                CFile.WriteLine(String.Format(Templt, VSettings.ShaderDetail, _ShaderDetail));
                CFile.WriteLine(String.Format(Templt, VSettings.WaterDetail, _WaterDetail));
                CFile.WriteLine(String.Format(Templt, VSettings.WaterReflections, _WaterReflections));
                CFile.WriteLine(String.Format(Templt, VSettings.ShadowDetail, _ShadowQuality));
                CFile.WriteLine(String.Format(Templt, VSettings.Brightness, (_Brightness / 10.0).ToString(CI)));
                CFile.WriteLine(String.Format(Templt, VSettings.ColorCorrection, _ColorCorrection));
                CFile.WriteLine(String.Format(Templt, VSettings.AntiAliasing, _AntiAliasing));
                CFile.WriteLine(String.Format(Templt, VSettings.AntiAliasQuality, _AntiAliasQuality));
                CFile.WriteLine(String.Format(Templt, VSettings.AntiAliasingMSAA, _AntiAliasing));
                CFile.WriteLine(String.Format(Templt, VSettings.AntiAliasQualityMSAA, _AntiAliasQuality));
                CFile.WriteLine(String.Format(Templt, VSettings.FilteringMode, _FilteringMode));
                CFile.WriteLine(String.Format(Templt, VSettings.FilteringTrilinear, _FilteringTrilinear));
                CFile.WriteLine(String.Format(Templt, VSettings.VSync, _VSync));
                CFile.WriteLine(String.Format(Templt, VSettings.MotionBlur, _MotionBlur));
                CFile.WriteLine(String.Format(Templt, VSettings.DirectXMode, _DirectXMode));
                CFile.WriteLine(String.Format(Templt, VSettings.HDRMode, _HDRMode));
                CFile.WriteLine(String.Format(Templt, VSettings.DisplayBorderless, _DisplayBorderless));
                CFile.WriteLine(String.Format(Templt, VSettings.ShadowDepth, _ShadowDepth));
                CFile.WriteLine(String.Format(Templt, VSettings.VendorID, _VendorID));
                CFile.WriteLine(String.Format(Templt, VSettings.DeviceID, _DeviceID));

                // Adding standard footer...
                CFile.WriteLine("}");
            }
        }

        /// <summary>
        /// Type4Video class constructor.
        /// </summary>
        /// <param name="SAppName">The name of registry subkey, used for storing video settings.</param>
        public Type4Video(string VFile) : base (String.Empty)
        {
            VSettings = new Type4Settings();
            VideoFileName = VFile;
            VideoFile = new List<String>();
        }
    }
}
