/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2017 EasyCoding Team.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace srcrepair
{
    /// <summary>
    /// Класс формы менеджера очистки.
    /// </summary>
    public partial class FrmCleaner : Form
    {
        /// <summary>
        /// Конструктор класса формы менеджера очистки.
        /// </summary>
        /// <param name="CD">Каталоги для очистки</param>
        /// <param name="BD">Каталог для сохранения резервных копий</param>
        /// <param name="CI">Текст заголовка</param>
        /// <param name="SM">Текст сообщения, которое будет выдаваться по завершении очистки</param>
        /// <param name="RO">Разрешает / запрещает изменять выбор удаляемых файлов</param>
        /// <param name="NA">Включает / отключает автовыбор файлов флажками</param>
        /// <param name="RS">Включает / отключает рекурсивную очистку</param>
        /// <param name="FB">Включает / отключает принудительное создание резервных копий</param>
        public FrmCleaner(List<String> CD, string BD, string CI, string SM, bool RO, bool NA, bool RS, bool FB)
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

        /// <summary>
        /// Хранит список каталогов для очистки.
        /// </summary>
        private List<String> CleanDirs { get; set; }

        /// <summary>
        /// Разрешает / запрещает изменять выбор удаляемых файлов.
        /// </summary>
        private bool IsReadOnly { get; set; }

        /// <summary>
        /// Включает / отключает автовыбор файлов флажками.
        /// </summary>
        private bool NoAutoCheck { get; set; }

        /// <summary>
        /// Включает / отключает рекурсивную очистку.
        /// </summary>
        private bool IsRecursive { get; set; }

        /// <summary>
        /// Включает / отключает принудительное создание резервных копий.
        /// </summary>
        private bool ForceBackUp { get; set; }

        /// <summary>
        /// Хранит путь к каталогу для сохранения резервных копий.
        /// </summary>
        private string FullBackUpDirPath { get; set; }

        /// <summary>
        /// Содержит текст сообщения, которое будет выдаваться по завершении очистки.
        /// </summary>
        private string SuccessMessage { get; set; }

        /// <summary>
        /// Содержит текст заголовка.
        /// </summary>
        private string CleanInfo { get; set; }

        /// <summary>
        /// Счётчик общего размера удаляемых файлов (в байтах).
        /// </summary>
        private long TotalSize { get; set; } = 0;

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
                            // Обрабатываем найденное. Добавляем...
                            ListViewItem LvItem = new ListViewItem(DItem.Name)
                            {
                                Checked = !NoAutoCheck, // Помечаем флажком...
                                ToolTipText = Path.Combine(CleanDir, DItem.Name) // Указываем полный путь во всплывающую подсказку...
                            };

                            // Вычисляем и указываем размер и дату изменения...
                            LvItem.SubItems.Add(CoreLib.SclBytes(DItem.Length));
                            LvItem.SubItems.Add(DItem.LastWriteTime.ToString());
                            
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

        /// <summary>
        /// Метод, срабатывающий при возникновении события "загрузка формы".
        /// </summary>
        private void FrmCleaner_Load(object sender, EventArgs e)
        {
            // Изменяем заголовок окна...
            Text = String.Format(Text, CleanInfo);
            
            // Запускаем очистку согласно полученным параметрам...
            if (!GttWrk.IsBusy) { GttWrk.RunWorkerAsync(); }

            // Блокируем возможность изменять выбор...
            CM_FTable.Enabled = !IsReadOnly;
        }

        /// <summary>
        /// Метод, срабатывающий при возникновении события "нажатие клавиши"
        /// внутри списка найденных элементов.
        /// </summary>
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
                    string SelectedFiles = String.Empty;
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

        /// <summary>
        /// Метод, срабатывающий асинхронно при запуске механизма очистки.
        /// </summary>
        private void ClnWrk_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Задаём массив для хранения имён удаляемых файлов...
                List<string> DeleteQueue = new List<string>();

                // Добавляем в очередь для очистки...
                Invoke((MethodInvoker)delegate()
                {
                    CM_Info.Text = AppStrings.PS_ProcessPrepare;
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
                    Invoke((MethodInvoker)delegate() { CM_Info.Text = AppStrings.PS_ProgressArchive; });
                    if (!FileManager.CompressFiles(DeleteQueue, FileManager.GenerateBackUpFileName(FullBackUpDirPath, Properties.Resources.BU_PrefixDef)))
                    {
                        MessageBox.Show(AppStrings.PS_ArchFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // Меняем текст в строке статуса...
                Invoke((MethodInvoker)delegate() { CM_Info.Text = AppStrings.PS_ProgressCleanup; });

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
                            FileManager.RemoveEmptyDirectories(Path.GetDirectoryName(Dir));
                        }
                    }
                    catch (Exception Ex)
                    {
                        CoreLib.HandleExceptionEx(AppStrings.PS_CleanEmptyDirsError, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception Ex)
            {
                // Произошло исключение...
                CoreLib.HandleExceptionEx(AppStrings.PS_CleanupErr, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Метод, информирующий основную форму о прогрессе очистки внутри, выполняющейся
        /// в отдельном потоке.
        /// </summary>
        private void ClnWrk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PrbMain.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Метод, срабатывающий по окончании работы механизма очистки в отдельном потоке.
        /// </summary>
        private void ClnWrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                // Выводим сообщение об успешном окончании очистки...
                CM_Info.Text = AppStrings.PS_ProgressFinished;
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

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку запуска очистки.
        /// </summary>
        private void CM_Clean_Click(object sender, EventArgs e)
        {
            if (CM_FTable.Items.Count > 0)
            {
                if (CM_FTable.CheckedItems.Count > 0)
                {
                    if (MessageBox.Show(String.Format(AppStrings.PS_CleanupExecuteQ, CleanInfo), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Отключаем кнопку отмены, очистки и меняем её текст...
                        CM_Clean.Text = AppStrings.PS_CleanInProgress;
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
                    MessageBox.Show(AppStrings.PS_SelectItemsMsg, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(AppStrings.PS_LoadErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку отмены очистки.
        /// </summary>
        private void CM_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Метод, срабатывающий при двойном клике по таблице кандидатов на удаление.
        /// </summary>
        private void CM_FTable_DoubleClick(object sender, EventArgs e)
        {
            // Исправим известный баг VS с обработчиком двойного клика, снимающим флажок у файла.
            CM_FTable.SelectedItems[0].Checked = !CM_FTable.SelectedItems[0].Checked;

            // Запускаем Проводник и выделяем в нём выбранный пользователем файл...
            ProcessManager.OpenExplorer(CM_FTable.SelectedItems[0].ToolTipText, new CurrentPlatform().OS);
        }

        /// <summary>
        /// Асинхронный метод, обнаруживающий и помечающий файлы для удаления.
        /// </summary>
        private void GttWrk_DoWork(object sender, DoWorkEventArgs e)
        {
            DetectFilesForCleanup(CleanDirs, IsRecursive);
        }

        /// <summary>
        /// Метод, срабатывающий по окончании работы механизма поиска кандидатов
        /// на удаление в отдельном потоке.
        /// </summary>
        private void GttWrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Указываем сколько МБ освободится при удалении всех файлов...
            CM_Info.Text = String.Format(AppStrings.PS_FrFInfo, CoreLib.SclBytes(TotalSize));

            // Проверим есть ли кандидаты для удаления (очистки)...
            if (CM_FTable.Items.Count == 0)
            {
                // Выдадим сообщение если очищать нечего...
                MessageBox.Show(AppStrings.PS_LoadErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
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

        /// <summary>
        /// Метод, срабатывающий при попытке закрытия формы.
        /// </summary>
        private void FrmCleaner_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = ((e.CloseReason == CloseReason.UserClosing) && (ClnWrk.IsBusy || GttWrk.IsBusy));
        }
    }
}
