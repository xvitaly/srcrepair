/*
 * Модуль "Установщик спреев, демок и конфигов" программы SRC Repair.
 * 
 * Copyright 2011 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2011 EasyCoding Team.
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace srcrepair
{
    public partial class frmInstaller : Form
    {
        public frmInstaller()
        {
            InitializeComponent();
        }

        private const string PluginName = "Quick Installer";

        private void InstallFileNow(string FileName, string SubDir)
        {
            // Устанавливаем файл...
            File.Copy(FileName, Path.Combine(GV.FullGamePath, SubDir, Path.GetFileName(FileName)), true);
        }

        private void CompileFromVTF(string FileName)
        {
            // Начинаем...
            using (System.IO.StreamWriter CFile = new System.IO.StreamWriter(FileName))
            {
                CFile.WriteLine(@"""UnlitGeneric""");
                CFile.WriteLine("{");
                CFile.WriteLine("\t" + @"""$basetexture""	""vgui\logos\" + Path.GetFileNameWithoutExtension(FileName) + @"""");
                CFile.WriteLine("\t" + @"""$translucent"" ""1""");
                CFile.WriteLine("\t" + @"""$ignorez"" ""1""");
                CFile.WriteLine("\t" + @"""$vertexcolor"" ""1""");
                CFile.WriteLine("\t" + @"""$vertexalpha"" ""1""");
                CFile.WriteLine("}");
                CFile.Close();
            }
        }

        private void InstallSprayNow(string FileName)
        {
            // Заполняем необходимые переменные...
            string CDir = Path.GetDirectoryName(FileName); // Получаем каталог с файлами для копирования...
            string FPath = Path.Combine(GV.FullGamePath, "materials", "vgui", "logos"); // Получаем путь к каталогу назначения...
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
                if (MessageBox.Show(CoreLib.GetLocalizedString("QI_GenVMTMsg"), PluginName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                    // У нас два алгоритма, поэтому придётся делать проверки...
                    switch (Path.GetExtension(InstallPath.Text))
                    {
                        case ".dem": InstallFileNow(InstallPath.Text, ""); // Будем устанавливать демку...
                            break;
                        case ".cfg": InstallFileNow(InstallPath.Text, "cfg" + Path.DirectorySeparatorChar); // Будем устанавливать конфиг...
                            break;
                        case ".bsp": InstallFileNow(InstallPath.Text, "maps" + Path.DirectorySeparatorChar); // Будем устанавливать карту...
                            break;
                        case ".vtf": InstallSprayNow(InstallPath.Text); // Будем устанавливай спрей...
                            break;
                    }

                    // Выведем сообщение...
                    MessageBox.Show(CoreLib.GetLocalizedString("QI_InstSuccessfull"), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    // Произошло исключение, выведем сообщение...
                    MessageBox.Show(CoreLib.GetLocalizedString("QI_Excpt"), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Пользователь ничего не выбрал для установки, укажем ему на это...
                MessageBox.Show(CoreLib.GetLocalizedString("QI_InstUnav"), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
