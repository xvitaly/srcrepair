/*
 * SRC Repair.
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
using System.Threading;
using System.Windows.Forms;

namespace srcrepair
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Создаём новый мьютекс на время работы программы...
            using (Mutex Mtx = new Mutex(false, Properties.Resources.AppName))
            {
                // Пробуем занять и заблокировать, тем самым проверяя запущена ли ещё одна копия программы или нет...
                if (Mtx.WaitOne(0, false))
                {
                    // Включаем визуальные стили...
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    // Запускаем главную форму...
                    Application.Run(new FrmMainW());
                }
                else
                {
                    // Программа уже запущена. Выводим сообщение об этом...
                    MessageBox.Show(Properties.Resources.AppAlrLaunched, Properties.Resources.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Завершаем работу приложения с кодом 16...
                    Environment.Exit(16);
                }
            }
        }
    }
}
