/*
 * Класс, предназначенный для работы с графическими настройками NCF игр.
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

namespace srcrepair
{
    /// <summary>
    /// Управляет графическими настройками NCF приложений.
    /// </summary>
    public class NCFVideo : VideoSettings
    {
        private int ScreenRatio;
        private int Brightness;
        private int ShadowQuality;
        private int MotionBlur;
        private int DisplayMode;
        private int AntiAliasing;
        private int Filtering;
        private int VSync;
        private int MCRendering;
        private int ShaderEffects;
        private int EffectDetails;
        private int MemoryPoolType;
        private int TextureModelQuality;
    }
}
