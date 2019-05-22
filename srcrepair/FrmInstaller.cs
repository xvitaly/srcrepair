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
    /// Класс формы модуля быстрой установки.
    /// </summary>
    public partial class FrmInstaller : Form
    {
        /// <summary>
        /// Управляет записью событий в журнал.
        /// </summary>
        private Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Конструктор класса формы модуля распаковки файлов из архивов.
        /// </summary>
        /// <param name="F">Путь к каталогу игры</param>
        /// <param name="I">Использует ли игра кастомный каталог</param>
        /// <param name="U">Имя кастомного каталога</param>
        public FrmInstaller(string F, bool I, string U)
        {
            InitializeComponent();
            FullGamePath = F;
            IsUsingUserDir = I;
            CustomInstallDir = U;
        }

        /// <summary>
        /// Хранит название плагина для служебных целей.
        /// </summary>
        private const string PluginName = "Quick Installer";

        /// <summary>
        /// Хранит полный путь к каталогу игры.
        /// </summary>
        private string FullGamePath { get; set; }

        /// <summary>
        /// Использует ли игра особый кастомный каталог.
        /// </summary>
        private bool IsUsingUserDir { get; set; }

        /// <summary>
        /// Хранит название директории внутри кастомного каталога.
        /// </summary>
        private string CustomInstallDir { get; set; }

        /// <summary>
        /// Устанавливает файл в указанный каталог.
        /// </summary>
        /// <param name="FileName">Имя устанавливаемого файла с полным путём</param>
        /// <param name="DestDir">Каталог, в который файл будет установлен</param>
        private void InstallFileNow(string FileName, string DestDir)
        {
            // Проверяем существование каталога установки и если его нет, создаём...
            if (!(Directory.Exists(DestDir))) { Directory.CreateDirectory(DestDir); }

            // Устанавливаем файл...
            File.Copy(FileName, Path.Combine(DestDir, Path.GetFileName(FileName)), true);
        }

        /// <summary>
        /// Компилирует VMT файл из VTF.
        /// </summary>
        /// <param name="FileName">Имя VMT файла с полным путём до него</param>
        private void CompileFromVTF(string FileName)
        {
            // Начинаем...
            using (StreamWriter CFile = new StreamWriter(FileName))
            {
                try { CFile.WriteLine(CoreLib.GetTemplateFromResource(Properties.Resources.PI_TemplateFile).Replace("{D}", Path.Combine("vgui", "logos", Path.GetFileNameWithoutExtension(FileName)))); } catch (Exception Ex) { Logger.Warn(Ex); }
            }
        }

        /// <summary>
        /// Устанавливает спрей в игру.
        /// </summary>
        /// <param name="FileName">Имя файла спрея с полным путём для установки</param>
        private void InstallSprayNow(string FileName)
        {
            // Заполняем необходимые переменные...
            string CDir = Path.GetDirectoryName(FileName); // Получаем каталог с файлами для копирования...
            string FPath = Path.Combine(FullGamePath, "materials", "vgui", "logos"); // Получаем путь к каталогу назначения...
            string FFPath = Path.Combine(FPath, Path.GetFileName(FileName)); // Получаем полный путь к файлу...
            string VMTFileDest = Path.Combine(FPath, Path.GetFileNameWithoutExtension(Path.GetFileName(FileName)) + ".vmt"); // Генерируем путь назначения с именем файла...
            string VMTFile = Path.Combine(CDir, Path.GetFileName(VMTFileDest)); // Получаем путь до VMT-файла, лежащего в папке с VTF...
            bool UseVMT;

            // Начинаем...
            // Проверим наличие VMT-файла и если его нет, соберём вручную...
            if (File.Exists(VMTFile)) // Проверяем...
            {
                UseVMT = true; // Файл найден, включаем установку и VMT...
            }
            else
            {
                // Файл не найден, спросим нужно ли создать...
                if (MessageBox.Show(AppStrings.QI_GenVMTMsg, PluginName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Да, нужно.
                    UseVMT = true;
                    CompileFromVTF(VMTFile);
                }
                else
                {
                    UseVMT = false; // Отключаем установку VMT-файла...
                }
            }

            // Проверим существование каталога назначения...
            if (!Directory.Exists(FPath)) { Directory.CreateDirectory(FPath); }

            // Копируем VTF-файл...
            File.Copy(FileName, Path.Combine(FPath, Path.GetFileName(FFPath)), true);

            // Копируем VMT-файл если задано...
            if (UseVMT) { File.Copy(VMTFile, VMTFileDest, true); }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии кнопки открытия файла.
        /// </summary>
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            // Открываем диалоговое окно выбора файла и записываем путь в Edit...
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                InstallPath.Text = openDialog.FileName;
            }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии кнопки, запускающей установку.
        /// </summary>
        private void BtnInstall_Click(object sender, EventArgs e)
        {
            // А здесь собственно установка...
            if (!(String.IsNullOrEmpty(InstallPath.Text)))
            {
                try
                {
                    // Сгенерируем путь...
                    string InstallDir = IsUsingUserDir ? Path.Combine(CustomInstallDir, Properties.Settings.Default.UserCustDirName) : FullGamePath;

                    // У нас множество алгоритмов, поэтому придётся делать проверки...
                    switch (Path.GetExtension(InstallPath.Text))
                    {
                        // Будем устанавливать демку...
                        case ".dem": InstallFileNow(InstallPath.Text, FullGamePath);
                            break;
                        // Будем устанавливать пакет...
                        case ".vpk": InstallFileNow(InstallPath.Text, CustomInstallDir);
                            break;
                        // Будем устанавливать конфиг...
                        case ".cfg": InstallFileNow(InstallPath.Text, Path.Combine(InstallDir, "cfg"));
                            break;
                        // Будем устанавливать карту...
                        case ".bsp": InstallFileNow(InstallPath.Text, Path.Combine(InstallDir, "maps"));
                            break;
                        // Будем устанавливать хитсаунд...
                        case ".wav": InstallFileNow(InstallPath.Text, Path.Combine(InstallDir, "sound", "ui"));
                            break;
                        // Будем устанавливай спрей...
                        case ".vtf": InstallSprayNow(InstallPath.Text);
                            break;
                        // Будем устанавливать содержимое архива...
                        case ".zip": GuiHelpers.FormShowArchiveExtract(InstallPath.Text, CustomInstallDir);
                            break;
                        // Будем устанавливать бинарный модуль (плагин)...
                        case ".dll": InstallFileNow(InstallPath.Text, Path.Combine(InstallDir, "addons"));
                            break;
                    }

                    // Выведем сообщение...
                    MessageBox.Show(AppStrings.QI_InstSuccessfull, PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Закрываем окно...
                    Close();
                }
                catch (Exception Ex)
                {
                    // Произошло исключение, выведем сообщение...
                    MessageBox.Show(AppStrings.QI_Excpt, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logger.Error(Ex, AppStrings.AppDbgExInstRun);
                }
            }
            else
            {
                // Пользователь ничего не выбрал для установки, укажем ему на это...
                MessageBox.Show(AppStrings.QI_InstUnav, PluginName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
