/*
 * Модуль Редактор Hosts SRC Repair.
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Net;

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
        private const string PluginVersion = "0.6";
        #endregion

        #region IV
        private string HostsFilePath = "";
        #endregion

        #region IM
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

        private void WriteTableToHosts(string Path)
        {
            using (StreamWriter CFile = new StreamWriter(Path, false, Encoding.Default))
            {
                try { CFile.WriteLine(CoreLib.GetTemplateFromResource(Properties.Resources.AHE_TemplateFile)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                for (int i = 0; i < HEd_Table.Rows.Count - 1; i++)
                {
                    if ((HEd_Table.Rows[i].Cells[0].Value != null) && (HEd_Table.Rows[i].Cells[1].Value != null))
                    {
                        IPAddress IPAddr;
                        if (IPAddress.TryParse(HEd_Table.Rows[i].Cells[0].Value.ToString(), out IPAddr))
                        {
                            CFile.WriteLine("{0} {1}", IPAddr, HEd_Table.Rows[i].Cells[1].Value);
                        }
                    }
                }
            }
        }
        
        private void frmHEd_Load(object sender, EventArgs e)
        {
            // Проверим наличие прав администратора. Если они отсутствуют - отключим функции сохранения...
            if (!(CoreLib.IsCurrentUserAdmin())) { HEd_M_Save.Enabled = false; HEd_T_Save.Enabled = false; HEd_M_RestDef.Enabled = false; HEd_Table.ReadOnly = true; HEd_T_Cut.Enabled = false; HEd_T_Paste.Enabled = false; HEd_T_RemRw.Enabled = false; }

            // Укажем версию в заголовке главной формы...
            Text = String.Format(Text, PluginVersion);

            // Определим расположение файла Hosts...
            HostsFilePath = CoreLib.GetHostsFileFullPath();

            // Проверим существование файла...
            if (File.Exists(HostsFilePath))
            {
                // Запишем путь в статусную строку...
                HEd_St_Wrn.Text = HostsFilePath;

                // Считаем содержимое...
                try { ReadHostsToTable(HostsFilePath); } catch (Exception Ex) { CoreLib.HandleExceptionEx(String.Format(CoreLib.GetLocalizedString("AHE_ExceptionDetected"), HostsFilePath), PluginName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
            }
            else
            {
                MessageBox.Show(String.Format(CoreLib.GetLocalizedString("AHE_NoFileDetected"), HostsFilePath), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
        }

        private void HEd_T_Refresh_Click(object sender, EventArgs e)
        {
            try { ReadHostsToTable(HostsFilePath); } catch (Exception Ex) { CoreLib.HandleExceptionEx(String.Format(CoreLib.GetLocalizedString("AHE_ExceptionDetected"), HostsFilePath), PluginName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }

        private void HEd_T_Save_Click(object sender, EventArgs e)
        {
            if (CoreLib.IsCurrentUserAdmin()) { try { WriteTableToHosts(HostsFilePath); MessageBox.Show(CoreLib.GetLocalizedString("AHE_Saved"), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information); } catch (Exception Ex) { CoreLib.HandleExceptionEx(String.Format(CoreLib.GetLocalizedString("AHE_SaveException"), HostsFilePath), PluginName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); } } else { MessageBox.Show(String.Format(CoreLib.GetLocalizedString("AHE_NoAdminRights"), HostsFilePath), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void HEd_M_Quit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HEd_M_RestDef_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(CoreLib.GetLocalizedString("AHE_RestDef"), PluginName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                HEd_Table.Rows.Clear();
                HEd_Table.Rows.Add("127.0.0.1", "localhost");
                HEd_T_Save.PerformClick();
            }
        }

        private void HEd_M_OnlHelp_Click(object sender, EventArgs e)
        {
            CoreLib.OpenWebPage(Properties.Resources.AHE_HelpURL);
        }

        private void HEd_M_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Format(Properties.Resources.AHE_About, PluginName, Properties.Resources.AppName, "V1TSK", PluginVersion), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HEd_T_RemRw_Click(object sender, EventArgs e)
        {
            try { if (HEd_Table.Rows.Count > 0) { HEd_Table.Rows.Remove(HEd_Table.CurrentRow); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
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
                try { Process.Start(Properties.Settings.Default.ShBin, String.Format("{0} \"{1}\"", Properties.Settings.Default.ShParam, HostsFilePath)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
            }
        }

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

        private void HEd_T_Paste_Click(object sender, EventArgs e)
        {
            try { if (Clipboard.ContainsText()) { HEd_Table.Rows[HEd_Table.CurrentRow.Index].Cells[HEd_Table.CurrentCell.ColumnIndex].Value = Clipboard.GetText(); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void HEd_M_Notepad_Click(object sender, EventArgs e)
        {
            try { Process.Start(Properties.Settings.Default.EditorBin, String.Format("\"{0}\"", HostsFilePath)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }
        #endregion
    }
}
