/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of Steam garbage cleaner window.
    /// </summary>
    public partial class FrmStmClean : Form
    {
        /// <summary>
        /// CurrentPlatform instance for FrmStmClean class.
        /// </summary>
        private readonly CurrentPlatform Platform = CurrentPlatform.Create();

        /// <summary>
        /// Gets or sets full path to Steam client directory.
        /// </summary>
        private string SteamPath { get; set; }

        /// <summary>
        /// Gets or sets full path to directory for saving backups.
        /// </summary>
        private string BackUpDir { get; set; }

        /// <summary>
        /// Gets or sets the list of files and directories for cleanup.
        /// </summary>
        private List<string> CleanDirs { get; set; }

        /// <summary>
        /// FrmStmClean class constructor.
        /// </summary>
        /// <param name="SP">Full path to Steam client directory.</param>
        /// <param name="BD">Full path to directory for saving backups.</param>
        public FrmStmClean(string SP, string BD)
        {
            InitializeComponent();
            SteamPath = SP;
            BackUpDir = BD;
            CleanDirs = new List<string>();
        }

        /// <summary>
        /// Adds HTML cache of internal browser (both client and overlay) directories
        /// to the list.
        /// </summary>
        private void EnqueueHTMLCache()
        {
            if (EC_HTMLCache.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "config", "htmlcache", "*.*"));
                CleanDirs.Add(Path.Combine(SteamPath, "config", "overlayhtmlcache", "*.*"));
                CleanDirs.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Steam", "htmlcache", "*.*"));
            }
        }

        /// <summary>
        /// Adds HTTP cache directories to the list.
        /// </summary>
        private void EnqueueHTTPCache()
        {
            if (EC_HTTPCache.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "appcache", "httpcache", "*.*"));
            }
        }

        /// <summary>
        /// Adds Depot cache directories to the list.
        /// </summary>
        private void EnqueueDepotCache()
        {
            if (EC_DepotCache.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "depotcache", "*.*"));
            }
        }

        /// <summary>
        /// Adds Downloaded shaders cache directories to the list.
        /// </summary>
        private void EnqueueShaderCache()
        {
            if (EC_ShaderCache.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, Platform.SteamAppsFolderName, "shadercache", "*.*"));
            }
        }

        /// <summary>
        /// Adds Steam library cache directories to the list.
        /// </summary>
        private void EnqueueLibraryCache()
        {
            if (EC_LibraryCache.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "appcache", "librarycache", "*.*"));
            }
        }

        /// <summary>
        /// Adds Client logs directories to the list.
        /// </summary>
        private void EnqueueClientLogs()
        {
            if (EC_Logs.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "logs", "*.*"));
                CleanDirs.Add(Path.Combine(SteamPath, "*.log*"));
            }
        }

        /// <summary>
        /// Adds Old binaries directories to the list.
        /// </summary>
        private void EnqueueOldBinaries()
        {
            if (EC_OldBins.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "*.old"));
                CleanDirs.Add(Path.Combine(SteamPath, "bin", "*.old"));
            }
        }

        /// <summary>
        /// Adds Crash dumps directories to the list.
        /// </summary>
        private void EnqueueCrashDumps()
        {
            if (EC_ErrDmps.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "dumps", "*.*"));
                CleanDirs.Add(Path.Combine(SteamPath, "*.dmp"));
                CleanDirs.Add(Path.Combine(SteamPath, "*.mdmp"));
            }
        }

        /// <summary>
        /// Adds Games download and build caches directories to the list.
        /// </summary>
        private void EnqueueBuildCache()
        {
            if (EC_BuildCache.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, Platform.SteamAppsFolderName, "downloading"));
                CleanDirs.Add(Path.Combine(SteamPath, Platform.SteamAppsFolderName, "temp"));
            }
        }

        /// <summary>
        /// Adds Cached icons directories to the list.
        /// </summary>
        private void EnqueueIconsCache()
        {
            if (EC_GameIcons.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "steam", "games", "*.*"));
            }
        }

        /// <summary>
        /// Adds Local Cloud contents directories to the list.
        /// </summary>
        private void EnqueueCloudCache()
        {
            if (EC_Cloud.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "userdata", "*.*"));
            }
        }

        /// <summary>
        /// Adds Local offline game statistics directories to the list.
        /// </summary>
        private void EnqueueStatsCache()
        {
            if (EC_Stats.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "appcache", "stats", "*.*"));
            }
        }

        /// <summary>
        /// Adds Music database directories to the list.
        /// </summary>
        private void EnqueueMusicDbCache()
        {
            if (EC_Music.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "music", "_database", "*.*"));
            }
        }

        /// <summary>
        /// Adds Custom skins directories to the list.
        /// </summary>
        private void EnqueueSkinsCache()
        {
            if (EC_Skins.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "skins", "*.*"));
            }
        }

        /// <summary>
        /// Adds Old updater packages directories to the list.
        /// </summary>
        private void EnqueuePackagesCache()
        {
            if (EC_Updater.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "package", "*.*"));
            }
        }

        /// <summary>
        /// Adds Guard supercookies and auth files to the list.
        /// </summary>
        private void EnqueueGuardCache()
        {
            if (EC_Guard.Checked)
            {
                CleanDirs.Add(Path.Combine(SteamPath, "ssfn*"));
            }
        }

        /// <summary>
        /// Performs basic caches cleanup.
        /// </summary>
        private void CheckBasicCaches()
        {
            EnqueueHTMLCache();
            EnqueueHTTPCache();
            EnqueueDepotCache();
            EnqueueShaderCache();
            EnqueueLibraryCache();
        }

        /// <summary>
        /// Performs common garbage cleanup.
        /// </summary>
        private void CheckCommonGarbage()
        {
            EnqueueClientLogs();
            EnqueueOldBinaries();
            EnqueueCrashDumps();
            EnqueueBuildCache();
        }

        /// <summary>
        /// Performs extended caches cleanup.
        /// </summary>
        private void CheckExtendedCleanup()
        {
            EnqueueIconsCache();
            EnqueueCloudCache();
            EnqueueStatsCache();
            EnqueueMusicDbCache();
            EnqueueSkinsCache();
        }

        /// <summary>
        /// Performs special cleanup.
        /// </summary>
        private void CheckTroubleshooting()
        {
            EnqueuePackagesCache();
            EnqueueGuardCache();
        }

        /// <summary>
        /// Executes the cleanup sequence.
        /// </summary>
        private void PerformCleanup()
        {
            if (CleanDirs.Count > 0)
            {
                GuiHelpers.FormShowCleanup(CleanDirs, Text, AppStrings.PS_CleanupSuccess, BackUpDir, Platform.SteamProcName);
                Close();
            }
            else
            {
                MessageBox.Show(AppStrings.AC_NoItemsSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Execute cleanup" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void EC_Execute_Click(object sender, EventArgs e)
        {
            CheckBasicCaches();
            CheckCommonGarbage();
            CheckExtendedCleanup();
            CheckTroubleshooting();
            PerformCleanup();
        }

        /// <summary>
        /// "Cancel" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void EC_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
