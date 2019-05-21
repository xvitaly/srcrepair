/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2019 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2019 EasyCoding Team.
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
using srcrepair.core;

namespace srcrepair
{
    /// <summary>
    /// Класс формы "О программе".
    /// </summary>
    partial class FrmAbout : Form
    {
        /// <summary>
        /// Конструктор класса формы "О программе".
        /// </summary>
        public FrmAbout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "OK".
        /// </summary>
        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Метод, срабатывающий создании формы.
        /// </summary>
        private void FrmAbout_Load(object sender, EventArgs e)
        {
            // Заполняем информацию о версии, копирайте...
            string AppProduct = CurrentApp.AppProduct;
            Text = String.Format("About {0}...", AppProduct);
            labelProductName.Text = AppProduct;
            #if DEBUG
            labelProductName.Text += " DEBUG";
            #endif
            labelVersion.Text = String.Format("Version: {0}", CurrentApp.AppVersion);
            labelCopyright.Text = CurrentApp.AppCopyright;
            labelCompanyName.Text = CurrentApp.AppCompany;

            // Получаем текущий месяц и число для проверки на НГ...
            DateTime XDate = DateTime.Now;
            int XMonth = XDate.Month;
            int XDay = XDate.Day;

            // Проверяем систему на НГ (диапазон от 20.12.XXXX до 10.1.XXXX+1)...
            if ((XMonth == 12 && (XDay >= 20 && XDay <= 31)) || (XMonth == 1 && (XDay >= 1 && XDay <= 10)))
            {
                // НГ! Меняем логотип программы на специально заготовленный...
                iconApp.Image = Properties.Resources.Xmas;
            }
        }
    }
}
