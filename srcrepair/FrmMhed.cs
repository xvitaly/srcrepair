/*
 * Модуль Редактор Hosts SRC Repair.
 * 
 * Copyright 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2017 EasyCoding Team.
 * 
 * Лицензия: GPL v3 (см. файл GPL.txt).
 * Лицензия контента: Creative Commons 3.0 BY.
 * 
 * Запрещается использовать этот файл при использовании любой
 * лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
 * 
 * Официальный блог EasyCoding Team: https://www.easycoding.org/
 * Официальная страница проекта: https://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля Micro Hosts Editor.
    /// </summary>
    public partial class FrmHEd : Form
    {
        /// <summary>
        /// Конструктор класса формы модуля Micro Hosts Editor.
        /// </summary>
        public FrmHEd()
        {
            InitializeComponent();
        }

        #region IC
        /// <summary>
        /// Хранит название модуля для служебных целей.
        /// </summary>
        private const string PluginName = "Micro Hosts Editor";

        /// <summary>
        /// Хранит версию модуля для служебных целей.
        /// </summary>
        private const string PluginVersion = "0.8.0";
        #endregion

        #region IV
        /// <summary>
        /// Хранит путь к файлу Hosts.
        /// </summary>
        private string HostsFilePath { get; set; }

        /// <summary>
        /// Хранит и возвращает ID текущей платформы, на которой запущено
        /// приложение (внешний модуль).
        /// </summary>
        private CurrentPlatform Platform { get; set; }
        #endregion

        #region IM
        /// <summary>
        /// Загружаеи содержимое файла Hosts в редактор на форме.
        /// </summary>
        /// <param name="FilePath">Путь к файлу Hosts</param>
        private void ReadHostsToTable(string FilePath)
        {
            // Очистим таблицу...
            HEd_Table.Rows.Clear();

            if (File.Exists(FilePath))
            {
                using (StreamReader OpenedHosts = new StreamReader(FilePath, Encoding.Default))
                {
                    while (OpenedHosts.Peek() >= 0)
                    {
                        // Почистим строку от лишних символов...
                        string ImpStr = CoreLib.CleanStrWx(OpenedHosts.ReadLine());

                        // Проверяем, не пустая ли строка...
                        if (!(String.IsNullOrEmpty(ImpStr)))
                        {
                            // Проверяем, не комментарий ли...
                            if (ImpStr[0] != '#')
                            {
                                // Определим позицию пробела в строке...
                                int SpPos = ImpStr.IndexOf(" ");

                                // Строка почищена, продолжаем...
                                if (SpPos != -1)
                                {
                                    // Записываем в таблицу...
                                    HEd_Table.Rows.Add(ImpStr.Substring(0, SpPos), ImpStr.Remove(0, SpPos + 1));
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Сохраняет содержимое редактора на форме в файл Hosts.
        /// </summary>
        /// <param name="Path">Путь к файлу Hosts</param>
        private void WriteTableToHosts(string Path)
        {
            using (StreamWriter CFile = new StreamWriter(Path, false, Encoding.Default))
            {
                try { CFile.WriteLine(CoreLib.GetTemplateFromResource(Properties.Resources.AHE_TemplateFile)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                foreach (DataGridViewRow Row in HEd_Table.Rows)
                {
                    if ((Row.Cells[0].Value != null) && (Row.Cells[1].Value != null))
                    {
                        if (IPAddress.TryParse(Row.Cells[0].Value.ToString(), out IPAddress IPAddr))
                        {
                            CFile.WriteLine("{0} {1}", IPAddr, Row.Cells[1].Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Начинает процесс сохранения таблицы в файл.
        /// </summary>
        private void SaveToFile()
        {
            if (ProcessManager.IsCurrentUserAdmin()) { try { WriteTableToHosts(HostsFilePath); MessageBox.Show(AppStrings.AHE_Saved, PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information); } catch (Exception Ex) { CoreLib.HandleExceptionEx(String.Format(AppStrings.AHE_SaveException, HostsFilePath), PluginName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); } } else { MessageBox.Show(String.Format(AppStrings.AHE_NoAdminRights, HostsFilePath), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        /// <summary>
        /// Метод, срабатывающий при возникновении события "загрузка формы".
        /// </summary>
        private void FrmHEd_Load(object sender, EventArgs e)
        {
            // Проверим используемую платформу...
            Platform = new CurrentPlatform();
            
            // Проверим наличие прав администратора. Если они отсутствуют - отключим функции сохранения...
            if (!(ProcessManager.IsCurrentUserAdmin())) { HEd_M_Save.Enabled = false; HEd_T_Save.Enabled = false; HEd_M_RestDef.Enabled = false; HEd_Table.ReadOnly = true; HEd_T_Cut.Enabled = false; HEd_T_Paste.Enabled = false; HEd_T_RemRw.Enabled = false; }

            // Укажем версию в заголовке главной формы...
            Text = String.Format(Text, PluginVersion);

            // Определим расположение файла Hosts...
            HostsFilePath = FileManager.GetHostsFileFullPath(Platform.OS);

            // Проверим существование файла...
            if (File.Exists(HostsFilePath))
            {
                // Запишем путь в статусную строку...
                HEd_St_Wrn.Text = HostsFilePath;

                // Считаем содержимое...
                try { ReadHostsToTable(HostsFilePath); } catch (Exception Ex) { CoreLib.HandleExceptionEx(String.Format(AppStrings.AHE_ExceptionDetected, HostsFilePath), PluginName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
            }
            else
            {
                MessageBox.Show(String.Format(AppStrings.AHE_NoFileDetected, HostsFilePath), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Обновить".
        /// </summary>
        private void HEd_T_Refresh_Click(object sender, EventArgs e)
        {
            try { ReadHostsToTable(HostsFilePath); } catch (Exception Ex) { CoreLib.HandleExceptionEx(String.Format(AppStrings.AHE_ExceptionDetected, HostsFilePath), PluginName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Сохранить".
        /// </summary>
        private void HEd_T_Save_Click(object sender, EventArgs e)
        {
            SaveToFile();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Выход".
        /// </summary>
        private void HEd_M_Quit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Восстановить стандартные значения".
        /// </summary>
        private void HEd_M_RestDef_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(AppStrings.AHE_RestDef, PluginName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                HEd_Table.Rows.Clear();
                HEd_Table.Rows.Add("127.0.0.1", "localhost");
                SaveToFile();
            }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Показать справку".
        /// </summary>
        private void HEd_M_OnlHelp_Click(object sender, EventArgs e)
        {
            ProcessManager.OpenWebPage(Properties.Resources.AHE_HelpURL);
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "О программе".
        /// </summary>
        private void HEd_M_About_Click(object sender, EventArgs e)
        {
            FormManager.FormShowAboutApp();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Удалить строку".
        /// </summary>
        private void HEd_T_RemRw_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewCell Cell in HEd_Table.SelectedCells)
                {
                    if (Cell.Selected) { HEd_Table.Rows.RemoveAt(Cell.RowIndex); }
                }
            } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Метод, срабатывающий при прохождении курсора мыши над строкой состояния.
        /// </summary>
        private void HEd_St_Wrn_MouseEnter(object sender, EventArgs e)
        {
            HEd_St_Wrn.ForeColor = Color.Red;
        }

        /// <summary>
        /// Метод, срабатывающий при уходе курсора мыши со строки состояния.
        /// </summary>
        private void HEd_St_Wrn_MouseLeave(object sender, EventArgs e)
        {
            HEd_St_Wrn.ForeColor = Color.Black;
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии левой кнопкой мыши по тексту
        /// строки состояния.
        /// </summary>
        private void HEd_St_Wrn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format(AppStrings.AHE_HMessg, HostsFilePath), PluginName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                ProcessManager.OpenExplorer(HostsFilePath, Platform.OS);
            }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Вырезать".
        /// </summary>
        private void HEd_T_Cut_Click(object sender, EventArgs e)
        {
            try
            {
                if (HEd_Table.Rows[HEd_Table.CurrentRow.Index].Cells[HEd_Table.CurrentCell.ColumnIndex].Value != null)
                {
                    Clipboard.SetText(HEd_Table.Rows[HEd_Table.CurrentRow.Index].Cells[HEd_Table.CurrentCell.ColumnIndex].Value.ToString());
                    HEd_Table.Rows[HEd_Table.CurrentRow.Index].Cells[HEd_Table.CurrentCell.ColumnIndex].Value = null;
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Копировать".
        /// </summary>
        private void HEd_T_Copy_Click(object sender, EventArgs e)
        {
            try
            {
                if (HEd_Table.Rows[HEd_Table.CurrentRow.Index].Cells[HEd_Table.CurrentCell.ColumnIndex].Value != null)
                {
                    Clipboard.SetText(HEd_Table.Rows[HEd_Table.CurrentRow.Index].Cells[HEd_Table.CurrentCell.ColumnIndex].Value.ToString());
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Вставить".
        /// </summary>
        private void HEd_T_Paste_Click(object sender, EventArgs e)
        {
            try { if (Clipboard.ContainsText()) { HEd_Table.Rows[HEd_Table.CurrentRow.Index].Cells[HEd_Table.CurrentCell.ColumnIndex].Value = Clipboard.GetText(); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Открыть в Блокноте".
        /// </summary>
        private void HEd_M_Notepad_Click(object sender, EventArgs e)
        {
            ProcessManager.OpenTextEditor(HostsFilePath, Platform.OS);
        }
        #endregion
    }
}
