﻿/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of the Options module.
    /// </summary>
    public partial class FrmOptions : Form
    {
        /// <summary>
        /// FrmOptions class constructor.
        /// </summary>
        public FrmOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the application options from the configuration file.
        /// </summary>
        private void LoadOptions()
        {
            MO_ConfirmExit.Checked = Properties.Settings.Default.ConfirmExit;
            MO_HideUnsupported.Checked = Properties.Settings.Default.HideUnsupportedGames;
            MO_RemEmptyDirs.Checked = Properties.Settings.Default.RemoveEmptyDirs;
            MO_HighlightOldBackUps.Checked = Properties.Settings.Default.HighlightOldBackUps;
            MO_TextEdBin.Text = Properties.Settings.Default.EditorBin;
            MO_CustDirName.Text = Properties.Settings.Default.UserCustDirName;
            MO_ZipCompress.Checked = Properties.Settings.Default.PackBeforeCleanup;
            MO_AutoCheckUpdates.Checked = Properties.Settings.Default.AutoUpdateCheck;
            MO_UnSafeOps.Checked = Properties.Settings.Default.AllowUnSafeCleanup;
            MO_UseUpstream.Checked = Properties.Settings.Default.HUDUseUpstream;
            MO_HideOutdatedHUDs.Checked = Properties.Settings.Default.HUDHideOutdated;
        }

        /// <summary>
        /// Saves the application options to the configuration file.
        /// </summary>
        private void SaveOptions()
        {
            Properties.Settings.Default.ConfirmExit = MO_ConfirmExit.Checked;
            Properties.Settings.Default.HideUnsupportedGames = MO_HideUnsupported.Checked;
            Properties.Settings.Default.RemoveEmptyDirs = MO_RemEmptyDirs.Checked;
            Properties.Settings.Default.HighlightOldBackUps = MO_HighlightOldBackUps.Checked;
            Properties.Settings.Default.EditorBin = MO_TextEdBin.Text;
            Properties.Settings.Default.PackBeforeCleanup = MO_ZipCompress.Checked;
            Properties.Settings.Default.AutoUpdateCheck = MO_AutoCheckUpdates.Checked;
            Properties.Settings.Default.AllowUnSafeCleanup = MO_UnSafeOps.Checked;
            Properties.Settings.Default.HUDUseUpstream = MO_UseUpstream.Checked;
            Properties.Settings.Default.HUDHideOutdated = MO_HideOutdatedHUDs.Checked;
            if (Regex.IsMatch(MO_CustDirName.Text, Properties.Resources.MO_CustomDirRegex)) { Properties.Settings.Default.UserCustDirName = MO_CustDirName.Text; }
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Shows a message and closes the form.
        /// </summary>
        private void FormFinalize()
        {
            MessageBox.Show(AppStrings.MO_OptionsSaved, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmOptions_Load(object sender, EventArgs e)
        {
            LoadOptions();
        }

        /// <summary>
        /// "OK" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MO_Okay_Click(object sender, EventArgs e)
        {
            SaveOptions();
            FormFinalize();
        }

        /// <summary>
        /// "Cancel" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MO_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// "Browse" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MO_FindTextEd_Click(object sender, EventArgs e)
        {
            if (MO_SearchBin.ShowDialog() == DialogResult.OK)
            {
                MO_TextEdBin.Text = MO_SearchBin.FileName;
            }
        }

        /// <summary>
        /// "Textbox text changed" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MO_CustDirName_TextChanged(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Regex.IsMatch(((TextBox)sender).Text, Properties.Resources.MO_CustomDirRegex) ? SystemColors.Window : Color.FromArgb(255, 155, 95);
        }
    }
}
