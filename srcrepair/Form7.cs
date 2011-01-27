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
    public partial class frmCleaner : Form
    {
        public frmCleaner(string CD, string CM, string CI)
        {
            InitializeComponent();
            CleanDir = CD;
            CleanMask = CM;
            CleanInfo = CI;
        }

        private string CleanDir;
        private string CleanMask;
        private string CleanInfo;

        private void frmCleaner_Load(object sender, EventArgs e)
        {
            // Изменяем заголовок окна...
            this.Text = String.Format(this.Text, CleanInfo);
            // Задаём и обнуляем счётчик общего размера удаляемых файлов...
            long TotalSize = 0;
            // Открываем каталог...
            DirectoryInfo DInfo = new DirectoryInfo(CleanDir);
            // Считываем список файлов по заданной маске...
            FileInfo[] DirList = DInfo.GetFiles(CleanMask);
            // Начинаем обход массива...
            foreach (FileInfo DItem in DirList)
            {
                // Обрабатываем найденное...
                ListViewItem LvItem = new ListViewItem(DItem.Name); // Добавляем...
                LvItem.Checked = true; // Помечаем флажком...
                LvItem.ToolTipText = CleanDir + DItem.Name; // Указываем полный путь во всплывающую подсказку...
                LvItem.SubItems.Add((DItem.Length / 1024 / 1024).ToString() + " MB"); // Вычисляем размер...
                CM_FTable.Items.Add(LvItem); // Вставляем в таблицу...
                TotalSize += DItem.Length; // Инкрементируем общий счётчик...
            }
            // Указываем сколько МБ освободится при удалении всех файлов...
            CM_Info.Text = String.Format(CM_Info.Text, (TotalSize / 1024 / 1024).ToString());
        }

        private void CM_FTable_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem LVI in CM_FTable.Items)
                {
                    LVI.Checked = true;
                }
            }
        }

        private void CM_Clean_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format(frmMainW.RM.GetString("PS_CleanupExecuteQ"), CleanInfo), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    // Чистим...
                    foreach (ListViewItem LVI in CM_FTable.Items)
                    {
                        if (LVI.Checked)
                        {
                            if (File.Exists(CleanDir + LVI.Text))
                            {
                                File.Delete(CleanDir + LVI.Text);
                            }
                        }
                    }
                    // Выводим сообщение об успешном окончании очистки...
                    MessageBox.Show(frmMainW.RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    // Произошло исключение...
                    MessageBox.Show(frmMainW.RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void CM_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
