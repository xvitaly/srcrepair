/*
 * Модуль выбора элементов для очистки Steam.
 * 
 * Copyright 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2017 EasyCoding Team.
 * 
 * Лицензия: GPL v3 (см. файл GPL.txt).
 * Лицензия контента: Creative Commons 3.0 BY.
 * 
 * Запрещается использовать этот файл при использовании любой
 * лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
 * 
 * Официальный блог EasyCoding Team: https://www.easycoding.org/
 * Официальная страница проекта: https://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля очистки кэшей Steam.
    /// </summary>
    public partial class FrmStmClean : Form
    {
        /// <summary>
        /// Хранит путь к каталогу установки клиента Steam.
        /// </summary>
        private string SteamPath { get; set; }

        /// <summary>
        /// Хранит путь к каталогу хранения резервных копий.
        /// </summary>
        private string BackUpDir { get; set; }

        /// <summary>
        /// Хранит платформо-зависимое название каталога SteamApps.
        /// </summary>
        private string SteamAppsDirName { get; set; }

        /// <summary>
        /// Конструктор класса формы модуля очистки кэшей Steam.
        /// </summary>
        /// <param name="SP">Каталог установки Steam</param>
        /// <param name="BD">Каталог хранения резервных копий</param>
        /// <param name="BD">Платформо-зависимое название каталога SteamApps</param>
        public FrmStmClean(string SP, string BD, string SA)
        {
            InitializeComponent();
            SteamPath = SP;
            BackUpDir = BD;
            SteamAppsDirName = SA;
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку запуска очистки.
        /// </summary>
        private void EC_Execute_Click(object sender, EventArgs e)
        {
            // Создаём список файлов и каталогов для очистки...
            List<String> CleanDirs = new List<string>();

            // Очистим HTML-кэш внутреннего браузера Steam...
            if (EC_HTMLCache.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "config", "htmlcache", "*.*"));
                CleanDirs.Add(Path.Combine(SteamPath, "config", "overlayhtmlcache", "*.*"));
                CleanDirs.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Steam", "htmlcache", "*.*"));
            }

            if (EC_HTTPCache.Checked)
            {
                // Очистим HTTP-кэш...
                CleanDirs.Add(Path.Combine(SteamPath, "appcache", "httpcache", "*.*"));
            }

            if (EC_DepotCache.Checked)
            {
                // Очистим Depot-кэш...
                CleanDirs.Add(Path.Combine(SteamPath, "depotcache", "*.*"));
            }

            if (EC_Logs.Checked)
            {
                // Очистим логи клиента Steam...
                CleanDirs.Add(Path.Combine(SteamPath, "logs", "*.*"));
                CleanDirs.Add(Path.Combine(SteamPath, "*.log*"));
            }

            if (EC_OldBins.Checked)
            {
                // Удаляем старые бинарные файлы клиента Steam...
                CleanDirs.Add(Path.Combine(SteamPath, "*.old"));
                CleanDirs.Add(Path.Combine(SteamPath, "bin", "*.old"));
            }

            if (EC_ErrDmps.Checked)
            {
                // Очистим краш-дампы...
                CleanDirs.Add(Path.Combine(SteamPath, "dumps", "*.*"));
                CleanDirs.Add(Path.Combine(SteamPath, "*.dmp"));
                CleanDirs.Add(Path.Combine(SteamPath, "*.mdmp"));
            }

            if (EC_BuildCache.Checked)
            {
                // Очистим кэш сборки обновлений игр с новой системой контента...
                CleanDirs.Add(Path.Combine(SteamPath, Properties.Resources.SteamAppsFolderName, "downloading"));
                CleanDirs.Add(Path.Combine(SteamPath, Properties.Resources.SteamAppsFolderName, "temp"));
            }

            if (EC_GameIcons.Checked)
            {
                // Очистим кэшированные значки игр...
                CleanDirs.Add(Path.Combine(SteamPath, "steam", "games", "*.*"));
            }

            if (EC_Cloud.Checked)
            {
                // Очистим локальное зеркало Cloud...
                CleanDirs.Add(Path.Combine(SteamPath, "userdata", "*.*"));
            }

            if (EC_Stats.Checked)
            {
                // Очистим кэшированную статистику игр...
                CleanDirs.Add(Path.Combine(SteamPath, "appcache", "stats", "*.*"));
            }

            if (EC_Music.Checked)
            {
                // Очистим базу данных функции Steam Music...
                CleanDirs.Add(Path.Combine(SteamPath, "music", "_database", "*.*"));
            }

            if (EC_Skins.Checked)
            {
                // Очистим установленные скины Steam...
                CleanDirs.Add(Path.Combine(SteamPath, "skins", "*.*"));
            }

            if (EC_Updater.Checked)
            {
                // Очистим кэш обновлений Steam...
                CleanDirs.Add(Path.Combine(SteamPath, "package", "*.*"));
            }

            if (EC_Guard.Checked)
            {
                // Удаляем кэш Steam Guard...
                CleanDirs.Add(Path.Combine(SteamPath, "ssfn*"));
            }

            // Запустим очистку...
            if (CleanDirs.Count > 0) { FormManager.FormShowCleanup(CleanDirs, Text, AppStrings.PS_CleanupSuccess, BackUpDir, Properties.Resources.SteamProcName); } else { MessageBox.Show(AppStrings.AC_NoItemsSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
    }
}
