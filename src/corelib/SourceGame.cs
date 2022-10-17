/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with a single game.
    /// </summary>
    public sealed class SourceGame
    {
        /// <summary>
        /// Gets full path to custom user stuff directory.
        /// </summary>
        public string CustomInstallDir { get; private set; }

        /// <summary>
        /// Gets full path to installation directory.
        /// </summary>
        public string FullGamePath { get; private set; }

        /// <summary>
        /// Gets full path to installation directory without
        /// adding SmallAppName.
        /// </summary>
        public string GamePath { get; private set; }

        /// <summary>
        /// Gets full path to engine's core binaries.
        /// </summary>
        public string CoreEngineBinPath { get; private set; }

        /// <summary>
        /// Gets full user-friendly name.
        /// </summary>
        public string FullAppName { get; private set; }

        /// <summary>
        /// Gets internal name (the name of subdirectory in GamePath).
        /// </summary>
        public string SmallAppName { get; private set; }

        /// <summary>
        /// Gets launcher's binary name.
        /// </summary>
        public string GameBinaryFile { get; private set; }

        /// <summary>
        /// Gets full paths to local copies of configs, stored in Cloud.
        /// </summary>
        public List<string> CloudConfigs { get; private set; }

        /// <summary>
        /// Gets full path to local copies of cloud screenshots.
        /// </summary>
        public string CloudScreenshotsPath { get; private set; }

        /// <summary>
        /// Gets full path to directory with game configs.
        /// </summary>
        public string FullCfgPath { get; private set; }

        /// <summary>
        /// Gets full path to directory for saving backups.
        /// </summary>
        public string FullBackUpDirPath { get; private set; }

        /// <summary>
        /// Gets information if current game is using configs for
        /// storing video settings, or not (use registry instead).
        /// Always True on non-Windows platforms.
        /// </summary>
        public bool IsUsingVideoFile { get; private set; }

        /// <summary>
        /// Gets information if current game is using a special directory
        /// for custom user stuff.
        /// </summary>
        public bool IsUsingUserDir { get; private set; }

        /// <summary>
        /// Gets information if current game has available HUDs.
        /// </summary>
        public bool IsHUDsAvailable { get; private set; }

        /// <summary>
        /// Gets internal unique ID.
        /// </summary>
        public string GameInternalID { get; private set; }

        /// <summary>
        /// Gets Source engine type.
        /// </summary>
        public int SourceType { get; private set; }

        /// <summary>
        /// Gets full paths to all found configs with video settings.
        /// </summary>
        public List<string> VideoCfgFiles { get; private set; }

        /// <summary>
        /// Gets the name of directory or registry key with video settings.
        /// </summary>
        public string ConfDir { get; private set; }

        /// <summary>
        /// Gets full path to the local HUDs download directory.
        /// </summary>
        public string AppHUDDir { get; private set; }

        /// <summary>
        /// Gets full path to the local FPS-configs download directory.
        /// </summary>
        public string AppCfgDir { get; private set; }

        /// <summary>
        /// Gets full paths to all found FPS-configs.
        /// </summary>
        public List<string> FPSConfigs { get; set; }

        /// <summary>
        /// Gets or sets instance of HUDManager class.
        /// </summary>
        public HUDManager HUDMan { get; set; }

        /// <summary>
        /// Gets or sets instance of ConfigManager class.
        /// </summary>
        public ConfigManager CFGMan { get; set; }

        /// <summary>
        /// Gets or sets instance of CleanupManager class.
        /// </summary>
        public CleanupManager ClnMan { get; set; }

        /// <summary>
        /// Gets full path of Workshop directory.
        /// </summary>
        public string AppWorkshopDir { get; private set; }

        /// <summary>
        /// Gets full paths to all found voice ban list files.
        /// </summary>
        public List<string> BanlistFiles { get; private set; }

        /// <summary>
        /// Gets if the game is installed.
        /// </summary>
        public bool IsInstalled { get; private set; }

        /// <summary>
        /// Gets full path to installed Steam client.
        /// </summary>
        private string SteamPath { get; set; }

        /// <summary>
        /// Gets selected in main window user ID.
        /// </summary>
        private string SteamID { get; set; }

        /// <summary>
        /// Gets or sets video settings of selected game.
        /// </summary>
        public CommonVideo Video { get; private set; }

        /// <summary>
        /// Generates full path to installed game.
        /// </summary>
        /// <param name="AppName">Game directory name.</param>
        /// <param name="GameDirs">Paths to all available game libraries.</param>
        /// <param name="OSType">Operating system type.</param>
        /// <returns>Returns full path or empty string if nothing was found.</returns>
        private string GetGameDirectory(string AppName, List<string> GameDirs, CurrentPlatform.OSType OSType)
        {
            foreach (string Dir in GameDirs)
            {
                string GameDirectory = Path.Combine(Dir, AppName);
                if (Directory.Exists(Path.Combine(GameDirectory, SmallAppName)) && (File.Exists(Path.Combine(GameDirectory, GameBinaryFile)) || OSType != CurrentPlatform.OSType.Windows))
                {
                    return GameDirectory;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns full paths to all found local copies of configs,
        /// stored in Cloud.
        /// </summary>
        /// <param name="Mask">File mask (pattern).</param>
        /// <returns>List of all found local copies of cloud configs.</returns>
        private List<string> GetCloudConfigs(string Mask = "*.*cfg")
        {
            return FileManager.FindFiles(Path.Combine(SteamPath, "userdata", SteamID, GameInternalID), Mask);
        }

        /// <summary>
        /// Updates list of files with video settings.
        /// </summary>
        private void UpdateVideoFilesList()
        {
            VideoCfgFiles = GetCloudConfigs("video.txt");
            VideoCfgFiles.Add(Path.Combine(GamePath, ConfDir, "cfg", "video.txt"));
            VideoCfgFiles.Add(Path.Combine(GamePath, ConfDir, "videoconfig.cfg"));
        }

        /// <summary>
        /// Updates list of voice ban files.
        /// </summary>
        private void UpdateBanlistFilesList()
        {
            BanlistFiles = GetCloudConfigs("voice_ban.dt");
            BanlistFiles.Add(Path.Combine(FullGamePath, "voice_ban.dt"));
        }

        /// <summary>
        /// Returns the newerest file with video settings.
        /// </summary>
        /// <returns>Full path to the newerest file with video settings.</returns>
        public string GetActualVideoFile()
        {
            return FileManager.FindNewerestFile(VideoCfgFiles);
        }

        /// <summary>
        /// Returns the newerest file with voice bans.
        /// </summary>
        /// <returns>Full path to the newerest file with voice bans.</returns>
        public string GetActualBanlistFile()
        {
            return FileManager.FindNewerestFile(BanlistFiles);
        }

        /// <summary>
        /// Factory method. Creates video settings class instance.
        /// </summary>
        private CommonVideo GetVideoSettings()
        {
            switch (SourceType)
            {
                case 1:
                    return new Type1Video(ConfDir);
                case 2:
                    return new Type2Video(GetActualVideoFile());
                case 4:
                    return new Type4Video(GetActualVideoFile());
                default:
                    throw new NotSupportedException(DebugStrings.AppDbgExCoreUnknownEngineVersion);
            }
        }

        /// <summary>
        /// SourceGame class constructor.
        /// </summary>
        /// <param name="AppName">Name (from database).</param>
        /// <param name="DirName">Directory name (from database).</param>
        /// <param name="SmallName">Internal directory name (from database).</param>
        /// <param name="Executable">Binary name (from database).</param>
        /// <param name="SID">Internal ID (from database).</param>
        /// <param name="SV">Source type (from database).</param>
        /// <param name="VFDir">Name of directory with video settings (from database).</param>
        /// <param name="UserDir">Is using custom directory (from database).</param>
        /// <param name="AUserDir">Full path to data directory.</param>
        /// <param name="SteamDir">Full path to Steam directory.</param>
        /// <param name="SteamAppsDirName">Platform-dependent SteamApps directory name.</param>
        /// <param name="OS">Operating system type.</param>
        public SourceGame(string AppName, string DirName, string SmallName, string Executable, string SID, int SV, string VFDir, bool UserDir, bool HUDAv, string AUserDir, string SteamDir, string SteamAppsDirName, string SelectedSteamID, List<string> GameDirs, CurrentPlatform.OSType OS)
        {
            // Setting basic properties...
            FullAppName = AppName;
            SmallAppName = SmallName;
            GameBinaryFile = Executable;
            GameInternalID = SID;
            SourceType = (OS != CurrentPlatform.OSType.Windows) && (SV == 1) ? 2 : SV;
            ConfDir = VFDir;
            IsUsingVideoFile = SourceType != 1;
            IsUsingUserDir = UserDir;
            IsHUDsAvailable = HUDAv;
            SteamPath = SteamDir;

            // Getting game installation directory...
            GamePath = GetGameDirectory(DirName, GameDirs, OS);

            // Checking if game installed...
            IsInstalled = !string.IsNullOrWhiteSpace(GamePath);

            // Setting all other properties only for installed games...
            if (IsInstalled)
            {
                SteamID = SelectedSteamID;
                FullGamePath = Path.Combine(GamePath, SmallAppName);
                FullCfgPath = Path.Combine(FullGamePath, "cfg");
                CoreEngineBinPath = Path.Combine(GamePath, "bin");
                FullBackUpDirPath = Path.Combine(AUserDir, "backups", Path.GetFileName(SmallAppName));
                AppHUDDir = Path.Combine(AUserDir, Properties.Resources.HUDLocalDir, SmallAppName);
                AppCfgDir = Path.Combine(AUserDir, Properties.Resources.CfgLocalDir);
                CustomInstallDir = Path.Combine(FullGamePath, IsUsingUserDir ? "custom" : string.Empty);
                AppWorkshopDir = Path.Combine(SteamDir, SteamAppsDirName, Properties.Resources.WorkshopFolderName, "content", GameInternalID);
                CloudScreenshotsPath = Path.Combine(SteamDir, "userdata", SteamID, "760", "remote", GameInternalID, "screenshots");
                if (IsUsingVideoFile) { UpdateVideoFilesList(); }
                UpdateBanlistFilesList();
                CloudConfigs = GetCloudConfigs();
                Video = GetVideoSettings();
            }
        }
    }
}
