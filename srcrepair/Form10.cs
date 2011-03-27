using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace srcrepair
{
    public partial class frmKBHelper : Form
    {
        public frmKBHelper()
        {
            InitializeComponent();
        }

        private void WriteKBS(byte[] Value)
        {
            RegistryKey ResKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Keyboard Layout", true);
            ResKey.SetValue("Scancode Map", Value, RegistryValueKind.Binary);
            ResKey.Close();
        }

        private void DeleteKBS(string Value)
        {
            RegistryKey ResKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Keyboard Layout", true);
            ResKey.DeleteValue(Value);
            ResKey.Close();
        }

        private void Dis_LWIN_Click(object sender, EventArgs e)
        {
            // Отключаем левую клавишу WIN...
            // 00 00 00 00 00 00 00 00 02 00 00 00 00 00 5B E0 00 00 00 00
            if (MessageBox.Show(String.Format(CoreLib.GetLocalizedString("KB_ExQuestion"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    WriteKBS(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 91, 224, 0, 0, 0, 0 });
                    MessageBox.Show(CoreLib.GetLocalizedString("KB_ExSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("KB_ExException"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Dis_BWIN_Click(object sender, EventArgs e)
        {
            // Отключаем обе клавиши WIN...
            // 00 00 00 00 00 00 00 00 03 00 00 00 00 00 5B E0 00 00 5C E0 00 00 00 00
            if (MessageBox.Show(String.Format(CoreLib.GetLocalizedString("KB_ExQuestion"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    WriteKBS(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 91, 224, 0, 0, 92, 224, 0, 0, 0, 0 });
                    MessageBox.Show(CoreLib.GetLocalizedString("KB_ExSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("KB_ExException"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Dis_RWinMnu_Click(object sender, EventArgs e)
        {
            // Отключаем правую клавишу WIN и MENU...
            // 00 00 00 00 00 00 00 00 03 00 00 00 00 00 5C E0 00 00 5D E0 00 00 00 00
            if (MessageBox.Show(String.Format(CoreLib.GetLocalizedString("KB_ExQuestion"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    WriteKBS(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 91, 224, 0, 0, 93, 224, 0, 0, 0, 0 });
                    MessageBox.Show(CoreLib.GetLocalizedString("KB_ExSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("KB_ExException"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Dis_BWinMnu_Click(object sender, EventArgs e)
        {
            // Отключаем обе WIN и MENU...
            // 00 00 00 00 00 00 00 00 04 00 00 00 00 00 5B E0 00 00 5C E0 00 00 5D E0 00 00 00 00
            if (MessageBox.Show(String.Format(CoreLib.GetLocalizedString("KB_ExQuestion"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    WriteKBS(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 92, 224, 0, 0, 93, 224, 0, 0, 0, 0 });
                    MessageBox.Show(CoreLib.GetLocalizedString("KB_ExSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("KB_ExException"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Dis_Restore_Click(object sender, EventArgs e)
        {
            // Восстанавливаем настройки по умолчанию...
            if (MessageBox.Show(CoreLib.GetLocalizedString("KB_ExRestore"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DeleteKBS("Scancode Map");
                    MessageBox.Show(CoreLib.GetLocalizedString("KB_ExSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("KB_ExException"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
