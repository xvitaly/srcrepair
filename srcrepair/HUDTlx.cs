/*
 * Класс системы управления HUD'ами.
 * 
 * Copyright 2011 - 2015 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2015 EasyCoding Team.
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
        /// Содержит URI для загрузки.
        /// </summary>
        public string URI;

        /// <summary>
        /// Содержит URI скриншота.
        /// </summary>
        public string Preview;

        /// <summary>
        /// Содержит ссылку на официальный сайт HUD.
        /// </summary>
        public string Site;

        /// <summary>
        /// Содержит имя каталога установки.
        /// </summary>
        public string IntDir;

        /// <summary>
        /// Содержит локальный путь к загруженному файлу.
        /// </summary>
        public string LocalFile;

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
        /// <param name="HUDName">Имя HUD, информацию о котором нужно получить</param>
        /// <param name="AppPath">Путь к каталогу установки SRC Repair</param>
        /// <param name="AppHUDDir">Путь к локальному каталогу с HUD</param>
        public HUDTlx(string HUDName, string AppPath, string AppHUDDir)
        {
            XmlDocument XMLD = new XmlDocument();
            FileStream XMLFS = new FileStream(Path.Combine(AppPath, Properties.Settings.Default.HUDDbFile), FileMode.Open, FileAccess.Read);
            XMLD.Load(XMLFS);
            for (int i = 0; i < XMLD.GetElementsByTagName("HUD").Count; i++)
            {
                if (String.Compare(XMLD.GetElementsByTagName("Name")[i].InnerText, HUDName, true) == 0)
                {
                    this.Name = XMLD.GetElementsByTagName("Name")[i].InnerText;
                    this.URI = XMLD.GetElementsByTagName("URI")[i].InnerText;
                    this.Preview = XMLD.GetElementsByTagName("Preview")[i].InnerText;
                    this.Site = XMLD.GetElementsByTagName("Site")[i].InnerText;
                    this.IntDir = XMLD.GetElementsByTagName("IntDir")[i].InnerText;
                    this.LocalFile = Path.Combine(AppHUDDir, Path.ChangeExtension(Path.GetFileName(this.Name), ".zip"));
                    break;
                }
            }
            XMLFS.Close();
        }
    }
}
