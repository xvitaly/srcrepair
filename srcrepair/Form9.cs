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
            MO_ShowSingle.Checked = GO.ShowSinglePlayer;
            MO_ConfirmExit.Checked = GO.ConfirmExit;
            MO_HideNotInst.Checked = GO.HideNotInstalled;
            // Укажем название приложения в заголовке окна...
            this.Text = String.Format(this.Text, GV.AppName);
        }

        private void MO_Okay_Click(object sender, EventArgs e)
        {
            // Сохраняем настройки для текущего сеанса...
            GO.ShowSinglePlayer = MO_ShowSingle.Checked;
            GO.ConfirmExit = MO_ConfirmExit.Checked;
            GO.HideNotInstalled = MO_HideNotInst.Checked;
            
            try
            {
                // Проверим и создадим ключ реестра для хранения настроек...
                CoreLib.CheckRegKeyAndCreateCU(@"Software\" + GV.AppName);
                
                // Запишем настройки в реестр...
                CoreLib.WriteAppBool("ShowSinglePlayer", GV.AppName, MO_ShowSingle.Checked);
                CoreLib.WriteAppBool("ConfirmExit", GV.AppName, MO_ConfirmExit.Checked);
                //CoreLib.WriteAppBool("HideNotInstalled", GV.AppName, MO_HideNotInst.Checked);
                
                // Показываем сообщение...
                MessageBox.Show(frmMainW.RM.GetString("Opts_Saved"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Закрываем форму...
                this.Close();
            }
            catch
            {
                // Произошло исключение, выведем сообщение...
                MessageBox.Show(frmMainW.RM.GetString("Opts_Error"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MO_Cancel_Click(object sender, EventArgs e)
        {
            // Закрываем форму без сохранения изменений...
            this.Close();
        }
    }
}
