using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace srcrepair
{
    public class GCFSettings
    {
        public string ScreenWidth { get; private set; }
        public string ScreenHeight { get; private set; }
        public string DisplayMode { get; private set; }
        public string ModelDetail { get; private set; }
        public string TextureDetail { get; private set; }
        public string ShaderDetail { get; private set; }
        public string WaterDetail { get; private set; }
        public string WaterReflections { get; private set; }
        public string ShadowDetail { get; private set; }
        public string ColorCorrection { get; private set; }
        public string AntiAliasing { get; private set; }
        public string AntiAliasQuality { get; private set; }
        public string FilteringMode { get; private set; }
        public string FilteringTrilinear { get; private set; }
        public string VSync { get; private set; }
        public string MotionBlur { get; private set; }
        public string DirectXMode { get; private set; }
        public string HDRMode { get; private set; }

        private void SetSettingsV1()
        {
            ScreenWidth = "ScreenWidth";
            ScreenHeight = "ScreenHeight";
            DisplayMode = "ScreenWindowed";
            ModelDetail = "r_rootlod";
            TextureDetail = "mat_picmip";
            ShaderDetail = "mat_reducefillrate";
            WaterDetail = "r_waterforceexpensive";
            WaterReflections = "r_waterforcereflectentities";
            ShadowDetail = "r_shadowrendertotexture";
            ColorCorrection = "mat_colorcorrection";
            AntiAliasing = "mat_antialias";
            AntiAliasQuality = "mat_aaquality";
            FilteringMode = "mat_forceaniso";
            FilteringTrilinear = "mat_trilinear";
            VSync = "mat_vsync";
            MotionBlur = "MotionBlur";
            DirectXMode = "DXLevel_V1";
            HDRMode = "mat_hdr_level";
        }

        public GCFSettings()
        {
            // Заполняем значения...
            SetSettingsV1();
        }
    }
}
