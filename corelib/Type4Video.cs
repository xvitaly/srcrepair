using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with Type 4 game video settings.
    /// </summary>
    public class Type4Video : Type1Video, ICommonVideo, IType1Video
    {
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
        /// Gets Cvar value of integer type from video file.
        /// </summary>
        /// <param name="CVar">Cvar name.</param>
        /// <returns>Cvar value from video file.</returns>
        protected int GetNCFDWord(string CVar)
        {
            int Result = -1;
            try
            {
                string StrRes = VideoFile.FirstOrDefault(s => s.Contains(CVar));
                Result = Convert.ToInt32(Type2Video.ExtractCVFromLine(StrRes));
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex);
                Result = -1;
            }
            return Result;
        }

        /// <summary>
        /// Reads Type 4 game video settings from config file.
        /// </summary>
        protected new void ReadSettings()
        {
            _ScreenWidth = GetNCFDWord(VSettings.ScreenWidth);
            _ScreenHeight = GetNCFDWord(VSettings.ScreenHeight);
            _DisplayMode = GetNCFDWord(VSettings.DisplayMode);
            _ModelDetail = GetNCFDWord(VSettings.ModelDetail);
            _TextureDetail = GetNCFDWord(VSettings.TextureDetail);
            _ShaderDetail = GetNCFDWord(VSettings.ShaderDetail);
            _WaterDetail = GetNCFDWord(VSettings.WaterDetail);
            _WaterReflections = GetNCFDWord(VSettings.WaterReflections);
            _ShadowDetail = GetNCFDWord(VSettings.ShadowDetail);
            _ColorCorrection = GetNCFDWord(VSettings.ColorCorrection);
            _AntiAliasing = GetNCFDWord(VSettings.AntiAliasing);
            _AntiAliasQuality = GetNCFDWord(VSettings.AntiAliasQuality);
            _FilteringMode = GetNCFDWord(VSettings.FilteringMode);
            _FilteringTrilinear = GetNCFDWord(VSettings.FilteringTrilinear);
            _VSync = GetNCFDWord(VSettings.VSync);
            _MotionBlur = GetNCFDWord(VSettings.MotionBlur);
            _DirectXMode = GetNCFDWord(VSettings.DirectXMode);
            _HDRMode = GetNCFDWord(VSettings.HDRMode);
        }

        /// <summary>
        /// Writes Type 2 game video settings to file.
        /// </summary>
        public new void WriteSettings()
        {
            // Checking if file exists. If not - create it...
            if (!File.Exists(VideoFileName)) { FileManager.CreateFile(VideoFileName); }

            // Writing to file...
            using (StreamWriter CFile = new StreamWriter(VideoFileName))
            {
                // Generating template...
                string Templt = "\t\"{0}\"\t\t\"{1}\"";

                // Explicitly setting en-US locale for writing floating point numbers...
                //CultureInfo CI = new CultureInfo("en-US");

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
                CFile.WriteLine(String.Format(Templt, VSettings.ShadowDetail, _ShadowDetail));
                //CFile.WriteLine(String.Format(Templt, VSettings.Brightness, (_Brightness / 10.0).ToString(CI)));
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

                // Adding standard footer...
                CFile.WriteLine("}");
            }
        }

        /// <summary>
        /// Type4Video class constructor.
        /// </summary>
        /// <param name="SAppName">The name of registry subkey, used for storing video settings.</param>
        /// <param name="ReadNow">Enable immediate reading of video settings.</param>
        public Type4Video(string SAppName, string VFile, bool ReadNow = true) : base (SAppName, false)
        {
            VSettings = new Type4Settings();
            VideoFileName = VFile;
            VideoFile = new List<String>();
            if (ReadNow)
            {
                VideoFile.AddRange(File.ReadAllLines(VideoFileName));
                ReadSettings();
            }
        }
    }
}
