using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Web;
using System.IO;
using System.Reflection;

namespace srcrepair
{
    public partial class frmBugReporter : Form
    {
        private string BResult;

        public frmBugReporter()
        {
            InitializeComponent();
        }

        private string GetAppSmVersion()
        {
            Version AppV = Assembly.GetEntryAssembly().GetName().Version;
            return String.Format("{0}.{1}", AppV.Major, AppV.Minor);
        }

        private string GenerateOSVersion()
        {
            return String.Format("{0}.{1}", Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor);
        }

        private string GenerateCategory(int Index)
        {
            string Result = "";
            switch (Index)
            {
                case 1: Result = "Feature request";
                    break;
                case 2: Result = "Task";
                    break;
                default: Result = "Bug";
                    break;
            }
            return Result;
        }
        
        private string GeneratePOSTRequest(string Title, int Category, string OS, string Contents)
        {
            return String.Format("title={0}&category={1}&version={2}&platform={3}&os={4}&os_version={5}&contents={6}", Title, GenerateCategory(Category), GetAppSmVersion(), OS, OS, GenerateOSVersion(), Contents);
        }

        private void frmBugReporter_Load(object sender, EventArgs e)
        {
            // Выберем категорию "Ошибка" по умолчанию...
            try { BR_Category.SelectedIndex = 0; } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void BR_Send_Click(object sender, EventArgs e)
        {
            // Проверим заполнены ли обязательные поля...
            if (!(String.IsNullOrWhiteSpace(BR_Title.Text)) && !(String.IsNullOrWhiteSpace(BR_Message.Text)))
            {
                // Изменяем текст кнопки и отключаем её...
                BR_Send.Text = CoreLib.GetLocalizedString("BR_SendButtonAlt");
                
                // Отключаем часть контролов...
                BR_Title.ReadOnly = true;
                BR_Message.ReadOnly = true;
                BR_Send.Enabled = false;

                // Запускаем обработчик асинхронно...
                if (!BR_WrkMf.IsBusy) { BR_WrkMf.RunWorkerAsync(); }
            }
            else
            {
                // Выводим сообщение об ошибке...
                MessageBox.Show(CoreLib.GetLocalizedString("BR_MsgFieldsEmpty"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BR_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BR_WrkMf_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Описываем всевозможные буферные переменные...
                byte[] ByteReqC;
                string BTitle = "", BText = "";
                int BType = 0;

                // Формируем Web-запрос...
                HttpWebRequest WrQ = (HttpWebRequest)WebRequest.Create(Properties.Resources.AppBtURL);

                // Задаём User-Agent, метод запроса и таймаут ожидания ответа...
                WrQ.UserAgent = GV.UserAgent;
                WrQ.Method = "POST";
                WrQ.Timeout = 250000;

                // Заполняем значениями, получаемыми из основного инстанса...
                this.Invoke((MethodInvoker)delegate()
                {
                    BTitle = BR_Title.Text;
                    BText = BR_Message.Text;
                    BType = BR_Category.SelectedIndex;
                });

                // Кодируем POST-запрос в UTF8...
                ByteReqC = Encoding.UTF8.GetBytes(GeneratePOSTRequest(BTitle, BType, Properties.Resources.PlatformFriendlyName, BText));

                // Указываем тип отправляемых данных [форма] и длину запроса...
                WrQ.ContentType = "application/x-www-form-urlencoded";
                WrQ.ContentLength = ByteReqC.Length;

                // Открываем поток...
                using (Stream HTTPStreamRq = WrQ.GetRequestStream())
                {
                    HTTPStreamRq.Write(ByteReqC, 0, ByteReqC.Length);
                    HTTPStreamRq.Close();
                }

                // Получаем ответ от сервера...
                HttpWebResponse HTTPWResp = (HttpWebResponse)WrQ.GetResponse();

                // Разбираем ответ сервера...
                if (HTTPWResp.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream RespStream = HTTPWResp.GetResponseStream())
                    {
                        using (StreamReader StrRead = new StreamReader(RespStream))
                        {
                            BResult = StrRead.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void BR_WrkMf_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Выводим сообщение...
            if (e.Error == null) { if (String.Compare(this.BResult, "OK") != 0) { MessageBox.Show(CoreLib.GetLocalizedString("BR_SendCompleted"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information); } else { MessageBox.Show(CoreLib.GetLocalizedString("BR_SendError"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); } } else { CoreLib.WriteStringToLog(e.Error.Message); }

            // Закрываем форму...
            this.Close();
        }

        private void frmBugReporter_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = ((e.CloseReason == CloseReason.UserClosing) && BR_WrkMf.IsBusy);
        }
    }
}
