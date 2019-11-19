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

namespace srcrepair.gui
{
    /// <summary>
    /// Class of Steam garbage cleaner window.
    /// </summary>
    public partial class FrmStmClean : Form
    {
        /// <summary>
        /// Gets or sets full path to Steam client directory.
        /// </summary>
        private string SteamPath { get; set; }

        /// <summary>
        /// Gets or sets full path to directory for saving backups.
        /// </summary>
        private string BackUpDir { get; set; }

        /// <summary>
        /// Gets or sets platform-dependent SteamApps directory name.
        /// </summary>
        private string SteamAppsDirName { get; set; }

        /// <summary>
        /// Get or sets platform-dependent Steam process name.
        /// </summary>
        private string SteamProcName { get; set; }

        /// <summary>
        /// FrmStmClean class constructor.
        /// </summary>
        /// <param name="SP">Full path to Steam client directory.</param>
        /// <param name="BD">Full path to directory for saving backups.</param>
        /// <param name="SA">Platform-dependent SteamApps directory name.</param>
        /// <param name="PN">Platform-dependent Steam process name.</param>
        public FrmStmClean(string SP, string BD, string SA, string PN)
        {
            InitializeComponent();
            SteamPath = SP;
            BackUpDir = BD;
            SteamAppsDirName = SA;
            SteamProcName = PN;
        }

        /// <summary>
        /// "Execute cleanup" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void EC_Execute_Click(object sender, EventArgs e)
        {
            // Creatin an empty list for storing candidates...
            List<String> CleanDirs = new List<string>();

            // HTML cache of internal browser (both client and overlay)...
            if (EC_HTMLCache.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "config", "htmlcache", "*.*"));
                CleanDirs.Add(Path.Combine(SteamPath, "config", "overlayhtmlcache", "*.*"));
                CleanDirs.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Steam", "htmlcache", "*.*"));
            }

            // HTTP cache...
            if (EC_HTTPCache.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "appcache", "httpcache", "*.*"));
            }

            // Depot cache...
            if (EC_DepotCache.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "depotcache", "*.*"));
            }

            // Downloaded shaders cache...
            if (EC_ShaderCache.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "shadercache", "*.*"));
            }

            // Steam library cache...
            if (EC_LibraryCache.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "appcache", "librarycache", "*.*"));
            }

            // Client logs...
            if (EC_Logs.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "logs", "*.*"));
                CleanDirs.Add(Path.Combine(SteamPath, "*.log*"));
            }

            // Old binaries...
            if (EC_OldBins.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "*.old"));
                CleanDirs.Add(Path.Combine(SteamPath, "bin", "*.old"));
            }

            // Crash dumps...
            if (EC_ErrDmps.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "dumps", "*.*"));
                CleanDirs.Add(Path.Combine(SteamPath, "*.dmp"));
                CleanDirs.Add(Path.Combine(SteamPath, "*.mdmp"));
            }

            // Games download and build caches...
            if (EC_BuildCache.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, SteamAppsDirName, "downloading"));
                CleanDirs.Add(Path.Combine(SteamPath, SteamAppsDirName, "temp"));
            }

            // Cached icons...
            if (EC_GameIcons.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "steam", "games", "*.*"));
            }

            // Local Cloud contents...
            if (EC_Cloud.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "userdata", "*.*"));
            }

            // Local offline game statistics...
            if (EC_Stats.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "appcache", "stats", "*.*"));
            }

            // Music database...
            if (EC_Music.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "music", "_database", "*.*"));
            }

            // Custom skins...
            if (EC_Skins.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "skins", "*.*"));
            }

            // Old updater packages...
            if (EC_Updater.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "package", "*.*"));
            }

            // Guard supercookies and auth files...
            if (EC_Guard.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "ssfn*"));
            }

            // Executing cleanup sequence...
            if (CleanDirs.Count > 0)
            {
                GuiHelpers.FormShowCleanup(CleanDirs, Text, AppStrings.PS_CleanupSuccess, BackUpDir, SteamProcName);
                Close();
            }
            else
            {
                MessageBox.Show(AppStrings.AC_NoItemsSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
