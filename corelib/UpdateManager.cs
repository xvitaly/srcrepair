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
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;

namespace srcrepair.core
{
    /// <summary>
    /// Класс, используемый для проверки обновлений.
    /// </summary>
    public class UpdateManager
    {
        /// <summary>
        /// Хранит последнюю доступную версию приложения.
        /// </summary>
        public Version AppUpdateVersion { get; private set; }

        /// <summary>
        /// Хранит URL для загрузки последней доступной версии приложения.
        /// </summary>
        public string AppUpdateURL { get; private set; }

        /// <summary>
        /// Хранит хеш-сумму установщика последней доступной версии приложения.
        /// </summary>
        public string AppUpdateHash { get; private set; }

        /// <summary>
        /// Хранит URL для загрузки последней доступной версии базы игр.
        /// </summary>
        public string GameUpdateURL { get; private set; }

        /// <summary>
        /// Хранит хеш-сумму последней доступной версии базы игр.
        /// </summary>
        public string GameUpdateHash { get; private set; }

        /// <summary>
        /// Хранит URL для загрузки последней доступной версии базы HUD.
        /// </summary>
        public string HUDUpdateURL { get; private set; }

        /// <summary>
        /// Хранит хеш-сумму последней доступной версии базы HUD.
        /// </summary>
        public string HUDUpdateHash { get; private set; }

        /// <summary>
        /// Хранит URL для загрузки последней доступной версии базы конфигов.
        /// </summary>
        public string CfgUpdateURL { get; private set; }

        /// <summary>
        /// Хранит хеш-сумму последней доступной версии базы конфигов.
        /// </summary>
        public string CfgUpdateHash { get; private set; }

        /// <summary>
        /// Хранит путь к каталогу приложения для служебных целей.
        /// </summary>
        private string FullAppPath;

        /// <summary>
        /// Хранит UserAgent, который будет использоваться в соответствующем
        /// HTTP заголовке запроса.
        /// </summary>
        private string UserAgent;

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
                Downloader.Headers.Add("User-Agent", UserAgent);
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
        /// Проверяет наличие обновлений для приложения.
        /// </summary>
        /// <returns>Возвращает булево наличия обновлений</returns>
        public bool CheckAppUpdate()
        {
            return AppUpdateVersion > Assembly.GetCallingAssembly().GetName().Version;
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
            return FileManager.CalculateFileMD5(Path.Combine(FullAppPath, Properties.Resources.GameListFile)) != GameUpdateHash;
        }

        /// <summary>
        /// Проверяет наличие обновлений для базы HUD.
        /// </summary>
        /// <returns>Возвращает булево наличия обновлений</returns>
        public bool CheckHUDUpdate()
        {
            return FileManager.CalculateFileMD5(Path.Combine(FullAppPath, Properties.Resources.HUDDbFile)) != HUDUpdateHash;
        }

        /// <summary>
        /// Проверяет наличие обновлений для базы FPS-конфигов.
        /// </summary>
        /// <returns>Возвращает булево наличия обновлений</returns>
        public bool CheckCfgUpdate()
        {
            return FileManager.CalculateFileMD5(Path.Combine(FullAppPath, Properties.Resources.CfgDbFile)) != CfgUpdateHash;
        }

        /// <summary>
        /// Генерирует имя файла на диске для обновления.
        /// </summary>
        /// <param name="Url">URL загрузки</param>
        /// <returns>Возвращает имя файла</returns>
        public static string GenerateUpdateFileName(string Url)
        {
            return Path.HasExtension(Url) ? Url : Path.ChangeExtension(Url, "exe");
        }

        /// <summary>
        /// Конструктор класса. Получает информацию об обновлениях.
        /// </summary>
        /// <param name="AppPath">Путь к каталогу приложения</param>
        /// <param name="UA">UserAgent приложения</param>
        public UpdateManager(string AppPath, string UA)
        {
            // Сохраняем путь...
            FullAppPath = AppPath;
            UserAgent = UA;

            // Загружаем и парсим XML...
            DownloadXML();
            ParseXML();
        }
    }
}
