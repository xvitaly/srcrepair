/*
 * Модуль просмотра журналов SRC Repair.
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
using System.Windows.Forms;
using System.IO;

namespace srcrepair
{
    public partial class frmLogView : Form
    {
        private string LogFileName;
        public frmLogView(string LogFile)
        {
            InitializeComponent();
            LogFileName = LogFile;
        }

        private void frmLogView_Load(object sender, EventArgs e)
        {
            // Считаем содержимое выбранного файла...
            try { LV_LogArea.AppendText(File.ReadAllText(LogFileName)); } catch (Exception Ex) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("LV_LoadFailed"), Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }

        private void LV_MenuFileOpen_Click(object sender, EventArgs e)
        {
            //
        }

        private void LV_MenuFileReload_Click(object sender, EventArgs e)
        {
            //
        }

        private void LV_MenuFileExit_Click(object sender, EventArgs e)
        {
            //
        }

        private void LV_MenuEditCut_Click(object sender, EventArgs e)
        {
            //
        }

        private void LV_MenuEditCopy_Click(object sender, EventArgs e)
        {
            //
        }

        private void LV_MenuEditPaste_Click(object sender, EventArgs e)
        {
            //
        }

        private void LV_MenuHelpAbout_Click(object sender, EventArgs e)
        {
            //
        }
    }
}
