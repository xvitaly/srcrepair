/*
 * Модуль распаковки файлов из архива с индикатором прогресса.
 * 
 * Copyright 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2017 EasyCoding Team.
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
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Ionic.Zip;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля распаковки файлов из архивов.
    /// </summary>
    public partial class FrmArchWrk : Form
    {
        /// <summary>
        /// Хранит статус выполнения процесса распаковки.
        /// </summary>
        private bool IsRunning { get; set; } = true;

        /// <summary>
        /// Хранит имя архива, который мы распаковываем.
        /// </summary>
        private string ArchName { get; set; }

        /// <summary>
        /// Хранит каталог назначения.
        /// </summary>
        private string DestDir { get; set; }

        /// <summary>
        /// Конструктор класса формы модуля распаковки файлов из архивов.
        /// </summary>
        /// <param name="A">Архив для распаковки</param>
        /// <param name="D">Каталог назначения</param>
        public FrmArchWrk(string A, string D)
        {
            InitializeComponent();
            ArchName = A;
            DestDir = D;
        }

        /// <summary>
        /// Метод, срабатывающий при возникновении события "загрузка формы".
        /// </summary>
        private void FrmArchWrk_Load(object sender, EventArgs e)
        {
            // Начинаем процесс распаковки асинхронно...
            if (!AR_Wrk.IsBusy) { AR_Wrk.RunWorkerAsync(); }
        }

        /// <summary>
        /// Метод, работающий в отдельном потоке при запуске механизма распаковки.
        /// </summary>
        private void AR_Wrk_DoWork(object sender, DoWorkEventArgs e)
        {
            // Начинаем процесс распаковки с выводом индикатора прогресса...
            if (File.Exists(ArchName))
            {
                using (ZipFile Zip = ZipFile.Read(ArchName))
                {
                    // Формируем счётчики...
                    int TotalFiles = Zip.Count;
                    int i = 1, j = 0;
                    
                    // Начинаем распаковку архива...
                    foreach (ZipEntry ZFile in Zip)
                    {
                        try { ZFile.Extract(DestDir, ExtractExistingFileAction.OverwriteSilently); j = (int)Math.Round(((double)i / (double)TotalFiles * (double)100.00), 0); i++; if ((j >= 0) && (j <= 100)) { AR_Wrk.ReportProgress(j); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("Archive not found.", ArchName);
            }
        }

        /// <summary>
        /// Метод, информирующий основную форму о прогрессе распаковки файлов, который
        /// выполняется в отдельном потоке.
        /// </summary>
        private void AR_Wrk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Отображаем прогресс распаковки архива в баре...
            AR_PrgBr.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Метод, срабатывающий по окончании работы механизма распаковки в отдельном потоке.
        /// </summary>
        private void AR_Wrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Работа завершена. Закроем форму...
            IsRunning = false;
            if (e.Error != null) { CoreLib.HandleExceptionEx(AppStrings.AR_UnpackException, Properties.Resources.AppName, e.Error.Message, e.Error.Source, MessageBoxIcon.Warning); }
            Close();
        }

        /// <summary>
        /// Метод, срабатывающий при попытке закрытия формы.
        /// </summary>
        private void FrmArchWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Блокируем возможность закрытия формы при работающем процессе...
            e.Cancel = IsRunning;
        }
    }
}
