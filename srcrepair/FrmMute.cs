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
    public partial class FrmMute : Form
    {
        public FrmMute(string BL, string BD)
        {
            InitializeComponent();
            Banlist = BL;
            BackUpDir = BD;
        }

        #region IV
        private string Banlist;
        private string BackUpDir;
        #endregion

        #region IM
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

        private List<String> ParseRow(string Row)
        {
            List<String> Result = new List<String>();
            MatchCollection Matches = Regex.Matches(Row, Properties.Resources.MM_SteamIDRegex);
            foreach (Match Mh in Matches) { Result.Add(Mh.Value); }
            return Result;
        }

        private void UpdateTable(object sender, EventArgs e)
        {
            try { MM_Table.Rows.Clear(); ReadFileToTable(Banlist); } catch (Exception Ex) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("MM_ExceptionDetected"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }

        private void WriteTable(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.SafeCleanup) { if (File.Exists(Banlist)) { CoreLib.CreateConfigBackUp(CoreLib.SingleToArray(Banlist), BackUpDir, Properties.Resources.BU_PrefixVChat); } }
                WriteTableToFile(Banlist); MessageBox.Show(CoreLib.GetLocalizedString("MM_SavedOK"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("MM_SaveException"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }

        private void AboutDlg(object sender, EventArgs e)
        {
            MessageBox.Show(String.Format(CoreLib.GetLocalizedString("MM_AboutDlg"), Text, CoreLib.GetAppCompany()), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FrmMute_Load(object sender, EventArgs e)
        {
            UpdateTable(sender, e);
        }

        private void MM_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

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
                            MessageBox.Show(CoreLib.GetLocalizedString("MM_ConvRest"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void MM_Steam_Click(object sender, EventArgs e)
        {
            try
            {
                if (MM_Table.Rows[MM_Table.CurrentRow.Index].Cells[MM_Table.CurrentCell.ColumnIndex].Value != null)
                {
                    string Value = MM_Table.Rows[MM_Table.CurrentRow.Index].Cells[MM_Table.CurrentCell.ColumnIndex].Value.ToString();
                    CoreLib.OpenWebPage(String.Format(Properties.Resources.MM_CommunityURL, Regex.IsMatch(Value, Properties.Resources.MM_SteamID32Regex) ? SteamConv.ConvSid32Sid64(Value) : SteamConv.ConvSidv3Sid64(Value)));
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }
        #endregion
    }
}
