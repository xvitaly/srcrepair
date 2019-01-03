/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2019 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2019 EasyCoding Team.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
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
        public string Name { get; private set; }

        /// <summary>
        /// Задаёт / возвращает имя файла конфига.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Задаёт / возвращает описание конфига.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Задаёт / возвращает список поддерживаемых конфигом игр.
        /// </summary>
        public List<String> SupportedGames { get; private set; }

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
