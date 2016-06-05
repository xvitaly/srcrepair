/*
 * Класс системы управления FPS-конфигами.
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace srcrepair
{
    /// <summary>
    /// Класс для работы с коллекцией FPS-конфигов.
    /// </summary>
    class ConfigManager
    {
        /// <summary>
        /// Хранит информацию обо всех доступных конфигах.
        /// </summary>
        private List<CFGTlx> Configs;

        /// <summary>
        /// Управляет выбранным конфигом.
        /// </summary>
        public CFGTlx FPSConfig { get; set; }

        /// <summary>
        /// Получает имена найденных конфигов для указанной игры.
        /// </summary>
        /// <param name="GameID">ID игры</param>
        /// <returns>Возвращает имена найденных конфигов</returns>
        public List<String> GetCfgNames(string GameID)
        {
            // Инициализируем список...
            List<String> Result = new List<String>();

            // Выполняем запрос посредством LINQ...
            foreach (CFGTlx Cfg in Configs.FindAll(Item => Item.SupportedGames.Exists(ID => ID.Equals(GameID))))
            {
                Result.Add(Cfg.Name);
            }

            // Возвращаем результат...
            return Result;
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
                    try { Configs.Add(new CFGTlx(XMLD.GetElementsByTagName("Name")[i].InnerText, XMLD.GetElementsByTagName("FileName")[i].InnerText, XMLD.GetElementsByTagName(LangPrefix)[i].InnerText, XMLD.GetElementsByTagName("SupportedGames")[i].InnerText.Split(','))); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                }
            }
        }
    }
}
