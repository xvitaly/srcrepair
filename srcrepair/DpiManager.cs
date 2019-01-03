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
using System.Drawing;
using System.Windows.Forms;

namespace srcrepair
{
    /// <summary>
    /// Класс для работы с экранами с высокой плотностью пикселей.
    /// </summary>
    public static class DpiManager
    {
        /// <summary>
        /// Изменяет размеры столбцов в DataGridView, т.к. сама платформа CLR
        /// не способна сделать это автоматически.
        /// </summary>
        /// <param name="ScaleSource">Ссылка на контрол DataGridView</param>
        /// <param name="ScaleFactor">Множитель</param>
        public static void ScaleColumnsInControl(DataGridView ScaleSource, SizeF ScaleFactor)
        {
            foreach (DataGridViewColumn Column in ScaleSource.Columns)
            {
                Column.Width = (int)Math.Round(Column.Width * ScaleFactor.Width);
            }
        }

        /// <summary>
        /// Изменяет размеры столбцов в ListView, т.к. сама платформа CLR
        /// не способна сделать это автоматически.
        /// </summary>
        /// <param name="ScaleSource">Ссылка на контрол ListView</param>
        /// <param name="ScaleFactor">Множитель</param>
        public static void ScaleColumnsInControl(ListView ScaleSource, SizeF ScaleFactor)
        {
            foreach (ColumnHeader Column in ScaleSource.Columns)
            {
                Column.Width = (int)Math.Round(Column.Width * ScaleFactor.Width);
            }
        }

        /// <summary>
        /// Изменяет размеры столбцов в StatusStrip, т.к. сама платформа CLR
        /// не способна сделать это автоматически.
        /// </summary>
        /// <param name="ScaleSource">Ссылка на контрол StatusStrip</param>
        /// <param name="ScaleFactor">Множитель</param>
        public static void ScaleColumnsInControl(StatusStrip ScaleSource, SizeF ScaleFactor)
        {
            foreach (ToolStripItem StatusBarItem in ScaleSource.Items)
            {
                StatusBarItem.Width = (int)Math.Round(StatusBarItem.Width * ScaleFactor.Width);
            }
        }
    }
}
