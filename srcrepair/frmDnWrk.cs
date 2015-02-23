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
            if (!DN_Wrk.IsBusy) { DN_Wrk.RunWorkerAsync(); }
        }

        private void frmDnWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && DN_Wrk.IsBusy;
        }

        private void DN_Wrk_DoWork(object sender, DoWorkEventArgs e)
        {
            //
        }

        private void DN_Wrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //
        }
        
    }
}
