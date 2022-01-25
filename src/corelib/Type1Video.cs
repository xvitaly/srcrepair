﻿/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.IO;
using Microsoft.Win32;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with Type 1 game video settings.
    /// </summary>
    public class Type1Video : CommonVideo, IType1Video
    {
        /// <summary>
        /// Stores instance of Type1Settings class.
        /// </summary>
        protected Type1Settings VSettings;

        /// <summary>
        /// Stores model quality: r_rootlod.
        /// </summary>
        protected int _ModelDetail;

        /// <summary>
        /// Stores texture quality: mat_picmip.
        /// </summary>
        protected int _TextureDetail;

        /// <summary>
        /// Stores shader effects quality: mat_reducefillrate.
        /// </summary>
        protected int _ShaderDetail;

        /// <summary>
        /// Stores water quality: r_waterforceexpensive.
        /// </summary>
        protected int _WaterDetail;

        /// <summary>
        /// Stores water reflections quality: r_waterforcereflectentities.
        /// </summary>
        protected int _WaterReflections;

        /// <summary>
        /// Stores color correction setting: mat_colorcorrection.
        /// </summary>
        protected int _ColorCorrection;

        /// <summary>
        /// Stores trilinear filtering mode type: mat_trilinear.
        /// </summary>
        protected int _FilteringTrilinear;

        /// <summary>
        /// Stores DirectX mode (effects level): DXLevel_V1.
        /// </summary>
        protected int _DirectXMode;

        /// <summary>
        /// Stores HDR level: mat_hdr_level.
        /// </summary>
        protected int _HDRMode;

        /// <summary>
        /// Stores graphic settings registry key full path.
        /// </summary>
        protected string RegKey;

        /// <summary>
        /// Gets or sets display mode (fullscreen, windowed) video setting.
        /// </summary>
        public int DisplayMode
        {
            get
            {
                int res = -1;

                switch (_DisplayMode)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _DisplayMode = 0;
                        break;
                    case 1:
                        _DisplayMode = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets model quality video setting.
        /// </summary>
        public int ModelQuality
        {
            get
            {
                int res = -1;

                switch (_ModelDetail)
                {
                    case 0:
                        res = 2;
                        break;
                    case 1:
                        res = 1;
                        break;
                    case 2:
                        res = 0;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _ModelDetail = 2;
                        break;
                    case 1:
                        _ModelDetail = 1;
                        break;
                    case 2:
                        _ModelDetail = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets texture quality video setting.
        /// </summary>
        public int TextureQuality
        {
            get
            {
                int res = -1;

                switch (_TextureDetail)
                {
                    case -1:
                        res = 3;
                        break;
                    case 0:
                        res = 2;
                        break;
                    case 1:
                        res = 1;
                        break;
                    case 2:
                        res = 0;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _TextureDetail = 2;
                        break;
                    case 1:
                        _TextureDetail = 1;
                        break;
                    case 2:
                        _TextureDetail = 0;
                        break;
                    case 3:
                        _TextureDetail = -1;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets shader effects quality video setting.
        /// </summary>
        public int ShaderQuality
        {
            get
            {
                int res = -1;

                switch (_ShaderDetail)
                {
                    case 0:
                        res = 1;
                        break;
                    case 1:
                        res = 0;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _ShaderDetail = 1;
                        break;
                    case 1:
                        _ShaderDetail = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets water reflections quality video setting.
        /// </summary>
        public int ReflectionsQuality
        {
            get
            {
                int res = -1;

                switch (_WaterDetail)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        switch (_WaterReflections)
                        {
                            case 0:
                                res = 1;
                                break;
                            case 1:
                                res = 2;
                                break;
                        }
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _WaterDetail = 0;
                        _WaterReflections = 0;
                        break;
                    case 1:
                        _WaterDetail = 1;
                        _WaterReflections = 0;
                        break;
                    case 2:
                        _WaterDetail = 1;
                        _WaterReflections = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets shadow effects quality video setting.
        /// </summary>
        public override int ShadowQuality
        {
            get
            {
                int res = -1;

                switch (_ShadowQuality)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _ShadowQuality = 0;
                        break;
                    case 1:
                        _ShadowQuality = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets color correction video setting.
        /// </summary>
        public int ColorCorrection
        {
            get
            {
                int res = -1;

                switch (_ColorCorrection)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _ColorCorrection = 0;
                        break;
                    case 1:
                        _ColorCorrection = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets filtering mode video setting.
        /// </summary>
        public int FilteringMode
        {
            get
            {
                int res = -1;

                switch (_FilteringMode)
                {
                    case 1:
                        switch (_FilteringTrilinear)
                        {
                            case 0:
                                res = 0;
                                break;
                            case 1:
                                res = 1;
                                break;
                        }
                        break;
                    case 2:
                        res = 2;
                        break;
                    case 4:
                        res = 3;
                        break;
                    case 8:
                        res = 4;
                        break;
                    case 16:
                        res = 5;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0: // Bilinear
                        _FilteringMode = 1;
                        _FilteringTrilinear = 0;
                        break;
                    case 1: // Trilinear
                        _FilteringMode = 1;
                        _FilteringTrilinear = 1;
                        break;
                    case 2: // Anisotrophic 2x
                        _FilteringMode = 2;
                        _FilteringTrilinear = 0;
                        break;
                    case 3: // Anisotrophic 4x
                        _FilteringMode = 4;
                        _FilteringTrilinear = 0;
                        break;
                    case 4: // Anisotrophic 8x
                        _FilteringMode = 8;
                        _FilteringTrilinear = 0;
                        break;
                    case 5: // Anisotrophic 16x
                        _FilteringMode = 16;
                        _FilteringTrilinear = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets vertical synchronization video setting.
        /// </summary>
        public override int VSync
        {
            get
            {
                int res = -1;

                switch (_VSync)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _VSync = 0;
                        break;
                    case 1:
                        _VSync = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets DirectX mode (version) video setting.
        /// </summary>
        public int DirectXMode
        {
            get
            {
                int res = -1;

                switch (_DirectXMode)
                {
                    case 80:
                        res = 0;
                        break;
                    case 81:
                        res = 1;
                        break;
                    case 90:
                        res = 2;
                        break;
                    case 95:
                        res = 3;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _DirectXMode = 80;
                        break;
                    case 1:
                        _DirectXMode = 81;
                        break;
                    case 2:
                        _DirectXMode = 90;
                        break;
                    case 3:
                        _DirectXMode = 95;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets HDR level video setting.
        /// </summary>
        public int HDRType
        {
            get
            {
                int res = -1;

                switch (_HDRMode)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                    case 2:
                        res = 2;
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _HDRMode = 0;
                        break;
                    case 1:
                        _HDRMode = 1;
                        break;
                    case 2:
                        _HDRMode = 2;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets Cvar value as string from video file.
        /// </summary>
        /// <param name="CVar">Cvar name.</param>
        /// <returns>Cvar value as string from video file.</returns>
        protected override string GetRawValue(string CVar)
        {
            return String.Empty;
        }

        /// <summary>
        /// Reads Type 1 game video settings from Windows registry.
        /// </summary>
        public override void ReadSettings()
        {
            using (RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(RegKey, false))
            {
                if (ResKey != null)
                {
                    try { _ScreenWidth = Convert.ToInt32(ResKey.GetValue(VSettings.ScreenWidth)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _ScreenHeight = Convert.ToInt32(ResKey.GetValue(VSettings.ScreenHeight)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _DisplayMode = Convert.ToInt32(ResKey.GetValue(VSettings.DisplayMode)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _ModelDetail = Convert.ToInt32(ResKey.GetValue(VSettings.ModelDetail)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _TextureDetail = Convert.ToInt32(ResKey.GetValue(VSettings.TextureDetail)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _ShaderDetail = Convert.ToInt32(ResKey.GetValue(VSettings.ShaderDetail)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _WaterDetail = Convert.ToInt32(ResKey.GetValue(VSettings.WaterDetail)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _WaterReflections = Convert.ToInt32(ResKey.GetValue(VSettings.WaterReflections)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _ShadowQuality = Convert.ToInt32(ResKey.GetValue(VSettings.ShadowDetail)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _ColorCorrection = Convert.ToInt32(ResKey.GetValue(VSettings.ColorCorrection)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _AntiAliasing = Convert.ToInt32(ResKey.GetValue(VSettings.AntiAliasing)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _AntiAliasQuality = Convert.ToInt32(ResKey.GetValue(VSettings.AntiAliasQuality)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _FilteringMode = Convert.ToInt32(ResKey.GetValue(VSettings.FilteringMode)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _FilteringTrilinear = Convert.ToInt32(ResKey.GetValue(VSettings.FilteringTrilinear)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _VSync = Convert.ToInt32(ResKey.GetValue(VSettings.VSync)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _MotionBlur = Convert.ToInt32(ResKey.GetValue(VSettings.MotionBlur)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _DirectXMode = Convert.ToInt32(ResKey.GetValue(VSettings.DirectXMode)); } catch (Exception Ex) { Logger.Warn(Ex); }
                    try { _HDRMode = Convert.ToInt32(ResKey.GetValue(VSettings.HDRMode)); } catch (Exception Ex) { Logger.Warn(Ex); }
                }
                else
                {
                    throw new Exception(DebugStrings.AppDbgExCoreType1VideoRegOpenError);
                }
            }
        }

        /// <summary>
        /// Writes Type 1 game video settings to Windows registry.
        /// </summary>
        public override void WriteSettings()
        {
            using (RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(RegKey, true))
            {
                try { ResKey.SetValue(VSettings.ScreenWidth, _ScreenWidth, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.ScreenHeight, _ScreenHeight, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.DisplayMode, _DisplayMode, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.ModelDetail, _ModelDetail, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.TextureDetail, _TextureDetail, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.ShaderDetail, _ShaderDetail, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.WaterDetail, _WaterDetail, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.WaterReflections, _WaterReflections, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.ShadowDetail, _ShadowQuality, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.ColorCorrection, _ColorCorrection, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.AntiAliasing, _AntiAliasing, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.AntiAliasQuality, _AntiAliasQuality, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.AntiAliasingMSAA, _AntiAliasing, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.AntiAliasQualityMSAA, _AntiAliasQuality, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.FilteringMode, _FilteringMode, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.FilteringTrilinear, _FilteringTrilinear, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.VSync, _VSync, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.MotionBlur, _MotionBlur, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.DirectXMode, _DirectXMode, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
                try { ResKey.SetValue(VSettings.HDRMode, _HDRMode, RegistryValueKind.DWord); } catch (Exception Ex) { Logger.Warn(Ex); }
            }
        }

        /// <summary>
        /// Gets registry key, used for storing video settings.
        /// </summary>
        /// <param name="SAppName">The name of registry subkey, used for storing video settings.</param>
        /// <param name="ExpandPath">Allow to expand path.</param>
        /// <returns>Full registry key path.</returns>
        public static string GetGameRegKey(string SAppName, bool ExpandPath = true)
        {
            return ExpandPath ? Path.Combine("Software", "Valve", "Source", SAppName, "Settings") : String.Format(@"Software\Valve\Source\{0}\Settings", SAppName);
        }

        /// <summary>
        /// Checks if specified registry subkey exists in HKEY_CURRENT_USER branch.
        /// </summary>
        /// <param name="Subkey">Subkey.</param>
        /// <returns>Returns True if registry subkey exists.</returns>
        public static bool CheckRegKeyExists(string Subkey)
        {
            bool Result;
            using (RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(Subkey, false))
            {
                Result = ResKey != null;
            }
            return Result;
        }

        /// <summary>
        /// Creates specified subkey in registry (HKEY_CURRENT_USER branch).
        /// </summary>
        /// <param name="Subkey">Subkey.</param>
        public static void CreateRegKey(string Subkey)
        {
            Registry.CurrentUser.CreateSubKey(Subkey);
        }

        /// <summary>
        /// Removed specified subkey from registry (HKEY_CURRENT_USER branch).
        /// </summary>
        /// <param name="Subkey">Подключ реестра для удаления</param>
        public static void RemoveRegKey(string Subkey)
        {
            Registry.CurrentUser.DeleteSubKeyTree(Subkey, false);
        }

        /// <summary>
        /// Creates a backup copy of specified registry key.
        /// </summary>
        /// <param name="RegKey">Registry key.</param>
        /// <param name="FileName">Backup file name.</param>
        /// <param name="DestDir">Directory for saving backups.</param>
        public static void CreateRegBackUpNow(string RegKey, string FileName, string DestDir)
        {
            ProcessManager.StartProcessAndWait(Properties.Resources.RegExecutable, String.Format(Properties.Resources.RegExportCmdLine, RegKey, Path.Combine(DestDir, String.Format(Properties.Resources.RegOutFilePattern, FileName, FileManager.DateTime2Unix(DateTime.Now)))));
        }

        /// <summary>
        /// Restores a specified registry backup.
        /// </summary>
        /// <param name="FileName">Full path to the backup file.</param>
        public static void RestoreRegBackUpNow(string FileName)
        {
            ProcessManager.StartProcessAndWait(Properties.Resources.RegExecutable, String.Format(Properties.Resources.RegImportCmdLine, FileName));
        }

        /// <summary>
        /// Creates a backup copy of specified registry subkey
        /// (HKEY_CURRENT_USER branch).
        /// </summary>
        /// <param name="RegKey">Registry subkey.</param>
        /// <param name="FileName">Backup file name.</param>
        /// <param name="DestDir">Directory for saving backups.</param>
        public static void BackUpVideoSettings(string RegKey, string FileName, string DestDir)
        {
            CreateRegBackUpNow(Path.Combine("HKEY_CURRENT_USER", RegKey), FileName, DestDir);
        }

        /// <summary>
        /// Type1Video class constructor.
        /// </summary>
        /// <param name="SAppName">The name of registry subkey, used for storing video settings.</param>
        public Type1Video(string SAppName)
        {
            VSettings = new Type1Settings();
            RegKey = GetGameRegKey(SAppName);
        }
    }
}
