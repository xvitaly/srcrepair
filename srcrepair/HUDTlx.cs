/*
 * Класс выбранного HUD.
 * 
 * Copyright 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2017 EasyCoding Team.
 * 
 * Лицензия: GPL v3 (см. файл GPL.txt).
 * Лицензия контента: Creative Commons 3.0 BY.
 * 
 * Запрещается использовать этот файл при использовании любой
 * лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
 * 
 * Официальный блог EasyCoding Team: https://www.easycoding.org/
 * Официальная страница проекта: https://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/
using System;

namespace srcrepair
{
    /// <summary>
    /// Класс для работы с определённым HUD.
    /// </summary>
    public sealed class HUDTlx
    {
        /// <summary>
        /// Содержит имя HUD.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Короткое название игры.
        /// </summary>
        public string Game { get; private set; }

        /// <summary>
        /// Содержит URI для загрузки.
        /// </summary>
        public string URI { get; private set; }

        /// <summary>
        /// Содержит URI апстрима.
        /// </summary>
        public string UpURI { get; private set; }

        /// <summary>
        /// Принимает истинное значение если HUD поддерживает последнюю версию игры.
        /// </summary>
        public bool IsUpdated { get; private set; }

        /// <summary>
        /// Содержит URI скриншота.
        /// </summary>
        public string Preview { get; private set; }

        /// <summary>
        /// Содержит время последнего обновления базы в формате unixtime.
        /// </summary>
        public long LastUpdate { get; private set; }

        /// <summary>
        /// Содержит ссылку на официальный сайт HUD.
        /// </summary>
        public string Site { get; private set; }

        /// <summary>
        /// Содержит имя каталога внутри архива.
        /// </summary>
        public string ArchiveDir { get; private set; }

        /// <summary>
        /// Содержит имя каталога для установки.
        /// </summary>
        public string InstallDir { get; private set; }

        /// <summary>
        /// Содержит локальный путь к загруженному файлу.
        /// </summary>
        public string LocalFile { get; private set; }

        public string UpdateHash { get; private set; }

        /// <summary>
        /// Конструктор класса. Получает информацию о выбранном HUD.
        /// </summary>
        /// <param name="AppHUDDir">Путь к локальному каталогу с HUD</param>
        /// <param name="HDName">Значение Name из БД</param>
        /// <param name="HDGame">Значение Game из БД</param>
        /// <param name="HDURI">Значение URI из БД</param>
        /// <param name="HDUpURI">Значение UpURI из БД</param>
        /// <param name="HDIsUp">Значение IsUpdated из БД</param>
        /// <param name="HDPreview">Значение Preview из БД</param>
        /// <param name="HDSite">Значение Site из БД</param>
        /// <param name="HDAd">Значение ArchiveDir из БД</param>
        /// <param name="HDId">Значение InstallDir из БД</param>
        /// <param name="HDLocal">Локальный путь к файлу с HUD</param>
        public HUDTlx(string AppHUDDir, string HDName, string HDGame, string HDURI, string HDUpURI, bool HDIsUp, string HDPreview, string HDUpTime, string HDSite, string HDAd, string HDId, string HDLocal)
        {
            Name = HDName;
            Game = HDGame;
            URI = HDURI;
            UpURI = HDUpURI;
            IsUpdated = HDIsUp;
            Preview = HDPreview;
            LastUpdate = Convert.ToInt64(HDUpTime);
            Site = HDSite;
            ArchiveDir = HDAd;
            InstallDir = HDId;
            LocalFile = HDLocal;
        }
    }
}
