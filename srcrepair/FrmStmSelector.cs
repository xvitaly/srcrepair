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
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of UserID selection window.
    /// </summary>
    public partial class FrmStmSelector : Form
    {
        /// <summary>
        /// Logger instance for FrmStmSelector class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets or sets Steam UserID.
        /// </summary>
        public string SteamID { get; private set; }

        /// <summary>
        /// Gets or sets list of available Steam UserIDs.
        /// </summary>
        private List<String> SteamIDs { get; set; }

        /// <summary>
        /// FrmStmSelector class constructor.
        /// </summary>
        /// <param name="S">List of available Steam UserIDs.</param>
        public FrmStmSelector(List<String> S)
        {
            InitializeComponent();
            SteamIDs = S;
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmStmSelector_Load(object sender, EventArgs e)
        {
            // Connecting SteamIDs object with ComboBox on form...
            SD_IDSel.DataSource = SteamIDs;
        }

        /// <summary>
        /// "OK" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void ST_OK_Click(object sender, EventArgs e)
        {
            SteamID = SD_IDSel.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// "Cancel" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void ST_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// "Show profile page" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void SD_Follow_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessManager.OpenWebPage(String.Format(Properties.Resources.MM_CommunityURL, SteamConv.ConvSidv3Sid64(SD_IDSel.Text)));
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExUrlStmSel);
            }
        }
    }
}
