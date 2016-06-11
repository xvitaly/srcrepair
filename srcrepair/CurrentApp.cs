/*
 * Класс CurrentApp SRC Repair.
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
using System.Reflection;
using System.IO;
using System.Globalization;

namespace srcrepair
{
    public sealed class CurrentApp
    {
        /// <summary>
        /// Хранит User-Agent, которым представляется удалённым службам...
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// В этой переменной мы будем хранить полный путь к каталогу установленного
        /// клиента Steam.
        /// </summary>
        public string FullSteamPath { get; set; }

        /// <summary>
        /// В этой переменной будем хранить полный путь к каталогу с утилитой
        /// SRCRepair для служебных целей.
        /// </summary>
        public string FullAppPath { get; set; }

        /// <summary>
        /// В этой переменной будем хранить путь до каталога пользователя
        /// программы. Используется для служебных целей.
        /// </summary>
        public string AppUserDir { get; set; }

        /// <summary>
        /// В этой переменной мы будем хранить полную информацию о версии
        /// приложения для служебных целей.
        /// </summary>
        public string AppVersionInfo { get; set; }

        /// <summary>
        /// Конструктор класса. Получает информацию для рантайма.
        /// </summary>
        public CurrentApp()
        {
            // Получаем путь к каталогу приложения...
            Assembly Assmbl = Assembly.GetEntryAssembly();
            FullAppPath = Path.GetDirectoryName(Assmbl.Location);

            // Укажем путь к пользовательским данным и создадим если не существует...
            AppUserDir = Properties.Settings.Default.IsPortable ? Path.Combine(FullAppPath, "portable") : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Properties.Resources.AppName);

            // Проверим существование каталога пользовательских данных и при необходимости создадим...
            if (!(Directory.Exists(AppUserDir)))
            {
                Directory.CreateDirectory(AppUserDir);
            }

            // Получаем информацию о версии нашего приложения...
            AppVersionInfo = Assmbl.GetName().Version.ToString();

            // Генерируем User-Agent для SRC Repair...
            UserAgent = String.Format(Properties.Resources.AppDefUA, Properties.Resources.PlatformFriendlyName, Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor, CultureInfo.CurrentCulture.Name, AppVersionInfo, Properties.Resources.AppName, CoreLib.GetSystemArch());
        }
    }
}
