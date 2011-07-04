/*
 * Модуль менеджера очистки SRC Repair.
 * 
 * Copyright 2011 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2011 EasyCoding Team.
 * 
 * Лицензия: GPL v3 (см. файл GPL.txt).
 * Лицензия контента: Creative Commons 3.0 BY.
 * 
 * Запрещается использовать этот файл при использовании любой
 * лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
 * 
 * Официальный блог EasyCoding Team: http://www.easycoding.org/
 * Официальная страница проекта: http://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/
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
        private long TotalSize = 0; // Задаём и обнуляем счётчик общего размера удаляемых файлов...

        /// <summary>
        /// Ищет и добавляет файлы для удаления в таблицу модуля очистки.
        /// </summary>
        /// <param name="CleanDir">Каталог для выполнения очистки</param>
        /// <param name="CleanMask">Маска файлов для очистки</param>
        private void DetectFilesForCleanup(string CleanDir, string CleanMask)
        {
            // Проверим чтобы каталог существовал...
            if (Directory.Exists(CleanDir))
            {
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
                        LvItem.ToolTipText = Path.Combine(CleanDir, DItem.Name); // Указываем полный путь во всплывающую подсказку...
                        LvItem.SubItems.Add(CoreLib.SclBytes(DItem.Length)); // Вычисляем размер...
                        LvItem.SubItems.Add(CoreLib.WriteDateToString(DItem.LastWriteTime, false)); // Указываем дату изменения...
                        CM_FTable.Items.Add(LvItem); // Вставляем в таблицу...
                        TotalSize += DItem.Length; // Инкрементируем общий счётчик...
                    }
                    // Если задана маска любых файлов, пройдём по подкаталогам рекурсивно...
                    if (CleanMask == "*.*")
                    {
                        // Получим список подкаталогов в искомом и обойдём их все...
                        foreach (DirectoryInfo Dir in DInfo.GetDirectories())
                        {
                            // Вызовем функцию рекурсивно...
                            DetectFilesForCleanup(Dir.FullName, CleanMask);
                        }
                    }
                }
                catch
                {
                    // Подавляем возможные сообщения...
                }
            }
        }

        /// <summary>
        /// Ищет и удаляет пустые каталоги, оставшиеся после удаления файлов из них.
        /// </summary>
        /// <param name="StartDir">Каталог для выполнения очистки</param>
        private void RemoveEmptyDirectories(string StartDir)
        {
            foreach (var Dir in Directory.GetDirectories(StartDir))
            {
                RemoveEmptyDirectories(Dir);
                if ((Directory.GetFiles(Dir).Length == 0) && (Directory.GetDirectories(Dir).Length == 0))
                {
                    Directory.Delete(Dir, false);
                }
            }
        }

        private void frmCleaner_Load(object sender, EventArgs e)
        {
            // Изменяем заголовок окна...
            this.Text = String.Format(this.Text, CleanInfo);
            
            // Запускаем очистку согласно полученным параметрам...
            DetectFilesForCleanup(CleanDir, CleanMask);
            
            // Проверим есть ли кандидаты для удаления (очистки)...
            if (CM_FTable.Items.Count == 0)
            {
                // Выдадим сообщение если очищать нечего...
                MessageBox.Show(CoreLib.GetLocalizedString("PS_LoadErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Отключим кнопку запуска очистки...
                CM_Clean.Enabled = false;
                // Закроем форму.
                this.Close();
            }
            // Указываем сколько МБ освободится при удалении всех файлов...
            CM_Info.Text = String.Format(CM_Info.Text, CoreLib.SclBytes(TotalSize));
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
                    if (MessageBox.Show(String.Format(CoreLib.GetLocalizedString("PS_CleanupExecuteQ"), CleanInfo), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        try
                        {
                            // Чистим...
                            foreach (ListViewItem LVI in CM_FTable.Items)
                            {
                                if (LVI.Checked)
                                {
                                    if (File.Exists(LVI.ToolTipText))
                                    {
                                        File.Delete(LVI.ToolTipText);
                                    }
                                }
                            }
                            
                            // Удалим пустые каталоги (если разрешено)...
                            if (Properties.Settings.Default.RemoveEmptyDirs)
                            {
                                try
                                {
                                    RemoveEmptyDirectories(CleanDir);
                                }
                                catch
                                {
                                    MessageBox.Show(CoreLib.GetLocalizedString("PS_CleanEmptyDirsError"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }

                            // Выводим сообщение об успешном окончании очистки...
                            MessageBox.Show(CoreLib.GetLocalizedString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Закрываем форму...
                            this.Close();
                        }
                        catch
                        {
                            // Произошло исключение...
                            MessageBox.Show(CoreLib.GetLocalizedString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("PS_SelectItemsMsg"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("PS_LoadErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CM_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CM_FTable_DoubleClick(object sender, EventArgs e)
        {
            // Исправим известный баг VS с обработчиком двойного клика, снимающим флажок у файла.
            if (!(CM_FTable.SelectedItems[0].Checked)) { CM_FTable.SelectedItems[0].Checked = true; } else { CM_FTable.SelectedItems[0].Checked = false; }
            // Запускаем Проводник и выделяем в нём выбранный пользователем файл...
            Process.Start("explorer.exe", @"/select," + @"""" + CM_FTable.SelectedItems[0].ToolTipText + @"""");
        }
    }
}
