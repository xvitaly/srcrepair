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
        private List<String> SteamIDs;

        public FrmStmSelector(List<String> S)
        {
            InitializeComponent();
            SteamIDs = S;
        }

        private void FrmStmSelector_Load(object sender, EventArgs e)
        {
            SD_IDSel.DataSource = SteamIDs;
        }

        private void ST_OK_Click(object sender, EventArgs e)
        {
            //
            DialogResult = DialogResult.OK;
        }

        private void ST_Cancel_Click(object sender, EventArgs e)
        {
            //
            DialogResult = DialogResult.Cancel;
        }
    }
}
