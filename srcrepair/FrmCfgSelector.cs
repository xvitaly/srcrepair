/*
 * Модуль выбора конфига из списка.
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
    }
}
