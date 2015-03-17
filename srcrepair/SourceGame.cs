/*
 * Класс SourceEngine SRC Repair.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace srcrepair
{
    public sealed class SourceGame
    {
        /// <summary>
        /// В этой переменной будем хранить путь установки кастомных файлов.
        /// </summary>
        public string CustomInstallDir;

        /// <summary>
        /// В этой переменной будем хранить полный путь к каталогу игры, которой
        /// мы будем управлять данной утилитой.
        /// </summary>
        public static string FullGamePath;

        /// <summary>
        /// В этой переменной будем хранить полный путь к каталогу игры без
        /// включения в путь GV.SmallAppName для служебных целей.
        /// </summary>
        public string GamePath;

        /// <summary>
        /// В этой переменной будем хранить полное имя управляемого приложения
        /// для служебных целей.
        /// </summary>
        public string FullAppName;

        /// <summary>
        /// В этой переменной мы будем хранить краткое имя управляемого приложения
        /// для служебных целей (SteamAlias).
        /// </summary>
        public string SmallAppName;

        /// <summary>
        /// В этой переменной мы будем хранить полный путь до каталога с
        /// файлами конфигурации управляемого приложения.
        /// </summary>
        public string FullCfgPath;

        /// <summary>
        /// В этой переменной мы будем хранить полный путь до каталога с
        /// резервными копиями управляемого приложения.
        /// </summary>
        public string FullBackUpDirPath;

        /// <summary>
        /// Указывает использует ли игра файл video.txt для хранения
        /// своих настроек.
        /// </summary>
        public bool IsUsingVideoFile;

        /// <summary>
        /// Определяет использует ли игра специальный каталог для хранения
        /// пользовательских настроек и скриптов.
        /// </summary>
        public bool IsUsingUserDir;

        /// <summary>
        /// Эта переменная хранит ID игры по базе данных Steam. Используется
        /// для служебных целей.
        /// </summary>
        public string GameInternalID;

        /// <summary>
        /// В этой переменной хранится путь к файлу с настройками видео,
        /// используется в NCF-играх.
        /// </summary>
        public string VideoCfgFile;

        /// <summary>
        /// Содержит имя каталога с конфигами. Используется в последних
        /// играх.
        /// </summary>
        public string ConfDir;

        /// <summary>
        /// Содержит пути к установленным FPS-конфигам управляемой игры.
        /// </summary>
        public List<String> FPSConfigs;

        /// <summary>
        /// Конструктор класса. Получает информацию о выбранном приложении.
        /// </summary>
        /// <param name="GameName">Название выбранного приложения</param>
        public SourceGame(string AppName)
        {
            // Начинаем определять нужные нам значения переменных...
            try
            {
                XmlDocument XMLD = new XmlDocument();
                FileStream XMLFS = new FileStream(Path.Combine(GV.FullAppPath, Properties.Settings.Default.GameListFile), FileMode.Open, FileAccess.Read);
                XMLD.Load(XMLFS);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("Game");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    if (String.Compare(XMLD.GetElementsByTagName("DirName")[i].InnerText, Path.GetFileName(AppSelector.Text), true) == 0)
                    {
                        GV.FullAppName = XMLD.GetElementsByTagName("DirName")[i].InnerText;
                        GV.SmallAppName = XMLD.GetElementsByTagName("SmallName")[i].InnerText;
                        GV.GameInternalID = XMLD.GetElementsByTagName("SID")[i].InnerText;
                        GV.ConfDir = XMLD.GetElementsByTagName("VFDir")[i].InnerText;
                        GV.IsUsingVideoFile = XMLD.GetElementsByTagName("HasVF")[i].InnerText == "1";
                        GV.IsUsingUserDir = XMLD.GetElementsByTagName("UserDir")[i].InnerText == "1";
                        break;
                    }
                }
                XMLFS.Close();
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("AppXMLParseError"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Error);
            }
        }
    }
}
