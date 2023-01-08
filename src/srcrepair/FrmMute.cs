/**
 * SPDX-FileCopyrightText: 2011-2023 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
        /// Logger instance for FrmMute class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// CurrentPlatform instance for FrmMute class.
        /// </summary>
        private readonly CurrentPlatform Platform = CurrentPlatform.Create();

        /// <summary>
        /// Stores the number of bytes in database file header.
        /// </summary>
        private readonly int HeaderLength = 4;

        /// <summary>
        /// Stores the number of bytes in each database entry.
        /// </summary>
        private readonly int ElementLength = 32;

        /// <summary>
        /// Stores full path to muted players database file.
        /// </summary>
        private readonly string Banlist;

        /// <summary>
        /// Stores full path to game backups directory.
        /// </summary>
        private readonly string BackUpDir;

        /// <summary>
        /// Stores database header.
        /// </summary>
        private byte[] DatabaseHeader;

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
        /// <param name="ControlBounds">The bounds of the control.</param>
        protected override void ScaleControl(SizeF ScalingFactor, BoundsSpecified ControlBounds)
        {
            base.ScaleControl(ScalingFactor, ControlBounds);
            if (!DpiManager.CompareFloats(Math.Max(ScalingFactor.Width, ScalingFactor.Height), 1.0f))
            {
                DpiManager.ScaleColumnsInControl(MM_Table, ScalingFactor);
            }
        }

        /// <summary>
        /// Reads contents from muted players database file and
        /// renders it on form.
        /// </summary>
        private void ReadFileToTable()
        {
            if (File.Exists(Banlist))
            {
                using (FileStream BanlistStream = File.Open(Banlist, FileMode.Open))
                using (BinaryReader BanlistReader = new BinaryReader(BanlistStream, Encoding.ASCII, false))
                {
                    DatabaseHeader = BanlistReader.ReadBytes(HeaderLength);
                    do
                    {
                        MM_Table.Rows.Add(Encoding.ASCII.GetString(BanlistReader.ReadBytes(ElementLength)).TrimEnd('\0'));
                    }
                    while (BanlistStream.Position < BanlistStream.Length);
                }
            }
        }

        /// <summary>
        /// Writes contents of muted players to database file.
        /// </summary>
        private void WriteTableToFile()
        {
            using (FileStream BanlistStream = File.Open(Banlist, FileMode.Truncate))
            using (BinaryWriter BanlistWriter = new BinaryWriter(BanlistStream, Encoding.ASCII, false))
            {
                // Writing mandatory header...
                BanlistWriter.Write(DatabaseHeader);

                // Writing contents...
                foreach (DataGridViewRow CurrentRow in MM_Table.Rows)
                {
                    if (!CurrentRow.IsNewRow && CurrentRow.Cells[0].Value != null)
                    {
                        string Str = CurrentRow.Cells[0].Value.ToString();
                        if (Regex.IsMatch(Str, string.Format("^{0}$", Properties.Resources.MM_SteamIDRegex)))
                        {
                            BanlistWriter.Write(Encoding.ASCII.GetBytes(AlignString(Str)));
                        }
                        else
                        {
                            Logger.Warn(DebugStrings.AppDbgMutedPlayersWrongFormat, Str);
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
        private List<string> ParseRow(string Row)
        {
            List<string> Result = new List<string>();
            foreach (Match Mh in Regex.Matches(Row, Properties.Resources.MM_SteamIDRegex))
            {
                Result.Add(Mh.Value);
            }
            return Result;
        }

        /// <summary>
        /// Aligns string to specified length with NUL bytes.
        /// </summary>
        /// <param name="Source">Source string.</param>
        /// <returns>Aligned string.</returns>
        private string AlignString(string Source)
        {
            StringBuilder SB = new StringBuilder();
            SB.Append(Source);
            SB.Append('\0', ElementLength - Source.Length);
            return SB.ToString();
        }

        /// <summary>
        /// Handles copy and cut operations in table.
        /// </summary>
        /// <param name="RemoveItems">True to cut items and False to copy.</param>
        private void CopyOrCutItems(bool RemoveItems)
        {
            StringBuilder SB = new StringBuilder();
            string Delimeter = string.Empty;
            foreach (DataGridViewCell Cell in MM_Table.SelectedCells)
            {
                SB.Append(Delimeter);
                SB.Append(Cell.Value);
                if (RemoveItems) { MM_Table.Rows.RemoveAt(Cell.RowIndex); }
                Delimeter = " ";
            }
            Clipboard.SetText(SB.ToString());
        }

        /// <summary>
        /// Tries to find Steam UserIDs in clipboard and then insert
        /// them to the table.
        /// </summary>
        private void PasteItems()
        {
            if (Clipboard.ContainsText())
            {
                foreach (string Item in ParseRow(Clipboard.GetText()))
                {
                    MM_Table.Rows.Add(Item);
                }
            }
        }

        /// <summary>
        /// Deletes selected items from the table.
        /// </summary>
        private void DeleteItems()
        {
            foreach (DataGridViewCell Cell in MM_Table.SelectedCells)
            {
                MM_Table.Rows.RemoveAt(Cell.RowIndex);
            }
        }

        /// <summary>
        /// Converts selected items from SteamID32 to SteamIDv3.
        /// </summary>
        private void ConvertItems()
        {
            foreach (DataGridViewCell Cell in MM_Table.SelectedCells)
            {
                string CellText = Cell.Value.ToString();
                if (Regex.IsMatch(CellText, Properties.Resources.MM_SteamID32Regex))
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

        /// <summary>
        /// Opens Steam profile page of selected user ID.
        /// </summary>
        private void OpenItemProfile()
        {
            if (!MM_Table.CurrentRow.IsNewRow && MM_Table.CurrentCell.Value != null)
            {
                string CellText = MM_Table.CurrentCell.Value.ToString();
                Platform.OpenWebPage(string.Format(Properties.Resources.MM_CommunityURL, Regex.IsMatch(CellText, Properties.Resources.MM_SteamID32Regex) ? SteamConv.ConvSid32Sid64(CellText) : SteamConv.ConvSidv3Sid64(CellText)));
            }
        }

        /// <summary>
        /// Creates backup of database file if safe cleanup is enabled.
        /// </summary>
        private void CreateTableBackUp()
        {
            if (File.Exists(Banlist) && Properties.Settings.Default.SafeCleanup)
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

        /// <summary>
        /// Reads elements from a database file.
        /// </summary>
        private void UpdateTable()
        {
            MM_Table.Rows.Clear();
            ReadFileToTable();
        }

        /// <summary>
        /// Saves current contents to a database file.
        /// </summary>
        private void WriteTable()
        {
            CreateTableBackUp();
            WriteTableToFile();
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmMute_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateTable();
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, DebugStrings.AppDbgExMMReadDb);
                MessageBox.Show(AppStrings.MM_ExceptionDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Update table" menu item and button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MM_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateTable();
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, DebugStrings.AppDbgExMMRefresh);
                MessageBox.Show(AppStrings.MM_RefreshError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Save database" menu item and button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MM_Save_Click(object sender, EventArgs e)
        {
            try
            {
                WriteTable();
                MessageBox.Show(AppStrings.MM_SavedOK, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, DebugStrings.AppDbgExMMSaveDb);
                MessageBox.Show(AppStrings.MM_SaveException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                CopyOrCutItems(true);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExMMCutItems);
                MessageBox.Show(AppStrings.MM_CutItemsError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                CopyOrCutItems(false);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExMMCopyItems);
                MessageBox.Show(AppStrings.MM_CopyItemsError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                PasteItems();
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExMMPasteItems);
                MessageBox.Show(AppStrings.MM_PasteItemsError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                DeleteItems();
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExMMRemoveItems);
                MessageBox.Show(AppStrings.MM_RemoveItemsError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                ConvertItems();
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExMMConvertItems);
                MessageBox.Show(AppStrings.MM_ConvertItemsError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                OpenItemProfile();
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExMMOpenItemProfile);
                MessageBox.Show(AppStrings.MM_OpenItemProfileError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
        /// "About" menu item and button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MM_About_Click(object sender, EventArgs e)
        {
            GuiHelpers.FormShowAboutApp();
        }

        /// <summary>
        /// "Selection changed" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MM_Table_SelectionChanged(object sender, EventArgs e)
        {
            bool SelectionState = MM_Table.SelectedCells.Count <= 1;
            MM_Steam.Enabled = SelectionState;
            MM_CSteam.Enabled = SelectionState;
        }
    }
}
