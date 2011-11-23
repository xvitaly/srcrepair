using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Diagnostics;

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
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                MessageBox.Show(Properties.Resources.AppAlrLaunched, GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Environment.Exit(16);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                string[] CMDLineA = Environment.GetCommandLineArgs();
                for (int StrNum = 0; StrNum < CMDLineA.Length; StrNum++)
                {
                    switch (CMDLineA[StrNum])
                    {
                        case "/russian":
                            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
                            break;
                        case "/english":
                            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                            break;
                    }
                }
                Application.Run(new frmMainW());
            }
        }
    }
    public delegate void CFGEdDelegate(string Cv, string Cn);
}
