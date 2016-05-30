using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public string[] SupportedGames { get; set; }

        /// <summary>
        /// Конструктор класса. Прописывает информацию о выбранном конфиге.
        /// </summary>
        /// <param name="CfName">Имя конфига</param>
        /// <param name="CfFileName">Имя файла конфига</param>
        /// <param name="CfDescr">Описание конфига</param>
        /// <param name="CfGames">Список поддерживаемых конфигом игр</param>
        public CFGTlx(string CfName, string CfFileName, string CfDescr, string[] CfGames)
        {
            //
        }
    }
}
