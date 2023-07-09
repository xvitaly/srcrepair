/**
 * SPDX-FileCopyrightText: 2011-2023 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Globalization;
using NLog;

namespace srcrepair.core
{
    /// <summary>
    /// Abstract class with common properties and methods for
    /// all types of games.
    /// </summary>
    public abstract class CommonVideo : ICommonVideo
    {
        /// <summary>
        /// Logger instance for video classes.
        /// </summary>
        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Stores instance of CultureInfo class.
        /// </summary>
        protected readonly CultureInfo CI = new CultureInfo("en-US");

        /// <summary>
        /// Stores anti-aliasing setting.
        /// </summary>
        protected int _AntiAliasing;

        /// <summary>
        /// Stores anti-aliasing multiplier.
        /// </summary>
        protected int _AntiAliasQuality;

        /// <summary>
        /// Stores video config schema version.
        /// </summary>
        protected int _ConfigVersion;

        /// <summary>
        /// Stores motion blur setting: MotionBlur.
        /// </summary>
        protected int _MotionBlur;

        /// <summary>
        /// Stores screen width.
        /// </summary>
        protected int _ScreenWidth;

        /// <summary>
        /// Stores screen height.
        /// </summary>
        protected int _ScreenHeight;

        /// <summary>
        /// Stores hardware vendor ID.
        /// </summary>
        protected int _VendorID;

        /// <summary>
        /// Stores video card ID.
        /// </summary>
        protected int _DeviceID;

        /// <summary>
        /// Stores window mode settings: fullscreen, windoweed.
        /// </summary>
        protected int _DisplayMode;

        /// <summary>
        /// Stores vertical synchronization setting.
        /// </summary>
        protected int _VSync;

        /// <summary>
        /// Stores shadow effects quality.
        /// </summary>
        protected int _ShadowQuality;

        /// <summary>
        /// Stores filtering mode type.
        /// </summary>
        protected int _FilteringMode;

        /// <summary>
        /// Stores borderless window video setting.
        /// </summary>
        protected int _DisplayBorderless;

        /// <summary>
        /// Stores brightness value.
        /// </summary>
        protected int _Brightness;

        /// <summary>
        /// Gets or sets anti-aliasing video setting.
        /// </summary>
        public int AntiAliasing
        {
            get
            {
                int res = -1;

                switch (_AntiAliasing)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 0;
                        break;
                    case 2:
                        res = 1;
                        break;
                    case 4:
                        switch (_AntiAliasQuality)
                        {
                            case 0:
                                res = 2;
                                break;
                            case 2:
                                res = 3;
                                break;
                            case 4:
                                res = 4;
                                break;
                            default:
                                Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _AntiAliasQuality, "_AntiAliasQuality");
                                break;
                        }
                        break;
                    case 8:
                        switch (_AntiAliasQuality)
                        {
                            case 0:
                                res = 5;
                                break;
                            case 2:
                                res = 6;
                                break;
                            default:
                                Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _AntiAliasQuality, "_AntiAliasQuality");
                                break;
                        }
                        break;
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _AntiAliasing, "_AntiAliasing");
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0: // No AA
                        _AntiAliasing = 1;
                        _AntiAliasQuality = 0;
                        break;
                    case 1: // 2x MSAA
                        _AntiAliasing = 2;
                        _AntiAliasQuality = 0;
                        break;
                    case 2: // 4x MSAA
                        _AntiAliasing = 4;
                        _AntiAliasQuality = 0;
                        break;
                    case 3: // 8x CSAA
                        _AntiAliasing = 4;
                        _AntiAliasQuality = 2;
                        break;
                    case 4: // 16x CSAA
                        _AntiAliasing = 4;
                        _AntiAliasQuality = 4;
                        break;
                    case 5: // 8x MSAA
                        _AntiAliasing = 8;
                        _AntiAliasQuality = 0;
                        break;
                    case 6: // 16xQ CSAA
                        _AntiAliasing = 8;
                        _AntiAliasQuality = 2;
                        break;
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreSetterOutOfRange, value, "AntiAliasing");
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets motion blur video setting.
        /// </summary>
        public int MotionBlur
        {
            get
            {
                int res = -1;

                switch (_MotionBlur)
                {
                    case 0:
                        res = 0;
                        break;
                    case 1:
                        res = 1;
                        break;
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreFieldOutOfRange, _MotionBlur, "_MotionBlur");
                        break;
                }

                return res;
            }

            set
            {
                switch (value)
                {
                    case 0:
                        _MotionBlur = 0;
                        break;
                    case 1:
                        _MotionBlur = 1;
                        break;
                    default:
                        Logger.Warn(DebugStrings.AppDbgCoreSetterOutOfRange, value, "MotionBlur");
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets shadow effects quality video setting.
        /// </summary>
        public abstract int ShadowQuality { get; set; }

        /// <summary>
        /// Gets or sets screen width video setting.
        /// </summary>
        public int ScreenWidth { get => _ScreenWidth; set { _ScreenWidth = value; } }

        /// <summary>
        /// Gets or sets screen height video setting.
        /// </summary>
        public int ScreenHeight { get => _ScreenHeight; set { _ScreenHeight = value; } }

        /// <summary>
        /// Gets or sets vertical synchronization video setting.
        /// </summary>
        public abstract int VSync { get; set; }

        /// <summary>
        /// Gets Cvar value as string from video file.
        /// </summary>
        /// <param name="CVar">Cvar name.</param>
        /// <returns>Cvar value as string from video file.</returns>
        protected abstract string GetRawValue(string CVar);

        /// <summary>
        /// Gets Cvar value of integer type from video file. Supports
        /// user-specified default value.
        /// </summary>
        /// <param name="CVar">Cvar name.</param>
        /// <param name="DefaultValue">Cvar default value.</param>
        /// <returns>Cvar value from video file.</returns>
        protected int GetNCFDWord(string CVar, int DefaultValue)
        {
            try
            {
                return Convert.ToInt32(GetRawValue(CVar));
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, DebugStrings.AppDbgExCoreVideoLoadCvar, CVar);
                return DefaultValue;
            }
        }

        /// <summary>
        /// Gets Cvar value of integer type from video file.
        /// </summary>
        /// <param name="CVar">Cvar name.</param>
        /// <returns>Cvar value from video file.</returns>
        protected int GetNCFDWord(string CVar)
        {
            return GetNCFDWord(CVar, -1);
        }

        /// <summary>
        /// Gets Cvar value of decimal type from video file. Supports
        /// user-specified default value.
        /// </summary>
        /// <param name="CVar">Cvar name.</param>
        /// <param name="DefaultValue">Cvar default value.</param>
        /// <returns>Cvar value from video file.</returns>
        protected decimal GetNCFDble(string CVar, decimal DefaultValue)
        {
            try
            {
                return Convert.ToDecimal(GetRawValue(CVar), CI);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, DebugStrings.AppDbgExCoreVideoLoadCvar, CVar);
                return DefaultValue;
            }
        }

        /// <summary>
        /// Gets Cvar value of decimal type from video file.
        /// </summary>
        /// <param name="CVar">Cvar name.</param>
        /// <param name="DefaultValue">Cvar default value.</param>
        /// <returns>Cvar value from video file.</returns>
        protected decimal GetNCFDble(string CVar)
        {
            return GetNCFDble(CVar, 2.2M);
        }

        /// <summary>
        /// Reads game video settings.
        /// </summary>
        public abstract void ReadSettings();

        /// <summary>
        /// Writes game video settings.
        /// </summary>
        public abstract void WriteSettings();

        /// <summary>
        /// Gets Cvar value from string.
        /// </summary>
        /// <param name="LineA">Source string.</param>
        /// <returns>Extracted from source string value.</returns>
        protected static string ExtractCVFromLine(string LineA)
        {
            LineA = StringsManager.CleanString(LineA, true, false, false);
            return LineA.Substring(LineA.LastIndexOf(" ", StringComparison.InvariantCulture)).Trim();
        }
    }
}
