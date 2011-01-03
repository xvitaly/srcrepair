/*
 * Пустой модуль SRC Repair.
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace srcrepair
{
    public partial class frmHEd : Form
    {
        public frmHEd()
        {
            InitializeComponent();
        }

        private const string PluginName = "Advanced Hosts Editor";
        
        private string HostsFilePath = "";

        private void ReadHostsToTable(string FilePath)
        {
            // Очистим таблицу...
            HEd_Table.Rows.Clear();

            // Начинаем считывать и парсить содержимое файла...
            string ImpStr; // Строка для парсинга...
            string Buf; // Буферная переменная...            
            using (StreamReader OpenedHosts = new StreamReader(@FilePath, Encoding.Default))
            {
                while (OpenedHosts.Peek() >= 0)
                {
                    // Начинаем...
                    ImpStr = OpenedHosts.ReadLine(); // считали строку...
                    ImpStr = ImpStr.Trim(); // почистим строку от лишних пробелов...
                    // Начинаем парсить считанную строку...
                    if (!(String.IsNullOrEmpty(ImpStr))) // проверяем, не пустая ли строка...
                    {
                        if (ImpStr[0] != '#') // проверяем, не комментарий ли...
                        {
                            // Ищем символ табуляции и заменим его на пробел...
                            while (ImpStr.IndexOf("\t") != -1)
                            {
                                ImpStr = ImpStr.Replace("\t", " ");
                            }

                            // Удалим все лишние пробелы...
                            while (ImpStr.IndexOf("  ") != -1) // пока остались двойные пробелы, продолжаем...
                            {
                                ImpStr = ImpStr.Replace("  ", " "); // удаляем найденный лишний пробел...
                            }

                            // Строка почищена, продолжаем...
                            if (ImpStr.IndexOf(" ") != -1)
                            {
                                Buf = ImpStr.Substring(0, ImpStr.IndexOf(" ")); // мы получили IP-адрес...
                                ImpStr = ImpStr.Remove(0, ImpStr.IndexOf(" ") + 1); // удаляем полученное на предыдущем шаге...
                                HEd_Table.Rows.Add(Buf, ImpStr); // записываем в таблицу...
                            }
                        }
                    }
                }
            }
        }
        
        private void frmHEd_Load(object sender, EventArgs e)
        {
            // Проверим наличие прав админа...

            // Получим путь к файлу hosts (вдруг он переопределён каким-либо зловредом)...
            try
            {
                RegistryKey ResKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", false);
                if (ResKey != null)
                {
                    HostsFilePath = (string)ResKey.GetValue("DataBasePath");
                }
            }
            catch
            {
                // Произошло исключение, поэтому укажем вручную...
                HostsFilePath = @"%SystemRoot%\System32\drivers\etc";
            }

            // Сгенерируем полный путь к файлу hosts...
            HostsFilePath = frmMainW.IncludeTrDelim(HostsFilePath) + "hosts";

            // Считаем содержимое...
            try
            {
                ReadHostsToTable(HostsFilePath);
            }
            catch
            {
                MessageBox.Show(String.Format(frmMainW.RM.GetString("AHE_ExceptionDetected"), HostsFilePath), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void HEd_T_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                ReadHostsToTable(HostsFilePath);
            }
            catch
            {
                MessageBox.Show(String.Format(frmMainW.RM.GetString("AHE_ExceptionDetected"), HostsFilePath), PluginName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
