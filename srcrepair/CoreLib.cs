/*
 * Модуль с общими функциями SRC Repair.
 * 
 * Copyright 2011 - 2016 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2016 EasyCoding Team.
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
using System.Security.Permissions; // для работы с EnvironmentPermissionAttribute...
using System.Threading; // для управления потоками...
using System.Security.Cryptography; // для расчёта хешей...
using System.Reflection; // для работы со сборками...
using System.Management; // для работы с WMI...
using Ionic.Zip; // для работы с архивами...

namespace srcrepair
{
    /// <summary>
    /// Предоставляет методы для общих целей.
    /// </summary>
    public static class CoreLib
    {
        /// <summary>
        /// Получает из реестра и возвращает путь к установленному клиенту Steam.
        /// </summary>
        /// <returns>Путь к клиенту Steam</returns>
        public static string GetSteamPath()
        {
            // Подключаем реестр и открываем ключ только для чтения...
            RegistryKey ResKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam", false);

            // Создаём строку для хранения результатов...
            string ResString = String.Empty;

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
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
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
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public static bool IsProcessRunning(string ProcessName)
        {
            Process[] LocalByName = Process.GetProcessesByName(ProcessName);
            return LocalByName.Length > 0;
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
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
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
        /// Форматирует размер файла для удобства пользователя.
        /// Файлы от 0 до 1 КБ - 1 записываются в байтах, от 1 КБ до
        /// 1 МБ - 1 - в килобайтах, от 1 МБ до 1 ГБ - 1 - в мегабайтах.
        /// </summary>
        /// <param name="InpNumber">Размер файла в байтах</param>
        /// <returns>Форматированная строка</returns>
        public static string SclBytes(long InpNumber)
        {
            // Задаём константы...
            const long B = 1024;
            const long KB = B * B;
            const long MB = B * B * B;
            const long GB = B * B * B * B;
            const string Template = "{0} {1}";

            // Проверяем на размер в байтах...
            if ((InpNumber >= 0) && (InpNumber < B)) { return String.Format(Template, InpNumber, AppStrings.AppSizeBytes); }
            // ...килобайтах...
            else if ((InpNumber >= B) && (InpNumber < KB)) { return String.Format(Template, Math.Round((float)InpNumber / B, 2), AppStrings.AppSizeKilobytes); }
            // ...мегабайтах...
            else if ((InpNumber >= KB) && (InpNumber < MB)) { return String.Format(Template, Math.Round((float)InpNumber / KB, 2), AppStrings.AppSizeMegabytes); }
            // ...гигабайтах.
            else if ((InpNumber >= MB) && (InpNumber < GB)) { return String.Format(Template, Math.Round((float)InpNumber / MB, 2), AppStrings.AppSizeGigabytes); }
            
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
        /// Определяет путь к файлу Hosts...
        /// </summary>
        /// <returns>Полный путь к Hosts...</returns>
        public static string GetHostsFileFullPath()
        {
            string Result = String.Empty;
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

            // Заменим все NULL символы на пробелы...
            while (RecvStr.IndexOf("\0") != -1)
            {
                RecvStr = RecvStr.Replace("\0", " ");
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
                    RecvStr = RecvStr.Replace(@"""", String.Empty);
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
            string Result = String.Empty;
            using (StreamReader Reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(FileName)))
            {
                Result = Reader.ReadToEnd();
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
            using (StreamReader Reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(FileName)))
            {
                while (Reader.Peek() >= 0)
                {
                    Template.Add(Reader.ReadLine());
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
            try { using (FileStream fs = File.Create(Path.Combine(DirName, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose)) { /* Nothing here. */ } } catch { return false; }
            return true;
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
                    ZBkUp.AddFiles(Files, true, String.Empty);
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
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public static void OpenWebPage(string URI)
        {
            try { Process.Start(URI); } catch (Exception Ex) { WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Открывает указанный URL в выбранном в настройках текстовом редакторе.
        /// </summary>
        /// <param name="FileName">Файл для загрузки</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public static void OpenTextEditor(string FileName)
        {
            try { Process.Start(Properties.Settings.Default.EditorBin, FileName); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Показывает выбранный файл в Проводнике Windows или другой выбранной
        /// пользователем оболочке.
        /// </summary>
        /// <param name="FileName">Файл для отображения</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public static void OpenExplorer(string FileName)
        {
            try { Process.Start(Properties.Settings.Default.ShBin, String.Format("{0} \"{1}\"", Properties.Settings.Default.ShParam, FileName)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Распаковывает архив в указанный каталог при помощи библиотеки DotNetZip
        /// с выводом прогресса в отдельном окне.
        /// </summary>
        /// <param name="ArchName">Имя архивного файла с указанием полного пути</param>
        /// <param name="DestDir">Каталог назначения</param>
        public static void ExtractFiles(string ArchName, string DestDir)
        {
            using (FrmArchWrk ArW = new FrmArchWrk(ArchName, DestDir))
            {
                ArW.ShowDialog();
            }
        }

        /// <summary>
        /// Возвращает размер файла в байтах.
        /// </summary>
        /// <param name="FileName">Имя файла с полным путём</param>
        public static long GetFileSize(string FileName)
        {
            FileInfo FI = new FileInfo(FileName);
            return FI.Length;
        }

        /// <summary>
        /// Ищет и удаляет пустые каталоги, оставшиеся после удаления файлов из них.
        /// </summary>
        /// <param name="StartDir">Каталог для выполнения очистки</param>
        public static void RemoveEmptyDirectories(string StartDir)
        {
            if (Directory.Exists(StartDir))
            {
                foreach (var Dir in Directory.GetDirectories(StartDir))
                {
                    RemoveEmptyDirectories(Dir);
                    if ((Directory.GetFiles(Dir).Length == 0) && (Directory.GetDirectories(Dir).Length == 0))
                    {
                        Directory.Delete(Dir, false);
                    }
                }
            }
        }

        /// <summary>
        /// Отображает диалоговое окно менеджера быстрой очистки.
        /// </summary>
        /// <param name="Path">Путь к каталогу очистки</param>
        /// <param name="Mask">Маска файлов, подлежащих очистке</param>
        /// <param name="LText">Текст заголовка</param>
        /// <param name="CheckBin">Имя бинарника, работа которого будет проверяться перед запуском очистки</param>
        /// <param name="ResultMsg">Текст сообщения, которое будет выдаваться по завершении очистки</param>
        /// <param name="BackUpDir">Каталог для сохранения резервных копий</param>
        /// <param name="ReadOnly">Пользователю будет запрещено изменять выбор удаляемых файлов</param>
        /// <param name="NoAuto">Включает / отключает автовыбор файлов флажками</param>
        /// <param name="Recursive">Включает / отключает рекурсивный обход</param>
        /// <param name="ForceBackUp">Включает / отключает принудительное создание резервных копий</param>
        public static void OpenCleanupWindow(List<String> Paths, string LText, string ResultMsg, string BackUpDir, string CheckBin, bool ReadOnly = false, bool NoAuto = false, bool Recursive = true, bool ForceBackUp = false)
        {
            try
            {
                if (!CoreLib.IsProcessRunning(Path.GetFileNameWithoutExtension(CheckBin))) { using (frmCleaner FCl = new frmCleaner(Paths, BackUpDir, LText, ResultMsg, ReadOnly, NoAuto, Recursive, ForceBackUp)) { FCl.ShowDialog(); } } else { MessageBox.Show(String.Format(AppStrings.PS_AppRunning, CheckBin), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Удаляет указанные файлы или каталоги с выводом прогресса.
        /// </summary>
        /// <param name="Path">Пути к каталогам или файлам для очистки</param>
        public static void RemoveFileDirectoryEx(List<String> Paths)
        {
            using (FrmRmWrk Rm = new FrmRmWrk(Paths))
            {
                Rm.ShowDialog();
            }
        }

        /// <summary>
        /// Ищет файлы по заданной маске в указанном каталоге.
        /// </summary>
        /// <param name="SearchPath">Каталог, в котором будем искать файлы</param>
        /// <param name="SrcMask">Маска файлов</param>
        /// <param name="IsRecursive">Включает / отключает рекурсивный поиск</param>
        /// <returns>Возвращает список файлов, удовлетворяющих указанной маске.</returns>
        public static List<String> FindFiles(string SearchPath, string SrcMask, bool IsRecursive = true)
        {
            List<String> Result = new List<String>();
            if (Directory.Exists(SearchPath))
            {
                DirectoryInfo DInfo = new DirectoryInfo(SearchPath);
                FileInfo[] DirList = DInfo.GetFiles(SrcMask);
                foreach (FileInfo DItem in DirList) { Result.Add(DItem.FullName); }
                if (IsRecursive) { foreach (DirectoryInfo Dir in DInfo.GetDirectories()) { Result.AddRange(FindFiles(Dir.FullName, SrcMask)); } }
            }
            return Result;
        }

        /// <summary>
        /// Выводит название компании-разработчика сборки.
        /// </summary>
        public static string GetAppCompany()
        {
            object[] Attribs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            return Attribs.Length != 0 ? ((AssemblyCompanyAttribute)Attribs[0]).Company : String.Empty;
        }

        /// <summary>
        /// Проверяет существует ли хотя бы один из файлов, указанный в списке.
        /// </summary>
        /// <param name="Configs">Список файлов с полными путями</param>
        /// <returns>Возвращает true если хотя бы один файл существует</returns>
        public static bool CheckFilesInList(List<String> Configs)
        {
            foreach (string Config in Configs)
            {
                if (File.Exists(Config))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Создаёт резервную копию конфигов, имена которых переданы в параметре.
        /// </summary>
        /// <param name="Configs">Конфиги для бэкапа</param>
        /// <param name="BackUpDir">Путь к каталогу с резервными копиями</param>
        /// <param name="Prefix">Префикс имени файла резервной копии</param>
        public static void CreateConfigBackUp(List<String> Configs, string BackUpDir, string Prefix)
        {
            // Проверяем чтобы каталог для бэкапов существовал...
            if (!(Directory.Exists(BackUpDir))) { Directory.CreateDirectory(BackUpDir); }

            // Копируем оригинальный файл в файл бэкапа...
            try { if (CheckFilesInList(Configs)) { CompressFiles(Configs, GenerateBackUpFileName(BackUpDir, Prefix)); } } catch (Exception Ex) { WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Возвращает массив для передачи в особые функции
        /// </summary>
        /// <param name="Str">Строка для создания</param>
        /// <returns>Возвращает массив</returns>
        public static List<String> SingleToArray(string Str)
        {
            List<String> Result = new List<String>();
            Result.Add(Str);
            return Result;
        }

        /// <summary>
        /// Возвращает список используемых на данном компьютере SteamID.
        /// </summary>
        /// <param name="SteamPath">Каталог установки Steam</param>
        /// <returns>Список Steam user ID</returns>
        public static List<String> GetUserIDs(string SteamPath)
        {
            // Создаём новый список...
            List<String> Result = new List<String>();

            // Получаем список каталогов...
            string DDir = Path.Combine(SteamPath, "userdata");
            if (Directory.Exists(DDir))
            {
                DirectoryInfo DInfo = new DirectoryInfo(DDir);
                foreach (DirectoryInfo SubDir in DInfo.GetDirectories())
                {
                    Result.Add(SubDir.FullName);
                }
            }

            // Возвращаем результат...
            return Result;
        }

        /// <summary>
        /// Ищет самый свежий файл в переданном списке.
        /// </summary>
        /// <param name="FileList">Список файлов с полными путями для обхода</param>
        /// <returns>Полный путь к самому свежему файлу</returns>
        public static string FindNewerestFile(List<String> FileList)
        {
            // Создаём список типа FileInfo...
            List<FileInfo> FF = new List<FileInfo>();

            // Заполняем наш список...
            foreach (string Config in FileList)
            {
                FF.Add(new FileInfo(Config));
            }

            // При помощи Linq ищем самый свежий...
            return FF.OrderByDescending(x => x.LastWriteTimeUtc).FirstOrDefault().FullName;
        }
    }
}
