/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2018 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2018 EasyCoding Team.
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
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace srcrepair
{
    /// <summary>
    /// Класс для взаимодействия с клиентом Steam.
    /// </summary>
    public sealed class SteamManager
    {
        /// <summary>
        /// Хранит полный путь к каталогу установленного клиента Steam.
        /// </summary>
        public string FullSteamPath { get; set; }

        /// <summary>
        /// Содержит найденные userid профилей Steam.
        /// </summary>
        public List<String> SteamIDs { get; private set; }

        /// <summary>
        /// Управляет текущим выбранным пользователем SteamID.
        /// </summary>
        public string SteamID { get; set; }

        /// <summary>
        /// Проверяет доступен ли переданный в качестве параметра SteamID. Если нет,
        /// то возвращает первый элемент списка из SteamID.
        /// </summary>
        /// <param name="SID">SteamID для проверки</param>
        /// <returns>Возвращает значение SteamID</returns>
        public string GetCurrentSteamID(string SID)
        {
            if (SteamIDs.Count < 1) { throw new ArgumentOutOfRangeException("SteamID list is empty. Can not select one of them."); }
            return SteamIDs.IndexOf(SID) != -1 ? SID : SteamIDs[0];
        }

        /// <summary>
        /// Получает из реестра и возвращает путь к установленному клиенту Steam.
        /// </summary>
        /// <returns>Путь к клиенту Steam</returns>
        private string GetSteamPath()
        {
            // Создаём строку для хранения результатов...
            string ResString = String.Empty;

            // Подключаем реестр и открываем ключ только для чтения...
            using (RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam", false))
            {
                // Проверяем чтобы ключ реестр существовал и был доступен...
                if (ResKey != null)
                {
                    // Получаем значение открытого ключа...
                    object ResObj = ResKey.GetValue("SteamPath");

                    // Проверяем чтобы значение существовало...
                    if (ResObj != null)
                    {
                        // Существует, возвращаем...
                        ResString = Path.GetFullPath(Convert.ToString(ResObj));
                    }
                    else
                    {
                        // Значение не существует, поэтому сгенерируем исключение для обработки в основном коде...
                        throw new NullReferenceException("Exception: No InstallPath value detected! Please run Steam.");
                    }
                }
            }

            // Возвращаем результат...
            return ResString;
        }

        /// <summary>
        /// Получает из реестра и возвращает текущий язык клиента Steam.
        /// </summary>
        /// <returns>Язык клиента Steam</returns>
        public string GetSteamLanguage()
        {
            string Result = String.Empty; using (RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam", false)) { if (ResKey != null) { object ResObj = ResKey.GetValue("Language"); if (ResObj != null) { Result = Convert.ToString(ResObj); } else { throw new NullReferenceException("Exception: No Language value detected! Please run Steam."); } } } return Result;
        }

        /// <summary>
        /// Тестирует переданный каталог в качестве пути установки Steam.
        /// </summary>
        /// <returns>Каталог установки Steam</returns>
        public static string TrySteamPath(string SteamPath)
        {
            if (Directory.Exists(SteamPath)) { return SteamPath; } else { throw new DirectoryNotFoundException(); }
        }

        /// <summary>
        /// Возвращает путь к главному VDF конфигу Steam.
        /// </summary>
        /// <returns>Путь к VDF конфигу</returns>
        public string GetSteamConfig()
        {
            return Path.Combine(FullSteamPath, "config", "config.vdf");
        }

        /// <summary>
        /// Возвращает путь к локально хранящемуся VDF конфигу Steam.
        /// </summary>
        /// <returns>Путь к локальному VDF конфигу</returns>
        public List<String> GetSteamLocalConfig()
        {
            List<String> Result = new List<String>();
            foreach (string ID in GetUserIDs())
            {
                Result.AddRange(FileManager.FindFiles(Path.Combine(FullSteamPath, "userdata", ID, "config"), "localconfig.vdf"));
            }
            return Result;
        }

        /// <summary>
        /// Возвращает список используемых на данном компьютере SteamID.
        /// </summary>
        /// <returns>Список Steam user ID</returns>
        private List<String> GetUserIDs()
        {
            // Создаём новый список...
            List<String> Result = new List<String>();

            // Получаем список каталогов...
            string DDir = Path.Combine(FullSteamPath, "userdata");
            if (Directory.Exists(DDir))
            {
                DirectoryInfo DInfo = new DirectoryInfo(DDir);
                foreach (DirectoryInfo SubDir in DInfo.GetDirectories())
                {
                    Result.Add(SubDir.Name);
                }
            }

            // Возвращаем результат...
            return Result;
        }

        /// <summary>
        /// Очищает блобы (файлы с расширением *.blob) из каталога Steam.
        /// </summary>
        public void CleanBlobsNow()
        {
            // Инициализируем буферную переменную, в которой будем хранить имя файла...
            string FileName;

            // Генерируем имя первого кандидата на удаление с полным путём до него...
            FileName = Path.Combine(FullSteamPath, "AppUpdateStats.blob");

            // Проверяем существует ли данный файл...
            if (File.Exists(FileName))
            {
                // Удаляем...
                File.Delete(FileName);
            }

            // Аналогично генерируем имя второго кандидата...
            FileName = Path.Combine(FullSteamPath, "ClientRegistry.blob");

            // Проверяем, существует ли файл...
            if (File.Exists(FileName))
            {
                // Удаляем...
                File.Delete(FileName);
            }
        }

        /// <summary>
        /// Удаляет значения реестра, отвечающие за настройки клиента Steam,
        /// а также записывает значение языка.
        /// </summary>
        /// <param name="LangCode">ID языка Steam</param>
        public void CleanRegistryNow(int LangCode)
        {
            // Удаляем ключ HKEY_LOCAL_MACHINE\Software\Valve рекурсивно (если есть права администратора)...
            if (ProcessManager.IsCurrentUserAdmin()) { Registry.LocalMachine.DeleteSubKeyTree(Path.Combine("Software", "Valve"), false); }

            // Удаляем ключ HKEY_CURRENT_USER\Software\Valve рекурсивно...
            Registry.CurrentUser.DeleteSubKeyTree(Path.Combine("Software", "Valve"), false);

            // Начинаем вставлять значение языка клиента Steam...
            // Инициализируем буферную переменную для хранения названия языка...
            string XLang;

            // Генерируем...
            switch (LangCode)
            {
                case 0:
                    XLang = "english";
                    break;
                case 1:
                    XLang = "russian";
                    break;
                default:
                    XLang = "english";
                    break;
            }

            // Подключаем реестр и создаём ключ HKEY_CURRENT_USER\Software\Valve\Steam...
            RegistryKey RegLangKey = Registry.CurrentUser.CreateSubKey(Path.Combine("Software", "Valve", "Steam"));

            // Если не было ошибок, записываем значение...
            if (RegLangKey != null)
            {
                // Записываем значение в реестр...
                RegLangKey.SetValue("language", XLang);
            }

            // Закрываем ключ...
            RegLangKey.Close();
        }

        /// <summary>
        /// Считывает из главного файла конфигурации Steam пути к дополнительным точкам монтирования.
        /// </summary>
        private List<String> GetSteamMountPoints()
        {
            // Создаём массив, в который будем помещать найденные пути...
            List<String> Result = new List<String> { FullSteamPath };

            // Начинаем чтение главного файла конфигурации...
            try
            {
                // Открываем файл как поток...
                using (StreamReader SteamConfig = new StreamReader(Path.Combine(FullSteamPath, "config", "config.vdf"), Encoding.Default))
                {
                    // Инициализируем буферную переменную...
                    string RdStr;

                    // Читаем поток построчно...
                    while (SteamConfig.Peek() >= 0)
                    {
                        // Считываем строку и сразу очищаем от лишнего...
                        RdStr = SteamConfig.ReadLine().Trim();

                        // Проверяем наличие данных в строке...
                        if (!(String.IsNullOrWhiteSpace(RdStr)))
                        {
                            // Ищем в строке путь установки...
                            if (RdStr.IndexOf("BaseInstallFolder", StringComparison.CurrentCultureIgnoreCase) != -1)
                            {
                                RdStr = CoreLib.CleanStrWx(RdStr, true, true);
                                RdStr = RdStr.Remove(0, RdStr.IndexOf(" ") + 1);
                                if (!(String.IsNullOrWhiteSpace(RdStr))) { Result.Add(RdStr); }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                CoreLib.WriteStringToLog(Ex.Message);
            }

            // Возвращаем сформированный массив...
            return Result;
        }

        /// <summary>
        /// Формирует полные пути к библиотекам с установленными играми.
        /// </summary>
        /// <param name="SteamAppsFolderName">Платформо-зависимое название каталога SteamApps</param>
        public List<String> FormatInstallDirs(string SteamAppsFolderName)
        {
            // Создаём массив, в который будем помещать найденные пути...
            List<String> Result = new List<String>();

            // Считываем все возможные расположения локальных библиотек игр...
            List<String> MntPnts = GetSteamMountPoints();

            // Начинаем обход каталога и получение поддиректорий...
            foreach (string MntPnt in MntPnts)
            {
                Result.Add(Path.Combine(MntPnt, SteamAppsFolderName, "common"));
            }

            // Возвращаем сформированный массив...
            return Result;
        }

        /// <summary>
        /// Главный конструктор класса SteamManager.
        /// </summary>
        public SteamManager(CurrentPlatform.OSType OS)
        {
            // Получим путь к Steam...
            FullSteamPath = OS == CurrentPlatform.OSType.Windows ? GetSteamPath() : TrySteamPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Steam"));
            SteamIDs = GetUserIDs();
            SteamID = GetCurrentSteamID(Properties.Settings.Default.LastSteamID);
        }
    }
}
