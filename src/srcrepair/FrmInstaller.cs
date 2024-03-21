/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
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
        /// Stores path to game installation directory.
        /// </summary>
        private readonly string FullGamePath;

        /// <summary>
        /// Stores if current game is using a special directory
        /// for custom user stuff.
        /// </summary>
        private readonly bool IsUsingUserDir;

        /// <summary>
        /// Stores path to custom user stuff directory.
        /// </summary>
        private readonly string CustomInstallDir;

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
                if (MessageBox.Show(AppStrings.QI_GenVMTMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
            if (!string.IsNullOrEmpty(InstallPath.Text))
            {
                try
                {
                    // Generating full path to destination directory, depending on selected in main window game...
                    string InstallDir = IsUsingUserDir ? Path.Combine(CustomInstallDir, Properties.Settings.Default.UserCustDirName) : FullGamePath;

                    // Using different methods, based on source file extension...
                    switch (Path.GetExtension(InstallPath.Text))
                    {
                        case ".dem": // Installing demo file...
                            InstallFileNow(InstallPath.Text, FullGamePath);
                            break;
                        case ".vpk": // Installing VPK package...
                            InstallFileNow(InstallPath.Text, CustomInstallDir);
                            break;
                        case ".cfg": // Installing game config...
                            InstallFileNow(InstallPath.Text, Path.Combine(InstallDir, "cfg"));
                            break;
                        case ".bsp": // Installing map...
                            InstallFileNow(InstallPath.Text, Path.Combine(InstallDir, "maps"));
                            break;
                        case ".wav": // Installing hitsound...
                            InstallFileNow(InstallPath.Text, Path.Combine(InstallDir, "sound", "ui"));
                            break;
                        case ".vtf": // Installing spray...
                            InstallSprayNow(InstallPath.Text);
                            break;
                        case ".zip": // Installing contents of Zip archive...
                            GuiHelpers.FormShowArchiveExtract(InstallPath.Text, CustomInstallDir);
                            break;
                        case ".dll": // Installing binary plugin...
                            InstallFileNow(InstallPath.Text, Path.Combine(InstallDir, "addons"));
                            break;
                        default:
                            Logger.Warn(DebugStrings.AppDbgQIUnknownFileType);
                            break;
                    }

                    // Showing message...
                    MessageBox.Show(AppStrings.QI_InstSuccessfull, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                MessageBox.Show(AppStrings.QI_InstUnav, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
