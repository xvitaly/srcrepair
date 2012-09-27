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

        private void BwGen_DoWork(object sender, DoWorkEventArgs e)
        {
            // Сгенерируем путь для каталога с рапортами...
            string RepDir = Path.Combine(GV.AppUserDir, "reports");
            // Проверим чтобы каталог для рапортов существовал...
            if (!Directory.Exists(RepDir))
            {
                // Не существует, поэтому создадим...
                Directory.CreateDirectory(RepDir);
            }
            // Начинаем создавать отчёт...
            string TempDir = Path.Combine(Path.GetTempPath(), "repbuilder");
            string CrDt = CoreLib.WriteDateToString(DateTime.Now, true);
            if (!Directory.Exists(TempDir)) { Directory.CreateDirectory(TempDir); }
            string FileName = String.Format("report_{0}", CrDt);
            string RepName = FileName + ".txt";
            string HostsFile = CoreLib.GetHostsFileFullPath(GV.RunningPlatform);
            string FNamePing = Path.Combine(TempDir, String.Format("ping_{0}.log", CrDt));
            string FNameTrace = Path.Combine(TempDir, String.Format("traceroute_{0}.log", CrDt));
            string FNameIpConfig = Path.Combine(TempDir, String.Format("ipconfig_{0}.log", CrDt));
            string FNameRouting = Path.Combine(TempDir, String.Format("routing_{0}.log", CrDt));
            string FNameDxDiag = Path.Combine(TempDir, String.Format("dxdiag_{0}.log", CrDt));
            try
            {
                // Запускаем последовательность...
                try { CoreLib.StartProcessAndWait("msinfo32.exe", String.Format("/report \"{0}\"", Path.Combine(TempDir, RepName))); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                try { CoreLib.StartProcessAndWait("dxdiag.exe", String.Format("/t {0}", FNameDxDiag)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); } /* DxDiag неадекватно реагирует на кавычки в пути. */
                try { CoreLib.StartProcessAndWait("cmd.exe", String.Format("/C ping steampowered.com > \"{0}\"", FNamePing)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                try { CoreLib.StartProcessAndWait("cmd.exe", String.Format("/C tracert steampowered.com > \"{0}\"", FNameTrace)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                try { CoreLib.StartProcessAndWait("cmd.exe", String.Format("/C ipconfig /all > \"{0}\"", FNameIpConfig)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                try { CoreLib.StartProcessAndWait("cmd.exe", String.Format("/C route print > \"{0}\"", FNameRouting)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                try
                {
                    using (ZipFile ZBkUp = new ZipFile(Path.Combine(RepDir, FileName + ".zip"), Encoding.UTF8))
                    {
                        // Добавляем в архив созданный рапорт...
                        if (File.Exists(Path.Combine(TempDir, RepName))) { ZBkUp.AddFile(Path.Combine(TempDir, RepName), "report"); }
                        // Добавляем в архив все конфиги выбранной игры...
                        if (Directory.Exists(GV.FullCfgPath)) { ZBkUp.AddDirectory(GV.FullCfgPath, "configs"); }
                        // Добавляем в архив все краш-дампы...
                        if (Directory.Exists(Path.Combine(GV.FullSteamPath, "dumps"))) { ZBkUp.AddDirectory(Path.Combine(GV.FullSteamPath, "dumps"), "dumps"); }
                        // Добавляем содержимое файла Hosts...
                        if (File.Exists(HostsFile)) { ZBkUp.AddFile(HostsFile, "hosts"); }
                        // Добавляем в архив отчёты утилит ping, трассировки и т.д.
                        if (File.Exists(FNamePing)) { ZBkUp.AddFile(FNamePing, "system"); }
                        if (File.Exists(FNameTrace)) { ZBkUp.AddFile(FNameTrace, "system"); }
                        if (File.Exists(FNameIpConfig)) { ZBkUp.AddFile(FNameIpConfig, "system"); }
                        if (File.Exists(FNameRouting)) { ZBkUp.AddFile(FNameRouting, "system"); }
                        if (File.Exists(FNameDxDiag)) { ZBkUp.AddFile(FNameDxDiag, "system"); }
                        // Сохраняем архив...
                        ZBkUp.Save();
                    }
                    MessageBox.Show(String.Format(CoreLib.GetLocalizedString("RPB_ComprGen"), FileName), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("PS_ArchFailed"), GV.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }

                // Выполняем очистку...
                try
                {
                    if (File.Exists(Path.Combine(TempDir, RepName))) { File.Delete(Path.Combine(TempDir, RepName)); } // удаляем несжатый отчёт
                    if (Directory.Exists(TempDir)) { Directory.Delete(TempDir, true); }
                }
                catch (Exception Ex)
                {
                    CoreLib.WriteStringToLog(Ex.Message);
                }

                // Открываем каталог с отчётами в оболочке и выделяем созданный файл...
                Process.Start(Properties.Settings.Default.ShBin, String.Format("{0} \"{1}\"", Properties.Settings.Default.ShParam, Path.Combine(RepDir, FileName + ".zip")));
            }
            catch (Exception Ex)
            {
                // Произошло исключение...
                CoreLib.HandleExceptionEx(CoreLib.GetLocalizedString("RPB_GenException"), PluginName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
        }

        private void BwGen_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Снова активируем кнопку...
            GenerateNow.Text = CoreLib.GetLocalizedString("RPB_CloseCpt");
            GenerateNow.Enabled = true;
            this.ControlBox = true;
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
                    // Запускаем асинхронный обработчик...
                    if (!BwGen.IsBusy) { BwGen.RunWorkerAsync(); } else { CoreLib.WriteStringToLog("RepGen Worker is busy. Can't start build sequence."); }
                }
            }
            else
            {
                this.Close();
            }
        }
    }
}
