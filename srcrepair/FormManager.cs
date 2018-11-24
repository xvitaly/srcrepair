/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2018 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2018 EasyCoding Team.
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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace srcrepair
{
    /// <summary>
    /// Класс для взаимодействия с отдельным формами и расширениями.
    /// </summary>
    public static class FormManager
    {
        /// <summary>
        /// Начинает загрузку с указанного URL с подробным отображением процесса.
        /// </summary>
        /// <param name="URI">URL для загрузки</param>
        /// <param name="FileName">Путь для сохранения</param>
        public static void FormShowDownloader(string URI, string FileName)
        {
            using (FrmDnWrk DnW = new FrmDnWrk(URI, FileName))
            {
                DnW.ShowDialog();
            }
        }

        /// <summary>
        /// Распаковывает архив в указанный каталог при помощи библиотеки DotNetZip
        /// с выводом прогресса в отдельном окне.
        /// </summary>
        /// <param name="ArchName">Имя архивного файла с указанием полного пути</param>
        /// <param name="DestDir">Каталог назначения</param>
        public static void FormShowArchiveExtract(string ArchName, string DestDir)
        {
            using (FrmArchWrk ArW = new FrmArchWrk(ArchName, DestDir))
            {
                ArW.ShowDialog();
            }
        }

        /// <summary>
        /// Отображает диалоговое окно менеджера быстрой очистки.
        /// </summary>
        /// <param name="Paths">Каталоги для очистки</param>
        /// <param name="Mask">Маска файлов, подлежащих очистке</param>
        /// <param name="LText">Текст заголовка</param>
        /// <param name="CheckBin">Имя бинарника, работа которого будет проверяться перед запуском очистки</param>
        /// <param name="ResultMsg">Текст сообщения, которое будет выдаваться по завершении очистки</param>
        /// <param name="BackUpDir">Каталог для сохранения резервных копий</param>
        /// <param name="ReadOnly">Пользователю будет запрещено изменять выбор удаляемых файлов</param>
        /// <param name="NoAuto">Включает / отключает автовыбор файлов флажками</param>
        /// <param name="Recursive">Включает / отключает рекурсивный обход</param>
        /// <param name="ForceBackUp">Включает / отключает принудительное создание резервных копий</param>
        public static void FormShowCleanup(List<String> Paths, string LText, string ResultMsg, string BackUpDir, string CheckBin, bool ReadOnly = false, bool NoAuto = false, bool Recursive = true, bool ForceBackUp = false)
        {
            try
            {
                if (!ProcessManager.IsProcessRunning(Path.GetFileNameWithoutExtension(CheckBin))) { using (FrmCleaner FCl = new FrmCleaner(Paths, BackUpDir, LText, ResultMsg, ReadOnly, NoAuto, Recursive, ForceBackUp)) { FCl.ShowDialog(); } } else { MessageBox.Show(String.Format(AppStrings.PS_AppRunning, CheckBin), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Удаляет указанные файлы или каталоги с выводом прогресса.
        /// </summary>
        /// <param name="Path">Пути к каталогам или файлам для очистки</param>
        public static void FormShowRemoveFiles(List<String> Paths)
        {
            using (FrmRmWrk Rm = new FrmRmWrk(Paths))
            {
                Rm.ShowDialog();
            }
        }

        /// <summary>
        /// Вызывает форму выбора SteamID из заданных значений.
        /// </summary>
        /// <param name="SteamIDs">Список доступных SteamID</param>
        /// <returns>Выбранный пользователем SteamID</returns>
        public static string FormShowIDSelect(List<String> SteamIDs)
        {
            // Проверяем количество SteamID в списке...
            if (SteamIDs.Count < 1) { throw new ArgumentOutOfRangeException(AppStrings.SD_NEParamsFormException); }

            // Создаём переменную для хранения результата...
            string Result = String.Empty;

            // Вызываем форму и получам результат выбора пользователя...
            using (FrmStmSelector StmSel = new FrmStmSelector(SteamIDs))
            {
                if (StmSel.ShowDialog() == DialogResult.OK)
                {
                    Result = StmSel.SteamID;
                }
            }

            // Возвращаем результат...
            return Result;
        }

        /// <summary>
        /// Вызывает форму выбора конфига из заданных значений.
        /// </summary>
        /// <param name="Cfgs">Список доступных конфигов</param>
        /// <returns>Выбранный пользователем конфиг</returns>
        public static string FormShowCfgSelect(List<String> Cfgs)
        {
            // Проверяем количество конфигов в списке...
            if (Cfgs.Count < 1) { throw new ArgumentOutOfRangeException(AppStrings.CS_NEParamsFormException); }

            // Создаём переменную для хранения результата...
            string Result = String.Empty;

            // Вызываем форму и получам результат выбора пользователя...
            using (FrmCfgSelector CfgSel = new FrmCfgSelector(Cfgs))
            {
                if (CfgSel.ShowDialog() == DialogResult.OK)
                {
                    Result = CfgSel.Config;
                }
            }

            // Возвращаем результат...
            return Result;
        }

        /// <summary>
        /// Вызывает форму модуля создания отчётов для Техподдержки.
        /// </summary>
        /// <param name="AppUserDir">Путь к каталогу пользователя программы</param>
        /// <param name="FullSteamPath">Путь к каталогу установки Steam</param>
        /// <param name="FullCfgPath">Путь к каталогу с конфигами выбранной игры</param>
        /// <param name="SelectedGame">Конфигурация выбранной в главном окне игры</param>
        public static void FormShowRepBuilder(string AppUserDir, string FullSteamPath, SourceGame SelectedGame)
        {
            using (FrmRepBuilder RBF = new FrmRepBuilder(AppUserDir, FullSteamPath, SelectedGame))
            {
                RBF.ShowDialog();
            }
        }

        /// <summary>
        /// Вызывает форму модуля установки кастомного контента в игру.
        /// </summary>
        /// <param name="FullGamePath">Путь к каталогу установки выбранной игры</param>
        /// <param name="IsUsingUserDir">Использует ли игра отдельный кастомный каталог</param>
        /// <param name="CustomInstallDir">Путь к каталогу кастомных файлов</param>
        public static void FormShowInstaller(string FullGamePath, bool IsUsingUserDir, string CustomInstallDir)
        {
            using (FrmInstaller InstF = new FrmInstaller(FullGamePath, IsUsingUserDir, CustomInstallDir))
            {
                InstF.ShowDialog();
            }
        }

        /// <summary>
        /// Вызывает форму "О программе".
        /// </summary>
        public static void FormShowAboutApp()
        {
            using (FrmAbout AboutFrm = new FrmAbout())
            {
                AboutFrm.ShowDialog();
            }
        }

        /// <summary>
        /// Вызывает форму модуля обновления программы.
        /// </summary>
        /// <param name="UserAgent">Заголовок HTTP User-Agent, который будет отправляться при проверке обновлений</param>
        /// <param name="FullAppPath">Полный путь к каталогу установки программы</param>
        /// <param name="AppUserDir">Путь к каталогу пользователя программы</param>
        /// <param name="Platform">Тип ОС, под которой запущено приложение</param>
        public static void FormShowUpdater(string UserAgent, string FullAppPath, string AppUserDir, CurrentPlatform Platform)
        {
            using (FrmUpdate UpdFrm = new FrmUpdate(UserAgent, FullAppPath, AppUserDir, Platform))
            {
                UpdFrm.ShowDialog();
            }
        }

        /// <summary>
        /// Вызывает форму модуля настроек программы.
        /// </summary>
        public static void FormShowOptions()
        {
            using (FrmOptions OptsFrm = new FrmOptions())
            {
                OptsFrm.ShowDialog();
            }
        }

        /// <summary>
        /// Вызывает форму модуля отключения системных клавиш.
        /// </summary>
        public static void FormShowKBHelper()
        {
            using (FrmKBHelper KBHlp = new FrmKBHelper())
            {
                KBHlp.ShowDialog();
            }
        }

        /// <summary>
        /// Вызывает форму модуля просмотра отладочных журналов.
        /// </summary>
        /// <param name="LogFile">Путь к файлу журнала</param>
        public static void FormShowLogViewer(string LogFile)
        {
            using (FrmLogView Lv = new FrmLogView(LogFile))
            {
                Lv.ShowDialog();
            }
        }

        /// <summary>
        /// Вызывает форму модуля очистки кэшей клиента Steam.
        /// </summary>
        /// <param name="FullSteamPath">Путь к каталогу установки Steam</param>
        /// <param name="FullBackUpDirPath">Путь к каталогу хранения резервных копий</param>
        /// <param name="SteamAppsDirName">Платформо-зависимое название каталога SteamApps</param>
        /// <param name="SteamProcName">Платформо-зависимое имя процесса Steam</param>
        public static void FormShowStmCleaner(string FullSteamPath, string FullBackUpDirPath, string SteamAppsDirName, string SteamProcName)
        {
            using (FrmStmClean StmCln = new FrmStmClean(FullSteamPath, FullBackUpDirPath, SteamAppsDirName, SteamProcName))
            {
                StmCln.ShowDialog();
            }
        }

        /// <summary>
        /// Вызывает форму модуля управления отключёнными игроками.
        /// </summary>
        /// <param name="Banlist">Путь к базе отключённых игроков</param>
        /// <param name="FullBackUpDirPath">Путь к каталогу хранения резервных копий</param>
        public static void FormShowMuteManager(string Banlist, string FullBackUpDirPath)
        {
            using (FrmMute FMm = new FrmMute(Banlist, FullBackUpDirPath))
            {
                FMm.ShowDialog();
            }
        }

        /// <summary>
        /// Изменяет размеры столбцов в DataGridView, т.к. сама платформа CLR
        /// не способна сделать это автоматически.
        /// </summary>
        /// <param name="ScaleSource">Ссылка на контрол DataGridView</param>
        /// <param name="ScaleFactor">Множитель</param>
        public static void ScaleColumnsInControl(DataGridView ScaleSource, SizeF ScaleFactor)
        {
            foreach (DataGridViewColumn Column in ScaleSource.Columns)
            {
                Column.Width = (int)Math.Round(Column.Width * ScaleFactor.Width);
            }
        }

        /// <summary>
        /// Изменяет размеры столбцов в ListView, т.к. сама платформа CLR
        /// не способна сделать это автоматически.
        /// </summary>
        /// <param name="ScaleSource">Ссылка на контрол ListView</param>
        /// <param name="ScaleFactor">Множитель</param>
        public static void ScaleColumnsInControl(ListView ScaleSource, SizeF ScaleFactor)
        {
            foreach (ColumnHeader Column in ScaleSource.Columns)
            {
                Column.Width = (int)Math.Round(Column.Width * ScaleFactor.Width);
            }
        }

        /// <summary>
        /// Изменяет размеры столбцов в StatusStrip, т.к. сама платформа CLR
        /// не способна сделать это автоматически.
        /// </summary>
        /// <param name="ScaleSource">Ссылка на контрол StatusStrip</param>
        /// <param name="ScaleFactor">Множитель</param>
        public static void ScaleColumnsInControl(StatusStrip ScaleSource, SizeF ScaleFactor)
        {
            foreach (ToolStripItem StatusBarItem in ScaleSource.Items)
            {
                StatusBarItem.Width = (int)Math.Round(StatusBarItem.Width * ScaleFactor.Width);
            }
        }
    }
}
