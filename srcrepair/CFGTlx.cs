/*
 * Класс выбранного конфига.
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

namespace srcrepair
{
    /// <summary>
    /// Класс для работы с определённым конфигом.
    /// </summary>
    public sealed class CFGTlx
    {
        /// <summary>
        /// Задаёт / возвращает имя конфига.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Задаёт / возвращает имя файла конфига.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Задаёт / возвращает описание конфига.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Задаёт / возвращает список поддерживаемых конфигом игр.
        /// </summary>
        public List<String> SupportedGames { get; set; }

        /// <summary>
        /// Проверяет совместимость конфига с игрой.
        /// </summary>
        /// <param name="GameID">ID проверямой игры</param>
        public bool CheckCompactibility(string GameID)
        {
            // Проверяем список...
            return SupportedGames.Contains(GameID);
        }

        /// <summary>
        /// Конструктор класса. Прописывает информацию о выбранном конфиге.
        /// </summary>
        /// <param name="CfName">Имя конфига</param>
        /// <param name="CfFileName">Имя файла конфига</param>
        /// <param name="CfDescr">Описание конфига</param>
        /// <param name="CfGames">Список поддерживаемых конфигом игр</param>
        public CFGTlx(string CfName, string CfFileName, string CfDescr, string[] CfGames)
        {
            // Заполняем свойства класса...
            Name = CfName;
            FileName = CfFileName;
            Description = CfDescr;
            SupportedGames = new List<String>(CfGames);
        }
    }
}
