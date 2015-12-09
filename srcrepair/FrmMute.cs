using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

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
            try { ReadFileToTable(Banlist); } catch (Exception Ex) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("MM_ExceptionDetected"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }

        private void WriteTable(object sender, EventArgs e)
        {
            WriteTableToFile(Banlist);
        }

        private void FrmMute_Load(object sender, EventArgs e)
        {
            UpdateTable(sender, e);
        }

        private void MM_Exit_Click(object sender, EventArgs e)
        {
            //
            Close();
        }

        private void MM_HAbout_Click(object sender, EventArgs e)
        {
            //
        }

        private void MM_Cut_Click(object sender, EventArgs e)
        {
            //
        }

        private void MM_Copy_Click(object sender, EventArgs e)
        {
            //
        }

        private void MM_Paste_Click(object sender, EventArgs e)
        {
            //
        }

        private void MM_About_Click(object sender, EventArgs e)
        {
            //
        }
    }
}
