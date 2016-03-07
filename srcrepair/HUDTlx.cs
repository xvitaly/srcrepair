/*
 * Класс системы управления HUD'ами.
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
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace srcrepair
{
    public sealed class HUDTlx
    {
        /// <summary>
        /// Содержит имя HUD.
        /// </summary>
        public string Name;

        /// <summary>
        /// Короткое название игры.
        /// </summary>
        public string Game;

        /// <summary>
        /// Содержит URI для загрузки.
        /// </summary>
        public string URI;

        /// <summary>
        /// Содержит URI апстрима.
        /// </summary>
        public string UpURI;

        /// <summary>
        /// Принимает истинное значение если HUD поддерживает последнюю версию игры.
        /// </summary>
        public bool IsUpdated;

        /// <summary>
        /// Содержит URI скриншота.
        /// </summary>
        public string Preview;

        /// <summary>
        /// Содержит ссылку на официальный сайт HUD.
        /// </summary>
        public string Site;

        /// <summary>
        /// Содержит имя каталога внутри архива.
        /// </summary>
        public string ArchiveDir;

        /// <summary>
        /// Содержит имя каталога для установки.
        /// </summary>
        public string InstallDir;

        /// <summary>
        /// Содержит локальный путь к загруженному файлу.
        /// </summary>
        public string LocalFile;

        /// <summary>
        /// Форматирует путь в соответствии с типом ОС.
        /// </summary>
        /// <param name="IntDir">Исходное значение</param>
        /// <returns>Отформатированное значение</returns>
        public string FormatIntDir(string IntDir)
        {
            return IntDir.Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Проверяет установлен ли указанный HUD.
        /// </summary>
        /// <param name="CustomInstallDir">Каталог установки кастомных файлов</param>
        /// <param name="HUDDir">Каталог установки проверяемого HUD</param>
        /// <returns>Возвращает истину если HUD с указанным именем установлен</returns>
        public bool CheckInstalledHUD(string CustomInstallDir, string HUDDir)
        {
            // Описываем локальные переменные...
            bool Result = false;
            string HUDPath = Path.Combine(CustomInstallDir, HUDDir);
            
            // Проверим существование каталога...
            if (Directory.Exists(HUDPath))
            {
                // Проверим наличие файлов или каталогов внутри...
                using (IEnumerator<String> StrEn = Directory.EnumerateFileSystemEntries(HUDPath).GetEnumerator())
                {
                    Result = StrEn.MoveNext();
                }
            }

            // Возвращаем результат...
            return Result;
        }

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
        public HUDTlx(string AppHUDDir, string HDName, string HDGame, string HDURI, string HDUpURI, bool HDIsUp, string HDPreview, string HDSite, string HDAd, string HDId, string HDLocal)
        {
            Name = HDName;
            Game = HDGame;
            URI = HDURI;
            UpURI = HDUpURI;
            IsUpdated = HDIsUp;
            Preview = HDPreview;
            Site = HDSite;
            ArchiveDir = HDAd;
            InstallDir = HDId;
            LocalFile = HDLocal;
        }
    }
}
