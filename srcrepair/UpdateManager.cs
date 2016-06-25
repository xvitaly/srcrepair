/*
 * Класс системы проверки обновлений.
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
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;

namespace srcrepair
{
    /// <summary>
    /// Класс, используемый для проверки обновлений.
    /// </summary>
    public class UpdateManager
    {
        /// <summary>
        /// Хранит последнюю доступную версию приложения.
        /// </summary>
        private Version AppUpdateVersion;

        /// <summary>
        /// Хранит URL для загрузки последней доступной версии приложения.
        /// </summary>
        private string AppUpdateURL;

        /// <summary>
        /// Хранит хеш-сумму установщика последней доступной версии приложения.
        /// </summary>
        private string AppUpdateHash;

        /// <summary>
        /// Хранит URL для загрузки последней доступной версии базы игр.
        /// </summary>
        private string GameUpdateURL;

        /// <summary>
        /// Хранит хеш-сумму последней доступной версии базы игр.
        /// </summary>
        private string GameUpdateHash;

        /// <summary>
        /// Хранит URL для загрузки последней доступной версии базы HUD.
        /// </summary>
        private string HUDUpdateURL;

        /// <summary>
        /// Хранит хеш-сумму последней доступной версии базы HUD.
        /// </summary>
        private string HUDUpdateHash;

        /// <summary>
        /// Хранит URL для загрузки последней доступной версии базы конфигов.
        /// </summary>
        private string CfgUpdateURL;

        /// <summary>
        /// Хранит хеш-сумму последней доступной версии базы конфигов.
        /// </summary>
        private string CfgUpdateHash;

        /// <summary>
        /// Хранит путь к каталогу приложения для служебных целей.
        /// </summary>
        private string FullAppPath;

        /// <summary>
        /// Хранит загруженный с сервера обновлений XML.
        /// </summary>
        private string UpdateXML;

        /// <summary>
        /// Загружает XML со списком обновлений с сервера обновлений.
        /// Вызывается конструктором класса.
        /// </summary>
        private void DownloadXML()
        {
            // Загружаем XML...
            using (WebClient Downloader = new WebClient())
            {
                Downloader.Headers.Add("User-Agent", "UA");
                UpdateXML = Downloader.DownloadString(Properties.Resources.UpdateDBURL);
            }
        }

        /// <summary>
        /// Парсит загруженный XML файл. Заполняет поля класса значениями.
        /// Вызывается конструктором класса.
        /// </summary>
        private void ParseXML()
        {
            // Загружаем XML...
            XmlDocument XMLD = new XmlDocument();
            XMLD.LoadXml(UpdateXML);

            // Разбираем XML в цикле...
            foreach (XmlNode Node in XMLD.SelectNodes("Updates"))
            {
                foreach (XmlNode Child in Node.ChildNodes)
                {
                    switch (Child.Name)
                    {
                        case "Application":
                            AppUpdateVersion = new Version(Child.ChildNodes[0].InnerText);
                            AppUpdateURL = Child.ChildNodes[1].InnerText;
                            AppUpdateHash = Child.ChildNodes[2].InnerText;
                            break;
                        case "GameDB":
                            GameUpdateURL = Child.ChildNodes[1].InnerText;
                            GameUpdateHash = Child.ChildNodes[2].InnerText;
                            break;
                        case "HUDDB":
                            HUDUpdateURL = Child.ChildNodes[1].InnerText;
                            HUDUpdateHash = Child.ChildNodes[2].InnerText;
                            break;
                        case "CfgDB":
                            CfgUpdateURL = Child.ChildNodes[1].InnerText;
                            CfgUpdateHash = Child.ChildNodes[2].InnerText;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает URL для загрузки новой версии приложения.
        /// </summary>
        /// <returns>URL для загрузки новой версии приложения</returns>
        public string GetAppUpdateURL()
        {
            return AppUpdateURL;
        }

        /// <summary>
        /// Возвращает URL для загрузки новой версии базы игр.
        /// </summary>
        /// <returns>URL для загрузки новой версии базы игр</returns>
        public string GetGameDBUpdateURL()
        {
            return GameUpdateURL;
        }

        /// <summary>
        /// Возвращает URL для загрузки новой версии базы HUD.
        /// </summary>
        /// <returns>URL для загрузки новой версии базы HUD</returns>
        public string GetHUDDBUpdateURL()
        {
            return HUDUpdateURL;
        }

        /// <summary>
        /// Возвращает URL для загрузки новой версии базы конфигов.
        /// </summary>
        /// <returns>URL для загрузки новой версии базы конфигов</returns>
        public string GetCfgDBUpdateURL()
        {
            return CfgUpdateURL;
        }

        /// <summary>
        /// Проверяет наличие обновлений для приложения.
        /// </summary>
        /// <returns>Возвращает булево наличия обновлений</returns>
        public bool CheckAppUpdate()
        {
            return AppUpdateVersion > Assembly.GetEntryAssembly().GetName().Version;
        }

        /// <summary>
        /// Проверяет hash обновления приложения с переданным в качестве параметра.
        /// </summary>
        /// <param name="Hash">Хеш загруженного файла</param>
        /// <returns>Возвращает булево соответствия хешей</returns>
        public bool CheckAppHash(string Hash)
        {
            return AppUpdateHash == Hash;
        }

        /// <summary>
        /// Проверяет наличие обновлений для базы игр.
        /// </summary>
        /// <returns>Возвращает булево наличия обновлений</returns>
        public bool CheckGameDBUpdate()
        {
            return CoreLib.CalculateFileMD5(Path.Combine(FullAppPath, Properties.Settings.Default.GameListFile)) == GameUpdateHash;
        }

        /// <summary>
        /// Проверяет наличие обновлений для базы HUD.
        /// </summary>
        /// <returns>Возвращает булево наличия обновлений</returns>
        public bool CheckHUDUpdate()
        {
            return CoreLib.CalculateFileMD5(Path.Combine(FullAppPath, Properties.Settings.Default.HUDDbFile)) == HUDUpdateHash;
        }

        /// <summary>
        /// Проверяет наличие обновлений для базы FPS-конфигов.
        /// </summary>
        /// <returns>Возвращает булево наличия обновлений</returns>
        public bool CheckCfgUpdate()
        {
            return CoreLib.CalculateFileMD5(Path.Combine(FullAppPath, Properties.Settings.Default.CfgDbFile)) == CfgUpdateHash;
        }

        /// <summary>
        /// Конструктор класса. Получает информацию об обновлениях.
        /// </summary>
        public UpdateManager(string AppPath)
        {
            // Сохраняем путь...
            FullAppPath = AppPath;

            // Загружаем и парсим XML...
            DownloadXML();
            ParseXML();
        }
    }
}
