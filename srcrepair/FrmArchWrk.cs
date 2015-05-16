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
    public partial class FrmArchWrk : Form
    {
        private bool IsRunning = true;
        private string ArchName;
        private string DestDir;
        
        public FrmArchWrk(string A, string D)
        {
            InitializeComponent();
            ArchName = A;
            DestDir = D;
        }

        private void FrmArchWrk_Load(object sender, EventArgs e)
        {
            //
        }
    }
}
