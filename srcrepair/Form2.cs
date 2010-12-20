using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace srcrepair
{
    partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}...", AssemblyTitle);
            this.labelProductName.Text = String.Format(AssemblyProduct + " OSE {0}", AssemblyVersion);
            this.labelVersion.Text = String.Format("Version: {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            //this.textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

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

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

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
                System.Diagnostics.Process.Start("skype:easycoding?chat");
            }
            catch
            {
                System.Diagnostics.Process.Start("steam://friends/message/76561197994204416");
            }
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            // Проверяем систему на НГ (диапазон от 30.12.XXXX до 10.1.XXXX)...
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
