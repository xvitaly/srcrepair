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

        private void UpdateTable(object sender, EventArgs e)
        {
            try { ReadFileToTable(Banlist); } catch (Exception Ex) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("MM_ExceptionDetected"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }

        private void FrmMute_Load(object sender, EventArgs e)
        {
            UpdateTable(sender, e);
        }

        private void MM_FSave_Click(object sender, EventArgs e)
        {
            //
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

        private void MM_Save_Click(object sender, EventArgs e)
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
