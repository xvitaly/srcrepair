using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace srcrepair
{
    public partial class frmMainW : Form
    {
        public frmMainW()
        {
            InitializeComponent();
        }

        private void frmMainW_Load(object sender, EventArgs e)
        {
            // Событие инициализации формы...
            
            // Получаем путь к каталогу приложения...
            System.Reflection.Assembly Assmbl = System.Reflection.Assembly.GetEntryAssembly();
            GV.FullAppPath = CoreFn.IncludeTrDelim(System.IO.Path.GetDirectoryName(Assmbl.Location));

            // Найдём и завершим в памяти процесс Steam...
            if (CoreFn.ProcessTerminate("Steam") != 0)
            {
                MessageBox.Show(Properties.Resources.PS_ProcessDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Получаем параметры командной строки, переданные приложению...
            string[] CMDLineArgs = Environment.GetCommandLineArgs();

            // Ищем параметр командной строки path...
            if (CoreFn.FindCommandLineSwitch(CMDLineArgs, "-path"))
            {
                // Параметр найден, считываем следующий за ним параметр с путём к Steam...
                GV.FullSteamPath = CoreFn.IncludeTrDelim(CMDLineArgs[CoreFn.FindStringInStrArray(CMDLineArgs, "-path") + 1]);
            }
            else
            {
                // Параметр не найден, поэтому получим из реестра...
                try
                {
                    // Получаем из реестра...
                    GV.FullSteamPath = CoreFn.IncludeTrDelim(CoreFn.GetSteamPath());
                }
                catch
                {
                    // Произошло исключение, пользователю придётся ввести путь самостоятельно!
                    MessageBox.Show(Properties.Resources.SteamPathNotDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    string Buf = "";
                    if (CoreFn.InputBox(Properties.Resources.SteamPathEnterTitle, Properties.Resources.SteamPathEnterText, ref Buf) == DialogResult.OK)
                    {
                        Buf = CoreFn.IncludeTrDelim(Buf);
                        if (!(System.IO.File.Exists(Buf + "Steam.exe")))
                        {
                            MessageBox.Show(Properties.Resources.SteamPathEnterErr, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                        else
                        {
                            GV.FullSteamPath = Buf;
                        }
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.SteamPathCancel, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
            }

            // Ищем параметр командной строки login...
            if (CoreFn.FindCommandLineSwitch(CMDLineArgs, "-login"))
            {
                // Параметр найден, добавим его и отключим автоопределение...
                try
                {
                    // Добавляем параметр в ComboBox...
                    LoginSel.Items.Add((string)CMDLineArgs[CoreFn.FindStringInStrArray(CMDLineArgs, "-login") + 1]);
                }
                catch
                {
                    // Произошло исключение, поэтому просто выдадим сообщение...
                    MessageBox.Show(Properties.Resources.ParamError, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Параметр не найден, будем использовать автоопределение...
                // Создаём объект DirInfo...
                System.IO.DirectoryInfo DInfo = new System.IO.DirectoryInfo(GV.FullSteamPath + "steamapps\\");
                // Получаем список директорий из текущего...
                System.IO.DirectoryInfo[] DirList = DInfo.GetDirectories();
                // Обходим созданный массив в поиске нужных нам логинов...
                foreach (System.IO.DirectoryInfo DItem in DirList)
                {
                    // Фильтруем известные каталоги...
                    if ((DItem.Name != "common") && (DItem.Name != "sourcemods") && (DItem.Name != "media"))
                    {
                        // Добавляем найденный логин в список ComboBox...
                        LoginSel.Items.Add((string)DItem.Name);
                    }
                }
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
                    if ((CoreFn.InputBox(Properties.Resources.SteamLoginEnterTitle, Properties.Resources.SteamLoginEnterText, ref SBuf) == DialogResult.Cancel) || (SBuf == ""))
                    {
                        // Пользователь нажал Cancel, либо ввёл пустую строку, поэтому
                        // выводим сообщение и завершаем работу приложения...
                        MessageBox.Show(Properties.Resources.SteamLoginCancel, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Application.Exit();
                    };
                } while (!(System.IO.Directory.Exists(GV.FullSteamPath + "steamapps\\" + SBuf + "\\")));
                
                // Добавляем полученный логин в список...
                LoginSel.Items.Add((string)SBuf);
            }

            // TODO: Реализовать проверку на наличие non-ASCII символов в пути...
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
            PS_SteamLang.Enabled = PS_CleanRegistry.Checked;

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
            DialogResult UserConfirmation = MessageBox.Show(Properties.Resources.PS_ExecuteMSG, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (UserConfirmation == DialogResult.Yes)
            {
                // Подтверждение получено...
                if ((PS_CleanBlobs.Checked) || (PS_CleanRegistry.Checked))
                {
                    // Найдём и завершим работу клиента Steam...
                    if (CoreFn.ProcessTerminate("Steam") != 0)
                    {
                        MessageBox.Show(Properties.Resources.PS_ProcessDetected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Проверяем нужно ли чистить блобы...
                    if (PS_CleanBlobs.Checked)
                    {
                        // Чистим блобы...
                        CoreFn.CleanBlobsNow(GV.FullSteamPath);
                    }

                    // Проверяем нужно ли чистить реестр...
                    if (PS_CleanRegistry.Checked)
                    {
                        // Проверяем выбрал ли пользователь язык из выпадающего списка...
                        if (PS_SteamLang.SelectedIndex != -1)
                        {
                            // Всё в порядке, чистим реестр...
                            CoreFn.CleanRegistryNow(PS_SteamLang.SelectedIndex);
                        }
                        else
                        {
                            // Пользователь не выбрал язык, поэтому будем использовать английский...
                            MessageBox.Show(Properties.Resources.PS_NoLangSelected, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            CoreFn.CleanRegistryNow(0);
                        }
                    }

                    // Выполнение закончено, выведем сообщение о завершении...
                    MessageBox.Show(Properties.Resources.PS_SeqCompleted, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Завершаем работу приложения...
                    Application.Exit();
                }
            }
        }

        private void PS_AllowRemCtrls_CheckedChanged(object sender, EventArgs e)
        {
            PS_RemCustMaps.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemDnlCache.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemOldSpray.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemOldCfgs.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemGraphCache.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemSoundCache.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemNavFiles.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemScreenShots.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemDemos.Enabled = PS_AllowRemCtrls.Checked;
            PS_RemGraphOpts.Enabled = PS_AllowRemCtrls.Checked;
        }

        private void frmMainW_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Проверим, делал ли что-то пользователь с формой. Если не делал - не будем
            // спрашивать и завершим форму автоматически...
            if (AppSelector.Enabled)
            {
                // Создаём MessageBox...
                DialogResult UserConfirmation = MessageBox.Show(Properties.Resources.FrmCloseQuery, Properties.Resources.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                // Запрашиваем подтверждение у пользователя на закрытие формы...
                if (UserConfirmation == DialogResult.Yes)
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

        private void AppSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Начинаем определять нужные нам значения переменных...
            switch (AppSelector.SelectedIndex)
            {
                case 0:
                    GV.FullAppName = "team fortress 2";
                    GV.SmallAppName = "tf";
                    break;
                case 1:
                    GV.FullAppName = "counter-strike source";
                    GV.SmallAppName = "cstrike";
                    break;
                case 2:
                    GV.FullAppName = "garrysmod";
                    GV.SmallAppName = "garrysmod";
                    break;
                default:
                    AppSelector.SelectedIndex = -1;
                    break;
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
            GV.FullAppPath = CoreFn.IncludeTrDelim(GV.FullSteamPath + "steamapps\\" + LoginSel.Text + "\\" + GV.FullAppName + "\\" + GV.SmallAppName);

            // Включаем основные элементы управления (контролы)...
            MainTabControl.Enabled = true;

            // Начинаем заполнять таблицу...

            // Получаем значение разрешения по горизонтали
            try
            {
                GT_ResHor.Value = CoreFn.GetSRCDWord("ScreenWidth", GV.SmallAppName);
            }
            catch
            {
                GT_ResHor.Value = 800;
            }

            // Получаем значение разрешения по вертикали
            try
            {
                GT_ResVert.Value = CoreFn.GetSRCDWord("ScreenHeight", GV.SmallAppName);
            }
            catch
            {
                GT_ResVert.Value = 600;
            }

            // Получаем режим окна (ScreenWindowed): 1-window, 0-fullscreen
            try
            {
                GT_ScreenType.SelectedIndex = CoreFn.GetSRCDWord("ScreenWindowed", GV.SmallAppName);
            }
            catch
            {
                GT_ScreenType.SelectedIndex = 0;
            }

            // Получаем детализацию моделей (r_rootlod): 0-high, 1-med, 2-low
            try
            {
                switch (CoreFn.GetSRCDWord("r_rootlod", GV.SmallAppName))
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
                GT_ModelQuality.SelectedIndex = 1;
            }

            // Получаем детализацию текстур (mat_picmip): 0-high, 1-med, 2-low
            try
            {
                switch (CoreFn.GetSRCDWord("mat_picmip", GV.SmallAppName))
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
                GT_TextureQuality.SelectedIndex = 1;
            }

            // Получаем настройки шейдеров (mat_reducefillrate): 0-high, 1-low
            try
            {
                switch (CoreFn.GetSRCDWord("mat_reducefillrate", GV.SmallAppName))
                {
                    case 0: GT_ShaderQuality.SelectedIndex = 1;
                        break;
                    case 1: GT_ShaderQuality.SelectedIndex = 0;
                        break;
                }
            }
            catch
            {
                GT_ShaderQuality.SelectedIndex = 0;
            }

            // Начинаем работать над отражениями (здесь сложнее)
            try
            {
                switch (CoreFn.GetSRCDWord("r_waterforceexpensive", GV.SmallAppName))
                {
                    case 0: GT_WaterQuality.SelectedIndex = 0;
                        break;
                    case 1:
                        switch (CoreFn.GetSRCDWord("r_waterforcereflectentities", GV.SmallAppName))   
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
                GT_WaterQuality.SelectedIndex = 1;
            }

            // Получаем настройки теней (r_shadowrendertotexture): 0-low, 1-high
            try
            {
                switch (CoreFn.GetSRCDWord("r_shadowrendertotexture", GV.SmallAppName))
                {
                    case 0: GT_ShadowQuality.SelectedIndex = 0;
                        break;
                    case 1: GT_ShadowQuality.SelectedIndex = 1;
                        break;
                }
            }
            catch
            {
                GT_ShadowQuality.SelectedIndex = 1;
            }

            // Получаем настройки коррекции цвета (mat_colorcorrection): 0-off, 1-on
            try
            {
                switch (CoreFn.GetSRCDWord("mat_colorcorrection", GV.SmallAppName))
                {
                    case 0: GT_ColorCorrectionT.SelectedIndex = 0;
                        break;
                    case 1: GT_ColorCorrectionT.SelectedIndex = 1;
                        break;
                }
            }
            catch
            {
                GT_ColorCorrectionT.SelectedIndex = 1;
            }

            // Получаем настройки сглаживания (mat_antialias): 1-off, 2-2x, 4-4x, etc
            // 2x MSAA - 2:0; 4xMSAA - 4:0; 8xCSAA - 4:2; 16xCSAA - 4:4; 8xMSAA - 8:0; 16xQ CSAA - 8:2;
            try
            {
                switch (CoreFn.GetSRCDWord("mat_antialias", GV.SmallAppName))
                {
                    case 0: GT_AntiAliasing.SelectedIndex = 0;
                        break;
                    case 1: GT_AntiAliasing.SelectedIndex = 0;
                        break;
                    case 2: GT_AntiAliasing.SelectedIndex = 1;
                        break;
                    case 4:
                        switch (CoreFn.GetSRCDWord("mat_aaquality", GV.SmallAppName))
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
                        switch (CoreFn.GetSRCDWord("mat_aaquality", GV.SmallAppName))
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
                GT_AntiAliasing.SelectedIndex = 0;
            }

            // Получаем настройки анизотропии (mat_forceaniso): 1-off, etc
            try
            {
                switch (CoreFn.GetSRCDWord("mat_forceaniso", GV.SmallAppName))
                {
                    case 1:
                        switch (CoreFn.GetSRCDWord("mat_trilinear", GV.SmallAppName))
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
                GT_Filtering.SelectedIndex = 1;
            }

            // Получаем настройки вертикальной синхронизации (mat_vsync): 0-off, 1-on
            try
            {
                switch (CoreFn.GetSRCDWord("mat_vsync", GV.SmallAppName))
                {
                    case 0: GT_VSync.SelectedIndex = 0;
                        break;
                    case 1: GT_VSync.SelectedIndex = 1;
                        break;
                }
            }
            catch
            {
                GT_VSync.SelectedIndex = 0;
            }

            // Получаем настройки размытия движения (MotionBlur): 0-off, 1-on
            try
            {
                switch (CoreFn.GetSRCDWord("MotionBlur", GV.SmallAppName))
                {
                    case 0: GT_MotionBlur.SelectedIndex = 0;
                        break;
                    case 1: GT_MotionBlur.SelectedIndex = 1;
                        break;
                }
            }
            catch
            {
                GT_MotionBlur.SelectedIndex = 0;
            }

            // Получаем настройки режима рендера (DXLevel_V1):
            // 80-DirectX 8.0; 81-DirectX 8.1; 90-DirectX 9.0; 95-DirectX 9.0c
            try
            {
                switch (CoreFn.GetSRCDWord("DXLevel_V1", GV.SmallAppName))
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
                GT_DxMode.SelectedIndex = 3;
            }

            // Получаем настройки HDR (mat_hdr_level): 0-off,1-bloom,2-Full
            try
            {
                switch (CoreFn.GetSRCDWord("mat_hdr_level", GV.SmallAppName))
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
                GT_HDR.SelectedIndex = 2;
            }
        }

        private void LoginSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
            AppSelector.Enabled = true;
        }
    }
}
