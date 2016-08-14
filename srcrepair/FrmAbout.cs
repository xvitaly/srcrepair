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

        #region Assembly Attribute Accessors

        /// <summary>
        /// Возвращает название приложения (из ресурса сборки).
        /// </summary>
        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        /// <summary>
        /// Возвращает версию приложения (из ресурса сборки).
        /// </summary>
        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Возвращает описание приложения (из ресурса сборки).
        /// </summary>
        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        /// <summary>
        /// Возвращает название продукта (из ресурса сборки).
        /// </summary>
        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <summary>
        /// Возвращает копирайты приложения (из ресурса сборки).
        /// </summary>
        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        /// <summary>
        /// Возвращает название компании-разработчика приложения (из ресурса сборки).
        /// </summary>
        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnContact_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("mailto:srcrepair@easycoding.org?subject=Contact author of SRC Repair");
            }
            catch
            {
                System.Diagnostics.Process.Start("http://www.easycoding.org/about");
            }
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            // Заполняем информацию о версии, копирайте...
            Text = String.Format("About {0}...", AssemblyTitle);
            labelProductName.Text = AssemblyProduct + " OSE";
            #if DEBUG
            labelProductName.Text += " DEBUG";
            #endif
            labelVersion.Text = String.Format("Version: {0}", AssemblyVersion);
            labelCopyright.Text = AssemblyCopyright;
            labelCompanyName.Text = AssemblyCompany;

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
