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
    public partial class frmMainW : Form
    {
        public frmMainW()
        {
            InitializeComponent();
        }

        private void frmMainW_Load(object sender, EventArgs e)
        {
            // Событие инициализации формы...
        }

        private void PS_CleanBlobs_CheckedChanged(object sender, EventArgs e)
        {
            if (PS_CleanBlobs.Checked || PS_CleanRegistry.Checked)
            {
                PS_ExecuteNow.Enabled = true;
            }
            else
            {
                PS_ExecuteNow.Enabled = false;
            }
        }

        private void PS_CleanRegistry_CheckedChanged(object sender, EventArgs e)
        {
            if (PS_CleanRegistry.Checked)
            {
                PS_SteamLang.Enabled = true;
            }
            else
            {
                PS_SteamLang.Enabled = false;
            }

            if (PS_CleanRegistry.Checked || PS_CleanBlobs.Checked)
            {
                PS_ExecuteNow.Enabled = true;
            }
            else
            {
                PS_ExecuteNow.Enabled = false;
            }
        }

        private void PS_ExecuteNow_Click(object sender, EventArgs e)
        {
            // Начинаем очистку...

            // Запрашиваем подтверждение у пользователя...
            DialogResult UserConfirmation = MessageBox.Show(Properties.Resources.PS_ExecuteMSG, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                // Подтверждение получено...
                if ((PS_CleanBlobs.Checked) || (PS_CleanRegistry.Checked))
                {
                    // Найдём и завершим работу клиента Steam...
                    if (CoreFn.ProcessTerminate("Steam") != 0)
                    {
                        MessageBox.Show(Properties.Resources.PS_ProcessDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Проверяем нужно ли чистить блобы...
                    if (PS_CleanBlobs.Checked)
                    {
                        // Чистим блобы...
                        CoreFn.CleanBlobsNow(CoreFn.FullSteamPath);
                    }

                    // Проверяем нужно ли чистить реестр...
                    if (PS_CleanRegistry.Checked)
                    {
                        // Проверяем выбрал ли пользователь язык из выпадающего списка...
                        if (PS_SteamLang.SelectedIndex != -1)
                        {
                            // Всё в порядке, чистим реестр...
                            CoreFn.CleanRegistryNow(PS_SteamLang.SelectedIndex);
                        }
                        else
                        {
                            // Пользователь не выбрал язык, поэтому будем использовать английский...
                            MessageBox.Show(Properties.Resources.PS_NoLangSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CoreFn.CleanRegistryNow(0);
                        }
                    }

                    // Выполнение закончено, выведем сообщение о завершении...
                    MessageBox.Show(Properties.Resources.PS_SeqCompleted, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Завершаем работу приложения...
                    Application.Exit();
                }
            }
        }

        private void PS_AllowRemCtrls_CheckedChanged(object sender, EventArgs e)
        {
            PS_RemCustMaps.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemDnlCache.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemOldSpray.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemOldCfgs.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemGraphCache.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemSoundCache.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemNavFiles.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemScreenShots.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemDemos.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemGraphOpts.Enabled = PS_AllowRemCtrls.Checked;
        }
    }
}
