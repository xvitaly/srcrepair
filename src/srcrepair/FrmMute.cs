﻿/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of the Muted players manager module.
    /// </summary>
    public partial class FrmMute : Form
    {
        /// <summary>
        /// Logger instance for the FrmMute class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// CurrentPlatform instance for the FrmMute class.
        /// </summary>
        private readonly CurrentPlatform Platform = CurrentPlatform.Create();

        /// <summary>
        /// Stores the number of bytes in the database file header.
        /// </summary>
        private readonly int HeaderLength = 4;

        /// <summary>
        /// Stores the number of bytes in each database entry.
        /// </summary>
        private readonly int ElementLength = 32;

        /// <summary>
        /// Stores the full path to the muted players database file.
        /// </summary>
        private readonly string Banlist;

        /// <summary>
        /// Stores the full path to the directory for saving backups.
        /// </summary>
        private readonly string FullBackUpDirPath;

        /// <summary>
        /// Stores the database header.
        /// </summary>
        private byte[] DatabaseHeader;

        /// <summary>
        /// FrmMute class constructor.
        /// </summary>
        /// <param name="DBFileName">Full path to the muted players database file.</param>
        /// <param name="BackUpDir">Full path to the directory for saving backups.</param>
        public FrmMute(string DBFileName, string BackUpDir)
        {
            InitializeComponent();
            Banlist = DBFileName;
            FullBackUpDirPath = BackUpDir;
        }

        /// <summary>
        /// Reads the contents of the muted players database file and
        /// renders it on the form.
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
                        string Item = Encoding.ASCII.GetString(BanlistReader.ReadBytes(ElementLength)).TrimEnd('\0');
                        if (!string.IsNullOrEmpty(Item))
                        {
                            MM_Table.Rows.Add(Item);
                        }
                    }
                    while (BanlistStream.Position < BanlistStream.Length);
                }
            }
        }

        /// <summary>
        /// Writes the contents of muted players table to the database file.
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
                        string Item = CurrentRow.Cells[0].Value.ToString();
                        if (Regex.IsMatch(Item, Properties.Resources.MM_SteamIDRegex))
                        {
                            BanlistWriter.Write(Encoding.ASCII.GetBytes(AlignString(Item)));
                        }
                        else
                        {
                            Logger.Warn(DebugStrings.AppDbgMutedPlayersWrongFormat, Item);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Aligns a string to the specified length with NUL bytes.
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
        /// Copies the contents of the selected cells to the clipboard.
        /// </summary>
        private void CopyItems()
        {
            if (MM_Table.SelectedCells.Count > 0)
            {
                Clipboard.SetDataObject(MM_Table.GetClipboardContent());
            }
        }

        /// <summary>
        /// Clears the contents of the selected cells.
        /// </summary>
        private void ClearItems()
        {
            foreach (DataGridViewCell Cell in MM_Table.SelectedCells)
            {
                if (!Cell.OwningRow.IsNewRow)
                {
                    Cell.Value = null;
                }
            }
        }

        /// <summary>
        /// Tries to find UserIDs in the clipboard and then inserts them
        /// into the table.
        /// </summary>
        private void PasteItems()
        {
            if (Clipboard.ContainsText())
            {
                string[] Items = Clipboard.GetText().Split('\n');
                for (int i = 0; i < Items.Length; i++)
                {
                    string Item = Items[i].Trim();
                    if (!Regex.IsMatch(Item, Properties.Resources.MM_SteamIDRegex)) { continue; }
                    if ((i < MM_Table.SelectedCells.Count) && !MM_Table.SelectedCells[i].OwningRow.IsNewRow)
                    {
                        MM_Table.SelectedCells[i].Value = Item;
                    }
                    else
                    {
                        MM_Table.Rows.Add(Item);
                    }
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
                if (!Cell.OwningRow.IsNewRow)
                {
                    MM_Table.Rows.RemoveAt(Cell.RowIndex);
                }
            }
        }

        /// <summary>
        /// Converts selected items from SteamID32 to SteamIDv3.
        /// </summary>
        private void ConvertItems()
        {
            foreach (DataGridViewCell Cell in MM_Table.SelectedCells)
            {
                if (Cell.OwningRow.IsNewRow || (Cell.Value == null)) { continue; }
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
        /// Opens the Steam profile page of the selected user ID.
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
        /// Creates a backup of the database file if the safe cleanup
        /// is enabled.
        /// </summary>
        private void CreateTableBackUp()
        {
            if (Properties.Settings.Default.SafeCleanup && File.Exists(Banlist))
            {
                try
                {
                    FileManager.CreateConfigBackUp(Banlist, FullBackUpDirPath, Properties.Resources.BU_PrefixVChat);
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex, DebugStrings.AppDbgExMMAutoSave);
                }
            }
        }

        /// <summary>
        /// Reads the contents of the database file.
        /// </summary>
        private void UpdateTable()
        {
            MM_Table.Rows.Clear();
            ReadFileToTable();
        }

        /// <summary>
        /// Writes the contents to the database file.
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
                Logger.Error(Ex, DebugStrings.AppDbgExMMReadDatabase);
                MessageBox.Show(AppStrings.MM_ReadDatabaseError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// "Refresh table" menu item and button click event handler.
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
                Logger.Error(Ex, DebugStrings.AppDbgExMMRefreshTable);
                MessageBox.Show(AppStrings.MM_RefreshTableError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(AppStrings.MM_DatabaseSaved, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, DebugStrings.AppDbgExMMSaveDatabase);
                MessageBox.Show(AppStrings.MM_SaveDatabaseError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// "Cut to the clipboard" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MM_Cut_Click(object sender, EventArgs e)
        {
            try
            {
                CopyItems();
                ClearItems();
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExMMCutItems);
                MessageBox.Show(AppStrings.MM_CutItemsError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Copy to the clipboard" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MM_Copy_Click(object sender, EventArgs e)
        {
            try
            {
                CopyItems();
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExMMCopyItems);
                MessageBox.Show(AppStrings.MM_CopyItemsError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// "Paste from the clipboard" button click event handler.
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
        /// "Delete selected items" button click event handler.
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
                Logger.Warn(Ex, DebugStrings.AppDbgExMMDeleteItems);
                MessageBox.Show(AppStrings.MM_DeleteItemsError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            GuiHelpers.FormShowAbout();
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
