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
using System.Windows.Forms;
using Microsoft.Win32;
using NLog;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of system keyboard keys disable window.
    /// </summary>
    public partial class FrmKBHelper : Form
    {
        /// <summary>
        /// Logger instance for FrmKBHelper class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Stores registry value name with scan code overrides.
        /// </summary>
        private const string RegValueName = "Scancode Map";

        /// <summary>
        /// Stores registry key name for storing keyboard settings.
        /// </summary>
        private const string RegKeyName = @"SYSTEM\CurrentControlSet\Control\Keyboard Layout";

        /// <summary>
        /// FrmKBHelper class constructor.
        /// </summary>
        public FrmKBHelper()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Writes bytes array with new keyboard overrides to registry.
        /// </summary>
        /// <param name="Value">Bytes array with new settings.</param>
        private void WriteKBS(byte[] Value)
        {
            using (RegistryKey ResKey = Registry.LocalMachine.OpenSubKey(RegKeyName, true))
            {
                ResKey.SetValue(RegValueName, Value, RegistryValueKind.Binary);
            }
        }

        /// <summary>
        /// Removes specified value from registry key with keyboard settings.
        /// </summary>
        /// <param name="Value">Value name to remove.</param>
        private void DeleteKBS(string Value)
        {
            using (RegistryKey ResKey = Registry.LocalMachine.OpenSubKey(RegKeyName, true))
            {
                ResKey.DeleteValue(Value);
            }
        }

        /// <summary>
        /// "Disable left WIN" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void Dis_LWIN_Click(object sender, EventArgs e)
        {
            // Disable left Windows button...
            // 00 00 00 00 00 00 00 00 02 00 00 00 00 00 5B E0 00 00 00 00
            if (MessageBox.Show(String.Format(AppStrings.KB_ExQuestion, ((Button)sender).Text.ToLower()), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    WriteKBS(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 91, 224, 0, 0, 0, 0 });
                    MessageBox.Show(AppStrings.KB_ExSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.KB_ExException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExKbLW);
                }
            }
        }

        /// <summary>
        /// "Disable both WIN" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void Dis_BWIN_Click(object sender, EventArgs e)
        {
            // Disable both Windows keys...
            // 00 00 00 00 00 00 00 00 03 00 00 00 00 00 5B E0 00 00 5C E0 00 00 00 00
            if (MessageBox.Show(String.Format(AppStrings.KB_ExQuestion, ((Button)sender).Text.ToLower()), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    WriteKBS(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 91, 224, 0, 0, 92, 224, 0, 0, 0, 0 });
                    MessageBox.Show(AppStrings.KB_ExSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.KB_ExException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExKbBW);
                }
            }
        }

        /// <summary>
        /// "Disable right WIN and CONTEXT" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void Dis_RWinMnu_Click(object sender, EventArgs e)
        {
            // Disable right Windows and Context keys...
            // 00 00 00 00 00 00 00 00 03 00 00 00 00 00 5C E0 00 00 5D E0 00 00 00 00
            if (MessageBox.Show(String.Format(AppStrings.KB_ExQuestion, ((Button)sender).Text.ToLower()), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    WriteKBS(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 91, 224, 0, 0, 93, 224, 0, 0, 0, 0 });
                    MessageBox.Show(AppStrings.KB_ExSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.KB_ExException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExKbRWCtx);
                }
            }
        }

        /// <summary>
        /// "Disable both WIN and CONTEXT" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void Dis_BWinMnu_Click(object sender, EventArgs e)
        {
            // Disable both Windows and Context buttons...
            // 00 00 00 00 00 00 00 00 04 00 00 00 00 00 5B E0 00 00 5C E0 00 00 5D E0 00 00 00 00
            if (MessageBox.Show(String.Format(AppStrings.KB_ExQuestion, ((Button)sender).Text.ToLower()), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    WriteKBS(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 92, 224, 0, 0, 93, 224, 0, 0, 0, 0 });
                    MessageBox.Show(AppStrings.KB_ExSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.KB_ExException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExKbBWCtx);
                }
            }
        }

        /// <summary>
        /// "Restore settings" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void Dis_Restore_Click(object sender, EventArgs e)
        {
            // Restoring default settings by removing manual overrides...
            if (MessageBox.Show(AppStrings.KB_ExRestore, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DeleteKBS(RegValueName);
                    MessageBox.Show(AppStrings.KB_ExSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(AppStrings.KB_ExException, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExKbRestore);
                }
            }
        }
    }
}
