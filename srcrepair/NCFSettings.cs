using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace srcrepair
{
    public class NCFSettings
    {
        public string ScreenWidth { get; private set; }
        public string ScreenHeight { get; private set; }
        public string ScreenRatio { get; private set; }
        public string Brightness { get; private set; }
        public string ShadowQuality { get; private set; }
        public string MotionBlur { get; private set; }
        public string DisplayMode { get; private set; }
        public string DisplayBorderless { get; private set; }
        public string AntiAliasing { get; private set; }
        public string AntiAliasQuality { get; private set; }
        public string FilteringMode { get; private set; }
        public string VSync { get; private set; }
        public string VSyncMode { get; private set; }
        public string MCRendering { get; private set; }
        public string ShaderEffects { get; private set; }
        public string EffectDetails { get; private set; }
        public string MemoryPoolType { get; private set; }
        public string TextureModelQuality { get; private set; }

        public NCFSettings(string ID)
        {
            //
        }
    }
}
