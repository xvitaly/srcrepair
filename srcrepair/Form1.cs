using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing; // аналогично Forms...
using System.Linq;
using System.Text;
using System.Windows.Forms; // для работы с формами...
using System.IO; // для работы с файлами...
using System.Diagnostics; // для управления процессами...
using Microsoft.Win32; // для работы с реестром...
using System.Text.RegularExpressions; // для работы с регулярными выражениями...
using System.Reflection; // для управления сборками...
using System.Security.Principal; // для определения прав админа...
using System.Threading; // для управления потоками...
using System.Globalization; // для управления локализациями...
using System.Resources; // для управления ресурсами...

namespace srcrepair
{
    public partial class frmMainW : Form
    {
        public static ResourceManager RM; // Описываем менеджер ресурсов...
        public frmMainW()
        {
            // Инициализация...
            InitializeComponent();
            // Создаём экземпляр менеджера ресурсов с нужным нам ресурсом...
            RM = new ResourceManager("srcrepair.AppStrings", typeof(frmMainW).Assembly);
        }

        #region Internal Variables

        /* В этой переменной будем хранить имя открытого конфига для служебных целей. */
        private string CFGFileName;

        #endregion

        #region Internal Functions

        /*
         * Реализуем аналог полезной дельфийской фукнции IncludeTrailingPathDelimiter,
         * которая возвращает строку, добавив на конец обратный слэш если его нет,
         * либо возвращает ту же строку, если обратный слэш уже присутствует.
         */
        public static string IncludeTrDelim(string SourceStr)
        {
            // Проверяем наличие закрывающего слэша у строки, переданной как параметр...
            if (SourceStr[SourceStr.Length - 1] != '\\')
            {
                // Закрывающего слэша не найдено, поэтому добавим его...
                SourceStr += Path.DirectorySeparatorChar.ToString();
            }

            // Возвращаем результат...
            return SourceStr;
        }

        /*
         * Эта функция получает из реестра и возвращает путь к установленному
         * клиенту Steam.
         */
        private static string GetSteamPath()
        {
            // Подключаем реестр и открываем ключ только для чтения...
            RegistryKey ResKey = Registry.LocalMachine.OpenSubKey(@"Software\Valve\Steam", false);

            // Создаём строку для хранения результатов...
            string ResString = "";

            // Проверяем чтобы ключ реестр существовал и был доступен...
            if (ResKey != null)
            {
                // Получаем значение открытого ключа...
                ResString = (string)ResKey.GetValue("InstallPath");

                // Проверяем чтобы значение существовало...
                if (String.IsNullOrEmpty(ResString))
                {
                    // Значение не существует, поэтому сгенерируем исключение для обработки в основном коде...
                    throw new System.NullReferenceException("Exception: No InstallPath value detected! Please run Steam.");
                }
            }

            // Закрываем открытый ранее ключ реестра...
            ResKey.Close();

            // Возвращаем результат...
            return ResString;
        }

        /*
         * Эта функция возвращает PID процесса если он был найден в памяти и
         * завершает, либо 0 если процесс не был найден.
         */
        private static int ProcessTerminate(string ProcessName, bool ShowConfirmationMsg)
        {
            // Обнуляем PID...
            int ProcID = 0;

            // Фильтруем список процессов по заданной маске в параметрах и вставляем в массив...
            Process[] LocalByName = Process.GetProcessesByName(ProcessName);

            // Запускаем цикл по поиску и завершению процессов...
            foreach (Process ResName in LocalByName)
            {
                if (ShowConfirmationMsg) // Проверим, нужно ли подтверждение...
                {
                    // Запросим подтверждение...
                    DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("ST_KillMessage"), ResName.ProcessName), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (UserConfirmation == DialogResult.Yes)
                    {
                        ProcID = ResName.Id;
                        ResName.Kill();
                    }
                }
                else
                {
                    // Подтверждение не требуется, завершаем процесс...
                    ProcID = ResName.Id; // Сохраняем PID процесса...
                    ResName.Kill(); // Завершаем процесс...
                }
            }

            // Возвращаем PID как результат функции...
            return ProcID;
        }

        /*
         * Эта функция очищает блобы (файлы с расширением *.blob) из каталога Steam.
         * В качестве параметра ей передаётся полный путь к каталогу Steam.
         */
        private static void CleanBlobsNow(string SteamPath)
        {
            // Инициализируем буферную переменную, в которой будем хранить имя файла...
            string FileName;

            // Генерируем имя первого кандидата на удаление с полным путём до него...
            FileName = SteamPath + "AppUpdateStats.blob";

            // Проверяем существует ли данный файл...
            if (File.Exists(FileName))
            {
                // Удаляем...
                File.Delete(FileName);
            }

            // Аналогично генерируем имя второго кандидата...
            FileName = SteamPath + "ClientRegistry.blob";

            // Проверяем, существует ли файл...
            if (File.Exists(FileName))
            {
                // Удаляем...
                File.Delete(FileName);
            }
        }

        /*
         * Эта функция удаляет значения реестра, отвечающие за настройки клиента
         * Steam, а также записывает значение языка.
         */
        private static void CleanRegistryNow(int LangCode)
        {
            // Удаляем ключ HKEY_LOCAL_MACHINE\Software\Valve рекурсивно...
            Registry.LocalMachine.DeleteSubKeyTree(@"Software\Valve", false);

            // Удаляем ключ HKEY_CURRENT_USER\Software\Valve рекурсивно...
            Registry.CurrentUser.DeleteSubKeyTree(@"Software\Valve", false);

            // Начинаем вставлять значение языка клиента Steam...
            // Инициализируем буферную переменную для хранения названия языка...
            string XLang;

            // Генерируем...
            switch (LangCode)
            {
                case 0:
                    XLang = "english";
                    break;
                case 1:
                    XLang = "russian";
                    break;
                default:
                    XLang = "english";
                    break;
            }

            // Подключаем реестр и создаём ключ HKEY_CURRENT_USER\Software\Valve\Steam...
            RegistryKey RegLangKey = Registry.CurrentUser.CreateSubKey(@"Software\Valve\Steam");

            // Если не было ошибок, записываем значение...
            if (RegLangKey != null)
            {
                // Записываем значение в реестр...
                RegLangKey.SetValue("language", XLang);
            }

            // Закрываем ключ...
            RegLangKey.Close();
        }

        /*
         * Эта функция получает из реестра значение нужной нам переменной
         * для указанного игрового приложения.
         */
        private static int GetSRCDWord(string CVar, string CApp)
        {
            // Подключаем реестр и открываем ключ только для чтения...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Source\" + CApp + @"\Settings", false);

            // Создаём переменную для хранения результатов...
            int ResInt = -1;

            // Проверяем чтобы ключ реестр существовал и был доступен...
            if (ResKey != null)
            {
                // Получаем значение открытого ключа...
                ResInt = (int)ResKey.GetValue(CVar);
            }

            // Закрываем открытый ранее ключ реестра...
            ResKey.Close();

            // Возвращаем результат...
            return ResInt;
        }

        /*
         * Эта процедура записывает в реестр новое значение нужной нам переменной
         * для указанного игрового приложения.
         */
        private static void WriteSRCDWord(string CVar, int CValue, string CApp)
        {
            // Подключаем реестр и открываем ключ для чтения и записи...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Source\" + CApp + @"\Settings", true);

            // Записываем в реестр...
            ResKey.SetValue(CVar, CValue, RegistryValueKind.DWord);

            // Закрываем открытый ранее ключ реестра...
            ResKey.Close();
        }

        /*
         * Эта функция удаляет нужный ключ из ветки HKEY_LOCAL_MACHINE...
         */
        private static void RemoveRegistryKeyLM(string RKey)
        {
            // Удаляем запрощенную ветку рекурсивно из HKLM...
            Registry.LocalMachine.DeleteSubKeyTree(RKey, false);
        }

        /*
         * Эта функция удаляет нужный ключ из ветки HKEY_CURRENT_USER...
         */
        private static void RemoveRegistryKeyCU(string RKey)
        {
            // Удаляем запрощенную ветку рекурсивно из HKCU...
            Registry.CurrentUser.DeleteSubKeyTree(RKey, false);
        }

        /*
         * Эта функция считывает содержимое текстового файла в строку и
         * возвращает в качестве результата.
         */
        private static string ReadTextFileNow(string FileName)
        {
            // Считываем всё содержимое файла...
            string TextFile = File.ReadAllText(FileName);
            // Возвращаем результат...
            return TextFile;
        }

        /*
         * Эта функция проверяет есть ли у пользователя, с правами которого запускается
         * программа, привилегии локального администратора.
         */
        private static bool IsCurrentUserAdmin()
        {
            bool Result; // Переменная для хранения результата...
            try
            {
                // Получаем сведения...
                WindowsPrincipal UP = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                // Проверяем, состоит ли пользователь в группе администраторов...
                Result = UP.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                // Произошло исключение. Пользователь не администратор...
                Result = false;
            }
            // Возвращает результат...
            return Result;
        }

        /*
         * Эта функция ищет указанную строку в массиве строк и возвращает её индекс,
         * либо -1 если такой строки в массиве не найдено.
         */
        private static int FindStringInStrArray(string[] SourceStr, string What)
        {
            int StrNum;
            int StrIndex = -1;
            for (StrNum = 0; StrNum < SourceStr.Length; StrNum++)
            {
                if (SourceStr[StrNum] == What)
                {
                    StrIndex = StrNum;
                }
            }
            return StrIndex;
        }

        /*
         * Эта функция ищет в массиве строк нужный нам параметр командной строки
         * и возвращает true если параметр был найден, либо false если нет.
         */
        private static bool FindCommandLineSwitch(string[] Source, string CLineArg)
        {
            if (FindStringInStrArray(Source, CLineArg) != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
         * Эта функция устанавливает требуемый FPS-конфиг...
         */
        private static void InstallConfigNow(string ConfName)
        {
            // Устанавливаем...
            File.Copy(GV.FullAppPath + @"cfgs\" + GV.SmallAppName + @"\" + ConfName, GV.FullCfgPath + "autoexec.cfg", true);
        }

        /*
         * Эта функция генерирует ДДММГГЧЧММСС из указанного времени в строку.
         * Применяется для служебных целей.
         */
        public static string WriteDateToString(DateTime XDate, bool MicroDate)
        {
            if (MicroDate)
            {
                // Возвращаем строку с результатом (краткой датой)...
                return XDate.Day.ToString() + XDate.Month.ToString() + XDate.Year.ToString() + XDate.Hour.ToString() + XDate.Minute.ToString() + XDate.Second.ToString();
            }
            else
            {
                // Возвращаем строку с результатом (датой по ГОСТу)...
                return XDate.Day.ToString() + "." + XDate.Month.ToString() + "." + XDate.Year.ToString() + " " + XDate.Hour.ToString() + ":" + XDate.Minute.ToString() + ":" + XDate.Second.ToString();
            }
        }

        /*
         * Эта функция создаёт резервную копию конфига, имя которого передано
         * в параметре.
         */
        private static void CreateBackUpNow(string ConfName)
        {
            // Сначала проверим, существует ли запрошенный файл...
            if (File.Exists(GV.FullCfgPath + ConfName))
            {
                // Проверяем чтобы каталог для бэкапов существовал...
                if (!(Directory.Exists(GV.FullBackUpDirPath)))
                {
                    // Каталоги не существуют. Создадим общий каталог для резервных копий...
                    Directory.CreateDirectory(GV.FullBackUpDirPath);
                }

                try
                {
                    // Копируем оригинальный файл в файл бэкапа...
                    File.Copy(GV.FullCfgPath + ConfName, GV.FullBackUpDirPath + ConfName + "." + WriteDateToString(DateTime.Now, true), true);
                }
                catch
                {
                    // Произошло исключение. Уведомим пользователя об этом...
                    MessageBox.Show(RM.GetString("BackUpCreationFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /*
         * Эта функция проверяет наличие не-ASCII-символов в строке. Возвращает True
         * если не обнаружено запрещённых симолов и False - если они были обнаружены.
         */
        private static bool CheckNonASCII(string Path)
        {
            // Проверяем строку на соответствие регулярному выражению...
            //return Regex.IsMatch(Path, "");
            bool Result = true; // переменная для промежуточного результата...
            for (int i = 1; i < Path.Length; i++) // запускаем цикл...
            {
                // Проверяем, соответствует ли символ шаблону допустимых символов...
                if (!(Regex.IsMatch(Path[i].ToString(), "[0-9a-zA-Z :()\\\\]")))
                {
                    // Не соответствует, следовательно найден недопустимый.
                    // Вернём False и прекратим цикл, т.к. дальнейшая проверка бессмысленна...
                    Result = false;
                    break;
                }
            }
            // Возвращаем результат функции...
            return Result;
        }

        /*
         * Эта функция запускает указанное в параметре SAppName приложение на
         * выполнение с параметрами, указанными в SParameters и ждёт его завершения...
         */
        public static void StartProcessAndWait(string SAppName, string SParameters)
        {
            // Запускаем процесс...
            Process NewProcess = Process.Start(SAppName, SParameters);
            // Ждём завершения процесса...
            while (!(NewProcess.HasExited))
            {
                // Заставляем приложение "заснуть"...
                Thread.Sleep(600);
            }
        }

        /*
         * Эта функция удаляет файлы в заданной папке по указанной в параметре
         * CleanupMask маске.
         */
        private static void CleanDirectoryNow(string DirPath, string CleanupMask)
        {
            // Открываем каталог...
            DirectoryInfo DInfo = new DirectoryInfo(DirPath);
            // Считываем список файлов по заданной маске...
            FileInfo[] DirList = DInfo.GetFiles(CleanupMask);
            // Начинаем обход массива...
            foreach (FileInfo DItem in DirList)
            {
                // Обрабатываем найденное...
                DItem.Delete(); // Удаляем файл...
            }
        }

        // Источник данной функции: http://www.csharp-examples.net/inputbox/ //
        private static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = RM.GetString("InputBoxCancelBtnName");
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
        
        /* Эта функция сохраняет содержимое таблицы в файл конфигурации, указанный в
         * параметре. Используется в Save и SaveAs Редактора конфигов. */
        private void WriteTableToFileNow(string Path)
        {
            // Начинаем сохранять содержимое редактора в файл...
            using (StreamWriter CFile = new StreamWriter(Path))
            {
                CFile.WriteLine("// Generated by " + GV.AppName + " //"); // Вставляем сообщение ;-).
                CFile.WriteLine(); // вставляем пустую строку
                for (int i = 0; i < CE_Editor.Rows.Count; i++) // запускаем цикл
                {
                    CFile.Write(CE_Editor.Rows[i].Cells[0].Value); // вставляем содержимое первого столбца (название переменной)
                    CFile.Write(" "); // вставляем пробел
                    CFile.WriteLine(CE_Editor.Rows[i].Cells[1].Value); // вставляем содержимое второго столбца (значение переменной)
                }
                CFile.Close(); // закрываем файл
            }
        }

        /*
         * Эта функция используется для записи значений в таблицу Редактора конфигов.
         * Используется делегатом. Прямой вызов не допускается.
         */
        private void AddRowToTable(string Cv, string Cn)
        {
            CE_Editor.Rows.Add(Cv, Cn);
        }

        /*
         * Эта функция используется для создания резервной копии выбранной ветки
         * реестра в переданный в параметре файл.
         */
        private static void CreateRegBackUpNow(string RKey, string FileName)
        {
            DialogResult UserConfirmation = MessageBox.Show(RM.GetString("BU_RegCreate"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                // Генерируем строку с параметрами...
                string Params = @"/ea """ + GV.FullBackUpDirPath + FileName + "_" + WriteDateToString(DateTime.Now, true) + @".reg""" + " " + RKey;
                // Запускаем и ждём завершения...
                StartProcessAndWait("regedit.exe", Params);
            }
        }

        /*
         * Эта функция возвращает описание переданной в качестве параметра
         * переменной, получая эту информацию из ресурса CVList с учётом
         * локализации.
         */
        private static string GetCVDescription(string CVar)
        {
            ResourceManager DM = new ResourceManager("srcrepair.CVList", typeof(frmMainW).Assembly);
            return DM.GetString(CVar);
        }

        #endregion

        private void frmMainW_Load(object sender, EventArgs e)
        {
            // Событие инициализации формы...

            // Получаем параметры командной строки, переданные приложению...
            string[] CMDLineArgs = Environment.GetCommandLineArgs();
            
            // Получаем путь к каталогу приложения...
            Assembly Assmbl = Assembly.GetEntryAssembly();
            GV.FullAppPath = IncludeTrDelim(Path.GetDirectoryName(Assmbl.Location));

            // Проверяем, запущена ли программа с правами администратора...
            if (!(IsCurrentUserAdmin()))
            {
                // Программа запущена с правами пользователя, поэтому принимаем меры...
                // Выводим сообщение об этом...
                MessageBox.Show(RM.GetString("AppLaunchedNotAdmin"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Блокируем контролы, требующие для своей работы прав админа...
                PS_CleanRegistry.Enabled = false;
                PS_SteamLang.Enabled = false;
            }
            
            // Получаем информацию о версии нашего приложения...
            GV.AppVersionInfo = Assmbl.GetName().Version.ToString();

            // Вставляем информацию о версии в заголовок формы...
            //this.Text += " (version " + GV.AppVersionInfo + ")";
            this.Text = String.Format(this.Text, GV.AppVersionInfo);

            // Найдём и завершим в памяти процесс Steam...
            if (ProcessTerminate("Steam", true) != 0)
            {
                MessageBox.Show(RM.GetString("PS_ProcessTerminated"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Ищем параметр командной строки path...
            if (FindCommandLineSwitch(CMDLineArgs, "/path"))
            {
                // Параметр найден, считываем следующий за ним параметр с путём к Steam...
                GV.FullSteamPath = IncludeTrDelim(CMDLineArgs[FindStringInStrArray(CMDLineArgs, "/path") + 1]);
            }
            else
            {
                // Параметр не найден, поэтому получим из реестра...
                try
                {
                    // Получаем из реестра...
                    GV.FullSteamPath = IncludeTrDelim(GetSteamPath());
                }
                catch
                {
                    // Произошло исключение, пользователю придётся ввести путь самостоятельно!
                    MessageBox.Show(RM.GetString("SteamPathNotDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    string Buf = "";
                    if (InputBox(RM.GetString("SteamPathEnterTitle"), RM.GetString("SteamPathEnterText"), ref Buf) == DialogResult.OK)
                    {
                        Buf = IncludeTrDelim(Buf);
                        if (!(File.Exists(Buf + "Steam.exe")))
                        {
                            MessageBox.Show(RM.GetString("SteamPathEnterErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                        else
                        {
                            GV.FullSteamPath = Buf;
                        }
                    }
                    else
                    {
                        MessageBox.Show(RM.GetString("SteamPathCancel"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
            }

            // Ищем параметр командной строки login...
            if (FindCommandLineSwitch(CMDLineArgs, "/login"))
            {
                // Параметр найден, добавим его и отключим автоопределение...
                try
                {
                    // Добавляем параметр в ComboBox...
                    LoginSel.Items.Add((string)CMDLineArgs[FindStringInStrArray(CMDLineArgs, "/login") + 1]);
                }
                catch
                {
                    // Произошло исключение, поэтому просто выдадим сообщение...
                    MessageBox.Show(RM.GetString("ParamError"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Параметр не найден, будем использовать автоопределение...
                // Создаём объект DirInfo...
                DirectoryInfo DInfo = new DirectoryInfo(GV.FullSteamPath + @"steamapps\");
                // Получаем список директорий из текущего...
                DirectoryInfo[] DirList = DInfo.GetDirectories();
                // Обходим созданный массив в поиске нужных нам логинов...
                foreach (DirectoryInfo DItem in DirList)
                {
                    // Фильтруем известные каталоги...
                    if ((DItem.Name != "common") && (DItem.Name != "sourcemods") && (DItem.Name != "media"))
                    {
                        // Добавляем найденный логин в список ComboBox...
                        LoginSel.Items.Add((string)DItem.Name);
                    }
                }
            }

            // Проверим, были ли логины добавлены в список...
            if (LoginSel.Items.Count == 0)
            {
                // Логинов не было найдено, поэтому запросим у пользователя...
                // Описываем буферную переменную...
                string SBuf = "";
                
                // Запускаем цикл с постусловием...
                do
                {
                    // Отображаем InputBox, а также анализируем ввод пользователя...
                    if ((InputBox(RM.GetString("SteamLoginEnterTitle"), RM.GetString("SteamLoginEnterText"), ref SBuf) == DialogResult.Cancel) || (SBuf == ""))
                    {
                        // Пользователь нажал Cancel, либо ввёл пустую строку, поэтому
                        // выводим сообщение и завершаем работу приложения...
                        MessageBox.Show(RM.GetString("SteamLoginCancel"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Application.Exit();
                    };
                } while (!(Directory.Exists(GV.FullSteamPath + @"steamapps\" + SBuf + @"\")));
                
                // Добавляем полученный логин в список...
                LoginSel.Items.Add((string)SBuf);
            }

            // Укажем путь к Steam на странице "Устранение проблем"...
            PS_RSteamPath.Text = GV.FullSteamPath;
            
            // Проверим на наличие запрещённых символов в пути к установленному клиенту Steam...
            if (!(CheckNonASCII(GV.FullSteamPath)))
            {
                // Запрещённые символы найдены!
                PS_PathDetector.Text = RM.GetString("SteamNonASCIITitle");
                PS_PathDetector.ForeColor = Color.Red;
                PS_WarningMsg.Text = RM.GetString("SteamNonASCIISmall");
                PS_WarningMsg.ForeColor = Color.Red;
                MessageBox.Show(RM.GetString("SteamNonASCIIDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Выводим сообщение в строку статуса...
            SB_Status.Text = RM.GetString("StatusSLogin");
        }

        private void PS_CleanBlobs_CheckedChanged(object sender, EventArgs e)
        {
            if (PS_CleanBlobs.Checked || PS_CleanRegistry.Checked)
            {
                PS_ExecuteNow.Enabled = true;
            }
            else
            {
                PS_ExecuteNow.Enabled = false;
            }
        }

        private void PS_CleanRegistry_CheckedChanged(object sender, EventArgs e)
        {
            PS_SteamLang.Enabled = PS_CleanRegistry.Checked;

            if (PS_CleanRegistry.Checked || PS_CleanBlobs.Checked)
            {
                PS_ExecuteNow.Enabled = true;
            }
            else
            {
                PS_ExecuteNow.Enabled = false;
            }
        }

        private void PS_ExecuteNow_Click(object sender, EventArgs e)
        {
            // Начинаем очистку...

            // Запрашиваем подтверждение у пользователя...
            DialogResult UserConfirmation = MessageBox.Show(RM.GetString("PS_ExecuteMSG"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                // Подтверждение получено...
                if ((PS_CleanBlobs.Checked) || (PS_CleanRegistry.Checked))
                {
                    // Найдём и завершим работу клиента Steam...
                    if (ProcessTerminate("Steam", false) != 0)
                    {
                        MessageBox.Show(RM.GetString("PS_ProcessDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Проверяем нужно ли чистить блобы...
                    if (PS_CleanBlobs.Checked)
                    {
                        // Чистим блобы...
                        CleanBlobsNow(GV.FullSteamPath);
                    }

                    // Проверяем нужно ли чистить реестр...
                    if (PS_CleanRegistry.Checked)
                    {
                        // Проверяем выбрал ли пользователь язык из выпадающего списка...
                        if (PS_SteamLang.SelectedIndex != -1)
                        {
                            // Всё в порядке, чистим реестр...
                            CleanRegistryNow(PS_SteamLang.SelectedIndex);
                        }
                        else
                        {
                            // Пользователь не выбрал язык, поэтому будем использовать английский...
                            MessageBox.Show(RM.GetString("PS_NoLangSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CleanRegistryNow(0);
                        }
                    }

                    // Выполнение закончено, выведем сообщение о завершении...
                    MessageBox.Show(RM.GetString("PS_SeqCompleted"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Завершаем работу приложения...
                    Application.Exit();
                }
            }
        }

        private void PS_AllowRemCtrls_CheckedChanged(object sender, EventArgs e)
        {
            PS_RemCustMaps.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemDnlCache.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemOldSpray.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemOldCfgs.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemGraphCache.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemSoundCache.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemNavFiles.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemScreenShots.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemDemos.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemGraphOpts.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemOldBin.Enabled = PS_AllowRemCtrls.Checked;
        }

        private void frmMainW_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Проверим, делал ли что-то пользователь с формой. Если не делал - не будем
            // спрашивать и завершим форму автоматически...
            if (AppSelector.Enabled)
            {
                // Создаём MessageBox...
                DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("FrmCloseQuery"), GV.AppName), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                // Запрашиваем подтверждение у пользователя на закрытие формы...
                if (UserConfirmation == DialogResult.Yes)
                {
                    // Подтверждение получено, закрываем форму...
                    e.Cancel = false;
                }
                else
                {
                    // Пользователь передумал, отменяем закрытие формы...
                    e.Cancel = true;
                }
            }
        }

        private void AppSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Создаём буферную переменную для дальнейшего использования...
            string ptha = "";

            /* Значение локальной переменной ptha:
             * LoginSel.Text - для GCF-приложний (логин Steam);
             * common - для NCF-приложений.
             */
            
            // Начинаем определять нужные нам значения переменных...
            switch (AppSelector.SelectedIndex)
            {
                case 0: // Team Fortress 2
                    GV.FullAppName = "team fortress 2"; // имя каталога...
                    GV.SmallAppName = "tf"; // имя индивидуального подкаталога...
                    ptha = LoginSel.Text; // это GCF-приложение...
                    GV.IsGCFApp = true;
                    break;
                case 1: // Counter-Strike: Source
                    GV.FullAppName = "counter-strike source";
                    GV.SmallAppName = "cstrike";
                    ptha = LoginSel.Text;
                    GV.IsGCFApp = true;
                    break;
                case 2: // Garry's Mod
                    GV.FullAppName = "garrysmod";
                    GV.SmallAppName = "garrysmod";
                    ptha = LoginSel.Text;
                    GV.IsGCFApp = true;
                    break;
                case 3: // Day of Defeat: Source
                    GV.FullAppName = "day of defeat source";
                    GV.SmallAppName = "dod";
                    ptha = LoginSel.Text;
                    GV.IsGCFApp = true;
                    break;
                /*case 4: // Left 4 Dead 1
                    GV.FullAppName = "left 4 dead"; // имя каталога...
                    GV.SmallAppName = "left4dead"; // имя индивидуального подкаталога...
                    ptha = "common"; // это NCF-приложение...
                    GV.IsGCFApp = false;
                    break;
                case 5: // Left 4 Dead 2
                    GV.FullAppName = "left 4 dead 2";
                    GV.SmallAppName = "left4dead2";
                    ptha = "common";
                    GV.IsGCFApp = false;
                    break;
                case 6: // Alien Swarm
                    GV.FullAppName = "alien swarm";
                    GV.SmallAppName = "swarm";
                    ptha = "common";
                    GV.IsGCFApp = false;
                    break;*/
                default: // Ничего не выбрано
                    AppSelector.SelectedIndex = -1;
                    break;
            }

            // Если выбор верный, включаем контрол выбора логина Steam...
            if (AppSelector.SelectedIndex != -1)
            {
                LoginSel.Enabled = true;
            }
            else
            {
                LoginSel.Enabled = false;
            }

            // Генерируем полный путь до каталога управляемого приложения...
            GV.GamePath = IncludeTrDelim(GV.FullSteamPath + @"steamapps\" + ptha + @"\" + GV.FullAppName);
            GV.FullGamePath = IncludeTrDelim(GV.GamePath + GV.SmallAppName);

            // Заполняем другие служебные переменные...
            GV.FullCfgPath = GV.FullGamePath + @"cfg\";
            GV.FullBackUpDirPath = GV.FullAppPath + @"backups\" + GV.SmallAppName + @"\";
            
            // Включаем основные элементы управления (контролы)...
            MainTabControl.Enabled = true;

            if (GV.IsGCFApp)
            {
                // Включим модули очистки...
                PS_AllowRemCtrls.Enabled = true;
                PS_ResetSettings.Enabled = true;

                // Начинаем заполнять таблицу...

                // Получаем значение разрешения по горизонтали
                try
                {
                    GT_ResHor.Value = GetSRCDWord("ScreenWidth", GV.SmallAppName);
                }
                catch
                {
                    GT_ResHor.Value = 800;
                }

                // Получаем значение разрешения по вертикали
                try
                {
                    GT_ResVert.Value = GetSRCDWord("ScreenHeight", GV.SmallAppName);
                }
                catch
                {
                    GT_ResVert.Value = 600;
                }

                // Получаем режим окна (ScreenWindowed): 1-window, 0-fullscreen
                try
                {
                    GT_ScreenType.SelectedIndex = GetSRCDWord("ScreenWindowed", GV.SmallAppName);
                }
                catch
                {
                    GT_ScreenType.SelectedIndex = -1;
                }

                // Получаем детализацию моделей (r_rootlod): 0-high, 1-med, 2-low
                try
                {
                    switch (GetSRCDWord("r_rootlod", GV.SmallAppName))
                    {
                        case 0: GT_ModelQuality.SelectedIndex = 2;
                            break;
                        case 1: GT_ModelQuality.SelectedIndex = 1;
                            break;
                        case 2: GT_ModelQuality.SelectedIndex = 0;
                            break;
                    }
                }
                catch
                {
                    GT_ModelQuality.SelectedIndex = -1;
                }

                // Получаем детализацию текстур (mat_picmip): 0-high, 1-med, 2-low
                try
                {
                    switch (GetSRCDWord("mat_picmip", GV.SmallAppName))
                    {
                        case 0: GT_TextureQuality.SelectedIndex = 2;
                            break;
                        case 1: GT_TextureQuality.SelectedIndex = 1;
                            break;
                        case 2: GT_TextureQuality.SelectedIndex = 0;
                            break;
                    }
                }
                catch
                {
                    GT_TextureQuality.SelectedIndex = -1;
                }

                // Получаем настройки шейдеров (mat_reducefillrate): 0-high, 1-low
                try
                {
                    switch (GetSRCDWord("mat_reducefillrate", GV.SmallAppName))
                    {
                        case 0: GT_ShaderQuality.SelectedIndex = 1;
                            break;
                        case 1: GT_ShaderQuality.SelectedIndex = 0;
                            break;
                    }
                }
                catch
                {
                    GT_ShaderQuality.SelectedIndex = -1;
                }

                // Начинаем работать над отражениями (здесь сложнее)
                try
                {
                    switch (GetSRCDWord("r_waterforceexpensive", GV.SmallAppName))
                    {
                        case 0: GT_WaterQuality.SelectedIndex = 0;
                            break;
                        case 1:
                            switch (GetSRCDWord("r_waterforcereflectentities", GV.SmallAppName))
                            {
                                case 0: GT_WaterQuality.SelectedIndex = 1;
                                    break;
                                case 1: GT_WaterQuality.SelectedIndex = 2;
                                    break;
                            }
                            break;
                    }
                }
                catch
                {
                    GT_WaterQuality.SelectedIndex = -1;
                }

                // Получаем настройки теней (r_shadowrendertotexture): 0-low, 1-high
                try
                {
                    switch (GetSRCDWord("r_shadowrendertotexture", GV.SmallAppName))
                    {
                        case 0: GT_ShadowQuality.SelectedIndex = 0;
                            break;
                        case 1: GT_ShadowQuality.SelectedIndex = 1;
                            break;
                    }
                }
                catch
                {
                    GT_ShadowQuality.SelectedIndex = -1;
                }

                // Получаем настройки коррекции цвета (mat_colorcorrection): 0-off, 1-on
                try
                {
                    switch (GetSRCDWord("mat_colorcorrection", GV.SmallAppName))
                    {
                        case 0: GT_ColorCorrectionT.SelectedIndex = 0;
                            break;
                        case 1: GT_ColorCorrectionT.SelectedIndex = 1;
                            break;
                    }
                }
                catch
                {
                    GT_ColorCorrectionT.SelectedIndex = -1;
                }

                // Получаем настройки сглаживания (mat_antialias): 1-off, 2-2x, 4-4x, etc
                // 2x MSAA - 2:0; 4xMSAA - 4:0; 8xCSAA - 4:2; 16xCSAA - 4:4; 8xMSAA - 8:0; 16xQ CSAA - 8:2;
                try
                {
                    switch (GetSRCDWord("mat_antialias", GV.SmallAppName))
                    {
                        case 0: GT_AntiAliasing.SelectedIndex = 0;
                            break;
                        case 1: GT_AntiAliasing.SelectedIndex = 0;
                            break;
                        case 2: GT_AntiAliasing.SelectedIndex = 1;
                            break;
                        case 4:
                            switch (GetSRCDWord("mat_aaquality", GV.SmallAppName))
                            {
                                case 0: GT_AntiAliasing.SelectedIndex = 2;
                                    break;
                                case 2: GT_AntiAliasing.SelectedIndex = 3;
                                    break;
                                case 4: GT_AntiAliasing.SelectedIndex = 4;
                                    break;
                            }
                            break;
                        case 8:
                            switch (GetSRCDWord("mat_aaquality", GV.SmallAppName))
                            {
                                case 0: GT_AntiAliasing.SelectedIndex = 5;
                                    break;
                                case 2: GT_AntiAliasing.SelectedIndex = 6;
                                    break;
                            }
                            break;
                    }
                }
                catch
                {
                    GT_AntiAliasing.SelectedIndex = -1;
                }

                // Получаем настройки анизотропии (mat_forceaniso): 1-off, etc
                try
                {
                    switch (GetSRCDWord("mat_forceaniso", GV.SmallAppName))
                    {
                        case 1:
                            switch (GetSRCDWord("mat_trilinear", GV.SmallAppName))
                            {
                                case 0: GT_Filtering.SelectedIndex = 0;
                                    break;
                                case 1: GT_Filtering.SelectedIndex = 1;
                                    break;
                            }
                            break;
                        case 2: GT_Filtering.SelectedIndex = 2;
                            break;
                        case 4: GT_Filtering.SelectedIndex = 3;
                            break;
                        case 8: GT_Filtering.SelectedIndex = 4;
                            break;
                        case 16: GT_Filtering.SelectedIndex = 5;
                            break;
                    }
                }
                catch
                {
                    GT_Filtering.SelectedIndex = -1;
                }

                // Получаем настройки вертикальной синхронизации (mat_vsync): 0-off, 1-on
                try
                {
                    switch (GetSRCDWord("mat_vsync", GV.SmallAppName))
                    {
                        case 0: GT_VSync.SelectedIndex = 0;
                            break;
                        case 1: GT_VSync.SelectedIndex = 1;
                            break;
                    }
                }
                catch
                {
                    GT_VSync.SelectedIndex = -1;
                }

                // Получаем настройки размытия движения (MotionBlur): 0-off, 1-on
                try
                {
                    switch (GetSRCDWord("MotionBlur", GV.SmallAppName))
                    {
                        case 0: GT_MotionBlur.SelectedIndex = 0;
                            break;
                        case 1: GT_MotionBlur.SelectedIndex = 1;
                            break;
                    }
                }
                catch
                {
                    GT_MotionBlur.SelectedIndex = -1;
                }

                // Получаем настройки режима рендера (DXLevel_V1):
                // 80-DirectX 8.0; 81-DirectX 8.1; 90-DirectX 9.0; 95-DirectX 9.0c
                try
                {
                    switch (GetSRCDWord("DXLevel_V1", GV.SmallAppName))
                    {
                        case 80: GT_DxMode.SelectedIndex = 0;
                            break;
                        case 81: GT_DxMode.SelectedIndex = 1;
                            break;
                        case 90: GT_DxMode.SelectedIndex = 2;
                            break;
                        case 95: GT_DxMode.SelectedIndex = 3;
                            break;
                    }
                }
                catch
                {
                    GT_DxMode.SelectedIndex = -1;
                }

                // Получаем настройки HDR (mat_hdr_level): 0-off,1-bloom,2-Full
                try
                {
                    switch (GetSRCDWord("mat_hdr_level", GV.SmallAppName))
                    {
                        case 0: GT_HDR.SelectedIndex = 0;
                            break;
                        case 1: GT_HDR.SelectedIndex = 1;
                            break;
                        case 2: GT_HDR.SelectedIndex = 2;
                            break;
                    }
                }
                catch
                {
                    GT_HDR.SelectedIndex = -1;
                }
            }
            else
            {
                // Отключим модули очистки...
                PS_AllowRemCtrls.Enabled = false;
                PS_ResetSettings.Enabled = false;
                
                // Приложение NCF, поэтому настройки хранятся не в реестре, а в
                // файле video.txt. Будем парсить...
                
                // TODO: реализовать парсинг файла video.txt и заполнить таблицу из него...
            }

            // Проверим, установлен ли FPS-конфиг...
            if (File.Exists(GV.FullCfgPath + "autoexec.cfg"))
            {
                GT_Warning.Visible = true;
            }
            else
            {
                GT_Warning.Visible = false;
            }
            
            // Очистим список FPS-конфигов...
            FP_ConfigSel.Items.Clear();

            // Считаем имеющиеся FPS-конфиги...
            try
            {
                // Открываем каталог...
                DirectoryInfo DInfo = new DirectoryInfo(GV.FullAppPath + @"cfgs\" + GV.SmallAppName + @"\");
                // Считываем список файлов по заданной маске...
                FileInfo[] DirList = DInfo.GetFiles("*.cfg");
                // Начинаем обход массива...
                foreach (FileInfo DItem in DirList)
                {
                    // Обрабатываем найденное...
                    if (DItem.Name != "config_default.cfg")
                    {
                        FP_ConfigSel.Items.Add((string)DItem.Name);
                    }
                }
                // Проверяем, нашлись ли конфиги...
                if (FP_ConfigSel.Items.Count >= 1)
                {
                    FP_Description.Text = RM.GetString("FP_SelectFromList");
                    FP_Description.ForeColor = Color.Black;
                    FP_Install.Enabled = true;
                    FP_ConfigSel.Enabled = true;
                }
            }
            catch
            {
                // FPS-конфигов для выбранного приложения не найдено.
                // Выводим текст об этом...
                FP_Description.Text = RM.GetString("FP_NoCfgGame");
                FP_Description.ForeColor = Color.Red;
                // ...и блокируем контролы, отвечающие за установку...
                FP_Install.Enabled = false;
                FP_ConfigSel.Enabled = false;
            }

            // Включаем заблокированные ранее контролы...
            MNUFPSWizard.Enabled = true;
            MNUInstaller.Enabled = true;
            
            // Выводим сообщение о завершении считывания в статус-бар...
            SB_Status.Text = RM.GetString("StatusNormal");
            SB_App.Text = AppSelector.SelectedItem.ToString();
        }

        private void LoginSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Логин выбран, включаем контролы...
            AppSelector.Enabled = true;
            if (AppSelector.SelectedIndex == -1)
            {
                SB_Status.Text = RM.GetString("StatusSApp");
            }
            else
            {
                SB_Status.Text = RM.GetString("StatusLoginChanged");
            }

            // Выводим логин на страницу "Устранение проблем"...
            PS_RSteamLogin.Text = LoginSel.Text;
        }

        private void GT_Maximum_Graphics_Click(object sender, EventArgs e)
        {
            // Нажатие этой кнопки устанавливает графические настройки на рекомендуемый максимум...
            // Зададим вопрос, а нужно ли это юзеру?
            DialogResult UserConfirmation = MessageBox.Show(RM.GetString("GT_MaxPerfMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                // Пользователь согласился, продолжаем...
                GT_ScreenType.SelectedIndex = 0; // полноэкранный режим
                GT_ModelQuality.SelectedIndex = 2; // высокая детализация моделей
                GT_TextureQuality.SelectedIndex = 2; // высокая детализация текстур
                GT_ShaderQuality.SelectedIndex = 1; // высокое качество шейдерных эффектов
                GT_WaterQuality.SelectedIndex = 1; // отражать мир в воде
                GT_WaterQuality.SelectedIndex = 1; // высокое качество теней
                GT_ColorCorrectionT.SelectedIndex = 1; // корренкция цвета включена
                GT_AntiAliasing.SelectedIndex = 0; // сглаживание выключено
                GT_Filtering.SelectedIndex = 3; // анизотропная фильтрация 4x
                GT_VSync.SelectedIndex = 0; // вертикальная синхронизация выключена
                GT_MotionBlur.SelectedIndex = 0; // размытие движения выключено
                GT_DxMode.SelectedIndex = 3; // режим DirecX 9.0c
                GT_HDR.SelectedIndex = 2; // HDR полные
                MessageBox.Show(RM.GetString("GT_PerfSet"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GT_Maximum_Performance_Click(object sender, EventArgs e)
        {
            // Нажатие этой кнопки устанавливает графические настройки на рекомендуемый минимум...
            // Спросим пользователя.
            DialogResult UserConfirmation = MessageBox.Show(RM.GetString("GT_MinPerfMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                // Пользователь согласился, продолжаем...
                GT_ScreenType.SelectedIndex = 0; // полноэкранный режим
                GT_ModelQuality.SelectedIndex = 0; // низкая детализация моделей
                GT_TextureQuality.SelectedIndex = 0; // низкая детализация текстур
                GT_ShaderQuality.SelectedIndex = 0; // низкое качество шейдерных эффектов
                GT_WaterQuality.SelectedIndex = 0; // простые отражения в воде
                GT_WaterQuality.SelectedIndex = 0; // низкое качество теней
                GT_ColorCorrectionT.SelectedIndex = 0; // корренкция цвета выключена
                GT_AntiAliasing.SelectedIndex = 0; // сглаживание выключено
                GT_Filtering.SelectedIndex = 1; // трилинейная фильтрация текстур
                GT_VSync.SelectedIndex = 0; // вертикальная синхронизация выключена
                GT_MotionBlur.SelectedIndex = 0; // размытие движения выключено
                // Спросим у пользователя о режиме DirectX...
                UserConfirmation = MessageBox.Show(RM.GetString("GT_DxLevelMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (UserConfirmation == DialogResult.Yes)
                {
                    GT_DxMode.SelectedIndex = 0; // режим DirecX 8.0
                }
                else
                {
                    GT_DxMode.SelectedIndex = 3; // режим DirecX 9.0c
                }
                GT_HDR.SelectedIndex = 0; // эффекты HDR выключены
                MessageBox.Show(RM.GetString("GT_PerfSet"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GT_SaveApply_Click(object sender, EventArgs e)
        {
            // Сохраняем изменения в графических настройках...
            // Запрашиваем подтверждение у пользователя...
            DialogResult UserConfirmation = MessageBox.Show(RM.GetString("GT_SaveMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                if (GV.IsGCFApp)
                {
                    // Создаём резервную копию...
                    try
                    {
                        CreateRegBackUpNow(@"HKEY_CURRENT_USER\Software\Valve\Source\" + GV.SmallAppName + @"\Settings", "Game_AutoBackUp");
                    }
                    catch
                    {
                        // Подавляем сообщение об ошибке если оно возникнет...
                    }
                    
                    // Запишем в реестр настройки разрешения экрана...
                    // По горизонтали (ScreenWidth):
                    WriteSRCDWord("ScreenWidth", (int)GT_ResHor.Value, GV.SmallAppName);

                    // По вертикали (ScreenHeight):
                    WriteSRCDWord("ScreenWidth", (int)GT_ResVert.Value, GV.SmallAppName);

                    // Запишем в реестр настройки режима запуска приложения (ScreenWindowed):
                    switch (GT_ScreenType.SelectedIndex)
                    {
                        case 0: WriteSRCDWord("ScreenWindowed", 0, GV.SmallAppName);
                            break;
                        case 1: WriteSRCDWord("ScreenWindowed", 1, GV.SmallAppName);
                            break;
                    }

                    // Запишем в реестр настройки детализации моделей (r_rootlod):
                    switch (GT_ModelQuality.SelectedIndex)
                    {
                        case 0: WriteSRCDWord("r_rootlod", 2, GV.SmallAppName);
                            break;
                        case 1: WriteSRCDWord("r_rootlod", 1, GV.SmallAppName);
                            break;
                        case 2: WriteSRCDWord("r_rootlod", 0, GV.SmallAppName);
                            break;
                    }

                    // Запишем в реестр настройки детализации текстур (mat_picmip):
                    switch (GT_TextureQuality.SelectedIndex)
                    {
                        case 0: WriteSRCDWord("mat_picmip", 2, GV.SmallAppName);
                            break;
                        case 1: WriteSRCDWord("mat_picmip", 1, GV.SmallAppName);
                            break;
                        case 2: WriteSRCDWord("mat_picmip", 0, GV.SmallAppName);
                            break;
                    }

                    // Запишем в реестр настройки качества шейдерных эффектов (mat_reducefillrate):
                    switch (GT_ShaderQuality.SelectedIndex)
                    {
                        case 0: WriteSRCDWord("mat_reducefillrate", 1, GV.SmallAppName);
                            break;
                        case 1: WriteSRCDWord("mat_reducefillrate", 0, GV.SmallAppName);
                            break;
                    }

                    // Запишем в реестр настройки отражений в воде (r_waterforceexpensive и r_waterforcereflectentities):
                    switch (GT_WaterQuality.SelectedIndex)
                    {
                        case 0:
                            // Simple reflections
                            WriteSRCDWord("r_waterforceexpensive", 0, GV.SmallAppName);
                            WriteSRCDWord("r_waterforcereflectentities", 0, GV.SmallAppName);
                            break;
                        case 1:
                            // Reflect world
                            WriteSRCDWord("r_waterforceexpensive", 1, GV.SmallAppName);
                            WriteSRCDWord("r_waterforcereflectentities", 0, GV.SmallAppName);
                            break;
                        case 2:
                            // Reflect all
                            WriteSRCDWord("r_waterforceexpensive", 1, GV.SmallAppName);
                            WriteSRCDWord("r_waterforcereflectentities", 1, GV.SmallAppName);
                            break;
                    }

                    // Запишем в реестр настройки прорисовки теней (r_shadowrendertotexture):
                    switch (GT_ShadowQuality.SelectedIndex)
                    {
                        case 0: WriteSRCDWord("r_shadowrendertotexture", 0, GV.SmallAppName);
                            break;
                        case 1: WriteSRCDWord("r_shadowrendertotexture", 1, GV.SmallAppName);
                            break;
                    }

                    // Запишем в реестр настройки коррекции цвета (mat_colorcorrection):
                    switch (GT_ColorCorrectionT.SelectedIndex)
                    {
                        case 0: WriteSRCDWord("mat_colorcorrection", 0, GV.SmallAppName);
                            break;
                        case 1: WriteSRCDWord("mat_colorcorrection", 1, GV.SmallAppName);
                            break;
                    }

                    // Запишем в реестр настройки сглаживания (mat_antialias и mat_aaquality):
                    switch (GT_AntiAliasing.SelectedIndex)
                    {
                        case 0:
                            // Нет сглаживания
                            WriteSRCDWord("mat_antialias", 1, GV.SmallAppName);
                            WriteSRCDWord("mat_aaquality", 0, GV.SmallAppName);
                            WriteSRCDWord("ScreenMSAA", 0, GV.SmallAppName); // Дублируем значение mat_antialias
                            WriteSRCDWord("ScreenMSAAQuality", 0, GV.SmallAppName); // Дублируем значение mat_aaquality
                            break;
                        case 1:
                            // 2x MSAA
                            WriteSRCDWord("mat_antialias", 2, GV.SmallAppName);
                            WriteSRCDWord("mat_aaquality", 0, GV.SmallAppName);
                            WriteSRCDWord("ScreenMSAA", 2, GV.SmallAppName);
                            WriteSRCDWord("ScreenMSAAQuality", 0, GV.SmallAppName);
                            break;
                        case 2:
                            // 4x MSAA
                            WriteSRCDWord("mat_antialias", 4, GV.SmallAppName);
                            WriteSRCDWord("mat_aaquality", 0, GV.SmallAppName);
                            WriteSRCDWord("ScreenMSAA", 4, GV.SmallAppName);
                            WriteSRCDWord("ScreenMSAAQuality", 0, GV.SmallAppName);
                            break;
                        case 3:
                            // 8x CSAA
                            WriteSRCDWord("mat_antialias", 4, GV.SmallAppName);
                            WriteSRCDWord("mat_aaquality", 2, GV.SmallAppName);
                            WriteSRCDWord("ScreenMSAA", 4, GV.SmallAppName);
                            WriteSRCDWord("ScreenMSAAQuality", 2, GV.SmallAppName);
                            break;
                        case 4:
                            // 16x CSAA
                            WriteSRCDWord("mat_antialias", 4, GV.SmallAppName);
                            WriteSRCDWord("mat_aaquality", 4, GV.SmallAppName);
                            WriteSRCDWord("ScreenMSAA", 4, GV.SmallAppName);
                            WriteSRCDWord("ScreenMSAAQuality", 4, GV.SmallAppName);
                            break;
                        case 5:
                            // 8x MSAA
                            WriteSRCDWord("mat_antialias", 8, GV.SmallAppName);
                            WriteSRCDWord("mat_aaquality", 0, GV.SmallAppName);
                            WriteSRCDWord("ScreenMSAA", 8, GV.SmallAppName);
                            WriteSRCDWord("ScreenMSAAQuality", 0, GV.SmallAppName);
                            break;
                        case 6:
                            // 16xQ CSAA
                            WriteSRCDWord("mat_antialias", 8, GV.SmallAppName);
                            WriteSRCDWord("mat_aaquality", 2, GV.SmallAppName);
                            WriteSRCDWord("ScreenMSAA", 8, GV.SmallAppName);
                            WriteSRCDWord("ScreenMSAAQuality", 2, GV.SmallAppName);
                            break;
                    }

                    // Запишем в реестр настройки фильтрации (mat_forceaniso):
                    switch (GT_Filtering.SelectedIndex)
                    {
                        case 0:
                            // Билинейная
                            WriteSRCDWord("mat_forceaniso", 1, GV.SmallAppName);
                            WriteSRCDWord("mat_trilinear", 0, GV.SmallAppName);
                            break;
                        case 1:
                            // Трилинейная
                            WriteSRCDWord("mat_forceaniso", 1, GV.SmallAppName);
                            WriteSRCDWord("mat_trilinear", 1, GV.SmallAppName);
                            break;
                        case 2:
                            // Анизотропная 2x
                            WriteSRCDWord("mat_forceaniso", 2, GV.SmallAppName);
                            WriteSRCDWord("mat_trilinear", 0, GV.SmallAppName);
                            break;
                        case 3:
                            // Анизотропная 4x
                            WriteSRCDWord("mat_forceaniso", 4, GV.SmallAppName);
                            WriteSRCDWord("mat_trilinear", 0, GV.SmallAppName);
                            break;
                        case 4:
                            // Анизотропная 8x
                            WriteSRCDWord("mat_forceaniso", 8, GV.SmallAppName);
                            WriteSRCDWord("mat_trilinear", 0, GV.SmallAppName);
                            break;
                        case 5:
                            // Анизотропная 16x
                            WriteSRCDWord("mat_forceaniso", 16, GV.SmallAppName);
                            WriteSRCDWord("mat_trilinear", 0, GV.SmallAppName);
                            break;
                    }

                    // Запишем в реестр настройки вертикальной синхронизации (mat_vsync):
                    switch (GT_VSync.SelectedIndex)
                    {
                        case 0: WriteSRCDWord("mat_vsync", 0, GV.SmallAppName);
                            break;
                        case 1: WriteSRCDWord("mat_vsync", 1, GV.SmallAppName);
                            break;
                    }

                    // Запишем в реестр настройки размытия движения (MotionBlur):
                    switch (GT_MotionBlur.SelectedIndex)
                    {
                        case 0: WriteSRCDWord("MotionBlur", 0, GV.SmallAppName);
                            break;
                        case 1: WriteSRCDWord("MotionBlur", 1, GV.SmallAppName);
                            break;
                    }

                    // Запишем в реестр настройки режима DirectX (DXLevel_V1):
                    switch (GT_DxMode.SelectedIndex)
                    {
                        case 0: WriteSRCDWord("DXLevel_V1", 80, GV.SmallAppName); // DirectX 8.0
                            break;
                        case 1: WriteSRCDWord("DXLevel_V1", 81, GV.SmallAppName); // DirectX 8.1
                            break;
                        case 2: WriteSRCDWord("DXLevel_V1", 90, GV.SmallAppName); // DirectX 9.0
                            break;
                        case 3: WriteSRCDWord("DXLevel_V1", 95, GV.SmallAppName); // DirectX 9.0c
                            break;
                    }

                    // Запишем в реестр настройки HDR (mat_hdr_level):
                    switch (GT_HDR.SelectedIndex)
                    {
                        case 0: WriteSRCDWord("mat_hdr_level", 0, GV.SmallAppName);
                            break;
                        case 1: WriteSRCDWord("mat_hdr_level", 1, GV.SmallAppName);
                            break;
                        case 2: WriteSRCDWord("mat_hdr_level", 2, GV.SmallAppName);
                            break;
                    }
                }
                else
                {
                    // Это NCG-приложение, поэтому будем записывать настройки в файл...

                    // TODO: реализовать запись таблицы в файл...
                }

                // Запишем в реестр пользовательскую строку запуска TF2...
                // TODO: реализовать возможность записывать параметры строки запуска...

                // Закончили запись основных настроек, указанных пользователем или выбранных по умолчанию...

                // Выводим подтверждающее сообщение...
                MessageBox.Show(RM.GetString("GT_SaveSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FP_ConfigSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получаем описание выбранного пользователем конфига...
            try
            {
                FP_Description.Text = ReadTextFileNow(GV.FullAppPath + @"cfgs\" + GV.SmallAppName + @"\" + Path.GetFileNameWithoutExtension(FP_ConfigSel.Text) + "_" + RM.GetString("AppLangPrefix") + ".txt");
            }
            catch
            {
                FP_Description.Text = RM.GetString("FP_NoDescr");
            }
        }

        private void FP_Install_Click(object sender, EventArgs e)
        {
            // Начинаем устанавливать FPS-конфиг в управляемое приложение...
            if (FP_ConfigSel.SelectedIndex != -1)
            {
                DialogResult UserConfirmation = MessageBox.Show(RM.GetString("FP_InstallQuestion"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (UserConfirmation == DialogResult.Yes)
                {
                    // Проверим, не нужно ли создавать резервную копию...
                    if (FP_CreateBackUp.Checked)
                    {
                        // Создаём резервную копию...
                        CreateBackUpNow("autoexec.cfg");
                    }

                    try
                    {
                        // Устанавливаем...
                        InstallConfigNow(FP_ConfigSel.Text);
                        // Выводим сообщение об успешной установке...
                        MessageBox.Show(RM.GetString("FP_InstallSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Отобразим значок предупреждения на странице графических настроек...
                        GT_Warning.Visible = true;
                    }
                    catch
                    {
                        // Установка не удалась. Выводим сообщение об этом...
                        MessageBox.Show(RM.GetString("FP_InstallFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                // Пользователь не выбрал конфиг. Сообщим об этом...
                MessageBox.Show(RM.GetString("FP_NothingSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FP_Uninstall_Click(object sender, EventArgs e)
        {
            // Начинаем удаление установленного конфига...
            // Генерируем имя файла с полным путём до него...
            string CfgFile = GV.FullCfgPath + "autoexec.cfg";
            // Проверяем, существует ли файл...
            if (File.Exists(CfgFile))
            {
                // Файл существует. Запросим подтверждение на удаление...
                DialogResult UserConfirmation = MessageBox.Show(RM.GetString("FP_RemoveQuestion"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (UserConfirmation == DialogResult.Yes)
                {
                    // Создадим бэкап если установлен флажок...
                    if (FP_CreateBackUp.Checked)
                    {
                        // Создаём резервную копию...
                        CreateBackUpNow("autoexec.cfg");
                    }

                    try
                    {
                        // Удалим файл...
                        File.Delete(CfgFile);
                        // Выводим сообщение об успешном удалении...
                        MessageBox.Show(RM.GetString("FP_RemoveSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Скрываем значок предупреждения на странице графических настроек...
                        GT_Warning.Visible = false;
                    }
                    catch
                    {
                        // Произошло исключение при попытке удаления. Уведомим пользователя об этом...
                        MessageBox.Show(RM.GetString("FP_RemoveFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show(RM.GetString("FP_RemoveNotExists"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GT_Warning_Click(object sender, EventArgs e)
        {
            // Выдадим сообщение о наличии FPS-конфига...
            MessageBox.Show(RM.GetString("GT_FPSCfgDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void CE_New_Click(object sender, EventArgs e)
        {
            // Создаём новый файл...
            CE_Editor.Rows.Clear();
            CFGFileName = "";
            SB_Status.Text = RM.GetString("StatusOpenedFile") + " " + RM.GetString("UnnamedFileName");
        }

        private void CE_Open_Click(object sender, EventArgs e)
        {
            // Прочитаем конфиг и заполним его содержимым нашу таблицу редактора...
            
            // Указываем стартовый каталог в диалоге открытия файла на каталог с конфигами игры...
            CE_OpenCfgDialog.InitialDirectory = GV.FullCfgPath;

            // Считывает файл конфига и помещает записи в таблицу
            DialogResult OpenResult = CE_OpenCfgDialog.ShowDialog(); // Отображаем стандартный диалог открытия файла...
            if (OpenResult == DialogResult.OK)
            {
                //
                string Buf = CE_OpenCfgDialog.FileName; // Получаем имя файла с полным путём...
                string ImpStr; // Строка для парсинга...
                string CVarName, CVarContent;
                if (File.Exists(Buf)) // Проверяем, существует ли файл...
                {
                    // Файл существует. Продолжаем...
                    CFGFileName = Path.GetFileName(Buf); // Получаем имя открытого в Редакторе файла без пути...
                    if (CFGFileName == "config.cfg") // Проверяем, не открыл ли пользователь файл config.cfg и, если да, то сообщаем об этом...
                    {
                        MessageBox.Show(RM.GetString("CE_RestConfigOpenWarn"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    CE_Editor.Rows.Clear(); // Очищаем область редактирования...
                    try
                    {
                        using (StreamReader OpenedConfig = new StreamReader(@Buf, Encoding.Default)) // Открываем файл таким способом...
                        {
                            // Будем читать поток построчно...
                            while (OpenedConfig.Peek() >= 0)
                            {
                                // Начинаем работу...
                                ImpStr = OpenedConfig.ReadLine(); // считали строку...
                                ImpStr = ImpStr.Trim(); // почистим строку от лишних пробелов...
                                // Начинаем парсить считанную строку...
                                if (!(String.IsNullOrEmpty(ImpStr))) // проверяем, не пустая ли строка...
                                {
                                    if ((ImpStr[1] != '/') && (ImpStr.Substring(0, 4) != "echo")) // проверяем, не комментарий ли или сообщение...
                                    {
                                        // Удалим все лишние пробелы...
                                        while (ImpStr.IndexOf("  ") != -1) // пока остались двойные пробелы, продолжаем...
                                        {
                                            ImpStr = ImpStr.Replace("  ", " "); // удаляем найденный лишний пробел...
                                        }

                                        // Ищем и удаляем символ табуляции из строки...
                                        while (ImpStr.IndexOf("\t") != -1)
                                        {
                                            ImpStr = ImpStr.Replace("\t", "");
                                        }

                                        // Строка почищена, продолжаем...
                                        if (ImpStr.IndexOf(" ") != -1)
                                        {
                                            Buf = ImpStr.Substring(0, ImpStr.IndexOf(" ")); // мы получили переменную...
                                            ImpStr = ImpStr.Remove(0, ImpStr.IndexOf(" ") + 1); // удаляем полученное...
                                            //ImpStr = ImpStr.Replace(Buf, "");
                                            // Buf теперь содержит всё до пробела. Нужно чистить...
                                            if ((Buf.IndexOf("/") == -1) && (Buf.IndexOf(" ") == -1) && (Buf != ""))
                                            {
                                                CVarName = Buf; // заполняем имя переменной...
                                                // Отлично, имя переменной мы получили и храним в CVarName. Осталось получить значение...
                                                if (ImpStr.IndexOf("/") != -1) // ищем в строке комментарии...
                                                {
                                                    Buf = ImpStr.Substring(0, ImpStr.IndexOf("/") - 1); // копируем всё до комментария...
                                                    CVarContent = Buf; // возвращаем значение...
                                                }
                                                else
                                                {
                                                    CVarContent = ImpStr; // комментариев нет, сразу возвращаем значение...
                                                }
                                                // Пишем в нашу таблицу...
                                                CE_Editor.Rows.Add(CVarName, CVarContent);
                                            }
                                        }
                                        else
                                        {
                                            CE_Editor.Rows.Add(ImpStr, "");
                                        }
                                    }
                                }
                            }
                        }
                        SB_Status.Text = RM.GetString("StatusOpenedFile") + " " + CFGFileName;
                    }
                    catch
                    {
                        // Произошло исключение...
                        // Подавляем сообщение об этом. Юзеру не обязательно знать...
                        MessageBox.Show(RM.GetString("CE_ExceptionDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(RM.GetString("CE_OpenFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void CE_Save_Click(object sender, EventArgs e)
        {
            CE_SaveCfgDialog.InitialDirectory = GV.FullCfgPath; // Указываем путь по умолчанию к конфигам управляемого приложения...
            DialogResult UserConfirmation = MessageBox.Show(RM.GetString("CE_CfgSV"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes) // Спрашиваем пользователя о необходимости сохранения файла...
            {
                if (!(String.IsNullOrEmpty(CFGFileName))) // Проверяем, открыт ли какой-либо файл...
                {
                    // Будем бэкапировать только файлы, находящиеся в каталоге /cfg/
                    // управляемоего приложения. Остальные - нет.
                    if (File.Exists(GV.FullCfgPath + CFGFileName))
                    {
                        // Спрашиваем пользователя о создании резервной копии...
                        UserConfirmation = MessageBox.Show(RM.GetString("CE_CreateBackUp"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (UserConfirmation == DialogResult.Yes)
                        {
                            CreateBackUpNow(CFGFileName);
                        }
                    }
                    try
                    {
                        //WriteTableToFileNow(CFGFileName);
                        WriteTableToFileNow(CE_OpenCfgDialog.FileName);
                    }
                    catch
                    {
                        // Произошла ошибка при сохранении файла...
                        MessageBox.Show(RM.GetString("CE_CfgSVVEx"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // Файл не был открыт. Нужно сохранить и дать имя...
                    DialogResult SaveResult = CE_SaveCfgDialog.ShowDialog(); // Отображаем стандартный диалог сохранения файла...
                    if (SaveResult == DialogResult.OK)
                    {
                        WriteTableToFileNow(CE_SaveCfgDialog.FileName);
                        CFGFileName = Path.GetFileName(CE_SaveCfgDialog.FileName);
                        CE_OpenCfgDialog.FileName = CE_SaveCfgDialog.FileName;
                        SB_Status.Text = RM.GetString("StatusOpenedFile") + " " + CFGFileName;
                    }
                }
            }
        }

        private void CE_SaveAs_Click(object sender, EventArgs e)
        {
            CE_SaveCfgDialog.InitialDirectory = GV.FullCfgPath;
            DialogResult SaveResult = CE_SaveCfgDialog.ShowDialog();
            if (SaveResult == DialogResult.OK)
            {
                WriteTableToFileNow(CE_SaveCfgDialog.FileName);
            }
        }

        private void PS_RemCustMaps_Click(object sender, EventArgs e)
        {
            // Удаляем кастомные (нестандартные) карты...
            DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("PS_CleanupExecuteQ"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                try
                {
                    CleanDirectoryNow(GV.FullGamePath + @"maps\", "*.bsp");
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PS_RemDnlCache_Click(object sender, EventArgs e)
        {
            // Удаляем кэш загрузок...
            DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("PS_CleanupExecuteQ"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                try
                {
                    CleanDirectoryNow(GV.FullGamePath + @"downloads\", "*.dat");
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PS_RemOldSpray_Click(object sender, EventArgs e)
        {
            // Удаляем кэш спреев...
            DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("PS_CleanupExecuteQ"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                try
                {
                    CleanDirectoryNow(GV.FullGamePath + @"materials\temp\", "*.vtf");
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PS_RemOldCfgs_Click(object sender, EventArgs e)
        {
            // Удаляем все конфиги...
            DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("PS_CleanupExecuteQ"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                try
                {
                    CleanDirectoryNow(GV.FullGamePath + @"cfg\", "*.*");
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PS_RemGraphCache_Click(object sender, EventArgs e)
        {
            // Удаляем графический кэш...
            DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("PS_CleanupExecuteQ"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                try
                {
                    CleanDirectoryNow(GV.FullGamePath + @"maps\graphs\", "*.*");
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PS_RemSoundCache_Click(object sender, EventArgs e)
        {
            // Удаляем звуковой кэш...
            DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("PS_CleanupExecuteQ"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                try
                {
                    CleanDirectoryNow(GV.FullGamePath + @"maps\soundcache\", "*.*");
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PS_RemNavFiles_Click(object sender, EventArgs e)
        {
            // Удаляем файлы навигации ботов...
            DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("PS_CleanupExecuteQ"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                try
                {
                    CleanDirectoryNow(GV.FullGamePath + @"maps\", "*.nav");
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PS_RemScreenShots_Click(object sender, EventArgs e)
        {
            // Удаляем все скриншоты...
            DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("PS_CleanupExecuteQ"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                try
                {
                    CleanDirectoryNow(GV.FullGamePath + @"screenshots\", "*.*");
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PS_RemDemos_Click(object sender, EventArgs e)
        {
            // Удаляем все записанные демки...
            DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("PS_CleanupExecuteQ"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                try
                {
                    CleanDirectoryNow(GV.FullGamePath, "*.dem");
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PS_RemGraphOpts_Click(object sender, EventArgs e)
        {
            // Удаляем графические настройки...
            DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("PS_CleanupExecuteQ"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                // Создаём резервную копию...
                try
                {
                    CreateRegBackUpNow(@"HKEY_CURRENT_USER\Software\Valve\Source\" + GV.SmallAppName + @"\Settings", "Game_AutoBackUp");
                }
                catch
                {
                    // Подавляем сообщение об ошибке если оно возникнет...
                }

                // Работаем...
                try
                {
                    // Удаляем ключ HKEY_CURRENT_USER\Software\Valve\Source\tf\Settings из реестра...
                    Registry.CurrentUser.DeleteSubKeyTree(@"Software\Valve\Source\" + GV.SmallAppName + @"\Settings", false);
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PS_RemOldBin_Click(object sender, EventArgs e)
        {
            // Удаляем старые бинарники...
            DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("PS_CleanupExecuteQ"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                try
                {
                    Directory.Delete(GV.GamePath + @"bin\", true); // Удаляем бинарники...
                    Directory.Delete(GV.GamePath + @"platform\", true); // Удаляем настройки платформы...
                    Directory.Delete(GV.FullGamePath + @"bin\", true); // Удаляем библиотеки игры...
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void GT_ResHor_Btn_Click(object sender, EventArgs e)
        {
            // Выводим краткую справку о вводе разрешения по горизонтали...
            MessageBox.Show(String.Format(RM.GetString("GT_ResMsg"), RM.GetString("GT_ResMsgHor")), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GT_ResVert_Btn_Click(object sender, EventArgs e)
        {
            // Выводим краткую справку о вводе разрешения по вертикали...
            MessageBox.Show(String.Format(RM.GetString("GT_ResMsg"), RM.GetString("GT_ResMsgVert")), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GT_LaunchOptions_Btn_Click(object sender, EventArgs e)
        {
            // Выводим краткую справку о строке параметров запуска...
            MessageBox.Show(RM.GetString("NotImplementedYet"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PS_ResetSettings_Click(object sender, EventArgs e)
        {
            // Удаляем все настройки...
            DialogResult UserConfirmation = MessageBox.Show(RM.GetString("PS_ResetSettingsMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                // Создаём резервную копию...
                try
                {
                    CreateRegBackUpNow(@"HKEY_CURRENT_USER\Software\Valve\Source\" + GV.SmallAppName + @"\Settings", "Game_AutoBackUp");
                }
                catch
                {
                    // Подавляем сообщение об ошибке если оно возникнет...
                }
                
                // Работаем...
                try
                {
                    Directory.Delete(GV.GamePath, true); // Удаляем всю папку с файлами игры...
                    Registry.CurrentUser.DeleteSubKeyTree(@"Software\Valve\Source\" + GV.SmallAppName + @"\Settings", false); // Удаляем настройки видео...
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void MNUShowEdHint_Click(object sender, EventArgs e)
        {
            // Покажем подсказку...
            CE_ShowHint.PerformClick();
        }

        private void MNUReportBuilder_Click(object sender, EventArgs e)
        {
            // Запускаем форму создания отчёта для Техподдержки...
            frmRepBuilder RBF = new frmRepBuilder();
            RBF.ShowDialog();
        }

        private void MNUInstaller_Click(object sender, EventArgs e)
        {
            // Запускаем форму установщика спреев, демок и конфигов...
            frmInstaller InstF = new frmInstaller();
            InstF.ShowDialog();
        }

        private void MNUExit_Click(object sender, EventArgs e)
        {
            // Завершаем работу программы...
            Close();
        }

        private void MNUFPSWizard_Click(object sender, EventArgs e)
        {
            // Очистим Редактор конфигов...
            CE_New.PerformClick();
            // Запускаем форму мастера FPS-конфигов...
            frmFPGen FPFrm = new frmFPGen(new CFGEdDelegate(AddRowToTable));
            FPFrm.ShowDialog();
            MainTabControl.SelectedIndex = 1;
        }

        private void MNUAbout_Click(object sender, EventArgs e)
        {
            // Отобразим форму "О программе"...
            frmAbout AboutFrm = new frmAbout();
            AboutFrm.ShowDialog();
        }

        private void MNUReportBug_Click(object sender, EventArgs e)
        {
            // Открываем в браузере страницу сообщения об ошибке в багтрекере гуглкода...
            Process.Start("http://code.google.com/p/srcrepair/issues/entry");
        }

        private void BUT_Refresh_Click(object sender, EventArgs e)
        {
            // Считаем список резервных копий и заполним таблицу...
            // Очистим таблицу...
            BU_ListTable.Rows.Clear();
            try
            {
                // Открываем каталог...
                DirectoryInfo DInfo = new DirectoryInfo(GV.FullBackUpDirPath);
                // Считываем список файлов по заданной маске...
                FileInfo[] DirList = DInfo.GetFiles("*.*");
                // Инициализируем буферную переменную...
                string Buf;
                // Начинаем обход массива...
                foreach (FileInfo DItem in DirList)
                {
                    // Обрабатываем найденное...
                    if (DItem.Extension == ".reg") { Buf = "Registry"; } else { Buf = "Config"; }
                    BU_ListTable.Rows.Add(Path.GetFileNameWithoutExtension(DItem.Name), Buf, DItem.Length / 1024, DItem.CreationTime, DItem.Name);
                }
            }
            catch
            {
                // Произошло исключение...
                MessageBox.Show(RM.GetString("BU_ListLdFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BUT_RestoreB_Click(object sender, EventArgs e)
        {
            // Восстановим выделенный бэкап...
            if (BU_ListTable.Rows.Count > 0)
            {
                // Получаем имя файла...
                string FName = BU_ListTable.Rows[BU_ListTable.CurrentRow.Index].Cells[4].Value.ToString();
                // Запрашиваем подтверждение...
                DialogResult UserConfirmation = MessageBox.Show(String.Format(RM.GetString("BU_QMsg"), Path.GetFileNameWithoutExtension(FName), BU_ListTable.Rows[BU_ListTable.CurrentRow.Index].Cells[3].Value.ToString()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (UserConfirmation == DialogResult.Yes)
                {
                    // Проверяем что восстанавливать: конфиг или реестр...
                    if (Path.GetExtension(FName) != ".reg")
                    {
                        // Сохраняем оригинальное имя файла резервной копии для функции копирования...
                        string OrigName = FName;
                        // Отбрасываем двойное расширение...
                        FName = Path.GetFileNameWithoutExtension(FName);
                        try
                        {
                            // Копируем файл...
                            File.Copy(GV.FullBackUpDirPath + OrigName, GV.FullCfgPath + FName, true);
                            // Показываем сообщение об успешном восстановлении...
                            MessageBox.Show(RM.GetString("BU_RestSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch
                        {
                            // Произошло исключение...
                            MessageBox.Show(RM.GetString("BU_RestFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        // Восстанавливаем файл реестра...
                        try
                        {
                            // Восстанавливаем...
                            Process.Start("regedit.exe", @"/s """ + GV.FullBackUpDirPath + FName + @"""");
                            // Показываем сообщение об успешном восстановлении...
                            MessageBox.Show(RM.GetString("BU_RestSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch
                        {
                            // Произошло исключение...
                            MessageBox.Show(RM.GetString("BU_RestFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(RM.GetString("BU_NoFiles"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BUT_DelB_Click(object sender, EventArgs e)
        {
            // Удалим выбранный бэкап...
            string FName = BU_ListTable.Rows[BU_ListTable.CurrentRow.Index].Cells[4].Value.ToString();
            // Запросим подтверждение...
            DialogResult UserConfirmation = MessageBox.Show(RM.GetString("BU_DelMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                try
                {
                    // Удаляем файл...
                    File.Delete(GV.FullBackUpDirPath + BU_ListTable.Rows[BU_ListTable.CurrentRow.Index].Cells[4].Value.ToString());
                    // Удаляем строку...
                    BU_ListTable.Rows.Remove(BU_ListTable.CurrentRow);
                    // Показываем сообщение об успешном удалении...
                    MessageBox.Show(RM.GetString("BU_DelSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    // Произошло исключение при попытке удаления файла резервной копии...
                    MessageBox.Show(RM.GetString("BU_DelFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void BUT_CrBkupReg_ButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show(RM.GetString("BU_TBSelCat"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BUT_L_GameSettings_Click(object sender, EventArgs e)
        {
            // Создадим резервную копию графических настроек игры...
            try
            {
                CreateRegBackUpNow(@"HKEY_CURRENT_USER\Software\Valve\Source\" + GV.SmallAppName + @"\Settings", "Game_Options");
                MessageBox.Show(RM.GetString("BU_RegDone"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show(RM.GetString("BU_RegErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BUT_L_AllSteam_Click(object sender, EventArgs e)
        {
            // Создадим резервную копию всех настроек Steam...
            try
            {
                // Создаём...
                CreateRegBackUpNow(@"HKEY_CURRENT_USER\Software\Valve", "Steam_BackUp");
                // Выводим сообщение...
                MessageBox.Show(RM.GetString("BU_RegDone"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                // Произошло исключение, уведомим пользователя...
                MessageBox.Show(RM.GetString("BU_RegErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void BUT_L_AllSRC_Click(object sender, EventArgs e)
        {
            // Созданим резервную копию графических настроек всех Source-игр...
            try
            {
                CreateRegBackUpNow(@"HKEY_CURRENT_USER\Software\Valve\Source", "Source_Options");
                MessageBox.Show(RM.GetString("BU_RegDone"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show(RM.GetString("BU_RegErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Проверяем открыта ли страница "Редактор конфигов"...
            if (MainTabControl.SelectedIndex == 1)
            {
                // Включаем заблокированный контрол...
                MNUShowEdHint.Enabled = true;
                // Проверяем открыт ли файл в Редакторе конфигов...
                if (String.IsNullOrEmpty(CFGFileName))
                {
                    // Нет, не открыт. Выводим имя "Безымянный.cfg"...
                    SB_Status.Text = RM.GetString("StatusOpenedFile") + " " + RM.GetString("UnnamedFileName");
                }
                else
                {
                    // Да, открыт. Выводим настоящее имя...
                    SB_Status.Text = RM.GetString("StatusOpenedFile") + " " + CFGFileName;
                }
            }
            else
            {
                // Открыта другая страница...
                // Блокируем контрол подсказки...
                MNUShowEdHint.Enabled = false;
                // ...и выводим стандартное сообщение в статус-бар...
                SB_Status.Text = RM.GetString("StatusNormal");
            }

            // Проверяем, открыта ли страница "Резервные копии"...
            if (MainTabControl.SelectedIndex == 4)
            {
                BUT_Refresh.PerformClick();
            }
        }

        private void CE_ShowHint_Click(object sender, EventArgs e)
        {
            try
            {
                string Buf = CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[0].Value.ToString();
                if (!(String.IsNullOrEmpty(Buf)))
                {
                    Buf = GetCVDescription(Buf);
                    if (!(String.IsNullOrEmpty(Buf)))
                    {
                        MessageBox.Show(Buf, GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(RM.GetString("CE_ClNoDescr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(RM.GetString("CE_ClSelErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show(RM.GetString("CE_ClSelErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUHelp_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.easycoding.org/projects/srcrepair/help");
        }

        private void MNUOpinion_Click(object sender, EventArgs e)
        {
            Process.Start("mailto:srcrepair@easycoding.org?subject=SRC Repair Opinion");
        }

        private void MNUSteamGroup_Click(object sender, EventArgs e)
        {
            Process.Start("http://steamcommunity.com/groups/srcrepair");
        }

        private void MNUGroup1_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.easycoding.org/");
        }

        private void MNUGroup2_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.team-fortress.ru/");
        }

        private void MNUGroup3_Click(object sender, EventArgs e)
        {
            Process.Start("http://tf2world.ru/");
        }
    }
}
