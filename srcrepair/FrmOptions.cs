/*
 * Модуль настроек программы SRC Repair.
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
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля настроек программы.
    /// </summary>
    public partial class FrmOptions : Form
    {
        /// <summary>
        /// Конструктор класса формы модуля настроек программы.
        /// </summary>
        public FrmOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод, срабатывающий при возникновении события "загрузка формы".
        /// </summary>
        private void frmOptions_Load(object sender, EventArgs e)
        {
            // Считаем текущие настройки...
            MO_ConfirmExit.Checked = Properties.Settings.Default.ConfirmExit;
            MO_AutoUpdate.Checked = Properties.Settings.Default.EnableAutoUpdate;
            MO_RemEmptyDirs.Checked = Properties.Settings.Default.RemoveEmptyDirs;
            MO_HighlightOldBackUps.Checked = Properties.Settings.Default.HighlightOldBackUps;
            MO_EnableAppLogs.Checked = Properties.Settings.Default.EnableDebugLog;
            MO_ShBin.Text = Properties.Settings.Default.ShBin;
            MO_TextEdBin.Text = Properties.Settings.Default.EditorBin;
            MO_CustDirName.Text = Properties.Settings.Default.UserCustDirName;
            MO_ZipCompress.Checked = Properties.Settings.Default.PackBeforeCleanup;
            MO_UnSafeOps.Checked = Properties.Settings.Default.AllowUnSafeCleanup;
            MO_UseUpstream.Checked = Properties.Settings.Default.HUDUseUpstream;

            // Укажем название приложения в заголовке окна...
            Text = String.Format(Text, Properties.Resources.AppName);
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "OK".
        /// </summary>
        private void MO_Okay_Click(object sender, EventArgs e)
        {
            // Сохраняем настройки для текущего сеанса...
            Properties.Settings.Default.ConfirmExit = MO_ConfirmExit.Checked;
            Properties.Settings.Default.EnableAutoUpdate = MO_AutoUpdate.Checked;
            Properties.Settings.Default.RemoveEmptyDirs = MO_RemEmptyDirs.Checked;
            Properties.Settings.Default.HighlightOldBackUps = MO_HighlightOldBackUps.Checked;
            Properties.Settings.Default.EnableDebugLog = MO_EnableAppLogs.Checked;
            Properties.Settings.Default.EditorBin = MO_TextEdBin.Text;
            Properties.Settings.Default.ShBin = MO_ShBin.Text;
            Properties.Settings.Default.PackBeforeCleanup = MO_ZipCompress.Checked;
            Properties.Settings.Default.AllowUnSafeCleanup = MO_UnSafeOps.Checked;
            Properties.Settings.Default.HUDUseUpstream = MO_UseUpstream.Checked;
            if (Regex.IsMatch(MO_CustDirName.Text, Properties.Resources.MO_CustomDirRegex)) { Properties.Settings.Default.UserCustDirName = MO_CustDirName.Text; }

            // Сохраняем настройки...
            Properties.Settings.Default.Save();

            // Показываем сообщение...
            MessageBox.Show(AppStrings.Opts_Saved, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // Закрываем форму...
            Close();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Отмена".
        /// </summary>
        private void MO_Cancel_Click(object sender, EventArgs e)
        {
            // Закрываем форму без сохранения изменений...
            Close();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку поиска приложения
        /// для открытия текстовых файлов.
        /// </summary>
        private void MO_FindTextEd_Click(object sender, EventArgs e)
        {
            if (MO_SearchBin.ShowDialog() == DialogResult.OK)
            {
                MO_TextEdBin.Text = MO_SearchBin.FileName;
            }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку поиска шелла.
        /// </summary>
        private void MO_FindShBin_Click(object sender, EventArgs e)
        {
            if (MO_SearchBin.ShowDialog() == DialogResult.OK)
            {
                MO_ShBin.Text = MO_SearchBin.FileName;
            }
        }

        /// <summary>
        /// Метод, срабатывающий при начале ввода текста в поле кастомного
        /// названия каталога.
        /// </summary>
        private void MO_CustDirName_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Regex.IsMatch(((TextBox)sender).Text, Properties.Resources.MO_CustomDirRegex) ? SystemColors.Window : Color.FromArgb(255, 155, 95);
        }
    }
}
