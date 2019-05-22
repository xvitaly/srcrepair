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
using System.IO;
using System.Reflection;

namespace srcrepair.core
{
    /// <summary>
    /// Класс, предоставляющий методы для общих целей.
    /// </summary>
    public static class CoreLib
    {
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
    }
}
