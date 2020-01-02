/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2020 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2020 EasyCoding Team.
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
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of editor of muted players window.
    /// </summary>
    public partial class FrmMute : Form
    {
        /// <summary>
        /// FrmMute class constructor.
        /// </summary>
        /// <param name="BL">Full path to muted players database file.</param>
        /// <param name="BD">Full path to game backups directory.</param>
        public FrmMute(string BL, string BD)
        {
            InitializeComponent();
            Banlist = BL;
            BackUpDir = BD;
        }

        /// <summary>
        /// Scales controls on current form with some additional hacks applied.
        /// </summary>
        /// <param name="ScalingFactor">Scaling factor.</param>
        /// <param name="Bounds">Bounds of control.</param>
        protected override void ScaleControl(SizeF ScalingFactor, BoundsSpecified Bounds)
        {
            base.ScaleControl(ScalingFactor, Bounds);
            if (!DpiManager.CompareFloats(Math.Max(ScalingFactor.Width, ScalingFactor.Height), 1.0f))
            {
                DpiManager.ScaleColumnsInControl(MM_Table, ScalingFactor);
            }
        }

        #region IV
        /// <summary>
        /// Logger instance for FrmMute class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets or sets full path to muted players database file.
        /// </summary>
        private string Banlist { get; set; }

        /// <summary>
        /// Gets or sets full path to game backups directory.
        /// </summary>
        private string BackUpDir { get; set; }
        #endregion

        #region IM
        /// <summary>
        /// Reads contents from muted players database file and
        /// renders it on form.
        /// </summary>
        private void ReadFileToTable()
        {
            if (File.Exists(Banlist))
            {
                using (StreamReader OpenedHosts = new StreamReader(Banlist, Encoding.Default))
                {
                    while (OpenedHosts.Peek() >= 0)
                    {
                        List<String> Res = ParseRow(StringsManager.CleanString(OpenedHosts.ReadLine()));
                        foreach (string Str in Res)
                        {
                            MM_Table.Rows.Add(Str);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Writes contents of muted players to database file.
        /// </summary>
        private void WriteTableToFile()
        {
            using (StreamWriter CFile = new StreamWriter(Banlist, false, Encoding.Default))
            {
                // Writing mandatory header...
                CFile.Write("\x01\x00\x00\x00");
                
                for (int i = 0; i < MM_Table.Rows.Count - 1; i++)
                {
                    if (MM_Table.Rows[i].Cells[0].Value != null)
                    {
                        string Str = MM_Table.Rows[i].Cells[0].Value.ToString();
                        if (Regex.IsMatch(Str, String.Format("^{0}$", Properties.Resources.MM_SteamIDRegex)))
                        {
                            // Building result with using NULL bytes for aligning...
                            StringBuilder SB = new StringBuilder();
                            SB.Append(Str);
                            SB.Append('\0', 32 - Str.Length);
                            CFile.Write(SB);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Extracts valid SteamIDs from source string.
        /// </summary>
        /// <param name="Row">Source string.</param>
        /// <returns>List of valid SteamIDs.</returns>
        private List<String> ParseRow(string Row)
        {
            List<String> Result = new List<String>();
            foreach (Match Mh in Regex.Matches(Row, Properties.Resources.MM_SteamIDRegex))
            {
                Result.Add(Mh.Value);
            }
            return Result;
        }

        /// <summary>
        /// "Update table" menu item and button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void UpdateTable(object sender, EventArgs e)
        {
            try
            {
                MM_Table.Rows.Clear();
                ReadFileToTable();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.MM_ExceptionDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExMMReadDb);
            }
        }

        /// <summary>
        /// "Save database" menu item and button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void WriteTable(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.SafeCleanup)
                {
                    if (File.Exists(Banlist))
                    {
                        try
                        {
                            FileManager.CreateConfigBackUp(Banlist, BackUpDir, Properties.Resources.BU_PrefixVChat);
                        }
                        catch (Exception Ex)
                        {
                            Logger.Warn(Ex, DebugStrings.AppDbgExMMAutoSave);
                        }
                    }
                }
                WriteTableToFile();
                MessageBox.Show(AppStrings.MM_SavedOK, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.MM_SaveException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExMMSaveDb);
            }
        }

        /// <summary>
        /// "About" menu item and button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void AboutDlg(object sender, EventArgs e)
        {
            GuiHelpers.FormShowAboutApp();
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmMute_Load(object sender, EventArgs e)
        {
            UpdateTable(sender, e);
        }

        /// <summary>
        /// "Exit" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MM_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// "Cut to clipboard" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MM_Cut_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                foreach (DataGridViewCell Cell in MM_Table.SelectedCells)
                {
                    if (Cell.Selected)
                    {
                        SB.AppendFormat("{0} ", Cell.Value);
                        MM_Table.Rows.RemoveAt(Cell.RowIndex);
                    }
                }
                Clipboard.SetText(SB.ToString());
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
            }
        }

        /// <summary>
        /// "Copy to clipboard" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MM_Copy_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                foreach (DataGridViewCell Cell in MM_Table.SelectedCells)
                {
                    if (Cell.Selected)
                    {
                        SB.AppendFormat("{0} ", Cell.Value);
                    }
                }
                Clipboard.SetText(SB.ToString());
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
            }
        }

        /// <summary>
        /// "Paste from clipboard" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MM_Paste_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    foreach (string Row in ParseRow(Clipboard.GetText()))
                    {
                        MM_Table.Rows.Add(Row);
                    }
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
            }
            
        }

        /// <summary>
        /// "Delete row" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MM_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewCell Cell in MM_Table.SelectedCells)
                {
                    if (Cell.Selected)
                    {
                        MM_Table.Rows.RemoveAt(Cell.RowIndex);
                    }
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
            }
        }

        /// <summary>
        /// "Convert SteamID format" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MM_Convert_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewCell Cell in MM_Table.SelectedCells)
                {
                    string CellText = Cell.Value.ToString();
                    if (Cell.Selected && Regex.IsMatch(CellText, Properties.Resources.MM_SteamID32Regex))
                    {
                        Cell.Value = SteamConv.ConvSid32Sidv3(CellText);
                    }
                    else
                    {
                        if (MM_Table.SelectedCells.Count == 1)
                        {
                            MessageBox.Show(AppStrings.MM_ConvRest, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
            }
        }

        /// <summary>
        /// "Show profile page" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MM_Steam_Click(object sender, EventArgs e)
        {
            try
            {
                if (MM_Table.Rows[MM_Table.CurrentRow.Index].Cells[MM_Table.CurrentCell.ColumnIndex].Value != null)
                {
                    string Value = MM_Table.Rows[MM_Table.CurrentRow.Index].Cells[MM_Table.CurrentCell.ColumnIndex].Value.ToString();
                    ProcessManager.OpenWebPage(String.Format(Properties.Resources.MM_CommunityURL, Regex.IsMatch(Value, Properties.Resources.MM_SteamID32Regex) ? SteamConv.ConvSid32Sid64(Value) : SteamConv.ConvSidv3Sid64(Value)));
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
            }
        }
        #endregion
    }
}
