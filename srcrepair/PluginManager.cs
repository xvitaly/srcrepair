/*
 * Класс для взаимодействия с отдельным формами SRC Repair.
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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace srcrepair
{
    /// <summary>
    /// Класс для взаимодействия с отдельным формами и расширениями.
    /// </summary>
    public static class PluginManager
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
            if (SteamIDs.Count < 1) { throw new ArgumentOutOfRangeException("Not enough SteamIDs in list."); }

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

        public static void FormShowRepBuilder(string AppUserDir, string FullSteamPath, string FullCfgPath)
        {
            using (FrmRepBuilder RBF = new FrmRepBuilder(AppUserDir, FullSteamPath, FullCfgPath))
            {
                RBF.ShowDialog();
            }
        }
    }
}
