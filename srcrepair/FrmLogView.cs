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
using System.Windows.Forms;
using System.IO;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of log viewer window.
    /// </summary>
    public partial class FrmLogView : Form
    {
        /// <summary>
        /// Logger instance for FrmLogView class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets or sets full path to log file.
        /// </summary>
        private string LogFileName { get; set; }

        /// <summary>
        /// FrmLogView class constructor.
        /// </summary>
        /// <param name="LogFile">Full path to log file.</param>
        public FrmLogView(string LogFile)
        {
            InitializeComponent();
            LogFileName = LogFile;
        }

        /// <summary>
        /// Loads contents of text file and renders it on form.
        /// </summary>
        /// <param name="FileName">Full path to log file.</param>
        private void LoadTextFile(string FileName)
        {
            LV_LogArea.Clear();
            LV_LogArea.AppendText(File.ReadAllText(FileName));
        }

        /// <summary>
        /// Loads contents of log file and handles it.
        /// </summary>
        /// <param name="FileName">Full path to log file.</param>
        private void LoadLog(string FileName)
        {
            try
            {
                LoadTextFile(FileName);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.LV_LoadFailed, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExLvLoad);
            }
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmLogView_Load(object sender, EventArgs e)
        {
            // Reading log file contents...
            LoadLog(LogFileName);
        }

        /// <summary>
        /// "Reload file" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void LV_MenuFileReload_Click(object sender, EventArgs e)
        {
            // Re-reading log file contents...
            LoadLog(LogFileName);
        }

        /// <summary>
        /// "Exit" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void LV_MenuFileExit_Click(object sender, EventArgs e)
        {
            // Closing window...
            Close();
        }

        /// <summary>
        /// "About" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void LV_MenuHelpAbout_Click(object sender, EventArgs e)
        {
            // Show about dialog...
            GuiHelpers.FormShowAboutApp();
        }

        /// <summary>
        /// "Clear log" menu item click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void LV_MunuFileClearLog_Click(object sender, EventArgs e)
        {
            // Clearing text area...
            LV_LogArea.Clear();

            // Clearing log file...
            try
            {
                if (File.Exists(LogFileName))
                {
                    File.Delete(LogFileName);
                    FileManager.CreateFile(LogFileName);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(AppStrings.LV_ClearEx, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.Error(Ex, DebugStrings.AppDbgExLvClean);
            }
        }
    }
}
