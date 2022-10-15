/**
 * SPDX-FileCopyrightText: 2011-2022 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace srcrepair.gui.kbhelper
{
    /// <summary>
    /// Class of system keyboard keys disable window.
    /// </summary>
    public partial class FrmKBHelper : Form
    {
        /// <summary>
        /// Stores registry value name with scan code overrides.
        /// </summary>
        private const string RegValueName = "Scancode Map";

        /// <summary>
        /// Stores registry key name for storing keyboard settings.
        /// </summary>
        private const string RegKeyName = @"SYSTEM\CurrentControlSet\Control\Keyboard Layout";

        /// <summary>
        /// Stores the value of the registry key to disable only the left WIN key.
        /// </summary>
        private readonly byte[] RegLWinValue;

        /// <summary>
        /// Stores the value of the registry key to disable both WIN keys.
        /// </summary>
        private readonly byte[] RegBothWinValue;

        /// <summary>
        /// Stores the value of the registry key to disable the right WIN
        /// and the MENU keys.
        /// </summary>
        private readonly byte[] RegRWinMenuValue;

        /// <summary>
        /// Stores the value of the registry key to disable both WIN and
        /// the MENU keys.
        /// </summary>
        private readonly byte[] RegBothWinMenuValue;

        /// <summary>
        /// FrmKBHelper class constructor.
        /// </summary>
        public FrmKBHelper()
        {
            InitializeComponent();
            RegLWinValue = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5B, 0xE0, 0x00, 0x00, 0x00, 0x00 };
            RegBothWinValue = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5B, 0xE0, 0x00, 0x00, 0x5C, 0xE0, 0x00, 0x00, 0x00, 0x00 };
            RegRWinMenuValue = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5C, 0xE0, 0x00, 0x00, 0x5D, 0xE0, 0x00, 0x00, 0x00, 0x00 };
            RegBothWinMenuValue = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5B, 0xE0, 0x00, 0x00, 0x5C, 0xE0, 0x00, 0x00, 0x5D, 0xE0, 0x00, 0x00, 0x00, 0x00 };
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
        /// Installs specified keyboard settings override.
        /// </summary>
        /// <param name="Value">Bytes array with new settings.</param>
        /// <param name="Name">User-friendly setting name.</param>
        private void InstallKBS(byte[] Value, string Name)
        {
            if (MessageBox.Show(String.Format(AppStrings.KB_DisableQuestion, Name), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    WriteKBS(Value);
                    MessageBox.Show(AppStrings.KB_Success, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch
                {
                    MessageBox.Show(AppStrings.KB_ExBothWin, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Uninstalls all keyboard settings overrides.
        /// </summary>
        private void UninstallKBS()
        {
            if (MessageBox.Show(AppStrings.KB_RestoreQuestion, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                try
                {
                    DeleteKBS(RegValueName);
                    MessageBox.Show(AppStrings.KB_Success, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch
                {
                    MessageBox.Show(AppStrings.KB_ExRestore, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// "Disable left WIN" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void Dis_LWIN_Click(object sender, EventArgs e)
        {
            // 00 00 00 00 00 00 00 00 02 00 00 00 00 00 5B E0 00 00 00 00
            InstallKBS(RegLWinValue, ((Button)sender).Text.ToLowerInvariant());
        }

        /// <summary>
        /// "Disable both WIN" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void Dis_BWIN_Click(object sender, EventArgs e)
        {
            // 00 00 00 00 00 00 00 00 03 00 00 00 00 00 5B E0 00 00 5C E0 00 00 00 00
            InstallKBS(RegBothWinValue, ((Button)sender).Text.ToLowerInvariant());
        }

        /// <summary>
        /// "Disable right WIN and CONTEXT" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void Dis_RWinMnu_Click(object sender, EventArgs e)
        {
            // 00 00 00 00 00 00 00 00 03 00 00 00 00 00 5C E0 00 00 5D E0 00 00 00 00
            InstallKBS(RegRWinMenuValue, ((Button)sender).Text.ToLowerInvariant());
        }

        /// <summary>
        /// "Disable both WIN and CONTEXT" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void Dis_BWinMnu_Click(object sender, EventArgs e)
        {
            // 00 00 00 00 00 00 00 00 04 00 00 00 00 00 5B E0 00 00 5C E0 00 00 5D E0 00 00 00 00
            InstallKBS(RegBothWinMenuValue, ((Button)sender).Text.ToLowerInvariant());
        }

        /// <summary>
        /// "Restore settings" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void Dis_Restore_Click(object sender, EventArgs e)
        {
            // Restoring default settings by removing manual overrides...
            UninstallKBS();
        }
    }
}
