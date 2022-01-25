﻿/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace srcrepair.core
{
    /// <summary>
    /// Class with hacks for high pixel density displays.
    /// </summary>
    public static class DpiManager
    {
        /// <summary>
        /// Correctly scale columns width in DataGridView container on high
        /// pixel density displays.
        /// </summary>
        /// <param name="ScaleSource">DataGridView control name.</param>
        /// <param name="ScaleFactor">Scale factor value.</param>
        public static void ScaleColumnsInControl(DataGridView ScaleSource, SizeF ScaleFactor)
        {
            foreach (DataGridViewColumn Column in ScaleSource.Columns)
            {
                Column.Width = (int)Math.Round(Column.Width * ScaleFactor.Width);
            }
        }

        /// <summary>
        /// Correctly scale columns width in ListView container on high
        /// pixel density displays.
        /// </summary>
        /// <param name="ScaleSource">ListView control name.</param>
        /// <param name="ScaleFactor">Scale factor value.</param>
        public static void ScaleColumnsInControl(ListView ScaleSource, SizeF ScaleFactor)
        {
            foreach (ColumnHeader Column in ScaleSource.Columns)
            {
                Column.Width = (int)Math.Round(Column.Width * ScaleFactor.Width);
            }
        }

        /// <summary>
        /// Correctly scale columns width in StatusStrip container on high
        /// pixel density displays.
        /// </summary>
        /// <param name="ScaleSource">StatusStrip control name.</param>
        /// <param name="ScaleFactor">Scale factor value.</param>
        public static void ScaleColumnsInControl(StatusStrip ScaleSource, SizeF ScaleFactor)
        {
            foreach (ToolStripItem StatusBarItem in ScaleSource.Items)
            {
                StatusBarItem.Width = (int)Math.Round(StatusBarItem.Width * ScaleFactor.Width);
            }
        }

        /// <summary>
        /// Compare two floating point numbers.
        /// </summary>
        /// <param name="First">First floating point number.</param>
        /// <param name="Second">Second floating point number.</param>
        /// <returns>Comparison result.</returns>
        public static bool CompareFloats(float First, float Second)
        {
            return Math.Abs(First - Second) < 0.00001f;
        }
    }
}
