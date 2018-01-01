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
using System.Windows.Forms;
using System.IO;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля просмотра журналов.
    /// </summary>
    public partial class FrmLogView : Form
    {
        /// <summary>
        /// Хранит путь к файлу журнала.
        /// </summary>
        private string LogFileName { get; set; }

        /// <summary>
        /// Конструктор класса формы модуля просмотра журналов.
        /// </summary>
        /// <param name="LogFile">Путь к файлу журнала</param>
        public FrmLogView(string LogFile)
        {
            InitializeComponent();
            LogFileName = LogFile;
        }

        /// <summary>
        /// Непосредственно загружает содержимое текстового файла в TextBox на форме.
        /// </summary>
        /// <param name="FileName">Путь к текстовому файлу</param>
        private void LoadTextFile(string FileName)
        {
            LV_LogArea.Clear();
            LV_LogArea.AppendText(File.ReadAllText(FileName));
        }

        /// <summary>
        /// Загружает содержимое журнала в TextBox на форме.
        /// </summary>
        /// <param name="FileName">Путь к файлу журнала</param>
        private void LoadLog(string FileName)
        {
            try { LoadTextFile(FileName); } catch (Exception Ex) { CoreLib.HandleExceptionEx(AppStrings.LV_LoadFailed, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }

        /// <summary>
        /// Метод, срабатывающий при возникновении события "загрузка формы".
        /// </summary>
        private void FrmLogView_Load(object sender, EventArgs e)
        {
            // Считаем содержимое выбранного файла...
            LoadLog(LogFileName);
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Перечитать файл".
        /// </summary>
        private void LV_MenuFileReload_Click(object sender, EventArgs e)
        {
            // Перечитаем содержимое журнала...
            LoadLog(LogFileName);
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку выхода.
        /// </summary>
        private void LV_MenuFileExit_Click(object sender, EventArgs e)
        {
            // Закроем модуль...
            Close();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "О модуле".
        /// </summary>
        private void LV_MenuHelpAbout_Click(object sender, EventArgs e)
        {
            // Выводим сообщение с краткой информацией о плагине...
            FormManager.FormShowAboutApp();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Очистить журнал".
        /// </summary>
        private void LV_MunuFileClearLog_Click(object sender, EventArgs e)
        {
            // Очистим форму...
            LV_LogArea.Clear();

            // Очистим файл журнала...
            try { if (File.Exists(LogFileName)) { File.Delete(LogFileName); FileManager.CreateFile(LogFileName); } } catch (Exception Ex) { CoreLib.HandleExceptionEx(AppStrings.LV_ClearEx, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning); }
        }
    }
}
