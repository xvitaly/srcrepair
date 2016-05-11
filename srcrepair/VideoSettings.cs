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
    public class VideoSettings
    {
        /// <summary>
        /// Хранит разрешение по горизонтали.
        /// </summary>
        protected int ScreenWidth = 800;

        /// <summary>
        /// Хранит разрешение по вертикали.
        /// </summary>
        protected int ScreenHeight = 600;

        /// <summary>
        /// Возвращает разрешение по горизонтали.
        /// </summary>
        public int GetScreenWidth()
        {
            return ScreenWidth;
        }

        /// <summary>
        /// Задаёт разрешение по горизонтали.
        /// </summary>
        /// <param name="Value">Текущее значение</param>
        public void SetScreenWidth(int Value)
        {
            ScreenWidth = Value;
        }

        /// <summary>
        /// Возвращает разрешение по вертикали.
        /// </summary>
        public int GetScreenHeight()
        {
            return ScreenHeight;
        }

        /// <summary>
        /// Задаёт разрешение по вертикали.
        /// </summary>
        /// <param name="Value">Текущее значение</param>
        public void SetScreenHeight(int Value)
        {
            ScreenHeight = Value;
        }
    }
}
