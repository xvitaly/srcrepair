/*
 * Модуль формы "О программе" SRC Repair.
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
using System.Reflection;
using System.Windows.Forms;

namespace srcrepair
{
    /// <summary>
    /// Класс формы "О программе".
    /// </summary>
    partial class frmAbout : Form
    {
        /// <summary>
        /// Конструктор класса формы "О программе".
        /// </summary>
        public frmAbout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод, срабатывающий при нажатии на кнопку "OK".
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Метод, срабатывающий создании формы.
        /// </summary>
        private void frmAbout_Load(object sender, EventArgs e)
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

            // Проверяем систему на НГ (диапазон от 20.12.XXXX до 10.1.XXXX+1)...
            DateTime XDate = DateTime.Now; // Получаем текущую дату...
            if (((Convert.ToInt32(XDate.Month) == 12) && ((Convert.ToInt32(XDate.Day) >= 20)
                && (Convert.ToInt32(XDate.Day) <= 31))) || ((Convert.ToInt32(XDate.Month) == 1)
                    && ((Convert.ToInt32(XDate.Day) >= 1) && (Convert.ToInt32(XDate.Day) <= 10))))
            {
                // НГ! Меняем логотип программы на специально заготовленный...
                iconApp.Image = Properties.Resources.Xmas;
            }
        }
    }
}
