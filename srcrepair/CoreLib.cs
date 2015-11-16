/*
 * Модуль с общими функциями SRC Repair.
 * 
 * Copyright 2011 - 2015 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2015 EasyCoding Team.
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
using System.Reflection; // для работы со сборками...
using System.Security.AccessControl; // для определения прав доступа...
using System.Management; // для работы с WMI...
using Ionic.Zip; // для работы с архивами...

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
        /// Проверяет запущен ли процесс, имя образа которого передано в качестве параметра.
        /// </summary>
        /// <param name="ProcessName">Имя образа процесса</param>
        /// <returns>Возвращает булево true если такой процесс запущен, иначе - false.</returns>
        public static bool IsProcessRunning(string ProcessName)
        {
            Process[] LocalByName = Process.GetProcessesByName(ProcessName);
            return LocalByName.Length > 0;
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
                if (MessageBox.Show(String.Format(ConfMsg, ResName.ProcessName), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ResName.Kill();
                }
            }
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
        /// Проверяет наличие не-ASCII-символов в строке.
        /// </summary>
        /// <param name="Path">Путь для проверки</param>
        /// <returns>Возвращает True если не обнаружено запрещённых симолов</returns>
        public static bool CheckNonASCII(string Path)
        {
            // Проверяем строку на соответствие регулярному выражению...
            return Regex.IsMatch(Path, @"^[0-9a-zA-Z :()-./\\\\]*$");
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
            return NVer > CVer;
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
                using (FileStream fs = File.Create(FileName)) { }
            }
            catch { /* Do nothing */ }
        }

        /// <summary>
        /// Возвращает путь к пользовательскому каталогу SRC Repair.
        /// </summary>
        public static string GetApplicationPath()
        {
            return Properties.Settings.Default.IsPortable ? Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "portable") : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Properties.Resources.AppName);
        }

        /// <summary>
        /// Функция, записывающая в лог-файл строку. Например, сообщение об ошибке.
        /// </summary>
        /// <param name="TextMessage">Сообщение для записи в лог</param>
        public static void WriteStringToLog(string TextMessage)
        {
            if (Properties.Settings.Default.EnableDebugLog) // Пишем в лог если включено...
            {
                try // Начинаем работу...
                {
                    // Сгенерируем путь к файлу с логом...
                    string DebugFileName = Path.Combine(GetApplicationPath(), Properties.Settings.Default.DebugLogFileName);
                    // Если файл не существует, создадим его и сразу закроем...
                    if (!File.Exists(DebugFileName))
                    {
                        CreateFile(DebugFileName);
                    }
                    // Начинаем записывать в лог-файл...
                    using (StreamWriter DFile = new StreamWriter(DebugFileName, true))
                    {
                        // Делаем запись...
                        DFile.WriteLine(String.Format("{0}: {1}", DateTime.Now.ToString(), TextMessage));
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
            MessageBox.Show(FrindlyMsg, Properties.Resources.AppName, MessageBoxButtons.OK, MsgIcon);
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
            bool Result = ResKey != null;
            // Если ключ был успешно открыт, закрываем.
            if (Result) { ResKey.Close(); }
            // Возвращаем результат функции...
            return Result;
        }

        /// <summary>
        /// Вычисляет MD5 хеш файла.
        /// </summary>
        /// <param name="FileName">Имя файла</param>
        public static string CalculateFileMD5(string FileName)
        {
            byte[] RValue;
            using (FileStream FileP = new FileStream(FileName, FileMode.Open))
            {
                using (MD5 MD5Crypt = new MD5CryptoServiceProvider())
                {
                    RValue = MD5Crypt.ComputeHash(FileP);
                }
            }
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
        /// <returns>Полный путь к Hosts...</returns>
        public static string GetHostsFileFullPath()
        {
            string Result = "";
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
        /// <param name="CleanQuotes">Задаёт параметры очистки кавычек</param>
        /// <param name="CleanSlashes">Задаёт параметры очистки двойных слэшей</param>
        public static string CleanStrWx(string RecvStr, bool CleanQuotes = false, bool CleanSlashes = false)
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

            // Удаляем двойные слэши если разрешено...
            if (CleanSlashes)
            {
                while (RecvStr.IndexOf(@"\\") != -1)
                {
                    RecvStr = RecvStr.Replace(@"\\", @"\");
                }
            }

            // Возвращаем результат очистки...
            return RecvStr.Trim();
        }

        /// <summary>
        /// Получает содержимое текстового файла из внутреннего ресурса приложения.
        /// </summary>
        /// <param name="FileName">Внутреннее имя ресурсного файла</param>
        /// <returns>Содержимое текстового файла</returns>
        public static string GetTemplateFromResource(string FileName)
        {
            string Result = "";
            using (Stream Strm = Assembly.GetExecutingAssembly().GetManifestResourceStream(FileName))
            {
                using (StreamReader Reader = new StreamReader(Strm))
                {
                    Result = Reader.ReadToEnd();
                }
            }
            return Result;
        }

        /// <summary>
        /// Получает содержимое текстового файла в массив построчно.
        /// </summary>
        /// <param name="FileName">Внутреннее имя ресурсного файла</param>
        /// <returns>Массив с построчным содержимым текстового файла</returns>
        public static List<String> ReadRowsFromResource(string FileName)
        {
            List<String> Template = new List<String>();
            using (Stream Strm = Assembly.GetExecutingAssembly().GetManifestResourceStream(FileName))
            {
                using (StreamReader Reader = new StreamReader(Strm))
                {
                    while (Reader.Peek() >= 0)
                    {
                        Template.Add(Reader.ReadLine());
                    }
                }
            }
            return Template;
        }

        /// <summary>
        /// Проверяет наличие прав на запись в указанном в качестве параметра каталоге.
        /// </summary>
        /// <param name="DirName">Путь к проверяемому каталогу</param>
        /// <returns>Булево наличия прав на запись</returns>
        public static bool IsDirectoryWritable(string DirName)
        {
            bool Result = false;
            try
            {
                DirectoryInfo DirInfo = new DirectoryInfo(DirName);
                DirectorySecurity DirSec = DirInfo.GetAccessControl();
                AuthorizationRuleCollection AuthRules = DirSec.GetAccessRules(true, true, typeof(NTAccount));
                foreach (AuthorizationRule AuthRule in AuthRules)
                {
                    if (AuthRule.IdentityReference.Value.Equals(WindowsIdentity.GetCurrent().Name.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        Result = (((FileSystemAccessRule)AuthRule).FileSystemRights & FileSystemRights.WriteData) > 0;
                    }
                }
            }
            catch { Result = false; }
            return Result;
        }

        /// <summary>
        /// Конвертирует дату и время из формата DateTime .NET в Unix-формат.
        /// </summary>
        /// <param name="DTime">Дата и время в формате DateTime</param>
        /// <returns>Дата и время в формате UnixTime</returns>
        public static string DateTime2Unix(DateTime DTime)
        {
            return Math.Round((DTime - new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime()).TotalSeconds, 0).ToString();
        }

        /// <summary>
        /// Конвертирует дату и время из Unix-формата в DateTime.
        /// </summary>
        /// <param name="UnixTime">Дата и время в Unix-формате</param>
        /// <returns>Дата и время в формате DateTime</returns>
        public static DateTime Unix2DateTime(double UnixTime)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(UnixTime);
        }

        /// <summary>
        /// Определяет архитектуру операционной системы.
        /// </summary>
        /// <returns>Артитектура ОС</returns>
        public static string GetSystemArch()
        {
            return Environment.Is64BitOperatingSystem ? "Amd64" : "x86";
        }

        /// <summary>
        /// Определяет все доступные в системе разрешения экрана посредством запроса к WMI.
        /// </summary>
        /// <returns>Возвращает список доступных разрешений</returns>
        public static List<String> GetDesktopResolutions()
        {
            // Инициализируем переменные...
            List<String> Result = new List<String>();
            ManagementScope MMCScope = new ManagementScope();

            try
            {
                // Выполняем запрос к WMI...
                ObjectQuery MMCQuery = new ObjectQuery("SELECT * FROM CIM_VideoControllerResolution");

                // Обрабатываем результаты...
                using (ManagementObjectSearcher MMCSearcher = new ManagementObjectSearcher(MMCScope, MMCQuery))
                {
                    ManagementObjectCollection MMCQueryResults = MMCSearcher.Get();
                    foreach (ManagementBaseObject MMCRes in MMCQueryResults)
                    {
                        Result.Add(String.Format("{0}x{1}@{2}Hz", MMCRes["HorizontalResolution"], MMCRes["VerticalResolution"], MMCRes["RefreshRate"]));
                    }
                }
            }
            catch (Exception Ex) { WriteStringToLog(Ex.Message); }

            // Отдаём результат...
            return Result.Distinct().ToList();
        }

        /// <summary>
        /// Генерирует уникальное имя для файла резервной копии.
        /// </summary>
        /// <param name="BackUpDir">Каталог хранения резервных копий</param>
        /// <param name="Prefix">Префикс имени файла резервной копии</param>
        /// <returns>Имя файла резервной копии</returns>
        public static string GenerateBackUpFileName(string BackUpDir, string Prefix)
        {
            return Path.Combine(BackUpDir, String.Format("{0}_{1}.bud", Prefix, DateTime2Unix(DateTime.Now)));
        }

        /// <summary>
        /// Упаковывает файлы, имена которых переданых в массиве, в Zip-архив с
        /// произвольным именем.
        /// </summary>
        /// <param name="Files">Массив с именами файлов, которые будут добавлены в архив</param>
        /// <param name="ArchiveName">Имя для создаваемого архивного файла</param>
        /// <returns>В случае успеха возвращает истину, иначе - ложь</returns>
        public static bool CompressFiles(List<String> Files, string ArchiveName)
        {
            try
            {
                using (ZipFile ZBkUp = new ZipFile(ArchiveName, Encoding.UTF8))
                {
                    ZBkUp.AddFiles(Files, true, "");
                    ZBkUp.Save();
                }
            }
            catch (Exception Ex)
            {
                try { if (File.Exists(ArchiveName)) { File.Delete(ArchiveName); } } catch (Exception E1) { WriteStringToLog(E1.Message); }
                WriteStringToLog(Ex.Message);
            }
            return File.Exists(ArchiveName);
        }

        /// <summary>
        /// Начинает загрузку с указанного URL с подробным отображением процесса.
        /// </summary>
        /// <param name="URI">URL для загрузки</param>
        /// <param name="FileName">Путь для сохранения</param>
        public static void DownloadFileEx(string URI, string FileName)
        {
            using (frmDnWrk DnW = new frmDnWrk(URI, FileName))
            {
                DnW.ShowDialog();
            }
        }

        /// <summary>
        /// Открывает указанный URL в системном браузере по умолчанию.
        /// </summary>
        /// <param name="URI">URL для загрузки в браузере</param>
        public static void OpenWebPage(string URI)
        {
            try { Process.Start(URI); } catch (Exception Ex) { WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Распаковывает архив в указанный каталог при помощи библиотеки DotNetZip
        /// с выводом прогресса в отдельном окне.
        /// </summary>
        /// <param name="ArchName">Имя архивного файла с указанием полного пути</param>
        /// <param name="DestDir">Каталог назначения</param>
        public static void ExtractFiles(string ArchName, string DestDir)
        {
            FrmArchWrk ArW = new FrmArchWrk(ArchName, DestDir);
            ArW.ShowDialog();
        }

        /// <summary>
        /// Динамически создаёт форму по указанному %namespace%.%classname%.
        /// </summary>
        /// <param name="ClassName">Имя класса формы</param>
        public static void ShowBasicFormDialog(string ClassName)
        {
            Form ActiveForm = (Form)Assembly.GetExecutingAssembly().CreateInstance(ClassName);
            ActiveForm.ShowDialog();
        }
    }
}
