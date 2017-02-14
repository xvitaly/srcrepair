using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace srcrepair
{
    public partial class FrmCfgSelector : Form
    {
        public string Config { get; private set; }
        private List<String> Configs { get; set; }

        public FrmCfgSelector(List<String> C)
        {
            InitializeComponent();
            Configs = C;
        }

        private void FrmCfgSelector_Load(object sender, EventArgs e)
        {
            // Указываем откуда следует брать список с конфигами...
            CS_CfgSel.DataSource = Configs;
        }

        private void CS_OK_Click(object sender, EventArgs e)
        {
            // Возвращаем результат...
            Config = CS_CfgSel.Text;

            // Возвращаем результат формы "успех"...
            DialogResult = DialogResult.OK;

            // Закрываем форму...
            Close();
        }

        private void CS_Cancel_Click(object sender, EventArgs e)
        {
            // Возвращаем результат формы "отменено"...
            DialogResult = DialogResult.Cancel;

            // Закрываем форму...
            Close();
        }
    }
}
