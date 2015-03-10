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
        public string IntDir;
        public string LocalFile;
        
        public HUDTlx(string HUDName)
        {
            XmlDocument XMLD = new XmlDocument();
            FileStream XMLFS = new FileStream(Path.Combine(GV.FullAppPath, Properties.Settings.Default.HUDDbFile), FileMode.Open, FileAccess.Read);
            XMLD.Load(XMLFS);
            for (int i = 0; i < XMLD.GetElementsByTagName("HUD").Count; i++)
            {
                if (String.Compare(XMLD.GetElementsByTagName("Name")[i].InnerText, HUDName, true) == 0)
                {
                    this.Name = XMLD.GetElementsByTagName("Name")[i].InnerText;
                    this.URI = XMLD.GetElementsByTagName("URI")[i].InnerText;
                    this.Preview = XMLD.GetElementsByTagName("Preview")[i].InnerText;
                    this.Site = XMLD.GetElementsByTagName("Site")[i].InnerText;
                    this.IntDir = XMLD.GetElementsByTagName("IntDir")[i].InnerText;
                    this.LocalFile = Path.Combine(GV.AppHUDDir, Path.ChangeExtension(Path.GetFileName(this.Name), ".zip"));
                    break;
                }
            }
            XMLFS.Close();
        }
    }
}
