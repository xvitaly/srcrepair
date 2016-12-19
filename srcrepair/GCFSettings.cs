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

        public GCFSettings()
        {
            //
        }
    }
}
