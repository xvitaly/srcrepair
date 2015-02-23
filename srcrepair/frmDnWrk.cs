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
    public partial class frmDnWrk : Form
    {
        private string RemoteURI;
        private string LocalFile;

        public frmDnWrk(string R, string L)
        {
            InitializeComponent();
            RemoteURI = R;
            LocalFile = L;
        }

        private void frmDnWrk_Load(object sender, EventArgs e)
        {
            //
        }
    }
}
