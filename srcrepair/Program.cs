/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2019 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2019 EasyCoding Team.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
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
