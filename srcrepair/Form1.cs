/*
 * Основной модуль программы SRC Repair.
 * 
 * Copyright 2011 EasyCoding Team (ECTeam).
 * Copyright 2005 - 2011 EasyCoding Team.
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
using System.ComponentModel;
using System.Data;
using System.Drawing; // аналогично Forms...
using System.Linq;
using System.Text;
using System.Windows.Forms; // для работы с формами...
using System.IO; // для работы с файлами...
using System.Diagnostics; // для управления процессами...
using Microsoft.Win32; // для работы с реестром...
using System.Reflection; // для управления сборками...
using System.Globalization; // для управления локализациями...
using System.Resources; // для управления ресурсами...
using System.Net; // для скачивания файлов...
using System.Xml; // для разбора (парсинга) XML...

namespace srcrepair
{
    public partial class frmMainW : Form
    {
        private ResourceManager RM; // Описываем менеджер ресурсов...
        public frmMainW()
        {
            // Инициализация...
            InitializeComponent();
            
            // Создаём экземпляр менеджера ресурсов с нужным нам ресурсом...
            RM = new ResourceManager("srcrepair.AppStrings", typeof(frmMainW).Assembly);
            
            // Импортируем настройки из предыдущей версии...
            if (Properties.Settings.Default.CallUpgrade)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.CallUpgrade = false;
            }
            
            // Проверим на первый запуск...
            if (Properties.Settings.Default.IsFirstRun)
            {
                // Это первый запуск программы от текущего профиля или из текущего каталога...
                // Установим дату последней проверки обновлений в сегодняшнюю...
                Properties.Settings.Default.LastUpdateTime = DateTime.Now;
                // Первый запуск состоялся, поэтому переведём значение переменной в false...
                Properties.Settings.Default.IsFirstRun = false;
            }
        }

        #region Internal Variables

        /* В этой переменной будем хранить имя открытого конфига для служебных целей. */
        private string CFGFileName;

        #endregion

        #region Internal Functions

        /// <summary>
        /// Устанавливает требуемый FPS-конфиг.
        /// </summary>
        /// <param name="ConfName">Имя конфига</param>
        /// <param name="AppPath">Путь к программе SRC Repair</param>
        /// <param name="DirName">Короткое имя игры</param>
        /// <param name="DestPath">Каталог назначения</param>
        private void InstallConfigNow(string ConfName, string AppPath, string DirName, string DestPath)
        {
            // Устанавливаем...
            File.Copy(Path.Combine(AppPath, "cfgs", DirName, ConfName), Path.Combine(DestPath, "autoexec.cfg"), true);
        }

        /// <summary>
        /// Создаёт резервную копию конфига, имя которого передано в параметре.
        /// </summary>
        /// <param name="ConfName">Имя конфига без пути</param>
        /// <param name="ConfigDir">Путь к каталогу с конфигом</param>
        /// <param name="BackUpDir">Путь к каталогу с резервными копиями</param>
        private void CreateBackUpNow(string ConfName, string ConfigDir, string BackUpDir)
        {
            // Сначала проверим, существует ли запрошенный файл...
            if (File.Exists(Path.Combine(ConfigDir, ConfName)))
            {
                // Проверяем чтобы каталог для бэкапов существовал...
                if (!(Directory.Exists(BackUpDir)))
                {
                    // Каталоги не существуют. Создадим общий каталог для резервных копий...
                    Directory.CreateDirectory(BackUpDir);
                }

                try
                {
                    // Копируем оригинальный файл в файл бэкапа...
                    File.Copy(Path.Combine(ConfigDir, ConfName), Path.Combine(BackUpDir, ConfName + "." + CoreLib.WriteDateToString(DateTime.Now, true)), true);
                }
                catch
                {
                    // Произошло исключение. Уведомим пользователя об этом...
                    MessageBox.Show(RM.GetString("BackUpCreationFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Отображает InputBox. Источник данной функции: http://www.csharp-examples.net/inputbox/
        /// </summary>
        /// <param name="title">Заголовок InputBox'а</param>
        /// <param name="promptText">Текст сообщения InputBox'а</param>
        /// <param name="value">Стартовое значение и результат</param>
        /// <returns>Значение типа DialogResult</returns>
        private DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = RM.GetString("InputBoxCancelBtnName");
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        /// <summary>
        /// Сохраняет содержимое таблицы в файл конфигурации, указанный в
        /// параметре. Используется в Save и SaveAs Редактора конфигов.
        /// </summary>
        /// <param name="Path">Полный путь к файлу конфига</param>
        /// <param name="XAppName">Название программы</param>
        private void WriteTableToFileNow(string Path, string XAppName)
        {
            // Начинаем сохранять содержимое редактора в файл...
            using (StreamWriter CFile = new StreamWriter(Path))
            {
                CFile.WriteLine("// Generated by " + XAppName + " //"); // Вставляем сообщение ;-).
                CFile.WriteLine(); // вставляем пустую строку
                for (int i = 0; i < CE_Editor.Rows.Count; i++) // запускаем цикл
                {
                    CFile.Write(CE_Editor.Rows[i].Cells[0].Value); // вставляем содержимое первого столбца (название переменной)
                    CFile.Write(" "); // вставляем пробел
                    CFile.WriteLine(CE_Editor.Rows[i].Cells[1].Value); // вставляем содержимое второго столбца (значение переменной)
                }
                CFile.Close(); // закрываем файл
            }
        }

        /// <summary>
        /// Используется для записи значений в таблицу Редактора конфигов.
        /// Используется делегатом. Прямой вызов не допускается.
        /// </summary>
        /// <param name="Cv">Название переменной</param>
        /// <param name="Cn">Значение переменной</param>
        private void AddRowToTable(string Cv, string Cn)
        {
            CE_Editor.Rows.Add(Cv, Cn);
        }

        /// <summary>
        /// Используется для создания резервной копии выбранной ветки
        /// реестра в переданный в параметре файл.
        /// </summary>
        /// <param name="RKey">Ветка реестра для резервирования</param>
        /// <param name="FileName">Имя файла резервной копии</param>
        /// <param name="DestDir">Каталог с резервными копиями</param>
        private void CreateRegBackUpNow(string RKey, string FileName, string DestDir)
        {
            // Генерируем строку с параметрами...
            string Params = @"/ea """ + DestDir + FileName + "_" + CoreLib.WriteDateToString(DateTime.Now, true) + @".reg""" + " " + RKey;
            // Запускаем и ждём завершения...
            CoreLib.StartProcessAndWait("regedit.exe", Params);
        }

        /// <summary>
        /// Возвращает описание переданной в качестве параметра переменной, получая
        /// эту информацию из ресурса CVList с учётом локализации.
        /// </summary>
        /// <param name="CVar">Название переменной</param>
        /// <returns>Описание переменной с учётом локализации</returns>
        private string GetCVDescription(string CVar)
        {
            ResourceManager DM = new ResourceManager("srcrepair.CVList", typeof(frmMainW).Assembly);
            return DM.GetString(CVar);
        }

        /// <summary>
        /// Разрешает/блокирует кнопки, отвечающие за очистку (актуально
        /// для NCF-приложений).
        /// </summary>
        /// <param name="BStatus">Новый статус</param>
        private void EnableCleanButtons(bool BStatus)
        {
            PS_RemCustMaps.Enabled = BStatus;
            //PS_RemDnlCache.Enabled = BStatus;
            //PS_RemOldSpray.Enabled = BStatus;
            //PS_RemOldCfgs.Enabled = BStatus;
            PS_RemGraphCache.Enabled = BStatus;
            PS_RemSoundCache.Enabled = BStatus;
            PS_RemNavFiles.Enabled = BStatus;
            //PS_RemScreenShots.Enabled = BStatus;
            //PS_RemDemos.Enabled = BStatus;
            PS_RemGraphOpts.Enabled = BStatus;
            PS_RemOldBin.Enabled = BStatus;
        }

        /// <summary>
        /// Отображает диалоговое окно менеджера быстрой очистки.
        /// </summary>
        /// <param name="Path">Путь к каталогу очистки</param>
        /// <param name="Mask">Маска файлов, подлежащих очистке</param>
        /// <param name="LText">Текст заголовка</param>
        private void OpenCleanupWindow(string Path, string Mask, string LText)
        {
            frmCleaner FCl = new frmCleaner(Path, Mask, LText);
            FCl.ShowDialog();
        }

        /// <summary>
        /// Определяет установленные игры и заполняет комбо-бокс выбора
        /// доступных управляемых игр.
        /// </summary>
        /// <param name="SteamPath">Путь к клиенту Steam</param>
        /// <param name="SteamLogin">Логин Steam</param>
        private void DetectInstalledGames(string SteamPath, string SteamLogin)
        {
            // Очистим список игр...
            AppSelector.Items.Clear();

            // Опишем заготовки путей для GCF и NCF...
            string SearchPathGCF = CoreLib.IncludeTrDelim(Path.Combine(SteamPath, "steamapps", SteamLogin));
            string SearchPathNCF = CoreLib.IncludeTrDelim(Path.Combine(SteamPath, "steamapps", "common"));

            // Начинаем парсить...
            XmlDocument XMLD = new XmlDocument(); // Создаём объект документа XML...
            FileStream XMLFS = new FileStream(Properties.Settings.Default.GameListFile, FileMode.Open, FileAccess.Read); // Создаём поток с XML-файлом...
            XMLD.Load(XMLFS); // Загружаем поток в объект XML документа...
            XmlNodeList XMLNList = XMLD.GetElementsByTagName("Game"); // Получаем список игр с параметрами...
            for (int i = 0; i < XMLNList.Count; i++) // Обходим полученный список в цикле...
            {
                XmlElement GameID = (XmlElement)XMLD.GetElementsByTagName("Game")[i]; // Получаем элемент...
                if (XMLD.GetElementsByTagName("IsGCF")[i].InnerText == "1") // Проверяем GCF или NCF...
                {
                    // GCF-приложение...
                    if (Directory.Exists(CoreLib.IncludeTrDelim(Path.Combine(SearchPathGCF, XMLD.GetElementsByTagName("DirName")[i].InnerText)))) { AppSelector.Items.Add((string)GameID.GetAttribute("Name")); }
                }
                else
                {
                    // NCF-приложение...
                    if (Directory.Exists(CoreLib.IncludeTrDelim(Path.Combine(SearchPathNCF, XMLD.GetElementsByTagName("DirName")[i].InnerText)))) { AppSelector.Items.Add((string)GameID.GetAttribute("Name")); }
                }
            }
            XMLFS.Close(); // Закрываем файловый поток...
        }

        /// <summary>
        /// Записывает настройки GCF-игры в реестр Windows.
        /// </summary>
        /// <param name="SAppName">Краткое имя игры</param>
        private void WriteGCFGameSettings(string SAppName)
        {
            // Запишем в реестр настройки разрешения экрана...
            // По горизонтали (ScreenWidth):
            CoreLib.WriteSRCDWord("ScreenWidth", (int)GT_ResHor.Value, SAppName);

            // По вертикали (ScreenHeight):
            CoreLib.WriteSRCDWord("ScreenHeight", (int)GT_ResVert.Value, SAppName);

            // Запишем в реестр настройки режима запуска приложения (ScreenWindowed):
            switch (GT_ScreenType.SelectedIndex)
            {
                case 0: CoreLib.WriteSRCDWord("ScreenWindowed", 0, SAppName);
                    break;
                case 1: CoreLib.WriteSRCDWord("ScreenWindowed", 1, SAppName);
                    break;
            }

            // Запишем в реестр настройки детализации моделей (r_rootlod):
            switch (GT_ModelQuality.SelectedIndex)
            {
                case 0: CoreLib.WriteSRCDWord("r_rootlod", 2, SAppName);
                    break;
                case 1: CoreLib.WriteSRCDWord("r_rootlod", 1, SAppName);
                    break;
                case 2: CoreLib.WriteSRCDWord("r_rootlod", 0, SAppName);
                    break;
            }

            // Запишем в реестр настройки детализации текстур (mat_picmip):
            switch (GT_TextureQuality.SelectedIndex)
            {
                case 0: CoreLib.WriteSRCDWord("mat_picmip", 2, SAppName);
                    break;
                case 1: CoreLib.WriteSRCDWord("mat_picmip", 1, SAppName);
                    break;
                case 2: CoreLib.WriteSRCDWord("mat_picmip", 0, SAppName);
                    break;
            }

            // Запишем в реестр настройки качества шейдерных эффектов (mat_reducefillrate):
            switch (GT_ShaderQuality.SelectedIndex)
            {
                case 0: CoreLib.WriteSRCDWord("mat_reducefillrate", 1, SAppName);
                    break;
                case 1: CoreLib.WriteSRCDWord("mat_reducefillrate", 0, SAppName);
                    break;
            }

            // Запишем в реестр настройки отражений в воде (r_waterforceexpensive и r_waterforcereflectentities):
            switch (GT_WaterQuality.SelectedIndex)
            {
                case 0:
                    // Simple reflections
                    CoreLib.WriteSRCDWord("r_waterforceexpensive", 0, SAppName);
                    CoreLib.WriteSRCDWord("r_waterforcereflectentities", 0, SAppName);
                    break;
                case 1:
                    // Reflect world
                    CoreLib.WriteSRCDWord("r_waterforceexpensive", 1, SAppName);
                    CoreLib.WriteSRCDWord("r_waterforcereflectentities", 0, SAppName);
                    break;
                case 2:
                    // Reflect all
                    CoreLib.WriteSRCDWord("r_waterforceexpensive", 1, SAppName);
                    CoreLib.WriteSRCDWord("r_waterforcereflectentities", 1, SAppName);
                    break;
            }

            // Запишем в реестр настройки прорисовки теней (r_shadowrendertotexture):
            switch (GT_ShadowQuality.SelectedIndex)
            {
                case 0: CoreLib.WriteSRCDWord("r_shadowrendertotexture", 0, SAppName);
                    break;
                case 1: CoreLib.WriteSRCDWord("r_shadowrendertotexture", 1, SAppName);
                    break;
            }

            // Запишем в реестр настройки коррекции цвета (mat_colorcorrection):
            switch (GT_ColorCorrectionT.SelectedIndex)
            {
                case 0: CoreLib.WriteSRCDWord("mat_colorcorrection", 0, SAppName);
                    break;
                case 1: CoreLib.WriteSRCDWord("mat_colorcorrection", 1, SAppName);
                    break;
            }

            // Запишем в реестр настройки сглаживания (mat_antialias и mat_aaquality):
            switch (GT_AntiAliasing.SelectedIndex)
            {
                case 0:
                    // Нет сглаживания
                    CoreLib.WriteSRCDWord("mat_antialias", 1, SAppName);
                    CoreLib.WriteSRCDWord("mat_aaquality", 0, SAppName);
                    CoreLib.WriteSRCDWord("ScreenMSAA", 0, SAppName); // Дублируем значение mat_antialias
                    CoreLib.WriteSRCDWord("ScreenMSAAQuality", 0, SAppName); // Дублируем значение mat_aaquality
                    break;
                case 1:
                    // 2x MSAA
                    CoreLib.WriteSRCDWord("mat_antialias", 2, SAppName);
                    CoreLib.WriteSRCDWord("mat_aaquality", 0, SAppName);
                    CoreLib.WriteSRCDWord("ScreenMSAA", 2, SAppName);
                    CoreLib.WriteSRCDWord("ScreenMSAAQuality", 0, SAppName);
                    break;
                case 2:
                    // 4x MSAA
                    CoreLib.WriteSRCDWord("mat_antialias", 4, SAppName);
                    CoreLib.WriteSRCDWord("mat_aaquality", 0, SAppName);
                    CoreLib.WriteSRCDWord("ScreenMSAA", 4, SAppName);
                    CoreLib.WriteSRCDWord("ScreenMSAAQuality", 0, SAppName);
                    break;
                case 3:
                    // 8x CSAA
                    CoreLib.WriteSRCDWord("mat_antialias", 4, SAppName);
                    CoreLib.WriteSRCDWord("mat_aaquality", 2, SAppName);
                    CoreLib.WriteSRCDWord("ScreenMSAA", 4, SAppName);
                    CoreLib.WriteSRCDWord("ScreenMSAAQuality", 2, SAppName);
                    break;
                case 4:
                    // 16x CSAA
                    CoreLib.WriteSRCDWord("mat_antialias", 4, SAppName);
                    CoreLib.WriteSRCDWord("mat_aaquality", 4, SAppName);
                    CoreLib.WriteSRCDWord("ScreenMSAA", 4, SAppName);
                    CoreLib.WriteSRCDWord("ScreenMSAAQuality", 4, SAppName);
                    break;
                case 5:
                    // 8x MSAA
                    CoreLib.WriteSRCDWord("mat_antialias", 8, SAppName);
                    CoreLib.WriteSRCDWord("mat_aaquality", 0, SAppName);
                    CoreLib.WriteSRCDWord("ScreenMSAA", 8, SAppName);
                    CoreLib.WriteSRCDWord("ScreenMSAAQuality", 0, SAppName);
                    break;
                case 6:
                    // 16xQ CSAA
                    CoreLib.WriteSRCDWord("mat_antialias", 8, SAppName);
                    CoreLib.WriteSRCDWord("mat_aaquality", 2, SAppName);
                    CoreLib.WriteSRCDWord("ScreenMSAA", 8, SAppName);
                    CoreLib.WriteSRCDWord("ScreenMSAAQuality", 2, SAppName);
                    break;
            }

            // Запишем в реестр настройки фильтрации (mat_forceaniso):
            switch (GT_Filtering.SelectedIndex)
            {
                case 0:
                    // Билинейная
                    CoreLib.WriteSRCDWord("mat_forceaniso", 1, SAppName);
                    CoreLib.WriteSRCDWord("mat_trilinear", 0, SAppName);
                    break;
                case 1:
                    // Трилинейная
                    CoreLib.WriteSRCDWord("mat_forceaniso", 1, SAppName);
                    CoreLib.WriteSRCDWord("mat_trilinear", 1, SAppName);
                    break;
                case 2:
                    // Анизотропная 2x
                    CoreLib.WriteSRCDWord("mat_forceaniso", 2, SAppName);
                    CoreLib.WriteSRCDWord("mat_trilinear", 0, SAppName);
                    break;
                case 3:
                    // Анизотропная 4x
                    CoreLib.WriteSRCDWord("mat_forceaniso", 4, SAppName);
                    CoreLib.WriteSRCDWord("mat_trilinear", 0, SAppName);
                    break;
                case 4:
                    // Анизотропная 8x
                    CoreLib.WriteSRCDWord("mat_forceaniso", 8, SAppName);
                    CoreLib.WriteSRCDWord("mat_trilinear", 0, SAppName);
                    break;
                case 5:
                    // Анизотропная 16x
                    CoreLib.WriteSRCDWord("mat_forceaniso", 16, SAppName);
                    CoreLib.WriteSRCDWord("mat_trilinear", 0, SAppName);
                    break;
            }

            // Запишем в реестр настройки вертикальной синхронизации (mat_vsync):
            switch (GT_VSync.SelectedIndex)
            {
                case 0: CoreLib.WriteSRCDWord("mat_vsync", 0, SAppName);
                    break;
                case 1: CoreLib.WriteSRCDWord("mat_vsync", 1, SAppName);
                    break;
            }

            // Запишем в реестр настройки размытия движения (MotionBlur):
            switch (GT_MotionBlur.SelectedIndex)
            {
                case 0: CoreLib.WriteSRCDWord("MotionBlur", 0, SAppName);
                    break;
                case 1: CoreLib.WriteSRCDWord("MotionBlur", 1, SAppName);
                    break;
            }

            // Запишем в реестр настройки режима DirectX (DXLevel_V1):
            switch (GT_DxMode.SelectedIndex)
            {
                case 0: CoreLib.WriteSRCDWord("DXLevel_V1", 80, SAppName); // DirectX 8.0
                    break;
                case 1: CoreLib.WriteSRCDWord("DXLevel_V1", 81, SAppName); // DirectX 8.1
                    break;
                case 2: CoreLib.WriteSRCDWord("DXLevel_V1", 90, SAppName); // DirectX 9.0
                    break;
                case 3: CoreLib.WriteSRCDWord("DXLevel_V1", 95, SAppName); // DirectX 9.0c
                    break;
            }

            // Запишем в реестр настройки HDR (mat_hdr_level):
            switch (GT_HDR.SelectedIndex)
            {
                case 0: CoreLib.WriteSRCDWord("mat_hdr_level", 0, SAppName);
                    break;
                case 1: CoreLib.WriteSRCDWord("mat_hdr_level", 1, SAppName);
                    break;
                case 2: CoreLib.WriteSRCDWord("mat_hdr_level", 2, SAppName);
                    break;
            }
        }

        /// <summary>
        /// Получает настройки GCF-игры из реестра и заполняет полученными
        /// данными страницу графического твикера.
        /// </summary>
        /// <param name="SAppName">Краткое имя игры</param>
        private void ReadGCFGameSettings(string SAppName)
        {
            // Получаем значение разрешения по горизонтали
            try
            {
                GT_ResHor.Value = CoreLib.GetSRCDWord("ScreenWidth", SAppName);
            }
            catch
            {
                GT_ResHor.Value = 800;
            }

            // Получаем значение разрешения по вертикали
            try
            {
                GT_ResVert.Value = CoreLib.GetSRCDWord("ScreenHeight", SAppName);
            }
            catch
            {
                GT_ResVert.Value = 600;
            }

            // Получаем режим окна (ScreenWindowed): 1-window, 0-fullscreen
            try
            {
                GT_ScreenType.SelectedIndex = CoreLib.GetSRCDWord("ScreenWindowed", SAppName);
            }
            catch
            {
                GT_ScreenType.SelectedIndex = -1;
            }

            // Получаем детализацию моделей (r_rootlod): 0-high, 1-med, 2-low
            try
            {
                switch (CoreLib.GetSRCDWord("r_rootlod", SAppName))
                {
                    case 0: GT_ModelQuality.SelectedIndex = 2;
                        break;
                    case 1: GT_ModelQuality.SelectedIndex = 1;
                        break;
                    case 2: GT_ModelQuality.SelectedIndex = 0;
                        break;
                }
            }
            catch
            {
                GT_ModelQuality.SelectedIndex = -1;
            }

            // Получаем детализацию текстур (mat_picmip): 0-high, 1-med, 2-low
            try
            {
                switch (CoreLib.GetSRCDWord("mat_picmip", SAppName))
                {
                    case 0: GT_TextureQuality.SelectedIndex = 2;
                        break;
                    case 1: GT_TextureQuality.SelectedIndex = 1;
                        break;
                    case 2: GT_TextureQuality.SelectedIndex = 0;
                        break;
                }
            }
            catch
            {
                GT_TextureQuality.SelectedIndex = -1;
            }

            // Получаем настройки шейдеров (mat_reducefillrate): 0-high, 1-low
            try
            {
                switch (CoreLib.GetSRCDWord("mat_reducefillrate", SAppName))
                {
                    case 0: GT_ShaderQuality.SelectedIndex = 1;
                        break;
                    case 1: GT_ShaderQuality.SelectedIndex = 0;
                        break;
                }
            }
            catch
            {
                GT_ShaderQuality.SelectedIndex = -1;
            }

            // Начинаем работать над отражениями (здесь сложнее)
            try
            {
                switch (CoreLib.GetSRCDWord("r_waterforceexpensive", SAppName))
                {
                    case 0: GT_WaterQuality.SelectedIndex = 0;
                        break;
                    case 1:
                        switch (CoreLib.GetSRCDWord("r_waterforcereflectentities", SAppName))
                        {
                            case 0: GT_WaterQuality.SelectedIndex = 1;
                                break;
                            case 1: GT_WaterQuality.SelectedIndex = 2;
                                break;
                        }
                        break;
                }
            }
            catch
            {
                GT_WaterQuality.SelectedIndex = -1;
            }

            // Получаем настройки теней (r_shadowrendertotexture): 0-low, 1-high
            try
            {
                switch (CoreLib.GetSRCDWord("r_shadowrendertotexture", SAppName))
                {
                    case 0: GT_ShadowQuality.SelectedIndex = 0;
                        break;
                    case 1: GT_ShadowQuality.SelectedIndex = 1;
                        break;
                }
            }
            catch
            {
                GT_ShadowQuality.SelectedIndex = -1;
            }

            // Получаем настройки коррекции цвета (mat_colorcorrection): 0-off, 1-on
            try
            {
                switch (CoreLib.GetSRCDWord("mat_colorcorrection", SAppName))
                {
                    case 0: GT_ColorCorrectionT.SelectedIndex = 0;
                        break;
                    case 1: GT_ColorCorrectionT.SelectedIndex = 1;
                        break;
                }
            }
            catch
            {
                GT_ColorCorrectionT.SelectedIndex = -1;
            }

            // Получаем настройки сглаживания (mat_antialias): 1-off, 2-2x, 4-4x, etc
            // 2x MSAA - 2:0; 4xMSAA - 4:0; 8xCSAA - 4:2; 16xCSAA - 4:4; 8xMSAA - 8:0; 16xQ CSAA - 8:2;
            try
            {
                switch (CoreLib.GetSRCDWord("mat_antialias", SAppName))
                {
                    case 0: GT_AntiAliasing.SelectedIndex = 0;
                        break;
                    case 1: GT_AntiAliasing.SelectedIndex = 0;
                        break;
                    case 2: GT_AntiAliasing.SelectedIndex = 1;
                        break;
                    case 4:
                        switch (CoreLib.GetSRCDWord("mat_aaquality", SAppName))
                        {
                            case 0: GT_AntiAliasing.SelectedIndex = 2;
                                break;
                            case 2: GT_AntiAliasing.SelectedIndex = 3;
                                break;
                            case 4: GT_AntiAliasing.SelectedIndex = 4;
                                break;
                        }
                        break;
                    case 8:
                        switch (CoreLib.GetSRCDWord("mat_aaquality", SAppName))
                        {
                            case 0: GT_AntiAliasing.SelectedIndex = 5;
                                break;
                            case 2: GT_AntiAliasing.SelectedIndex = 6;
                                break;
                        }
                        break;
                }
            }
            catch
            {
                GT_AntiAliasing.SelectedIndex = -1;
            }

            // Получаем настройки анизотропии (mat_forceaniso): 1-off, etc
            try
            {
                switch (CoreLib.GetSRCDWord("mat_forceaniso", SAppName))
                {
                    case 1:
                        switch (CoreLib.GetSRCDWord("mat_trilinear", SAppName))
                        {
                            case 0: GT_Filtering.SelectedIndex = 0;
                                break;
                            case 1: GT_Filtering.SelectedIndex = 1;
                                break;
                        }
                        break;
                    case 2: GT_Filtering.SelectedIndex = 2;
                        break;
                    case 4: GT_Filtering.SelectedIndex = 3;
                        break;
                    case 8: GT_Filtering.SelectedIndex = 4;
                        break;
                    case 16: GT_Filtering.SelectedIndex = 5;
                        break;
                }
            }
            catch
            {
                GT_Filtering.SelectedIndex = -1;
            }

            // Получаем настройки вертикальной синхронизации (mat_vsync): 0-off, 1-on
            try
            {
                switch (CoreLib.GetSRCDWord("mat_vsync", SAppName))
                {
                    case 0: GT_VSync.SelectedIndex = 0;
                        break;
                    case 1: GT_VSync.SelectedIndex = 1;
                        break;
                }
            }
            catch
            {
                GT_VSync.SelectedIndex = -1;
            }

            // Получаем настройки размытия движения (MotionBlur): 0-off, 1-on
            try
            {
                switch (CoreLib.GetSRCDWord("MotionBlur", SAppName))
                {
                    case 0: GT_MotionBlur.SelectedIndex = 0;
                        break;
                    case 1: GT_MotionBlur.SelectedIndex = 1;
                        break;
                }
            }
            catch
            {
                GT_MotionBlur.SelectedIndex = -1;
            }

            // Получаем настройки режима рендера (DXLevel_V1):
            // 80-DirectX 8.0; 81-DirectX 8.1; 90-DirectX 9.0; 95-DirectX 9.0c
            try
            {
                switch (CoreLib.GetSRCDWord("DXLevel_V1", SAppName))
                {
                    case 80: GT_DxMode.SelectedIndex = 0;
                        break;
                    case 81: GT_DxMode.SelectedIndex = 1;
                        break;
                    case 90: GT_DxMode.SelectedIndex = 2;
                        break;
                    case 95: GT_DxMode.SelectedIndex = 3;
                        break;
                }
            }
            catch
            {
                GT_DxMode.SelectedIndex = -1;
            }

            // Получаем настройки HDR (mat_hdr_level): 0-off,1-bloom,2-Full
            try
            {
                switch (CoreLib.GetSRCDWord("mat_hdr_level", SAppName))
                {
                    case 0: GT_HDR.SelectedIndex = 0;
                        break;
                    case 1: GT_HDR.SelectedIndex = 1;
                        break;
                    case 2: GT_HDR.SelectedIndex = 2;
                        break;
                }
            }
            catch
            {
                GT_HDR.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Открывает конфиг, имя которого передано в качестве параметра
        /// и заполняет им Редактор конфигов с одноимённой страницы.
        /// </summary>
        /// <param name="ConfFileName">Полный путь к файлу конфига</param>
        private void ReadConfigFromFile(string ConfFileName)
        {
            string Buf = ConfFileName; // Получаем имя файла с полным путём...
            string ImpStr; // Строка для парсинга...
            string CVarName, CVarContent;
            if (File.Exists(Buf)) // Проверяем, существует ли файл...
            {
                // Файл существует. Продолжаем...
                CFGFileName = Path.GetFileName(Buf); // Получаем имя открытого в Редакторе файла без пути...
                if (CFGFileName == "config.cfg") // Проверяем, не открыл ли пользователь файл config.cfg и, если да, то сообщаем об этом...
                {
                    MessageBox.Show(RM.GetString("CE_RestConfigOpenWarn"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                CE_Editor.Rows.Clear(); // Очищаем область редактирования...
                try
                {
                    using (StreamReader OpenedConfig = new StreamReader(@Buf, Encoding.Default)) // Открываем файл таким способом...
                    {
                        // Будем читать поток построчно...
                        while (OpenedConfig.Peek() >= 0)
                        {
                            // Начинаем работу...
                            ImpStr = OpenedConfig.ReadLine(); // считали строку...
                            ImpStr = ImpStr.Trim(); // почистим строку от лишних пробелов...
                            // Начинаем парсить считанную строку...
                            if (!(String.IsNullOrEmpty(ImpStr))) // проверяем, не пустая ли строка...
                            {
                                if ((ImpStr[1] != '/') && (ImpStr.Length >= 4) && (ImpStr.Substring(0, 4) != "echo")) // проверяем, не комментарий ли или сообщение...
                                {
                                    // Удалим все лишние пробелы...
                                    while (ImpStr.IndexOf("  ") != -1) // пока остались двойные пробелы, продолжаем...
                                    {
                                        ImpStr = ImpStr.Replace("  ", " "); // удаляем найденный лишний пробел...
                                    }

                                    // Ищем и удаляем символ табуляции из строки...
                                    while (ImpStr.IndexOf("\t") != -1)
                                    {
                                        ImpStr = ImpStr.Replace("\t", "");
                                    }

                                    // Строка почищена, продолжаем...
                                    if (ImpStr.IndexOf(" ") != -1)
                                    {
                                        Buf = ImpStr.Substring(0, ImpStr.IndexOf(" ")); // мы получили переменную...
                                        ImpStr = ImpStr.Remove(0, ImpStr.IndexOf(" ") + 1); // удаляем полученное...
                                        //ImpStr = ImpStr.Replace(Buf, "");
                                        // Buf теперь содержит всё до пробела. Нужно чистить...
                                        if ((Buf.IndexOf("/") == -1) && (Buf.IndexOf(" ") == -1) && (Buf != ""))
                                        {
                                            CVarName = Buf; // заполняем имя переменной...
                                            // Отлично, имя переменной мы получили и храним в CVarName. Осталось получить значение...
                                            if (ImpStr.IndexOf("/") != -1) // ищем в строке комментарии...
                                            {
                                                Buf = ImpStr.Substring(0, ImpStr.IndexOf("/") - 1); // копируем всё до комментария...
                                                CVarContent = Buf; // возвращаем значение...
                                            }
                                            else
                                            {
                                                CVarContent = ImpStr; // комментариев нет, сразу возвращаем значение...
                                            }
                                            // Пишем в нашу таблицу...
                                            CE_Editor.Rows.Add(CVarName, CVarContent);
                                        }
                                    }
                                    else
                                    {
                                        CE_Editor.Rows.Add(ImpStr, "");
                                    }
                                }
                            }
                        }
                    }
                    SB_Status.Text = RM.GetString("StatusOpenedFile") + " " + CFGFileName;
                }
                catch
                {
                    // Произошло исключение...
                    // Подавляем сообщение об этом. Юзеру не обязательно знать...
                    MessageBox.Show(RM.GetString("CE_ExceptionDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(RM.GetString("CE_OpenFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Считывает файлы резервных копий из указанного каталога и помещает в таблицу.
        /// </summary>
        /// <param name="BUpDir">Путь к каталогу с резервными копиями</param>
        private void ReadBackUpList2Table(string BUpDir)
        {
            // Считаем список резервных копий и заполним таблицу...
            // Очистим таблицу...
            BU_LVTable.Items.Clear();
            // Открываем каталог...
            DirectoryInfo DInfo = new DirectoryInfo(BUpDir);
            // Считываем список файлов по заданной маске...
            FileInfo[] DirList = DInfo.GetFiles("*.*");
            // Инициализируем буферные переменные...
            string Buf, BufName;
            // Начинаем обход массива...
            foreach (FileInfo DItem in DirList)
            {
                // Обрабатываем найденное...
                BufName = Path.GetFileNameWithoutExtension(DItem.Name);
                if (DItem.Extension == ".reg")
                {
                    // Бэкап реестра...
                    Buf = RM.GetString("BU_BType_Reg");
                    // Заполним человеческое описание...
                    if (BufName.IndexOf("Game_Options") != -1)
                    {
                        BufName = RM.GetString("BU_BName_GRGame") + " " + DItem.CreationTime;
                    }
                    if (BufName.IndexOf("Source_Options") != -1)
                    {
                        BufName = RM.GetString("BU_BName_SRCAll") + " " + DItem.CreationTime;
                    }
                    if (BufName.IndexOf("Steam_BackUp") != -1)
                    {
                        BufName = RM.GetString("BU_BName_SteamAll") + " " + DItem.CreationTime;
                    }
                    if (BufName.IndexOf("Game_AutoBackUp") != -1)
                    {
                        BufName = RM.GetString("BU_BName_GameAuto") + " " + DItem.CreationTime;
                    }

                }
                else
                {
                    // Бэкап конфига...
                    Buf = RM.GetString("BU_BType_Cfg");
                }
                // Добавляем в таблицу...
                ListViewItem LvItem = new ListViewItem(BufName);
                LvItem.SubItems.Add(Buf);
                LvItem.SubItems.Add(CoreLib.SclBytes(DItem.Length));
                LvItem.SubItems.Add(CoreLib.WriteDateToString(DItem.CreationTime, false));
                LvItem.SubItems.Add(DItem.Name);
                BU_LVTable.Items.Add(LvItem);
            }
        }

        /// <summary>
        /// Обнуляет контролы на странице графических настроек. Функция-заглушка.
        /// </summary>
        private void NullGraphOptions()
        {
            GT_ResHor.Value = 640;
            GT_ResVert.Value = 480;
            GT_ScreenType.SelectedIndex = -1;
            GT_ModelQuality.SelectedIndex = -1;
            GT_TextureQuality.SelectedIndex = -1;
            GT_ShaderQuality.SelectedIndex = -1;
            GT_WaterQuality.SelectedIndex = -1;
            GT_ShadowQuality.SelectedIndex = -1;
            GT_ColorCorrectionT.SelectedIndex = -1;
            GT_AntiAliasing.SelectedIndex = -1;
            GT_Filtering.SelectedIndex = -1;
            GT_VSync.SelectedIndex = -1;
            GT_MotionBlur.SelectedIndex = -1;
            GT_DxMode.SelectedIndex = -1;
            GT_HDR.SelectedIndex = -1;
        }

        #endregion

        private void frmMainW_Load(object sender, EventArgs e)
        {
            // Событие инициализации формы...
            
            // Получаем путь к каталогу приложения...
            Assembly Assmbl = Assembly.GetEntryAssembly();
            GV.FullAppPath = CoreLib.IncludeTrDelim(Path.GetDirectoryName(Assmbl.Location));

            // Проверяем, запущена ли программа с правами администратора...
            if (!(CoreLib.IsCurrentUserAdmin()))
            {
                // Программа запущена с правами пользователя, поэтому принимаем меры...
                // Выводим сообщение об этом...
                MessageBox.Show(RM.GetString("AppLaunchedNotAdmin"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Блокируем контролы, требующие для своей работы прав админа...
                PS_CleanRegistry.Enabled = false;
                PS_SteamLang.Enabled = false;
                MNUHEd.Enabled = false;
                MNUUpdateCheck.Enabled = false;
                MNUWinMnuDisabler.Enabled = false;
            }
            
            // Получаем информацию о версии нашего приложения...
            GV.AppVersionInfo = Assmbl.GetName().Version.ToString();

            // Вставляем информацию о версии в заголовок формы...
            //this.Text += " (version " + GV.AppVersionInfo + ")";
            this.Text = String.Format(this.Text, GV.AppVersionInfo);

            // Найдём и завершим в памяти процесс Steam...
            CoreLib.ProcessTerminate("Steam", RM.GetString("ST_KillMessage"));

            // Узнаем путь к установленному клиенту Steam...
            try
            {
                // Получаем из реестра...
                GV.FullSteamPath = CoreLib.IncludeTrDelim(CoreLib.GetSteamPath());
            }
            catch
            {
                // Найдём путь...
                if (!(String.IsNullOrWhiteSpace(Properties.Settings.Default.LastSteamPath)) && File.Exists(Path.Combine(Properties.Settings.Default.LastSteamPath, "Steam.exe")))
                {
                    GV.FullSteamPath = Properties.Settings.Default.LastSteamPath;
                }
                else
                {
                    // Произошло исключение, пользователю придётся ввести путь самостоятельно!
                    MessageBox.Show(RM.GetString("SteamPathNotDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    string Buf = "";
                    FldrBrwse.Description = RM.GetString("SteamPathEnterText"); // Указываем текст в диалоге поиска каталога...
                    if (FldrBrwse.ShowDialog() == DialogResult.OK) // Отображаем стандартный диалог поиска каталога...
                    {
                        Buf = CoreLib.IncludeTrDelim(FldrBrwse.SelectedPath);
                        if (!(File.Exists(Path.Combine(Buf, "Steam.exe"))))
                        {
                            MessageBox.Show(RM.GetString("SteamPathEnterErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(7);
                        }
                        else
                        {
                            GV.FullSteamPath = Buf;
                            Properties.Settings.Default.LastSteamPath = GV.FullSteamPath;
                        }
                    }
                    else
                    {
                        MessageBox.Show(RM.GetString("SteamPathCancel"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(7);
                    }
                }
            }

            // Применим некоторые настройки...
            AppSelector.Sorted = Properties.Settings.Default.SortGamesList;
            FP_CreateBackUp.Checked = Properties.Settings.Default.FPConfCreateBackUps;

            // Определим логины пользователей Steam на данном ПК...
            try
            {
                // Создаём объект DirInfo...
                DirectoryInfo DInfo = new DirectoryInfo(CoreLib.IncludeTrDelim(Path.Combine(GV.FullSteamPath, "steamapps")));
                // Получаем список директорий из текущего...
                DirectoryInfo[] DirList = DInfo.GetDirectories();
                // Обходим созданный массив в поиске нужных нам логинов...
                foreach (DirectoryInfo DItem in DirList)
                {
                    // Фильтруем известные каталоги...
                    if ((DItem.Name != "common") && (DItem.Name != "sourcemods") && (DItem.Name != "media"))
                    {
                        // Добавляем найденный логин в список ComboBox...
                        LoginSel.Items.Add((string)DItem.Name);
                    }
                }
            }
            catch
            {
                MessageBox.Show(RM.GetString("AppNoSTADetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(6);
            }

            // Проверим, были ли логины добавлены в список...
            if (LoginSel.Items.Count == 0)
            {
                // Логинов не было найдено, поэтому запросим у пользователя...
                // Описываем буферную переменную...
                string SBuf = "";
                
                // Запускаем цикл с постусловием...
                do
                {
                    // Отображаем InputBox, а также анализируем ввод пользователя...
                    if ((InputBox(RM.GetString("SteamLoginEnterTitle"), RM.GetString("SteamLoginEnterText"), ref SBuf) == DialogResult.Cancel) || (SBuf == ""))
                    {
                        // Пользователь нажал Cancel, либо ввёл пустую строку, поэтому
                        // выводим сообщение и завершаем работу приложения...
                        MessageBox.Show(RM.GetString("SteamLoginCancel"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Environment.Exit(7);
                    };
                } while (!(Directory.Exists(CoreLib.IncludeTrDelim(Path.Combine(GV.FullSteamPath, "steamapps", SBuf)))));
                
                // Добавляем полученный логин в список...
                LoginSel.Items.Add((string)SBuf);
            }

            // Укажем путь к пользовательским данным и создадим если не существует...
            GV.AppUserDir = CoreLib.IncludeTrDelim(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GV.AppName));
            if (!(Directory.Exists(GV.AppUserDir)))
            {
                Directory.CreateDirectory(GV.AppUserDir);
            }

            // Выводим сообщение в строку статуса...
            SB_Status.Text = RM.GetString("StatusSLogin");

            // Проверяем единственный ли логин Steam на этом ПК...
            if (LoginSel.Items.Count == 1)
            {
                // Да, единственный. Выберем его...
                LoginSel.SelectedIndex = 0;
                if (AppSelector.SelectedIndex == -1)
                {
                    // Заменим содержимое строки состояния на требование выбора игры...
                    SB_Status.Text = RM.GetString("StatusSApp");
                }
            }

            // Укажем путь к Steam на странице "Устранение проблем"...
            PS_RSteamPath.Text = GV.FullSteamPath;
            
            // Проверим на наличие запрещённых символов в пути к установленному клиенту Steam...
            if (!(CoreLib.CheckNonASCII(GV.FullSteamPath)))
            {
                // Запрещённые символы найдены!
                PS_PathDetector.Text = RM.GetString("SteamNonASCIITitle");
                PS_PathDetector.ForeColor = Color.Red;
                PS_WarningMsg.Text = RM.GetString("SteamNonASCIISmall");
                PS_WarningMsg.ForeColor = Color.Red;
                MessageBox.Show(RM.GetString("SteamNonASCIIDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            try
            {
                // Проверим наличие обновлений программы (если разрешено в настройках)...
                if (Properties.Settings.Default.EnableAutoUpdate && (Properties.Settings.Default.LastUpdateTime != null))
                {
                    // Вычисляем разницу между текущей датой и датой последнего обновления...
                    TimeSpan TS = DateTime.Now - Properties.Settings.Default.LastUpdateTime;
                    if (TS.Days >= 7) // Проверяем не прошла ли неделя с момента последней прверки...
                    {
                        // Требуется проверка обновлений...
                        if (CoreLib.AutoUpdateCheck(GV.AppVersionInfo, Properties.Settings.Default.UpdateChURI))
                        {
                            // Доступны обновления...
                            MessageBox.Show(String.Format(RM.GetString("AppUpdateAvailable"), GV.AppName), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        // Установим время последней проверки обновлений...
                        Properties.Settings.Default.LastUpdateTime = DateTime.Now;
                    }
                }
            }
            catch { }
        }

        private void PS_CleanBlobs_CheckedChanged(object sender, EventArgs e)
        {
            if (PS_CleanBlobs.Checked || PS_CleanRegistry.Checked)
            {
                PS_ExecuteNow.Enabled = true;
            }
            else
            {
                PS_ExecuteNow.Enabled = false;
            }
        }

        private void PS_CleanRegistry_CheckedChanged(object sender, EventArgs e)
        {
            // Включаем список с доступными языками клиента Steam...
            PS_SteamLang.Enabled = PS_CleanRegistry.Checked;

            // Выбираем язык по умолчанию согласно языку приложения...
            PS_SteamLang.SelectedIndex = Convert.ToInt32(RM.GetString("AppDefaultSteamLangID"));

            if (PS_CleanRegistry.Checked || PS_CleanBlobs.Checked)
            {
                PS_ExecuteNow.Enabled = true;
            }
            else
            {
                PS_ExecuteNow.Enabled = false;
            }
        }

        private void PS_ExecuteNow_Click(object sender, EventArgs e)
        {
            // Начинаем очистку...

            // Запрашиваем подтверждение у пользователя...
            if (MessageBox.Show(RM.GetString("PS_ExecuteMSG"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Подтверждение получено...
                if ((PS_CleanBlobs.Checked) || (PS_CleanRegistry.Checked))
                {
                    // Найдём и завершим работу клиента Steam...
                    if (CoreLib.ProcessTerminate("Steam") != 0)
                    {
                        MessageBox.Show(RM.GetString("PS_ProcessDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Проверяем нужно ли чистить блобы...
                    if (PS_CleanBlobs.Checked)
                    {
                        // Чистим блобы...
                        CoreLib.CleanBlobsNow(GV.FullSteamPath);
                    }

                    // Проверяем нужно ли чистить реестр...
                    if (PS_CleanRegistry.Checked)
                    {
                        // Проверяем выбрал ли пользователь язык из выпадающего списка...
                        if (PS_SteamLang.SelectedIndex != -1)
                        {
                            // Всё в порядке, чистим реестр...
                            CoreLib.CleanRegistryNow(PS_SteamLang.SelectedIndex);
                        }
                        else
                        {
                            // Пользователь не выбрал язык, поэтому будем использовать английский...
                            MessageBox.Show(RM.GetString("PS_NoLangSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CoreLib.CleanRegistryNow(0);
                        }
                    }

                    // Выполнение закончено, выведем сообщение о завершении...
                    MessageBox.Show(RM.GetString("PS_SeqCompleted"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Запустим Steam...
                    if (File.Exists(Path.Combine(GV.FullSteamPath, "Steam.exe"))) { Process.Start(Path.Combine(GV.FullSteamPath, "Steam.exe")); }
                }
            }
        }

        private void frmMainW_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Properties.Settings.Default.ConfirmExit)
            {
                // Проверим, делал ли что-то пользователь с формой. Если не делал - не будем
                // спрашивать и завершим форму автоматически...
                if ((AppSelector.Enabled) && (AppSelector.SelectedIndex != -1))
                {
                    // Запрашиваем подтверждение у пользователя на закрытие формы...
                    if (MessageBox.Show(String.Format(RM.GetString("FrmCloseQuery"), GV.AppName), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Подтверждение получено, закрываем форму...
                        e.Cancel = false;
                    }
                    else
                    {
                        // Пользователь передумал, отменяем закрытие формы...
                        e.Cancel = true;
                    }
                }
            }
        }

        private void AppSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Создаём буферную переменную для дальнейшего использования...
            string ptha = "";

            /* Значение локальной переменной ptha:
             * LoginSel.Text - для GCF-приложний (логин Steam);
             * common - для NCF-приложений.
             */
            
            // Начинаем определять нужные нам значения переменных...
            try
            {
                XmlDocument XMLD = new XmlDocument();
                FileStream XMLFS = new FileStream(Properties.Settings.Default.GameListFile, FileMode.Open, FileAccess.Read);
                XMLD.Load(XMLFS);
                XmlNodeList XMLNList = XMLD.GetElementsByTagName("Game");
                for (int i = 0; i < XMLNList.Count; i++)
                {
                    XmlElement GameID = (XmlElement)XMLD.GetElementsByTagName("Game")[i];
                    if (GameID.GetAttribute("Name") == AppSelector.Text)
                    {
                        GV.FullAppName = XMLD.GetElementsByTagName("DirName")[i].InnerText;
                        GV.SmallAppName = XMLD.GetElementsByTagName("SmallName")[i].InnerText;
                        if (XMLD.GetElementsByTagName("IsGCF")[i].InnerText == "1")
                        {
                            GV.IsGCFApp = true;
                            ptha = LoginSel.Text;
                        }
                        else
                        {
                            GV.IsGCFApp = false;
                            ptha = "common";
                        }
                        break;
                    }
                }
                XMLFS.Close();
            }
            catch
            {
                MessageBox.Show(RM.GetString("AppXMLParseError"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Если выбор верный, включаем контрол выбора логина Steam...
            if (AppSelector.SelectedIndex != -1)
            {
                LoginSel.Enabled = true;
            }
            else
            {
                LoginSel.Enabled = false;
            }

            // Генерируем полный путь до каталога управляемого приложения...
            GV.GamePath = CoreLib.IncludeTrDelim(Path.Combine(GV.FullSteamPath, "steamapps", ptha, GV.FullAppName));
            GV.FullGamePath = CoreLib.IncludeTrDelim(Path.Combine(GV.GamePath, GV.SmallAppName));

            // Заполняем другие служебные переменные...
            GV.FullCfgPath = CoreLib.IncludeTrDelim(Path.Combine(GV.FullGamePath, "cfg"));
            GV.FullBackUpDirPath = CoreLib.IncludeTrDelim(Path.Combine(GV.AppUserDir, "backups", GV.SmallAppName));
            
            // Включаем основные элементы управления (контролы)...
            MainTabControl.Enabled = true;

            if (GV.IsGCFApp)
            {
                // Включим модули очистки...
                PS_ResetSettings.Enabled = true;
                EnableCleanButtons(true);
                
                // Включаем контролы на странице графических настроек...
                GT_SaveApply.Enabled = true;
                GT_Maximum_Graphics.Enabled = true;
                GT_Maximum_Performance.Enabled = true;

                // Начинаем заполнять таблицу...
                ReadGCFGameSettings(GV.SmallAppName);
            }
            else
            {
                // Отключим модули очистки...
                PS_ResetSettings.Enabled = false;
                if (!(Properties.Settings.Default.AllowNCFUnsafeOps)) { EnableCleanButtons(false); }
                
                // Отключаем контролы на странице графических настроек и зануляем их...
                GT_SaveApply.Enabled = false;
                GT_Maximum_Graphics.Enabled = false;
                GT_Maximum_Performance.Enabled = false;
                NullGraphOptions();
                
                // Приложение NCF, поэтому настройки хранятся не в реестре, а в
                // файле video.txt. Будем парсить...
                
                // TODO: реализовать парсинг файла video.txt и заполнить таблицу из него...
            }

            // Проверим, установлен ли FPS-конфиг...
            if (File.Exists(Path.Combine(GV.FullCfgPath, "autoexec.cfg")))
            {
                GT_Warning.Visible = true;
            }
            else
            {
                GT_Warning.Visible = false;
            }
            
            // Очистим список FPS-конфигов...
            FP_ConfigSel.Items.Clear();

            // Отключим кнопку редактирования FPS-конфигов...
            FP_OpenNotepad.Enabled = false;

            // Считаем имеющиеся FPS-конфиги...
            try
            {
                // Открываем каталог...
                DirectoryInfo DInfo = new DirectoryInfo(CoreLib.IncludeTrDelim(Path.Combine(GV.FullAppPath, "cfgs", GV.SmallAppName)));
                // Считываем список файлов по заданной маске...
                FileInfo[] DirList = DInfo.GetFiles("*.cfg");
                // Начинаем обход массива...
                foreach (FileInfo DItem in DirList)
                {
                    // Обрабатываем найденное...
                    if (DItem.Name != "config_default.cfg")
                    {
                        FP_ConfigSel.Items.Add((string)DItem.Name);
                    }
                }
                // Проверяем, нашлись ли конфиги...
                if (FP_ConfigSel.Items.Count >= 1)
                {
                    FP_Description.Text = RM.GetString("FP_SelectFromList");
                    FP_Description.ForeColor = Color.Black;
                    FP_Install.Enabled = true;
                    FP_ConfigSel.Enabled = true;
                }
            }
            catch
            {
                // FPS-конфигов для выбранного приложения не найдено.
                // Выводим текст об этом...
                FP_Description.Text = RM.GetString("FP_NoCfgGame");
                FP_Description.ForeColor = Color.Red;
                // ...и блокируем контролы, отвечающие за установку...
                FP_Install.Enabled = false;
                FP_ConfigSel.Enabled = false;
                FP_OpenNotepad.Enabled = false;
            }

            // Включаем заблокированные ранее контролы...
            MNUFPSWizard.Enabled = true;
            MNUInstaller.Enabled = true;
            
            // Выводим сообщение о завершении считывания в статус-бар...
            SB_Status.Text = RM.GetString("StatusNormal");
            SB_App.Text = AppSelector.Text;

            // Считаем список бэкапов...
            try { ReadBackUpList2Table(GV.FullBackUpDirPath); } catch { Directory.CreateDirectory(GV.FullBackUpDirPath); }
        }

        private void LoginSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Логин выбран, включаем контролы...
            AppSelector.Enabled = true;
            if (AppSelector.SelectedIndex == -1)
            {
                SB_Status.Text = RM.GetString("StatusSApp");
            }
            else
            {
                SB_Status.Text = RM.GetString("StatusLoginChanged");
            }

            // Выводим логин на страницу "Устранение проблем"...
            PS_RSteamLogin.Text = LoginSel.Text;

            // Начинаем определять установленные игры...
            try
            {
                DetectInstalledGames(GV.FullSteamPath, LoginSel.Text);
            }
            catch
            {
                MessageBox.Show(RM.GetString("AppXMLParseError"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(16);
            }

            // Проверим нашлись ли игры...
            if (AppSelector.Items.Count == 0)
            {
                // Нет, не нашлись, выведем сообщение...
                MessageBox.Show(RM.GetString("AppNoGamesDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // При наличии единственной игры в списке, выберем её автоматически...
            if (AppSelector.Items.Count == 1)
            {
                AppSelector.SelectedIndex = 0;
                SB_Status.Text = RM.GetString("StatusNormal");
            }
        }

        private void GT_Maximum_Graphics_Click(object sender, EventArgs e)
        {
            // Нажатие этой кнопки устанавливает графические настройки на рекомендуемый максимум...
            // Зададим вопрос, а нужно ли это юзеру?
            if (MessageBox.Show(RM.GetString("GT_MaxPerfMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Пользователь согласился, продолжаем...
                GT_ScreenType.SelectedIndex = 0; // полноэкранный режим
                GT_ModelQuality.SelectedIndex = 2; // высокая детализация моделей
                GT_TextureQuality.SelectedIndex = 2; // высокая детализация текстур
                GT_ShaderQuality.SelectedIndex = 1; // высокое качество шейдерных эффектов
                GT_WaterQuality.SelectedIndex = 1; // отражать мир в воде
                GT_ShadowQuality.SelectedIndex = 1; // высокое качество теней
                GT_ColorCorrectionT.SelectedIndex = 1; // корренкция цвета включена
                GT_AntiAliasing.SelectedIndex = 0; // сглаживание выключено
                GT_Filtering.SelectedIndex = 3; // анизотропная фильтрация 4x
                GT_VSync.SelectedIndex = 0; // вертикальная синхронизация выключена
                GT_MotionBlur.SelectedIndex = 0; // размытие движения выключено
                GT_DxMode.SelectedIndex = 3; // режим DirecX 9.0c
                GT_HDR.SelectedIndex = 2; // HDR полные
                MessageBox.Show(RM.GetString("GT_PerfSet"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GT_Maximum_Performance_Click(object sender, EventArgs e)
        {
            // Нажатие этой кнопки устанавливает графические настройки на рекомендуемый минимум...
            // Спросим пользователя.
            if (MessageBox.Show(RM.GetString("GT_MinPerfMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Пользователь согласился, продолжаем...
                GT_ScreenType.SelectedIndex = 0; // полноэкранный режим
                GT_ModelQuality.SelectedIndex = 0; // низкая детализация моделей
                GT_TextureQuality.SelectedIndex = 0; // низкая детализация текстур
                GT_ShaderQuality.SelectedIndex = 0; // низкое качество шейдерных эффектов
                GT_WaterQuality.SelectedIndex = 0; // простые отражения в воде
                GT_ShadowQuality.SelectedIndex = 0; // низкое качество теней
                GT_ColorCorrectionT.SelectedIndex = 0; // корренкция цвета выключена
                GT_AntiAliasing.SelectedIndex = 0; // сглаживание выключено
                GT_Filtering.SelectedIndex = 1; // трилинейная фильтрация текстур
                GT_VSync.SelectedIndex = 0; // вертикальная синхронизация выключена
                GT_MotionBlur.SelectedIndex = 0; // размытие движения выключено
                // Спросим у пользователя о режиме DirectX...
                if (MessageBox.Show(RM.GetString("GT_DxLevelMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    GT_DxMode.SelectedIndex = 0; // режим DirecX 8.0
                }
                else
                {
                    GT_DxMode.SelectedIndex = 3; // режим DirecX 9.0c
                }
                GT_HDR.SelectedIndex = 0; // эффекты HDR выключены
                MessageBox.Show(RM.GetString("GT_PerfSet"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GT_SaveApply_Click(object sender, EventArgs e)
        {
            // Сохраняем изменения в графических настройках...
            // Запрашиваем подтверждение у пользователя...
            if (MessageBox.Show(RM.GetString("GT_SaveMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (GV.IsGCFApp)
                {
                    // Создаём резервную копию...
                    try
                    {
                        CreateRegBackUpNow(Path.Combine(@"HKEY_CURRENT_USER\Software\Valve\Source\", GV.SmallAppName, "Settings"), "Game_AutoBackUp", GV.FullBackUpDirPath);
                    }
                    catch
                    {
                        // Подавляем сообщение об ошибке если оно возникнет...
                    }
                    
                    try
                    {
                        // Записываем выбранные настройки в реестр...
                        WriteGCFGameSettings(GV.SmallAppName);
                        // Выводим подтверждающее сообщение...
                        MessageBox.Show(RM.GetString("GT_SaveSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show(RM.GetString("GT_SaveFailure"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // Это NCG-приложение, поэтому будем записывать настройки в файл...

                    // TODO: реализовать запись таблицы в файл...
                }

                // Запишем в реестр пользовательскую строку запуска TF2...
                // TODO: реализовать возможность записывать параметры строки запуска...

                // Закончили запись основных настроек, указанных пользователем или выбранных по умолчанию...
            }
        }

        private void FP_ConfigSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получаем описание выбранного пользователем конфига...
            try
            {
                FP_Description.Text = File.ReadAllText(Path.Combine(GV.FullAppPath, "cfgs", GV.SmallAppName, Path.GetFileNameWithoutExtension(FP_ConfigSel.Text) + "_" + RM.GetString("AppLangPrefix") + ".txt"));
            }
            catch
            {
                FP_Description.Text = RM.GetString("FP_NoDescr");
            }
            // Включаем кнопку открытия конфига в Блокноте...
            FP_OpenNotepad.Enabled = true;
        }

        private void FP_Install_Click(object sender, EventArgs e)
        {
            // Начинаем устанавливать FPS-конфиг в управляемое приложение...
            if (FP_ConfigSel.SelectedIndex != -1)
            {
                if (MessageBox.Show(RM.GetString("FP_InstallQuestion"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Проверим, не нужно ли создавать резервную копию...
                    if (FP_CreateBackUp.Checked)
                    {
                        // Создаём резервную копию...
                        CreateBackUpNow("autoexec.cfg", GV.FullCfgPath, GV.FullBackUpDirPath);
                    }

                    try
                    {
                        // Устанавливаем...
                        InstallConfigNow(FP_ConfigSel.Text, GV.FullAppPath, GV.SmallAppName, GV.FullCfgPath);
                        // Выводим сообщение об успешной установке...
                        MessageBox.Show(RM.GetString("FP_InstallSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Отобразим значок предупреждения на странице графических настроек...
                        GT_Warning.Visible = true;
                    }
                    catch
                    {
                        // Установка не удалась. Выводим сообщение об этом...
                        MessageBox.Show(RM.GetString("FP_InstallFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                // Пользователь не выбрал конфиг. Сообщим об этом...
                MessageBox.Show(RM.GetString("FP_NothingSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FP_Uninstall_Click(object sender, EventArgs e)
        {
            // Начинаем удаление установленного конфига...
            // Генерируем имя файла с полным путём до него...
            string CfgFile = Path.Combine(GV.FullCfgPath, "autoexec.cfg");
            // Проверяем, существует ли файл...
            if (File.Exists(CfgFile))
            {
                // Файл существует. Запросим подтверждение на удаление...
                if (MessageBox.Show(RM.GetString("FP_RemoveQuestion"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    // Создадим бэкап если установлен флажок...
                    if (FP_CreateBackUp.Checked)
                    {
                        // Создаём резервную копию...
                        CreateBackUpNow("autoexec.cfg", GV.FullCfgPath, GV.FullBackUpDirPath);
                    }

                    try
                    {
                        // Удалим файл...
                        File.Delete(CfgFile);
                        // Выводим сообщение об успешном удалении...
                        MessageBox.Show(RM.GetString("FP_RemoveSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Скрываем значок предупреждения на странице графических настроек...
                        GT_Warning.Visible = false;
                    }
                    catch
                    {
                        // Произошло исключение при попытке удаления. Уведомим пользователя об этом...
                        MessageBox.Show(RM.GetString("FP_RemoveFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show(RM.GetString("FP_RemoveNotExists"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GT_Warning_Click(object sender, EventArgs e)
        {
            // Выдадим сообщение о наличии FPS-конфига...
            MessageBox.Show(RM.GetString("GT_FPSCfgDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void CE_New_Click(object sender, EventArgs e)
        {
            // Создаём новый файл...
            CE_Editor.Rows.Clear();
            CFGFileName = "";
            SB_Status.Text = RM.GetString("StatusOpenedFile") + " " + RM.GetString("UnnamedFileName");
        }

        private void CE_Open_Click(object sender, EventArgs e)
        {
            // Прочитаем конфиг и заполним его содержимым нашу таблицу редактора...
            
            // Указываем стартовый каталог в диалоге открытия файла на каталог с конфигами игры...
            CE_OpenCfgDialog.InitialDirectory = GV.FullCfgPath;

            // Считывает файл конфига и помещает записи в таблицу
            if (CE_OpenCfgDialog.ShowDialog() == DialogResult.OK) // Отображаем стандартный диалог открытия файла...
            {
                // Считываем...
                ReadConfigFromFile(CE_OpenCfgDialog.FileName);
            }
        }

        private void CE_Save_Click(object sender, EventArgs e)
        {
            CE_SaveCfgDialog.InitialDirectory = GV.FullCfgPath; // Указываем путь по умолчанию к конфигам управляемого приложения...
            if (!(String.IsNullOrEmpty(CFGFileName))) // Проверяем, открыт ли какой-либо файл...
            {
                // Будем бэкапировать только файлы, находящиеся в каталоге /cfg/
                // управляемоего приложения. Остальные - нет.
                if (File.Exists(GV.FullCfgPath + CFGFileName))
                {
                    // Создаём резервную копию...
                    CreateBackUpNow(CFGFileName, GV.FullCfgPath, GV.FullBackUpDirPath);
                }
                try
                {
                    //WriteTableToFileNow(CFGFileName);
                    WriteTableToFileNow(CE_OpenCfgDialog.FileName, GV.AppName);
                }
                catch
                {
                    // Произошла ошибка при сохранении файла...
                    MessageBox.Show(RM.GetString("CE_CfgSVVEx"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Зададим стандартное имя (см. issue 21)...
                if (!(File.Exists(Path.Combine(GV.FullCfgPath, "autoexec.cfg"))))
                {
                    // Файл autoexec.cfg не существует, поэтому предложим это имя...
                    CE_SaveCfgDialog.FileName = "autoexec.cfg";
                }
                else
                {
                    // Файл существует, поэтому предложим стандартное имя безымянного конфига...
                    CE_SaveCfgDialog.FileName = RM.GetString("UnnamedFileName");
                }

                // Файл не был открыт. Нужно сохранить и дать имя...
                if (CE_SaveCfgDialog.ShowDialog() == DialogResult.OK) // Отображаем стандартный диалог сохранения файла...
                {
                    WriteTableToFileNow(CE_SaveCfgDialog.FileName, GV.AppName);
                    CFGFileName = Path.GetFileName(CE_SaveCfgDialog.FileName);
                    CE_OpenCfgDialog.FileName = CE_SaveCfgDialog.FileName;
                    SB_Status.Text = RM.GetString("StatusOpenedFile") + " " + CFGFileName;
                }
            }
        }

        private void CE_SaveAs_Click(object sender, EventArgs e)
        {
            CE_SaveCfgDialog.InitialDirectory = GV.FullCfgPath;
            if (CE_SaveCfgDialog.ShowDialog() == DialogResult.OK)
            {
                WriteTableToFileNow(CE_SaveCfgDialog.FileName, GV.AppName);
            }
        }

        private void PS_RemCustMaps_Click(object sender, EventArgs e)
        {
            // Удаляем кастомные (нестандартные) карты...
            OpenCleanupWindow(CoreLib.IncludeTrDelim(Path.Combine(GV.FullGamePath, "maps")), "*.bsp", ((Button)sender).Text.ToLower());
        }

        private void PS_RemDnlCache_Click(object sender, EventArgs e)
        {
            // Удаляем кэш загрузок...
            OpenCleanupWindow(CoreLib.IncludeTrDelim(Path.Combine(GV.FullGamePath, "downloads")), "*.dat", ((Button)sender).Text.ToLower());
        }

        private void PS_RemOldSpray_Click(object sender, EventArgs e)
        {
            // Удаляем кэш спреев...
            OpenCleanupWindow(CoreLib.IncludeTrDelim(Path.Combine(GV.FullGamePath, "materials", "temp")), "*.vtf", ((Button)sender).Text.ToLower());
        }

        private void PS_RemOldCfgs_Click(object sender, EventArgs e)
        {
            // Удаляем все конфиги...
            OpenCleanupWindow(CoreLib.IncludeTrDelim(Path.Combine(GV.FullGamePath, "cfg")), "*.*", ((Button)sender).Text.ToLower());
        }

        private void PS_RemGraphCache_Click(object sender, EventArgs e)
        {
            // Удаляем графический кэш...
            OpenCleanupWindow(CoreLib.IncludeTrDelim(Path.Combine(GV.FullGamePath, "maps", "graphs")), "*.*", ((Button)sender).Text.ToLower());
        }

        private void PS_RemSoundCache_Click(object sender, EventArgs e)
        {
            // Удаляем звуковой кэш...
            OpenCleanupWindow(CoreLib.IncludeTrDelim(Path.Combine(GV.FullGamePath, "maps", "soundcache")), "*.*", ((Button)sender).Text.ToLower());
        }

        private void PS_RemNavFiles_Click(object sender, EventArgs e)
        {
            // Удаляем файлы навигации ботов...
            OpenCleanupWindow(CoreLib.IncludeTrDelim(Path.Combine(GV.FullGamePath, "maps")), "*.nav", ((Button)sender).Text.ToLower());
        }

        private void PS_RemScreenShots_Click(object sender, EventArgs e)
        {
            // Удаляем все скриншоты...
            OpenCleanupWindow(CoreLib.IncludeTrDelim(Path.Combine(GV.FullGamePath, "screenshots")), "*.*", ((Button)sender).Text.ToLower());
        }

        private void PS_RemDemos_Click(object sender, EventArgs e)
        {
            // Удаляем все записанные демки...
            OpenCleanupWindow(GV.FullGamePath, "*.dem", ((Button)sender).Text.ToLower());
        }

        private void PS_RemGraphOpts_Click(object sender, EventArgs e)
        {
            // Удаляем графические настройки...
            if (MessageBox.Show(String.Format(RM.GetString("PS_CleanupFull"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Создаём резервную копию...
                try
                {
                    CreateRegBackUpNow(Path.Combine(@"HKEY_CURRENT_USER\Software\Valve\Source\", GV.SmallAppName, "Settings"), "Game_AutoBackUp", GV.FullBackUpDirPath);
                }
                catch
                {
                    // Подавляем сообщение об ошибке если оно возникнет...
                }

                // Работаем...
                try
                {
                    // Удаляем ключ HKEY_CURRENT_USER\Software\Valve\Source\tf\Settings из реестра...
                    Registry.CurrentUser.DeleteSubKeyTree(Path.Combine(@"HKEY_CURRENT_USER\Software\Valve\Source\", GV.SmallAppName, "Settings"), false);
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PS_RemOldBin_Click(object sender, EventArgs e)
        {
            // Удаляем старые бинарники...
            if (MessageBox.Show(String.Format(RM.GetString("PS_CleanupFull"), ((Button)sender).Text.ToLower()), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    Directory.Delete(CoreLib.IncludeTrDelim(Path.Combine(GV.GamePath, "bin")), true); // Удаляем бинарники...
                    Directory.Delete(CoreLib.IncludeTrDelim(Path.Combine(GV.GamePath, "platform")), true); // Удаляем настройки платформы...
                    Directory.Delete(CoreLib.IncludeTrDelim(Path.Combine(GV.FullGamePath, "bin")), true); // Удаляем библиотеки игры...
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void GT_ResHor_Btn_Click(object sender, EventArgs e)
        {
            // Выводим краткую справку о вводе разрешения по горизонтали...
            MessageBox.Show(String.Format(RM.GetString("GT_ResMsg"), RM.GetString("GT_ResMsgHor")), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GT_ResVert_Btn_Click(object sender, EventArgs e)
        {
            // Выводим краткую справку о вводе разрешения по вертикали...
            MessageBox.Show(String.Format(RM.GetString("GT_ResMsg"), RM.GetString("GT_ResMsgVert")), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GT_LaunchOptions_Btn_Click(object sender, EventArgs e)
        {
            // Выводим краткую справку о строке параметров запуска...
            MessageBox.Show(RM.GetString("NotImplementedYet"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PS_ResetSettings_Click(object sender, EventArgs e)
        {
            // Удаляем все настройки...
            if (MessageBox.Show(RM.GetString("PS_ResetSettingsMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Создаём резервную копию...
                try
                {
                    CreateRegBackUpNow(Path.Combine(@"HKEY_CURRENT_USER\Software\Valve\Source\", GV.SmallAppName, "Settings"), "Game_AutoBackUp", GV.FullBackUpDirPath);
                }
                catch
                {
                    // Подавляем сообщение об ошибке если оно возникнет...
                }
                
                // Работаем...
                try
                {
                    Directory.Delete(GV.GamePath, true); // Удаляем всю папку с файлами игры...
                    Registry.CurrentUser.DeleteSubKeyTree(Path.Combine(@"HKEY_CURRENT_USER\Software\Valve\Source\", GV.SmallAppName, "Settings"), false); // Удаляем настройки видео...
                    MessageBox.Show(RM.GetString("PS_CleanupSuccess"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(RM.GetString("PS_CleanupErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void MNUShowEdHint_Click(object sender, EventArgs e)
        {
            // Покажем подсказку...
            CE_ShowHint.PerformClick();
        }

        private void MNUReportBuilder_Click(object sender, EventArgs e)
        {
            // Запускаем форму создания отчёта для Техподдержки...
            frmRepBuilder RBF = new frmRepBuilder();
            RBF.ShowDialog();
        }

        private void MNUInstaller_Click(object sender, EventArgs e)
        {
            // Запускаем форму установщика спреев, демок и конфигов...
            frmInstaller InstF = new frmInstaller();
            InstF.ShowDialog();
        }

        private void MNUExit_Click(object sender, EventArgs e)
        {
            // Завершаем работу программы...
            Environment.Exit(0);
        }

        private void MNUFPSWizard_Click(object sender, EventArgs e)
        {
            // Очистим Редактор конфигов...
            CE_New.PerformClick();
            // Запускаем форму мастера FPS-конфигов...
            frmFPGen FPFrm = new frmFPGen(new CFGEdDelegate(AddRowToTable));
            FPFrm.ShowDialog();
            MainTabControl.SelectedIndex = 1;
        }

        private void MNUAbout_Click(object sender, EventArgs e)
        {
            // Отобразим форму "О программе"...
            frmAbout AboutFrm = new frmAbout();
            AboutFrm.ShowDialog();
        }

        private void MNUReportBug_Click(object sender, EventArgs e)
        {
            // Открываем в браузере страницу сообщения об ошибке в багтрекере гуглкода...
            Process.Start("http://code.google.com/p/srcrepair/issues/entry");
        }

        private void BUT_Refresh_Click(object sender, EventArgs e)
        {            
            try
            {
                ReadBackUpList2Table(GV.FullBackUpDirPath);
            }
            catch
            {
                // Произошло исключение...
                MessageBox.Show(RM.GetString("BU_ListLdFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Создадим каталог для резервных копий...
                Directory.CreateDirectory(GV.FullBackUpDirPath);
            }
        }

        private void BUT_RestoreB_Click(object sender, EventArgs e)
        {
            // Восстановим выделенный бэкап...
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    // Получаем имя файла...
                    string FName = BU_LVTable.SelectedItems[0].SubItems[4].Text;

                    // Запрашиваем подтверждение...
                    if (MessageBox.Show(String.Format(RM.GetString("BU_QMsg"), Path.GetFileNameWithoutExtension(FName), BU_LVTable.SelectedItems[0].SubItems[3].Text), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        // Проверяем что восстанавливать: конфиг или реестр...
                        if (Path.GetExtension(FName) != ".reg")
                        {
                            // Сохраняем оригинальное имя файла резервной копии для функции копирования...
                            string OrigName = FName;
                            // Отбрасываем двойное расширение...
                            FName = Path.GetFileNameWithoutExtension(FName);
                            try
                            {
                                // Копируем файл...
                                File.Copy(Path.Combine(GV.FullBackUpDirPath, OrigName), Path.Combine(GV.FullCfgPath, FName), true);
                                // Показываем сообщение об успешном восстановлении...
                                MessageBox.Show(RM.GetString("BU_RestSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch
                            {
                                // Произошло исключение...
                                MessageBox.Show(RM.GetString("BU_RestFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            // Восстанавливаем файл реестра...
                            try
                            {
                                // Восстанавливаем...
                                Process.Start("regedit.exe", @"/s """ + GV.FullBackUpDirPath + FName + @"""");
                                // Показываем сообщение об успешном восстановлении...
                                MessageBox.Show(RM.GetString("BU_RestSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch
                            {
                                // Произошло исключение...
                                MessageBox.Show(RM.GetString("BU_RestFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(RM.GetString("BU_NoSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(RM.GetString("BU_NoFiles"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BUT_DelB_Click(object sender, EventArgs e)
        {
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    // Удалим выбранный бэкап...
                    string FName = BU_LVTable.SelectedItems[0].SubItems[4].Text;
                    // Запросим подтверждение...
                    if (MessageBox.Show(RM.GetString("BU_DelMsg"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        try
                        {
                            // Удаляем файл...
                            File.Delete(Path.Combine(GV.FullBackUpDirPath, FName));
                            // Удаляем строку...
                            BU_LVTable.Items.Remove(BU_LVTable.SelectedItems[0]);
                            // Показываем сообщение об успешном удалении...
                            MessageBox.Show(RM.GetString("BU_DelSuccessful"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch
                        {
                            // Произошло исключение при попытке удаления файла резервной копии...
                            MessageBox.Show(RM.GetString("BU_DelFailed"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(RM.GetString("BU_NoSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(RM.GetString("BU_NoFiles"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BUT_CrBkupReg_ButtonClick(object sender, EventArgs e)
        {
            BUT_CrBkupReg.ShowDropDown();
        }

        private void BUT_L_GameSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(RM.GetString("BU_RegCreate"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Создадим резервную копию графических настроек игры...
                try
                {
                    CreateRegBackUpNow(Path.Combine(@"HKEY_CURRENT_USER\Software\Valve\Source\", GV.SmallAppName, "Settings"), "Game_Options", GV.FullBackUpDirPath);
                    MessageBox.Show(RM.GetString("BU_RegDone"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BUT_Refresh.PerformClick();
                }
                catch
                {
                    MessageBox.Show(RM.GetString("BU_RegErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void BUT_L_AllSteam_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(RM.GetString("BU_RegCreate"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Создадим резервную копию всех настроек Steam...
                try
                {
                    // Создаём...
                    CreateRegBackUpNow(@"HKEY_CURRENT_USER\Software\Valve", "Steam_BackUp", GV.FullBackUpDirPath);
                    // Выводим сообщение...
                    MessageBox.Show(RM.GetString("BU_RegDone"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Обновим список бэкапов...
                    BUT_Refresh.PerformClick();
                }
                catch
                {
                    // Произошло исключение, уведомим пользователя...
                    MessageBox.Show(RM.GetString("BU_RegErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            
        }

        private void BUT_L_AllSRC_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(RM.GetString("BU_RegCreate"), GV.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Созданим резервную копию графических настроек всех Source-игр...
                try
                {
                    CreateRegBackUpNow(@"HKEY_CURRENT_USER\Software\Valve\Source", "Source_Options", GV.FullBackUpDirPath);
                    MessageBox.Show(RM.GetString("BU_RegDone"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BUT_Refresh.PerformClick();
                }
                catch
                {
                    MessageBox.Show(RM.GetString("BU_RegErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Проверяем открыта ли страница "Редактор конфигов"...
            if (MainTabControl.SelectedIndex == 1)
            {
                // Включаем заблокированный контрол...
                MNUShowEdHint.Enabled = true;
                // Проверяем открыт ли файл в Редакторе конфигов...
                if (String.IsNullOrEmpty(CFGFileName))
                {
                    // Нет, не открыт. Выводим имя "Безымянный.cfg"...
                    SB_Status.Text = RM.GetString("StatusOpenedFile") + " " + RM.GetString("UnnamedFileName");
                }
                else
                {
                    // Да, открыт. Выводим настоящее имя...
                    SB_Status.Text = RM.GetString("StatusOpenedFile") + " " + CFGFileName;
                }
            }
            else
            {
                // Открыта другая страница...
                // Блокируем контрол подсказки...
                MNUShowEdHint.Enabled = false;
                // ...и выводим стандартное сообщение в статус-бар...
                SB_Status.Text = RM.GetString("StatusNormal");
            }
        }

        private void CE_ShowHint_Click(object sender, EventArgs e)
        {
            try
            {
                string Buf = CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[0].Value.ToString();
                if (!(String.IsNullOrEmpty(Buf)))
                {
                    Buf = GetCVDescription(Buf);
                    if (!(String.IsNullOrEmpty(Buf)))
                    {
                        MessageBox.Show(Buf, GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(RM.GetString("CE_ClNoDescr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(RM.GetString("CE_ClSelErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show(RM.GetString("CE_ClSelErr"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUHelp_Click(object sender, EventArgs e)
        {
            string HelpFilePath = String.Format(GV.FullAppPath + "srcrepair_{0}.chm", RM.GetString("AppLangPrefix"));
            if (File.Exists(HelpFilePath))
            {
                Process.Start(HelpFilePath);
            }
            else
            {
                Process.Start(String.Format("http://code.google.com/p/srcrepair/wiki/UserManual_{0}", RM.GetString("AppLangPrefix")));
            }
        }

        private void MNUOpinion_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("mailto:srcrepair@easycoding.org?subject=SRC Repair Opinion");
            }
            catch
            {
                Process.Start("http://www.easycoding.org/projects/srcrepair#respond");
            }
        }

        private void MNUSteamGroup_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("steam://url/GroupSteamIDPage/103582791431662552");
            }
            catch
            {
                Process.Start("http://steamcommunity.com/groups/srcrepair");
            }
        }

        private void MNULnkEasyCoding_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.easycoding.org/");
        }

        private void MNULnkTFRU_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.team-fortress.ru/");
        }

        private void MNUHEd_Click(object sender, EventArgs e)
        {
            // Отобразим форму редактора файла hosts...
            frmHEd HEdFrm = new frmHEd();
            HEdFrm.ShowDialog();
        }

        private void CE_RmRow_Click(object sender, EventArgs e)
        {
            try
            {
                if (CE_Editor.Rows.Count > 0)
                {
                    CE_Editor.Rows.Remove(CE_Editor.CurrentRow);
                }
            }
            catch
            {
                // Подавляем возможные сообщения об ошибках...
            }
        }

        private void CE_Copy_Click(object sender, EventArgs e)
        {
            if (CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value != null)
            {
                Clipboard.SetText(CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value.ToString());
            }
        }

        private void CE_Cut_Click(object sender, EventArgs e)
        {
            if (CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value != null)
            {
                // Копируем в буфер...
                Clipboard.SetText(CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value.ToString());
                // Удаляем из ячейки...
                CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value = null;
            }
        }

        private void CE_Paste_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    CE_Editor.Rows[CE_Editor.CurrentRow.Index].Cells[CE_Editor.CurrentCell.ColumnIndex].Value = Clipboard.GetText();
                }
            }
            catch
            {
            }
        }

        private void FP_OpenNotepad_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", Path.Combine(GV.FullAppPath, "cfgs", GV.SmallAppName, FP_ConfigSel.Text));
        }

        private void MNUUpdateCheck_Click(object sender, EventArgs e)
        {
            try
            {
                string NewVersion, UpdateURI, DnlStr;
                // Получаем файл с номером версии и ссылкой на новую...
                using (WebClient Downloader = new WebClient())
                {
                    DnlStr = Downloader.DownloadString(Properties.Settings.Default.UpdateChURI);
                }
                // Установим дату последней проверки обновлений...
                Properties.Settings.Default.LastUpdateTime = DateTime.Now;
                // Мы получили URL и версию...
                NewVersion = DnlStr.Substring(0, DnlStr.IndexOf("!")); // Получаем версию...
                UpdateURI = DnlStr.Remove(0, DnlStr.IndexOf("!") + 1); // Получаем URL...
                // Проверим, является ли версия на сервере новее, чем текущая...
                if (CoreLib.CompareVersions(GV.AppVersionInfo, NewVersion))
                {
                    // Доступна новая версия, отобразим модуль обновления...
                    frmUpdate UpdFrm = new frmUpdate(NewVersion, UpdateURI);
                    UpdFrm.ShowDialog();
                }
                else
                {
                    // Новых версий не обнаружено.
                    MessageBox.Show(RM.GetString("UPD_LatestInstalled"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                // Произошло исключение...
                MessageBox.Show(RM.GetString("UPD_ExceptionDetected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BUT_OpenNpad_Click(object sender, EventArgs e)
        {
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    // Откроем выбранный бэкап в Блокноте Windows...
                    Process.Start("notepad.exe", Path.Combine(GV.FullBackUpDirPath, BU_LVTable.SelectedItems[0].SubItems[4].Text));
                }
                else
                {
                    MessageBox.Show(RM.GetString("BU_NoSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(RM.GetString("BU_NoFiles"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MNUAppOptions_Click(object sender, EventArgs e)
        {
            // Показываем форму настроек...
            frmOptions OptsFrm = new frmOptions();
            OptsFrm.ShowDialog();
        }

        private void BU_LVTable_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = BU_LVTable.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void BUT_ExploreBUp_Click(object sender, EventArgs e)
        {
            if (BU_LVTable.Items.Count > 0)
            {
                if (BU_LVTable.SelectedItems.Count > 0)
                {
                    // Откроем выбранный бэкап в Проводнике Windows...
                    Process.Start("explorer.exe", @"/select," + @"""" + GV.FullBackUpDirPath + BU_LVTable.SelectedItems[0].SubItems[4].Text + @"""");
                }
                else
                {
                    MessageBox.Show(RM.GetString("BU_NoSelected"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(RM.GetString("BU_NoFiles"), GV.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmMainW_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Форма была закрыта, сохраняем настройки приложения...
            Properties.Settings.Default.Save();
        }

        private void MNUWinMnuDisabler_Click(object sender, EventArgs e)
        {
            // Показываем модуля отключения клавиш...
            frmKBHelper KBHlp = new frmKBHelper();
            KBHlp.ShowDialog();
        }

        private void MNUGameStore_Click(object sender, EventArgs e)
        {
            // Для России и СНГ открываем Gamemarket, для остальных - Steam Store...
            Process.Start(RM.GetString("AppShopURL"));
        }

        private void MNUDonate_Click(object sender, EventArgs e)
        {
            Process.Start("http://code.google.com/p/srcrepair/wiki/Donate");
        }

        private void FP_CreateBackUp_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.FPConfCreateBackUps = FP_CreateBackUp.Checked;
        }
    }
}
