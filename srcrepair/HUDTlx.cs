using System;
using System.Xml;
using System.IO;

namespace srcrepair
{
    public sealed class HUDTlx
    {
        public string Name;
        public string URI;
        public string Preview;
        public string Site;
        public string ArchiveDir;
        public string InstallDir;
        
        public HUDTlx(string HUDName)
        {
            XmlDocument XMLD = new XmlDocument();
            FileStream XMLFS = new FileStream(Path.Combine(GV.FullAppPath, Properties.Settings.Default.GameListFile), FileMode.Open, FileAccess.Read);
            XMLD.Load(XMLFS);
            for (int i = 0; i < XMLD.GetElementsByTagName("HUD").Count; i++)
            {
                if (String.Compare(XMLD.GetElementsByTagName("Name")[i].InnerText, HUDName, true) == 0)
                {
                    this.Name = XMLD.GetElementsByTagName("Name")[i].InnerText;
                    this.URI = XMLD.GetElementsByTagName("URI")[i].InnerText;
                    this.Preview = XMLD.GetElementsByTagName("Preview")[i].InnerText;
                    this.Site = XMLD.GetElementsByTagName("Site")[i].InnerText;
                    this.ArchiveDir = XMLD.GetElementsByTagName("ArchiveDir")[i].InnerText;
                    this.InstallDir = XMLD.GetElementsByTagName("InstallDir")[i].InnerText;
                    break;
                }
            }
            XMLFS.Close();
        }
    }
}
