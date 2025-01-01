/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System.IO;
using System.IO.Compression;
using System.Linq;

namespace srcrepair.core
{
    /// <summary>
    /// Special class with System.IO.Compression extensions.
    /// </summary>
    public static class ZipArchiveExtensions
    {
        /// <summary>
        /// Recursively adds contents of the specified directory into the Zip archive.
        /// </summary>
        /// <param name="Archive">Source Zip archive.</param>
        /// <param name="SourceDirectoryName">Path to directory.</param>
        /// <param name="EntryName">The name of the entry or prefix.</param>
        /// <param name="CompressionLevel">Compression level.</param>
        public static void CreateEntryFromDirectory(this ZipArchive Archive, string SourceDirectoryName, string EntryName, CompressionLevel CompressionLevel)
        {
            if (!Directory.Exists(SourceDirectoryName)) { throw new DirectoryNotFoundException(string.Format(DebugStrings.AppDbgCoreZipAddDirNotFound, SourceDirectoryName)); }
            foreach (string SFile in Directory.GetFiles(SourceDirectoryName).Concat(Directory.GetDirectories(SourceDirectoryName)))
            {
                if (File.GetAttributes(SFile).HasFlag(FileAttributes.Directory))
                {
                    Archive.CreateEntryFromDirectory(SFile, Path.Combine(EntryName, Path.GetFileName(SFile)), CompressionLevel);
                }
                else
                {
                    Archive.CreateEntryFromFile(SFile, Path.Combine(EntryName, Path.GetFileName(SFile)), CompressionLevel);
                }
            }
        }

        /// <summary>
        /// Recursively adds contents of the specified directory into the Zip archive.
        /// </summary>
        /// <param name="Archive">Source Zip archive.</param>
        /// <param name="SourceDirectoryName">Path to directory.</param>
        /// <param name="EntryName">The name of the entry or prefix.</param>
        public static void CreateEntryFromDirectory(this ZipArchive Archive, string SourceDirectoryName, string EntryName)
        {
            CreateEntryFromDirectory(Archive, SourceDirectoryName, EntryName, CompressionLevel.Optimal);
        }
    }
}
