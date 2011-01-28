using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

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

        private string CleanDir; // Здесь будем хранить каталог для очистки,...
        private string CleanMask; // ...маску...
        private string CleanInfo; // ...и информацию о том, что будем очищать.

        private void frmCleaner_Load(object sender, EventArgs e)
        {
            // Изменяем заголовок окна...
            this.Text = String.Format(this.Text, CleanInfo);
            // Задаём и обнуляем счётчик общего размера удаляемых файлов...
            long TotalSize = 0;
            // Запускаем...
            try
            {
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
                    LvItem.SubItems.Add((DItem.Length / 1024).ToString() + " KB"); // Вычисляем размер...
                    CM_FTable.Items.Add(LvItem); // Вставляем в таблицу...
                    TotalSize += DItem.Length; // Инкрементируем общий счётчик...
                }
            }
            catch
            {
                // Подавляем возможные сообщения...
            }
            // Выдадим сообщение если очищать нечего...
            if (CM_FTable.Items.Count == 0)
            {
                MessageBox.Show(frmMainW.RM.GetString("PS_LoadErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CM_Clean.Enabled = false;
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
            if (CM_FTable.Items.Count > 0)
            {
                if (CM_FTable.CheckedItems.Count > 0)
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
                else
                {
                    MessageBox.Show(frmMainW.RM.GetString("PS_SelectItemsMsg"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(frmMainW.RM.GetString("PS_LoadErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CM_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CM_FTable_DoubleClick(object sender, EventArgs e)
        {
            // Запускаем Проводник и выделяем в нём выбранный пользователем файл...
            Process.Start("explorer.exe", @"/select," + @"""" + CleanDir + CM_FTable.SelectedItems[0].SubItems[0].Text + @"""");
        }
    }
}
