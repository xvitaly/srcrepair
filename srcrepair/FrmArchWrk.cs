/*
 * Модуль распаковки файлов из архива с индикатором прогресса.
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

namespace srcrepair
{
    public partial class FrmArchWrk : Form
    {
        private bool IsRunning = true;
        private string ArchName;
        private string DestDir;
        
        public FrmArchWrk(string A, string D)
        {
            InitializeComponent();
            ArchName = A;
            DestDir = D;
        }

        private void FrmArchWrk_Load(object sender, EventArgs e)
        {
            // Начинаем процесс распаковки асинхронно...
            if (!AR_Wrk.IsBusy) { AR_Wrk.RunWorkerAsync(); }
        }

        private void AR_Wrk_DoWork(object sender, DoWorkEventArgs e)
        {
            //
        }

        private void AR_Wrk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //
        }

        private void AR_Wrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Работа завершена. Закроем форму...
            this.Close();
        }

        private void FrmArchWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = IsRunning;
        }
    }
}
