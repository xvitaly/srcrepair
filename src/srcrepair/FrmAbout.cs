/**
 * SPDX-FileCopyrightText: 2011-2021 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
                iconApp.Image = Properties.Resources.IconXmas;
            }
        }
    }
}
