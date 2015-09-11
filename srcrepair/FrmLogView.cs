using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace srcrepair
{
    public partial class frmLogView : Form
    {
        private string LogFileName;
        public frmLogView(string LogFile)
        {
            InitializeComponent();
            LogFileName = LogFile;
        }

        private void frmLogView_Load(object sender, EventArgs e)
        {
            // Считаем содержимое выбранного файла...
            LV_LogArea.Lines = File.ReadAllLines(LogFileName);
        }
    }
}
