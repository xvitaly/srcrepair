/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
