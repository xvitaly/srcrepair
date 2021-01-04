/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 *
 * Copyright (c) 2011 - 2020 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2020 EasyCoding Team.
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
using System.Threading;
using System.Windows.Forms;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Main class of application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Imports settings from previous versions of the application.
        /// </summary>
        private static void ImportSettings()
        {
            try
            {
                if (Properties.Settings.Default.CallUpgrade)
                {
                    Properties.Settings.Default.Upgrade();
                    Properties.Settings.Default.CallUpgrade = false;
                }
            }
            catch
            {
                Properties.Settings.Default.CallUpgrade = false;
                MessageBox.Show(AppStrings.AppImportSettingsError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Checks if the required library version is equal with the current library version.
        /// </summary>
        private static void CheckLibrary()
        {
            if (!LibraryManager.CheckLibraryVersion())
            {
                MessageBox.Show(AppStrings.AppLibVersionMissmatch, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(ReturnCodes.CoreLibVersionMissmatch);
            }
        }

        /// <summary>
        /// Initializes and shows the main form of the application.
        /// </summary>
        private static void ShowMainForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMainW());
        }

        /// <summary>
        /// Shows the message about already running application and exits.
        /// </summary>
        private static void HandleRunning()
        {
            MessageBox.Show(AppStrings.AppAlreadyRunning, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.Exit(ReturnCodes.AppAlreadyRunning);
        }

        /// <summary>
        /// The main entry point of the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using (Mutex Mtx = new Mutex(false, Properties.Resources.AppName))
            {
                if (Mtx.WaitOne(0, false))
                {
                    ImportSettings();
                    CheckLibrary();
                    ShowMainForm();
                }
                else
                {
                    HandleRunning();
                }
            }
        }
    }
}
