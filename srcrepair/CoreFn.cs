using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO; // для работы с файлами...
using System.Diagnostics; // для управления процессами...
using Microsoft.Win32; // для работы с реестром...

namespace srcrepair
{
    public class CoreFn
    {
        /* В этой переменной мы будем хранить полный путь к каталогу установленного
         * клиента Steam. */
        public static string FullSteamPath = "";

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

        public static int GetSRCDWord(string CVar, string CApp)
        {
            // Подключаем реестр и открываем ключ только для чтения...
            RegistryKey ResKey = Registry.LocalMachine.OpenSubKey("Software\\Valve\\Source\\" + CApp + "\\Settings", false);

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

        public static void WriteSRCDWord(string CVar, int CValue, string CApp)
        {
            // Подключаем реестр и открываем ключ для чтения и записи...
            RegistryKey ResKey = Registry.LocalMachine.OpenSubKey("Software\\Valve\\Source\\" + CApp + "\\Settings", true);

            // Записываем в реестр...
            ResKey.SetValue(CVar, CValue);

            // Закрываем открытый ранее ключ реестра...
            ResKey.Close();
        }

        public static void RemoveRegistryKeyLM(string RKey)
        {
            // Удаляем запрощенную ветку рекурсивно из HKLM...
            Registry.LocalMachine.DeleteSubKeyTree(RKey, false);
        }

        public static void RemoveRegistryKeyCU(string RKey)
        {
            // Удаляем запрощенную ветку рекурсивно из HKCU...
            Registry.CurrentUser.DeleteSubKeyTree(RKey, false);
        }

        public static string ReadTextFileNow(string FileName)
        {
            // Считываем всё содержимое файла...
            string TextFile = File.ReadAllText(FileName);
            // Возвращаем результат...
            return TextFile;
        }
    }
}
