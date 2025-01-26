/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

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
            // Generating full path to the destination directory...
            string DirName = Path.GetDirectoryName(FileName);

            // Check if the destination directory exists. If not - creating...
            if (!Directory.Exists(DirName)) { Directory.CreateDirectory(DirName); }

            // Creating a new empty file...
            using (File.Create(FileName)) { /* Nothing here. */ }
        }

        /// <summary>
        /// Extracts string from array of bytes.
        /// </summary>
        /// <param name="Source">Source array.</param>
        /// <returns>Result string.</returns>
        private static string ConvertBytesToString(byte[] Source)
        {
            return BitConverter.ToString(Source).Replace("-", string.Empty).ToLowerInvariant();
        }

        /// <summary>
        /// Calculates SHA-512 hash of specified file.
        /// </summary>
        /// <param name="FileName">Full path to source file.</param>
        /// <returns>Returns SHA-512 hash of specified file.</returns>
        public static string CalculateFileSHA512(string FileName)
        {
            using (SHA512 SHA512Crypt = SHA512.Create())
            {
                using (FileStream SourceStream = File.OpenRead(FileName))
                {
                    return ConvertBytesToString(SHA512Crypt.ComputeHash(SourceStream));
                }
            }
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
            foreach (DriveInfo Dr in Drives.Where(e => string.Compare(e.Name, CDrive, true) == 0))
            {
                Result = Dr.DriveFormat;
                break;
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
            try { using (File.Create(Path.Combine(DirName, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose)) { /* Nothing here. */ } } catch { return false; }
            return true;
        }

        /// <summary>
        /// Converts date from DateTime format to Unixtime.
        /// </summary>
        /// <param name="DTime">Date in DateTime format.</param>
        /// <returns>Date in UnixTime format.</returns>
        public static string DateTime2Unix(DateTime DTime)
        {
            return Math.Round((DTime - new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime()).TotalSeconds, 0).ToString(CultureInfo.InvariantCulture);
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
            return Path.Combine(BackUpDir, string.Format("{0}_{1}.bud", Prefix, DateTime2Unix(DateTime.Now)));
        }

        /// <summary>
        /// Compresses specified files to a single Zip archive with preserving
        /// absolute paths.
        /// </summary>
        /// <param name="Files">List with full file names to be compressed.</param>
        /// <param name="ArchiveName">Archive name with full path.</param>
        /// <returns>Return True if archive was created successfully.</returns>
        public static bool CompressFiles(List<string> Files, string ArchiveName)
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
                RemoveFile(ArchiveName);
                return false;
            }
            return File.Exists(ArchiveName);
        }

        /// <summary>
        /// Compresses all files in a specified directory.
        /// </summary>
        /// <param name="DirectoryPath">Full path to source directory.</param>
        /// <param name="ArchiveName">Archive name with full path.</param>
        /// <returns>Return True if archive was created successfully.</returns>
        public static bool CompressDirectory(string DirectoryPath, string ArchiveName)
        {
            return CompressFiles(FindFiles(DirectoryPath), ArchiveName);
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
        /// Finds and removes empty directories in the specified directory.
        /// </summary>
        /// <param name="StartDir">Full path to start directory.</param>
        public static void RemoveEmptyDirectories(string StartDir)
        {
            if (Directory.Exists(StartDir))
            {
                foreach (string SubDir in Directory.EnumerateDirectories(StartDir))
                {
                    RemoveEmptyDirectories(SubDir);
                    if (!Directory.EnumerateFileSystemEntries(SubDir).Any())
                    {
                        Directory.Delete(SubDir, false);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the file exists and deletes it.
        /// </summary>
        /// <param name="FileName">File for deletion.</param>
        public static void RemoveFile(string FileName)
        {
            if (File.Exists(FileName))
            {
                File.SetAttributes(FileName, FileAttributes.Normal);
                File.Delete(FileName);
            }
        }

        /// <summary>
        /// Removes all existing files from the list.
        /// </summary>
        /// <param name="FileNames">The list of files for deletion.</param>
        public static void RemoveFiles(List<string> FileNames)
        {
            foreach (string FileName in FileNames.Where(e => File.Exists(e)))
            {
                File.SetAttributes(FileName, FileAttributes.Normal);
                File.Delete(FileName);
            }
        }

        /// <summary>
        /// Finds files in the specified list of directories that match the
        /// specified file mask and adds them to the list. If allowed, the
        /// list may also contain files, and if they exist, they will be
        /// added to the list too. Recursion can be explicitly enabled or
        /// disabled.
        /// </summary>
        /// <param name="SearchPaths">The list of directories and files for searching.</param>
        /// <param name="SrcMask">File mask (wildcards are supported).</param>
        /// <param name="IsRecursive">Use recursive (include subdirectories) search.</param>
        /// <param name="AllowFiles">Allow files in the search list.</param>
        /// <returns>The list of files with full paths.</returns>
        private static List<string> FindFiles(List<string> SearchPaths, string SrcMask, bool IsRecursive, bool AllowFiles)
        {
            List<string> Result = new List<string>();
            foreach (string CleanCnd in SearchPaths)
            {
                if (Directory.Exists(CleanCnd))
                {
                    foreach (string DItem in Directory.EnumerateFiles(CleanCnd, SrcMask, IsRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                    {
                        Result.Add(DItem);
                    }
                }
                else
                {
                    if (AllowFiles && File.Exists(CleanCnd))
                    {
                        Result.Add(CleanCnd);
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// Recursively finds all files in the specified list of directories and
        /// adds them to the list. The list can also contain files, and if they
        /// exist, they will be added too.
        /// </summary>
        /// <param name="SearchPaths">The list of directories and files for searching.</param>
        /// <returns>The list of files with full paths.</returns>
        public static List<string> FindFiles(List<string> SearchPaths)
        {
            return FindFiles(SearchPaths, "*.*", true, true);
        }

        /// <summary>
        /// Recursively finds files in the specified list of directories that match
        /// the specified file mask and adds them to the list. The list can also
        /// contain files, and if they exist, they will be added too.
        /// </summary>
        /// <param name="SearchPaths">The list of directories and files for searching.</param>
        /// <param name="SrcMask">File mask (wildcards are supported).</param>
        /// <returns>The list of files with full paths.</returns>
        public static List<string> FindFiles(List<string> SearchPaths, string SrcMask)
        {
            return FindFiles(SearchPaths, SrcMask, true, true);
        }

        /// <summary>
        /// Recursively finds all files in the specified directory and adds them to
        /// the list.
        /// </summary>
        /// <param name="SearchPath">Start directory.</param>
        /// <returns>The list of files with full paths.</returns>
        public static List<string> FindFiles(string SearchPath)
        {
            return FindFiles(new List<string> { SearchPath }, "*.*", true, false);
        }

        /// <summary>
        /// Recursively finds files in the specified list of directories that match
        /// the specified file mask and adds them to the list.
        /// </summary>
        /// <param name="SearchPath">Start directory.</param>
        /// <param name="SrcMask">File mask (wildcards are supported).</param>
        /// <returns>List of files with full paths, matches mask.</returns>
        public static List<string> FindFiles(string SearchPath, string SrcMask)
        {
            return FindFiles(new List<string> { SearchPath }, SrcMask, true, false);
        }

        /// <summary>
        /// Finds and returns only existing files from specified list.
        /// </summary>
        /// <param name="Configs">List of files with full paths to check.</param>
        /// <returns>List with existing files.</returns>
        public static List<string> GetRealFilesFromList(List<string> Configs)
        {
            // Creating a new empty list...
            List<string> Result = new List<string>();

            // Using loop to check a single item from source list...
            foreach (string Config in Configs.Where(e => File.Exists(e)))
            {
                Result.Add(Config);
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
        public static void CreateConfigBackUp(List<string> Configs, string BackUpDir, string Prefix)
        {
            // Checking if destination directory exists...
            if (!Directory.Exists(BackUpDir)) { Directory.CreateDirectory(BackUpDir); }

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
            // Running another overloaded version of this method...
            CreateConfigBackUp(new List<string> { Config }, BackUpDir, Prefix);
        }

        /// <summary>
        /// Finds and returns the newerest file from specified list.
        /// </summary>
        /// <param name="FileList">List of files for check.</param>
        /// <returns>Full path to the newerest file.</returns>
        public static string FindNewerestFile(List<string> FileList)
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

        /// <summary>
        /// Moves all source directory contents to destination. Will overwrite files
        /// with the same names.
        /// </summary>
        /// <param name="Source">Source directory.</param>
        /// <param name="Destination">Destination directory.</param>
        public static void MoveDirectoryContents(string Source, string Destination)
        {
            // Creating destination directory if does not exists...
            if (!Directory.Exists(Destination))
            {
                Directory.CreateDirectory(Destination);
            }

            // Enumerating all files from old location...
            foreach (string SingleFile in Directory.EnumerateFiles(Source))
            {
                // Generating new file name...
                string FinalDest = Path.Combine(Destination, Path.GetFileName(SingleFile));

                // Removing existing file if exists...
                if (File.Exists(FinalDest))
                {
                    File.Delete(FinalDest);
                }

                // Moving file to destination...
                File.Move(SingleFile, FinalDest);
            }
        }

        /// <summary>
        /// Corrects directory separator characters in the source string
        /// depending on the current platform.
        /// </summary>
        /// <param name="Source">Source string.</param>
        /// <returns>Fixed string.</returns>
        public static string NormalizeDirectorySeparators(string Source)
        {
            return Source.Replace('/', Path.DirectorySeparatorChar);
        }
    }
}
