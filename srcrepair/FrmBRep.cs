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
            BR_Category.SelectedIndex = 0;
        }

        private void BR_Send_Click(object sender, EventArgs e)
        {
            //
        }

        private void BR_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
