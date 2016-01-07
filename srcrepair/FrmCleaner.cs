/*
 * Модуль менеджера очистки SRC Repair.
 * 
 * Copyright 2011 - 2016 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2016 EasyCoding Team.
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
        public frmCleaner(List<String> CD, string BD, string CI, string SM, bool RO, bool NA, bool RS, bool FB)
        {
            InitializeComponent();
            CleanDirs = CD;
            CleanInfo = CI;
            IsReadOnly = RO;
            NoAutoCheck = NA;
            IsRecursive = RS;
            ForceBackUp = FB;
            SuccessMessage = SM;
            FullBackUpDirPath = BD;
        }

        private List<String> CleanDirs; // Здесь будем хранить каталог для очистки,...
        private bool IsReadOnly;
        private bool NoAutoCheck;
        private bool IsRecursive;
        private bool ForceBackUp;
        private string FullBackUpDirPath;
        private string SuccessMessage;
        private string CleanInfo; // ...и информацию о том, что будем очищать.
        private long TotalSize = 0; // Задаём и обнуляем счётчик общего размера удаляемых файлов...

        /// <summary>
        /// Ищет и добавляет файлы для удаления в таблицу модуля очистки.
        /// </summary>
        /// <param name="CleanDirs">Каталоги для выполнения очистки с маской имени</param>
        /// <param name="Recursive">Включает / отключает рекурсивный поиск</param>
        private void DetectFilesForCleanup(List<String> CleanDirs, bool Recursive)
        {
            foreach (string DirMs in CleanDirs)
            {
                // Извлечём имя каталога с полным путём и маску...
                string CleanDir = Path.GetDirectoryName(DirMs);
                string CleanMask = Path.GetFileName(DirMs);
                
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
                            LvItem.Checked = !NoAutoCheck; // Помечаем флажком...
                            LvItem.ToolTipText = Path.Combine(CleanDir, DItem.Name); // Указываем полный путь во всплывающую подсказку...
                            LvItem.SubItems.Add(CoreLib.SclBytes(DItem.Length)); // Вычисляем размер...
                            LvItem.SubItems.Add(DItem.LastWriteTime.ToString()); // Указываем дату изменения...
                            
                            if (CM_FTable.InvokeRequired)
                            {
                                // Вставляем в таблицу...
                                Invoke((MethodInvoker)delegate() { CM_FTable.Items.Add(LvItem); });
                            }
                            
                            TotalSize += DItem.Length; // Инкрементируем общий счётчик...
                        }

                        if (Recursive)
                        {
                            try
                            {
                                // Пройдём по подкаталогам рекурсивно. Получаем список вложенных...
                                List<String> SubDirs = new List<string>();

                                // Обойдём вложенные каталоги...
                                foreach (DirectoryInfo Dir in DInfo.GetDirectories())
                                {
                                    SubDirs.Add(Path.Combine(Dir.FullName, CleanMask));
                                }

                                // Проверим наличие элементов для обхода...
                                if (SubDirs.Count > 0)
                                {
                                    // Вызовем функцию рекурсивно...
                                    DetectFilesForCleanup(SubDirs, true);
                                }
                            }
                            catch (Exception Ex)
                            {
                                CoreLib.WriteStringToLog(Ex.Message);
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        // Подавляем возможные сообщения и пишем в лог...
                        CoreLib.WriteStringToLog(Ex.Message);
                    }
                }
            }
        }

        private void frmCleaner_Load(object sender, EventArgs e)
        {
            // Изменяем заголовок окна...
            Text = String.Format(Text, CleanInfo);
            
            // Запускаем очистку согласно полученным параметрам...
            if (!GttWrk.IsBusy) { GttWrk.RunWorkerAsync(); }

            // Блокируем возможность изменять выбор...
            CM_FTable.Enabled = !IsReadOnly;
        }

        private void CM_FTable_KeyDown(object sender, KeyEventArgs e)
        {
            if (!GttWrk.IsBusy)
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    foreach (ListViewItem LVI in CM_FTable.Items)
                    {
                        LVI.Checked = true;
                    }
                }

                if (e.Control && e.KeyCode == Keys.D)
                {
                    foreach (ListViewItem LVI in CM_FTable.Items)
                    {
                        LVI.Checked = false;
                    }
                }

                if (e.Control && e.KeyCode == Keys.R)
                {
                    foreach (ListViewItem LVI in CM_FTable.Items)
                    {
                        LVI.Checked = !LVI.Checked;
                    }
                }

                if (e.Control && e.KeyCode == Keys.C)
                {
                    string SelectedFiles = "";
                    foreach (ListViewItem LVI in CM_FTable.Items)
                    {
                        if (LVI.Checked)
                        {
                            SelectedFiles += String.Format("{0}{1}", LVI.ToolTipText, Environment.NewLine);
                        }
                    }
                    Clipboard.SetText(SelectedFiles);
                }
            }
        }

        private void ClnWrk_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Задаём массив для хранения имён удаляемых файлов...
                List<string> DeleteQueue = new List<string>();

                // Добавляем в очередь для очистки...
                Invoke((MethodInvoker)delegate()
                {
                    CM_Info.Text = CoreLib.GetLocalizedString("PS_ProcessPrepare");
                    foreach (ListViewItem LVI in CM_FTable.Items)
                    {
                        if (LVI.Checked)
                        {
                            DeleteQueue.Add(LVI.ToolTipText);
                        }
                    }
                });

                // Добавляем в архив (если выбрано)...
                if (Properties.Settings.Default.PackBeforeCleanup || ForceBackUp)
                {
                    Invoke((MethodInvoker)delegate() { CM_Info.Text = CoreLib.GetLocalizedString("PS_ProgressArchive"); });
                    if (!CoreLib.CompressFiles(DeleteQueue, CoreLib.GenerateBackUpFileName(FullBackUpDirPath, Properties.Resources.BU_PrefixDef)))
                    {
                        MessageBox.Show(CoreLib.GetLocalizedString("PS_ArchFailed"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // Меняем текст в строке статуса...
                Invoke((MethodInvoker)delegate() { CM_Info.Text = CoreLib.GetLocalizedString("PS_ProgressCleanup"); });

                // Формируем счётчики...
                int TotalFiles = DeleteQueue.Count;
                int i = 1, j = 0;

                // Удаляем файлы из очереди очистки...
                foreach (string Fl in DeleteQueue)
                {
                    try { j = (int)Math.Round(((double)i / (double)TotalFiles * (double)100.00), 0); i++; if ((j >= 0) && (j <= 100)) { ClnWrk.ReportProgress(j); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { if (File.Exists(Fl)) { File.SetAttributes(Fl, FileAttributes.Normal); File.Delete(Fl); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                }

                // Удалим пустые каталоги (если разрешено)...
                if (Properties.Settings.Default.RemoveEmptyDirs)
                {
                    try
                    {
                        foreach (string Dir in CleanDirs)
                        {
                            CoreLib.RemoveEmptyDirectories(Path.GetDirectoryName(Dir));
                        }
                    }
                    catch (Exception Ex)
                    {
                        CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("PS_CleanEmptyDirsError"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception Ex)
            {
                // Произошло исключение...
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("PS_CleanupErr"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
        }

        private void ClnWrk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PrbMain.Value = e.ProgressPercentage;
        }

        private void ClnWrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                // Выводим сообщение об успешном окончании очистки...
                CM_Info.Text = CoreLib.GetLocalizedString("PS_ProgressFinished");
                MessageBox.Show(SuccessMessage, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // В случае исключений пишем в отладочный лог...
                CoreLib.WriteStringToLog(e.Error.Message);
            }

            // Закрываем форму...
            Close();
        }

        private void CM_Clean_Click(object sender, EventArgs e)
        {
            if (CM_FTable.Items.Count > 0)
            {
                if (CM_FTable.CheckedItems.Count > 0)
                {
                    if (MessageBox.Show(String.Format(CoreLib.GetLocalizedString("PS_CleanupExecuteQ"), CleanInfo), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Отключаем кнопку отмены, очистки и меняем её текст...
                        CM_Clean.Text = CoreLib.GetLocalizedString("PS_CleanInProgress");
                        CM_Clean.Enabled = false;
                        CM_Clean.Visible = false;
                        
                        // Переключаем видимость контролов...
                        CM_Cancel.Enabled = false;
                        CM_Cancel.Visible = false;
                        PrbMain.Visible = true;

                        // Запускаем поток для выполнения очистки...
                        if (!ClnWrk.IsBusy)
                        {
                            ClnWrk.RunWorkerAsync();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(CoreLib.GetLocalizedString("PS_SelectItemsMsg"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(CoreLib.GetLocalizedString("PS_LoadErr"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CM_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CM_FTable_DoubleClick(object sender, EventArgs e)
        {
            // Исправим известный баг VS с обработчиком двойного клика, снимающим флажок у файла.
            CM_FTable.SelectedItems[0].Checked = !CM_FTable.SelectedItems[0].Checked;
            // Запускаем Проводник и выделяем в нём выбранный пользователем файл...
            Process.Start(Properties.Settings.Default.ShBin, Properties.Settings.Default.ShParam + @" """ + CM_FTable.SelectedItems[0].ToolTipText + @"""");
        }
        
        private void GttWrk_DoWork(object sender, DoWorkEventArgs e)
        {
            DetectFilesForCleanup(CleanDirs, IsRecursive);
        }

        private void GttWrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Указываем сколько МБ освободится при удалении всех файлов...
            CM_Info.Text = String.Format(CoreLib.GetLocalizedString("PS_FrFInfo"), CoreLib.SclBytes(TotalSize));

            // Проверим есть ли кандидаты для удаления (очистки)...
            if (CM_FTable.Items.Count == 0)
            {
                // Выдадим сообщение если очищать нечего...
                MessageBox.Show(CoreLib.GetLocalizedString("PS_LoadErr"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Отключим кнопку запуска очистки...
                CM_Clean.Enabled = false;
                // Закроем форму.
                Close();
            }
            else
            {
                // Включаем кнопку очистки...
                CM_Clean.Enabled = true;
            }
        }

        private void frmCleaner_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = ((e.CloseReason == CloseReason.UserClosing) && (ClnWrk.IsBusy || GttWrk.IsBusy));
        }
    }
}
