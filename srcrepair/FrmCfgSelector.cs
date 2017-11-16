/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2017 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2017 EasyCoding Team.
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
using System.Windows.Forms;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля выбора конфига.
    /// </summary>
    public partial class FrmCfgSelector : Form
    {
        /// <summary>
        /// Хранит и возвращает выбранный пользователем конфиг.
        /// </summary>
        public string Config { get; private set; }

        /// <summary>
        /// Хранит список доступных конфигов.
        /// </summary>
        private List<String> Configs { get; set; }

        /// <summary>
        /// Конструктор класса формы модуля выбора конфига.
        /// </summary>
        /// <param name="C">Список конфигов для выбора</param>
        public FrmCfgSelector(List<String> C)
        {
            InitializeComponent();
            Configs = C;
        }

        /// <summary>
        /// Метод, срабатывающий при возникновении события "загрузка формы".
        /// </summary>
        private void FrmCfgSelector_Load(object sender, EventArgs e)
        {
            // Указываем откуда следует брать список с конфигами...
            CS_CfgSel.DataSource = Configs;
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "OK".
        /// </summary>
        private void CS_OK_Click(object sender, EventArgs e)
        {
            // Возвращаем результат...
            Config = CS_CfgSel.Text;

            // Возвращаем результат формы "успех"...
            DialogResult = DialogResult.OK;

            // Закрываем форму...
            Close();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Отмена".
        /// </summary>
        private void CS_Cancel_Click(object sender, EventArgs e)
        {
            // Возвращаем результат формы "отменено"...
            DialogResult = DialogResult.Cancel;

            // Закрываем форму...
            Close();
        }

        /// <summary>
        /// Метод, срабатывающий при выборе одного из конфигов.
        /// </summary>
        private void CS_CfgSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Указываем полный путь во всплывающей подсказке...
            CS_ToolTip.SetToolTip((ComboBox)sender, ((ComboBox)sender).Text);
        }
    }
}
