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
using System.Resources; // для управления ресурсами...
using System.Threading; // для управления потоками...
using System.Net; // для скачивания файлов...

namespace srcrepair
{
    public class CoreLib
    {
        /// <summary>
        /// Эта функция позволяет получить локализованную строку по её ID
        /// согласно текущим региональным настройкам Windows. Рекомендуется
        /// применять только в формах, чтобы не нарушать ООП.
        /// </summary>
        /// <param name="MsgId">ID сообщения в ресурсе</param>
        /// <returns>Локализованная строка</returns>
        public static string GetLocalizedString(string MsgId)
        {
            ResourceManager RMLocal = new ResourceManager("srcrepair.AppStrings", typeof(frmMainW).Assembly);
            return RMLocal.GetString(MsgId);
        }

        /// <summary>
        /// Аналог полезной дельфийской фукнции IncludeTrailingPathDelimiter,
        /// которая возвращает строку, добавив на конец обратный слэш если его нет,
        /// либо возвращает ту же строку, если обратный слэш уже присутствует.
        /// </summary>
        /// <param name="SourceStr">Исходная строка</param>
        /// <returns>Строка с закрывающим слэшем</returns>
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

        /// <summary>
        /// Получает из реестра и возвращает путь к установленному клиенту Steam.
        /// </summary>
        /// <returns>Путь к клиенту Steam</returns>
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
                object ResObj = ResKey.GetValue("InstallPath");

                // Проверяем чтобы значение существовало...
                if (ResObj != null)
                {
                    // Существует, возвращаем...
                    ResString = Convert.ToString(ResObj);
                }
                else
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

        /// <summary>
        /// Возвращает PID процесса если он был найден в памяти и завершает его.
        /// </summary>
        /// <param name="ProcessName">Имя образа процесса</param>
        /// <returns>PID снятого процесса, либо 0 если процесс не был найден</returns>
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

        /// <summary>
        /// Запрашивает подтверждение и снимает процесс.
        /// </summary>
        /// <param name="ProcessName">Имя образа процесса</param>
        /// <param name="ConfMsg">Текст сообщения</param>
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

        /// <summary>
        /// Очищает блобы (файлы с расширением *.blob) из каталога Steam.
        /// </summary>
        /// <param name="SteamPath">Полный путь к каталогу Steam</param>
        public static void CleanBlobsNow(string SteamPath)
        {
            // Инициализируем буферную переменную, в которой будем хранить имя файла...
            string FileName;

            // Генерируем имя первого кандидата на удаление с полным путём до него...
            FileName = Path.Combine(SteamPath, "AppUpdateStats.blob");

            // Проверяем существует ли данный файл...
            if (File.Exists(FileName))
            {
                // Удаляем...
                File.Delete(FileName);
            }

            // Аналогично генерируем имя второго кандидата...
            FileName = Path.Combine(SteamPath, "ClientRegistry.blob");

            // Проверяем, существует ли файл...
            if (File.Exists(FileName))
            {
                // Удаляем...
                File.Delete(FileName);
            }
        }

        /// <summary>
        /// Удаляет значения реестра, отвечающие за настройки клиента Steam,
        /// а также записывает значение языка.
        /// </summary>
        /// <param name="LangCode">ID языка Steam</param>
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

        /// <summary>
        /// Получает из реестра значение нужной нам переменной для
        /// указанного игрового приложения.
        /// </summary>
        /// <param name="CVar">Название переменной</param>
        /// <param name="CKey">Ключ реестра, который будем просматривать</param>
        /// <returns>Значение переменной</returns>
        public static int GetSRCDWord(string CVar, string CKey)
        {
            // Подключаем реестр и открываем ключ только для чтения...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(CKey, false);

            // Создаём переменную для хранения результатов...
            int ResInt = -1;

            // Проверяем чтобы ключ реестр существовал и был доступен...
            if (ResKey != null)
            {
                // Получаем значение открытого ключа...
                object ResObj = ResKey.GetValue(CVar);

                // Проверяем существование значения...
                if (ResObj != null)
                {
                    // Возвращаем результат...
                    ResInt = Convert.ToInt32(ResObj);
                }
                else
                {
                    // Значение не существует, генерируем исключение...
                    throw new System.NullReferenceException("Specified value does not exists!");
                }
            }

            // Закрываем открытый ранее ключ реестра...
            ResKey.Close();

            // Возвращаем результат...
            return ResInt;
        }

        /// <summary>
        /// Записывает в реестр новое значение нужной нам переменной для
        /// указанного игрового приложения.
        /// </summary>
        /// <param name="CVar">Название переменной</param>
        /// <param name="CValue">Значение переменной</param>
        /// <param name="CKey">Ключ реестра</param>
        public static void WriteSRCDWord(string CVar, int CValue, string CKey)
        {
            // Подключаем реестр и открываем ключ для чтения и записи...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(CKey, true);

            // Записываем в реестр...
            ResKey.SetValue(CVar, CValue, RegistryValueKind.DWord);

            // Закрываем открытый ранее ключ реестра...
            ResKey.Close();
        }

        /// <summary>
        /// Проверяет есть ли у пользователя, с правами которого запускается
        /// программа, привилегии локального администратора.
        /// </summary>
        /// <returns>Булево true если есть</returns>
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

        /// <summary>
        /// Преобразует число в строку с добавлением незначащих нулей перед
        /// числами с 0 до 9 включительно. Используется для служебных целей.
        /// </summary>
        /// <param name="Numb">Число</param>
        /// <returns>Число с незначащим нулём в виде строки</returns>
        private static string SimpleIntStrWNull(int Numb)
        {
            string Result;
            if ((Numb >= 0) && (Numb <= 9)) { Result = "0" + Numb.ToString(); } else { Result = Numb.ToString(); }
            return Result;
        }

        /// <summary>
        /// Генерирует ДДММГГЧЧММСС из указанного времени в строку.
        /// Применяется для служебных целей.
        /// </summary>
        /// <param name="XDate">Дата и время для преобразования</param>
        /// <param name="MicroDate">Микро или по ГОСТ</param>
        /// <returns>Строка в выбранном формате</returns>
        public static string WriteDateToString(DateTime XDate, bool MicroDate)
        {
            return MicroDate ? SimpleIntStrWNull(XDate.Day) + SimpleIntStrWNull(XDate.Month) +
                SimpleIntStrWNull(XDate.Year) + SimpleIntStrWNull(XDate.Hour) +
                SimpleIntStrWNull(XDate.Minute) + SimpleIntStrWNull(XDate.Second) :
                SimpleIntStrWNull(XDate.Day) + "." + SimpleIntStrWNull(XDate.Month) + "." +
                SimpleIntStrWNull(XDate.Year) + " " + SimpleIntStrWNull(XDate.Hour) + ":" +
                SimpleIntStrWNull(XDate.Minute) + ":" + SimpleIntStrWNull(XDate.Second);
        }

        /// <summary>
        /// Проверяет наличие не-ASCII-символов в строке.
        /// </summary>
        /// <param name="Path">Путь для проверки</param>
        /// <returns>Возвращает True если не обнаружено запрещённых симолов</returns>
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

        /// <summary>
        /// Запускает указанное приложение и ждёт его завершения.
        /// </summary>
        /// <param name="SAppName">Путь к приложению или его имя</param>
        /// <param name="SParameters">Параметры запуска</param>
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

        /// <summary>
        /// Удаляет файлы в заданной папке по указанной маске.
        /// </summary>
        /// <param name="DirPath">Каталог для работы</param>
        /// <param name="CleanupMask">Маска файлов для удаления</param>
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

        /// <summary>
        /// Получает из реестра значение указанной переменной булевского типа.
        /// При отсутствии ключа или значение - возвращается значение по умолчанию.
        /// </summary>
        /// <param name="CVar">Название переменной</param>
        /// <param name="Subkey">Подключ в HKCU</param>
        /// <param name="Default">Значение по умолчанию</param>
        /// <returns>Значение запрошенной переменной</returns>
        public static bool GetAppBool(string CVar, string Subkey, bool Default)
        {
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(Path.Combine("Software", Subkey), false);
            bool Result = Default;
            if (ResKey != null)
            {
                object RetObj = ResKey.GetValue(CVar);
                if (RetObj != null)
                {
                    Result = Convert.ToBoolean((int)RetObj);
                }
                ResKey.Close();
            }
            return Result;
        }

        /// <summary>
        /// Проверяет существование указанного ключа в HKCU и при
        /// отсутствии создаёт автоматически.
        /// </summary>
        /// <param name="KeyName">Ключ реестра для проверки</param>
        public static void CheckRegKeyAndCreateCU(string KeyName)
        {
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(KeyName, false);
            if (ResKey == null) { Registry.CurrentUser.CreateSubKey(KeyName); } else { ResKey.Close(); }
        }

        /// <summary>
        /// Записывает в реестр параметр-булево настроек программы.
        /// </summary>
        /// <param name="CVar">Название переменной</param>
        /// <param name="Subkey">Подключ в HKCU</param>
        /// <param name="CValue">Значение переменной</param>
        public static void WriteAppBool(string CVar, string Subkey, bool CValue)
        {
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(Path.Combine("Software", Subkey), true);
            ResKey.SetValue(CVar, Convert.ToInt32(CValue), RegistryValueKind.DWord);
            ResKey.Close();
        }

        /// <summary>
        /// Форматирует размер файла для удобства пользователя.
        /// Файлы от 0 до 1 КБ - 1 записываются в байтах, от 1 КБ до
        /// 1 МБ - 1 - в килобайтах, от 1 МБ до 1 ГБ - 1 - в мегабайтах.
        /// </summary>
        /// <param name="InpNumber">Размер файла в байтах</param>
        /// <returns>Форматированная строка</returns>
        public static string SclBytes(long InpNumber)
        {
            // Проверяем на размер в байтах...
            if ((InpNumber >= 0) && (InpNumber <= 1023)) { return InpNumber.ToString() + " B"; }
            // ...килобайтах...
            if ((InpNumber >= 1024) && (InpNumber <= 1048575)) { return (Math.Round((float)InpNumber / 1024, 2)).ToString() + " KB"; }
            // ...мегабайтах...
            if ((InpNumber >= 1048576) && (InpNumber <= 1073741823)) { return (Math.Round((float)InpNumber / 1024 / 1024, 2)).ToString() + " MB"; }
            // ...гигабайтах.
            if ((InpNumber >= 1073741823) && (InpNumber <= 1099511627775)) { return (Math.Round((float)InpNumber / 1024 / 1024 / 1024, 2)).ToString() + " GB"; }
            // Если размер всё-таки больше, выведем просто строку...
            return InpNumber.ToString();
        }

        /// <summary>
        /// Проверяет является ли версия, указанная в параметре
        /// NewVer, новее, чем CurrVer. Используется модулем проверки обновлений
        /// и модулем автоматического обновления.
        /// </summary>
        /// <param name="CurrVer">Текущая версия</param>
        /// <param name="NewVer">Новая версия</param>
        /// <returns>Возвращает true, если новее</returns>
        public static bool CompareVersions(string CurrVer, string NewVer)
        {
            Version CVer = new Version(CurrVer);
            Version NVer = new Version(NewVer);
            if (NVer > CVer) { return true; } else { return false; }
        }

        /// <summary>
        /// Проверяет наличие обновлений для программы. Используется в модуле автообновления.
        /// </summary>
        /// <param name="CurrentVersion">Текущая версия</param>
        /// <param name="ChURI">URL проверки обновлений</param>
        /// <returns>Возвращает true при обнаружении обновлений</returns>
        public static bool AutoUpdateCheck(string CurrentVersion, string ChURI)
        {
            string NewVersion, DnlStr;
            using (WebClient Downloader = new WebClient()) { DnlStr = Downloader.DownloadString(ChURI); }
            NewVersion = DnlStr.Substring(0, DnlStr.IndexOf("!"));
            if (CompareVersions(CurrentVersion, NewVersion)) { return true; } else { return false; }
        }

        /// <summary>
        /// Функция, записывающая в лог-файл строку. Например, сообщение об ошибке.
        /// </summary>
        /// <param name="TextMessage">Сообщение для записи в лог</param>
        /// <param name="LogFile">Имя файла с логом</param>
        public static void WriteStringToLog(string TextMessage)
        {
            if (Properties.Settings.Default.EnableDebugLog) // Пишем в лог если включено...
            {
                try // Начинаем работу...
                {
                    // Сгенерируем путь к файлу с логом...
                    string DebugFileName = Path.Combine(GV.AppUserDir, Properties.Settings.Default.DebugLogFileName);
                    // Если файл не существует, создадим его и сразу закроем...
                    if (!File.Exists(DebugFileName))
                    {
                        using (FileStream fs = File.Create(DebugFileName)) // Создаём...
                        {
                            // Закрываем...
                            fs.Close();
                        }
                    }
                    // Начинаем записывать в лог-файл...
                    using (StreamWriter DFile = new StreamWriter(DebugFileName, true))
                    {
                        // Делаем запись...
                        DFile.WriteLine(String.Format("{0}: {1}", WriteDateToString(DateTime.Now, false), TextMessage));
                        // Закрываем файл...
                        DFile.Close();
                    }
                }
                catch { /* Подавляем исключения... */ }
            }
        }
        
        /// <summary>
        /// Функция, записывающая в лог-файл текст исключения, дату его возникновения
        /// и другую отладочную информацию, а также выводящая дружественное сообщение для
        /// пользователя и подробное для разработчика.
        /// </summary>
        /// <param name="FrindlyMsg">Понятное пользователю сообщение</param>
        /// <param name="DevMsg">Отладочное сообщение</param>
        /// <param name="DevMethod">Метод, вызвавший исключение</param>
        /// <param name="MsgIcon">Тип иконки: предупреждение, ошибка и т.д.</param>
        public static void HandleExceptionEx(string FrindlyMsg, string DevMsg, string DevMethod, MessageBoxIcon MsgIcon)
        {
            string ResultString = String.Format("{0} Raised by: {1}.", DevMsg, DevMethod);
            #if DEBUG
            // Для режима отладки покажем сообщение, понятное разработчикам...
            MessageBox.Show(ResultString, GV.AppName, MessageBoxButtons.OK, MsgIcon);
            #else
            // Для обычного режима покажем обычное сообщение...
            MessageBox.Show(FrindlyMsg, GV.AppName, MessageBoxButtons.OK, MsgIcon);
            #endif
            // Запишем в файл...
            WriteStringToLog(ResultString);
        }

        /// <summary>
        /// Проверяет существование в реестре требуемого ключа. При отсутствии оного
        /// возвращает false.
        /// </summary>
        /// <param name="Subkey">Подключ реестра для проверки</param>
        public static bool CheckIfHKCUSKeyExists(string Subkey)
        {
            // Открываем проверяемый ключ реестра... При ошибке вернёт null.
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(Subkey, false);
            // Получаем результат проверки...
            bool Result = (ResKey == null) ? false : true;
            // Если ключ был успешно открыт, закрываем.
            if (Result) { ResKey.Close(); }
            // Возвращаем результат функции...
            return Result;
        }
    }
}
