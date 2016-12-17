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
        public string GrainScaleOverride { get; private set; }

        private void SetSettingsV2()
        {
            ScreenWidth = "setting.defaultres";
            ScreenHeight = "setting.defaultresheight";
            ScreenRatio = "setting.aspectratiomode";
            Brightness = "setting.mat_monitorgamma";
            ShadowQuality = "setting.csm_quality_level";
            MotionBlur = "setting.mat_motion_blur_enabled";
            DisplayMode = "setting.fullscreen";
            DisplayBorderless = "setting.nowindowborder";
            AntiAliasing = "setting.mat_antialias";
            AntiAliasQuality = "setting.mat_aaquality";
            FilteringMode = "setting.mat_forceaniso";
            VSync = "setting.mat_vsync";
            VSyncMode = "setting.mat_triplebuffered";
            MCRendering = "setting.mat_queue_mode";
            ShaderEffects = "setting.gpu_level";
            EffectDetails = "setting.cpu_level";
            MemoryPoolType = "setting.mem_level";
            TextureModelQuality = "setting.gpu_mem_level";
            GrainScaleOverride = "setting.mat_grain_scale_override";
        }

        public NCFSettings(string ID)
        {
            // Временно заполним без указания ID...
            SetSettingsV2();
        }
    }
}
