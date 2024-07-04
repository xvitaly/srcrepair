/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

namespace srcrepair.core
{
    /// <summary>
    /// Interface with common properties and methods for
    /// all types of games.
    /// </summary>
    public interface ICommonVideo
    {
        /// <summary>
        /// Gets and sets anti-aliasing video setting.
        /// </summary>
        int AntiAliasing { get; set; }

        /// <summary>
        /// Gets and sets motion blur video setting.
        /// </summary>
        int MotionBlur { get; set; }

        /// <summary>
        /// Gets and sets shadow quality video setting.
        /// </summary>
        int ShadowQuality { get; set; }

        /// <summary>
        /// Gets and sets screen width video setting.
        /// </summary>
        int ScreenWidth { get; set; }

        /// <summary>
        /// Gets and sets screen height video setting.
        /// </summary>
        int ScreenHeight { get; set; }

        /// <summary>
        /// Gets and sets vertical synchronization video setting.
        /// </summary>
        int VSync { get; set; }

        /// <summary>
        /// Creates a backup copy of the game video settings.
        /// </summary>
        /// <param name="DestDir">Directory for saving backups.</param>
        /// <param name="IsManual">Determines whether the backup was initiated by the user or not.</param>
        void BackUpSettings(string DestDir, bool IsManual);

        /// <summary>
        /// Removes game video settings.
        /// </summary>
        void RemoveSettings();

        /// <summary>
        /// Reads game video settings.
        /// </summary>
        void ReadSettings();

        /// <summary>
        /// Saves video settings to file or registry.
        /// </summary>
        void WriteSettings();
    }
}
