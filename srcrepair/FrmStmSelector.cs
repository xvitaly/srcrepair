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
    public partial class FrmStmSelector : Form
    {
        public string SteamID { get; private set; }
        private List<String> SteamIDs;

        public FrmStmSelector(List<String> S)
        {
            InitializeComponent();
            SteamIDs = S;
        }

        private void FrmStmSelector_Load(object sender, EventArgs e)
        {
            // Указываем откуда следует брать список со SteamID...
            SD_IDSel.DataSource = SteamIDs;
        }

        private void ST_OK_Click(object sender, EventArgs e)
        {
            // Возвращаем результат...
            SteamID = SD_IDSel.Text;

            // Возвращаем результат формы "успех"...
            DialogResult = DialogResult.OK;

            // Закрываем форму...
            Close();
        }

        private void ST_Cancel_Click(object sender, EventArgs e)
        {
            // Возвращаем результат формы "отменено"...
            DialogResult = DialogResult.Cancel;

            // Закрываем форму...
            Close();
        }
    }
}
