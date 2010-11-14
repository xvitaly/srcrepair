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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string[] CMDLineA = Environment.GetCommandLineArgs();
            for (int StrNum = 0; StrNum < CMDLineA.Length; StrNum++)
            {
                if (CMDLineA[StrNum] == "/russian")
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
                }
            }
            Application.Run(new frmMainW());
        }
    }
}
