using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace srcrepair
{
    public partial class frmDnWrk : Form
    {
        private string RemoteURI;
        private string LocalFile;

        public frmDnWrk(string R, string L)
        {
            InitializeComponent();
            RemoteURI = R;
            LocalFile = L;
        }

        private void DownloaderCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //
        }

        private void DownloaderProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Отрисовываем статус в прогресс-баре...
            try { DN_PrgBr.Value = e.ProgressPercentage; } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void DownloaderStart(string URI, string FileName)
        {
            // Проверим существование файла и удалим...
            if (File.Exists(FileName)) { File.Delete(FileName); }

            // Начинаем асинхронную загрузку файла...
            using (WebClient FileDownloader = new WebClient())
            {
                FileDownloader.Headers.Add("User-Agent", Properties.Resources.AppDnlUA);
                FileDownloader.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloaderCompleted);
                FileDownloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloaderProgressChanged);
                FileDownloader.DownloadFileAsync(new Uri(URI), FileName);
            }
        }

        private void frmDnWrk_Load(object sender, EventArgs e)
        {
            if (!DN_Wrk.IsBusy) { DN_Wrk.RunWorkerAsync(); }
        }

        private void frmDnWrk_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason == CloseReason.UserClosing) && DN_Wrk.IsBusy;
        }

        private void DN_Wrk_DoWork(object sender, DoWorkEventArgs e)
        {
            //
        }

        private void DN_Wrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //
        }
        
    }
}
