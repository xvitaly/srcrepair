/*
 * Модуль управления отключёнными игроками.
 * 
 * Copyright 2011 - 2015 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2015 EasyCoding Team.
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
        private string Banlist;
        public FrmMute(string BL)
        {
            InitializeComponent();
            Banlist = BL;
        }

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
                        List<String> Res = new List<String>(ImpStr.Split(' '));
                        
                        // Обойдём полученный массив в цикле...
                        foreach (string Str in Res)
                        {
                            // Проверим валидность значения при помощи регулярного выражения...
                            if (Regex.IsMatch(Str, Properties.Resources.MM_SteamIDRegex))
                            {
                                // Добавляем в форму...
                                MM_Table.Rows.Add(Str);
                            }
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
                        if (Regex.IsMatch(Str, Properties.Resources.MM_SteamIDRegex))
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

        private void UpdateTable(object sender, EventArgs e)
        {
            try { MM_Table.Rows.Clear(); ReadFileToTable(Banlist); } catch (Exception Ex) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("MM_ExceptionDetected"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }

        private void WriteTable(object sender, EventArgs e)
        {
            try { WriteTableToFile(Banlist); MessageBox.Show(CoreLib.GetLocalizedString("MM_SavedOK"), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); } catch (Exception Ex) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("MM_SaveException"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }

        private void AboutDlg(object sender, EventArgs e)
        {
            MessageBox.Show(String.Format("{0} by {1}.", Text, CoreLib.GetAppCompany()), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (MM_Table.Rows[MM_Table.CurrentRow.Index].Cells[MM_Table.CurrentCell.ColumnIndex].Value != null)
                {
                    Clipboard.SetText(MM_Table.Rows[MM_Table.CurrentRow.Index].Cells[MM_Table.CurrentCell.ColumnIndex].Value.ToString());
                    MM_Table.Rows[MM_Table.CurrentRow.Index].Cells[MM_Table.CurrentCell.ColumnIndex].Value = null;
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void MM_Copy_Click(object sender, EventArgs e)
        {
            try
            {
                if (MM_Table.Rows[MM_Table.CurrentRow.Index].Cells[MM_Table.CurrentCell.ColumnIndex].Value != null)
                {
                    Clipboard.SetText(MM_Table.Rows[MM_Table.CurrentRow.Index].Cells[MM_Table.CurrentCell.ColumnIndex].Value.ToString());
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void MM_Paste_Click(object sender, EventArgs e)
        {
            try { if (Clipboard.ContainsText()) { MM_Table.Rows[MM_Table.CurrentRow.Index].Cells[MM_Table.CurrentCell.ColumnIndex].Value = Clipboard.GetText(); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void MM_Delete_Click(object sender, EventArgs e)
        {
            try { if (MM_Table.Rows.Count > 0) { MM_Table.Rows.Remove(MM_Table.CurrentRow); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }
    }
}
