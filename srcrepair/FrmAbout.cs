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
using srcrepair.core;
using NLog;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of form "About".
    /// </summary>
    partial class FrmAbout : Form
    {
        /// <summary>
        /// Logger instance for FrmAbout class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// FrmAbout class constructor.
        /// </summary>
        public FrmAbout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// "OK" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// "Form create" event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FrmAbout_Load(object sender, EventArgs e)
        {
            // Adding information about product version and copyrights...
            Text = String.Format("About {0}...", CurrentApp.AppProduct);
            labelProductName.Text = CurrentApp.AppProduct;
            #if DEBUG
            labelProductName.Text += " DEBUG";
            #endif
            labelVersion.Text = String.Format("Version: {0}", CurrentApp.AppVersion);
            labelCopyright.Text = CurrentApp.AppCopyright;
            labelCompanyName.Text = CurrentApp.AppCompany;

            // Checking for the New Year eve...
            if (CurrentApp.IsNewYear)
            {
                iconApp.Image = Properties.Resources.Xmas;
            }
        }

        /// <summary>
        /// "Legal info" label click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void LabelLicense_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessManager.OpenWebPage(Properties.Resources.AppLegalURL);
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex);
            }
        }
    }
}
