/**
 * SPDX-FileCopyrightText: 2011-2024 EasyCoding Team
 *
 * SPDX-License-Identifier: GPL-3.0-or-later
*/

using System;
using System.IO;
using System.Windows.Forms;
using NLog;

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
        /// Compiles a new VMT file from a template.
        /// </summary>
        /// <param name="FileName">Full path to the destination VMT file.</param>
        private void CompileFromTemplate(string FileName)
        {
            try
            {
                using (StreamWriter CFile = new StreamWriter(FileName))
                {
                    CFile.WriteLine(Properties.Resources.TemplateVMTSpray.Replace("{D}", Path.Combine("vgui", "logos", Path.GetFileNameWithoutExtension(FileName))));
                }
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, DebugStrings.AppDbgExInstallerCompile);
                MessageBox.Show(AppStrings.QI_GenVMTError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Installs a custom spray into the game.
        /// </summary>
        /// <param name="FileName">Full path to the source spray file.</param>
        private void InstallSprayNow(string FileName)
        {
            // Generating paths to files and directories...
            string DestDir = Path.Combine(FullGamePath, "materials", "vgui", "logos");
            string VMTFile = Path.Combine(Path.GetDirectoryName(FileName), Path.ChangeExtension(Path.GetFileName(FileName), ".vmt"));

            // Checking if the precompiled VMT file exists...
            if (!File.Exists(VMTFile) && MessageBox.Show(AppStrings.QI_GenVMTMsg, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CompileFromTemplate(VMTFile);
            }

            // Installing spray file...
            InstallFileNow(FileName, DestDir);

            // Installing VMT file...
            if (File.Exists(VMTFile))
            {
                InstallFileNow(VMTFile, DestDir);
            }
        }

        /// <summary>
        /// Installs a custom content into the game.
        /// </summary>
        /// <param name="FileName">Full path to the custom content file.</param>
        private void InstallContent(string FileName)
        {
            // Generating full path to the installation directory...
            string InstallDir = IsUsingUserDir ? Path.Combine(CustomInstallDir, Properties.Settings.Default.UserCustDirName) : FullGamePath;
            
            // Using different methods, based on source file extension...
            switch (Path.GetExtension(FileName))
            {
                case ".dem": // Installing demo file...
                    InstallFileNow(FileName, FullGamePath);
                    break;
                case ".vpk": // Installing VPK package...
                    InstallFileNow(FileName, CustomInstallDir);
                    break;
                case ".cfg": // Installing game config...
                    InstallFileNow(FileName, Path.Combine(InstallDir, "cfg"));
                    break;
                case ".bsp": // Installing map...
                    InstallFileNow(FileName, Path.Combine(InstallDir, "maps"));
                    break;
                case ".wav": // Installing hitsound...
                    InstallFileNow(FileName, Path.Combine(InstallDir, "sound", "ui"));
                    break;
                case ".vtf": // Installing spray...
                    InstallSprayNow(FileName);
                    break;
                case ".zip": // Installing contents of Zip archive...
                    GuiHelpers.FormShowArchiveExtract(FileName, CustomInstallDir);
                    break;
                case ".dll": // Installing binary plugin...
                    InstallFileNow(FileName, Path.Combine(InstallDir, "addons"));
                    break;
                default: // Unknown file type...
                    throw new NotImplementedException(DebugStrings.AppDbgQIUnknownFileType);
            }
        }

        /// <summary>
        /// "Browse" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void QI_Browse_Click(object sender, EventArgs e)
        {
            if (QI_OpenFile.ShowDialog() == DialogResult.OK)
            {
                QI_InstallPath.Text = QI_OpenFile.FileName;
            }
        }

        /// <summary>
        /// "Install" button click event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void QI_Install_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(QI_InstallPath.Text))
            {
                MessageBox.Show(AppStrings.QI_NoSelection, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                InstallContent(QI_InstallPath.Text);
                MessageBox.Show(AppStrings.QI_InstallationSuccessful, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, DebugStrings.AppDbgExInstallerRun);
                MessageBox.Show(AppStrings.QI_Excpt, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
