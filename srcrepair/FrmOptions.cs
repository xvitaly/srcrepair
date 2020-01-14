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
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of settings editor window.
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
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmOptions_Load(object sender, EventArgs e)
        {
            // Reading current settings from configuration file...
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
            MO_FPSUseMirror.Checked = Properties.Settings.Default.FPSUseMirror;

            // Settig application name in window title...
            Text = String.Format(Text, Properties.Resources.AppName);
        }

        /// <summary>
        /// "OK" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MO_Okay_Click(object sender, EventArgs e)
        {
            // Storing settings...
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
            Properties.Settings.Default.FPSUseMirror = MO_FPSUseMirror.Checked;
            if (Regex.IsMatch(MO_CustDirName.Text, Properties.Resources.MO_CustomDirRegex)) { Properties.Settings.Default.UserCustDirName = MO_CustDirName.Text; }

            // Saving settings to disk...
            Properties.Settings.Default.Save();

            // Showing message and closing form...
            MessageBox.Show(AppStrings.Opts_Saved, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        /// <summary>
        /// "Cancel" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MO_Cancel_Click(object sender, EventArgs e)
        {
            // Closing form without saving anything...
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
