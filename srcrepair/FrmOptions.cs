/*
 * Модуль настроек программы SRC Repair.
 * 
 * Copyright 2011 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2013 EasyCoding Team.
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
using System.Text.RegularExpressions;

namespace srcrepair
{
    public partial class frmOptions : Form
    {
        public frmOptions()
        {
            InitializeComponent();
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            // Считаем текущие настройки...
            MO_ConfirmExit.Checked = Properties.Settings.Default.ConfirmExit;
            MO_SortGameList.Checked = Properties.Settings.Default.SortGamesList;
            MO_AutoUpdate.Checked = Properties.Settings.Default.EnableAutoUpdate;
            MO_RemEmptyDirs.Checked = Properties.Settings.Default.RemoveEmptyDirs;
            MO_PrefHelpSystem.SelectedIndex = Properties.Settings.Default.PreferedHelpSystem;
            MO_EnableAppLogs.Checked = Properties.Settings.Default.EnableDebugLog;
            MO_ShBin.Text = Properties.Settings.Default.ShBin;
            MO_TextEdBin.Text = Properties.Settings.Default.EditorBin;
            MO_CustDirInstArc.Checked = Properties.Settings.Default.InstallToUserCustDir;
            MO_CustDirName.Text = Properties.Settings.Default.UserCustDirName;

            // Укажем название приложения в заголовке окна...
            this.Text = String.Format(this.Text, GV.AppName);
        }

        private void MO_Okay_Click(object sender, EventArgs e)
        {
            // Сохраняем настройки для текущего сеанса...
            Properties.Settings.Default.ConfirmExit = MO_ConfirmExit.Checked;
            Properties.Settings.Default.SortGamesList = MO_SortGameList.Checked;
            Properties.Settings.Default.EnableAutoUpdate = MO_AutoUpdate.Checked;
            Properties.Settings.Default.RemoveEmptyDirs = MO_RemEmptyDirs.Checked;
            Properties.Settings.Default.PreferedHelpSystem = MO_PrefHelpSystem.SelectedIndex;
            Properties.Settings.Default.EnableDebugLog = MO_EnableAppLogs.Checked;
            Properties.Settings.Default.EditorBin = MO_TextEdBin.Text;
            Properties.Settings.Default.ShBin = MO_ShBin.Text;
            Properties.Settings.Default.InstallToUserCustDir = MO_CustDirInstArc.Checked;
            if (Regex.IsMatch(MO_CustDirName.Text, @"^[0-9a-zA-Z]*$")) { Properties.Settings.Default.UserCustDirName = MO_CustDirName.Text; }

            // Сохраняем настройки...
            Properties.Settings.Default.Save();

            // Показываем сообщение...
            MessageBox.Show(CoreLib.GetLocalizedString("Opts_Saved"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // Закрываем форму...
            this.Close();
        }

        private void MO_Cancel_Click(object sender, EventArgs e)
        {
            // Закрываем форму без сохранения изменений...
            this.Close();
        }

        private void MO_FindTextEd_Click(object sender, EventArgs e)
        {
            if (MO_SearchBin.ShowDialog() == DialogResult.OK)
            {
                MO_TextEdBin.Text = MO_SearchBin.FileName;
            }
        }

        private void MO_FindShBin_Click(object sender, EventArgs e)
        {
            if (MO_SearchBin.ShowDialog() == DialogResult.OK)
            {
                MO_ShBin.Text = MO_SearchBin.FileName;
            }
        }
    }
}
