/**
 * SPDX-FileCopyrightText: 2011-2023 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with Type 4 game video settings names.
    /// </summary>
    public class Type4Settings : Type1Settings
    {
        /// <summary>
        /// Gets or sets shadow delpth video setting name.
        /// </summary>
        public string ShadowDepth { get; protected set; }

        /// <summary>
        /// Sets properties data for Type 4 game.
        /// </summary>
        private void SetSettings()
        {
            DisplayBorderless = "ScreenWindoNoBorder";
            Brightness = "ScreenMonitorGamma";
            ShadowDepth = "ShadowDepthTexture";
        }

        /// <summary>
        /// Type4Settings class constructor.
        /// </summary>
        public Type4Settings()
        {
            SetSettings();
        }
    }
}
