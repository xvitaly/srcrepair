﻿/*
 * Модуль "Создание отчёта для Техподдержки" программы SRC Repair.
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
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Ionic.Zip;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля создания отчётов для техподдержки.
    /// </summary>
    public partial class frmRepBuilder : Form
    {
        public frmRepBuilder(string A, string FS, string FC)
        {
            InitializeComponent();
            AppUserDir = A;
            FullSteamPath = FS;
            FullCfgPath = FC;
        }

        private const string PluginName = "Report Builder";
        private bool IsCompleted = false;
        private string AppUserDir;
        private string FullSteamPath;
        private string FullCfgPath;

        private void frmRepBuilder_Load(object sender, EventArgs e)
        {
            // Событие создания формы...
        }

        private void BwGen_DoWork(object sender, DoWorkEventArgs e)
        {
            // Сгенерируем путь для каталога с рапортами...
            string RepDir = Path.Combine(AppUserDir, "reports");

            // Проверим чтобы каталог для рапортов существовал...
            if (!Directory.Exists(RepDir))
            {
                // Не существует, поэтому создадим...
                Directory.CreateDirectory(RepDir);
            }
            
            // Генерируем пути к каталогам для вреенных файлов и создаём их...
            string TempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            string CrDt = CoreLib.DateTime2Unix(DateTime.Now);
            if (!Directory.Exists(TempDir)) { Directory.CreateDirectory(TempDir); }
            
            // Генерируем именя файлов с полными путями...
            string RepName = String.Format("report_{0}.{1}", CrDt, "txt");
            string ArchName = Path.Combine(RepDir, Path.ChangeExtension(RepName, ".zip"));
            string HostsFile = CoreLib.GetHostsFileFullPath();
            string FNameRep = Path.Combine(TempDir, RepName);
            string FNamePing = Path.Combine(TempDir, String.Format("ping_{0}.log", CrDt));
            string FNameTrace = Path.Combine(TempDir, String.Format("traceroute_{0}.log", CrDt));
            string FNameIpConfig = Path.Combine(TempDir, String.Format("ipconfig_{0}.log", CrDt));
            string FNameRouting = Path.Combine(TempDir, String.Format("routing_{0}.log", CrDt));
            string FNameNetStat = Path.Combine(TempDir, String.Format("netstat_{0}.log", CrDt));
            string FNameDxDiag = Path.Combine(TempDir, String.Format("dxdiag_{0}.log", CrDt));
            
            // Начинаем сборку отчёта...
            try
            {
                // Генерируем основной отчёт...
                try { CoreLib.StartProcessAndWait("msinfo32.exe", String.Format("/report \"{0}\"", FNameRep)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                
                // Если пользователь вдруг отменил его создание, больше ничего не делаем...
                if (File.Exists(FNameRep))
                {
                    // Запускаем последовательность...
                    try { CoreLib.StartProcessAndWait("dxdiag.exe", String.Format("/t {0}", FNameDxDiag)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); } /* DxDiag неадекватно реагирует на кавычки в пути. */
                    try { CoreLib.StartProcessAndWait("cmd.exe", String.Format("/C ping steampowered.com > \"{0}\"", FNamePing)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { CoreLib.StartProcessAndWait("cmd.exe", String.Format("/C tracert steampowered.com > \"{0}\"", FNameTrace)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { CoreLib.StartProcessAndWait("cmd.exe", String.Format("/C ipconfig /all > \"{0}\"", FNameIpConfig)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { CoreLib.StartProcessAndWait("cmd.exe", String.Format("/C netstat -a > \"{0}\"", FNameNetStat)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    try { CoreLib.StartProcessAndWait("cmd.exe", String.Format("/C route print > \"{0}\"", FNameRouting)); } catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
                    
                    try
                    {
                        // Создаём Zip-архив...
                        using (ZipFile ZBkUp = new ZipFile(ArchName, Encoding.UTF8))
                        {
                            // Добавляем в архив созданный рапорт...
                            if (File.Exists(FNameRep)) { ZBkUp.AddFile(FNameRep, "report"); }

                            // Добавляем в архив все конфиги выбранной игры...
                            if (Directory.Exists(FullCfgPath)) { ZBkUp.AddDirectory(FullCfgPath, "configs"); }

                            // Добавляем в архив все краш-дампы и логи Steam...
                            if (Directory.Exists(Path.Combine(FullSteamPath, "dumps"))) { ZBkUp.AddDirectory(Path.Combine(FullSteamPath, "dumps"), "dumps"); }
                            if (Directory.Exists(Path.Combine(FullSteamPath, "logs"))) { ZBkUp.AddDirectory(Path.Combine(FullSteamPath, "logs"), "logs"); }

                            // Добавляем содержимое файла Hosts...
                            if (File.Exists(HostsFile)) { ZBkUp.AddFile(HostsFile, "hosts"); }

                            // Добавляем в архив отчёты утилит ping, трассировки и т.д.
                            if (File.Exists(FNamePing)) { ZBkUp.AddFile(FNamePing, "system"); }
                            if (File.Exists(FNameTrace)) { ZBkUp.AddFile(FNameTrace, "system"); }
                            if (File.Exists(FNameIpConfig)) { ZBkUp.AddFile(FNameIpConfig, "system"); }
                            if (File.Exists(FNameRouting)) { ZBkUp.AddFile(FNameRouting, "system"); }
                            if (File.Exists(FNameNetStat)) { ZBkUp.AddFile(FNameNetStat, "system"); }
                            if (File.Exists(FNameDxDiag)) { ZBkUp.AddFile(FNameDxDiag, "system"); }

                            // Сохраняем архив...
                            ZBkUp.Save();
                        }

                        // Выводим сообщение об успешном создании отчёта...
                        MessageBox.Show(String.Format(AppStrings.RPB_ComprGen, Path.GetFileName(ArchName)), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Открываем каталог с отчётами в оболочке и выделяем созданный файл...
                        if (File.Exists(ArchName)) { CoreLib.OpenExplorer(ArchName); }
                    }
                    catch (Exception Ex)
                    {
                        CoreLib.HandleExceptionEx(AppStrings.PS_ArchFailed, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                    }
                }

                // Выполняем очистку...
                try
                {
                    // Удаляем не сжатый отчёт...
                    if (File.Exists(FNameRep)) { File.Delete(FNameRep); }
                    
                    // Удаляем временный каталог...
                    if (Directory.Exists(TempDir)) { Directory.Delete(TempDir, true); }
                }
                catch (Exception Ex)
                {
                    CoreLib.WriteStringToLog(Ex.Message);
                }
            }
            catch (Exception Ex)
            {
                // Произошло исключение...
                CoreLib.HandleExceptionEx(AppStrings.RPB_GenException, PluginName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
            }
        }

        private void BwGen_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Меняем текст на кнопке...
            GenerateNow.Text = AppStrings.RPB_CloseCpt;
            
            // Снова активируем ранее отключённые контролы...
            GenerateNow.Enabled = true;
            ControlBox = true;

            // Переключаем свойство...
            IsCompleted = true;
        }

        private void GenerateNow_Click(object sender, EventArgs e)
        {
            // Проверим необходим ли нам запуск очистки или закрытие формы...
            if (!IsCompleted)
            {
                // Отключим контролы...
                GenerateNow.Text = AppStrings.RPB_CptWrk;
                GenerateNow.Enabled = false;
                ControlBox = false;

                // Запускаем асинхронный обработчик...
                if (!BwGen.IsBusy) { BwGen.RunWorkerAsync(); } else { CoreLib.WriteStringToLog("RepGen Worker is busy. Can't start build sequence."); }
            }
            else
            {
                // Закрываем форму...
                Close();
            }
        }

        private void frmRepBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Блокируем возможность закрытия формы во время работы модуля...
            e.Cancel = BwGen.IsBusy;
        }
    }
}
