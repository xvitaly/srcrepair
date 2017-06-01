/*
 * Модуль выбора SteamID.
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
 * Официальный блог EasyCoding Team: https://www.easycoding.org/
 * Официальная страница проекта: https://www.easycoding.org/projects/srcrepair
 * 
 * Более подробная инфорация о программе в readme.txt,
 * о лицензии - в GPL.txt.
*/
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace srcrepair
{
    /// <summary>
    /// Класс формы модуля выбора SteamID.
    /// </summary>
    public partial class FrmStmSelector : Form
    {
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
            ProcessManager.OpenWebPage(String.Format(Properties.Resources.MM_CommunityURL, SteamConv.ConvSidv3Sid64(SD_IDSel.Text)));
        }
    }
}
