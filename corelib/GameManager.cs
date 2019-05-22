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
    /// Класс для работы с коллекцией установленных поддерживаемых игр.
    /// </summary>
    public sealed class GameManager
    {
        /// <summary>
        /// Управляет записью событий в журнал.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Хранит информацию о всех доступных поддерживаемых играх.
        /// </summary>
        private List<SourceGame> SourceGames;

        /// <summary>
        /// Хранит названия установленных игр в виде списка. Используется
        /// в основном селектором игр в главном окне.
        /// </summary>
        public List<String> InstalledGames { get; private set; }

        /// <summary>
        /// Хранит информацию о выбранной игре. Для заполнения используется метод Select().
        /// </summary>
        public SourceGame SelectedGame { get; private set; }

        /// <summary>
        /// Выбирает определённую игру по имени.
        /// </summary>
        /// <param name="GameName">Имя игры</param>
        public void Select(string GameName)
        {
            SelectedGame = SourceGames.Find(Item => String.Equals(Item.FullAppName, GameName, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Главный конструктор класса GameManager.
        /// </summary>
        /// <param name="App">Экземпляр класса с параметрами приложения</param>
        /// <param name="GameDbFile">Имя файла с базой игр</param>
        /// <param name="HideUnsupported">Добавлять ли в список неподдерживаемые</param>
        public GameManager(CurrentApp App, string GameDbFile, bool HideUnsupported)
        {
            // Создаём объекты для хранения базы игр...
            SourceGames = new List<SourceGame>();
            InstalledGames = new List<String>();

            // При использовании нового метода поиска установленных игр, считаем их из конфига Steam...
            List<String> GameDirs = App.SteamClient.FormatInstallDirs(App.Platform.SteamAppsFolderName);

            // Создаём поток с XML-файлом...
            using (FileStream XMLFS = new FileStream(Path.Combine(App.FullAppPath, GameDbFile), FileMode.Open, FileAccess.Read))
            {
                // Создаём объект документа XML...
                XmlDocument XMLD = new XmlDocument();

                // Загружаем поток в объект XML документа...
                XMLD.Load(XMLFS);

                // Обходим полученный список в цикле...
                XmlNodeList XMLNode = XMLD.GetElementsByTagName("Game");
                for (int i = 0; i < XMLNode.Count; i++)
                {
                    try
                    {
                        if (XMLD.GetElementsByTagName("Enabled")[i].InnerText == "1" || !HideUnsupported)
                        {
                            SourceGame SG = new SourceGame(XMLNode[i].Attributes["Name"].Value, XMLD.GetElementsByTagName("DirName")[i].InnerText, XMLD.GetElementsByTagName("SmallName")[i].InnerText, XMLD.GetElementsByTagName("Executable")[i].InnerText, XMLD.GetElementsByTagName("SID")[i].InnerText, XMLD.GetElementsByTagName("SVer")[i].InnerText, XMLD.GetElementsByTagName("VFDir")[i].InnerText, App.Platform.OS == CurrentPlatform.OSType.Windows ? XMLD.GetElementsByTagName("HasVF")[i].InnerText == "1" : true, XMLD.GetElementsByTagName("UserDir")[i].InnerText == "1", XMLD.GetElementsByTagName("HUDsAvail")[i].InnerText == "1", App.FullAppPath, App.AppUserDir, App.SteamClient.FullSteamPath, App.Platform.SteamAppsFolderName, App.SteamClient.SteamID, GameDirs, App.Platform.OS);
                            if (SG.IsInstalled)
                            {
                                SourceGames.Add(SG);
                                InstalledGames.Add(SG.FullAppName);
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        Logger.Warn(Ex, "Minor exception while building games list object.");
                    }
                }
            }
        }
    }
}
