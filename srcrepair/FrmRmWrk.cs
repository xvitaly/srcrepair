using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace srcrepair
{
    public partial class FrmRmWrk : Form
    {
        private bool IsRunning = true;
        private List<String> RemDirs;

        public FrmRmWrk(List<String> SL)
        {
            InitializeComponent();
            RemDirs = SL;
        }

        private List<String> DetectFilesForCleanup(List<String> CleanDirs)
        {
            List<String> Result = new List<String>();

            foreach (string DirMs in CleanDirs)
            {
                if (Directory.Exists(DirMs))
                {
                    try { DirectoryInfo DInfo = new DirectoryInfo(DirMs); FileInfo[] DirList = DInfo.GetFiles("*.*"); foreach (FileInfo DItem in DirList) { Result.Add(DItem.FullName); } try { List<String> SubDirs = new List<string>(); foreach (DirectoryInfo Dir in DInfo.GetDirectories()) { SubDirs.Add(Path.Combine(Dir.FullName)); } if (SubDirs.Count > 0) { Result.AddRange(DetectFilesForCleanup(SubDirs)); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                }
            }

            return Result;
        }

        private void FrmRmWrk_Load(object sender, EventArgs e)
        {
            // Запускаем удаление асинхронно...
            if (!RW_Wrk.IsBusy) { RW_Wrk.RunWorkerAsync(); }
        }

        private void RW_Wrk_DoWork(object sender, DoWorkEventArgs e)
        {
            // Создаём список файлов для удаления...
            List<string> DeleteQueue = DetectFilesForCleanup(RemDirs);

            // Формируем счётчики...
            int TotalFiles = DeleteQueue.Count;
            int i = 1, j = 0;

            // Удаляем файлы из очереди очистки...
            foreach (string Fl in DeleteQueue)
            {
                try { j = (int)Math.Round(((double)i / (double)TotalFiles * (double)100.00), 0); i++; if ((j >= 0) && (j <= 100)) { RW_Wrk.ReportProgress(j); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                try { if (File.Exists(Fl)) { File.SetAttributes(Fl, FileAttributes.Normal); File.Delete(Fl); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
            }

            // Удаляем пустые каталоги...
            foreach (string Dir in RemDirs) { CoreLib.RemoveEmptyDirectories(Path.GetDirectoryName(Dir)); }

        }

        private void RW_Wrk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Отображаем прогресс удаления файлов...
            RW_PrgBr.Value = e.ProgressPercentage;
        }

        private void RW_Wrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Удаление завершено. Закроем форму...
            IsRunning = false;
            if (e.Error != null) { CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("RW_RmException"), Properties.Resources.AppName, e.Error.Message, e.Error.Source, MessageBoxIcon.Warning); }
            Close();
        }

        private void FrmRmWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Блокируем возможность закрытия формы при работающем процессе...
            e.Cancel = IsRunning;
        }
    }
}
