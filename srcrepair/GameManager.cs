using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace srcrepair
{
    public sealed class GameManager
    {
        private List<SourceGame> SourceGames;
        public SourceGame SelectedGame { get; private set; }

        public void Select(string GameName)
        {
            SelectedGame = SourceGames.Find(Item => String.Equals(Item.FullAppName, GameName, StringComparison.CurrentCultureIgnoreCase));
        }

        public GameManager(CurrentApp App)
        {
            // При использовании нового метода поиска установленных игр, считаем их из конфига Steam...
            List<String> GameDirs = SteamManager.FormatInstallDirs(App.FullSteamPath, App.Platform.SteamAppsFolderName);

            // Создаём поток с XML-файлом...
            using (FileStream XMLFS = new FileStream(Path.Combine(App.FullAppPath, Properties.Resources.GameListFile), FileMode.Open, FileAccess.Read))
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
                        if (XMLD.GetElementsByTagName("Enabled")[i].InnerText == "1" || !Properties.Settings.Default.HideUnsupportedGames)
                        {
                            SourceGame SG = new SourceGame(XMLNode[i].Attributes["Name"].Value, XMLD.GetElementsByTagName("DirName")[i].InnerText, XMLD.GetElementsByTagName("SmallName")[i].InnerText, XMLD.GetElementsByTagName("Executable")[i].InnerText, XMLD.GetElementsByTagName("SID")[i].InnerText, XMLD.GetElementsByTagName("SVer")[i].InnerText, XMLD.GetElementsByTagName("VFDir")[i].InnerText, App.Platform.OS == CurrentPlatform.OSType.Windows ? XMLD.GetElementsByTagName("HasVF")[i].InnerText == "1" : true, XMLD.GetElementsByTagName("UserDir")[i].InnerText == "1", XMLD.GetElementsByTagName("HUDsAvail")[i].InnerText == "1", App.FullAppPath, App.AppUserDir, App.FullSteamPath, App.Platform.SteamAppsFolderName, GameDirs);
                            if (SG.IsInstalled)
                            {
                                SourceGames.Add(SG);
                            }
                        }
                    }
                    catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                }
            }
        }
    }
}
