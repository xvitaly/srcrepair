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
using System.IO;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Class of custom stuff installer window.
    /// </summary>
    public partial class FrmInstaller : Form
    {
        /// <summary>
        /// Logger instance for FrmInstaller class.
        /// </summary>
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// FrmInstaller class constructor.
        /// </summary>
        /// <param name="F">Path to game installation directory.</param>
        /// <param name="I">If current game is using a special directory for custom user stuff.</param>
        /// <param name="U">Path to custom user stuff directory.</param>
        public FrmInstaller(string F, bool I, string U)
        {
            InitializeComponent();
            FullGamePath = F;
            IsUsingUserDir = I;
            CustomInstallDir = U;
        }

        /// <summary>
        /// Stores plugin's name.
        /// </summary>
        private const string PluginName = "Quick Installer";

        /// <summary>
        /// Gets or sets path to game installation directory.
        /// </summary>
        private string FullGamePath { get; set; }

        /// <summary>
        /// Gets or sets if current game is using a special directory
        /// for custom user stuff.
        /// </summary>
        private bool IsUsingUserDir { get; set; }

        /// <summary>
        /// Gets or sets path to custom user stuff directory.
        /// </summary>
        private string CustomInstallDir { get; set; }

        /// <summary>
        /// Installs custom file to game.
        /// </summary>
        /// <param name="FileName">Full path to source file.</param>
        /// <param name="DestDir">Full path to destination file.</param>
        private void InstallFileNow(string FileName, string DestDir)
        {
            // Checking if destination directory exists...
            if (!Directory.Exists(DestDir)) { Directory.CreateDirectory(DestDir); }

            // Copying file from source to destination...
            File.Copy(FileName, Path.Combine(DestDir, Path.GetFileName(FileName)), true);
        }

        /// <summary>
        /// Compiles VMT file from VTF.
        /// </summary>
        /// <param name="FileName">Full path to destination VMT file.</param>
        private void CompileFromVTF(string FileName)
        {
            using (StreamWriter CFile = new StreamWriter(FileName))
            {
                try
                {
                    CFile.WriteLine(StringsManager.GetTemplateFromResource(Properties.Resources.PI_TemplateFile).Replace("{D}", Path.Combine("vgui", "logos", Path.GetFileNameWithoutExtension(FileName))));
                }
                catch (Exception Ex)
                {
                    Logger.Warn(Ex);
                }
            }
        }

        /// <summary>
        /// Installs custom spray to game.
        /// </summary>
        /// <param name="FileName">Full path to source file with spray.</param>
        private void InstallSprayNow(string FileName)
        {
            // Generating path to source directory...
            string CDir = Path.GetDirectoryName(FileName);

            // Generating path to destination directory...
            string FPath = Path.Combine(FullGamePath, "materials", "vgui", "logos");

            // Generating full path to destination spray file...
            string FFPath = Path.Combine(FPath, Path.GetFileName(FileName));

            // Generating full path to destination VMT file...
            string VMTFileDest = Path.Combine(FPath, Path.GetFileNameWithoutExtension(Path.GetFileName(FileName)) + ".vmt");

            // Generating full path to source VMT file...
            string VMTFile = Path.Combine(CDir, Path.GetFileName(VMTFileDest));
            bool UseVMT;

            // Checking if precompiled VMT file exists...
            if (File.Exists(VMTFile))
            {
                // Found. Will use it...
                UseVMT = true;
            }
            else
            {
                // Not found. Asking user to allow its compilation...
                if (MessageBox.Show(AppStrings.QI_GenVMTMsg, PluginName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Will compile VMT file automatically...
                    UseVMT = true;
                    CompileFromVTF(VMTFile);
                }
                else
                {
                    // Will not install VMT file...
                    UseVMT = false;
                }
            }

            // Checking if destination directory exists...
            if (!Directory.Exists(FPath))
            {
                Directory.CreateDirectory(FPath);
            }

            // Compying spray file...
            File.Copy(FileName, Path.Combine(FPath, Path.GetFileName(FFPath)), true);

            // Copying VMT file if allowed...
            if (UseVMT)
            {
                File.Copy(VMTFile, VMTFileDest, true);
            }
        }

        /// <summary>
        /// "Browse" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                InstallPath.Text = openDialog.FileName;
            }
        }

        /// <summary>
        /// "Install" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void BtnInstall_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(InstallPath.Text)))
            {
                try
                {
                    // Generating full path to destination directory, depending on selected in main window game...
                    string InstallDir = IsUsingUserDir ? Path.Combine(CustomInstallDir, Properties.Settings.Default.UserCustDirName) : FullGamePath;

                    // Using different methods, based on source file extension...
                    switch (Path.GetExtension(InstallPath.Text))
                    {
                        // Installing demo file...
                        case ".dem": InstallFileNow(InstallPath.Text, FullGamePath);
                            break;
                        // Installing VPK package...
                        case ".vpk": InstallFileNow(InstallPath.Text, CustomInstallDir);
                            break;
                        // Installing game config...
                        case ".cfg": InstallFileNow(InstallPath.Text, Path.Combine(InstallDir, "cfg"));
                            break;
                        // Installing map...
                        case ".bsp": InstallFileNow(InstallPath.Text, Path.Combine(InstallDir, "maps"));
                            break;
                        // Installing hitsound...
                        case ".wav": InstallFileNow(InstallPath.Text, Path.Combine(InstallDir, "sound", "ui"));
                            break;
                        // Installing spray...
                        case ".vtf": InstallSprayNow(InstallPath.Text);
                            break;
                        // Installing contents of Zip archive...
                        case ".zip": GuiHelpers.FormShowArchiveExtract(InstallPath.Text, CustomInstallDir);
                            break;
                        // Installing binary plugin...
                        case ".dll": InstallFileNow(InstallPath.Text, Path.Combine(InstallDir, "addons"));
                            break;
                    }

                    // Showing message...
                    MessageBox.Show(AppStrings.QI_InstSuccessfull, PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Closing window...
                    Close();
                }
                catch (Exception Ex)
                {
                    // An error occured. Showing message and writing issue to logs...
                    MessageBox.Show(AppStrings.QI_Excpt, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, DebugStrings.AppDbgExInstRun);
                }
            }
            else
            {
                // User selected nothing. Showing message...
                MessageBox.Show(AppStrings.QI_InstUnav, PluginName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
