using srcrepair.gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            using (Mutex Mtx = new Mutex(false, Properties.Resources.AppName))
            {
                if (Mtx.WaitOne(0, false))
                {
                    if (ProcessManager.IsCurrentUserAdmin())
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new FrmKBHelper());
                    }
                    else
                    {
                        MessageBox.Show(AppStrings.KB_NoAdminRights, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(AppStrings.KB_AlrLaunched, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Environment.Exit(ReturnCodes.AppAlreadyRunning);
                }
            }
        }
    }
}
