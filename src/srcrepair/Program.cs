/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
        /// Checks if the library version matches the application version.
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
