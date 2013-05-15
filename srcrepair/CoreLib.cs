/*
 * Модуль с общими функциями SRC Repair.
 * 
 * Copyright 2011 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2013 EasyCoding Team.
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
using System.Security.Cryptography; // для расчёта хешей...

namespace srcrepair
{
    /// <summary>
    /// Предоставляет методы для общих целей.
    /// </summary>
    public sealed class CoreLib
    {
        /// <summary>
        /// Эта функция позволяет получить локализованную строку по её ID
        /// согласно текущим региональным настройкам Windows. Рекомендуется
        /// применять везде, чтобы не нарушать ООП.
        /// </summary>
        /// <param name="MsgId">ID сообщения в ресурсе</param>
        /// <returns>Локализованная строка</returns>
        public static string GetLocalizedString(string MsgId)
        {
            ResourceManager RMLocal = new ResourceManager("srcrepair.AppStrings", typeof(frmMainW).Assembly);
            return RMLocal.GetString(MsgId);
        }

        /// <summary>
        /// Получает из реестра и возвращает путь к установленному клиенту Steam.
        /// </summary>
        /// <returns>Путь к клиенту Steam</returns>
        public static string GetSteamPath()
        {
            // Подключаем реестр и открываем ключ только для чтения...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam", false);

            // Создаём строку для хранения результатов...
            string ResString = "";

            // Проверяем чтобы ключ реестр существовал и был доступен...
            if (ResKey != null)
            {
                // Получаем значение открытого ключа...
                object ResObj = ResKey.GetValue("SteamPath");

                // Проверяем чтобы значение существовало...
                if (ResObj != null)
                {
                    // Существует, возвращаем...
                    ResString = Path.GetFullPath(Convert.ToString(ResObj));
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
        /// Возвращает PID процесса если он был найден в памяти.
        /// </summary>
        /// <param name="ProcessName">Имя образа процесса</param>
        /// <returns>PID процесса, либо 0 если процесс не был найден</returns>
        public static int FindProcess(string ProcessName)
        {
            int ProcID = 0;
            Process[] LocalByName = Process.GetProcessesByName(ProcessName);
            foreach (Process ResName in LocalByName) { ProcID = ResName.Id; }
            return ProcID;
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
            return String.Format(MicroDate ? "{0}{1}{2}{3}{4}{5}" : "{0}.{1}.{2} {3}:{4}:{5}", SimpleIntStrWNull(XDate.Day),
                SimpleIntStrWNull(XDate.Month), SimpleIntStrWNull(XDate.Year), SimpleIntStrWNull(XDate.Hour),
                SimpleIntStrWNull(XDate.Minute), SimpleIntStrWNull(XDate.Second));
        }

        /// <summary>
        /// Проверяет наличие не-ASCII-символов в строке.
        /// </summary>
        /// <param name="Path">Путь для проверки</param>
        /// <returns>Возвращает True если не обнаружено запрещённых симолов</returns>
        public static bool CheckNonASCII(string Path)
        {
            // Проверяем строку на соответствие регулярному выражению...
            return Regex.IsMatch(Path, @"^[0-9a-zA-Z :()./\\\\]*$");
        }

        /// <summary>
        /// Запускает указанное приложение и ждёт его завершения.
        /// </summary>
        /// <param name="SAppName">Путь к приложению или его имя</param>
        /// <param name="SParameters">Параметры запуска</param>
        public static void StartProcessAndWait(string SAppName, string SParameters)
        {
            // Создаём объект с нужными параметрами...
            ProcessStartInfo ST = new System.Diagnostics.ProcessStartInfo();
            ST.FileName = SAppName;
            ST.Arguments = SParameters;
            ST.WindowStyle = ProcessWindowStyle.Hidden;
            // Запускаем процесс...
            Process NewProcess = Process.Start(ST);
            // Ждём завершения процесса...
            while (!(NewProcess.HasExited))
            {
                // Заставляем приложение "заснуть"...
                Thread.Sleep(1200);
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
            using (WebClient Downloader = new WebClient())
            {
                Downloader.Headers.Add("User-Agent", GV.UserAgent);
                DnlStr = Downloader.DownloadString(ChURI);
            }
            NewVersion = DnlStr.Substring(0, DnlStr.IndexOf("!"));
            if (CompareVersions(CurrentVersion, NewVersion)) { return true; } else { return false; }
        }

        /// <summary>
        /// Создаёт новый файл по указанному адресу.
        /// </summary>
        /// <param name="FileName">Имя создаваемого файла</param>
        public static void CreateFile(string FileName)
        {
            try
            {
                // Проверим существование каталога...
                string Dir = Path.GetDirectoryName(FileName);

                // Создадим при отсутствии...
                if (!(Directory.Exists(Dir))) { Directory.CreateDirectory(Dir); }

                // Создаём файл...
                using (FileStream fs = File.Create(FileName))
                {
                    // Закрываем...
                    fs.Close();
                }
            }
            catch { /* Do nothing */ }
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
                        CreateFile(DebugFileName);
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
        /// <param name="WTitle">Текст в заголовке сообщения об ошибке</param>
        /// <param name="DevMsg">Отладочное сообщение</param>
        /// <param name="DevMethod">Метод, вызвавший исключение</param>
        /// <param name="MsgIcon">Тип иконки: предупреждение, ошибка и т.д.</param>
        public static void HandleExceptionEx(string FrindlyMsg, string WTitle, string DevMsg, string DevMethod, MessageBoxIcon MsgIcon)
        {
            string ResultString = String.Format("{0} Raised by: {1}.", DevMsg, DevMethod);
            #if DEBUG
            // Для режима отладки покажем сообщение, понятное разработчикам...
            MessageBox.Show(ResultString, WTitle, MessageBoxButtons.OK, MsgIcon);
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

        /// <summary>
        /// Возвращает первую строку, в которой встречается параметр.
        /// </summary>
        /// <param name="Rx">Подстрока для поиска</param>
        /// <param name="FileName">Имя файла</param>
        private static string FindLineContText(string Rx, string FileName)
        {
            string ImpStr;
            string Result = null;
            using (StreamReader TxtFl = new StreamReader(FileName, Encoding.Default))
            {
                while (TxtFl.Peek() >= 0)
                {
                    ImpStr = TxtFl.ReadLine();
                    ImpStr = ImpStr.Trim();
                    if (!(String.IsNullOrEmpty(ImpStr)))
                    {
                        if (ImpStr.IndexOf(Rx, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            Result = ImpStr;
                            break;
                        }
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// Извлекает значение переменной из строки.
        /// </summary>
        /// <param name="LineA">Строка для извлечения</param>
        private static string ExtractCVFromLine(string LineA)
        {
            LineA = CleanStrWx(LineA, true);
            return LineA.Substring(LineA.LastIndexOf(" ")).Trim();
        }

        /// <summary>
        /// Возвращает значение переменной, переданной в параметре, хранящейся в файле.
        /// </summary>
        /// <param name="CVar">Переменная</param>
        /// <param name="CFileName">Имя файла конфига</param>
        public static int GetNCFDWord(string CVar, string CFileName)
        {
            string Result = "";
            try
            {
                string CVLine = FindLineContText(CVar, CFileName);
                if (!(String.IsNullOrEmpty(CVLine))) { Result = ExtractCVFromLine(CVLine); }
            }
            catch (Exception Ex) { WriteStringToLog(Ex.Message); }
            return Convert.ToInt32(Result);
        }

        /// <summary>
        /// Возвращает значение переменной типа double, переданной в параметре, хранящейся в файле.
        /// </summary>
        /// <param name="CVar">Переменная</param>
        /// <param name="CFileName">Имя файла конфига</param>
        public static double GetNCFDble(string CVar, string CFileName)
        {
            string Result = "";
            try
            {
                string CVLine = FindLineContText(CVar, CFileName);
                if (!(String.IsNullOrEmpty(CVLine))) { Result = ExtractCVFromLine(CVLine); Result = Result.Replace(".", ","); }
            }
            catch (Exception Ex) { WriteStringToLog(Ex.Message); }
            return Double.Parse(Result);
        }

        /// <summary>
        /// Вычисляет MD5 хеш файла.
        /// </summary>
        /// <param name="FileName">Имя файла</param>
        public static string CalculateFileMD5(string FileName)
        {
            FileStream FileP = new FileStream(FileName, FileMode.Open);
            MD5 MD5Crypt = new MD5CryptoServiceProvider();
            byte[] RValue = MD5Crypt.ComputeHash(FileP);
            FileP.Close();
            StringBuilder StrRes = new StringBuilder();
            for (int i = 0; i < RValue.Length; i++) { StrRes.Append(RValue[i].ToString("x2")); }
            return StrRes.ToString();
        }

        /// <summary>
        /// Добавляет переменную и значение в Редактор конфигов.
        /// </summary>
        /// <param name="Cv">Название переменной</param>
        /// <param name="Cn">Значение переменной</param>
        public delegate void CFGEdDelegate(string Cv, string Cn);

        /// <summary>
        /// Определяет путь к файлу Hosts...
        /// </summary>
        /// <param name="PlatformID">Код платформы запуска</param>
        /// <returns>Полный путь к Hosts...</returns>
        public static string GetHostsFileFullPath(int PlatformID = 0)
        {
            string Result = "";
            if (PlatformID == 0)
            {
                try
                {
                    // Получим путь к файлу hosts (вдруг он переопределён каким-либо зловредом)...
                    RegistryKey ResKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", false);
                    if (ResKey != null) { Result = (string)ResKey.GetValue("DataBasePath"); }
                    // Проверим получен ли путь из реестра. Если нет, вставим стандартный...
                    if (String.IsNullOrWhiteSpace(Result)) { Result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "drivers", "etc"); }
                }
                catch
                {
                    // Произошло исключение, поэтому укажем вручную...
                    Result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "drivers", "etc");
                }

                // Сгенерируем полный путь к файлу hosts...
                Result = Path.Combine(Result, "hosts");
            }
            else
            {
                Result = Path.Combine("/etc", "hosts");
            }
            return Result;
        }

        /// <summary>
        /// Определяет файловую систему на диске...
        /// </summary>
        /// <param name="CDrive">Диск, ФС которого нужно получить</param>
        /// <returns>Название файловой системы или Unknown</returns>
        public static string DetectDriveFileSystem(string CDrive)
        {
            string Result = "Unknown";
            DriveInfo[] Drives = DriveInfo.GetDrives();
            foreach (DriveInfo Dr in Drives)
            {
                if (String.Compare(Dr.Name, CDrive, true) == 0)
                {
                    Result = Dr.DriveFormat;
                    break;
                }
            }
            return Result;
        }

        /// <summary>
        /// Чистит строку от табуляций и лишних пробелов.
        /// </summary>
        /// <param name="RecvStr">Исходная строка</param>
        public static string CleanStrWx(string RecvStr, bool CleanQuotes = false)
        {
            // Почистим от табуляций...
            while (RecvStr.IndexOf("\t") != -1)
            {
                RecvStr = RecvStr.Replace("\t", " ");
            }

            // Удалим все лишние пробелы...
            while (RecvStr.IndexOf("  ") != -1)
            {
                RecvStr = RecvStr.Replace("  ", " ");
            }

            // Удалим кавычки если это разрешено...
            if (CleanQuotes)
            {
                while (RecvStr.IndexOf('"') != -1)
                {
                    RecvStr = RecvStr.Replace(@"""", "");
                }
            }

            // Возвращаем результат очистки...
            return RecvStr;
        }
    }
}
