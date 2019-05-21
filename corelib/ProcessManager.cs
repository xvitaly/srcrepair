/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2019 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2019 EasyCoding Team.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Security.Principal;

namespace srcrepair.core
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
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
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
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
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
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
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
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
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
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
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
            NewProcess.WaitForExit();
        }

        /// <summary>
        /// Запускает указанное приложение и ждёт завершения с передачей
        /// его вывода.
        /// </summary>
        /// <param name="SAppName">Путь к приложению или его имя</param>
        /// <param name="SParameters">Параметры запуска</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static string StartProcessWithStdOut(string SAppName, string SParameters)
        {
            // Создаём переменную для хранения результата...
            string Result = String.Empty;

            // Создаём объект с нужными параметрами...
            ProcessStartInfo ST = new ProcessStartInfo()
            {
                FileName = SAppName,
                Arguments = SParameters,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            // Запускаем процесс...
            Process NewProcess = Process.Start(ST);

            // Читаем stdout запущенного процеса...
            Result = NewProcess.StandardOutput.ReadToEnd();

            // Ждём завершения процесса...
            NewProcess.WaitForExit();

            // Возвращаем результат...
            return Result;
        }

        /// <summary>
        /// Открывает указанный URL в системном браузере по умолчанию.
        /// </summary>
        /// <param name="URI">URL для загрузки в браузере</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static void OpenWebPage(string URI)
        {
            Process.Start(URI);
        }

        /// <summary>
        /// Открывает указанный URL в выбранном в настройках текстовом редакторе.
        /// </summary>
        /// <param name="FileName">Файл для загрузки</param>
        /// <param name="OS">Код платформы, на которой запущено приложение</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static void OpenTextEditor(string FileName, CurrentPlatform.OSType OS)
        {
            switch (OS)
            {
                case CurrentPlatform.OSType.Windows:
                    Process.Start(Properties.Settings.Default.EditorBin, FileName);
                    break;
                case CurrentPlatform.OSType.Linux:
                    Process.Start(Properties.Resources.AppOpenHandlerLin, FileName);
                    break;
                case CurrentPlatform.OSType.MacOSX:
                    Process.Start(Properties.Resources.AppOpenHandlerMac, String.Format("{0} \"{1}\"", "-t", FileName));
                    break;
            }
        }

        /// <summary>
        /// Показывает выбранный файл в Проводнике Windows или другой выбранной
        /// пользователем оболочке.
        /// </summary>
        /// <param name="FileName">Файл для отображения</param>
        /// <param name="OS">Код платформы, на которой запущено приложение</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static void OpenExplorer(string FileName, CurrentPlatform.OSType OS)
        {
            switch (OS)
            {
                case CurrentPlatform.OSType.Windows:
                    Process.Start(Properties.Resources.ShBinWin, String.Format("{0} \"{1}\"", Properties.Resources.ShParamWin, FileName));
                    break;
                case CurrentPlatform.OSType.Linux:
                    Process.Start(Properties.Resources.AppOpenHandlerLin, String.Format("\"{0}\"", Path.GetDirectoryName(FileName)));
                    break;
                case CurrentPlatform.OSType.MacOSX:
                    Process.Start(Properties.Resources.AppOpenHandlerMac, String.Format("\"{0}\"", Path.GetDirectoryName(FileName)));
                    break;
            }
        }
    }
}
