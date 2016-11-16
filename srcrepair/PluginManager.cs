using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace srcrepair
{
    public static class PluginManager
    {
        /// <summary>
        /// Начинает загрузку с указанного URL с подробным отображением процесса.
        /// </summary>
        /// <param name="URI">URL для загрузки</param>
        /// <param name="FileName">Путь для сохранения</param>
        public static void DownloadFileEx(string URI, string FileName)
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
        public static void ExtractFiles(string ArchName, string DestDir)
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
        public static void OpenCleanupWindow(List<String> Paths, string LText, string ResultMsg, string BackUpDir, string CheckBin, bool ReadOnly = false, bool NoAuto = false, bool Recursive = true, bool ForceBackUp = false)
        {
            try
            {
                if (!CoreLib.IsProcessRunning(Path.GetFileNameWithoutExtension(CheckBin))) { using (FrmCleaner FCl = new FrmCleaner(Paths, BackUpDir, LText, ResultMsg, ReadOnly, NoAuto, Recursive, ForceBackUp)) { FCl.ShowDialog(); } } else { MessageBox.Show(String.Format(AppStrings.PS_AppRunning, CheckBin), Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            catch (Exception Ex) { CoreLib.WriteStringToLog(Ex.Message); }
        }

        /// <summary>
        /// Удаляет указанные файлы или каталоги с выводом прогресса.
        /// </summary>
        /// <param name="Path">Пути к каталогам или файлам для очистки</param>
        public static void RemoveFileDirectoryEx(List<String> Paths)
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
        public static string OpenSteamIDSelector(List<String> SteamIDs)
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
    }
}
