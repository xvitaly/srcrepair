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
    public partial class FrmRmWrk : Form
    {
        private bool IsRunning = true;
        private List<String> RemDirs;

        public FrmRmWrk(List<String> SL)
        {
            InitializeComponent();
            RemDirs = SL;
        }

        private void FrmRmWrk_Load(object sender, EventArgs e)
        {
            // Запускаем удаление асинхронно...
            if (!RW_Wrk.IsBusy) { RW_Wrk.RunWorkerAsync(); }
        }

        private void RW_Wrk_DoWork(object sender, DoWorkEventArgs e)
        {
            //
        }

        private void RW_Wrk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Отображаем прогресс удаления файлов...
            RW_PrgBr.Value = e.ProgressPercentage;
        }

        private void RW_Wrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Удаление завершено. Закроем форму...
            IsRunning = false;
            if (e.Error != null) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("RW_RmException"), Properties.Resources.AppName, e.Error.Message, e.Error.Source, MessageBoxIcon.Warning); }
            Close();
        }

        private void FrmRmWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Блокируем возможность закрытия формы при работающем процессе...
            e.Cancel = IsRunning;
        }
    }
}
