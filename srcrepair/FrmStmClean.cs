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
using System.Windows.Forms;
using System.IO;
using srcrepair.core;

namespace srcrepair.gui
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
        /// Хранит платформо-зависимое имя процесса Steam.
        /// </summary>
        private string SteamProcName { get; set; }

        /// <summary>
        /// Конструктор класса формы модуля очистки кэшей Steam.
        /// </summary>
        /// <param name="SP">Каталог установки Steam</param>
        /// <param name="BD">Каталог хранения резервных копий</param>
        /// <param name="BD">Платформо-зависимое название каталога SteamApps</param>
        /// <param name="BD">Платформо-зависимое имя процесса Steam</param>
        public FrmStmClean(string SP, string BD, string SA, string PN)
        {
            InitializeComponent();
            SteamPath = SP;
            BackUpDir = BD;
            SteamAppsDirName = SA;
            SteamProcName = PN;
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

            if (EC_ShaderCache.Checked)
            {
                // Очистим кэш шейдеров клиента Steam...
                CleanDirs.Add(Path.Combine(SteamPath, "shadercache", "*.*"));
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
                CleanDirs.Add(Path.Combine(SteamPath, SteamAppsDirName, "downloading"));
                CleanDirs.Add(Path.Combine(SteamPath, SteamAppsDirName, "temp"));
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
            if (CleanDirs.Count > 0) { FormManager.FormShowCleanup(CleanDirs, Text, AppStrings.PS_CleanupSuccess, BackUpDir, SteamProcName); } else { MessageBox.Show(AppStrings.AC_NoItemsSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
    }
}
