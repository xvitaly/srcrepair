using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace srcrepair
{
    public sealed class HUDManager
    {
        private List<HUDTlx> HUDsAvailable;
        public HUDTlx SelectedHUD;

        public void Select(string HUDName)
        {
            SelectedHUD = HUDsAvailable.Find(Item => String.Equals(Item.Name, HUDName, StringComparison.CurrentCultureIgnoreCase));
        }

        public List<String> GetHUDNames(string GameName)
        {
            // Инициализируем список...
            List<String> Result = new List<String>();

            // Выполняем запрос...
            foreach (HUDTlx HUD in HUDsAvailable.FindAll(Item => String.Equals(Item.Game, GameName, StringComparison.CurrentCultureIgnoreCase)))
            {
                Result.Add(HUD.Name);
            }

            // Возвращаем результат...
            return Result;
        }

        public HUDManager(string HUDDbFile, string AppHUDDir)
        {
            // Инициализируем наш список...
            HUDsAvailable = new List<HUDTlx>();

            // Получаем полный список доступных HUD для данной игры. Открываем поток...
            using (FileStream XMLFS = new FileStream(HUDDbFile, FileMode.Open, FileAccess.Read))
            {
                // Загружаем XML из потока...
                XmlDocument XMLD = new XmlDocument();
                XMLD.Load(XMLFS);

                // Разбираем XML файл и обходим его в цикле...
                for (int i = 0; i < XMLD.GetElementsByTagName("HUD").Count; i++)
                {
                    try { HUDsAvailable.Add(new HUDTlx(AppHUDDir, XMLD.GetElementsByTagName("Name")[i].InnerText, XMLD.GetElementsByTagName("Game")[i].InnerText, XMLD.GetElementsByTagName("URI")[i].InnerText, XMLD.GetElementsByTagName("UpURI")[i].InnerText, XMLD.GetElementsByTagName("IsUpdated")[i].InnerText == "1", XMLD.GetElementsByTagName("Preview")[i].InnerText, XMLD.GetElementsByTagName("Site")[i].InnerText, XMLD.GetElementsByTagName("ArchiveDir")[i].InnerText, XMLD.GetElementsByTagName("InstallDir")[i].InnerText, Path.Combine(AppHUDDir, Path.ChangeExtension(Path.GetFileName(XMLD.GetElementsByTagName("Name")[i].InnerText), ".zip")))); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                }
            }
        }
    }
}
