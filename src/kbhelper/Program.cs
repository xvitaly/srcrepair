/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 *
 * Copyright (c) 2011 - 2021 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2021 EasyCoding Team.
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

namespace srcrepair.gui.kbhelper
{
    static class Program
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
