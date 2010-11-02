using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO; // для работы с файлами...
using System.Diagnostics; // для управления процессами...
using Microsoft.Win32; // для работы с реестром...
using System.Windows.Forms; // для работы с формами (в нашем случае - InputBox)...
using System.Drawing; // аналогично Forms...

namespace srcrepair
{
    public class CoreFn
    {        
        /* Реализуем аналог полезной дельфийской фукнции IncludeTrailingPathDelimiter,
         * которая возвращает строку, добавив на конец обратный слэш если его нет,
         * либо возвращает ту же строку, если обратный слэш уже присутствует. */
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

        /* Эта функция получает из реестра и возвращает путь к установленному
         * клиенту Steam. */
        public static string GetSteamPath()
        {
            // Подключаем реестр и открываем ключ только для чтения...
            RegistryKey ResKey = Registry.LocalMachine.OpenSubKey("Software\\Valve\\Steam", false);

            // Создаём строку для хранения результатов...
            string ResString = "";

            // Проверяем чтобы ключ реестр существовал и был доступен...
            if (ResKey != null)
            {
                // Получаем значение открытого ключа...
                ResString = (string)ResKey.GetValue("InstallPath");

                // Проверяем чтобы значение существовало...
                if (ResString == null)
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

        /* Эта функция возвращает PID процесса если он был найден в памяти и
         * завершает, либо 0 если процесс не был найден. */
        public static int ProcessTerminate(string ProcessName, bool ShowConfirmationMsg)
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
                    DialogResult UserConfirmation = MessageBox.Show(Properties.Resources.KillMessage1 + " " + ResName.ProcessName + "! " + Properties.Resources.KillMessage2, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        /* Эта функция очищает блобы (файлы с расширением *.blob) из каталога Steam.
         * В качестве параметра ей передаётся полный путь к каталогу Steam. */
        public static void CleanBlobsNow(string SteamPath)
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

        /* Эта функция удаляет значения реестра, отвечающие за настройки клиента
         * Steam, а также записывает значение языка. */
        public static void CleanRegistryNow(int LangCode)
        {
            // Удаляем ключ HKEY_LOCAL_MACHINE\Software\Valve рекурсивно...
            Registry.LocalMachine.DeleteSubKeyTree("Software\\Valve", false);

            // Удаляем ключ HKEY_CURRENT_USER\Software\Valve рекурсивно...
            Registry.CurrentUser.DeleteSubKeyTree("Software\\Valve", false);

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
            RegistryKey RegLangKey = Registry.CurrentUser.CreateSubKey("Software\\Valve\\Steam");

            // Если не было ошибок, записываем значение...
            if (RegLangKey != null)
            {
                // Записываем значение в реестр...
                RegLangKey.SetValue("language", XLang);
            }

            // Закрываем ключ...
            RegLangKey.Close();
        }

        /* Эта функция получает из реестра значение нужной нам переменной
         * для указанного игрового приложения. */
        public static int GetSRCDWord(string CVar, string CApp)
        {
            // Подключаем реестр и открываем ключ только для чтения...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Source\\" + CApp + "\\Settings", false);

            // Создаём переменную для хранения результатов...
            int ResInt = -1;

            // Проверяем чтобы ключ реестр существовал и был доступен...
            if (ResKey != null)
            {
                // Получаем значение открытого ключа...
                ResInt = (int)ResKey.GetValue(CVar);

                // Проверяем чтобы значение существовало...
                if (ResInt == null)
                {
                    // Значение не существует, поэтому сгенерируем исключение для обработки в основном коде...
                    throw new System.NullReferenceException("Exception: requested value does not exists!");
                }
            }

            // Закрываем открытый ранее ключ реестра...
            ResKey.Close();

            // Возвращаем результат...
            return ResInt;
        }

        /* Эта процедура записывает в реестр новое значение нужной нам переменной
         * для указанного игрового приложения. */
        public static void WriteSRCDWord(string CVar, int CValue, string CApp)
        {
            // Подключаем реестр и открываем ключ для чтения и записи...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Source\\" + CApp + "\\Settings", true);

            // Записываем в реестр...
            ResKey.SetValue(CVar, CValue, RegistryValueKind.DWord);

            // Закрываем открытый ранее ключ реестра...
            ResKey.Close();
        }

        /* Эта функция удаляет нужный ключ из ветки HKEY_LOCAL_MACHINE... */
        public static void RemoveRegistryKeyLM(string RKey)
        {
            // Удаляем запрощенную ветку рекурсивно из HKLM...
            Registry.LocalMachine.DeleteSubKeyTree(RKey, false);
        }

        /* Эта функция удаляет нужный ключ из ветки HKEY_CURRENT_USER... */
        public static void RemoveRegistryKeyCU(string RKey)
        {
            // Удаляем запрощенную ветку рекурсивно из HKCU...
            Registry.CurrentUser.DeleteSubKeyTree(RKey, false);
        }

        /* Эта функция считывает содержимое текстового файла в строку и
         * возвращает в качестве результата. */
        public static string ReadTextFileNow(string FileName)
        {
            // Считываем всё содержимое файла...
            string TextFile = File.ReadAllText(FileName);
            // Возвращаем результат...
            return TextFile;
        }

        /* Эта функция ищет указанную строку в массиве строк и возвращает её индекс,
         * либо -1 если такой строки в массиве не найдено. */
        public static int FindStringInStrArray(string[] SourceStr, string What)
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

        /* Эта функция ищет в массиве строк нужный нам параметр командной строки
         * и возвращает true если параметр был найден, либо false если нет. */
        public static bool FindCommandLineSwitch(string[] Source, string CLineArg)
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

        /* Эта функция устанавливает требуемый FPS-конфиг... */
        public static void InstallConfigNow(string ConfName)
        {
            // Устанавливаем...
            File.Copy(GV.FullAppPath + "cfgs\\" + GV.SmallAppName + "\\" + ConfName, GV.FullCfgPath + "autoexec.cfg", true);
        }

        /* Эта функция генерирует ДДММГГЧЧММСС из указанного времени в строку.
         * Применяется для служебных целей. */
        public static string SetYMDHMSToStr(DateTime XDate)
        {
            // Возвращаем строку с результатом...
            return XDate.Day.ToString() + XDate.Month.ToString() + XDate.Year.ToString() + XDate.Hour.ToString() + XDate.Minute.ToString() + XDate.Second.ToString();
        }

        /* Эта функция создаёт резервную копию конфига, имя которого передано
         * в параметре. */
        public static void CreateBackUpNow(string ConfName)
        {
            // Сначала проверим, существует ли запрошенный файл...
            if (File.Exists(GV.FullCfgPath + ConfName))
            {
                // Проверяем чтобы каталог для бэкапов существовал...
                if (!(Directory.Exists(GV.FullBackUpDirPath)))
                {
                    // Каталоги не существуют. Создадим общий каталог для резервных копий...
                    Directory.CreateDirectory(GV.FullBackUpDirPath);
                    // ...и реестра...
                    Directory.CreateDirectory(GV.FullBackUpDirPath + "registry\\");
                }
                
                try
                {
                    // Копируем оригинальный файл в файл бэкапа...
                    File.Copy(GV.FullCfgPath + ConfName, GV.FullBackUpDirPath + ConfName + "." + CoreFn.SetYMDHMSToStr(DateTime.Now), true);
                }
                catch
                {
                    // Произошло исключение. Уведомим пользователя об этом...
                    MessageBox.Show(Properties.Resources.BackUpCreationFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // Источник данной функции: http://www.csharp-examples.net/inputbox/ //
        public static DialogResult InputBox(string title, string promptText, ref string value)
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
            buttonCancel.Text = Properties.Resources.InputBoxCancelBtnName;
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

    }
}
