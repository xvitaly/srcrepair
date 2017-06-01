/*
 * Класс для взаимодействия с внешними процессами.
 * 
 * Copyright 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2017 EasyCoding Team.
 * 
 * Лицензия: GPL v3 (см. файл GPL.txt).
 * Лицензия контента: Creative Commons 3.0 BY.
 * 
 * Запрещается использовать этот файл при использовании любой
 * лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
 * 
 * Официальный блог EasyCoding Team: https://www.easycoding.org/
 * Официальная страница проекта: https://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/
using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace srcrepair
{
    /// <summary>
    /// Класс для взаимодействия с внешними процессами.
    /// </summary>
    public static class ProcessManager
    {
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
        /// Запускает процесс с правами администратора посредством UAC.
        /// </summary>
        /// <param name="FileName">Путь к файлу для выполнения</param>
        /// <returns>Возвращает PID запущенного процесса.</returns>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public static int StartWithUAC(string FileName)
        {
            // Создаём объекты...
            Process p = new Process();

            // Задаём свойства...
            ProcessStartInfo ps = new ProcessStartInfo()
            {
                FileName = FileName,
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = true
            };
            p.StartInfo = ps;

            // Запускаем процесс...
            p.Start();

            // Возвращаем PID запущенного процесса...
            return p.Id;
        }

        /// <summary>
        /// Проверяет есть ли у пользователя, с правами которого запускается
        /// программа, привилегии локального администратора.
        /// </summary>
        /// <returns>Булево true если есть</returns>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
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
        /// Запускает указанное приложение и ждёт его завершения.
        /// </summary>
        /// <param name="SAppName">Путь к приложению или его имя</param>
        /// <param name="SParameters">Параметры запуска</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public static void StartProcessAndWait(string SAppName, string SParameters)
        {
            // Создаём объект с нужными параметрами...
            ProcessStartInfo ST = new ProcessStartInfo()
            {
                FileName = SAppName,
                Arguments = SParameters,
                WindowStyle = ProcessWindowStyle.Hidden
            };

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
        /// Открывает указанный URL в системном браузере по умолчанию.
        /// </summary>
        /// <param name="URI">URL для загрузки в браузере</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public static void OpenWebPage(string URI)
        {
            try { Process.Start(URI); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
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
    }
}
