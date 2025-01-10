/**
 * SPDX-FileCopyrightText: 2011-2025 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of the UserID selection module.
    /// </summary>
    public partial class FrmStmSelector : Form
    {
        /// <summary>
        /// Logger instance for FrmStmSelector class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// CurrentPlatform instance for FrmStmSelector class.
        /// </summary>
        private readonly CurrentPlatform Platform = CurrentPlatform.Create();

        /// <summary>
        /// Stores the list of available Steam UserIDs.
        /// </summary>
        private readonly List<string> SteamIDs;

        /// <summary>
        /// Gets or sets Steam UserID.
        /// </summary>
        public string SteamID { get; private set; }

        /// <summary>
        /// FrmStmSelector class constructor.
        /// </summary>
        /// <param name="S">List of available Steam UserIDs.</param>
        public FrmStmSelector(List<string> S)
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
            ST_IDSel.DataSource = SteamIDs;
        }

        /// <summary>
        /// "OK" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void ST_Okay_Click(object sender, EventArgs e)
        {
            SteamID = ST_IDSel.Text;
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
        private void ST_ShowProfile_Click(object sender, EventArgs e)
        {
            try
            {
                Platform.OpenWebPage(string.Format(Properties.Resources.MM_CommunityURL, SteamConv.ConvSidv3Sid64(ST_IDSel.Text)));
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExUrlProfile);
            }
        }
    }
}
