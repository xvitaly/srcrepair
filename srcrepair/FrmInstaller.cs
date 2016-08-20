/*
 * Модуль "Установщик спреев, демок и конфигов" программы SRC Repair.
 * 
 * Copyright 2011 - 2016 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2016 EasyCoding Team.
 * 
 * Лицензия: GPL v3 (см. файл GPL.txt).
 * Лицензия контента: Creative Commons 3.0 BY.
 * 
 * Запрещается использовать этот файл при использовании любой
 * лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
 * 
 * Официальный блог EasyCoding Team: http://www.easycoding.org/
 * Официальная страница проекта: http://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/
using System;
using System.Windows.Forms;
using System.IO;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля быстрой установки.
    /// </summary>
    public partial class frmInstaller : Form
    {
        public frmInstaller(string F, bool I, string U)
        {
            InitializeComponent();
            FullGamePath = F;
            IsUsingUserDir = I;
            CustomInstallDir = U;
        }

        private const string PluginName = "Quick Installer";
        private string FullGamePath;
        private bool IsUsingUserDir;
        private string CustomInstallDir;

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
                try { CFile.WriteLine(CoreLib.GetTemplateFromResource(Properties.Resources.PI_TemplateFile).Replace("{D}", Path.Combine("vgui", "logos", Path.GetFileNameWithoutExtension(FileName)))); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
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
        
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // Открываем диалоговое окно выбора файла и записываем путь в Edit...
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                InstallPath.Text = openDialog.FileName;
            }
        }

        private void btnInstall_Click(object sender, EventArgs e)
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
                        case ".zip": CoreLib.ExtractFiles(InstallPath.Text, CustomInstallDir);
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
                    CoreLib.HandleExceptionEx(AppStrings.QI_Excpt, PluginName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
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
