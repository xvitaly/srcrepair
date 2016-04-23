/*
 * Классы, связанные с работой с графическими настройками.
 * 
 * Copyright 2011 - 2016 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2016 EasyCoding Team.
 * 
 * Лицензия: GPL v3 (см. файл GPL.txt).
 * Лицензия контента: Creative Commons 3.0 BY.
 * 
 * Запрещается использовать этот файл при использовании любой
 * лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
 * 
 * Официальный блог EasyCoding Team: http://www.easycoding.org/
 * Официальная страница проекта: http://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace srcrepair
{
    public class VideoSettings
    {
        protected bool GameType;

        protected int ScreenWidth;
        protected int ScreenHeight;
    }

    public class NCFVideo : VideoSettings
    {
    }

    public class GCFVideo: VideoSettings
    {
        private int DisplayMode;
        private int ModelDetail;
        private int TextureDetail;
        private int ShaderDetail;
        private int WaterDetail;
        private int WaterReflections;
        private int ShadowDetail;
        private int ColorCorrection;
        private int AntiAliasing;
        private int AnitiAliasQuality;
        private int FilteringMode;
        private int FilteringTrilinear;
        private int VSync;
        private int MotionBlur;
        private int DirectXMode;
        private int HDRMode;
    }
}
