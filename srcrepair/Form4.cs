/*
 * Модуль "Создание отчёта для Техподдержки" программы SRC Repair.
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
using System.Diagnostics;
using Ionic.Zip;

namespace srcrepair
{
    public partial class frmRepBuilder : Form
    {
        public frmRepBuilder()
        {
            InitializeComponent();
        }

        private const string PluginName = "Report Builder";

        private void frmRepBuilder_Load(object sender, EventArgs e)
        {
            // Событие создания формы...
        }

        private void GenerateNow_Click(object sender, EventArgs e)
        {
            if (GenerateNow.Text != CoreLib.GetLocalizedString("RPB_CloseCpt"))
            {
                if (MessageBox.Show(CoreLib.GetLocalizedString("RPB_GenQst"), PluginName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Отключим кнопку...
                    GenerateNow.Text = CoreLib.GetLocalizedString("RPB_CptWrk");
                    GenerateNow.Enabled = false;
                    this.ControlBox = false;
                    // Сгенерируем путь для каталога с рапортами...
                    string RepDir = CoreLib.IncludeTrDelim(Path.Combine(GV.AppUserDir, "reports"));
                    // Проверим чтобы каталог для рапортов существовал...
                    if (!Directory.Exists(RepDir))
                    {
                        // Не существует, поэтому создадим...
                        Directory.CreateDirectory(RepDir);
                    }
                    // Начинаем создавать отчёт...
                    string FilePath = "msinfo32.exe"; // Указываем имя exe-файла для запуска
                    string FileName = "Report_" + CoreLib.WriteDateToString(DateTime.Now, true);
                    string RepName = FileName + ".txt";
                    string Params = "/report " + '"' + RepDir + RepName + '"'; // Генерируем параметы для exe-файла...
                    string HostsFile = CoreLib.GetHostsFileFullPath(GV.RunningPlatform);
                    try
                    {
                        // Запускаем последовательность...
                        CoreLib.StartProcessAndWait(FilePath, Params);
                        try
                        {
                            using (ZipFile ZBkUp = new ZipFile(Path.Combine(RepDir, FileName + ".zip"), Encoding.UTF8))
                            {
                                ZBkUp.AddFile(Path.Combine(RepDir, RepName), "Report");
                                if (Directory.Exists(GV.FullCfgPath)) { ZBkUp.AddDirectory(GV.FullCfgPath, "Configs"); }
                                if (File.Exists(HostsFile)) { ZBkUp.AddFile(HostsFile, "Hosts"); }
                                ZBkUp.Save();
                            }
                            File.Delete(Path.Combine(RepDir, RepName)); // удаляем несжатый отчёт
                            MessageBox.Show(String.Format(CoreLib.GetLocalizedString("RPB_ComprGen"), FileName), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception Ex)
                        {
                            CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("PS_ArchFailed"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                        }
                        
                        // Открываем каталог с отчётами в Windows Explorer...
                        Process.Start(RepDir);
                    }
                    catch (Exception Ex)
                    {
                        // Произошло исключение...
                        CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("RPB_GenException"), PluginName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                    }

                    // Снова активируем кнопку...
                    GenerateNow.Text = CoreLib.GetLocalizedString("RPB_CloseCpt");
                    GenerateNow.Enabled = true;
                    this.ControlBox = true;
                }
            }
            else
            {
                Close();
            }
        }
    }
}
