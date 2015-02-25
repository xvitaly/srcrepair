using System;
using System.ComponentModel;
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

        private void frmDnWrk_Load(object sender, EventArgs e)
        {
            // Начинаем процесс загрузки в отдельном потоке...
            this.DownloaderStart(RemoteURI, LocalFile);
        }

        private void DownloaderProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Отрисовываем статус в прогресс-баре...
            try { DN_PrgBr.Value = e.ProgressPercentage; } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void DownloaderCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Загрузка завершена. Закроем форму...
            this.Close();
        }

        private void DownloaderStart(string URI, string FileName)
        {
            try
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
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }
    }
}
