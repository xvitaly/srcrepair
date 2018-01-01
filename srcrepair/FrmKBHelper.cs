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
