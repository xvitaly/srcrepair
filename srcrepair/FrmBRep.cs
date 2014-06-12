/*
 * Модуль отправки отчётов об ошибках в программе.
 * 
 * Copyright 2011 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2014 EasyCoding Team.
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
using System.Net;
using System.Web;
using System.IO;
using System.Reflection;
using System.Drawing.Drawing2D;

namespace srcrepair
{
    public partial class frmBugReporter : Form
    {
        private string BResult;
        private string CaptchaKey;

        public frmBugReporter()
        {
            InitializeComponent();
        }

        private string GenerateCaptchaKey(int StrLng)
        {
            string Result = "";
            Random Rnd = new Random();
            string SymbolsAvailable = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            for (int i = StrLng; i > 0; i--)
            {
                Result += SymbolsAvailable[Rnd.Next(1, SymbolsAvailable.Length)];
            }
            return Result;
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
            return String.Format("title={0}&category={1}&version={2}&platform={3}&os={4}&os_version={5}&contents={6}", Title, GenerateCategory(Category), GetAppSmVersion(), CoreLib.GetSystemArch(), OS, GenerateOSVersion(), Contents);
        }

        private Bitmap GenerateCaptchaImage(string CaptchaKey, int Width = 0, int Height = 0)
        {
            // Создаём объект изображения...
            Bitmap CI = new Bitmap(Width, Height);

            // Подключаем генератор псевдослучайных чисел...
            Random Rnd = new Random();

            // Создаём объект и задаём его основные свойства...
            Graphics Graph = Graphics.FromImage(CI);
            Graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // Начинаем рисовать...
            RectangleF CaptRecField = new RectangleF(0, 0, Width, Height);
            Graph.FillRectangle(new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White), CaptRecField);

            // Задаём параметры текста будущей капчи...
            StringFormat CptFormat = new StringFormat();
            CptFormat.Alignment = StringAlignment.Center;
            CptFormat.LineAlignment = StringAlignment.Center;

            // Выводим строку...
            GraphicsPath CaptGraphPath = new GraphicsPath();
            CaptGraphPath.AddString(CaptchaKey, FontFamily.GenericSansSerif, 1, 22, CaptRecField, CptFormat);

            // Заполняем мусором...
            PointF[] CaptPoints = { new PointF((float)Rnd.Next(Width) / 4, (float)Rnd.Next(Height) / 4), new PointF(Width - (float)Rnd.Next(Width) / 4, (float)Rnd.Next(Height) / 4), new PointF((float)Rnd.Next(Width) / 4, Height - (float)Rnd.Next(Height) / 4), new PointF(Width - (float)Rnd.Next(Width) / 4, Height - (float)Rnd.Next(Height) / 4) };
            CaptGraphPath.Warp(CaptPoints, CaptRecField, new Matrix(), WarpMode.Perspective, 0);
            Graph.FillPath(new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray), CaptGraphPath);
            int CaptMaxDim = Math.Max(Width, Height);
            for (int i = 0; i <= (int)Width * Height / 30; i++) { Graph.FillEllipse(new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray), Rnd.Next(Width), Rnd.Next(Height), (int)Rnd.Next(CaptMaxDim) / 50, (int)Rnd.Next(CaptMaxDim) / 50); }
            for (int i = 1; i <= 5; i++) { Graph.DrawLine(Pens.DarkGray, Rnd.Next(Width), Rnd.Next(Height), Rnd.Next(Width), Rnd.Next(Height)); }
            for (int i = 1; i <= 5; i++) { Graph.DrawLine(Pens.LightGray, Rnd.Next(Width), Rnd.Next(Height), Rnd.Next(Width), Rnd.Next(Height)); }

            // Уничтожаем ненужные более объекты...
            CaptGraphPath.Dispose();
            Graph.Dispose();

            // Возвращаем сгенерированное изображение...
            return CI;
        }

        private void GenerateCaptcha()
        {
            // Соберём новую капчу...
            if (!BR_CaptGen.IsBusy)
            {
                // Генерируем код капчи...
                this.CaptchaKey = GenerateCaptchaKey(BR_CaptCheck.MaxLength);

                // Запустим генерацию самой капчи в отдельном потоке...
                BR_CaptGen.RunWorkerAsync();
            }
        }

        private void frmBugReporter_Load(object sender, EventArgs e)
        {
            // Вызовем функцию построения капчи...
            this.GenerateCaptcha();

            // Выберем категорию "Ошибка" по умолчанию...
            try { BR_Category.SelectedIndex = 0; } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void BR_Send_Click(object sender, EventArgs e)
        {
            // Проверим заполнены ли обязательные поля...
            if (!(String.IsNullOrWhiteSpace(BR_Title.Text)) && !(String.IsNullOrWhiteSpace(BR_Message.Text)))
            {
                // Проверим капчу...
                if (BR_CaptCheck.Text == this.CaptchaKey)
                {
                    // Изменяем текст кнопки и отключаем её...
                    BR_Send.Text = CoreLib.GetLocalizedString("BR_SendButtonAlt");

                    // Отключаем часть контролов...
                    BR_Title.ReadOnly = true;
                    BR_Message.ReadOnly = true;
                    BR_CaptCheck.ReadOnly = true;
                    BR_Send.Enabled = false;

                    // Запускаем обработчик асинхронно...
                    if (!BR_WrkMf.IsBusy) { BR_WrkMf.RunWorkerAsync(); }
                }
                else
                {
                    // Выводим сообщение об ошибке при заполнении капчи...
                    MessageBox.Show(CoreLib.GetLocalizedString("BR_CaptErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Выводим сообщение об ошибке...
                MessageBox.Show(CoreLib.GetLocalizedString("BR_MsgFieldsEmpty"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BR_Cancel_Click(object sender, EventArgs e)
        {
            // Закрываем форму если обработчик не используется...
            if (!BR_WrkMf.IsBusy) { this.Close(); }
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

        private void BR_CaptGen_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Выводим результат...
                this.Invoke((MethodInvoker)delegate()
                {
                    BR_CaptImg.Image = GenerateCaptchaImage(this.CaptchaKey, BR_CaptImg.Width, BR_CaptImg.Height);
                });
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        private void BR_CaptImg_Click(object sender, EventArgs e)
        {
            // Обновим капчу...
            this.GenerateCaptcha();

            // Очистим поле ввода...
            BR_CaptCheck.Text = "";
        }
    }
}
