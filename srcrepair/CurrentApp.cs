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
using System.Globalization;
using System.IO;
using System.Reflection;
using NLog;

namespace srcrepair.core
{
    /// <summary>
    /// Класс работы с рантаймом.
    /// </summary>
    public sealed class CurrentApp
    {
        /// <summary>
        /// Хранит User-Agent, которым представляется удалённым службам...
        /// </summary>
        public string UserAgent { get; private set; }

        /// <summary>
        /// Хранит полный путь к каталогу с утилитой SRCRepair для служебных
        /// целей.
        /// </summary>
        public string FullAppPath { get; private set; }

        /// <summary>
        /// Хранить путь до каталога пользователя программы. Используется
        /// для служебных целей.
        /// </summary>
        public string AppUserDir { get; private set; }

        /// <summary>
        /// Хранит и возвращает код текущей платформы, на которой запущено приложение.
        /// </summary>
        public CurrentPlatform Platform { get; private set; }

        /// <summary>
        /// Управляет базой доступных для управления игр.
        /// </summary>
        public GameManager SourceGames { get; set; }

        /// <summary>
        /// Управляет настройками клиента Steam.
        /// </summary>
        public SteamManager SteamClient { get; set; }

        /// <summary>
        /// Возвращает архитектуру операционной системы.
        /// </summary>
        private string SystemArch { get { return Environment.Is64BitOperatingSystem ? "Amd64" : "x86"; } }

        /// <summary>
        /// Возвращает полный путь к используему файлу журнала Nlog.
        /// </summary>
        public string LogFileName
        {
            get
            {
                NLog.Targets.FileTarget LogTarget = (NLog.Targets.FileTarget)LogManager.Configuration.FindTargetByName("logfile");
                return Path.GetFullPath(LogTarget.FileName.Render(new LogEventInfo()));
            }
        }

        /// <summary>
        /// Возвращает путь к пользовательскому каталогу программы.
        /// </summary>
        public static string AppUserPath { get { return Properties.Settings.Default.IsPortable ? Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "portable") : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Properties.Resources.AppName); } }
        
        /// <summary>
        /// Возвращает название продукта (из ресурса сборки).
        /// </summary>
        public static string AppProduct { get { object[] Attribs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false); return Attribs.Length != 0 ? ((AssemblyProductAttribute)Attribs[0]).Product : String.Empty; } }

        /// <summary>
        /// Возвращает версию приложения ((из ресурса сборки).
        /// </summary>
        public static string AppVersion { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        /// <summary>
        /// Возвращает название компании-разработчика (из ресурса сборки).
        /// </summary>
        public static string AppCompany { get { object[] Attribs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false); return Attribs.Length != 0 ? ((AssemblyCompanyAttribute)Attribs[0]).Company : String.Empty; } }

        /// <summary>
        /// Возвращает копирайты приложения (из ресурса сборки).
        /// </summary>
        public static string AppCopyright { get { object[] Attribs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false); return Attribs.Length != 0 ? ((AssemblyCopyrightAttribute)Attribs[0]).Copyright : String.Empty; } }

        /// <summary>
        /// Конструктор класса. Получает информацию для рантайма.
        /// </summary>
        public CurrentApp()
        {
            // Получим информацию о платформе, на которой запущено приложение...
            Platform = new CurrentPlatform();

            // Получаем путь к каталогу приложения...
            FullAppPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            // Укажем путь к пользовательским данным и создадим если не существует...
            AppUserDir = AppUserPath;

            // Проверим существование каталога пользовательских данных и при необходимости создадим...
            if (!(Directory.Exists(AppUserDir)))
            {
                Directory.CreateDirectory(AppUserDir);
            }

            // Генерируем User-Agent для SRC Repair...
            UserAgent = String.Format(Properties.Resources.AppDefUA, Platform.OSFriendlyName, Platform.UASuffix, Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor, CultureInfo.CurrentCulture.Name, AppVersion, Properties.Resources.AppName, SystemArch);
        }
    }
}
