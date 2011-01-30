/*
 * Модуль с общими функциями SRC Repair.
 * 
 * Copyright 2011 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2011 EasyCoding Team.
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
using System.Windows.Forms; // для работы с формами...
using System.Linq;
using System.Text;
using System.IO; // для работы с файлами...
using System.Diagnostics; // для управления процессами...
using Microsoft.Win32; // для работы с реестром...
using System.Text.RegularExpressions;  // для работы с регулярными выражениями...
using System.Security.Principal; // для определения прав админа...
using System.Threading; // для управления потоками...

namespace srcrepair
{
    public class CoreLib
    {
        /*
         * Реализуем аналог полезной дельфийской фукнции IncludeTrailingPathDelimiter,
         * которая возвращает строку, добавив на конец обратный слэш если его нет,
         * либо возвращает ту же строку, если обратный слэш уже присутствует.
         */
        public static string IncludeTrDelim(string SourceStr)
        {
            // Проверяем наличие закрывающего слэша у строки, переданной как параметр...
            if (SourceStr[SourceStr.Length - 1] != Path.DirectorySeparatorChar)
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
        public static string GetSteamPath()
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
        public static int ProcessTerminate(string ProcessName)
        {
            // Обнуляем PID...
            int ProcID = 0;

            // Фильтруем список процессов по заданной маске в параметрах и вставляем в массив...
            Process[] LocalByName = Process.GetProcessesByName(ProcessName);

            // Запускаем цикл по поиску и завершению процессов...
            foreach (Process ResName in LocalByName)
            {
                ProcID = ResName.Id; // Сохраняем PID процесса...
                ResName.Kill(); // Завершаем процесс...
            }

            // Возвращаем PID как результат функции...
            return ProcID;
        }

        /*
         * Второй вариант функции. Запрашивает подтверждение и снимает процесс.
         */
        public static void ProcessTerminate(string ProcessName, string ConfMsg)
        {
            Process[] LocalByName = Process.GetProcessesByName(ProcessName);
            foreach (Process ResName in LocalByName)
            {
                if (MessageBox.Show(String.Format(ConfMsg, ResName.ProcessName), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ResName.Kill();
                }
            }
        }

        /*
         * Эта функция очищает блобы (файлы с расширением *.blob) из каталога Steam.
         * В качестве параметра ей передаётся полный путь к каталогу Steam.
         */
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

        /*
         * Эта функция удаляет значения реестра, отвечающие за настройки клиента
         * Steam, а также записывает значение языка.
         */
        public static void CleanRegistryNow(int LangCode)
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
        public static int GetSRCDWord(string CVar, string CApp)
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
        public static void WriteSRCDWord(string CVar, int CValue, string CApp)
        {
            // Подключаем реестр и открываем ключ для чтения и записи...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Source\" + CApp + @"\Settings", true);

            // Записываем в реестр...
            ResKey.SetValue(CVar, CValue, RegistryValueKind.DWord);

            // Закрываем открытый ранее ключ реестра...
            ResKey.Close();
        }

        /*
         * Эта функция проверяет есть ли у пользователя, с правами которого запускается
         * программа, привилегии локального администратора.
         */
        public static bool IsCurrentUserAdmin()
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

        /*
         * Эта функция ищет в массиве строк нужный нам параметр командной строки
         * и возвращает true если параметр был найден, либо false если нет.
         */
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

        /*
         * Эта функция генерирует ДДММГГЧЧММСС из указанного времени в строку.
         * Применяется для служебных целей.
         */
        public static string WriteDateToString(DateTime XDate, bool MicroDate)
        {
            return MicroDate ? XDate.Day.ToString() + XDate.Month.ToString() + XDate.Year.ToString() +
                XDate.Hour.ToString() + XDate.Minute.ToString() + XDate.Second.ToString() :
                XDate.Day.ToString() + "." + XDate.Month.ToString() + "." + XDate.Year.ToString() + " " +
                XDate.Hour.ToString() + ":" + XDate.Minute.ToString() + ":" + XDate.Second.ToString();
        }

        /*
         * Эта функция проверяет наличие не-ASCII-символов в строке. Возвращает True
         * если не обнаружено запрещённых симолов и False - если они были обнаружены.
         */
        public static bool CheckNonASCII(string Path)
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
        public static void CleanDirectoryNow(string DirPath, string CleanupMask)
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
    }
}
