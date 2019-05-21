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
using System.IO;
using System.Xml;
using NLog;

namespace srcrepair.core
{
    /// <summary>
    /// Класс для работы с коллекцией FPS-конфигов.
    /// </summary>
    public sealed class ConfigManager
    {
        /// <summary>
        /// Управляет записью событий в журнал.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Хранит информацию обо всех доступных конфигах.
        /// </summary>
        private List<CFGTlx> Configs;

        /// <summary>
        /// Управляет выбранным конфигом.
        /// </summary>
        public CFGTlx FPSConfig { get; private set; }

        /// <summary>
        /// Получает имена найденных конфигов для указанной игры.
        /// </summary>
        /// <param name="GameID">ID игры</param>
        /// <returns>Возвращает имена найденных конфигов</returns>
        public List<String> GetCfgById(string GameID)
        {
            // Инициализируем список...
            List<String> Result = new List<String>();

            // Выполняем запрос...
            foreach (CFGTlx Cfg in Configs.FindAll(Item => Item.SupportedGames.Exists(ID => ID.Equals(GameID))))
            {
                Result.Add(Cfg.Name);
            }

            // Возвращаем результат...
            return Result;
        }

        /// <summary>
        /// Получает имена всех доступных конфигов.
        /// </summary>
        /// <returns>Возвращает имена всех конфигов</returns>
        public List<String> GetAllCfg()
        {
            List<String> Result = new List<String>();
            foreach (CFGTlx Cfg in Configs) { Result.Add(Cfg.Name); }
            return Result;
        }

        /// <summary>
        /// Выбирает определённый конфиг.
        /// </summary>
        /// <param name="HUDName">Имя конфига, информацию о котором надо получить</param>
        public void Select(string CfgName)
        {
            FPSConfig = Configs.Find(Item => String.Equals(Item.Name, CfgName, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Генерирует массив, содержащий пути к FPS-конфигам.
        /// </summary>
        /// <param name="GamePath">Каталог управляемого приложения</param>
        /// <param name="UserDir">Указывает использует ли управляемое приложение пользовательский каталог</param>
        /// <returns>Возвращает массив с сгенерированными путями до FPS-конфигов</returns>
        public static List<String> ListFPSConfigs(string GamePath, bool UserDir)
        {
            List<String> Result = new List<String> { Path.Combine(GamePath, "cfg", "autoexec.cfg") };
            if (UserDir) { Result.Add(Path.Combine(GamePath, "custom", "autoexec.cfg")); }
            return Result;
        }

        /// <summary>
        /// Устанавливает требуемый FPS-конфиг.
        /// </summary>
        /// <param name="ConfName">Имя конфига</param>
        /// <param name="AppPath">Путь к программе SRC Repair</param>
        /// <param name="GameDir">Путь к каталогу игры</param>
        /// <param name="CustmDir">Флаг использования игрой н. с. к.</param>
        public static void InstallConfigNow(string ConfName, string AppPath, string GameDir, bool CustmDir)
        {
            // Генерируем путь к каталогу установки конфига...
            string DestPath = Path.Combine(GameDir, CustmDir ? Path.Combine("custom", Properties.Settings.Default.UserCustDirName) : String.Empty, "cfg");

            // Проверяем существование каталога и если его не существует - создаём...
            if (!Directory.Exists(DestPath)) { Directory.CreateDirectory(DestPath); }

            // Устанавливаем...
            File.Copy(Path.Combine(AppPath, "cfgs", ConfName), Path.Combine(DestPath, "autoexec.cfg"), true);
        }

        /// <summary>
        /// Конструктор класса. Читает базу данных в формате XML и заполняет нашу структуру.
        /// </summary>
        /// <param name="CfgDbFile">Путь к БД конфигов</param>
        /// <param name="LangPrefix">Языковой код</param>
        public ConfigManager(string CfgDbFile, string LangPrefix)
        {
            // Инициализируем список...
            Configs = new List<CFGTlx>();

            // Получаем полный список доступных конфигов. Открываем поток...
            using (FileStream XMLFS = new FileStream(CfgDbFile, FileMode.Open, FileAccess.Read))
            {
                // Загружаем XML из потока...
                XmlDocument XMLD = new XmlDocument();
                XMLD.Load(XMLFS);

                // Разбираем XML файл и обходим его в цикле...
                for (int i = 0; i < XMLD.GetElementsByTagName("Config").Count; i++)
                {
                    try { Configs.Add(new CFGTlx(XMLD.GetElementsByTagName("Name")[i].InnerText, XMLD.GetElementsByTagName("FileName")[i].InnerText, XMLD.GetElementsByTagName(LangPrefix)[i].InnerText, XMLD.GetElementsByTagName("SupportedGames")[i].InnerText.Split(';'))); } catch (Exception Ex) { Logger.Warn(Ex, "Minor exception while while building Config list object."); }
                }
            }
        }
    }
}
