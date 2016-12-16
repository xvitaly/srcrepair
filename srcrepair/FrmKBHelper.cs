/*
 * Модуль отключения системных клавиш SRC Repair.
 * 
 * Copyright 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2017 EasyCoding Team.
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
using Microsoft.Win32;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля отключения системных клавиш.
    /// </summary>
    public partial class FrmKBHelper : Form
    {
        /// <summary>
        /// Конструктор класса формы модуля отключения системных клавиш.
        /// </summary>
        public FrmKBHelper()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Записывает массив байт в реестр.
        /// </summary>
        private void WriteKBS(byte[] Value)
        {
            RegistryKey ResKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Keyboard Layout", true);
            ResKey.SetValue("Scancode Map", Value, RegistryValueKind.Binary);
            ResKey.Close();
        }

        /// <summary>
        /// Удаляет указанное значение из реестра.
        /// </summary>
        private void DeleteKBS(string Value)
        {
            RegistryKey ResKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Keyboard Layout", true);
            ResKey.DeleteValue(Value);
            ResKey.Close();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии кнопки отключения левой клавиши WIN.
        /// </summary>
        private void Dis_LWIN_Click(object sender, EventArgs e)
        {
            // Отключаем левую клавишу WIN...
            // 00 00 00 00 00 00 00 00 02 00 00 00 00 00 5B E0 00 00 00 00
            if (MessageBox.Show(String.Format(AppStrings.KB_ExQuestion, ((Button)sender).Text.ToLower()), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    WriteKBS(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 91, 224, 0, 0, 0, 0 });
                    MessageBox.Show(AppStrings.KB_ExSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(AppStrings.KB_ExException, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии кнопки отключения обеих клавиш WIN.
        /// </summary>
        private void Dis_BWIN_Click(object sender, EventArgs e)
        {
            // Отключаем обе клавиши WIN...
            // 00 00 00 00 00 00 00 00 03 00 00 00 00 00 5B E0 00 00 5C E0 00 00 00 00
            if (MessageBox.Show(String.Format(AppStrings.KB_ExQuestion, ((Button)sender).Text.ToLower()), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    WriteKBS(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 91, 224, 0, 0, 92, 224, 0, 0, 0, 0 });
                    MessageBox.Show(AppStrings.KB_ExSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(AppStrings.KB_ExException, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии кнопки отключения правой клавиши WIN и CONTEXT.
        /// </summary>
        private void Dis_RWinMnu_Click(object sender, EventArgs e)
        {
            // Отключаем правую клавишу WIN и MENU...
            // 00 00 00 00 00 00 00 00 03 00 00 00 00 00 5C E0 00 00 5D E0 00 00 00 00
            if (MessageBox.Show(String.Format(AppStrings.KB_ExQuestion, ((Button)sender).Text.ToLower()), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    WriteKBS(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 91, 224, 0, 0, 93, 224, 0, 0, 0, 0 });
                    MessageBox.Show(AppStrings.KB_ExSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(AppStrings.KB_ExException, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии кнопки отключения обеих клавиш WIN и CONTEXT.
        /// </summary>
        private void Dis_BWinMnu_Click(object sender, EventArgs e)
        {
            // Отключаем обе WIN и MENU...
            // 00 00 00 00 00 00 00 00 04 00 00 00 00 00 5B E0 00 00 5C E0 00 00 5D E0 00 00 00 00
            if (MessageBox.Show(String.Format(AppStrings.KB_ExQuestion, ((Button)sender).Text.ToLower()), Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    WriteKBS(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 92, 224, 0, 0, 93, 224, 0, 0, 0, 0 });
                    MessageBox.Show(AppStrings.KB_ExSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(AppStrings.KB_ExException, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии кнопки отмены произведённых изменений.
        /// </summary>
        private void Dis_Restore_Click(object sender, EventArgs e)
        {
            // Восстанавливаем настройки по умолчанию...
            if (MessageBox.Show(AppStrings.KB_ExRestore, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DeleteKBS("Scancode Map");
                    MessageBox.Show(AppStrings.KB_ExSuccess, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception Ex)
                {
                    CoreLib.HandleExceptionEx(AppStrings.KB_ExException, Properties.Resources.AppName, Ex.Message, Ex.Source, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
