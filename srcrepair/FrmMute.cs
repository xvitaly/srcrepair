/*
 * Модуль управления отключёнными игроками.
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
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Text;
using System.IO;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля управления отключёнными игроками.
    /// </summary>
    public partial class FrmMute : Form
    {
        /// <summary>
        /// Конструктор класса формы модуля управления отключёнными игроками.
        /// </summary>
        /// <param name="BL">Путь к файлу с БД отключённых игроков</param>
        /// <param name="BD">Путь к каталогу резервных копий</param>
        public FrmMute(string BL, string BD)
        {
            InitializeComponent();
            Banlist = BL;
            BackUpDir = BD;
        }

        #region IV
        /// <summary>
        /// Хранит путь к файлу с БД отключённых игроков.
        /// </summary>
        private string Banlist;

        /// <summary>
        /// Хранит путь к каталогу резервных копий.
        /// </summary>
        private string BackUpDir;
        #endregion

        #region IM
        /// <summary>
        /// Считывает содержимое БД и помещает в таблицу формы.
        /// </summary>
        /// <param name="FileName">Путь к файлу с БД отключённых игроков</param>
        private void ReadFileToTable(string FileName)
        {
            // Проверим существование файла...
            if (File.Exists(FileName))
            {
                // Откроем файл...
                using (StreamReader OpenedHosts = new StreamReader(FileName, Encoding.Default))
                {
                    // Читаем файл до получения маркера конца файла...
                    while (OpenedHosts.Peek() >= 0)
                    {
                        // Почистим строку от лишних символов...
                        string ImpStr = CoreLib.CleanStrWx(OpenedHosts.ReadLine());

                        // Представим строку как массив...
                        List<String> Res = ParseRow(ImpStr);
                        
                        // Обойдём полученный массив в цикле...
                        foreach (string Str in Res)
                        {
                            // Добавляем в форму...
                            MM_Table.Rows.Add(Str);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Сохраняет содержимое таблицы формы в файл с БД отключённых игроков.
        /// </summary>
        /// <param name="FileName">Путь к файлу с БД отключённых игроков</param>
        private void WriteTableToFile(string FileName)
        {
            using (StreamWriter CFile = new StreamWriter(FileName, false, Encoding.Default))
            {
                // Записываем заголовок...
                CFile.Write("\x01\x00\x00\x00");
                
                // Обходим таблицу в цикле...
                for (int i = 0; i < MM_Table.Rows.Count - 1; i++)
                {
                    // Работаем только с заполненными полями...
                    if (MM_Table.Rows[i].Cells[0].Value != null)
                    {
                        // Получаем строку...
                        string Str = MM_Table.Rows[i].Cells[0].Value.ToString();
                        
                        // Проверяем на соответствие регулярному выражению...
                        if (Regex.IsMatch(Str, String.Format("^{0}$", Properties.Resources.MM_SteamIDRegex)))
                        {
                            // Строим строку. Для выравнивания используем NULL символы...
                            StringBuilder SB = new StringBuilder();
                            SB.Append(Str);
                            SB.Append('\0', 32 - Str.Length);
                            CFile.Write(SB);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ищет в строке валидные SteamID.
        /// </summary>
        /// <param name="Row">Строка для разбора</param>
        /// <returns>Возвращает найденные валидные SteamID.</returns>
        private List<String> ParseRow(string Row)
        {
            List<String> Result = new List<String>();
            MatchCollection Matches = Regex.Matches(Row, Properties.Resources.MM_SteamIDRegex);
            foreach (Match Mh in Matches) { Result.Add(Mh.Value); }
            return Result;
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку обновления таблицы.
        /// </summary>
        private void UpdateTable(object sender, EventArgs e)
        {
            try { MM_Table.Rows.Clear(); ReadFileToTable(Banlist); } catch (Exception Ex) { CoreLib.HandleExceptionEx(AppStrings.MM_ExceptionDetected, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку сохранения.
        /// </summary>
        private void WriteTable(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.SafeCleanup) { if (File.Exists(Banlist)) { CoreLib.CreateConfigBackUp(CoreLib.SingleToArray(Banlist), BackUpDir, Properties.Resources.BU_PrefixVChat); } }
                WriteTableToFile(Banlist); MessageBox.Show(AppStrings.MM_SavedOK, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex) { CoreLib.HandleExceptionEx(AppStrings.MM_SaveException, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "О плагине".
        /// </summary>
        private void AboutDlg(object sender, EventArgs e)
        {
            MessageBox.Show(String.Format(AppStrings.AppPluginAboutDlg, Text, CurrentApp.AppCompany), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Метод, срабатывающий по окончании загрузки формы.
        /// </summary>
        private void FrmMute_Load(object sender, EventArgs e)
        {
            UpdateTable(sender, e);
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку выхода.
        /// </summary>
        private void MM_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку вырезания
        /// строки в буфер обмена.
        /// </summary>
        private void MM_Cut_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                foreach (DataGridViewCell Cell in MM_Table.SelectedCells)
                {
                    if (Cell.Selected)
                    {
                        SB.AppendFormat("{0} ", Cell.Value);
                        MM_Table.Rows.RemoveAt(Cell.RowIndex);
                    }
                }
                Clipboard.SetText(SB.ToString());
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку копирования
        /// строки в буфер обмена.
        /// </summary>
        private void MM_Copy_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                foreach (DataGridViewCell Cell in MM_Table.SelectedCells)
                {
                    if (Cell.Selected) { SB.AppendFormat("{0} ", Cell.Value); }
                }
                Clipboard.SetText(SB.ToString());
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку вставки из
        /// буфера обмена.
        /// </summary>
        private void MM_Paste_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    List<String> Rows = ParseRow(Clipboard.GetText());
                    foreach (string Row in Rows) { MM_Table.Rows.Add(Row); }
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
            
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку удаления строки.
        /// </summary>
        private void MM_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewCell Cell in MM_Table.SelectedCells)
                {
                    if (Cell.Selected) { MM_Table.Rows.RemoveAt(Cell.RowIndex); }
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку преобразования
        /// формата SteamID.
        /// </summary>
        private void MM_Convert_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewCell Cell in MM_Table.SelectedCells)
                {
                    string CellText = Cell.Value.ToString();
                    if (Cell.Selected && Regex.IsMatch(CellText, Properties.Resources.MM_SteamID32Regex))
                    {
                        Cell.Value = SteamConv.ConvSid32Sidv3(CellText);
                    }
                    else
                    {
                        if (MM_Table.SelectedCells.Count == 1)
                        {
                            MessageBox.Show(AppStrings.MM_ConvRest, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку перехода к Steam
        /// профилю выбранного пользователя.
        /// </summary>
        private void MM_Steam_Click(object sender, EventArgs e)
        {
            try
            {
                if (MM_Table.Rows[MM_Table.CurrentRow.Index].Cells[MM_Table.CurrentCell.ColumnIndex].Value != null)
                {
                    string Value = MM_Table.Rows[MM_Table.CurrentRow.Index].Cells[MM_Table.CurrentCell.ColumnIndex].Value.ToString();
                    ProcessManager.OpenWebPage(String.Format(Properties.Resources.MM_CommunityURL, Regex.IsMatch(Value, Properties.Resources.MM_SteamID32Regex) ? SteamConv.ConvSid32Sid64(Value) : SteamConv.ConvSidv3Sid64(Value)));
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }
        #endregion
    }
}
