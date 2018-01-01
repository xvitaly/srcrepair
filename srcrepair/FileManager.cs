/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2018 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2018 EasyCoding Team.
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using Ionic.Zip;

namespace srcrepair
{
    /// <summary>
    /// Класс для работы с отдельными файлами и каталогами.
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// Проверяет наличие не-ASCII-символов в строке.
        /// </summary>
        /// <param name="Path">Путь для проверки</param>
        /// <returns>Возвращает True если не обнаружено запрещённых симолов</returns>
        public static bool CheckNonASCII(string Path)
        {
            // Проверяем строку на соответствие регулярному выражению...
            return Regex.IsMatch(Path, Properties.Resources.PathValidateRegex);
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
        /// <param name="OS">Платформа, на которой запущен плагин</param>
        /// <returns>Полный путь к Hosts...</returns>
        public static string GetHostsFileFullPath(CurrentPlatform.OSType OS)
        {
            // Создаём переменную для промежуточного хранения результата...
            string Result = String.Empty;

            // Определяем платформу, на которой запущено приложение...
            if (OS == CurrentPlatform.OSType.Windows)
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
            }
            else
            {
                // Мы под Unix, поэтому вставим стандартный путь к /etc...
                Result = "/etc";
            }

            // Сгенерируем полный путь к файлу hosts...
            Result = Path.Combine(Result, "hosts");

            // Возвращаем результат...
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
        public static DateTime Unix2DateTime(long UnixTime)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(UnixTime);
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
                try { if (File.Exists(ArchiveName)) { File.Delete(ArchiveName); } } catch (Exception E1) { CoreLib.WriteStringToLog(E1.Message); }
                CoreLib.WriteStringToLog(Ex.Message);
            }
            return File.Exists(ArchiveName);
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
        /// Ищет файлы по указанным маскам в указанных каталогах.
        /// </summary>
        /// <param name="CleanDirs">Каталоги для выполнения очистки с маской имени</param>
        /// <param name="IsRecursive">Включает / отключает рекурсивный поиск</param>
        /// <returns>Возвращает массив с именами файлов и полными путями</returns>
        public static List<String> ExpandFileList(List<String> CleanDirs, bool IsRecursive)
        {
            List<String> Result = new List<String>();
            foreach (string DirMs in CleanDirs)
            {
                string CleanDir = Path.GetDirectoryName(DirMs); string CleanMask = Path.GetFileName(DirMs);
                if (Directory.Exists(CleanDir))
                {
                    try
                    {
                        DirectoryInfo DInfo = new DirectoryInfo(CleanDir);
                        FileInfo[] DirList = DInfo.GetFiles(CleanMask);
                        foreach (FileInfo DItem in DirList) { Result.Add(DItem.FullName); }
                        if (IsRecursive) { try { List<String> SubDirs = new List<string>(); foreach (DirectoryInfo Dir in DInfo.GetDirectories()) { SubDirs.Add(Path.Combine(Dir.FullName, CleanMask)); } if (SubDirs.Count > 0) { Result.AddRange(ExpandFileList(SubDirs, true)); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); } }
                    }
                    catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                }
            }
            return Result;
        }

        /// <summary>
        /// Обходит список и возвращает только имена реально существующих файлов.
        /// </summary>
        /// <param name="Configs">Список файлов с полными путями</param>
        /// <returns>Возвращает только существующие файлы в списке</returns>
        public static List<String> GetRealFilesFromList(List<String> Configs)
        {
            // Создаём новый список...
            List<String> Result = new List<String>();

            // Обходим в цикле и проверяем существование...
            foreach (string Config in Configs)
            {
                if (File.Exists(Config))
                {
                    Result.Add(Config);
                }
            }

            // Возвращаем результат...
            return Result;
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

            // Проверим существование конфигов и запишем в список только имена реально существующих файлов...
            Configs = GetRealFilesFromList(Configs);

            // Копируем оригинальный файл в файл бэкапа...
            try { if (Configs.Count > 0) { CompressFiles(Configs, GenerateBackUpFileName(BackUpDir, Prefix)); } } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Создаёт резервную копию конфигов, имена которых переданы в параметре.
        /// </summary>
        /// <param name="Config">Конфиг для бэкапа</param>
        /// <param name="BackUpDir">Путь к каталогу с резервными копиями</param>
        /// <param name="Prefix">Префикс имени файла резервной копии</param>
        public static void CreateConfigBackUp(string Config, string BackUpDir, string Prefix)
        {
            // Создаём список...
            List<String> Configs = new List<String> { Config };

            // Выполняем...
            CreateConfigBackUp(Configs, BackUpDir, Prefix);
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
