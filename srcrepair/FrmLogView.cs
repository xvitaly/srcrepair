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
            try { LV_LogArea.AppendText(File.ReadAllText(LogFileName)); } catch (Exception Ex) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("LV_LoadFailed"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }
    }
}
