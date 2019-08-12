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
using System.Collections.Generic;
using System.Windows.Forms;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of config file selector window.
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
        private List<String> Configs { get; set; }

        /// <summary>
        /// FrmCfgSelector class constructor.
        /// </summary>
        /// <param name="C">List of available configs.</param>
        public FrmCfgSelector(List<String> C)
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
