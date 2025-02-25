/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
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
    /// Class of the Steam cleanup module.
    /// </summary>
    public partial class FrmStmClean : Form
    {
        /// <summary>
        /// CurrentPlatform instance for the FrmStmClean class.
        /// </summary>
        private readonly CurrentPlatform Platform = CurrentPlatform.Create();

        /// <summary>
        /// Stores the full path to the Steam client installation directory.
        /// </summary>
        private readonly string FullSteamPath;

        /// <summary>
        /// Stores the full path to the directory for saving backups.
        /// </summary>
        private readonly string FullBackUpDirPath;

        /// <summary>
        /// Stores the list of files and directories for cleanup.
        /// </summary>
        private readonly List<CleanupItem> CleanDirs;

        /// <summary>
        /// FrmStmClean class constructor.
        /// </summary>
        /// <param name="SteamDir">Full path to the Steam client installation directory.</param>
        /// <param name="BackUpDir">Full path to the directory for saving backups.</param>
        public FrmStmClean(string SteamDir, string BackUpDir)
        {
            InitializeComponent();
            FullSteamPath = SteamDir;
            FullBackUpDirPath = BackUpDir;
            CleanDirs = new List<CleanupItem>();
        }

        /// <summary>
        /// Adds HTML cache of internal browser (both client and overlay) directories
        /// to the list.
        /// </summary>
        private void EnqueueHTMLCache()
        {
            if (EC_HTMLCache.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Steam", "htmlcache"), "*.*", true, true));
            }
        }

        /// <summary>
        /// Adds HTTP cache directories to the list.
        /// </summary>
        private void EnqueueHTTPCache()
        {
            if (EC_HTTPCache.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, "appcache", "httpcache"), "*.*", true, true));
            }
        }

        /// <summary>
        /// Adds Depot cache directories to the list.
        /// </summary>
        private void EnqueueDepotCache()
        {
            if (EC_DepotCache.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, "depotcache"), "*.*", true, true));
            }
        }

        /// <summary>
        /// Adds Downloaded shaders cache directories to the list.
        /// </summary>
        private void EnqueueShaderCache()
        {
            if (EC_ShaderCache.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, Platform.SteamAppsFolderName, "shadercache"), "*.*", true, true));
            }
        }

        /// <summary>
        /// Adds Steam library cache directories to the list.
        /// </summary>
        private void EnqueueLibraryCache()
        {
            if (EC_LibraryCache.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, "appcache", "librarycache"), "*.*", true, true));
            }
        }

        /// <summary>
        /// Adds Client logs directories to the list.
        /// </summary>
        private void EnqueueClientLogs()
        {
            if (EC_Logs.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, "logs"), "*.*", true, true));
                CleanDirs.Add(new CleanupItem(FullSteamPath, "*.log*", false, false));
            }
        }

        /// <summary>
        /// Adds Old binaries directories to the list.
        /// </summary>
        private void EnqueueOldBinaries()
        {
            if (EC_OldBins.Checked)
            {
                CleanDirs.Add(new CleanupItem(FullSteamPath, "*.old", false, false));
            }
        }

        /// <summary>
        /// Adds Crash dumps directories to the list.
        /// </summary>
        private void EnqueueCrashDumps()
        {
            if (EC_ErrDmps.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, "dumps"), "*.*", true, false));
            }
        }

        /// <summary>
        /// Adds Games download and build caches directories to the list.
        /// </summary>
        private void EnqueueBuildCache()
        {
            if (EC_BuildCache.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, Platform.SteamAppsFolderName, "downloading"), "*.*", true, true));
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, Platform.SteamAppsFolderName, "temp"), "*.*", true, true));
            }
        }

        /// <summary>
        /// Adds Cached icons directories to the list.
        /// </summary>
        private void EnqueueIconsCache()
        {
            if (EC_GameIcons.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, "steam", "games"), "*.*", true, true));
            }
        }

        /// <summary>
        /// Adds Local Cloud contents directories to the list.
        /// </summary>
        private void EnqueueCloudCache()
        {
            if (EC_Cloud.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, "userdata"), "*.*", true, true));
            }
        }

        /// <summary>
        /// Adds Local offline game statistics directories to the list.
        /// </summary>
        private void EnqueueStatsCache()
        {
            if (EC_Stats.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, "appcache", "stats"), "*.*", true, true));
            }
        }

        /// <summary>
        /// Adds Music database directories to the list.
        /// </summary>
        private void EnqueueMusicDbCache()
        {
            if (EC_Music.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, "music", "_database"), "*.*", true, true));
            }
        }

        /// <summary>
        /// Adds Custom skins directories to the list.
        /// </summary>
        private void EnqueueSkinsCache()
        {
            if (EC_Skins.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, "skins"), "*.*", true, true));
            }
        }

        /// <summary>
        /// Adds Old updater packages directories to the list.
        /// </summary>
        private void EnqueuePackagesCache()
        {
            if (EC_Updater.Checked)
            {
                CleanDirs.Add(new CleanupItem(Path.Combine(FullSteamPath, "package"), "*.*", true, true));
            }
        }

        /// <summary>
        /// Adds Guard supercookies and auth files to the list.
        /// </summary>
        private void EnqueueGuardCache()
        {
            if (EC_Guard.Checked)
            {
                CleanDirs.Add(new CleanupItem(FullSteamPath, "ssfn*", false, false));
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
                GuiHelpers.FormShowCleanup(CleanDirs, Text, AppStrings.PS_CleanupSuccess, FullBackUpDirPath, Platform.SteamProcName);
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
