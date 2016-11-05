using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace srcrepair
{
    public partial class FrmStmSelector : Form
    {
        /// <summary>
        /// Хранит и возвращает SteamID.
        /// </summary>
        public string SteamID { get; private set; }

        /// <summary>
        /// Хранит список полученных SteamID.
        /// </summary>
        private List<String> SteamIDs;

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
    }
}
