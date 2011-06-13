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
    public partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            // Считаем текущие настройки...
            MO_ConfirmExit.Checked = Properties.Settings.Default.ConfirmExit;
            MO_HideNotInst.Checked = Properties.Settings.Default.HideNotInstalled;
            MO_SortGameList.Checked = Properties.Settings.Default.SortGamesList;
            MO_AutoUpdate.Checked = Properties.Settings.Default.EnableAutoUpdate;
            MO_AllowUnsafeNCFOps.Checked = Properties.Settings.Default.AllowNCFUnsafeOps;

            // Укажем название приложения в заголовке окна...
            this.Text = String.Format(this.Text, GV.AppName);
        }

        private void MO_Okay_Click(object sender, EventArgs e)
        {
            // Сохраняем настройки для текущего сеанса...
            Properties.Settings.Default.ConfirmExit = MO_ConfirmExit.Checked;
            Properties.Settings.Default.HideNotInstalled = MO_HideNotInst.Checked;
            Properties.Settings.Default.SortGamesList = MO_SortGameList.Checked;
            Properties.Settings.Default.EnableAutoUpdate = MO_AutoUpdate.Checked;
            Properties.Settings.Default.AllowNCFUnsafeOps = MO_AllowUnsafeNCFOps.Checked;
            // Сохраняем настройки...
            Properties.Settings.Default.Save();
            // Показываем сообщение...
            MessageBox.Show(CoreLib.GetLocalizedString("Opts_Saved"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Закрываем форму...
            this.Close();
        }

        private void MO_Cancel_Click(object sender, EventArgs e)
        {
            // Закрываем форму без сохранения изменений...
            this.Close();
        }
    }
}
