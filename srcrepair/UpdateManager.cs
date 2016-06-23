/*
 * Класс системы проверки обновлений.
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

namespace srcrepair
{
    /// <summary>
    /// Класс для проверки обновлений.
    /// </summary>
    public class UpdateManager
    {
        private string AppUpdateVersion;
        private string AppUpdateURL;
        private string AppUpdateHash;

        private string GameUpdateURL;
        private string GameUpdateHash;

        private string HUDUpdateURL;
        private string HUDUpdateHash;

        private string CfgUpdateURL;
        private string CfgUpdateHash;

        private string FullAppPath;

        public bool CheckGameDBUpdate()
        {
            return CoreLib.CalculateFileMD5(Path.Combine(FullAppPath, Properties.Settings.Default.GameListFile)) == GameUpdateHash;
        }

        public bool CheckHUDUpdate()
        {
            return CoreLib.CalculateFileMD5(Path.Combine(FullAppPath, Properties.Settings.Default.HUDDbFile)) == HUDUpdateHash;
        }

        public bool CheckCfgUpdate()
        {
            return CoreLib.CalculateFileMD5(Path.Combine(FullAppPath, Properties.Settings.Default.CfgDbFile)) == CfgUpdateHash;
        }

        public UpdateManager(string AppPath)
        {
            //
        }
    }
}
