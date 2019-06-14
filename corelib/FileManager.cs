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
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with files and directories.
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// Checks for non-ASCII characters in string.
        /// </summary>
        /// <param name="Path">Source string to check.</param>
        /// <returns>Returns True if string doesn't contains any restricted characters.</returns>
        public static bool CheckNonASCII(string Path)
        {
            // Use regular expression to check source string...
            return Regex.IsMatch(Path, Properties.Resources.PathValidateRegex);
        }

        /// <summary>
        /// Creates a new empty file with specified path. Will
        /// automatically create directories when needed.
        /// </summary>
        /// <param name="FileName">Full path to file.</param>
        public static void CreateFile(string FileName)
        {
            try
            {
                // Generating full path to destination directory...
                string Dir = Path.GetDirectoryName(FileName);

                // Check if destination directory exists. If not - creating...
                if (!(Directory.Exists(Dir))) { Directory.CreateDirectory(Dir); }

                // Creating a mew empty file...
                using (FileStream fs = File.Create(FileName)) { }
            }
            catch { /* Do nothing */ }
        }

        /// <summary>
        /// Calculates MD5 hash of specified file.
        /// </summary>
        /// <param name="FileName">Full path to source file.</param>
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
        /// Return platform-dependent path to Hosts file.
        /// </summary>
        /// <param name="OS">Running platform ID.</param>
        /// <returns>Full path to Hosts file.</returns>
        public static string GetHostsFileFullPath(CurrentPlatform.OSType OS)
        {
            // Creating an empty string...
            string Result = String.Empty;

            // Checking of running platform...
            if (OS == CurrentPlatform.OSType.Windows)
            {
                try
                {
                    // Getting full Hosts path from Windows Registry (can be overrided by some applications)...
                    RegistryKey ResKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", false);
                    if (ResKey != null) { Result = (string)ResKey.GetValue("DataBasePath"); }
                    
                    // Checking result. If empty, using generic...
                    if (String.IsNullOrWhiteSpace(Result)) { Result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "drivers", "etc"); }
                }
                catch
                {
                    // Exception occured. Generating Hosts file path using generic patterns...
                    Result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "drivers", "etc");
                }
            }
            else
            {
                // Unix detected, returning standard POSIX path...
                Result = "/etc";
            }

            // Generating full file name...
            Result = Path.Combine(Result, "hosts");

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Get file system on specified drive or mount point.
        /// </summary>
        /// <param name="CDrive">Drive letter or mount point path.</param>
        /// <returns>File system name or Unknown if we cannot detect it.</returns>
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
        /// Checks if specified directory writable.
        /// </summary>
        /// <param name="DirName">Full directory path.</param>
        /// <returns>Return True if directory writable.</returns>
        public static bool IsDirectoryWritable(string DirName)
        {
            try { using (FileStream fs = File.Create(Path.Combine(DirName, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose)) { /* Nothing here. */ } } catch { return false; }
            return true;
        }

        /// <summary>
        /// Converts date from DateTime format to Unixtime.
        /// </summary>
        /// <param name="DTime">Date in DateTime format.</param>
        /// <returns>Date in UnixTime format.</returns>
        public static string DateTime2Unix(DateTime DTime)
        {
            return Math.Round((DTime - new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime()).TotalSeconds, 0).ToString();
        }

        /// <summary>
        /// Converts date from Unixtime to DateTime format.
        /// </summary>
        /// <param name="UnixTime">Date in Unixtime format.</param>
        /// <returns>Date in DateTime format.</returns>
        public static DateTime Unix2DateTime(long UnixTime)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(UnixTime);
        }

        /// <summary>
        /// Generated full unique file name for backup file.
        /// </summary>
        /// <param name="BackUpDir">Full path to directory for saving backups.</param>
        /// <param name="Prefix">Backup file pre-defined prefix.</param>
        /// <returns>Full backup file name.</returns>
        public static string GenerateBackUpFileName(string BackUpDir, string Prefix)
        {
            return Path.Combine(BackUpDir, String.Format("{0}_{1}.bud", Prefix, DateTime2Unix(DateTime.Now)));
        }

        /// <summary>
        /// Compresses specified files to a single Zip archive.
        /// </summary>
        /// <param name="Files">List with full file names to be compressed.</param>
        /// <param name="ArchiveName">Archive name with full path.</param>
        /// <returns>Return True if archive was created successfully.</returns>
        public static bool CompressFiles(List<String> Files, string ArchiveName)
        {
            try
            {
                using (FileStream ZipStream = new FileStream(ArchiveName, FileMode.Create))
                {
                    using (ZipArchive ZipArch = new ZipArchive(ZipStream, ZipArchiveMode.Create))
                    {
                        foreach (string SFile in Files)
                        {
                            ZipArch.CreateEntryFromFile(SFile, Path.GetFullPath(SFile), CompressionLevel.Optimal);
                        }
                    }
                }
            }
            catch
            {
                if (File.Exists(ArchiveName)) { File.Delete(ArchiveName); }
                return false;
            }
            return File.Exists(ArchiveName);
        }

        /// <summary>
        /// Returns size in bytes of specified file.
        /// </summary>
        /// <param name="FileName">Full file path.</param>
        /// <returns>File size in bytes.</returns>
        public static long GetFileSize(string FileName)
        {
            FileInfo FI = new FileInfo(FileName);
            return FI.Length;
        }

        /// <summary>
        /// Finds and removes empty directories in specified directory.
        /// </summary>
        /// <param name="StartDir">Full path to start directory.</param>
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
        /// Finds files by specified mask in specified directory.
        /// </summary>
        /// <param name="SearchPath">Start directory.</param>
        /// <param name="SrcMask">File mask (wildcards are supported).</param>
        /// <param name="IsRecursive">Use recursive (include subdirectories) search.</param>
        /// <returns>List of files with full paths, matches mask.</returns>
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
        /// Finds files by specified mask in specified directories. Mask must be
        /// added to the end of path.
        /// </summary>
        /// <param name="CleanDirs">List of directories with masks.</param>
        /// <param name="IsRecursive">Use recursive (include subdirectories) search.</param>
        /// <returns>List of files with full paths, matches mask.</returns>
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
                        if (IsRecursive) { try { List<String> SubDirs = new List<string>(); foreach (DirectoryInfo Dir in DInfo.GetDirectories()) { SubDirs.Add(Path.Combine(Dir.FullName, CleanMask)); } if (SubDirs.Count > 0) { Result.AddRange(ExpandFileList(SubDirs, true)); } } catch { } }
                    }
                    catch { }
                }
            }
            return Result;
        }

        /// <summary>
        /// Finds and returns only existing files from specified list.
        /// </summary>
        /// <param name="Configs">List of files with full paths to check.</param>
        /// <returns>List with existing files.</returns>
        public static List<String> GetRealFilesFromList(List<String> Configs)
        {
            // Creating a new empty list...
            List<String> Result = new List<String>();

            // Using loop to check a single item from source list...
            foreach (string Config in Configs)
            {
                if (File.Exists(Config))
                {
                    Result.Add(Config);
                }
            }

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Creates a backup copy in Zip archive of specified files.
        /// </summary>
        /// <param name="Configs">List of files to be backed up.</param>
        /// <param name="BackUpDir">Full path to directory for saving backups.</param>
        /// <param name="Prefix">Backup file pre-defined prefix.</param>
        public static void CreateConfigBackUp(List<String> Configs, string BackUpDir, string Prefix)
        {
            // Checking if destination directory exists...
            if (!(Directory.Exists(BackUpDir))) { Directory.CreateDirectory(BackUpDir); }

            // Generating a new list only with existing files...
            Configs = GetRealFilesFromList(Configs);

            // Running backup sequence only if we have at least one candidate...
            if (Configs.Count > 0)
            {
                CompressFiles(Configs, GenerateBackUpFileName(BackUpDir, Prefix));
            }
        }

        /// <summary>
        /// Creates a backup copy in Zip archive of specified file.
        /// </summary>
        /// <param name="Config">Full path to file to be backed up.</param>
        /// <param name="BackUpDir">Full path to directory for saving backups.</param>
        /// <param name="Prefix">Backup file pre-defined prefix.</param>
        public static void CreateConfigBackUp(string Config, string BackUpDir, string Prefix)
        {
            // Adding only one file to list...
            List<String> Configs = new List<String> { Config };

            // Running another overloaded version of this method...
            CreateConfigBackUp(Configs, BackUpDir, Prefix);
        }

        /// <summary>
        /// Finds and returns the newerest file from specified list.
        /// </summary>
        /// <param name="FileList">List of files for check.</param>
        /// <returns>Full path to the newerest file.</returns>
        public static string FindNewerestFile(List<String> FileList)
        {
            // Creating a new list of FileInfo...
            List<FileInfo> FF = new List<FileInfo>();

            // Adding FileInfo objects to our list...
            foreach (string Config in FileList)
            {
                FF.Add(new FileInfo(Config));
            }

            // Using LINQ to find the newerest file...
            return FF.OrderByDescending(x => x.LastWriteTimeUtc).FirstOrDefault().FullName;
        }
    }
}
