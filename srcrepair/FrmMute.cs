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
    public partial class FrmMute : Form
    {
        private string Banlist;
        public FrmMute(string BL)
        {
            InitializeComponent();
            Banlist = BL;
        }

        private void FrmMute_Load(object sender, EventArgs e)
        {
            //
        }
    }
}
