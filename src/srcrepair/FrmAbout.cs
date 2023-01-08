/**
 * SPDX-FileCopyrightText: 2011-2023 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.Windows.Forms;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of form "About".
    /// </summary>
    public partial class FrmAbout : Form
    {
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
            AF_ProductName.Text = CurrentApp.AppProduct;
            AF_ProductVersion.Text = string.Format("Version: {0}", CurrentApp.AppVersion);
            AF_Copyright.Text = CurrentApp.AppCopyright;
            AF_CompanyName.Text = CurrentApp.AppCompany;

            // Checking for the New Year eve...
            if (CurrentApp.IsNewYear)
            {
                AF_ProductIcon.Image = Properties.Resources.ImageXmas;
            }
        }
    }
}
