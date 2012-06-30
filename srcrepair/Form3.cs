/*
 * Пустой модуль SRC Repair.
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
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace srcrepair
{
    public partial class frmHEd : Form
    {
        public frmHEd()
        {
            InitializeComponent();
        }

        #region IC
        private const string PluginName = "Micro Hosts Editor";
        private const string PluginVersion = "0.4";
        #endregion

        #region IV
        private string HostsFilePath = "";
        #endregion

        #region IM
        private void ReadHostsToTable(string FilePath)
        {
            // Очистим таблицу...
            HEd_Table.Rows.Clear();

            // Начинаем считывать и парсить содержимое файла...
            string ImpStr; // Строка для парсинга...
            string Buf; // Буферная переменная...            
            using (StreamReader OpenedHosts = new StreamReader(@FilePath, Encoding.Default))
            {
                while (OpenedHosts.Peek() >= 0)
                {
                    // Начинаем...
                    ImpStr = OpenedHosts.ReadLine(); // считали строку...
                    ImpStr = ImpStr.Trim(); // почистим строку от лишних пробелов...
                    // Начинаем парсить считанную строку...
                    if (!(String.IsNullOrEmpty(ImpStr))) // проверяем, не пустая ли строка...
                    {
                        if (ImpStr[0] != '#') // проверяем, не комментарий ли...
                        {
                            // Ищем символ табуляции и заменим его на пробел...
                            while (ImpStr.IndexOf("\t") != -1)
                            {
                                ImpStr = ImpStr.Replace("\t", " ");
                            }

                            // Удалим все лишние пробелы...
                            while (ImpStr.IndexOf("  ") != -1) // пока остались двойные пробелы, продолжаем...
                            {
                                ImpStr = ImpStr.Replace("  ", " "); // удаляем найденный лишний пробел...
                            }

                            // Строка почищена, продолжаем...
                            if (ImpStr.IndexOf(" ") != -1)
                            {
                                Buf = ImpStr.Substring(0, ImpStr.IndexOf(" ")); // мы получили IP-адрес...
                                ImpStr = ImpStr.Remove(0, ImpStr.IndexOf(" ") + 1); // удаляем полученное на предыдущем шаге...
                                HEd_Table.Rows.Add(Buf, ImpStr); // записываем в таблицу...
                            }
                        }
                    }
                }
            }
        }

        private void WriteTableToHosts(string Path)
        {
            using (StreamWriter CFile = new StreamWriter(Path))
            {
                if (GV.RunningPlatform == 0)
                {
                    CFile.WriteLine("# Copyright (c) 1993-2009 Microsoft Corp.");
                    CFile.WriteLine("#");
                    CFile.WriteLine("# This is a sample HOSTS file used by Microsoft TCP/IP for Windows.");
                    CFile.WriteLine("#");
                    CFile.WriteLine("# This file contains the mappings of IP addresses to host names. Each");
                    CFile.WriteLine("# entry should be kept on an individual line. The IP address should");
                    CFile.WriteLine("# be placed in the first column followed by the corresponding host name.");
                    CFile.WriteLine("# The IP address and the host name should be separated by at least one");
                    CFile.WriteLine("# space.");
                    CFile.WriteLine("#");
                    CFile.WriteLine("# Additionally, comments (such as these) may be inserted on individual");
                    CFile.WriteLine("# lines or following the machine name denoted by a ‘#’ symbol.");
                    CFile.WriteLine("#");
                    CFile.WriteLine("# For example:");
                    CFile.WriteLine("#");
                    CFile.WriteLine("# 102.54.94.97 rhino.acme.com # source server");
                    CFile.WriteLine("# 38.25.63.10 x.acme.com # x client host");
                    CFile.WriteLine("");
                    CFile.WriteLine("# localhost name resolution is handled within DNS itself.");
                    CFile.WriteLine("# 127.0.0.1 localhost");
                    CFile.WriteLine("# ::1 localhost");
                    CFile.WriteLine("");
                }
                for (int i = 0; i < HEd_Table.Rows.Count - 1; i++)
                {
                    if ((HEd_Table.Rows[i].Cells[0].Value != null) && (HEd_Table.Rows[i].Cells[1].Value != null))
                    {
                        CFile.Write(HEd_Table.Rows[i].Cells[0].Value.ToString());
                        CFile.Write(" ");
                        CFile.WriteLine(HEd_Table.Rows[i].Cells[1].Value.ToString());
                    }
                }
                CFile.Close();
            }
        }
        
        private void frmHEd_Load(object sender, EventArgs e)
        {
            // Проверим наличие прав администратора. Если они отсутствуют - отключим функции сохранения...
            if (!(CoreLib.IsCurrentUserAdmin()))
            {
                HEd_M_Save.Enabled = false;
                HEd_T_Save.Enabled = false;
                HEd_M_RestDef.Enabled = false;
                HEd_Table.ReadOnly = true;
                HEd_T_Cut.Enabled = false;
                HEd_T_Paste.Enabled = false;
                HEd_T_RemRw.Enabled = false;
            }

            // Укажем версию в заголовке главной формы...
            this.Text = String.Format(this.Text, PluginVersion);

            if (GV.RunningPlatform == 0)
            {
                try
                {
                    // Получим путь к файлу hosts (вдруг он переопределён каким-либо зловредом)...
                    RegistryKey ResKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", false);
                    if (ResKey != null) { HostsFilePath = (string)ResKey.GetValue("DataBasePath"); }
                    // Проверим получен ли путь из реестра. Если нет, вставим стандартный...
                    if (String.IsNullOrWhiteSpace(HostsFilePath))
                    {
                        MessageBox.Show(CoreLib.GetLocalizedString("AHE_KRegErr"), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        HostsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "drivers", "etc");
                    }
                }
                catch
                {
                    // Произошло исключение, поэтому укажем вручную...
                    HostsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "drivers", "etc");
                }

                // Сгенерируем полный путь к файлу hosts...
                HostsFilePath = Path.Combine(HostsFilePath, "hosts");
            }
            else
            {
                HostsFilePath = Path.Combine("/etc", "hosts");
            }

            // Проверим существование файла...
            if (File.Exists(HostsFilePath))
            {
                // Запишем путь в статусную строку...
                HEd_St_Wrn.Text = HostsFilePath;

                // Считаем содержимое...
                try
                {
                    ReadHostsToTable(HostsFilePath);
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(String.Format(CoreLib.GetLocalizedString("AHE_ExceptionDetected"), HostsFilePath), PluginName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(String.Format(CoreLib.GetLocalizedString("AHE_NoFileDetected"), HostsFilePath), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void HEd_T_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                ReadHostsToTable(HostsFilePath);
            }
            catch (Exception Ex)
            {
                CoreLib.HandleExceptionEx(String.Format(CoreLib.GetLocalizedString("AHE_ExceptionDetected"), HostsFilePath), PluginName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
        }

        private void HEd_T_Save_Click(object sender, EventArgs e)
        {
            // Сохраняем файл если есть соответствующие права...
            if (CoreLib.IsCurrentUserAdmin())
            {
                try
                {
                    WriteTableToHosts(HostsFilePath);
                    MessageBox.Show(CoreLib.GetLocalizedString("AHE_Saved"), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(String.Format(CoreLib.GetLocalizedString("AHE_SaveException"), HostsFilePath), PluginName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(String.Format(CoreLib.GetLocalizedString(""), HostsFilePath), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HEd_M_Quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HEd_M_RestDef_Click(object sender, EventArgs e)
        {
            // Restore default
            if (MessageBox.Show(CoreLib.GetLocalizedString("AHE_RestDef"), PluginName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                HEd_Table.Rows.Clear();
                HEd_Table.Rows.Add("127.0.0.1", "localhost");
                HEd_T_Save.PerformClick();
            }
        }

        private void HEd_M_OnlHelp_Click(object sender, EventArgs e)
        {
            Process.Start(String.Format("http://code.google.com/p/srcrepair/wiki/HostsEditorPlugin_{0}", CoreLib.GetLocalizedString("AppLangPrefix")));
        }

        private void HEd_M_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Format("{0} for {1} by {2}. Version: {3}.", PluginName, GV.AppName, "V1TSK", PluginVersion), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HEd_T_RemRw_Click(object sender, EventArgs e)
        {
            try
            {
                if (HEd_Table.Rows.Count > 0)
                {
                    HEd_Table.Rows.Remove(HEd_Table.CurrentRow);
                }
            }
            catch
            {
                // Подавляем возможные сообщения об ошибках...
            }
        }

        private void HEd_St_Wrn_MouseEnter(object sender, EventArgs e)
        {
            HEd_St_Wrn.ForeColor = Color.Red;
        }

        private void HEd_St_Wrn_MouseLeave(object sender, EventArgs e)
        {
            HEd_St_Wrn.ForeColor = Color.Black;
        }

        private void HEd_St_Wrn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format(CoreLib.GetLocalizedString("AHE_HMessg"), HostsFilePath), PluginName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Process.Start(Properties.Settings.Default.ShBin, Properties.Settings.Default.ShParam + @" """ + HostsFilePath + @"""");
            }
        }

        private void HEd_T_Cut_Click(object sender, EventArgs e)
        {
            if (HEd_Table.Rows[HEd_Table.CurrentRow.Index].Cells[HEd_Table.CurrentCell.ColumnIndex].Value != null)
            {
                // Копируем в буфер...
                Clipboard.SetText(HEd_Table.Rows[HEd_Table.CurrentRow.Index].Cells[HEd_Table.CurrentCell.ColumnIndex].Value.ToString());
                // Удаляем из ячейки...
                HEd_Table.Rows[HEd_Table.CurrentRow.Index].Cells[HEd_Table.CurrentCell.ColumnIndex].Value = null;
            }
        }

        private void HEd_T_Copy_Click(object sender, EventArgs e)
        {
            if (HEd_Table.Rows[HEd_Table.CurrentRow.Index].Cells[HEd_Table.CurrentCell.ColumnIndex].Value != null)
            {
                Clipboard.SetText(HEd_Table.Rows[HEd_Table.CurrentRow.Index].Cells[HEd_Table.CurrentCell.ColumnIndex].Value.ToString());
            }
        }

        private void HEd_T_Paste_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    HEd_Table.Rows[HEd_Table.CurrentRow.Index].Cells[HEd_Table.CurrentCell.ColumnIndex].Value = Clipboard.GetText();
                }
            }
            catch
            {
            }
        }

        private void HEd_M_Notepad_Click(object sender, EventArgs e)
        {
            // Откроем файл Hosts в Блокноте...
            Process.Start(Properties.Settings.Default.EditorBin, @"""" + HostsFilePath + @"""");
        }

        private void HEd_M_RepBug_Click(object sender, EventArgs e)
        {
            Process.Start("http://code.google.com/p/mhed/issues/entry");
        }
        #endregion
    }
}
