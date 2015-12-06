using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace srcrepair
{
    public partial class FrmStmClean : Form
    {
        public FrmStmClean()
        {
            InitializeComponent();
        }

        private void EC_Execute_Click(object sender, EventArgs e)
        {
            //
            // Очистим HTML-кэш внутреннего браузера Steam...
            List<String> CleanDirs = new List<string>();
            //CleanDirs.Add(Path.Combine(App.FullSteamPath, "config", "htmlcache", "*.*"));
            //CleanDirs.Add(Path.Combine(App.FullSteamPath, "config", "overlayhtmlcache", "*.*"));
            //CleanDirs.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Steam", "htmlcache", "*.*"));
            //CoreLib.OpenCleanupWindow(CleanDirs, ((ToolStripMenuItem)sender).Text.ToLower().Replace("&", ""), CoreLib.GetLocalizedString("PS_CleanupSuccess"), Properties.Resources.SteamProcName);
            if (EC_HTMLCache.Checked)
            {
            }
            if (EC_HTTPCache.Checked)
            {
            }
            if (EC_Logs.Checked)
            {
            }
            if (EC_OldBins.Checked)
            {
            }
            if (EC_ErrDmps.Checked)
            {
            }
            if (EC_BuildCache.Checked)
            {
            }
            if (EC_GameIcons.Checked)
            {
            }
            if (EC_Cloud.Checked)
            {
            }
            if (EC_Stats.Checked)
            {
            }
            if (EC_Music.Checked)
            {
            }
            if (EC_Skins.Checked)
            {
            }
        }
    }
}
