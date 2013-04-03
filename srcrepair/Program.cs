using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace srcrepair
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Mutex Mtx = new Mutex(false, GV.AppName))
            {
                if (Mtx.WaitOne(0, false))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    string[] CMDLineA = Environment.GetCommandLineArgs();
                    if (CMDLineA.Length > 2) { if (CMDLineA[1] == "/lang") { try { Thread.CurrentThread.CurrentUICulture = new CultureInfo(CMDLineA[2]); } catch { MessageBox.Show(Properties.Resources.AppUnsupportedLanguage, GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); } } }
                    Application.Run(new frmMainW());
                }
                else
                {
                    MessageBox.Show(Properties.Resources.AppAlrLaunched, GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Environment.Exit(16);
                }
            }
        }
    }
}
