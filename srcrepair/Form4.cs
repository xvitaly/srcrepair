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
            if ((File.Exists(GV.FullAppPath + "7z.exe")) && (File.Exists(GV.FullAppPath + "7z.dll")))
            {
                Compress.Enabled = true;
            }
            else
            {
                Compress.Enabled = false;
            }
        }

        private void GenerateNow_Click(object sender, EventArgs e)
        {
            if (GenerateNow.Text != frmMainW.RM.GetString("RPB_CloseCpt"))
            {
                MessageBox.Show(frmMainW.RM.GetString("RPB_GenWarn"), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult UserConfirmation = MessageBox.Show(frmMainW.RM.GetString("RPB_GenQst"), PluginName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (UserConfirmation == DialogResult.Yes)
                {
                    // Отключим кнопку...
                    GenerateNow.Enabled = false;
                    GenerateNow.Text = frmMainW.RM.GetString("RPB_CptWrk");
                    // Начинаем создавать отчёт...
                    string FilePath = "msinfo32.exe"; // Указываем имя exe-файла для запуска
                    string FileName = "Report_" + frmMainW.WriteDateToString(DateTime.Now, true);
                    string RepName = FileName + ".txt";
                    string Params = "/report " + '"' + GV.FullAppPath + RepName + '"'; // Генерируем параметы для exe-файла...
                    try
                    {
                        // Запускаем последовательность...
                        frmMainW.StartProcessAndWait(FilePath, Params);
                        if (Compress.Checked)
                        {
                            frmMainW.StartProcessAndWait(GV.FullAppPath + "7z.exe", "a " + FileName + ".7z " + RepName);
                            File.Delete(GV.FullAppPath + RepName); // удаляем несжатый отчёт
                            MessageBox.Show(String.Format(frmMainW.RM.GetString("RPB_ComprGen"), FileName), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(String.Format(frmMainW.RM.GetString("RPB_Generated"), RepName), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        
                        // Открываем каталог с отчётами в Windows Explorer...
                        Process.Start(GV.FullAppPath);
                    }
                    catch
                    {
                        // Произошло исключение...
                        MessageBox.Show(frmMainW.RM.GetString("RPB_GenException"), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Снова активируем кнопку...
                    GenerateNow.Text = frmMainW.RM.GetString("RPB_CloseCpt");
                    GenerateNow.Enabled = true;
                }
            }
            else
            {
                Close();
            }
        }
    }
}
