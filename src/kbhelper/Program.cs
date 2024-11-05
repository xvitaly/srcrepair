/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Threading;
using System.Windows.Forms;
using srcrepair.core;

namespace srcrepair.gui.kbhelper
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CurrentPlatform Platform = CurrentPlatform.Create();
            if (Platform.OS == CurrentPlatform.OSType.Windows)
            {
                if (ProcessManager.IsCurrentUserAdmin())
                {
                    using (Mutex Mtx = new Mutex(false, Properties.Resources.AppName))
                    {
                        if (Mtx.WaitOne(0, false))
                        {
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            Application.Run(new FrmKBHelper());
                        }
                        else
                        {
                            MessageBox.Show(AppStrings.KB_AlrLaunched, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Environment.Exit(ReturnCodes.AppAlreadyRunning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.KB_NoAdminRights, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.KB_OSNotSupported, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
