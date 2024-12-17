/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of the config file selection module.
    /// </summary>
    public partial class FrmCfgSelector : Form
    {
        /// <summary>
        /// Gets or sets full path to selected by user config file.
        /// </summary>
        public string Config { get; private set; }

        /// <summary>
        /// Gets or sets list of available configs.
        /// </summary>
        private List<string> Configs { get; set; }

        /// <summary>
        /// FrmCfgSelector class constructor.
        /// </summary>
        /// <param name="C">List of available configs.</param>
        public FrmCfgSelector(List<string> C)
        {
            InitializeComponent();
            Configs = C;
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmCfgSelector_Load(object sender, EventArgs e)
        {
            CS_CfgSel.DataSource = Configs;
        }

        /// <summary>
        /// "OK" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CS_OK_Click(object sender, EventArgs e)
        {
            Config = CS_CfgSel.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// "Cancel" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CS_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// "Config selected" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void CS_CfgSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Setting full path to tooltip...
            CS_ToolTip.SetToolTip((ComboBox)sender, ((ComboBox)sender).Text);
        }
    }
}
