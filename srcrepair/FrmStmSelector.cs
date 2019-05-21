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
using System.Collections.Generic;
using System.Windows.Forms;
using NLog;
using srcrepair.core;

namespace srcrepair.gui
{
    /// <summary>
    /// Класс формы модуля выбора SteamID.
    /// </summary>
    public partial class FrmStmSelector : Form
    {
        /// <summary>
        /// Управляет записью событий в журнал.
        /// </summary>
        private Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Хранит и возвращает SteamID.
        /// </summary>
        public string SteamID { get; private set; }

        /// <summary>
        /// Хранит список полученных SteamID.
        /// </summary>
        private List<String> SteamIDs { get; set; }

        /// <summary>
        /// Конструктор класса формы модуля выбора SteamID.
        /// </summary>
        /// <param name="S">Список SteamID для выбора</param>
        public FrmStmSelector(List<String> S)
        {
            InitializeComponent();
            SteamIDs = S;
        }

        /// <summary>
        /// Метод, срабатывающий при возникновении события "загрузка формы".
        /// </summary>
        private void FrmStmSelector_Load(object sender, EventArgs e)
        {
            // Указываем откуда следует брать список со SteamID...
            SD_IDSel.DataSource = SteamIDs;
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "OK".
        /// </summary>
        private void ST_OK_Click(object sender, EventArgs e)
        {
            // Возвращаем результат...
            SteamID = SD_IDSel.Text;

            // Возвращаем результат формы "успех"...
            DialogResult = DialogResult.OK;

            // Закрываем форму...
            Close();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Отмена".
        /// </summary>
        private void ST_Cancel_Click(object sender, EventArgs e)
        {
            // Возвращаем результат формы "отменено"...
            DialogResult = DialogResult.Cancel;

            // Закрываем форму...
            Close();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "Показать профиль в браузере".
        /// </summary>
        private void SD_Follow_Click(object sender, EventArgs e)
        {
            // Открываем URL в браузере по умолчанию...
            try
            {
                ProcessManager.OpenWebPage(String.Format(Properties.Resources.MM_CommunityURL, SteamConv.ConvSidv3Sid64(SD_IDSel.Text)));
            }
            catch (Exception Ex)
            {
                Logger.Warn(Ex, AppStrings.AppDbgExUrlStmSel);
            }
        }
    }
}
