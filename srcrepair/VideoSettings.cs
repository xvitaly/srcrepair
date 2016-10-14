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

namespace srcrepair
{
    /// <summary>
    /// Общий класс VideoSettings. Напрямую не используется.
    /// </summary>
    public abstract class VideoSettings
    {
        /// <summary>
        /// Хранит разрешение по горизонтали.
        /// </summary>
        protected int _ScreenWidth = 800;

        /// <summary>
        /// Хранит разрешение по вертикали.
        /// </summary>
        protected int _ScreenHeight = 600;

        /// <summary>
        /// Возвращает / задаёт разрешение по горизонтали.
        /// </summary>
        public int ScreenWidth { get { return _ScreenWidth; } set { _ScreenWidth = value; } }

        /// <summary>
        /// Возвращает / задаёт разрешение по вертикали.
        /// </summary>
        public int ScreenHeight { get { return _ScreenHeight; } set { _ScreenHeight = value; } }
    }
}
