/*
 * Модуль просмотра журналов SRC Repair.
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
using System.Windows.Forms;
using System.IO;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля просмотра журналов.
    /// </summary>
    public partial class frmLogView : Form
    {
        private string LogFileName;
        public frmLogView(string LogFile)
        {
            InitializeComponent();
            LogFileName = LogFile;
        }

        private void LoadTextFile(string FileName)
        {
            LV_LogArea.Clear();
            LV_LogArea.AppendText(File.ReadAllText(FileName));
        }

        private void LoadLog(string FileName)
        {
            try { LoadTextFile(FileName); } catch (Exception Ex) { CoreLib.HandleExceptionEx(AppStrings.LV_LoadFailed, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }

        private void frmLogView_Load(object sender, EventArgs e)
        {
            // Считаем содержимое выбранного файла...
            LoadLog(LogFileName);
        }

        private void LV_MenuFileReload_Click(object sender, EventArgs e)
        {
            // Перечитаем содержимое журнала...
            LoadLog(LogFileName);
        }

        private void LV_MenuFileExit_Click(object sender, EventArgs e)
        {
            // Закроем модуль...
            Close();
        }

        private void LV_MenuHelpAbout_Click(object sender, EventArgs e)
        {
            // Выводим сообщение с краткой информацией о плагине...
            MessageBox.Show(String.Format(AppStrings.AppPluginAboutDlg, Text, CurrentApp.AppCompany), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LV_MunuFileClearLog_Click(object sender, EventArgs e)
        {
            // Очистим форму...
            LV_LogArea.Clear();

            // Очистим файл журнала...
            try { if (File.Exists(LogFileName)) { File.Delete(LogFileName); CoreLib.CreateFile(LogFileName); } } catch (Exception Ex) { CoreLib.HandleExceptionEx(AppStrings.LV_ClearEx, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }
    }
}
