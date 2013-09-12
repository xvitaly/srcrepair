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
    public partial class frmBugReporter : Form
    {
        public frmBugReporter()
        {
            InitializeComponent();
        }

        private void frmBugReporter_Load(object sender, EventArgs e)
        {
            // Выберем категорию "Ошибка" по умолчанию...
            try { BR_Category.SelectedIndex = 0; } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void BR_Send_Click(object sender, EventArgs e)
        {
            // Запускаем обработчик асинхронно...
            if (!BR_WrkMf.IsBusy) { BR_WrkMf.RunWorkerAsync(); }
        }

        private void BR_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BR_WrkMf_DoWork(object sender, DoWorkEventArgs e)
        {
            //
        }

        private void BR_WrkMf_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Выводим сообщение...
            if (e.Error == null) { MessageBox.Show(CoreLib.GetLocalizedString("BR_SendCompleted"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); } else { CoreLib.WriteStringToLog(e.Error.Message); }

            // Закрываем форму...
            this.Close();
        }

        private void frmBugReporter_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = ((e.CloseReason == CloseReason.UserClosing) && BR_WrkMf.IsBusy);
        }
    }
}
